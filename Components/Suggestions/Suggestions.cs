using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Component to 
    /// </summary>
    public class Suggestions : ComponentManager
    {

        #region Fields

        private ICollection<SuggestionItem> _items;
        private TextRange _fragment;
        private bool _forced;
        private bool _delete;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Suggestions"/> instance with associated <see cref="SuggestionDropDown"/> instance.
        /// </summary>
        /// <param name="editor">>The <see cref="IEditor"/> editor to which the <see cref="Suggestions"/> is attached.</param>
        public Suggestions(IEditor editor) {
            MinimumFragmentLength = 3;
            MaximumVisibleItems = 5;
            DelimiterPattern = @"[\s]";

            if (editor != null) {
                Editor = editor;
                DropDown = CreateDropDownInstance();
                DropDown.List.ItemSelected += ListBox_Selected;
                Font = GetEditorFont();
                MaximumWidth = 500;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="Suggestions"/> instance.
        /// </summary>
        public Suggestions() : this(null) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the associated <see cref="IEditor"/> control that <see cref="Suggestions"/> can act upon.
        /// </summary>
        public override IEditor Editor {
            get {
                return base.Editor;
            }
            set {
                if (base.Editor != null) {
                    Editor.LostFocus -= Editor_LostFocus;
                    Editor.MouseDown -= Editor_MouseDown;
                    Editor.KeyDown -= Editor_KeyDown;
                    Editor.TextChanged -= Editor_TextChanged;
                    Editor.Scroll -= Editor_Scroll;
                }
                base.Editor = value;
                Editor.LostFocus += Editor_LostFocus;
                Editor.MouseDown += Editor_MouseDown;
                Editor.KeyDown += Editor_KeyDown;
                Editor.TextChanged += Editor_TextChanged;
                Editor.Scroll += Editor_Scroll;
            }
        }

        /// <summary>
        /// Gets the <see cref="SuggestionDropDown"/>.
        /// </summary>
        public SuggestionDropDown DropDown { get; private set; }

        /// <summary>
        /// Gets the list of visible suggestion items.
        /// </summary>
        [Browsable(false)]
        public IList<SuggestionItem> VisibleItems {
            get { return DropDown.List.VisibleItems; }
            private set { DropDown.List.VisibleItems = value; }
        }

        /// <summary>
        /// Gets or sets the suggestion text to be displayed in the list.
        /// </summary>
        public string[] Items {
            get {
                if (_items == null) {
                    return null;
                }
                var list = new List<string>();
                foreach (SuggestionItem item in _items) {
                    list.Add(item.ToString());
                }
                return list.ToArray();
            }
            set { SetSuggestions(value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of items that will be displayed in the list.
        /// </summary>
        public int MaximumVisibleItems { get; set; }

        /// <summary>
        /// Gets the index of the selected suggestion item.
        /// </summary>
        [Browsable(false)]
        public int SelectedItemIndex {
            get { return DropDown.List.SelectedItemIndex; }
            private set { DropDown.List.SelectedItemIndex = value; }
        }

        /// <summary>
        /// Gets the index of the selected suggestion item.
        /// </summary>
        [Browsable(false)]
        public int HighlightedItemIndex {
            get { return DropDown.List.HighlightedItemIndex; }
            private set { DropDown.List.HighlightedItemIndex = value; }
        }

        /// <summary>
        /// Gets or sets whether the autocomplete suggestions will be displayed.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets whether the autcoplete suggestions will be automatically displayed as the user types.
        /// </summary>
        public bool AutoShow { get; set; }

        /// <summary>
        /// Gets or sets the search pattern used to break up text around the caret.
        /// </summary>
        public string DelimiterPattern { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of text that must be typed to show the suggestion list when <see cref="AutoShow"/> is true.
        /// </summary>
        public int MinimumFragmentLength { get; set; }

        /// <summary>
        /// Gets or sets whether the targeted editor's font will be used in the suggestion list.
        /// </summary>
        public bool UseEditorFont { get; set; }

        /// <summary>
        /// Gets or sets the font used in the suggestion list. This is ignored if <see cref="UseEditorFont"/> is true.
        /// </summary>
        public Font Font {
            get { return DropDown.List.Font; }
            set { DropDown.List.Font = value; }
        }

        /// <summary>
        /// Gets or sets the list of images to optionally display alongside the <see cref="SuggestionItem"/> list.
        /// </summary>
        public ImageList SuggestionImages {
            get {
                return DropDown.List.ImageList;
            }
            set {
                DropDown.List.ImageList = value;
            }
        }

        /// <summary>
        /// Gets or sets the color theme for the suggestion menu.
        /// </summary>
        public ListTheme Theme {
            get {
                return DropDown.List.Theme;
            }
            set {
                DropDown.List.Theme = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum width in pixels of the list.
        /// </summary>
        public int MaximumWidth {
            get {
                return DropDown.MaximumSize.Width;
            }
            set {
                DropDown.MaximumSize = new Size(value, DropDown.MaximumSize.Height);
            }
        }


        #endregion Properties

        #region Events & Handlers

        /// <summary>
        /// Triggered when a suggestion has been selected but before it is inserted.
        /// </summary>
        public event EventHandler<SelectingEventArgs> SuggestionSelecting;

        /// <summary>
        /// Triggered when a suggestion has been selected and inserted.
        /// </summary>
        public event EventHandler<SelectedEventArgs> SuggestionSelected;

        // Raises the suggestion selecting event.
        private void OnSuggestionSelecting(SelectingEventArgs e) {
            SuggestionSelecting?.Invoke(this, e);
        }

        // Raises the suggestion selected event.
        private void OnSuggestionSelected(SelectedEventArgs e) {
            SuggestionSelected?.Invoke(this, e);
        }

        // Called when the editor loses focus. 
        private void Editor_LostFocus(object sender, EventArgs e) {
            if (!DropDown.Focused) {
                HideSuggestions();
            }
        }

        // Called when the mouse is clicked in the editor.
        private void Editor_MouseDown(object sender, MouseEventArgs e) {
            HideSuggestions();
        }

        // Called when the editor is scrolled.
        private void Editor_Scroll(object sender, ScrollEventArgs e) {
            HideSuggestions();
        }

        // Called when a key is pressed in the editor.
        private void Editor_KeyDown(object sender, KeyEventArgs e) {
            if (DropDown.Visible) {
                if (ProcessKey(e.KeyCode)) {
                    e.SuppressKeyPress = true;
                    return;
                }
            }
            switch (e.KeyCode) {
                case Keys.Back:
                case Keys.Delete:
                    _delete = true;
                    return;
                case Keys.Space:
                    if (e.Control) {
                        ShowSuggestions(true);
                        e.SuppressKeyPress = true;
                    }
                    return;
                default:
                    _delete = false;
                    return;
            }
        }

        private void Editor_TextChanged(object sender, EventArgs e) {
            if (DropDown.Visible || !_delete) {
                ShowSuggestions(false);
            }
            else {
                HideSuggestions();
            }
        }

        // Called when an item is double-clicked in the listbox.
        private void ListBox_Selected(object sender, EventArgs e) {
            ProcessSelection();
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Displays the list of suggestions.
        /// </summary>
        public void ShowSuggestions(bool forced) {
            if (forced) _forced = true;
            BuildVisibleSuggestions(_forced);
            if (VisibleItems.Count > 0) {
                Point point = Editor.GetPointFromPosition(_fragment.Start);
                point.Offset(0, Editor.LineHeight);

                if (UseEditorFont) {
                    Font = GetEditorFont();
                }
                // Adjust size according to contents and limits
                int numVisibleItems = Math.Min(MaximumVisibleItems, VisibleItems.Count);
                int listWidth = DropDown.List.ImageWidth + DropDown.List.ItemWidth + SystemInformation.VerticalScrollBarWidth;
                int listHeight = numVisibleItems * DropDown.List.ItemHeight;
                DropDown.List.Size = new Size(listWidth, listHeight);
                DropDown.List.VerticalScroll.Value = 0;

                int hostWidth = Math.Min(listWidth + DropDown.List.Margin.Size.Width + 5, DropDown.MaximumSize.Width);
                int hostHeight = listHeight + DropDown.List.Margin.Size.Height + DropDown.List.Location.Y + 1;
                DropDown.Size = new Size(hostWidth, hostHeight);

                DropDown.Show(Editor.Target, point, ToolStripDropDownDirection.BelowRight);
            }
            else {
                HideSuggestions();
            }
        }

        /// <summary>
        /// Hides the list of suggestions.
        /// </summary>
        public void HideSuggestions() {
            _forced = false;
            DropDown.Close();
        }

        /// <summary>
        /// Sets the list of <see cref="SuggestionItem"/> objects that can be displayed if a matching query is found.
        /// </summary>
        /// <param name="suggestions"><see cref="ICollection{Suggestion}"/> to be stored.</param>
        public void SetSuggestions(ICollection<SuggestionItem> suggestions) {
            _items = suggestions;
            DropDown.List.VisibleItems = (IList<SuggestionItem>)suggestions;
        }

        /// <summary>
        /// Sets the list of text suggestions that can be displayed if a matching query is found.
        /// </summary>
        /// <param name="suggestions"><see cref="ICollection"/> of strings to be stored.</param>
        public void SetSuggestions(ICollection<string> suggestions) {
            var list = new List<SuggestionItem>();
            if (suggestions == null) {
                _items = null;
                return;
            }
            foreach (string text in suggestions) {
                list.Add(new SuggestionItem(text));
            }
            SetSuggestions(list);
        }

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="Suggestions"/> instance.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                DropDown?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates and returns a new <see cref="SuggestionDropDown"/> object.
        /// </summary>
        /// <returns>A new <see cref="SuggestionDropDown"/> object.</returns>
        private SuggestionDropDown CreateDropDownInstance() {
            return new SuggestionDropDown(this);
        }

        /// <summary>
        /// Moves the selection in the suggestion list by the specified delta.
        /// </summary>
        /// <param name="delta">The change in selection index; set to positive to move selection down or negative to move selection up.</param>
        public void MoveSelection(int delta) {
            int initialIndex = -1;
            if (DropDown.List.SelectedItemIndex >= 0) {
                initialIndex = DropDown.List.SelectedItemIndex;
            }
            else if (DropDown.List.HighlightedItemIndex >= 0) {
                initialIndex = DropDown.List.HighlightedItemIndex;
            }
            DropDown.List.Focus();
            DropDown.List.SelectedItemIndex = Math.Max(0, Math.Min(initialIndex + delta, DropDown.List.VisibleItems.Count - 1));
        }

        private void BuildVisibleSuggestions(bool forced) {
            if (_items == null) {
                return;
            }

            var visibleItems = new List<SuggestionItem>();
            bool itemSelected = false;
            int selectedItemIndex = -1;
            TextRange fragment = GetFragment(DelimiterPattern);
            if (forced || (fragment.End - fragment.Start) >= MinimumFragmentLength) {
                _fragment = fragment;
                var fragmentText = Editor.GetTextRange(fragment);
                if (!string.IsNullOrWhiteSpace(fragmentText)) {
                    foreach (var item in _items) {
                        if (item.Match(fragmentText, false)) {
                            visibleItems.Add(item);
                            if (!itemSelected) {
                                itemSelected = true;
                                selectedItemIndex = visibleItems.Count - 1;
                            }
                        }
                    }
                }
            }
            VisibleItems = visibleItems;

            if (itemSelected) {
                SelectedItemIndex = selectedItemIndex;
            }
            else {
                SelectedItemIndex = 0;
            }
        }

        // Process key presses
        internal bool ProcessKey(Keys key) {
            var page = DropDown.Height / (DropDown.List.ItemHeight);
            switch (key) {
                case Keys.Down:
                    MoveSelection(+1);
                    return true;
                case Keys.PageDown:
                    MoveSelection(+page);
                    return true;
                case Keys.Up:
                    MoveSelection(-1);
                    return true;
                case Keys.PageUp:
                    MoveSelection(-page);
                    return true;
                case Keys.Enter:
                    ProcessSelection();
                    return true;
                case Keys.Tab:
                    ProcessSelection();
                    return true;
                case Keys.Left:
                case Keys.Right:
                    HideSuggestions();
                    // Do not want to discard
                    return false;
                case Keys.Escape:
                    HideSuggestions();
                    return true;
            }
            return false;
        }

        private void ProcessSelection() {
            if (SelectedItemIndex < 0) {
                return;
            }
            var selection = VisibleItems[SelectedItemIndex];
            var selectingArgs = new SelectingEventArgs { Selection = selection, SelectionIndex = SelectedItemIndex };
            OnSuggestionSelecting(selectingArgs);

            // An external handler could modify the args, so we may have to cancel
            if (selectingArgs.Cancel) {
                // Restore the selection to be sure
                SelectedItemIndex = selectingArgs.SelectionIndex;
                return;
            }
            // Or the external handler may handle the autocomplete
            if (!selectingArgs.Handled) {
                // Autocomplete
                Editor.SelectionStart = _fragment.Start;
                Editor.SelectionEnd = _fragment.End;
                Editor.SelectedText = selection.InsertText;
                var selectedArgs = new SelectedEventArgs { Selection = selection, SelectionIndex = SelectedItemIndex, Control = Editor.Target };
                selection.OnSuggestionSelected(selectedArgs);
                OnSuggestionSelected(selectedArgs);
            }
            HideSuggestions();

            Editor.Target.Focus();
        }

        private TextRange GetFragment(string delimiterPattern) {
            // Use existing selection if available
            if (Editor.SelectionLength > 0) {
                return new TextRange(Editor.SelectionStart, Editor.SelectionEnd);
            }

            string text = Editor.Text;
            var regex = new Regex(delimiterPattern);

            // Search forwards
            int i = Editor.SelectionStart;
            while (i >= 0 && i < text.Length) {
                if (regex.IsMatch(text[i].ToString())) {
                    break;
                }
                i++;
            }
            int fragmentEnd = i;

            if (fragmentEnd != Editor.SelectionStart) {
                // Search backwards
                i = Editor.SelectionStart;
                while (i > 0 && (i - 1) < text.Length) {
                    if (regex.IsMatch(text[i - 1].ToString())) {
                        break;
                    }
                    i--;
                }
            }
            int fragmentStart = i;

            return new TextRange(fragmentStart, fragmentEnd);
        }

        #endregion Methods
    }

    #region Event Classes

    /// <summary>
    /// Event data for the selecting event, which is fired when a suggestion has been selected but before it is inserted.
    /// </summary>
    public class SelectingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the currently-selected suggestion item.
        /// </summary>
        public SuggestionItem Selection { get; internal set; }

        /// <summary>
        /// Gets the index of the current selection suggestion item.
        /// </summary>
        public int SelectionIndex { get; internal set; }

        /// <summary>
        /// Gets or sets whether the selection should be canceled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets whether the selection has been handled externally.
        /// </summary>
        public bool Handled { get; set; }
    }

    /// <summary>
    /// Event data for the selected event, which is fired after inserting the suggestion into the target.
    /// </summary>
    public class SelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the currently-selected suggestion item.
        /// </summary>
        public SuggestionItem Selection { get; internal set; }

        /// <summary>
        /// Gets the index of the current selection suggestion item.
        /// </summary>
        public int SelectionIndex { get; internal set; }

        /// <summary>
        /// Gets the text control where the suggestion was inserted.
        /// </summary>
        public Control Control { get; internal set; }
    }

    /// <summary>
    /// Event data for the hovered event, which is fired when the mouse is over a suggestion, but has not clicked it.
    /// </summary>
    public class HoveredEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the currently-highlighted suggestion item. 
        /// </summary>
        public SuggestionItem Highlighted { get; internal set; }
    }

    #endregion Event Classes
}
