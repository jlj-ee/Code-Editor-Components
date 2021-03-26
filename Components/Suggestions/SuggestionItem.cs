using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to represent an item in a <see cref="Suggestions"/>.
    /// </summary>
    public class SuggestionItem
    {
        #region Fields

        private List<TextMatch> _segments;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the text for inserting when selected.
        /// </summary>
        public string InsertText { get; set; }

        /// <summary>
        /// Gets or sets the text to be displayed in the suggestion list.
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets the title of the tooltip. If null, the tooltip is disabled.
        /// </summary>
        public virtual string ToolTipTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text. <see cref="ToolTipTitle"/> must not be null to enable tooltip.
        /// </summary>
        public virtual string ToolTipText { get; set; }

        /// <summary>
        /// Gets or sets the index for the suggestion image.
        /// </summary>
        public int IconIndex { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructs a suggestion with the given insertion text.
        /// </summary>
        /// <param name="insertText">Text to be inserted if the suggestion is selected.</param>
        public SuggestionItem(string insertText) {
            InsertText = DisplayText = insertText;
        }

        /// <summary>
        /// Constructs a suggestion with the given insertion text, display text, and tooltip.
        /// </summary>
        /// <param name="insertText">Text to be inserted if the suggestion is selected.</param>
        /// <param name="displayText">Text to be displayed in the suggestion box.</param>
        /// <param name="toolTipTitle">Title text to be displayed at the start of the tooltip.</param>
        /// <param name="toolTipText">Text to be displayed in the tooltip.</param>
        public SuggestionItem(string insertText, string displayText, string toolTipTitle, string toolTipText) : this(insertText) {
            DisplayText = displayText;
            ToolTipTitle = toolTipTitle;
            ToolTipText = toolTipText;
        }

        #endregion MyRegion

        #region Events & Handlers

        /// <summary>
        /// Event that is raised when this item is selected. Override for advanced functionality.
        /// </summary>
        /// <param name="e">Selected item event data.</param>
        public virtual void OnSuggestionSelected(SelectedEventArgs e) { }

        /// <summary>
        /// Overrides the paint method to draw the suggestion item.
        /// </summary>
        /// <param name="e">Paint suggestion event data.</param>
        public virtual void OnPaint(PaintSuggestionEventArgs e) {
            // Center text vertically
            Point centeredPoint = new Point(e.TextRect.Left + 3, e.TextRect.Y + e.TextRect.Height / 2 - e.Font.Height / 2);
            // Iterate over segments and emphasize matches
            TextFormatFlags format = TextFormatFlags.NoPadding;
            using (Font boldFont = new Font(e.Font, FontStyle.Bold)) {
                foreach (TextMatch s in _segments) {
                    Font font = s.IsMatch ? boldFont : e.Font;
                    TextRenderer.DrawText(e.Graphics, s.Text, font, centeredPoint, e.Theme.ForeColor, format);
                    centeredPoint.X += TextRenderer.MeasureText(e.Graphics, s.Text, font, Size.Empty, format).Width;
                }
            }
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Gets the string representation of the suggestion.
        /// </summary>
        /// <returns><see cref="DisplayText"/> if it is not null, otherwise <see cref="InsertText"/>.</returns>
        public override string ToString() {
            return DisplayText;
        }

        /// <summary>
        /// Compares the given pattern to the suggestion item.
        /// </summary>
        /// <param name="pattern">String pattern to compare.</param>
        /// <param name="matchCase">If true, the case of pattern must match the item case for a match to be found.</param>
        /// <returns>True if a pattern match was found.</returns>
        public bool Match(string pattern, bool matchCase) {
            try {
                // Try an exact match first
                Match match = new Regex(pattern, matchCase ? RegexOptions.None : RegexOptions.IgnoreCase).Match(DisplayText);
                if (match.Success) {
                    _segments = new List<TextMatch>();
                    var end = match.Index + match.Length;
                    if (match.Index > 0) _segments.Add(new TextMatch(DisplayText.Substring(0, match.Index), false));
                    _segments.Add(new TextMatch(DisplayText.Substring(match.Index, match.Length), true));
                    if (end < DisplayText.Length) _segments.Add(new TextMatch(DisplayText.Substring(end, DisplayText.Length - end), false));
                    return true;
                }
            }
            catch (System.ArgumentException) { /* Error in regular expression. */ }
            // Fuzzy match
            return FuzzyMatch(pattern, matchCase);
        }

        /// <summary>
        /// Compares the given pattern to the suggestion item using a fuzzy technique.
        /// </summary>
        /// <param name="pattern">String pattern to compare.</param>
        /// <param name="matchCase">If true, the case of pattern must match the item case for a match to be found.</param>
        /// <returns>True if a pattern match was found.</returns>
        protected virtual bool FuzzyMatch(string pattern, bool matchCase) {
            _segments = new List<TextMatch>();
            string source = DisplayText;
            if (!matchCase) {
                pattern = pattern.ToLower();
                source = source.ToLower();
            }
            bool patternMatched = false;
            int sourceIndex = 0, patternIndex;
            for (patternIndex = 0; patternIndex < pattern.Length; patternIndex++) {
                int start = sourceIndex;
                for (; sourceIndex < source.Length; sourceIndex++) {
                    // If the characters match, add the corresponding segments
                    if (source[sourceIndex] == pattern[patternIndex]) {
                        // Only a full match if every pattern character has been matched
                        patternMatched = (patternIndex == pattern.Length - 1);
                        // If the match is at the beginning, add or modify a segment for the match
                        if (start == sourceIndex) {
                            // If no segments have been added yet, add one
                            if (_segments.Count == 0) {
                                _segments.Add(new TextMatch(DisplayText.Substring(sourceIndex, 1), true));
                            }
                            // Append the existing segment
                            else {
                                TextMatch last = _segments[_segments.Count - 1];
                                _segments[_segments.Count - 1] = new TextMatch(last.Text + DisplayText[sourceIndex], last.IsMatch);
                            }
                        }
                        // If the match is in the middle, add segments for the match and for the preceding characters
                        else {
                            _segments.Add(new TextMatch(DisplayText.Substring(start, sourceIndex - start), false));
                            _segments.Add(new TextMatch(DisplayText.Substring(sourceIndex, 1), true));
                        }
                        // Move on to the next source and pattern characters
                        sourceIndex++;
                        break;
                    }
                }
                // Stop matching when every source character has been checked
                if (sourceIndex >= source.Length) {
                    break;
                }
            }
            // If not all of the source characters were checked, add a segment for those remaining characters
            if (sourceIndex < source.Length) {
                _segments.Add(new TextMatch(DisplayText.Substring(sourceIndex, source.Length - sourceIndex), false));
            }
            return patternMatched;
        }

        #endregion Methods
    }

    /// <summary>
    /// Class to encapsulate the data needed to paint a <see cref="SuggestionItem"/>.
    /// </summary>
    public class PaintSuggestionEventArgs : PaintEventArgs
    {
        /// <summary>
        /// Gets the rectangle in which item text will be drawn.
        /// </summary>
        public Rectangle TextRect { get; internal set; }

        /// <summary>
        /// Gets the font that will be used to draw item text.
        /// </summary>
        public Font Font { get; internal set; }

        /// <summary>
        /// Gets the color theme used to color the item.
        /// </summary>
        public ListTheme Theme { get; internal set; }

        /// <summary>
        /// Constructs a new event args object based on the standard <see cref="PaintEventArgs"/>.
        /// </summary>
        /// <param name="Graphics"></param>
        /// <param name="ClipRectangle"></param>
        public PaintSuggestionEventArgs(Graphics Graphics, Rectangle ClipRectangle) : base(Graphics, ClipRectangle) {
        }
    }
}
