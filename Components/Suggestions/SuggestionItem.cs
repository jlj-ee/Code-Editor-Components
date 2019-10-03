using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to represent an item in a <see cref="Suggestions"/>.
    /// </summary>
    public class SuggestionItem
    {
        /// <summary>
        /// Gets the <see cref="Suggestions"/> the contains the suggestion.
        /// </summary>
        public Suggestions Manager { get; internal set; }

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

        /// <summary>
        /// Constructs a suggestion with the given insertion text.
        /// </summary>
        /// <param name="insertText">Text to be inserted if the suggestion is selected.</param>
        public SuggestionItem(string insertText) {
            InsertText = insertText;
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

        /// <summary>
        /// Gets the text that will be used to replace the target fragment.
        /// </summary>
        /// <returns>The text that will replace the target fragment.</returns>
        public virtual string GetReplacementText() {
            return InsertText;
        }

        /// <summary>
        /// Gets the text that will be used to display the suggestion in the list.
        /// </summary>
        /// <returns><see cref="DisplayText"/> if it is not null, otherwise <see cref="InsertText"/>.</returns>
        public virtual string GetDisplayText() {
            return DisplayText ?? InsertText;
        }

        /// <summary>
        /// Gets the string representation of the suggestion.
        /// </summary>
        /// <returns><see cref="DisplayText"/> if it is not null, otherwise <see cref="InsertText"/>.</returns>
        public override string ToString() {
            return GetDisplayText();
        }

        /// <summary>
        /// Compares the given text fragment with this suggestion.
        /// </summary>
        /// <param name="fragmentText">Text fragment to compare with the suggestion.</param>
        /// <returns><see cref="DisplayState"/> which indicates whether the suggestion will be displayed.</returns>
        //public virtual DisplayState Compare(string fragmentText) {
        //    if (InsertText.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase) && InsertText != fragmentText) {
        //        return DisplayState.Selected;
        //    }
        //    else {
        //        return DisplayState.Disabled;
        //    }
        //}
        //public virtual void OnSelected(SelectedEventArgs e) { }

        /// <summary>
        /// Overrides the paint method to draw the suggestion item.
        /// </summary>
        /// <param name="e">Paint suggestion event data.</param>
        public virtual void OnPaint(PaintSuggestionEventArgs e) {
            // Center text vertically
            Point centeredPoint = new Point(e.TextRect.Left, e.TextRect.Y + e.TextRect.Height / 2 - e.Font.Height / 2);
            TextRenderer.DrawText(e.Graphics, GetDisplayText(), e.Font, centeredPoint, e.Theme.ForeColor);
        }
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

    /// <summary>
    /// Enumeration of display states for a <see cref="SuggestionItem"/>.
    /// </summary>
    public enum DisplayState
    {
        /// <summary>
        /// Suggestion will not be displayed.
        /// </summary>
        Disabled,

        /// <summary>
        /// Suggestion will be displayed.
        /// </summary>
        Enabled,

        /// <summary>
        /// Suggestion will be displayed and selected.
        /// </summary>
        Selected
    }
}
