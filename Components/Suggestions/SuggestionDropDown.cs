using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to display a list of suggested code completion items.
    /// Implements <see cref="IMessageFilter"/> to catch clicks outside the dropdown using low-level calls.
    /// </summary>
    public class SuggestionDropDown : ToolStripDropDown, IMessageFilter
    {

        #region Fields

        private readonly ToolStripControlHost _host;
        private readonly Suggestions _manager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="SuggestionDropDown"/> given a <see cref="Suggestions"/> intance to manages it.
        /// </summary>
        public SuggestionDropDown(Suggestions manager) : base() {
            _manager = manager;

            // Construct listbox
            List = new SuggestionList();
            List.LostFocus += List_LostFocus;
            _host = new ToolStripControlHost(List);

            // Set ToolStripDropDown properties
            AutoClose = false;
            AutoSize = false;
            DropShadowEnabled = false;
            _host.Margin = new Padding(2);
            _host.Padding = Padding.Empty;

            // Size container to ListBox
            List.MaximumSize = List.Size;

            Items.Add(_host);
            Application.AddMessageFilter(this);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the <see cref="SuggestionList"/> used by this dropdown.
        /// </summary>
        public SuggestionList List { get; }

        /// <summary>
        /// Gets the <see cref="Suggestions"/> instance that manages this dropdown.
        /// </summary>
        public Suggestions Manager {
            get {
                return _manager;
            }
        }

        #endregion Properties

        #region Events & Handlers

        /// <summary>
        /// Handles when the dropdown is closed and raises the <see cref="ToolStripDropDown.Closed"/> event.
        /// </summary>
        /// <param name="e">Toolstrip dropdown close event data.</param>
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e) {
            base.OnClosed(e);
            //Capture = false;
            List.Close();
        }

        /// <summary>
        /// Handle resizing of the <see cref="SuggestionDropDown"/> and raise the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnSizeChanged(EventArgs e) {
            if (List != null) {
                // Size and position content inside ToolStripDropdown
                _host.Size = new Size(Size.Width - _host.Margin.Size.Width, Size.Height - _host.Margin.Size.Height);
                List.MinimumSize = _host.Size;
                List.MaximumSize = _host.Size;
                List.Size = _host.Size;
                List.Location = new Point(2, 2);
            }
            base.OnSizeChanged(e);
        }

        // Close the dropdown if the List loses focus.
        private void List_LostFocus(object sender, EventArgs e) {
            if (!Focused) {
                Close();
            }
        }

        #endregion Events & Handlers

        #region Methods

        #endregion Methods

        #region IMessageFilter

        // Messages to monitor
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int WM_NCMBUTTONDOWN = 0x00A7;

        /// <summary>
        /// Converts (maps) a set of points from a coordinate space relative to one window to a coordinate space relative to another window.
        /// </summary>
        /// <param name="hWndFrom">Handle to the window from which points are converted. 
        /// If this parameter is NULL or HWND_DESKTOP, the points are presumed to be in screen coordinates.</param>
        /// <param name="hWndTo">Handle to the window to which points are converted. 
        /// If this parameter is NULL or HWND_DESKTOP, the points are converted to screen coordinates.</param>
        /// <param name="pt">Pointer to an array of POINT structures that contain the set of points to be converted.</param>
        /// <param name="cPoints">Number of POINT structures in the array pointed to by the pt parameter.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt, [MarshalAs(UnmanagedType.U4)] int cPoints);

        /// <summary>
        /// Monitor messages in order to detect if the user clicks outside the dropdown.
        /// </summary>
        public bool PreFilterMessage(ref Message m) {
            if (Visible) {
                switch (m.Msg) {
                    case WM_LBUTTONDOWN:
                    case WM_RBUTTONDOWN:
                    case WM_MBUTTONDOWN:
                    case WM_NCLBUTTONDOWN:
                    case WM_NCRBUTTONDOWN:
                    case WM_NCMBUTTONDOWN:
                        int i = unchecked((int)(long)m.LParam);
                        short x = (short)(i & 0xFFFF);
                        short y = (short)((i >> 16) & 0xffff);
                        Point clickPoint = new Point(x, y);
                        // non-client area: x, y are relative to the desktop
                        IntPtr srcWnd = IntPtr.Zero;
                        if ((m.Msg == WM_LBUTTONDOWN) || (m.Msg == WM_RBUTTONDOWN) || (m.Msg == WM_MBUTTONDOWN)) {
                            // client area: x, y are relative to the client area of the windows
                            srcWnd = m.HWnd;
                        }
                        MapWindowPoints(srcWnd, Handle, ref clickPoint, 1);
                        // Close the dropdown if click is outside
                        if (!ClientRectangle.Contains(clickPoint)) {
                            Close();
                        }
                        break;
                }
            }
            return false;
        }

        #endregion IMessageFilter
    }
}
