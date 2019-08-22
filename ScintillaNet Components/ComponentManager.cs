#region Using Directives

using ScintillaNET;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace ScintillaNET_Components
{
    /// <summary>
    /// Class to model a <see cref="Component"/> that interfaces with a <see cref="Scintilla"/> control.
    /// </summary>
    public class ComponentManager : Component
    {
        #region Fields

        private Scintilla _editor;

        #endregion Fields


        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Scintilla"/> control for this <see cref="ComponentManager"/>.
        /// </summary>
        public virtual Scintilla Editor {
            get {
                return _editor;
            }
            set {
                _editor = value;
            }
        }

        #endregion Properties


        #region Methods

        /// <summary>
        /// Gets the font from the default <see cref="Style"/> of the <see cref="Scintilla"/> control.
        /// </summary>
        /// <returns><see cref="Font"/> that the control is using, scaled by the zoom factor.</returns>
        public Font GetEditorFont() {
            // Copy font from Scintilla
            Style editorStyle = _editor.Styles[Style.Default];
            return new Font(editorStyle.Font, editorStyle.Size + _editor.Zoom - 1, GraphicsUnit.Point); // Font size seems to be off by one...why?
        }

        #region Position

        /// <summary>
        /// Calculates a location at which the given rectangle will not obscure the editor selection.
        /// </summary>
        /// <param name="r"><see cref="Rectangle"/> that must not cover the selection.</param>
        /// <returns><see cref="Point"/> where the rectangle will not cover the selection.</returns>
        public Point AvoidSelection(Rectangle r) {
            int pos = Editor.CurrentPosition;
            int x = Editor.PointXFromPosition(pos);
            int y = Editor.PointYFromPosition(pos);

            Point cursorPoint = Editor.PointToScreen(new Point(x, y));

            if (r.Contains(cursorPoint)) {
                Point newLocation;
                int lineHeight = GetLineHeight();
                if (cursorPoint.Y < (Screen.PrimaryScreen.Bounds.Height / 2)) {
                    // Top half of the screen: move down
                    newLocation = Editor.PointToClient(new Point(r.X, cursorPoint.Y + lineHeight * 2));
                }
                else {
                    // Bottom half of the screen: move up 
                    newLocation = Editor.PointToClient(new Point(r.X, cursorPoint.Y - r.Height - (lineHeight * 2)));
                }
                return Editor.PointToScreen(newLocation);
            }
            return r.Location;
        }

        /// <summary>
        /// Scrolls the editor to avoid the given rectangle.
        /// </summary>
        /// <param name="r"><see cref="Rectangle"/> to avoid.</param>
        public void EnsureVisible(Rectangle r) {
            int lineHeight = GetLineHeight();
            // Calculate how many lines the rectangle could block, rounding up to the nearest whole line
            int linesToAdd = r.Height / lineHeight + ((r.Height % lineHeight > 0) ? 1 : 0);
            // No supported API for setting caret policy from ScintillaNET, so use low-level messaging
            int SCI_SETYCARETPOLICY = 2403;
            // CARET_SLOP = linesToAdd, CARET_STRICT, CARET_EVEN
            Editor.DirectMessage(SCI_SETYCARETPOLICY, new IntPtr(0xD), new IntPtr(linesToAdd));
            if (Editor.PointYFromPosition(Editor.CurrentPosition) + lineHeight > r.Y) {
                Editor.ScrollCaret();
            }
        }

        // No supported API for getting line height from ScintillaNET, so use low-level messaging
        private int GetLineHeight() {
            int SCI_TEXTHEIGHT = 2279;
            return Editor.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
        }

        #endregion Position

        #endregion Methods
    }
}
