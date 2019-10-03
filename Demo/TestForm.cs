#region Using Directives

using ScintillaNET;
using ScintillaNET_Components;
using CodeEditor_Components;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

#endregion Using Directives

namespace Demo
{
    public partial class TestForm : Form
    {
        private readonly FindReplace MyFindReplace;
        private readonly GoTo MyGoTo;
        private readonly Suggestions MySuggestions;
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

            MySuggestions = new Suggestions(MyScintillaWrapper) {
                UseEditorFont = true,
            };
            var items = new List<SuggestionItem> {
                new SuggestionItem("Scintilla") {
                    IconIndex = 0,
                    ToolTipTitle = "Scintilla Text Editor",
                    ToolTipText = "Scintilla is a free source code editing component.",
                },
                new SuggestionItem("Item 1") { IconIndex = 1, ToolTipTitle = "Item 1" },
                new SuggestionItem("Item 2") { IconIndex = 2, ToolTipTitle = "Item 2" },
                new SuggestionItem("Item 3") { IconIndex = 3, ToolTipTitle = "Item 3" },
                new SuggestionItem("Item 4") { IconIndex = 4, ToolTipTitle = "Item 4" },
                new SuggestionItem("Item 5") { IconIndex = 5, ToolTipText = "Item 5" },
                new SuggestionItem("Item 6") { IconIndex = 6, ToolTipText = "Item 6" },
                new SuggestionItem("Item 7") { IconIndex = 7, ToolTipText = "Item 7" },
                new SuggestionItem("Item 8") { IconIndex = 8, ToolTipText = "Item 8" },
                new SuggestionItem("Item 9") { IconIndex = 9, ToolTipText = "Item 9" },
            };
            MySuggestions.SetSuggestions(items);
            MySuggestions.SuggestionImages = suggestionImages;
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