#region Using Directives

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Class that implements a <see cref="TextBox"/> extension to show a user-configurable text cue when text has not been entered.
    /// </summary>
    class CueTextBox : TextBox
    {
        #region Fields

        private const string DEFAULT_CUE = "<Input>";
        private string _cueText = DEFAULT_CUE;
        private Color _cueColor;
        private Color _cueActiveColor;
        private readonly Panel _cueContainer;
        private Font _cueFont;

        private ContextMenuStrip _contextMenuStrip;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance of a <see cref="CueTextBox"/>.
        /// </summary>
        public CueTextBox() {
            _cueColor = Color.LightGray;
            _cueActiveColor = Color.Gray;
            _cueFont = Font;

            // Set up panel that will be used to draw the cue text
            _cueContainer = new Panel();
            _cueContainer.Paint += new PaintEventHandler(CueContainer_Paint);
            _cueContainer.Click += new EventHandler(CueContainer_Click);

            // Set up custom context menu
            InitializeContextMenu();

            // Draw the cue so that it is visible at design time
            ShowCue();
        }

        #endregion Constructors

        #region Properties

        [Category("Cue"), Description("Text that will be displayed when the TextBox is empty")]
        public string CueText {
            get { return _cueText; }
            set {
                _cueText = value;
                Invalidate();
            }
        }

        [Category("Cue"), Description("Cue text color when the TextBox has focus")]
        public Color CueActiveColor {
            get { return _cueActiveColor; }
            set {
                _cueActiveColor = value;
                Invalidate();
            }
        }

        [Category("Cue"), Description("Cue text color when the TextBox does not have focus")]
        public Color CueColor {
            get { return _cueColor; }
            set {
                _cueColor = value;
                Invalidate();
            }
        }

        [Category("Cue"), Description("Cue text font")]
        public Font CueFont {
            get { return _cueFont; }
            set {
                _cueFont = value;
                Invalidate();
            }
        }

        #endregion

        #region Events & Handlers

        // Enable/disable items in the custom context menu
        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e) {
            _contextMenuStrip.Items[0].Enabled = SelectionLength > 0;
            _contextMenuStrip.Items[1].Enabled = SelectionLength > 0;
            _contextMenuStrip.Items[2].Enabled = Clipboard.ContainsText();
            _contextMenuStrip.Items[3].Enabled = TextLength > 0;
        }

        // Handle copy menu item clicked
        private void CopyMenuItem_Click(object sender, EventArgs e) {
            Copy();
        }

        // Handle cut menu item clicked
        private void CutMenuItem_Click(object sender, EventArgs e) {
            Cut();
        }

        // Handle paste menu item clicked
        private void PasteMenuItem_Click(object sender, EventArgs e) {
            Paste();
        }

        // Handle select all menu item clicked
        private void SelectAllMenuItem_Click(object sender, EventArgs e) {
            SelectAll();
        }

        // Handle cue container being clicked.
        private void CueContainer_Click(object sender, EventArgs e) {
            // Pass focus to the TextBox
            Focus();
        }

        // Handle cue container being painted.
        private void CueContainer_Paint(object sender, PaintEventArgs e) {
            // Set the container position and size
            int dx = SystemInformation.BorderSize.Width + 2;
            int dy = SystemInformation.BorderSize.Height;
            _cueContainer.Location = new Point(dx, dy);
            _cueContainer.Height = Height - (dy + 2);
            _cueContainer.Width = Width - (dx + 2);
            _cueContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            // Draw the text
            Color textColor = ContainsFocus ? CueActiveColor : CueColor;
            using (var brush = new SolidBrush(textColor)) {
                e.Graphics.DrawString(_cueText, _cueFont, brush, new PointF(-2f, 0));
            }

        }

        // Handle the TextBox getting focus.
        protected override void OnEnter(EventArgs e) {
            if (Text.Equals(string.Empty)) {
                ShowCue();
            }
            Invalidate();
            base.OnEnter(e);
        }

        // Handle the TextBox losing focus.
        protected override void OnLeave(EventArgs e) {
            if (!Text.Equals(string.Empty)) {
                HideCue();
            }
            Invalidate();
            base.OnLeave(e);
        }

        // Handle the text changing.
        protected override void OnTextChanged(EventArgs e) {
            if (!Text.Equals(string.Empty)) {
                HideCue();
            }
            else {
                ShowCue();
            }
            base.OnTextChanged(e);
        }

        // Handle the TextBox being painted.
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            ShowCue();
        }

        // Handle the TextBox being forced to redraw itself.
        protected override void OnInvalidated(InvalidateEventArgs e) {
            if (_cueContainer != null) {
                _cueContainer.Invalidate();
            }
            base.OnInvalidated(e);
        }

        #endregion Events & Handlers

        #region Methods

        // Initialize the custom context menu
        private void InitializeContextMenu() {
            var menuItems = new List<ToolStripItem>();
            var cutMenuItem = new ToolStripMenuItem("Cu&t");
            cutMenuItem.Click += CutMenuItem_Click;
            menuItems.Add(cutMenuItem);

            var copyMenuItem = new ToolStripMenuItem("&Copy");
            copyMenuItem.Click += CopyMenuItem_Click; ;
            menuItems.Add(copyMenuItem);

            var pasteMenuItem = new ToolStripMenuItem("&Paste");
            pasteMenuItem.Click += PasteMenuItem_Click;
            menuItems.Add(pasteMenuItem);

            var selectAllMenuItem = new ToolStripMenuItem("Select &All");
            selectAllMenuItem.Click += SelectAllMenuItem_Click;
            menuItems.Add(selectAllMenuItem);

            _contextMenuStrip = new ContextMenuStrip();
            _contextMenuStrip.Items.AddRange(menuItems.ToArray());
            ContextMenuStrip = _contextMenuStrip;
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        // Hides the cue text from the TextBox
        private void HideCue() {
            Controls.Remove(_cueContainer);
        }

        // Shows the cue text if there is nothing in the TextBox.
        private void ShowCue() {
            Controls.Add(_cueContainer);
        }

        #endregion Methods
    }
}
