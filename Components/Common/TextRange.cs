#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Specifies a range of characters. If the cpMin and cpMax members are equal, the range is empty.
    /// The range includes everything if cpMin is 0 and cpMax is –1.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TextRange : IComparable
    {
        /// <summary>
        /// Character position index immediately preceding the first character in the range.
        /// </summary>
        public int Start;

        /// <summary>
        /// Character position immediately following the last character in the range.
        /// </summary>
        public int End;

        /// <summary>
        /// Specifies a range of characters. If the cpMin and cpMax members are equal, the range is empty.
        /// The range includes everything if cpMin is 0 and cpMax is –1.
        /// </summary>
        /// <param name="startIndex">The minimum, or start position.</param>
        /// <param name="endIndex">The maximum, or end position.</param>
        public TextRange(int startIndex, int endIndex) {
            Start = startIndex;
            End = endIndex;
        }

        /// <summary>
        /// Compares this <see cref="TextRange"/> to the given <see cref="TextRange"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj"><see cref="TextRange"/> to which this instance will be compared.</param>
        /// <returns>-1 if <c>this.cpMin</c> is less than <c>obj.cpMin</c> or <c>this.cpMax</c> is less than <c>obj.cpMax</c>.
        /// 0 if equal <c>this.cpMin</c> is equal to <c>obj.cpMin</c> and <c>this.cpMax</c> is equal to <c>obj.cpMax</c>.
        /// 1 if <c>this.cpMin</c> is grater than <c>obj.cpMin</c> or <c>this.cpMax</c> is greater than <c>obj.cpMax</c>. </returns>
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
}
