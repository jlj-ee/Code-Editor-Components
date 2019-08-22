using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace ScintillaNET_Components
{

    /// <summary>
    /// A <see cref="TextBox"/> with extensible event-driven text validation capabilities.
    /// </summary>
    /// <remarks> 
    /// Inspired by https://stackoverflow.com/a/12374050
    /// </remarks>
    public class ValidatingTextBox : TextBox
    {
        #region Fields

        private string _validText;
        private int _selectionStart;
        private int _selectionEnd;
        private bool _dontProcessMessages;

        #endregion Fields

        #region Events

        /// <summary>
        /// Text validation event that must be handled to process new text.
        /// </summary>
        public event EventHandler<TextValidatingEventArgs> TextValidating;

        /// <summary>
        /// Raise event to parent so registered delegates receive it.
        /// </summary>
        /// <param name="sender">The TextBox that needs to validate text.</param>
        /// <param name="e">The new text to be validated.</param>
        protected virtual void OnTextValidating(object sender, TextValidatingEventArgs e) {
            TextValidating?.Invoke(sender, e);
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Intercept Windows messages to the TextBox in order to sanitize input.
        /// </summary>
        /// <param name="m">Windows message</param>
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (_dontProcessMessages) {
                return;
            }

            // Pre-store initial text and selection on key down/idle
            const int WM_KEYDOWN = 0x100;
            const int WM_ENTERIDLE = 0x121;
            const int VK_DELETE = 0x2e;
            bool delete = m.Msg == WM_KEYDOWN && (int)m.WParam == VK_DELETE;
            if ((m.Msg == WM_KEYDOWN && !delete) || m.Msg == WM_ENTERIDLE) {
                DontProcessMessage(() => {
                    _validText = Text;
                    _selectionStart = SelectionStart;
                    _selectionEnd = SelectionLength;
                });
            }

            // Store new character or pasted/edited text from key down
            const int WM_CHAR = 0x102;
            const int WM_PASTE = 0x302;
            if (m.Msg == WM_CHAR || m.Msg == WM_PASTE || delete) {
                string newText = null;
                DontProcessMessage(() => {
                    newText = Text;
                });

                // Raise text validation event with the new text
                var e = new TextValidatingEventArgs(newText);
                OnTextValidating(this, e);
                // Restore pre-stored text if validation fails
                if (e.Cancel) {
                    DontProcessMessage(() => {
                        Text = _validText;
                        SelectionStart = _selectionStart;
                        SelectionLength = _selectionEnd;
                    });
                }
            }
        }

        private void DontProcessMessage(Action action) {
            // Stop processing messages while trying the action
            _dontProcessMessages = true;
            try {
                action();
            }
            finally {
                _dontProcessMessages = false;
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// A cancelable text validation event.
    /// </summary>
    public class TextValidatingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="TextValidatingEventArgs"/> with the given text.
        /// </summary>
        /// <param name="newText">Text to be validated.</param>
        public TextValidatingEventArgs(string newText) {
            NewText = newText;
        }

        /// <summary>
        /// Gets the text to be validated.
        /// </summary>
        public string NewText { get; }
    }
}
