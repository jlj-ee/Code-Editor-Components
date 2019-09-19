#region Using Directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Class to encapsulate common capabilities of a utility dialog for a text control.
    /// </summary>
    public class Dialog : Form
    {
        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Dialog"/>.
        /// </summary>
        public Dialog() {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="ComponentManager"/> that handles the back-end for the dialog.
        /// </summary>
        public ComponentManager Manager { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="Dialog"/> should automatically move away from the selected text.
        /// </summary>
        public bool AutoPosition { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the location of the <see cref="Dialog"/> to the center of the given control area.
        /// </summary>
        /// <param name="control">The reference <see cref="Control"/> used to center the <see cref="Dialog"/></param>
        public void CenterDialog(Control control) {
            if (control != null) {
                Point p = control.PointToScreen(control.ClientRectangle.Location);
                Location = new Point(p.X + control.Width / 2 - Width / 2, p.Y + control.Height / 2 - Height / 2);
            }
        }

        /// <summary>
        /// Moves the <see cref="Dialog"/> so that it is not covering the selection.
        /// </summary>
        public virtual void MoveDialogAwayFromSelection() {
            if (!Visible || !AutoPosition) {
                return;
            }
            else {
                Location = Manager.AvoidSelection(Bounds);
            }
        }

        #endregion Methods

        #region Event Handlers

        /// <summary>
        /// Handle the form loading event by centering the <see cref="Dialog"/> in the editor.
        /// </summary>
        /// <param name="e">Information about the loading event.</param>
        protected override void OnLoad(EventArgs e) {
            if (Manager != null) {
                CenterDialog(Manager.Editor);
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Handle the form closing event by hiding the <see cref="Dialog"/> instead of closing it.
        /// </summary>
        /// <param name="e">Information about the closing event.</param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Hide();
            }

            base.OnFormClosing(e);
        }

        #endregion Event Handlers

    }
}
