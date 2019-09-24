#region Using Directives

using ScintillaNET;
using ScintillaNET_Components;
using CodeEditor_Components;
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
        private Suggestions MySuggestions;
        private ScintillaNETWrapper MyScintillaWrapper;

        public TestForm() {
            InitializeComponent();
            scintilla1.Styles[Style.Default].Font = "Consolas";
            scintilla1.Styles[Style.Default].Size = 10;
            scintilla2.Styles[Style.Default].Font = "Consolas";
            scintilla2.Styles[Style.Default].Size = 10;
            tabControl1.SelectedIndexChanged += Form_TabChanged;

            MyScintillaWrapper = new ScintillaNETWrapper(scintilla1);

            MyFindReplace = new FindReplace(MyScintillaWrapper);
            MyFindReplace.Window.AutoPosition = true;
            MyFindReplace.KeyPressed += MyFindReplace_KeyPressed;

            MyGoTo = new GoTo(MyScintillaWrapper);

            incrementalSearcherToolStrip.Manager = MyFindReplace;

            MySuggestions = new Suggestions(MyScintillaWrapper);
            var items = new List<SuggestionItem> {
                new SuggestionItem("Scintilla") {
                    ToolTipTitle = "Scintilla Text Editor",
                    ToolTipText = @"
Scintilla is a free source code editing component. It comes with complete source code and a license that permits use in any free project or commercial product.

As well as features found in standard text editing components, Scintilla includes features especially useful when editing and debugging source code.
These include support for syntax styling, error indicators, code completion and call tips.
The selection margin can contain markers like those used in debuggers to indicate breakpoints and the current line.
Styling choices are more open than with many editors, allowing the use of proportional fonts, bold and italics, multiple foreground and background colours and multiple fonts.",
                },
                new SuggestionItem("Item 1"),
                new SuggestionItem("Item 2"),
                new SuggestionItem("Item 3"),
                new SuggestionItem("Item 4"),
                new SuggestionItem("Item 5"),
                new SuggestionItem("Item 6")
            };
            MySuggestions.SetSuggestions(items);
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
                MySuggestions.HideSuggestions();
            }
        }

        /// <summary>
        /// Enter event tied to each Scintilla that will share a FindReplace dialog.
        /// Tie each Scintilla to this event.
        /// </summary>
        /// <param name="sender">The Scintilla receiving focus</param>
        /// <param name="e"></param>
        private void GenericScintilla_Enter(object sender, EventArgs e) {
            MyScintillaWrapper = new ScintillaNETWrapper(sender as Scintilla);
            MyFindReplace.Editor = MyScintillaWrapper;
            MyGoTo.Editor = MyScintillaWrapper;
            MySuggestions.Editor = MyScintillaWrapper;
        }

        private void GenericScintilla_Enter(object sender, MouseEventArgs e) {

        }
    }
}