#region Using Directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace Generic_Components
{
    /// <summary>
    /// Class to implement a list of <see cref="SuggestionItem"/> items.
    /// Primary reason to implement as a subclass is to make use of ControlStyles and overrides to reduce flicker.
    /// </summary>
    internal class SuggestionListBox : ListBox
    {
        #region Fields

        private Theme _theme;
        private readonly ToolTip _toolTip = new ToolTip()
        {
            ShowAlways = true,
            UseFading = true,
        };
        private readonly Timer _toolTipLaunchTimer = new Timer()
        {
            Interval = 500,
        };

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="SuggestionListBox"/>
        /// </summary>
        public SuggestionListBox() : base() {
            TabStop = false;
            Dock = DockStyle.Fill;
            DrawMode = DrawMode.OwnerDrawFixed;
            IntegralHeight = false;
            BorderStyle = BorderStyle.None;
            Margin = new Padding(1);
            SelectionMode = SelectionMode.One;
            Location = Point.Empty;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            HighlightedItemIndex = -1;
            Theme = new Theme();
            _toolTipLaunchTimer.Tick += ToolTipLaunchTimer_Tick;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the highlighted item index.
        /// </summary>
        public int HighlightedItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the color theme.
        /// </summary>
        public Theme Theme {
            get {
                return _theme;
            }
            set {
                _theme = value;
                BackColor = _theme.BackColorEven;
                ForeColor = _theme.ForeColor;
            }
        }

        #endregion Properties

        #region Events & Handlers

        public event EventHandler SuggestionChosen;

        /// <summary>
        /// Handles drawing an item in the listbox and raises the <see cref="ListBox.DrawItem"/> event.
        /// </summary>
        /// <param name="e">The draw info about the item that needs to be drawn.</param>
        protected override void OnDrawItem(DrawItemEventArgs e) {
            if (Items.Count > 0) {
                var rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool isHighlighted = (e.Index == HighlightedItemIndex);
                e.DrawBackground();
                // Draw background and border of selected row 
                if (isSelected) {
                    using (var brush = new SolidBrush(_theme.SelectedBackColor)) {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                    using (var pen = new Pen(_theme.SelectedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
                // Set row background of odd rows to alternate color
                else if (e.Index % 2 != 0) {
                    using (var brush = new SolidBrush(_theme.BackColorOdd)) {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                }
                // Draw background and border of highlighted (mouseover) row  
                if (isHighlighted) {
                    using (var brush = new SolidBrush(_theme.HighlightedBackColor)) {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                    using (var pen = new Pen(_theme.HighlightedBorderColor)) {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
            }
            SuggestionItem s = (SuggestionItem)Items[e.Index];
            // Use same font as editor and center text vertically
            Point centeredPoint = new Point(e.Bounds.Left, e.Bounds.Height / 2 - e.Font.Height / 2 + e.Bounds.Y);
            TextRenderer.DrawText(e.Graphics, s.GetDisplayText(), e.Font, centeredPoint, _theme.ForeColor);
            base.OnDrawItem(e);
        }

        /// <summary>
        /// Handles repainting the listbox with reduced flicker and raises the <see cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">The paint info about the item that needs to be painted.</param>
        protected override void OnPaint(PaintEventArgs e) {
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(Theme.BackColorEven), iRegion);
            if (Items.Count > 0) {
                for (int i = 0; i < Items.Count; ++i) {
                    Rectangle irect = GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect)) {
                        if (SelectedIndex == i) {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, Font,
                                irect, i,
                                DrawItemState.Selected, Theme.ForeColor,
                                Theme.BackColorEven));
                        }
                        else {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, Font,
                                irect, i,
                                DrawItemState.Default, Theme.ForeColor,
                                Theme.BackColorEven));
                        }
                        iRegion.Complement(irect);
                    }
                }
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
            ItemHeight = Font.Height + 2;
        }

        /// <summary>
        /// Handles the selected item index changing in the listbox and raises the <see cref="ListBox.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);
            ShowSelectedInfo();
        }

        /// <summary>
        /// Handles the mouse leaving the listbox and raises the <see cref="Control.MouseLeave"/> event.
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
        /// Handles the mouse moving inside the listbox and raises the <see cref="Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse move event.</param>
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            // Update the highlighted item index
            int index = IndexFromPoint(e.Location);
            if (index != HighlightedItemIndex) {
                HighlightedItemIndex = index;
                //_toolTipLaunchTimer.Stop();
                //_toolTipLaunchTimer.Start();
                Invalidate();
            }
        }

        /// <summary>
        /// Handles mouse clicks on the listbox and raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse click.</param>
        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left) {
                // Start the timer so that the tooltip will be displayed if double-click does not occur
                _toolTipLaunchTimer.Stop();
                _toolTipLaunchTimer.Start();
            }
        }

        /// <summary>
        /// Handles double mouse clicks on the listbox and raises the <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="e">The mouse info about the mouse double click.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left) {
                _toolTip.Hide(this);
                SuggestionChosen?.Invoke(this, EventArgs.Empty);
                Debug.WriteLine((SelectedItem as SuggestionItem).GetReplacementText());
                Invalidate();
            }
        }

        // Called when the timer hits the specified interval.
        private void ToolTipLaunchTimer_Tick(object sender, EventArgs e) {
            _toolTipLaunchTimer.Stop();
            // Show the tooltip if possible
            ShowSelectedInfo();
        }

        private void ShowSelectedInfo() {
            if (SelectedIndex != -1) {
                ShowToolTip(SelectedItem as SuggestionItem);
            }
        }

        #endregion Events & Handlers

        #region Methods

        // Hide listbox UI elements as necessary
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
            if (string.IsNullOrEmpty(text)) {
                _toolTip.ToolTipTitle = null;
                _toolTip.Show(title, this, Width + 3, 0);
            }
            else {
                _toolTip.ToolTipTitle = title;
                _toolTip.Show(text, this, Width + 3, 0);
                _toolTip.Active = true;
            }
        }

        #endregion Methods
    }


    /// <summary>
    /// Class to represent the color theme of a <see cref="SuggestionListBox"/>.
    /// </summary>
    public class Theme
    {
        #region Constructors
        
        /// <summary>
        /// Construct the default theme.
        /// </summary>
        public Theme() {
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
