namespace CodeEditor_Components
{
    #region Using Directives

    using CodeEditor_Components.SearchTypes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    #endregion Using Directives

    /// <summary>
    /// Class to enable programmatic and GUI-based finding and replacing of text in a <see cref="IEditor"/> control, programmatically or through a <see cref="FindReplaceDialog"/> or <see cref="IncrementalSearcher"/>.
    /// </summary>
    public class FindReplace : ComponentManager
    {
        #region Fields

        private readonly int _historyMaxCount = 10;
        private bool _updateHighlights;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="FindReplace"/> with associated
        /// <see cref="FindReplaceDialog"/> and <see cref="IncrementalSearcher"/> instances.
        /// </summary>
        /// <param name="editor">The <see cref="IEditor"/> control to which the <see cref="FindReplace"/> is attached.</param>
        public FindReplace(IEditor editor) : base() {
            Window = CreateWindowInstance();
            Window.KeyPressed += FindReplace_KeyPressed;

            SearchBar = CreateIncrementalSearcherInstance();
            SearchBar.KeyPressed += FindReplace_KeyPressed;
            SearchBar.Visible = false;

            FindHistory = new List<string> {
                Capacity = _historyMaxCount
            };
            ReplaceHistory = new List<string> {
                Capacity = _historyMaxCount
            };

            if (editor != null) {
                Editor = editor;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="FindReplace"/> with associated
        /// <see cref="FindReplaceDialog"/> and <see cref="IncrementalSearcher"/> instances.
        /// </summary>
        public FindReplace() : this(null) { }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Triggered when a key is pressed on the <see cref="FindReplaceDialog"/>.
        /// </summary>
        public event KeyPressedHandler KeyPressed;

        /// <summary>
        /// Handler for the key press on the <see cref="FindReplaceDialog"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The key info of the key(s) pressed.</param>
        public delegate void KeyPressedHandler(object sender, KeyEventArgs e);

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the associated <see cref="IEditor"/> that <see cref="FindReplace"/> can act upon.
        /// </summary>
        public override IEditor Editor {
            get {
                return base.Editor;
            }
            set {
                base.Editor = value;
                Editor.Target.Controls.Add(SearchBar);
                Editor.Target.Resize += ResizeEditor;
                _updateHighlights = true;
            }
        }

        /// <summary>
        /// Gets the <see cref="IncrementalSearcher"/>.
        /// </summary>
        public IncrementalSearcher SearchBar { get; private set; }

        /// <summary>
        /// Gets the <see cref="FindReplaceDialog"/> instance.
        /// </summary>
        public FindReplaceDialog Window { get; private set; }

        /// <summary>
        /// Gets the <see cref="Search"/> object that encapsualtes the last completed find/replace action.
        /// </summary>
        public Search CurrentQuery { get; private set; }

        /// <summary>
        /// Gets the <see cref="List{CharacterRange}"/> that contains the last find/replace results.
        /// </summary>
        public List<TextRange> CurrentResults { get; private set; }

        /// <summary>
        /// Gets the list of terms that have been searched.
        /// </summary>
        public List<string> FindHistory { get; }

        /// <summary>
        /// Gets the list of terms used to replace search results.
        /// </summary>
        public List<string> ReplaceHistory { get; }

        #endregion Properties

        #region Methods

        #region Find & Replace

        /// <summary>
        /// Searches for the first or last instance of the given <see cref="Search"/> query in the <see cref="IEditor"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="searchUp">Search direction. Set to true to search from the bottom up.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.start"/> will be the same as <see cref="TextRange.end"/> if no match was found.</returns>
        public TextRange Find(Search query, bool searchUp) {
            query.SearchUp = searchUp;
            return query.Find(Editor);
        }

        /// <summary>
        /// Searches for all instances of the given <see cref="Search"/> query in the <see cref="IEditor"/> text, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<TextRange> FindAll(Search query, bool mark, bool highlight) {
            return UpdateResults(query, mark, highlight);
        }

        /// <summary>
        /// Searches for the next instance of the given <see cref="Search"/> query in the <see cref="IEditor"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.start"/> will be the same as <see cref="TextRange.end"/> if no match was found.</returns>
        public TextRange FindNext(Search query, bool wrap) {
            UpdateResults(query);
            return query.FindNext(Editor, wrap);
        }

        /// <summary>
        /// Searches for the previous instance of the given <see cref="Search"/> query in the <see cref="IEditor"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.start"/> will be the same as <see cref="TextRange.end"/> if no match was found.</returns>
        public TextRange FindPrevious(Search query, bool wrap) {
            UpdateResults(query);
            return query.FindPrevious(Editor, wrap);
        }

        /// <summary>
        /// Replaces the next instance of the given <see cref="Search"/> query in the <see cref="IEditor"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.start"/> will be the same as <see cref="TextRange.end"/> if no match was found.</returns>
        public TextRange Replace(Search query, string replaceString, bool wrap) {
            UpdateResults(query);
            return query.Replace(Editor, replaceString, wrap);
        }

        /// <summary>
        /// Replaces all instances of the given <see cref="Search"/> query in the <see cref="IEditor"/> text with the given string, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern if <c>query</c> is a <see cref="RegexSearch"/> object.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns></returns>
        public List<TextRange> ReplaceAll(Search query, string replaceString) {
            return query.ReplaceAll(Editor, replaceString);
        }

        #endregion Find & Replace

        #region Show

        /// <summary>
        /// Shows the <see cref="FindReplaceDialog"/> with the Find tab active.
        /// </summary>
        public void ShowFind() {
            ShowFindReplaceTab("tpgFind");
        }

        /// <summary>
        /// Shows the <see cref="FindReplaceDialog"/> with the Replace tab active.
        /// </summary>
        public void ShowReplace() {
            ShowFindReplaceTab("tpgReplace");
        }

        /// <summary>
        /// Hides the <see cref="FindReplaceDialog"/>.
        /// </summary>
        public void HideFindReplace() {
            if (Window.Visible) {
                Window.Hide();
            }
            Editor.Target.Focus();
        }

        // Displays the FindReplaceDialog with the specified tab active, and sets the selection/text appropriately.
        private void ShowFindReplaceTab(string tabName) {
            HideIncrementalSearch();
            if (!Window.Visible) {
                Window.Show(Editor.Target.FindForm());
            }

            Window.tabAll.SelectedTab = Window.tabAll.TabPages[tabName];

            if (Editor.GetLineFromPosition(Editor.SelectionStart) != Editor.GetLineFromPosition(Editor.SelectionEnd)) {
                Window.chkSearchSelection.Checked = true;
            }
            if (CurrentQuery != null) {
                Window.txtFind.Text = CurrentQuery.ToString();
            }
            else if (Editor.SelectionEnd > Editor.SelectionStart) {
                Window.txtFind.Text = Editor.SelectedText;
            }
            Window.txtFind.Select();
            Window.txtFind.SelectAll();
        }

        /// <summary>
        /// Shows the <see cref="IncrementalSearcher"/> control.
        /// </summary>
        public void ShowIncrementalSearch() {
            HideFindReplace();
            SetIncrementalSearchPosition();
            if (CurrentQuery != null) {
                SearchBar.txtFind.Text = CurrentQuery.ToString();
            }
            else if (Editor.SelectionEnd > Editor.SelectionStart) {
                SearchBar.txtFind.Text = Editor.SelectedText;
            }
            SearchBar.Show();
            SearchBar.txtFind.Focus();
            SearchBar.txtFind.SelectAll();
        }

        /// <summary>
        /// Hides the <see cref="IncrementalSearcher"/> control.
        /// </summary>
        public void HideIncrementalSearch() {
            if (SearchBar.Visible) {
                SearchBar.Hide();
            }
            Editor.Target.Focus();
        }

        // Set the searchbar position along the bottom of the editor
        private void SetIncrementalSearchPosition() {
            SearchBar.Left = Editor.Target.ClientRectangle.Left;
            SearchBar.Top = Editor.Target.ClientRectangle.Bottom - SearchBar.Height;
            SearchBar.Width = Editor.Target.ClientRectangle.Width;
        }

        // Update the searchbar position if the editor resizes
        private void ResizeEditor(object sender, EventArgs e) {
            SetIncrementalSearchPosition();
        }

        #endregion Show

        #region Search UI

        /// <summary>
        /// Searches for the given <see cref="TextRange"/> in the current results and returns a string describing its index.
        /// </summary>
        /// <param name="r"><see cref="TextRange"/> of interest.</param>
        /// <returns>String in the format "i out of N matches".</returns>
        public string GetIndexString(TextRange r) {
            if ((CurrentResults != null) && (r.end != r.start)) {
                var index = CurrentResults.BinarySearch(r) + 1;
                return index + " out of " + CurrentResults.Count + " matches";
            }
            else {
                return string.Empty;
            }
        }

        /// <summary>
        /// Clears highlights from the entire <see cref="IEditor"/> text.
        /// </summary>
        public List<TextRange> ClearAllHighlights() {
            if (Editor != null) {
                try { Editor.ClearAllHighlights(); }
                catch (NotImplementedException) { }
            }
            return CurrentResults;
        }

        /// <summary>
        /// Clears find marks from the margins for the entire <see cref="IEditor"/> text.
        /// </summary>
        public List<TextRange> ClearAllMarks() {
            if (Editor != null) {
                try { Editor.ClearAllMarks(); }
                catch (NotImplementedException) { }
            }
            return CurrentResults;
        }

        /// <summary>
        /// Clears both marks and highlight.
        /// </summary>
        public List<TextRange> Clear() {
            if (Editor != null) {
                ClearAllMarks();
                ClearAllHighlights();
                CurrentQuery = null;
                CurrentResults = new List<TextRange>();
                _updateHighlights = true;
            }
            return CurrentResults;
        }

        /// <summary>
        /// Highlight ranges in the <see cref="IEditor"/> text using the configured <see cref="Indicator"/>.
        /// </summary>
        /// <param name="ranges">List of ranges to which highlighting should be applied.</param>
        public void Highlight(List<TextRange> ranges) {
            if (Editor != null) {
                ClearAllHighlights();
                try { Editor.Highlight(ranges); }
                catch (NotImplementedException) { }
            }
        }

        /// <summary>
        /// Mark lines in the <see cref="IEditor"/> text using the configured <see cref="Marker"/>.
        /// </summary>
        /// <param name="ranges">List of ranges to which marks should be applied.</param>
        public void Mark(List<TextRange> ranges) {
            if (Editor != null) {
                ClearAllMarks();
                try { Editor.Mark(ranges); }
                catch (NotImplementedException) { }
            }
        }

        // Update query/results and mark/highlight results as necessary.
        private List<TextRange> UpdateResults(Search query, bool mark = false, bool highlight = true) {
            if (query != null) {
                if (!query.Equals(CurrentQuery) || _updateHighlights) {
                    CurrentQuery = query;
                    CurrentResults = query.FindAll(Editor);
                    if (highlight) {
                        Highlight(CurrentResults);
                    }
                    else {
                        ClearAllHighlights();
                    }
                    if (mark) {
                        Mark(CurrentResults);
                    }
                    else {
                        ClearAllMarks();
                    }
                }
                return CurrentResults;
            }
            else {
                return Clear();
            }
        }

        #endregion Search UI

        #region Search Processing

        // Run find/replace next/previous:
        // - Update history
        // - Run search and navigate to first result
        // - Return status text
        internal string RunFindReplace(Func<bool, TextRange> findReplace, Action addMru, bool searchUp) {
            string statusText;
            addMru();
            TextRange result;
            try {
                result = findReplace(searchUp);
            }
            catch (NullReferenceException) {
                // Expected: Search object could be null if constructor fails due to improper regex
                return null;
            }
            if (result.start == result.end) {
                statusText = "Match not found";
            }
            else {
                statusText = GetIndexString(result);
                if (HasWrapped(result, searchUp)) {
                    string boundary = searchUp ? "bottom" : "top";
                    string delimit = (statusText == string.Empty) ? "" : " | ";
                    statusText += delimit + "Wrapped from " + boundary;
                }
                SetEditorSelection(result);
            }
            _updateHighlights = false;
            return statusText;
        }

        // Run find/replace all:
        // - Update history
        // - Run search
        // - Return status text
        internal string RunFindReplaceAll(Func<List<TextRange>> findReplaceAll, Action addMru, bool replace) {
            string statusText;

            addMru();
            List<TextRange> results;
            try {
                results = findReplaceAll();
            }
            catch (NullReferenceException) {
                // Expected: Search object could be null if constructor fails due to improper regex
                return null;
            }

            if (results.Count == 0) {
                statusText = "Match could not be found";
            }
            else {
                statusText = "Total " + (replace ? "replaced" : "found") + ": " + results.Count.ToString();
            }
            _updateHighlights = false;
            return statusText;
        }

        // Adds the given text to the find history
        internal void AddFindHistory(string findText) {
            AddHistory(findText, FindHistory);
        }

        // Adds the given text to the find & replace history
        internal void AddReplaceHistory(string findText, string replaceText) {
            AddHistory(findText, FindHistory);
            AddHistory(replaceText, ReplaceHistory);
        }

        // Adds the given text to the given list. If there is not enough room, the oldest entry is removed.
        private void AddHistory(string text, List<string> mru) {
            if (text != string.Empty) {
                mru.Remove(text);
                mru.Insert(0, text);

                if (mru.Count > _historyMaxCount) {
                    mru.RemoveAt(mru.Count - 1);
                }
            }
        }

        // Raise the KeyPressed event.
        private void FindReplace_KeyPressed(object sender, KeyEventArgs e) {
            KeyPressed?.Invoke(this, e);
        }

        #endregion Search Processing

        #region Utility

        /// <summary>
        /// Creates and returns a new <see cref="IncrementalSearcher" /> object.
        /// </summary>
        /// <returns>A new <see cref="IncrementalSearcher" /> object.</returns>
        private IncrementalSearcher CreateIncrementalSearcherInstance() {
            return new IncrementalSearcher(this);
        }

        /// <summary>
        /// Creates and returns a new <see cref="FindReplaceDialog" /> object.
        /// </summary>
        /// <returns>A new <see cref="FindReplaceDialog" /> object.</returns>
        private FindReplaceDialog CreateWindowInstance() {
            return new FindReplaceDialog(this);
        }

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="FindReplace"/>.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                Window?.Dispose();
                SearchBar?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if the given range has wrapped relative to the current selection in the editor.
        /// </summary>
        /// <param name="range"><see cref="TextRange"/> to check.</param>
        /// <param name="up">Direction to check.</param>
        /// <returns>True if the given range is below the current selection when the direction is up, or above the current selection when the direction is down.</returns>
        public bool HasWrapped(TextRange range, bool up) {
            return up ? (range.start > Editor.CurrentPosition) : (range.start < Editor.AnchorPosition);
        }

        /// <summary>
        /// Checks if the editor has any selected text.
        /// </summary>
        /// <returns>True if there are any selections.</returns>
        public bool EditorHasSelection() {
            return Editor.SelectionLength > 0;
        }

        /// <summary>
        /// Sets the editor selection to the given range.
        /// </summary>
        /// <param name="range">Target <see cref="TextRange"/> that will be the new editor selection.</param>
        public void SetEditorSelection(TextRange range) {
            Editor.SelectionStart = range.start;
            Editor.SelectionEnd = range.end;
            Editor.ScrollToCaret();
        }

        /// <summary>
        /// Returns the <see cref="TextRange"/> currently selected in the editor.
        /// </summary>
        /// <returns><see cref="TextRange"/> between the anchor and current position.</returns>
        public TextRange GetEditorSelectedRange() {
            return new TextRange(Editor.SelectionStart, Editor.SelectionEnd);
        }

        /// <summary>
        /// Returns the <see cref="TextRange"/> of the whole document in the editor.
        /// </summary>
        /// <returns><see cref="TextRange"/> between zero and the text length.</returns>
        public TextRange GetEditorWholeRange() {
            return new TextRange(0, Editor.TextLength);
        }

        #endregion Utility

        #endregion Methods
    }
}