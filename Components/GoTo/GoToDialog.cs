#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Class to define the logic for a GoTo <see cref="Dialog"/>.
    /// </summary>
    public partial class GoToDialog : Dialog
    {
        #region Fields

        private int _currentLineNumber;
        private int _maximumLineNumber;
        private int _goToLineNumber;
        private GoTo _manager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="GoToDialog"/>.
        /// </summary>
        public GoToDialog(GoTo manager) : base() {
            InitializeComponent();

            Manager = _manager = manager;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the current line number in the <see cref="GoToDialog"/>.
        /// </summary>
        public int CurrentLineNumber {
            get {
                return _currentLineNumber;
            }
            set {
                _currentLineNumber = value;
                txtCurrentLine.Text = (_currentLineNumber + 1).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the maximum line number in the <see cref="GoToDialog"/>.
        /// </summary>
        public int MaximumLineNumber {
            get {
                return _maximumLineNumber;
            }
            set {
                _maximumLineNumber = value;
                txtMaxLine.Text = _maximumLineNumber.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the target line number in the <see cref="GoToDialog"/>.
        /// </summary>
        public int GoToLineNumber {
            get {
                return _goToLineNumber;
            }
            set {
                _goToLineNumber = value;
                txtGotoLine.Text = (_goToLineNumber + 1).ToString();
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handle activation of the <see cref="GoToDialog"/> and raise the <see cref="Form.Activated"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnActivated(EventArgs e) {
            // Select the GoTo line 
            txtGotoLine.Select();

            base.OnActivated(e);
        }

        // Handle text validation of the GoTo line number.
        private void TxtGotoLine_TextValidating(object sender, TextValidatingEventArgs e) {
            // Cancel the text entry if the text is non-empty and cannot be parsed as an int
            e.Cancel = (!(string.IsNullOrEmpty(e.NewText)) && !int.TryParse(e.NewText, out int i));
        }

        // Handle ok button click.
        private void BtnOK_Click(object sender, EventArgs e) {
            // Parse the goto line text into an int (already validated) and navigate
            if (int.TryParse(txtGotoLine.Text, out int parseResult)) {
                //	Line numbers are zero-indexed
                _goToLineNumber = parseResult - 1;
                // Coerce to min (0)
                if (_goToLineNumber < 0) {
                    _manager.GoToLine(0);
                }
                // Coerce to max
                else if (_goToLineNumber >= _maximumLineNumber) {
                    _manager.GoToLine(_maximumLineNumber);
                }
                else {
                    _manager.GoToLine(_goToLineNumber);
                }
                Hide();
            }
        }

        // Handle cancel button click.
        private void BtnCancel_Click(object sender, EventArgs e) {
            // Hide the dialog (do not close)
            Hide();
        }

        #endregion Event Handlers
    }
}