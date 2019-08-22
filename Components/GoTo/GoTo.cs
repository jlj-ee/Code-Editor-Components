#region Using Directives

using ScintillaNET;
using System.ComponentModel;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Class for managing line navigation of a <see cref="Scintilla"/> control, programmatically or through a <see cref="GoToDialog"/>.
    /// </summary>
	public class GoTo : ComponentManager
    {
        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="GoTo"/> object that will be associated with the given <see cref="Scintilla"/> control.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control that <see cref="GoTo"/> can act upon.</param>
        public GoTo(Scintilla editor) : base() {
            Window = CreateWindowInstance();

            if (editor != null) {
                Editor = editor;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Scintilla"/> control associated with the <see cref="GoTo"/>.
        /// </summary>
        public override Scintilla Editor {
            get {
                return base.Editor;
            }
            set {
                base.Editor = value;
                UpdateDialog();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GoToDialog"/> that this <see cref="GoTo"/> controls.
        /// </summary>
        public GoToDialog Window { get; set; }
        
        #endregion Properties

        #region Methods
        
        /// <summary>
        /// Navigates the caret in the associated <see cref="ScintillaNET.Scintilla"/> control to the start of the given line.
        /// </summary>
        /// <param name="lineNum">Target line number.</param>
        public void GoToLine(int lineNum) {
            Editor.Lines[lineNum].Goto();
        }

        /// <summary>
        /// Navigates the caret in the associated <see cref="ScintillaNET.Scintilla"/> control to the given character position.
        /// </summary>
        /// <param name="pos">Target character</param>
		public void GoToPosition(int pos) {
            Editor.GotoPosition(pos);
        }

        /// <summary>
        /// Updates the <see cref="GoToDialog"/> with current information from the <see cref="ScintillaNET.Scintilla"/> control.
        /// </summary>
        public void UpdateDialog() {
            Window.CurrentLineNumber = Editor.CurrentLine;
            Window.GoToLineNumber = Editor.CurrentLine;
            Window.MaximumLineNumber = Editor.Lines.Count;
        }

        /// <summary>
        /// Displays the <see cref="GoToDialog"/> window.
        /// </summary>
		public void ShowDialog() {
            UpdateDialog();
            if (!Window.Visible) {
                Window.Show(Editor.FindForm());
            }
        }

        /// <summary>
        /// Creates and returns a new <see cref="GoToDialog" /> object.
        /// </summary>
        /// <returns>A new <see cref="GoToDialog" /> object.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual GoToDialog CreateWindowInstance() {
            return new GoToDialog(this);
        }

        #endregion Methods
    }
}