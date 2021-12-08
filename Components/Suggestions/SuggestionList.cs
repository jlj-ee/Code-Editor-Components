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
    public class SuggestionList : UserControl
    {
        #region Fields

        private const int FONT_HEIGHT_PADDING = 4;
        private const int TOOLTIP_PADDING = 3;
        private ListTheme _theme;
        private readonly SuggestionToolTip _toolTip = new SuggestionToolTip() {
            ShowAlways = true,
            UseFading = true,
        };
        private readonly Timer _toolTipLaunchTimer = new Timer() {
            Interval = 500,
        };
        private IList<SuggestionItem> _visibleItems;
        private ImageList _imageList;
        private int _selectedItemIndex;
        private int _itemHeight;
        private int _oldItemCount;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="SuggestionList"/>
        /// </summary>
        public SuggestionList() {
            TabStop = false;
            Dock = DockStyle.None;
            BorderStyle = BorderStyle.None;
            Margin = Padding = new Padding(1);
            Location = Point.Empty;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            HighlightedItemIndex = 0;
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
                BackColor = _theme.BorderColor;
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
                _selectedItemIndex = Math.Max(-1, Math.Min(value, VisibleItems.Count - 1));
                bool inRange = (_selectedItemIndex >= 0) && (_selectedItemIndex < VisibleItems.Count);
                if (inRange) {
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
        /// Gets the width of the longest list item.
        /// </summary>
        public int ItemWidth { get; private set; }

        /// <summary>
        /// Gets or sets the list of visible items.
        /// </summary>
        public IList<SuggestionItem> VisibleItems {
            get { return _visibleItems; }
            set {
                _visibleItems = value;
                MeasureItems();
                ConfigureScroll();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the collection of images that can be drawn with suggestion items.
        /// </summary>
        public ImageList ImageList {
            get {
                return _imageList;
            }
            set {
                _imageList = value;
                ImageWidth = (_imageList != null) ? _imageList.ImageSize.Width + 2 : 0;
            }
        }

        /// <summary>
        /// Gets the width of the displayed images.
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// Gets or sets the font used to draw the item text.
        /// </summary>
        public override Font Font {
            get { return base.Font; }
            set {
                base.Font = value;
                // Add some vertical padding
                ItemHeight = Font.Height + FONT_HEIGHT_PADDING;
            }
        }

        #endregion Properties

        #region Events & Handlers

        /// <summary>
        /// Event that will be generated when a suggestion item is selected.
        /// </summary>
        public event EventHandler ItemSelected;

        /// <summary>
        /// Handles repainting the list background.
        /// </summary>
        /// <param name="e">Paint event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            base.OnPaintBackground(e);
            e.Graphics.Clear(Theme.BackColor);
        }

        /// <summary>
        /// Handles repainting the list with reduced flicker and raises the <see cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">Paint event data.</param>
        protected override void OnPaint(PaintEventArgs e) {
            ConfigureScroll();
            int firstVisibleIndex = Math.Max(VerticalScroll.Value / ItemHeight, 0);
            int lastVisibleIndex = Math.Min((VerticalScroll.Value + ClientSize.Height) / ItemHeight + 1, VisibleItems.Count);
            int y;

            for (int i = firstVisibleIndex; i < lastVisibleIndex; i++) {
                y = i * ItemHeight - VerticalScroll.Value + 1;
                var itemBody = new Rectangle(ImageWidth + 1, y, ClientSize.Width - 1 - ImageWidth, ItemHeight);
                var itemBorder = new Rectangle(itemBody.X, itemBody.Y - 1, itemBody.Width - 1, itemBody.Height);
                // Set row background of odd rows to alternate color
                if (i % 2 != 0) {
                    using (var brush = new SolidBrush(_theme.BackColorOdd)) {
                        e.Graphics.FillRectangle(brush, itemBody);
                    }
                }
                // Draw background and border of selected row 
                if (i == SelectedItemIndex) {
                    using (var brush = new SolidBrush(_theme.SelectedBackColor)) {
                        e.Graphics.FillRectangle(brush, itemBody);
                    }
                    using (var pen = new Pen(_theme.HighlightedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, itemBorder);
                    }
                }
                // Draw background and border of highlighted (mouseover) row  
                if (i == HighlightedItemIndex) {
                    using (var brush = new SolidBrush(_theme.HighlightedBackColor)) {
                        e.Graphics.FillRectangle(brush, itemBody);
                    }
                    using (var pen = new Pen(_theme.HighlightedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, itemBorder);
                    }
                }
                // Draw image if available
                if ((ImageList != null) && (VisibleItems[i].IconIndex >= 0)) {
                    Point centeredPoint = new Point(1, itemBody.Y + itemBody.Height / 2 - ImageList.ImageSize.Height / 2);
                    e.Graphics.DrawImage(ImageList.Images[VisibleItems[i].IconIndex], centeredPoint);
                }
                // Draw the text last so it is on top of everything
                VisibleItems[i].OnPaint(new PaintSuggestionEventArgs(e.Graphics, e.ClipRectangle) {
                    Font = Font,
                    TextRect = itemBody,
                    Theme = _theme,
                }
                );
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Handles the font changing and raises the <see cref="Control.FontChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            MeasureItems();
        }

        /// <summary>
        /// Handles the list scrolling and raises the <see cref="ScrollableControl.Scroll"/> event.
        /// </summary>
        /// <param name="se">Scroll event data.</param>
        protected override void OnScroll(ScrollEventArgs se) {
            base.OnScroll(se);
            Invalidate();
        }

        /// <summary>
        /// Handles the mouse leaving the list and raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
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
        /// <param name="e">Mouse event data.</param>
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            // Update the highlighted item index
            HighlightedItemIndex = GetIndexFromPoint(e.Location);
            Invalidate();
        }

        /// <summary>
        /// Handles mouse clicks on the list and raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">Mouse event data.</param>
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
        /// <param name="e">Mouse event data.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left) {
                _toolTip.Hide(Parent);
                OnItemSelected(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Handles the mousewheel event. Note: Suppresses the <see cref="Control.MouseWheel"/> event to avoid system handling.
        /// </summary>
        /// <param name="e">Mouse event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            // If the mouse wheel delta is positive, move up one item.
            if (e.Delta > 0) {
                VerticalScroll.Value = Math.Max(VerticalScroll.Value - ItemHeight, 0);
            }
            // If the mouse wheel delta is negative, move down one item.
            if (e.Delta < 0) {
                VerticalScroll.Value = Math.Min(VerticalScroll.Value + ItemHeight, VerticalScroll.Maximum);
            }
            PerformLayout();
            OnMouseMove(e);
            // Don't raise base event:
            /* base.OnMouseWheel(e); */
        }

        /// <summary>
        /// Handles key presses. Using this event over standard KeyDown/KeyPress because UserControl has trouble keeping focus.
        /// </summary>
        /// <param name="msg"><see cref="Message"/> by reference to process.</param>
        /// <param name="keyData"><see cref="Keys"/> value to process.</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (Parent is SuggestionDropDown host) {
                if (host.Manager.ProcessKey(keyData)) {
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnItemSelected(EventArgs e) {
            ItemSelected?.Invoke(this, e);
        }

        // Display the tooltip when the timer hits the specified interval.
        private void ToolTipLaunchTimer_Tick(object sender, EventArgs e) {
            _toolTipLaunchTimer.Stop();
            if ((SelectedItemIndex >= 0) && Visible) {
                ShowToolTip(VisibleItems[SelectedItemIndex]);
            }
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Shows a tooltip for the given item.
        /// </summary>
        /// <param name="item"><see cref="SuggestionItem"/> for which the tooltip will be displayed.</param>
        public void ShowToolTip(SuggestionItem item) {
            string title = item.ToolTipTitle;
            string text = item.ToolTipText;

            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(text)) {
                _toolTip.Hide(Parent);
                _toolTip.RemoveAll();
                return;
            }
            else {
                bool hasText = !string.IsNullOrEmpty(text);
                _toolTip.ToolTipTitle = hasText ? title : null;
                _toolTip.ToolTipText = hasText ? text : title;
                Point location = new Point(Width + TOOLTIP_PADDING, GetPointFromIndex(SelectedItemIndex).Y);
                // Switch sides if the tooltip will cross the screen boundary
                if (Parent.PointToScreen(location).X + _toolTip.Size.Width > Screen.FromControl(this).Bounds.Right) {
                    location.Offset(-(Width + _toolTip.Size.Width + 2 * TOOLTIP_PADDING), 0);
                }
                // Make sure to connect the tooltip to the parent or it will not show unless the list has focus
                _toolTip.Show(hasText ? text : title, Parent, location);
            }
        }

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="SuggestionList"/> instance.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                _toolTip.Dispose();
                _toolTipLaunchTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Hide list UI elements.
        /// </summary>
        internal void Close() {            
            _toolTip.Hide(Parent);
        }

        /// <summary>
        /// Configures the vertical scroll bar based on the item height and visible items.
        /// </summary>
        private void ConfigureScroll() {
            if ((VisibleItems == null) || (_oldItemCount == VisibleItems.Count)) {
                return;
            }
            _oldItemCount = VisibleItems.Count;
            VerticalScroll.SmallChange = ItemHeight;
            VerticalScroll.LargeChange = ItemHeight * 3;
            var height = VisibleItems.Count > 1 ? ItemHeight * VisibleItems.Count + Location.Y - 1 : 0;
            AutoScrollMinSize = new Size(0, height);
            PerformLayout();
        }

        /// <summary>
        /// Scrolls the list to the selected item.
        /// </summary>
        private void EnsureVisible() {
            HighlightedItemIndex = -1;
            int y = (SelectedItemIndex * ItemHeight) - VerticalScroll.Value;
            if (y < 0) {
                VerticalScroll.Value = SelectedItemIndex * ItemHeight;
            }
            else if (y > (ClientSize.Height - ItemHeight)) {
                VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, (SelectedItemIndex * ItemHeight) - ClientSize.Height + ItemHeight + 1);
            }
            PerformLayout();
        }

        /// <summary>
        /// Gets the list index of the specified point.
        /// </summary>
        /// <param name="point"><see cref="Point"/> for which to find the list index.</param>
        /// <returns>Integer representing the list index that corresponds to the given point.</returns>
        public int GetIndexFromPoint(Point point) {
            return (point.Y + VerticalScroll.Value) / ItemHeight;
        }

        /// <summary>
        /// Returns the location of the the list item at the given index relative to the list origin.
        /// </summary>
        /// <param name="index">Item index for which the location will be returned.</param>
        /// <returns><see cref="Point"/> representing the location of the item at the given index.</returns>
        public Point GetPointFromIndex(int index) {
            return new Point(0, index * ItemHeight - VerticalScroll.Value);
        }

        /// <summary>
        /// Starts the timer so that the tooltip will be displayed if double-click does not occur within the timer interval.
        /// </summary>
        private void StartToolTipCountdown(int interval = 100) {
            _toolTipLaunchTimer.Stop();
            _toolTipLaunchTimer.Interval = interval;
            _toolTipLaunchTimer.Start();
        }

        // Sets the width of the list by measuring each item and determining the maximum width.
        private void MeasureItems() {
            int textLength, maxTextLength = 0;
            if (_visibleItems != null) {
                foreach (var item in _visibleItems) {
                    textLength = item.DisplayText.Length;
                    if (textLength > maxTextLength) {
                        maxTextLength = textLength;
                        ItemWidth = TextRenderer.MeasureText(item.DisplayText, Font).Width;
                    }
                }
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
            BackColor = Color.White;
            BackColorOdd = Color.LightGray;
            SelectedBackColor = Color.LightSkyBlue;
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
        /// Image background color.
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Background color for even-numbered rows.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Background color for odd-numbered rows.
        /// </summary>
        public Color BackColorOdd { get; set; }

        /// <summary>
        /// Selected item background color.
        /// </summary>
        public Color SelectedBackColor { get; set; }


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
