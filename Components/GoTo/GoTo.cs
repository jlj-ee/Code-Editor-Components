#region Using Directives

using System.ComponentModel;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Class for managing line navigation of a <see cref="IEditor"/> control, programmatically or through a <see cref="GoToDialog"/>.
    /// </summary>
	public class GoTo : ComponentManager
    {
        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="GoTo"/> object that will be associated with the given <see cref="IEditor"/> control.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control that <see cref="GoTo"/> can act upon.</param>
        public GoTo(IEditor editor) : base() {
            Window = CreateWindowInstance();

            if (editor != null) {
                Editor = editor;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IEditor"/> associated with the <see cref="GoTo"/>.
        /// </summary>
        public override IEditor Editor {
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
        /// Navigates the caret in the associated <see cref="IEditor"/> control to the start of the given line.
        /// </summary>
        /// <param name="lineNum">Target line number.</param>
        public void GoToLine(int lineNum) {
            Editor.GoToLine(lineNum);
        }

        /// <summary>
        /// Navigates the caret in the associated <see cref="IEditor"/> control to the given character position.
        /// </summary>
        /// <param name="pos">Target character</param>
		public void GoToPosition(int pos) {
            Editor.GoToPosition(pos);
        }

        /// <summary>
        /// Updates the <see cref="GoToDialog"/> with current information from the <see cref="IEditor"/> control.
        /// </summary>
        public void UpdateDialog() {
            Window.CurrentLineNumber = Editor.CurrentLine;
            Window.GoToLineNumber = Editor.CurrentLine;
            Window.MaximumLineNumber = Editor.LineCount;
        }

        /// <summary>
        /// Displays the <see cref="GoToDialog"/> window.
        /// </summary>
		public void ShowDialog() {
            UpdateDialog();
            if (!Window.Visible) {
                Window.Show(Editor.Target.FindForm());
            }
        }

        /// <summary>
        /// Creates and returns a new <see cref="GoToDialog" /> object.
        /// </summary>
        /// <returns>A new <see cref="GoToDialog" /> object.</returns>
        private GoToDialog CreateWindowInstance() {
            return new GoToDialog(this);
        }

        /// <summary>
        /// Release the resources of the components that are part of this <see cref="GoTo"/> instance.
        /// </summary>
        /// <param name="disposing">Set to true to release resources.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                Window?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Methods
    }
}