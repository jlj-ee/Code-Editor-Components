using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class that implements a <see cref="TextBox"/> extension to show a user-configurable text cue when text has not been entered.
    /// </summary>
    public class CueTextBox : TextBox
    {
        #region Fields

        private const string DEFAULT_CUE = "<Input>";
        private string _cueText = DEFAULT_CUE;
        private Color _cueColor;
        private Color _cueActiveColor;
        private Font _cueFont;
        private readonly Panel _cueContainer;

        private ContextMenuStrip _contextMenuStrip;
        private Button _clearButton;
        private bool _clearButtonVisible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance of a <see cref="CueTextBox"/>.
        /// </summary>
        public CueTextBox() {
            BorderStyle = BorderStyle.FixedSingle;
            _cueColor = Color.LightGray;
            _cueActiveColor = Color.Gray;
            _cueFont = Font;

            // Set up panel that will be used to draw the cue text
            _cueContainer = new Panel();
            _cueContainer.Paint += new PaintEventHandler(CueContainer_Paint);
            _cueContainer.Click += new EventHandler(CueContainer_Click);

            // Set up custom context menu
            InitializeContextMenu();
            // Set up button if necessary
            if (ClearButtonVisible) {
                AddClearButton();
            }

            // Draw the cue so that it is visible at design time
            ShowCue();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the cue text.
        /// </summary>
        [Category("Cue"), Description("Text that will be displayed when the TextBox is empty")]
        public string CueText {
            get { return _cueText; }
            set {
                _cueText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the cue text color when the TextBox has focus.
        /// </summary>
        [Category("Cue"), Description("Cue text color when the TextBox has focus")]
        public Color CueActiveColor {
            get { return _cueActiveColor; }
            set {
                _cueActiveColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the cue text color when the TextBox does not have focus.
        /// </summary>
        [Category("Cue"), Description("Cue text color when the TextBox does not have focus")]
        public Color CueColor {
            get { return _cueColor; }
            set {
                _cueColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Gets or sets the cue text font.
        /// </summary>
        [Category("Cue"), Description("Cue text font")]
        public Font CueFont {
            get { return _cueFont; }
            set {
                _cueFont = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the clear button is visible.
        /// </summary>
        [Category("Button"), Description("Button visibility")]
        public bool ClearButtonVisible {
            get { return _clearButtonVisible; }
            set {
                _clearButtonVisible = value;
                if (_clearButtonVisible) {
                    AddClearButton();
                }
                else {
                    RemoveClearButton();
                }
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
            _cueContainer.Width = Width - (dx + 2 + (_clearButtonVisible ? _clearButton.Width : 0));
            _cueContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // Draw the text
            Color textColor = ContainsFocus ? CueActiveColor : CueColor;
            using (var brush = new SolidBrush(textColor)) {
                e.Graphics.DrawString(CueText, CueFont, brush, new PointF(-1f, 1f));
            }
        }

        // Handle the button being clicked.
        private void ClearButton_Click(object sender, EventArgs e) {
            Text = string.Empty;
            Focus();
        }

        /// <summary>
        /// Handle the TextBox getting focus by repainting and raise the <see cref="Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnGotFocus(EventArgs e) {
            // Kick off SelectAll asyncronously so that it occurs after Click
            BeginInvoke((Action)delegate {
                SelectAll();
            });
            Invalidate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Handle the TextBox losing focus by repainting and raise the <see cref="Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e) {
            Invalidate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Handle TextBox text changes by showing or hiding the cue, and raise the <see cref="Control.TextChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnTextChanged(EventArgs e) {
            if (Text.Equals(string.Empty)) {
                ShowCue();
            }
            else {
                HideCue();
            }
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Handle the TextBox being repainted by showing or hiding the button, and raise the <see cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnPaint(PaintEventArgs e) {
            if (ClearButtonVisible) {
                _clearButton.Size = new Size(17, ClientSize.Height - 2);
                _clearButton.Location = new Point(ClientSize.Width - _clearButton.Width - 1, 1);

            }
            const int EM_SETMARGINS = 0xD3;
            SendMessage(Handle, EM_SETMARGINS, (IntPtr)2, (IntPtr)((ClearButtonVisible ? _clearButton.Width : 0) << 16));
            base.OnPaint(e);
        }

        /// <summary>
        /// Handle the TextBox being invalidated by repainting the cue, and raise the <see cref="Control.Invalidated"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnInvalidated(InvalidateEventArgs e) {
            if (_cueContainer != null) {
                _cueContainer.Invalidate();
            }
            base.OnInvalidated(e);
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="CueTextBox"/> instance.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                _cueContainer.Dispose();
                _contextMenuStrip.Dispose();
                if (_clearButton != null) {
                    _clearButton.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Initialize the custom context menu.
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

        // Hides the button from the TextBox
        private void RemoveClearButton() {
            if (_clearButton != null) {
                Controls.Remove(_clearButton);
                _clearButton.Click -= ClearButton_Click;
                _clearButton.Dispose();
            }
        }

        // Show the button in the TextBox
        private void AddClearButton() {
            _clearButton = new Button() {
                Padding = new Padding(0, 0, 1, 0),
                Cursor = Cursors.Default,
                Image = Properties.Resources.clear,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = {
                    BorderSize = 0,
                    //MouseOverBackColor = Color.Transparent,
                    //MouseDownBackColor = Color.Transparent,
                },
                UseVisualStyleBackColor = false,
                Visible = true,
                Anchor = AnchorStyles.Right,
            };
            _clearButton.Click += ClearButton_Click;
            Controls.Add(_clearButton);
        }

        // pinvoke for sending a message to the TextBox
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        #endregion Methods
    }
}
