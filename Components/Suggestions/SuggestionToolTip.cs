#region Using Directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to extend the standard tooltip and provide a custom appearance.
    /// </summary>
    public class SuggestionToolTip : ToolTip
    {
        #region Constructors

        /// <summary>
        /// Creates a new custom tooltip.
        /// </summary>
        public SuggestionToolTip() {
            OwnerDraw = true;
            Font = new Font("Segoe UI", 9f);
            Popup += new PopupEventHandler(this.OnPopup);
            Draw += new DrawToolTipEventHandler(this.OnDraw);

            const int GCL_STYLE = -26;
            const int CS_DROPSHADOW = 0x20000;

            var hwnd = (IntPtr)GetType().GetProperty("Handle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this, null);
            int cs = GetClassLong(hwnd, GCL_STYLE);
            if ((cs & CS_DROPSHADOW) == CS_DROPSHADOW) {
                cs &= ~CS_DROPSHADOW;
                SetClassLong(hwnd, GCL_STYLE, cs);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        public string ToolTipText { get; set; }

        /// <summary>
        /// Gets or sets the font for the tooltip title and text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets the size of the tooltip text.
        /// </summary>
        public Size ToolTipTextSize {
            get {
                return string.IsNullOrEmpty(ToolTipText) ? Size.Empty : TextRenderer.MeasureText(ToolTipText, Font);
            }
        }

        /// <summary>
        /// Gets the size of the tooltip title text.
        /// </summary>
        public Size ToolTipTitleSize {
            get {
                return string.IsNullOrEmpty(ToolTipTitle) ? Size.Empty : TextRenderer.MeasureText(ToolTipTitle, new Font(Font, FontStyle.Bold));
            }
        }

        /// <summary>
        /// Returns the size of the tooltip.
        /// </summary>
        public Size Size {
            get {
                return new Size(Math.Max(ToolTipTextSize.Width, ToolTipTitleSize.Width), ToolTipTextSize.Height + ToolTipTitleSize.Height);
            }
        }

        #endregion Properties

        #region Events & Handlers

        // Handles custom sizing of the tooltip based on the text and font.
        private void OnPopup(object sender, PopupEventArgs e) {
            e.ToolTipSize = Size;
        }

        // Handles custom drawing of the toolitp.
        private void OnDraw(object sender, DrawToolTipEventArgs e) {
            e.DrawBackground();
            using (var brush = new SolidBrush(Color.LightGray)) {
                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, e.Bounds.Height)));
            }
            using (var pen = new Pen(Color.Gray)) {
                e.Graphics.DrawRectangle(pen, new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width - 1, e.Bounds.Height - 1)));
            }
            var titleRectangle = new Rectangle(new Point(e.Bounds.X, e.Bounds.Y), ToolTipTitleSize);
            var textRectangle = new Rectangle(new Point(e.Bounds.X, e.Bounds.Y + ToolTipTitleSize.Height), ToolTipTextSize);
            TextRenderer.DrawText(e.Graphics, ToolTipTitle, new Font(Font, FontStyle.Bold), titleRectangle, Color.Black, TextFormatFlags.Left);
            TextRenderer.DrawText(e.Graphics, ToolTipText, Font, textRectangle, Color.Black, TextFormatFlags.Left);
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="SuggestionToolTip"/> instance.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                Font.Dispose();
            }
            base.Dispose(disposing);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetClassLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion Methods
    }
}
