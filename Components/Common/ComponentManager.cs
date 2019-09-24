#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to model a <see cref="Component"/> that interfaces with a <see cref="IEditor"/> control.
    /// </summary>
    public class ComponentManager : Component
    {
        #region Fields

        private IEditor _editor;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IEditor"/> control for this <see cref="ComponentManager"/>.
        /// </summary>
        public virtual IEditor Editor {
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
        /// Gets the font from the default <see cref="Style"/> of the <see cref="IEditor"/> control.
        /// </summary>
        /// <returns><see cref="Font"/> that the control is using, scaled by the zoom factor.</returns>
        public Font GetEditorFont() {
            return _editor.Font;
        }

        #region Position

        /// <summary>
        /// Calculates a location at which the given rectangle will not obscure the editor selection.
        /// </summary>
        /// <param name="r"><see cref="Rectangle"/> that must not cover the selection.</param>
        /// <returns><see cref="Point"/> where the rectangle will not cover the selection.</returns>
        public Point AvoidSelection(Rectangle r) {
            Point point = Editor.GetPointFromPosition(Editor.CurrentPosition);

            Point cursorPoint = Editor.Target.PointToScreen(new Point(point.X, point.Y));

            if (r.Contains(cursorPoint)) {
                Point newLocation;
                int lineHeight = Editor.LineHeight;
                if (cursorPoint.Y < (Screen.PrimaryScreen.Bounds.Height / 2)) {
                    // Top half of the screen: move down
                    newLocation = Editor.Target.PointToClient(new Point(r.X, cursorPoint.Y + lineHeight * 2));
                }
                else {
                    // Bottom half of the screen: move up 
                    newLocation = Editor.Target.PointToClient(new Point(r.X, cursorPoint.Y - r.Height - (lineHeight * 2)));
                }
                return Editor.Target.PointToScreen(newLocation);
            }
            return r.Location;
        }

        /// <summary>
        /// Scrolls the editor to avoid the given rectangle.
        /// </summary>
        /// <param name="regionToAvoid"><see cref="Rectangle"/> to avoid.</param>
        public void ScrollToCaret(Rectangle regionToAvoid = new Rectangle()) {
            Editor.ScrollToCaret(regionToAvoid);
        }

        #endregion Position

        #endregion Methodsbo
    }
}
