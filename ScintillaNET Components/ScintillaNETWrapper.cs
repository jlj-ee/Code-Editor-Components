#region Using Directives

using System;
using ScintillaNET;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using CodeEditor_Components;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Wrapper for <see cref="Scintilla"/> text editor control.
    /// </summary>
    public class ScintillaNETWrapper : IEditor
    {
        #region Fields

        private const int DEFAULT_FOUND_MARKER_INDEX = 10;
        private const int DEFAULT_FOUND_INDICATOR_INDEX = 16;

        private readonly Scintilla _scintilla;
        private ScrollEventHandler _scroll;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ScintillaNETWrapper"/> instance targeting the given control.
        /// </summary>
        /// <param name="target"><see cref="Scintilla"/> control targeted by the wrapper.</param>
        public ScintillaNETWrapper(Scintilla target) {
            _scintilla = target;

            FoundMarker = new Marker(_scintilla, DEFAULT_FOUND_MARKER_INDEX) {
                Symbol = MarkerSymbol.SmallRect,
            };
            FoundMarker.SetForeColor(Color.DarkOrange);
            FoundMarker.SetBackColor(Color.DarkOrange);

            FoundIndicator = new Indicator(_scintilla, DEFAULT_FOUND_INDICATOR_INDEX) {
                ForeColor = Color.DarkOrange,
                Alpha = 100,
                Style = IndicatorStyle.RoundBox,
                Under = true
            };

            _scintilla.UpdateUI += Editor_RaiseScroll;
        }

        #endregion Constructors

        #region Properties 

        /// <summary>
        /// Gets or sets the <see cref="Indicator"/> used to mark found results in the document.
        /// </summary>
        public Indicator FoundIndicator { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Marker"/> used to mark found results in the margin.
        /// </summary>
        public Marker FoundMarker { get; set; }

        /// <summary>
        /// Gets the <see cref="Control"/> targetd by this wrapper.
        /// </summary>
        public Control Target {
            get { return _scintilla; }
        }

        /// <summary>
        /// Gets a boolean representing whether the targeted control is read-only.
        /// </summary>
        public bool Readonly {
            get { return _scintilla.ReadOnly; }
        }

        /// <summary>
        /// Gets the text contents of the targeted control.
        /// </summary>
        public string Text {
            get { return _scintilla.Text; }
        }

        /// <summary>
        /// Gets the length in number of characters of the text in the targeted control.
        /// </summary>
        public int TextLength {
            get { return _scintilla.TextLength; }
        }

        /// <summary>
        /// Gets the font used to render text in the targeted control.
        /// </summary>
        public Font Font {
            get {
                // Copy font from Scintilla
                Style editorStyle = _scintilla.Styles[Style.Default];
                return new Font(editorStyle.Font, editorStyle.Size + _scintilla.Zoom - 1, GraphicsUnit.Point); // Font size seems to be off by one...why?
            }
        }

        /// <summary>
        /// Gets or sets the selected text.
        /// </summary>
        public string SelectedText {
            get { return _scintilla.SelectedText; }
            set {
                int start = _scintilla.SelectionStart;
                _scintilla.ReplaceSelection(value);
                _scintilla.SelectionStart = (start + value.Length);
                _scintilla.SelectionEnd = (start + value.Length);
            }
        }

        /// <summary>
        /// Gets the line number on which the caret is currently positioned.
        /// </summary>
        public int CurrentLine {
            get { return _scintilla.CurrentLine; }
        }

        /// <summary>
        /// Gets the height of the current line in pixels.
        /// </summary>
        public int LineHeight {
            get {
                //int SCI_TEXTHEIGHT = 2279;
                //return Editor.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
                return _scintilla.Lines[CurrentLine].Height;
            }
        }

        /// <summary>
        /// Gets the number of lines of text in the targeted control.
        /// </summary>
        public int LineCount {
            get {
                return _scintilla.Lines.Count;
            }
        }

        /// <summary>
        /// Gets or sets the character index at the start of the text selection.
        /// <code>Math.Min(<see cref="AnchorPosition"/>, <see cref="CurrentPosition"/>)</code>
        /// </summary>
        public int SelectionStart {
            get { return _scintilla.SelectionStart; }
            set { _scintilla.SelectionStart = value; }
        }

        /// <summary>
        /// Gets or sets the character index at the end of the text selection.
        /// <code>Math.Max(<see cref="AnchorPosition"/>, <see cref="CurrentPosition"/>)</code>
        /// </summary>
        public int SelectionEnd {
            get { return _scintilla.SelectionEnd; }
            set { _scintilla.SelectionEnd = value; }
        }

        /// <summary>
        /// Gets or sets the length in number of characters of the text selection.
        /// </summary>
        public int SelectionLength {
            get { return _scintilla.SelectionEnd - _scintilla.SelectionStart; }
            set { _scintilla.SelectionEnd = (_scintilla.SelectionStart + value); }
        }

        /// <summary>
        /// Gets or sets the current caret position.
        /// </summary>
        public int CurrentPosition {
            get { return _scintilla.CurrentPosition; }
            set { _scintilla.CurrentPosition = value; }
        }

        /// <summary>
        /// Gets or sets the current anchor position.
        /// </summary>
        public int AnchorPosition {
            get { return _scintilla.AnchorPosition; }
            set { _scintilla.AnchorPosition = value; }
        }

        /// <summary>
        /// Gets or sets the current character index range in which to search.
        /// </summary>
        public TextRange SearchRange {
            get { return new TextRange(_scintilla.TargetStart, _scintilla.TargetEnd); }
            set {
                _scintilla.TargetStart = value.start;
                _scintilla.TargetEnd = value.end;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Searches for the given text in the targeted control, using the specified pattern-matching options.
        /// </summary>
        /// <param name="text">The string for which to search.</param>
        /// <param name="matchCase">If true, a match will only occur with text that matches the case of the search string.</param>
        /// <param name="wholeWord">If true, a match will only occur if the characters before and after are not word characters.</param>
        /// <returns>Character index where the match was found. If not found, returns -1.</returns>
        public int Search(string text, bool matchCase, bool wholeWord) {
            if (matchCase) { _scintilla.SearchFlags |= SearchFlags.MatchCase; }
            else { _scintilla.SearchFlags &= ~SearchFlags.MatchCase; }

            if (wholeWord) { _scintilla.SearchFlags |= SearchFlags.WholeWord; }
            else { _scintilla.SearchFlags &= ~SearchFlags.WholeWord; }

            return _scintilla.SearchInTarget(text);
        }

        /// <summary>
        /// Replaces the current text selection with the given text.
        /// </summary>
        /// <param name="text">The string that will replace the selection.</param>
        public void ReplaceSelection(string text) {
            _scintilla.ReplaceSelection(text);
        }

        /// <summary>
        /// Returns the pixel coordinates of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns><see cref="Point"/> where the character index position is loacted.</returns>
        public Point GetPointFromPosition(int pos) {
            return new Point(_scintilla.PointXFromPosition(pos), _scintilla.PointYFromPosition(pos));
        }

        /// <summary>
        /// Returns the line number of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns>The line number on which the character index position is located.</returns>
        public int GetLineFromPosition(int pos) {
            return _scintilla.LineFromPosition(pos);
        }

        /// <summary>
        /// Gets the text in the specified range.
        /// </summary>
        /// <param name="pos">The character position at which the range starts.</param>
        /// <param name="length">The length in number of characters of the range.</param>
        /// <returns>The string in the given range.</returns>
        public string GetTextRange(int pos, int length) {
            return _scintilla.GetTextRange(pos, length);
        }

        /// <summary>
        /// Scrolls the targeted control to the current caret position, avoiding the given region.
        /// </summary>
        /// <param name="regionToAvoid"><see cref="Rectangle"/> in pixel coordinates that the caret should be kept out of when scrolling.</param>
        public void ScrollToCaret(Rectangle regionToAvoid) {
            if ((regionToAvoid == null) || regionToAvoid.IsEmpty) {
                _scintilla.ScrollCaret();
            }
            else {
                int lineHeight = LineHeight;
                // Calculate how many lines the rectangle could block, rounding up to the nearest whole line
                int linesToAdd = regionToAvoid.Height / lineHeight + ((regionToAvoid.Height % lineHeight > 0) ? 1 : 0);
                // No supported API for setting caret policy from ScintillaNET, so use low-level messaging
                int SCI_SETYCARETPOLICY = 2403;
                // CARET_SLOP = linesToAdd, CARET_STRICT, CARET_EVEN
                _scintilla.DirectMessage(SCI_SETYCARETPOLICY, new IntPtr(0xD), new IntPtr(linesToAdd));

                if (GetPointFromPosition(CurrentPosition).Y + lineHeight > regionToAvoid.Y) {
                    _scintilla.ScrollCaret();
                }
            }
        }

        /// <summary>
        /// Highlights text in the given ranges.
        /// </summary>
        /// <param name="ranges"><see cref="List{TextRange}"/> of selections to highlight.</param>
        public void Highlight(List<TextRange> ranges) {
            _scintilla.IndicatorCurrent = FoundIndicator.Index;

            foreach (var r in ranges) {
                _scintilla.IndicatorFillRange(r.start, r.end - r.start);
            }
        }

        /// <summary>
        /// Removes all highlights from the text.
        /// </summary>
        public void ClearAllHighlights() {
            int currentIndicator = _scintilla.IndicatorCurrent;
            _scintilla.IndicatorCurrent = FoundIndicator.Index;
            _scintilla.IndicatorClearRange(0, _scintilla.TextLength);
            _scintilla.IndicatorCurrent = currentIndicator;
        }

        /// <summary>
        /// Marks lines on which the given ranges are located
        /// </summary>
        /// <param name="ranges"><see cref="List{TextRange}"/> of selections to mark.</param>
        public void Mark(List<TextRange> ranges) {
            var lastLine = -1;
            foreach (var r in ranges) {
                Line line = new Line(_scintilla, GetLineFromPosition(r.start));
                if (line.Position > lastLine) {
                    line.MarkerAdd(FoundMarker.Index);
                }
                lastLine = line.Position;
            }
        }

        /// <summary>
        /// Removes all marks from the text.
        /// </summary>
        public void ClearAllMarks() {
            _scintilla.MarkerDeleteAll(FoundMarker.Index);
            _scintilla.SearchInTarget("Abc");
        }

        /// <summary>
        /// Moves the caret and scrolls the control to the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to which to navigate.</param>
        public void GoToPosition(int pos) {
            _scintilla.GotoPosition(pos);
        }

        /// <summary>
        /// Moves the caret and scrolls the control to the given line number.
        /// </summary>
        /// <param name="lineNumber">The line number to which to navigate.</param>
        public void GoToLine(int lineNumber) {
            _scintilla.Lines[lineNumber].Goto();
        }

        /// <summary>
        /// Marks the beginning of a set of coalesced actions that can be reverted.
        /// </summary>
        public void BeginUndoAction() {
            _scintilla.BeginUndoAction();
        }

        /// <summary>
        /// Marks the end of a set of coalesced actions that can be reverted.
        /// </summary>
        public void EndUndoAction() {
            _scintilla.EndUndoAction();
        }

        #endregion Methods

        #region Events & Handlers
        
        // Check the cause of the UpdateUI event and raise a scroll event if necessary
        private void Editor_RaiseScroll(object sender, UpdateUIEventArgs e) {
            if (e.Change == UpdateChange.VScroll) {
                _scroll?.Invoke(_scintilla, new ScrollEventArgs(ScrollEventType.EndScroll, _scintilla.FirstVisibleLine, ScrollOrientation.VerticalScroll));
            }
            else if (e.Change == UpdateChange.HScroll) {
                _scroll?.Invoke(_scintilla, new ScrollEventArgs(ScrollEventType.EndScroll, _scintilla.XOffset, ScrollOrientation.HorizontalScroll));
            }
        }

        /// <summary>
        /// Handle when focus is lost from the targetd control.
        /// </summary>
        public virtual event EventHandler LostFocus {
            add { _scintilla.LostFocus += value; }
            remove { _scintilla.LostFocus -= value; }
        }

        /// <summary>
        ///  Handle when the targeted control is scrolled.
        /// </summary>
        public virtual event ScrollEventHandler Scroll {
            add { _scroll += value; }
            remove { _scroll -= value; }
        }

        /// <summary>
        /// Handle when the keyboard is pressed when the targeted control has focus.
        /// </summary>
        public virtual event KeyEventHandler KeyDown {
            add { _scintilla.KeyDown += value; }
            remove { _scintilla.KeyDown -= value; }
        }

        /// <summary>
        /// Handle when the targeted control is clicked with the mouse.
        /// </summary>
        public virtual event MouseEventHandler MouseDown {
            add { _scintilla.MouseDown += value; }
            remove { _scintilla.MouseDown -= value; }
        }

        #endregion Events & Handlers
    }
}
