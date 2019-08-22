#region Using Directives

using ScintillaNET;
using ScintillaNET_Components;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion Using Directives

namespace Demo
{
    public partial class TestForm : Form
    {
        private FindReplace MyFindReplace;
        private GoTo MyGoTo;

        public TestForm() {
            InitializeComponent();
            scintilla1.Styles[Style.Default].Font = "Consolas";
            scintilla1.Styles[Style.Default].Size = 10;
            scintilla2.Styles[Style.Default].Font = "Consolas";
            scintilla2.Styles[Style.Default].Size = 10;
            tabControl1.SelectedIndexChanged += Form_TabChanged;

            MyFindReplace = new FindReplace(scintilla1);
            MyFindReplace.Window.AutoPosition = true;
            MyFindReplace.KeyPressed += MyFindReplace_KeyPressed;

            MyGoTo = new GoTo(scintilla1);

            incrementalSearcherToolStrip.Manager = MyFindReplace;
        }

        private void Form_TabChanged(object sender, EventArgs e) {
            switch (tabControl1.SelectedIndex) {
                case 0:
                    scintilla1.Focus();
                    break;
                case 1:
                    scintilla2.Focus();
                    break;
                default:
                    break;
            }
        }

        private void MyFindReplace_KeyPressed(object sender, KeyEventArgs e) {
            GenericScintilla_KeyDown(sender, e);
        }

        private void GotoButton_Click(object sender, EventArgs e) {
            // Use the FindReplace Scintilla as this will change based on focus
            MyGoTo.ShowDialog();
        }

        /// <summary>
        /// Key down event for each Scintilla. Tie each Scintilla to this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenericScintilla_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.F) {
                MyFindReplace.ShowFind();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.H) {
                MyFindReplace.ShowReplace();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.I) {
                MyFindReplace.ShowIncrementalSearch();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.G) {
                MyGoTo.ShowDialog();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape) {
                MyFindReplace.HideIncrementalSearch();
                MyFindReplace.HideFindReplace();
            }
        }

        /// <summary>
        /// Enter event tied to each Scintilla that will share a FindReplace dialog.
        /// Tie each Scintilla to this event.
        /// </summary>
        /// <param name="sender">The Scintilla receiving focus</param>
        /// <param name="e"></param>
        private void GenericScintilla_Enter(object sender, EventArgs e) {
            MyFindReplace.Editor = (Scintilla)sender;
            MyGoTo.Editor = (Scintilla)sender;
        }

        private void GenericScintilla_Enter(object sender, MouseEventArgs e) {

        }
    }
}