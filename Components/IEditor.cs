#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Interface that represents a wrapper for a text editor control.
    /// </summary>
    public interface IEditor
    {
       #region Properties 

        /// <summary>
        /// Gets the <see cref="Control"/> targetd by this wrapper.
        /// </summary>
        Control Target { get; }

        /// <summary>
        /// Gets a boolean representing whether the targeted control is read-only.
        /// </summary>
        bool Readonly { get; }

        /// <summary>
        /// Gets the text contents of the targeted control.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the length in number of characters of the text in the targeted control.
        /// </summary>
        int TextLength { get; }

        /// <summary>
        /// Gets the font used to render text in the targeted control.
        /// </summary>
        Font Font { get; }

        /// <summary>
        /// Gets or sets the selected text.
        /// </summary>
        string SelectedText { get; set; }

        /// <summary>
        /// Gets the line number on which the caret is currently positioned.
        /// </summary>
        int CurrentLine { get; }

        /// <summary>
        /// Gets the height of the current line in pixels.
        /// </summary>
        int LineHeight { get; }

        /// <summary>
        /// Gets the number of lines of text in the targeted control.
        /// </summary>
        int LineCount { get; }

        /// <summary>
        /// Gets or sets the character index at the start of the text selection.
        /// <code>Math.Min(<see cref="AnchorPosition"/>, <see cref="CurrentPosition"/>)</code>
        /// </summary>
       int SelectionStart { get; set; }

        /// <summary>
        /// Gets or sets the character index at the end of the text selection.
        /// <code>Math.Max(<see cref="AnchorPosition"/>, <see cref="CurrentPosition"/>)</code>
        /// </summary>
        int SelectionEnd { get; set; }

        /// <summary>
        /// Gets or sets the length in number of characters of the text selection.
        /// </summary>
        int SelectionLength { get; set; }

        /// <summary>
        /// Gets or sets the current caret position.
        /// </summary>
        int CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the current anchor position.
        /// </summary>
        int AnchorPosition { get; set; }

        /// <summary>
        /// Gets or sets the current character index range in which to search.
        /// </summary>
        TextRange SearchRange { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Searches for the given text in the targeted control, using the specified pattern-matching options.
        /// </summary>
        /// <param name="text">The string for which to search.</param>
        /// <param name="matchCase">If true, a match will only occur with text that matches the case of the search string.</param>
        /// <param name="wholeWord">If true, a match will only occur if the characters before and after are not word characters.</param>
        /// <returns><see cref="TextRange"/> where the match was found. If not found, returns an empty TextRange.</returns>
        TextRange Search(string text, bool matchCase, bool wholeWord);

        /// <summary>
        /// Replaces the current text selection with the given text.
        /// </summary>
        /// <param name="text">The string that will replace the selection.</param>
        void ReplaceSelection(string text);

        /// <summary>
        /// Returns the pixel coordinates of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns><see cref="Point"/> where the character index position is loacted.</returns>
        Point GetPointFromPosition(int pos);

        /// <summary>
        /// Returns the line number of the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to be located.</param>
        /// <returns>The line number on which the character index position is located.</returns>
        int GetLineFromPosition(int pos);

        /// <summary>
        /// Gets the text in the specified range.
        /// </summary>
        /// <param name="pos">The character position at which the range starts.</param>
        /// <param name="length">The length in number of characters of the range.</param>
        /// <returns>The string in the given range.</returns>
        string GetTextRange(int pos, int length);

        /// <summary>
        /// Scrolls the targeted control to the current caret position, avoiding the given region.
        /// </summary>
        /// <param name="regionToAvoid"><see cref="Rectangle"/> in pixel coordinates that the caret should be kept out of when scrolling.</param>
        void ScrollToCaret(Rectangle regionToAvoid = new Rectangle());

        /// <summary>
        /// Highlights text in the given ranges.
        /// </summary>
        /// <param name="ranges"><see cref="List{TextRange}"/> of selections to highlight.</param>
        void Highlight(List<TextRange> ranges);

        /// <summary>
        /// Removes all highlights from the text.
        /// </summary>
        void ClearAllHighlights();

        /// <summary>
        /// Marks lines on which the given ranges are located
        /// </summary>
        /// <param name="ranges"><see cref="List{TextRange}"/> of selections to mark.</param>
        void Mark(List<TextRange> ranges);

        /// <summary>
        /// Removes all marks from the text.
        /// </summary>
        void ClearAllMarks();

        /// <summary>
        /// Moves the caret and scrolls the control to the given character index position.
        /// </summary>
        /// <param name="pos">The character index position to which to navigate.</param>
        void GoToPosition(int pos);

        /// <summary>
        /// Moves the caret and scrolls the control to the given line number.
        /// </summary>
        /// <param name="lineNumber">The line number to which to navigate.</param>
        void GoToLine(int lineNumber);

        /// <summary>
        /// Marks the beginning of a set of coalesced actions that can be reverted.
        /// </summary>
        void BeginUndoAction();

        /// <summary>
        /// Marks the end of a set of coalesced actions that can be reverted.
        /// </summary>
        void EndUndoAction();

        #endregion Methods

        #region Events & Handlers

        /// <summary>
        /// Handle when focus is lost from the targeted control.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        ///  Handle when the targeted control is scrolled.
        /// </summary>
        event ScrollEventHandler Scroll;

        /// <summary>
        /// Handle when the keyboard is pressed when the targeted control has focus.
        /// </summary>
        event KeyEventHandler KeyDown;

        /// <summary>
        /// Handle when the targeted control is clicked with the mouse.
        /// </summary>
        event MouseEventHandler MouseDown;

        #endregion Events & Handlers
    }
}
