#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion Using Directives

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
        private string _longestString = "";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="SuggestionDropDown"/>.
        /// </summary>
        public SuggestionDropDown(Suggestions manager) : base() {
            _manager = manager;

            // Construct listbox
            List = new SuggestionList {
                Font = _manager.GetEditorFont()
            };
            List.LostFocus += List_LostFocus;
            _host = new ToolStripControlHost(List);

            // Set ToolStripDropDown properties
            AutoClose = false;
            AutoSize = false;
            DoubleBuffered = true;
            DropShadowEnabled = false;
            Padding = Margin = _host.Padding = _host.Margin = Padding.Empty;

            // Size container to ListBox
            List.MinimumSize = List.Size;
            List.MaximumSize = List.Size;
            Size = List.Size;

            Items.Add(_host);
            Application.AddMessageFilter(this);
        }

        private void List_LostFocus(object sender, EventArgs e) {
            Close();
        }


        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the color theme.
        /// </summary>
        public ListTheme Theme {
            get {
                return List.Theme;
            }
            set {
                List.Theme = value;
            }
        }

        internal SuggestionList List { get; }

        #endregion Properties

        #region Events & Handlers

        /// <summary>
        /// Handles when the dropdown is closed and raises the <see cref="ToolStripDropDown.Closed"/> event.
        /// </summary>
        /// <param name="e">The info about the toolstrip close event.</param>
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e) {
            base.OnClosed(e);
            //Capture = false;
            List.Close();
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Display the <see cref="SuggestionDropDown"/>.
        /// </summary>
        public void ShowSuggestionBox(Point point) {
            if (List.VisibleItems.Count > 0) {
                List.Font = _manager.GetEditorFont();
                // Adjust size according to contents and limits
                int textWidth = TextRenderer.MeasureText(_longestString, List.Font).Width;
                int listWidth = Math.Min(textWidth + SystemInformation.VerticalScrollBarWidth + 5, MaximumSize.Width);
                int numVisibleItems = Math.Min(_manager.MaxVisibleItems, List.VisibleItems.Count);
                int listHeight = (numVisibleItems * List.ItemHeight) + List.Margin.Size.Height + Padding.Size.Height;
                Size = new Size(listWidth, listHeight);
                List.SelectedItemIndex = -1;
                Show(_manager.Editor.Target, point, ToolStripDropDownDirection.BelowRight);
                //Capture = true;
            }
        }

        /// <summary>
        /// Adds the given items to the <see cref="SuggestionDropDown"/>.
        /// </summary>
        /// <param name="items"><see cref="List{Suggestion}"/> containing items to display.</param>
        public void AddItems(List<SuggestionItem> items) {
            int maxTextLength = 0;
            List.VisibleItems = items;
            //List.BeginUpdate();
            foreach (var item in items) {
                //    //List.Items.Add(item)
                int textLength = item.GetDisplayText().Length;
                if (textLength > maxTextLength) {
                    maxTextLength = textLength;
                    _longestString = item.GetDisplayText();
                }
            }
            //List.EndUpdate();
        }

        /// <summary>
        /// Handle resizing of the <see cref="SuggestionDropDown"/> and raise the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnSizeChanged(EventArgs e) {
            if (List != null) {
                // Size and position content inside ToolStripDropdown
                Size newSize = new Size(Size.Width - (Padding.Size.Width + 2), Size.Height - (Padding.Size.Height + 2));
                List.MinimumSize = newSize;
                List.MaximumSize = newSize;
                List.Size = newSize;
                List.Location = new Point(1, 1);
            }
            base.OnSizeChanged(e);
        }

        #endregion Methods

        #region IMessageFilter

        // Messages to monitor
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int WM_NCMBUTTONDOWN = 0x00A7;

        // Import API to map coordinates from one window to another
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
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
