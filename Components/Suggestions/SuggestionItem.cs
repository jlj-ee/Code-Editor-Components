using System;
using System.Drawing;

namespace ScintillaNET_Components
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
        /// Gets or sets the index for the suggestion icon.
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

        //public virtual void OnPaint(PaintItemEventArgs e) {
        //    using (var brush = new SolidBrush(e.IsSelected ? e.ThemeColors.SelectedForeColor : e.ThemeColors.ForeColor)) {
        //        e.Graphics.DrawString(GetDisplayText(), e.Font, brush, e.TextRect, e.Format);
        //    }
        //}
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
