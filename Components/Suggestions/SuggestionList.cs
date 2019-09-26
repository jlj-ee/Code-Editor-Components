using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to implement a list of <see cref="SuggestionItem"/> items.
    /// It would have been nice to just use a ListBox, but there was just too much UI flicker,
    /// even after sublcassing the ListBox to make use of ControlStyles.
    /// Implementing as a completely custom UserControl gives full control over painting,
    /// and results in the flicker-free UI needed.
    /// </summary>
    internal class SuggestionList : UserControl
    {
        #region Fields

        private const int FONT_HEIGHT_PADDING = 2;
        private ListTheme _theme;
        private readonly ToolTip _toolTip = new ToolTip() {
            ShowAlways = true,
            UseFading = true,
        };
        private readonly Timer _toolTipLaunchTimer = new Timer() {
            Interval = 1000,
        };
        private List<SuggestionItem> _visibleItems;
        private int _selectedItemIndex;
        private int _itemHeight;
        private int _oldItemCount;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="SuggestionList"/>
        /// </summary>
        public SuggestionList() : base() {
            TabStop = false;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            Margin = new Padding(1);
            Location = Point.Empty;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            HighlightedItemIndex = -1;
            Theme = new ListTheme();
            _toolTipLaunchTimer.Tick += ToolTipLaunchTimer_Tick;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the color theme.
        /// </summary>
        public ListTheme Theme {
            get {
                return _theme;
            }
            set {
                _theme = value;
                BackColor = _theme.BackColorEven;
                ForeColor = _theme.ForeColor;
            }
        }

        /// <summary>
        /// Gets or sets the highlighted item index.
        /// </summary>
        public int HighlightedItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the selected item index.
        /// </summary>
        public int SelectedItemIndex {
            get { return _selectedItemIndex; }
            set {
                _selectedItemIndex = Clamp(value, -1, VisibleItems.Count - 1);
                if ((_selectedItemIndex >= 0) && (_selectedItemIndex < VisibleItems.Count)) {
                    EnsureVisible();
                    StartToolTipCountdown();
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the list item height.
        /// </summary>
        public int ItemHeight {
            get { return _itemHeight; }
            set {
                _itemHeight = Math.Max(value, Font.Height + FONT_HEIGHT_PADDING);
                _oldItemCount = -1;
                ConfigureScroll();
            }
        }

        /// <summary>
        /// Gets or sets the list of visible items.
        /// </summary>
        public List<SuggestionItem> VisibleItems {
            get { return _visibleItems; }
            set {
                _visibleItems = value;
                SelectedItemIndex = -1;
                ConfigureScroll();
                Invalidate();
            }
        }

        public int IconWidth { get; set; }

        #endregion Properties

        #region Events & Handlers

        public event EventHandler SuggestionChosen;

        /// <summary>
        /// Handles repainting the list with reduced flicker and raises the <see cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">The paint info about the item that needs to be painted.</param>
        protected override void OnPaint(PaintEventArgs e) {
            ConfigureScroll();
            int firstVisibleIndex = Math.Max(VerticalScroll.Value / ItemHeight - 1, 0);
            int lastVisibleIndex = Math.Min((VerticalScroll.Value + ClientSize.Height) / ItemHeight + 1, VisibleItems.Count);
            int y;

            for (int i = firstVisibleIndex; i < lastVisibleIndex; i++) {
                y = i * ItemHeight - VerticalScroll.Value;
                var textRect = new Rectangle(IconWidth, y, ClientSize.Width - 1 - IconWidth, ItemHeight);
                var penRect = new Rectangle(textRect.X, textRect.Y, textRect.Width - 1, textRect.Height - 1);
                // Draw background and border of selected row 
                if (i == SelectedItemIndex) {
                    using (var brush = new SolidBrush(_theme.SelectedBackColor)) {
                        e.Graphics.FillRectangle(brush, textRect);
                    }
                    using (var pen = new Pen(_theme.SelectedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, penRect);
                    }
                }
                // Set row background of odd rows to alternate color
                else if (i % 2 != 0) {
                    using (var brush = new SolidBrush(_theme.BackColorOdd)) {
                        e.Graphics.FillRectangle(brush, textRect);
                    }
                }
                // Draw background and border of highlighted (mouseover) row  
                if (i == HighlightedItemIndex) {
                    using (var brush = new SolidBrush(_theme.HighlightedBackColor)) {
                        e.Graphics.FillRectangle(brush, textRect);
                    }
                    using (var pen = new Pen(_theme.HighlightedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, penRect);
                    }
                }
                // Center text vertically
                Point centeredPoint = new Point(textRect.Left, textRect.Height / 2 - Font.Height / 2 + textRect.Y);
                TextRenderer.DrawText(e.Graphics, VisibleItems[i].GetDisplayText(), Font, centeredPoint, _theme.ForeColor);
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Handles the font property changing and raises the <see cref="Control.FontChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            // Add some vertical padding
            ItemHeight = Font.Height + FONT_HEIGHT_PADDING;
        }

        /// <summary>
        /// Handles the mouse leaving the list and raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse leave event.</param>
        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            // Clear the highlighted item index
            if (HighlightedItemIndex > -1) {
                HighlightedItemIndex = -1;
                Invalidate();
            }
        }

        /// <summary>
        /// Handles the mouse moving inside the list and raises the <see cref="Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse move event.</param>
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            // Update the highlighted item index
            int index = GetIndexFromPoint(e.Location);
            if (index != HighlightedItemIndex) {
                HighlightedItemIndex = index;
                Invalidate();
            }
        }

        /// <summary>
        /// Handles mouse clicks on the list and raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse click.</param>
        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left) {
                SelectedItemIndex = GetIndexFromPoint(e.Location);
                Invalidate();
            }
        }

        /// <summary>
        /// Handles double mouse clicks on the list and raises the <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse double click.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left) {
                _toolTip.Hide(this);
                SuggestionChosen?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        // Display the tooltip when the timer hits the specified interval.
        private void ToolTipLaunchTimer_Tick(object sender, EventArgs e) {
            _toolTipLaunchTimer.Stop();
            ShowToolTip(VisibleItems[SelectedItemIndex]);
        }


        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Clamps the given value to the given range.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum acceptable value.</param>
        /// <param name="max">Maximum acceptable value.</param>
        /// <returns>The given value if it is between max and min, inclusive. Otherwise returns the limit that value exceeds.</returns>
        private int Clamp(int value, int min, int max) {
            return (value < min) ? min : (value > max) ? max : value;
        }

        /// <summary>
        /// Gets the list index of the specified point.
        /// </summary>
        /// <param name="point"><see cref="Point"/> for which to find the list index.</param>
        /// <returns>Integer representing the list index that corresponds to the given point.</returns>
        private int GetIndexFromPoint(Point point) {
            return (point.Y + VerticalScroll.Value) / ItemHeight;
        }

        /// <summary>
        /// Starts the timer so that the tooltip will be displayed if double-click does not occur within the timer interval.
        /// </summary>
        private void StartToolTipCountdown(int interval = 1000) {
            _toolTipLaunchTimer.Stop();
            _toolTipLaunchTimer.Interval = interval;
            _toolTipLaunchTimer.Start();
        }

        /// <summary>
        /// Configures the vertical scroll bar based on the item height and visible items.
        /// </summary>
        private void ConfigureScroll() {
            if ((VisibleItems != null) && (_oldItemCount != VisibleItems.Count)) {
                VerticalScroll.SmallChange = ItemHeight;
                VerticalScroll.LargeChange = ItemHeight * 3;
                int totalHeight = ItemHeight * VisibleItems.Count;
                Height = Math.Min(totalHeight, MaximumSize.Height);
                AutoScrollMinSize = new Size(0, totalHeight);
                _oldItemCount = VisibleItems.Count;
            }
        }

        /// <summary>
        /// Scrolls the list to the selected item.
        /// </summary>
        private void EnsureVisible() {
            int y = (SelectedItemIndex * ItemHeight) - VerticalScroll.Value;
            if (y < 0) {
                VerticalScroll.Value = SelectedItemIndex * ItemHeight;
            }
            else if (y > (ClientSize.Height - ItemHeight)) {
                // Selected item is at the bottom
                //VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, (SelectedItemIndex * ItemHeight) - ClientSize.Height + ItemHeight);
                // Selected item is at the top
                VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, SelectedItemIndex * ItemHeight);
            }
        }

        // Hide list UI elements as necessary
        internal void Close() {
            _toolTip.Hide(this);
        }

        /// <summary>
        /// Cleanup.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                _toolTip.Dispose();
                _toolTipLaunchTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Shows a tooltip for the given item.
        /// </summary>
        /// <param name="item"><see cref="SuggestionItem"/> for which the tooltip will be displayed.</param>
        public void ShowToolTip(SuggestionItem item) {
            string title = item.ToolTipTitle;
            string text = item.ToolTipText;

            if (string.IsNullOrEmpty(title)) {
                _toolTip.Hide(this);
                _toolTip.RemoveAll();
                return;
            }
            else {
                bool hasText = !string.IsNullOrEmpty(text);
                _toolTip.ToolTipTitle = hasText ? title : null;
                // Position will be wrong if tooltip will run off edge of screen, consider custom drawing
                _toolTip.Show(hasText ? text : title, this, Width + 3, 0);
                //_toolTip.Active = true;
            }
        }

        #endregion Methods
    }


    /// <summary>
    /// Class to represent the color theme of a <see cref="SuggestionList"/>.
    /// </summary>
    public class ListTheme
    {
        #region Constructors

        /// <summary>
        /// Construct the default theme.
        /// </summary>
        public ListTheme() {
            ForeColor = Color.Black;
            BackColorEven = Color.White;
            BackColorOdd = Color.LightGray;
            SelectedBackColor = Color.LightSkyBlue;
            SelectedBorderColor = Color.DodgerBlue;
            HighlightedBackColor = Color.Transparent;
            HighlightedBorderColor = Color.DodgerBlue;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Foreground color.
        /// </summary>
        public Color ForeColor { get; set; }

        /// <summary>
        /// Background color for even-numbered rows.
        /// </summary>
        public Color BackColorEven { get; set; }

        /// <summary>
        /// Background color for odd-numbered rows.
        /// </summary>
        public Color BackColorOdd { get; set; }

        /// <summary>
        /// Selected item background color.
        /// </summary>
        public Color SelectedBackColor { get; set; }

        /// <summary>
        /// Selected item border color.
        /// </summary>
        public Color SelectedBorderColor { get; set; }

        /// <summary>
        /// Highlighted item color.
        /// </summary>
        public Color HighlightedBackColor { get; set; }

        /// <summary>
        /// Selected item border color.
        /// </summary>
        public Color HighlightedBorderColor { get; set; }

        #endregion Properties
    }
}
