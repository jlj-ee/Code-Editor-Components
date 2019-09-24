#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace Generic_Components
{
    public interface IEditor
    {
        Control Target { get; }
        bool Readonly { get; }
        string Text { get; }
        int TextLength { get; }
        Font Font { get; }
        int CurrentLine { get; }
        int LineHeight { get; }
        int LineCount { get; }
        string SelectedText { get; set; }
        int SelectionLength { get; set; }
        int SelectionStart { get; set; }
        int SelectionEnd { get; set; }
        int CurrentPosition { get; set; }
        int AnchorPosition { get; set; }
        TextRange SearchRange { get; set; }

        int Search(string text, bool matchCase, bool wholeWord);
        void ReplaceSelection(string text);
        void ScrollToCaret(Rectangle regionToAvoid = new Rectangle());
        Point GetPointFromPosition(int pos);
        void GoToPosition(int pos);
        int GetLineFromPosition(int pos);
        void GoToLine(int lineNum);
        string GetTextRange(int pos, int length);
        void Highlight(List<TextRange> ranges);
        void ClearAllHighlights();
        void Mark(List<TextRange> ranges);
        void ClearAllMarks();
        void BeginUndoAction();
        void EndUndoAction();

        event EventHandler LostFocus;
        event ScrollEventHandler Scroll;
        event KeyEventHandler KeyDown;
        event MouseEventHandler MouseDown;
    }
}
