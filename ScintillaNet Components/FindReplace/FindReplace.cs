namespace ScintillaNET_Components
{
    #region Using Directives

    using ScintillaNET;
    using ScintillaNET_Components.SearchTypes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    #endregion Using Directives

    /// <summary>
    /// Class to enable programmatic and GUI-based finding and replacing of text in a <see cref="Scintilla"/> editor.
    /// </summary>
    public class FindReplace : ComponentManager
    {
        #region Fields

        private readonly int _historyMaxCount = 10;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="FindReplace"/> with associated
        /// <see cref="FindReplaceDialog"/> and <see cref="IncrementalSearcher"/> instances.
        /// </summary>
        /// <param name="editor">The <see cref="Scintilla"/> editor to which the <see cref="FindReplace"/> is attached.</param>
        public FindReplace(Scintilla editor) : base() {
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
        /// Gets or sets the associated <see cref="Scintilla"/> control that <see cref="FindReplace"/> can act upon.
        /// </summary>
        public override Scintilla Editor {
            get {
                return base.Editor;
            }
            set {
                base.Editor = value;

                FoundMarker = new Marker(Editor, Constants.FoundMarkerIndex) {
                    Symbol = MarkerSymbol.SmallRect,
                };
                FoundMarker.SetForeColor(Color.DarkOrange);
                FoundMarker.SetBackColor(Color.DarkOrange);

                FoundIndicator = new Indicator(Editor, Constants.FoundIndicatorIndex) {
                    ForeColor = Color.DarkOrange,
                    Alpha = 100,
                    Style = IndicatorStyle.RoundBox,
                    Under = true
                };

                Editor.Controls.Add(SearchBar);
                Editor.Resize += ResizeEditor;
                UpdateHighlights = true;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IncrementalSearcher"/>.
        /// </summary>
        public IncrementalSearcher SearchBar { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FindReplaceDialog"/> instance.
        /// </summary>
        public FindReplaceDialog Window { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Indicator"/> used to mark found results in the document.
        /// </summary>
        public Indicator FoundIndicator { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Marker"/> used to mark found results in the margin.
        /// </summary>
        public Marker FoundMarker { get; set; }

        /// <summary>
        /// Gets the <see cref="Search"/> object that encapsualtes the last completed find/replace action.
        /// </summary>
        public Search CurrentQuery { get; internal set; }

        /// <summary>
        /// Gets the <see cref="List{CharacterRange}"/> that contains the last find/replace results.
        /// </summary>
        public List<CharacterRange> CurrentResults { get; private set; }

        /// <summary>
        /// Gets the list of terms that have been searched.
        /// </summary>
        public List<string> FindHistory { get; }

        /// <summary>
        /// Gets the list of terms used to replace search results.
        /// </summary>
        public List<string> ReplaceHistory { get; }

        internal bool UpdateHighlights { get; set; }

        #endregion Properties

        #region Methods

        #region Find

        /// <summary>
        /// Searches for the first or last instance of the given Regular Expression in the given range of the <see cref="Scintilla"/> text.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="searchUp">Search direction. Set to true to search from the bottom up.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Find(CharacterRange searchRange, Regex findExpression, bool searchUp) {
            RegexSearch query = new RegexSearch {
                SearchRange = searchRange,
                SearchExpression = findExpression,
                SearchUp = searchUp
            };
            return query.Find(Editor);
        }

        /// <summary>
        /// Searches for the first or last instance of the given string in the given range of the <see cref="Scintilla"/> text, using the specified options.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="searchUp">Search direction. Set to true to search from the bottom up.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Find(CharacterRange searchRange, string searchString, SearchFlags searchFlags, bool searchUp) {
            StringSearch query = new StringSearch() {
                SearchRange = searchRange,
                SearchString = searchString,
                Flags = searchFlags,
                SearchUp = searchUp
            };
            return query.Find(Editor);
        }

        /// <summary>
        /// Searches for the first or last instance of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="searchUp">Search direction. Set to true to search from the bottom up.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Find(Search query, bool searchUp) {
            query.SearchUp = searchUp;
            return query.Find(Editor);
        }

        #endregion Find

        #region FindAll

        /// <summary>
        /// Searches for all instances of the given Regular Expression in the full body of the <see cref="Scintilla"/> text, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(Regex findExpression, bool mark, bool highlight) {
            return FindAll(new CharacterRange(0, Editor.TextLength), findExpression, mark, highlight);
        }

        /// <summary>
        /// Searches for all instances of the given Regular Expression in the given range of the <see cref="Scintilla"/> text, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(CharacterRange searchRange, Regex findExpression, bool mark, bool highlight) {
            RegexSearch query = new RegexSearch() { SearchRange = searchRange, SearchExpression = findExpression };
            return FindAll(query, mark, highlight);
        }

        /// <summary>
        /// Searches for all instances of the given string in the full body of the <see cref="Scintilla"/> text, using the specified options, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(string searchString, SearchFlags searchFlags, bool mark, bool highlight) {
            return FindAll(new CharacterRange(0, Editor.TextLength), searchString, searchFlags, mark, highlight);
        }

        /// <summary>
        /// Searches for all instances of the given string in the given range of the <see cref="Scintilla"/> text, using the specified options, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(CharacterRange searchRange, string searchString, SearchFlags searchFlags, bool mark, bool highlight) {
            StringSearch query = new StringSearch() { SearchRange = searchRange, SearchString = searchString, Flags = searchFlags };
            return FindAll(query, mark, highlight);
        }

        /// <summary>
        /// Searches for all instances of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(Search query, bool mark, bool highlight) {
            return UpdateResults(query, mark, highlight);
        }

        #endregion FindAll

        #region FindNext

        /// <summary>
        /// Searches for the next instance of the given Regular Expression in the full body of the <see cref="Scintilla"/> text based on the current caret position, optionally wrapping back to the beginning.
        /// </summary>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(Regex findExpression, bool wrap) {
            return FindNext(new CharacterRange(0, Editor.TextLength), findExpression, wrap);
        }

        /// <summary>
        /// Searches for the next instance of the given Regular Expression in the given range of the <see cref="Scintilla"/> text based on the current caret position, optionally wrapping back to the beginning.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(CharacterRange searchRange, Regex findExpression, bool wrap) {
            RegexSearch query = new RegexSearch() { SearchRange = searchRange, SearchExpression = findExpression };
            UpdateResults(query);
            return query.FindNext(Editor, wrap);
        }

        /// <summary>
        /// Searches for the next instance of the given string in the full body of the <see cref="Scintilla"/> text based on the current caret position, using the specified options, and optionally wrapping back to the beginning.
        /// </summary>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(string searchString, SearchFlags searchFlags, bool wrap) {
            return FindNext(new CharacterRange(0, Editor.TextLength), searchString, searchFlags, wrap);
        }

        /// <summary>
        /// Searches for the next instance of the given string in the given range of the <see cref="Scintilla"/> text based on the current caret position, using the specified options, and optionally wrapping back to the beginning.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(CharacterRange searchRange, string searchString, SearchFlags searchFlags, bool wrap) {
            StringSearch query = new StringSearch() { SearchRange = searchRange, SearchString = searchString, Flags = searchFlags };
            return FindNext(query, wrap);
        }

        /// <summary>
        /// Searches for the next instance of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(Search query, bool wrap) {
            UpdateResults(query);
            return query.FindNext(Editor, wrap);
        }

        #endregion FindNext

        #region FindPrevious

        /// <summary>
        /// Searches for the previous instance of the given Regular Expression in the full body of the <see cref="Scintilla"/> text based on the current caret position, optionally wrapping back to the end.
        /// </summary>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(Regex findExpression, bool wrap) {
            return FindPrevious(new CharacterRange(0, Editor.TextLength), findExpression, wrap);
        }

        /// <summary>
        /// Searches for the previous instance of the given Regular Expression in the given range of the <see cref="Scintilla"/> text based on the current caret position, optionally wrapping back to the end.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(CharacterRange searchRange, Regex findExpression, bool wrap) {
            RegexSearch query = new RegexSearch() { SearchRange = searchRange, SearchExpression = findExpression };
            UpdateResults(query);
            return query.FindPrevious(Editor, wrap);
        }

        /// <summary>
        /// Searches for the previous instance of the given string in the full body of the <see cref="Scintilla"/> text based on the current caret position, using the specified options, and optionally wrapping back to the end.
        /// </summary>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(string searchString, SearchFlags searchFlags, bool wrap) {
            return FindPrevious(new CharacterRange(0, Editor.TextLength), searchString, searchFlags, wrap);
        }

        /// <summary>
        /// Searches for the previous instance of the given string in the given range of the <see cref="Scintilla"/> text based on the current caret position, using the specified options, and optionally wrapping back to the end.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(CharacterRange searchRange, string searchString, SearchFlags searchFlags, bool wrap) {
            StringSearch query = new StringSearch() { SearchRange = searchRange, SearchString = searchString, Flags = searchFlags };
            return FindPrevious(query, wrap);
        }


        /// <summary>
        /// Searches for the previous instance of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(Search query, bool wrap) {
            UpdateResults(query);
            return query.FindPrevious(Editor, wrap);
        }

        #endregion FindPrevious

        #region Replace

        /// <summary>
        /// Replaces the next instance of the given Regular Expression in the full body of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Replace(Regex findExpression, string replaceString, bool wrap) {
            return Replace(new CharacterRange(0, Editor.TextLength), findExpression, replaceString, wrap);
        }

        /// <summary>
        /// Replaces the next instance of the given Regular Expression in the given range of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Replace(CharacterRange searchRange, Regex findExpression, string replaceString, bool wrap) {
            RegexSearch query = new RegexSearch() { SearchRange = searchRange, SearchExpression = findExpression };
            UpdateResults(query);
            return query.Replace(Editor, replaceString, wrap);
        }

        /// <summary>
        /// Replaces the next instance of the given string in the full body of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Replace(string searchString, string replaceString, SearchFlags searchFlags, bool wrap) {
            return Replace(new CharacterRange(0, Editor.TextLength), searchString, replaceString, searchFlags, wrap);
        }

        /// <summary>
        /// Replaces the next instance of the given string in the given range of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Replace(CharacterRange searchRange, string searchString, string replaceString, SearchFlags searchFlags, bool wrap) {
            StringSearch query = new StringSearch() { SearchRange = searchRange, SearchString = searchString, Flags = searchFlags };
            return Replace(query, replaceString, wrap);
        }


        /// <summary>
        /// Replaces the next instance of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange Replace(Search query, string replaceString, bool wrap) {
            UpdateResults(query);
            return query.Replace(Editor, replaceString, wrap);
        }

        #endregion Replace

        #region ReplaceAll

        /// <summary>
        /// Replaces all instances of the given Regular Expression in the full body of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> ReplaceAll(Regex findExpression, string replaceString, bool mark, bool highlight) {
            return ReplaceAll(new CharacterRange(0, Editor.TextLength), findExpression, replaceString, mark, highlight);
        }

        /// <summary>
        /// Replaces all instances of the given Regular Expression in the given range of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="findExpression"><see cref="Regex"/> for which to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> ReplaceAll(CharacterRange searchRange, Regex findExpression, string replaceString, bool mark, bool highlight) {
            RegexSearch query = new RegexSearch() { SearchRange = searchRange, SearchExpression = findExpression };
            return ReplaceAll(query, replaceString, mark, highlight);
        }

        /// <summary>
        /// Replaces all instances of the given string in the full body of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> ReplaceAll(string searchString, string replaceString, SearchFlags searchFlags, bool mark, bool highlight) {
            return ReplaceAll(new CharacterRange(0, Editor.TextLength), searchString, replaceString, searchFlags, mark, highlight);
        }

        /// <summary>
        /// Replaces all instances of the given string in the given range of the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="searchRange"><see cref="CharacterRange"/> in which to search.</param>
        /// <param name="searchString">Text for which to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <param name="searchFlags"><see cref="SearchFlags"/> enumeration that sets the pattern matching options.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> ReplaceAll(CharacterRange searchRange, string searchString, string replaceString, SearchFlags searchFlags, bool mark, bool highlight) {
            StringSearch query = new StringSearch() { SearchRange = searchRange, SearchString = searchString, Flags = searchFlags };
            return ReplaceAll(query, replaceString, mark, highlight);
        }

        /// <summary>
        /// Replaces all instances of the given <see cref="Search"/> query in the <see cref="Scintilla"/> text with the given string, and optionally and marks/highlights them.
        /// </summary>
        /// <param name="query"><see cref="Search"/> object to use to perform the search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern if <c>query</c> is a <see cref="RegexSearch"/> object.</param>
        /// <param name="mark">Set to true to use the configured margin marker to indicate the lines where matches were found.</param>
        /// <param name="highlight">Set to true to use the configured text indicator to highlight each match.</param>
        /// <returns></returns>
        public List<CharacterRange> ReplaceAll(Search query, string replaceString, bool mark, bool highlight) {
            return query.ReplaceAll(Editor, replaceString);
        }

        #endregion ReplaceAll

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
            Editor.Focus();
        }

        // Displays the FindReplaceDialog with the specified tab active, and sets the selection/text appropriately.
        private void ShowFindReplaceTab(string tabName) {
            HideIncrementalSearch();
            if (!Window.Visible) {
                Window.Show(Editor.FindForm());
            }

            Window.tabAll.SelectedTab = Window.tabAll.TabPages[tabName];

            if (Editor.LineFromPosition(Editor.Selections[0].Start) != Editor.LineFromPosition(Editor.Selections[0].End)) {
                Window.chkSearchSelection.Checked = true;
            }
            if (CurrentQuery != null) {
                Window.txtFind.Text = CurrentQuery.ToString();
            }
            else if (Editor.Selections[0].End > Editor.Selections[0].Start) {
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
            else if (Editor.Selections[0].End > Editor.Selections[0].Start) {
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
            Editor.Focus();
        }

        // Set the searchbar position along the bottom of the editor
        private void SetIncrementalSearchPosition() {
            SearchBar.Left = Editor.ClientRectangle.Left;
            SearchBar.Top = Editor.ClientRectangle.Bottom - SearchBar.Height;
            SearchBar.Width = Editor.ClientRectangle.Width;
        }

        // Update the searchbar position if the editor resizes
        private void ResizeEditor(object sender, EventArgs e) {
            SetIncrementalSearchPosition();
        }

        #endregion Show

        #region Search UI

        /// <summary>
        /// Searches for the given <see cref="CharacterRange"/> in the current results and returns a string describing its index.
        /// </summary>
        /// <param name="r"><see cref="CharacterRange"/> of interest.</param>
        /// <returns>String in the format "i out of N matches".</returns>
        public string GetIndexString(CharacterRange r) {
            if ((CurrentResults != null) && (r.cpMax != r.cpMin)) {
                var index = CurrentResults.BinarySearch(r) + 1;
                return index + " out of " + CurrentResults.Count + " matches";
            }
            else {
                return string.Empty;
            }
        }

        /// <summary>
        /// Clears highlights from the entire <see cref="Scintilla"/> text.
        /// </summary>
        public List<CharacterRange> ClearAllHighlights() {
            if (Editor != null) {
                int currentIndicator = Editor.IndicatorCurrent;
                Editor.IndicatorCurrent = FoundIndicator.Index;
                Editor.IndicatorClearRange(0, Editor.TextLength);
                Editor.IndicatorCurrent = currentIndicator;
            }
            return CurrentResults;
        }

        /// <summary>
        /// Clears find marks from the margins for the entire <see cref="Scintilla"/> text.
        /// </summary>
        public List<CharacterRange> ClearAllMarks() {
            if (Editor != null) {
                Editor.MarkerDeleteAll(FoundMarker.Index);
            }
            return CurrentResults;
        }

        /// <summary>
        /// Clears both marks and highlight.
        /// </summary>
        public List<CharacterRange> Clear() {
            if (Editor != null) {
                ClearAllMarks();
                ClearAllHighlights();
                CurrentQuery = null;
                CurrentResults = new List<CharacterRange>();
                UpdateHighlights = true;
            }
            return CurrentResults;
        }

        /// <summary>
        /// Highlight ranges in the <see cref="Scintilla"/> text using the configured <see cref="Indicator"/>.
        /// </summary>
        /// <param name="ranges">List of ranges to which highlighting should be applied.</param>
        public void Highlight(List<CharacterRange> ranges) {
            if (Editor != null) {
                ClearAllHighlights();
                Editor.IndicatorCurrent = FoundIndicator.Index;

                foreach (var r in ranges) {
                    Editor.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);
                }
            }
        }

        /// <summary>
        /// Mark lines in the <see cref="Scintilla"/> text using the configured <see cref="Marker"/>.
        /// </summary>
        /// <param name="ranges">List of ranges to which marks should be applied.</param>
        public void Mark(List<CharacterRange> ranges) {
            if (Editor != null) {
                ClearAllMarks();
                var lastLine = -1;
                foreach (var r in ranges) {
                    Line line = new Line(Editor, Editor.LineFromPosition(r.cpMin));
                    if (line.Position > lastLine) {
                        line.MarkerAdd(FoundMarker.Index);
                    }
                    lastLine = line.Position;
                }
            }
        }

        // Update query/results and mark/highlight results as necessary.
        private List<CharacterRange> UpdateResults(Search query, bool mark = false, bool highlight = true) {
            if (query != null) {
                if (!query.Equals(CurrentQuery) || UpdateHighlights) {
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
        internal string RunFindReplace(Func<bool, CharacterRange> findReplace, Action addMru, bool searchUp) {
            var statusText = string.Empty;
            addMru();
            CharacterRange result;
            try {
                result = findReplace(searchUp);
            }
            catch (NullReferenceException) {
                // Expected: Search object could be null if constructor fails due to improper regex
                return null;
            }
            if (result.cpMin == result.cpMax) {
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
            UpdateHighlights = false;
            return statusText;
        }

        // Run find/replace all:
        // - Update history
        // - Run search
        // - Return status text
        internal string RunFindReplaceAll(Func<List<CharacterRange>> findReplaceAll, Action addMru, bool replace) {
            var statusText = string.Empty;

            addMru();
            List<CharacterRange> results;
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
            UpdateHighlights = false;
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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual IncrementalSearcher CreateIncrementalSearcherInstance() {
            return new IncrementalSearcher(this);
        }

        /// <summary>
        /// Creates and returns a new <see cref="FindReplaceDialog" /> object.
        /// </summary>
        /// <returns>A new <see cref="FindReplaceDialog" /> object.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual FindReplaceDialog CreateWindowInstance() {
            return new FindReplaceDialog(this);
        }

        /// <summary>
        /// Checks if the given range has wrapped relative to the current selection in the editor.
        /// </summary>
        /// <param name="range"><see cref="CharacterRange"/> to check.</param>
        /// <param name="up">Direction to check.</param>
        /// <returns>True if the given range is below the current selection when the direction is up, or above the current selection when the direction is down.</returns>
        public bool HasWrapped(CharacterRange range, bool up) {
            return up ? (range.cpMin > Editor.CurrentPosition) : (range.cpMin < Editor.AnchorPosition);
        }

        /// <summary>
        /// Checks if the editor has any selected text.
        /// </summary>
        /// <returns>True if there are any selections.</returns>
        public bool EditorHasSelection() {
            return Editor.Selections.Count > 0;
        }

        /// <summary>
        /// Sets the editor selection to the given range.
        /// </summary>
        /// <param name="range">Target <see cref="CharacterRange"/> that will be the new editor selection.</param>
        public void SetEditorSelection(CharacterRange range) {
            Editor.SetSel(range.cpMin, range.cpMax);
        }

        /// <summary>
        /// Returns the <see cref="CharacterRange"/> currently selected in the editor.
        /// </summary>
        /// <returns><see cref="CharacterRange"/> between the anchor and current position.</returns>
        public CharacterRange GetEditorSelectedRange() {
            return new CharacterRange(Editor.SelectionStart, Editor.SelectionEnd);
        }

        /// <summary>
        /// Returns the <see cref="CharacterRange"/> of the whole document in the editor.
        /// </summary>
        /// <returns><see cref="CharacterRange"/> between zero and the text length.</returns>
        public CharacterRange GetEditorWholeRange() {
            return new CharacterRange(0, Editor.TextLength);
        }

        #endregion Utility

        #endregion Methods
    }
}