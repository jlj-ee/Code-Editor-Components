using System;
using System.Runtime.InteropServices;

namespace CodeEditor_Components
{
    /// <summary>
    /// Specifies a range of characters. If Start and End are equal, the range is empty.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TextRange : IComparable
    {
        /// <summary>
        /// Character position index immediately preceding the first character in the range.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Character position immediately following the last character in the range.
        /// </summary>
        public int End { get; }

        /// <summary>
        /// Specifies a range of characters. If the Start and End members are equal, the range is empty.
        /// </summary>
        /// <param name="startIndex">The start position.</param>
        /// <param name="endIndex">The end position.</param>
        public TextRange(int startIndex, int endIndex) {
            Start = startIndex;
            End = endIndex;
        }

        /// <summary>
        /// Compares this <see cref="TextRange"/> to the given <see cref="TextRange"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj"><see cref="TextRange"/> to which this instance will be compared.</param>
        /// <returns>-1 if <c>this.Start</c> is less than <c>obj.Start</c> or <c>this.End</c> is less than <c>obj.End</c>.
        /// 0 if equal <c>this.Start</c> is equal to <c>obj.Start</c> and <c>this.End</c> is equal to <c>obj.End</c>.
        /// 1 if <c>this.Start</c> is grater than <c>obj.Start</c> or <c>this.End</c> is greater than <c>obj.End</c>. </returns>
        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            TextRange r = (TextRange)obj;
            if (Start.CompareTo(r.Start) != 0) {
                return Start.CompareTo(r.Start);
            }
            if (End.CompareTo(r.End) != 0) {
                return End.CompareTo(r.End);
            }
            return 0;
        }
    }

    /// <summary>
    /// Specifies a result from searching for a string match.
    /// </summary>
    public struct TextMatch
    {
        /// <summary>
        /// String segment.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Whether the segment is a match.
        /// </summary>
        public bool IsMatch { get; }

        /// <summary>
        /// Specifies a result from searching for a string match.
        /// </summary>
        /// <param name="text">String segment.</param>
        /// <param name="isMatch">Whether the text is a match.</param>
        public TextMatch(string text, bool isMatch) {
            Text = text;
            IsMatch = isMatch;
        }
    }

}
