#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Wrapper for a standard <see cref="TextBox"/> control.
    /// </summary>
    public class TextBoxWrapper : IEditor
    {
        #region Fields

        private readonly TextBox _textBox;
        private ScrollEventHandler _scroll;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="TextBoxWrapper"/> instance targeting the given control.
        /// </summary>
        /// <param name="target"><see cref="TextBox"/> control targeted by the wrapper.</param>
        public TextBoxWrapper(TextBox target) {
            _textBox = target;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the <see cref="Control"/> targetd by this wrapper.
        /// </summary>
        public Control Target {
            get { return _textBox; }
        }

        /// <summary>
        /// Gets a boolean representing whether the targeted control is read-only.
        /// </summary>
        public bool Readonly {
            get { return _textBox.ReadOnly; }
        }

        /// <summary>
        /// Gets the text contents of the targeted control.
        /// </summary>
        public string Text {
            get { return _textBox.Text; }
        }

        /// <summary>
        /// Gets the length in number of characters of the text in the targeted control.
        /// </summary>
        public int TextLength {
            get { return _textBox.TextLength; }
        }

        /// <summary>
        /// Gets the font used to render text in the targeted control.
        /// </summary>
        public Font Font {
            get { return _textBox.Font; }
        }

        /// <summary>
        /// Gets or sets the selected text.
        /// </summary>
        public string SelectedText {
            get { return _textBox.SelectedText; }
            set { _textBox.SelectedText = value; }
        }

        /// <summary>
        /// Gets the line number on which the caret is currently positioned.
        /// </summary>
        public int CurrentLine {
            get { return _textBox.GetLineFromCharIndex(CurrentPosition); }
        }

        /// <summary>
        /// Gets the height of the current line in pixels.
        /// </summary>
        public int LineHeight {
            get { return _textBox.Font.Height; }
        }

        /// <summary>
        /// Gets the number of lines of text in the targeted control.
        /// </summary>
        public int LineCount {
            get { return _textBox.Lines.Length; }
        }

        /// <summary>
        /// Gets or sets the character index at the start of the text selection.
        /// </summary>
        public int SelectionStart {
            get { return _textBox.SelectionStart; }
            set { _textBox.SelectionStart = value; }
        }

        /// <summary>
        /// Gets or sets the character index at the end of the text selection.
        /// </summary>
        public int SelectionEnd {
            get { return _textBox.SelectionStart + _textBox.SelectionLength; }
            set { _textBox.SelectionLength = _textBox.SelectionStart + value; }
        }

        /// <summary>
        /// Gets or sets the length in number of characters of the text selection.
        /// </summary>
        public int SelectionLength {
            get { return _textBox.SelectionLength; }
            set { _textBox.SelectionLength = value; }
        }

        /// <summary>
        /// Gets or sets the current caret position.
        /// <code>Math.Max(<see cref="SelectionStart"/>, <see cref="SelectionEnd"/>)</code>
        /// </summary>
        public int CurrentPosition {
            get { return Math.Max(SelectionStart, SelectionEnd); }
            set {
                SelectionLength = 0;
                SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the current anchor position.
        /// <code>Math.Min(<see cref="SelectionStart"/>, <see cref="SelectionEnd"/>)</code>
        /// </summary>
        public int AnchorPosition {
            get { return Math.Min(SelectionStart, SelectionEnd); }
            set { SelectionStart = value; }
        }

        public TextRange SearchRange {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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
            string testValue = wholeWord ? "\\b" + text + "\\b" : text;
            var regex = new Regex(Regex.Escape(testValue), matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
            var match = regex.Match(_textBox.Text);

            return match.Captures.Count == 0 ? -1 : match.Groups[0].Index;
        }

        /// <summary>
        /// Replaces the current text selection with the given text.
        /// </summary>
        /// <param name="text">The string that will replace the selection.</param>
        public void ReplaceSelection(string text) {
            SelectedText = text;
        }

        /// <summary>
        /// Returns the pixel coordinates of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns><see cref="Point"/> where the character index position is loacted.</returns>
        public Point GetPointFromPosition(int pos) {
            return _textBox.GetPositionFromCharIndex(pos);
        }

        /// <summary>
        /// Returns the line number of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns>The line number on which the character index position is located.</returns>
        public int GetLineFromPosition(int pos) {
            return _textBox.GetLineFromCharIndex(pos);
        }

        /// <summary>
        /// Gets the text in the specified range.
        /// </summary>
        /// <param name="pos">The character position at which the range starts.</param>
        /// <param name="length">The length in number of characters of the range.</param>
        /// <returns>The string in the given range.</returns>
        public string GetTextRange(int pos, int length) {
            return _textBox.Text.Substring(pos, length);
        }

        /// <summary>
        /// Scrolls the targeted control to the current caret position, avoiding the given region.
        /// </summary>
        /// <param name="regionToAvoid"><see cref="Rectangle"/> in pixel coordinates that the caret should be kept out of when scrolling.</param>
        public void ScrollToCaret(Rectangle regionToAvoid = default) {
            _textBox.ScrollToCaret();
        }

        public void Highlight(List<TextRange> ranges) {
            throw new NotImplementedException();
        }

        public void ClearAllHighlights() {
            throw new NotImplementedException();
        }

        public void Mark(List<TextRange> ranges) {
            throw new NotImplementedException();
        }

        public void ClearAllMarks() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the caret and scrolls the control to the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to which to navigate.</param>
        public void GoToPosition(int pos) {
            CurrentPosition = pos;
            ScrollToCaret();
        }

        /// <summary>
        /// Moves the caret and scrolls the control to the given line number.
        /// </summary>
        /// <param name="lineNumber">The line number to which to navigate.</param>
        public void GoToLine(int lineNumber) {
            CurrentPosition = _textBox.GetFirstCharIndexFromLine(lineNumber);
            ScrollToCaret();
        }

        /// <summary>
        /// Marks the beginning of a set of coalesced actions that can be reverted.
        /// </summary>
        public void BeginUndoAction() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks the end of a set of coalesced actions that can be reverted.
        /// </summary>
        public void EndUndoAction() {
            throw new NotImplementedException();
        }

        #endregion Methods

        #region Events & Handlers

        /// <summary>
        /// Handle when focus is lost from the targeted control.
        /// </summary>
        public event EventHandler LostFocus {
            add { _textBox.LostFocus += value; }
            remove { _textBox.LostFocus -= value; }
        }

        /// <summary>
        ///  Handle when the targeted control is scrolled.
        /// </summary>
        public event ScrollEventHandler Scroll {
            add { _scroll += value; }
            remove { _scroll -= value; }
        }

        /// <summary>
        /// Handle when the keyboard is pressed when the targeted control has focus.
        /// </summary>
        public event KeyEventHandler KeyDown {
            add { _textBox.KeyDown += value; }
            remove { _textBox.KeyDown -= value; }
        }

        /// <summary>
        /// Handle when the targeted control is clicked with the mouse.
        /// </summary>
        public event MouseEventHandler MouseDown {
            add { _textBox.MouseDown += value; }
            remove { _textBox.MouseDown -= value; }
        }

        #endregion Events & Handlers
    }
}
