#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Specifies a range of characters. If the cpMin and cpMax members are equal, the range is empty.
    /// The range includes everything if cpMin is 0 and cpMax is –1.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRange : IComparable
    {
        /// <summary>
        /// Character position index immediately preceding the first character in the range.
        /// </summary>
        public int cpMin;

        /// <summary>
        /// Character position immediately following the last character in the range.
        /// </summary>
        public int cpMax;

        /// <summary>
        /// Specifies a range of characters. If the cpMin and cpMax members are equal, the range is empty.
        /// The range includes everything if cpMin is 0 and cpMax is –1.
        /// </summary>
        /// <param name="Min">The minimum, or start position.</param>
        /// <param name="Max">The maximum, or end position.</param>
        public CharacterRange(int Min, int Max) {
            cpMin = Min;
            cpMax = Max;
        }

        /// <summary>
        /// Compares this <see cref="CharacterRange"/> to the given <see cref="CharacterRange"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj"><see cref="CharacterRange"/> to which this instance will be compared.</param>
        /// <returns>-1 if <c>this.cpMin</c> is less than <c>obj.cpMin</c> or <c>this.cpMax</c> is less than <c>obj.cpMax</c>.
        /// 0 if equal <c>this.cpMin</c> is equal to <c>obj.cpMin</c> and <c>this.cpMax</c> is equal to <c>obj.cpMax</c>.
        /// 1 if <c>this.cpMin</c> is grater than <c>obj.cpMin</c> or <c>this.cpMax</c> is greater than <c>obj.cpMax</c>. </returns>
        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            CharacterRange r = (CharacterRange)obj;
            if (cpMin.CompareTo(r.cpMin) != 0) {
                return cpMin.CompareTo(r.cpMin);
            }
            else if (cpMax.CompareTo(r.cpMax) != 0) {
                return cpMax.CompareTo(r.cpMax);
            }
            else {
                return 0;
            }
        }
    }
}
