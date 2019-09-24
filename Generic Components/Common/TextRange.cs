#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives

namespace Generic_Components
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
        public int start;

        /// <summary>
        /// Character position immediately following the last character in the range.
        /// </summary>
        public int end;

        /// <summary>
        /// Specifies a range of characters. If the cpMin and cpMax members are equal, the range is empty.
        /// The range includes everything if cpMin is 0 and cpMax is –1.
        /// </summary>
        /// <param name="startIndex">The minimum, or start position.</param>
        /// <param name="endIndex">The maximum, or end position.</param>
        public TextRange(int startIndex, int endIndex) {
            start = startIndex;
            end = endIndex;
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
            if (start.CompareTo(r.start) != 0) {
                return start.CompareTo(r.start);
            }
            else if (end.CompareTo(r.end) != 0) {
                return end.CompareTo(r.end);
            }
            else {
                return 0;
            }
        }
    }
}
