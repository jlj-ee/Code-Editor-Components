#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Component to 
    /// </summary>
    public class Suggestions : ComponentManager
    {

        #region Fields

        private ICollection<SuggestionItem> _items;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Suggestions"/> instance with associated <see cref="SuggestionDropDown"/> instance.
        /// </summary>
        /// <param name="editor">>The <see cref="IEditor"/> editor to which the <see cref="Suggestions"/> is attached.</param>
        public Suggestions(IEditor editor) {
            MaximumVisibleItems = 5;

            if (editor != null) {
                Editor = editor;
                DropDown = CreateDropDownInstance();
                DropDown.List.SuggestionChosen += ListBox_Selected;
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
                    Editor.Scroll -= Editor_Scroll;
                }
                base.Editor = value;
                Editor.LostFocus += Editor_LostFocus;
                Editor.MouseDown += Editor_MouseDown;
                Editor.KeyDown += Editor_KeyDown;
                Editor.Scroll += Editor_Scroll;
            }
        }

        /// <summary>
        /// Gets the <see cref="SuggestionDropDown"/>.
        /// </summary>
        public SuggestionDropDown DropDown { get; private set; }

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
        /// Gets or sets the maximum number of items that will be displayed in the list.
        /// </summary>
        public int MaximumVisibleItems { get; set; }

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

        #endregion Properties

        #region Events & Handlers

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

        private void Editor_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && (e.KeyCode == Keys.Space)) {
                ShowSuggestions(true);
                e.SuppressKeyPress = true;
                return;
            }
            if (DropDown.Visible) {
                if (ProcessKey(e.KeyCode)) {
                    e.SuppressKeyPress = true;
                }
            }
        }

        // Called when an item is double-clicked in the listbox.
        private void ListBox_Selected(object sender, EventArgs e) {
            Editor.Target.Focus();
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Displays the list of suggestions.
        /// </summary>
        public void ShowSuggestions(bool forceOpened) {
            if (!DropDown.Visible && (DropDown.List.VisibleItems.Count > 0)) {
                Point point = Editor.GetPointFromPosition(Editor.CurrentPosition);
                point.Offset(0, Editor.LineHeight);

                if (UseEditorFont) {
                    Font = GetEditorFont();
                }
                // Adjust size according to contents and limits
                int listWidth = Math.Min(
                    DropDown.List.ImageWidth + DropDown.List.ItemWidth + SystemInformation.VerticalScrollBarWidth + DropDown.List.Margin.Size.Width + 5,
                    DropDown.MaximumSize.Width
                    );
                int numVisibleItems = Math.Min(MaximumVisibleItems, DropDown.List.VisibleItems.Count);
                int listHeight = (numVisibleItems * DropDown.List.ItemHeight) + DropDown.List.Margin.Size.Height + DropDown.List.Location.Y;
                DropDown.Size = new Size(listWidth, listHeight + 1);
                DropDown.List.VerticalScroll.Value = 0;
                DropDown.List.SelectedItemIndex = -1;
                DropDown.Show(Editor.Target, point, ToolStripDropDownDirection.BelowRight);
            }
        }

        /// <summary>
        /// Hides the list of suggestions.
        /// </summary>
        public void HideSuggestions() {
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
                //case Keys.Enter:
                //    OnSelecting();
                //    return true;
                //case Keys.Tab:
                //    if (!AllowsTabKey)
                //        break;
                //    OnSelecting();
                //    return true;
                case Keys.Left:
                case Keys.Right:
                    HideSuggestions();
                    // Do not want to discard left/right
                    return false;
                case Keys.Escape:
                    HideSuggestions();
                    return true;
            }
            return false;
        }

        #endregion Methods
    }

    //public class SelectingEventArgs : EventArgs
    //{
    //    public Suggestion Selection { get; internal set; }
    //    public bool Cancel { get; set; }
    //    public int SelectionIndex { get; set; }
    //    public bool Handled { get; set; }
    //}

    //public class SelectedEventArgs : EventArgs
    //{
    //    public Suggestion Selection { get; internal set; }
    //    public Control Control { get; set; }
    //}

    //public class HoveredEventArgs : EventArgs
    //{
    //    public Suggestion Item { get; internal set; }
    //}


    //public class PaintItemEventArgs : PaintEventArgs
    //{
    //    public RectangleF TextRect { get; internal set; }
    //    public StringFormat Format { get; internal set; }
    //    public Font Font { get; internal set; }
    //    public bool IsSelected { get; internal set; }
    //    public bool IsHovered { get; internal set; }
    //    public Theme ThemeColors { get; internal set; }

    //    public PaintItemEventArgs(Graphics graphics, Rectangle clipRect) : base(graphics, clipRect) {
    //    }
    //}

}
