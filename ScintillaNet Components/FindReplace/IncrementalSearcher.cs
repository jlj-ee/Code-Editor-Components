namespace ScintillaNET_Components
{
    #region Using Directives

    using ScintillaNET_Components.SearchTypes;
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    #endregion Using Directives

    /// <summary>
    /// Class to define the logic for a search toolbar.
    /// </summary>
    public partial class IncrementalSearcher : UserControl
    {
        #region Fields

        private bool _toolItem = false;
        private bool _goNext = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="IncrementalSearcher"/> panel.
        /// </summary>
        /// <param name="manager"><see cref="FindReplace"/> instance that will manage this <see cref="IncrementalSearcher"/>.</param>
        public IncrementalSearcher(FindReplace manager) {
            InitializeComponent();
            Manager = manager;
        }

        /// <summary>
        /// Construts a new <see cref="IncrementalSearcher"/> panel.
        /// </summary>
        public IncrementalSearcher() : this(null) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the <see cref="FindReplace"/> instance that manages this <see cref="IncrementalSearcher"/>.
        /// </summary>
        public FindReplace Manager { get; set; }

        /// <summary>
        /// Gets or sets whether this <see cref="IncrementalSearcher"/> instance is a tool item, i.e. embedded on an external panel.
        /// </summary>
        public bool ToolItem {
            get { return _toolItem; }
            set {
                _toolItem = value;
                if (_toolItem) {
                    panel.BackColor = Color.Transparent;
                    panel.BorderStyle = BorderStyle.None;
                }
                else {
                    panel.BackColor = Color.LightGray;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                }

            }
        }

        #endregion Properties

        #region Event Handlers

        #region Dialog

        /// <summary>
        /// Handle creation of the <see cref="IncrementalSearcher"/> and raise the <c>CreateControl</c> event./>
        /// </summary>
        protected override void OnCreateControl() {
            base.OnCreateControl();
            txtFind.Focus();
        }

        ///// <summary>
        ///// Handle leaving the <see cref="IncrementalSearcher"/> and raise the <see cref="Control.LostFocus"/> event.
        ///// </summary>
        ///// <param name="e">Event data.</param>
        //protected override void OnLeave(EventArgs e) {
        //    base.OnLostFocus(e);
        //}

        /// <summary>
        /// Handle the visibility of the <see cref="IncrementalSearcher"/> changing and raise the <see cref="Control.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnVisibleChanged(EventArgs e) {
            lblStatus.Text = string.Empty;
            txtFind.BackColor = SystemColors.Window;
            if ((!Visible) && (Manager != null)) {
                txtFind.Text = string.Empty;
            }
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Event that occurs when a key is pressed in the <see cref="IncrementalSearcher"/>.
        /// </summary>
        public event KeyPressedHandler KeyPressed;

        /// <summary>
        /// Represents the method that will handle key presses on the <see cref="IncrementalSearcher"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void KeyPressedHandler(object sender, KeyEventArgs e);

        #endregion Dialog

        #region Buttons

        // Handle find next button click.
        private void BtnNext_Click(object sender, EventArgs e) {
            GoToNextResult();
        }

        // Handle find previous button click.
        private void BtnPrevious_Click(object sender, EventArgs e) {
            GoToPreviousResult();
        }

        // Handle highlight all button click.
        private void BtnHighlightAll_Click(object sender, EventArgs e) {
            // Either add the highlights, or clear them
            if (chkHighlightAll.Checked) {
                Manager.Highlight(Manager.CurrentResults);
            }
            else {
                Manager.ClearAllHighlights();
            }
        }

        #endregion Buttons

        #region Text

        // Handle key presses on the search bar.
        private void TxtFind_KeyDown(object sender, KeyEventArgs e) {
            // Raise KeyPressed event so it can be handled externally
            KeyPressed?.Invoke(this, e);

            switch (e.KeyCode) {
                case Keys.Enter:
                    if (_goNext) {
                        GoToNextResult();
                    }
                    else {
                        GoToPreviousResult();
                    }
                    e.Handled = true;
                    break;

                case Keys.Down:
                    GoToNextResult();
                    e.Handled = true;
                    break;

                case Keys.Up:
                    GoToPreviousResult();
                    e.Handled = true;
                    break;

                case Keys.Escape:
                    if (!_toolItem) {
                        Hide();
                    }
                    break;
            }
            base.OnKeyDown(e);
        }

        // Handle text entry in the search field.
        private void TxtFind_TextChanged(object sender, EventArgs e) {
            ProcessFindReplace(SearchWrapper, AddFindHistory, false);
        }

        #endregion Text

        #endregion Event Handlers

        #region Methods

        private void GoToNextResult() {
            ProcessFindReplace(FindWrapper, AddFindHistory, false);
        }

        private void GoToPreviousResult() {
            ProcessFindReplace(FindWrapper, AddFindHistory, true);
        }

        // Adds the find text to the find history.
        private void AddFindHistory() {
            Manager.AddFindHistory(txtFind.Text);
        }

        // Returns a search object representing the search query
        private Search GetQuery() {
            CharacterRange searchRange = Manager.GetEditorWholeRange();
        
            if (chkRegex.Checked) {
                try {
                    return new RegexSearch(searchRange, txtFind.Text, RegexOptions.None);
                }
                catch (ArgumentException) {
                    lblStatus.Text = "Error in Regular Expression";
                    return null;
                }
            }
            else {
                return new StringSearch(searchRange, txtFind.Text, chkMatchCase.Checked, chkWholeWord.Checked);
            }
        }

        private CharacterRange SearchWrapper(bool searchUp) {
            Search query = GetQuery();
            int pos = Manager.GetEditorSelectedRange().cpMin;
            int length = Manager.GetEditorWholeRange().cpMax;
            query.SearchRange = new CharacterRange(pos, length);

            CharacterRange findRange = Manager.Find(query, false);
            if (findRange.cpMin == findRange.cpMax) {
                query.SearchRange = new CharacterRange(0, pos);
                findRange = Manager.Find(query, false);
            }

            if (findRange.cpMin != findRange.cpMax) {
                query.SearchRange = new CharacterRange(0, length);
                Manager.FindAll(query, false, chkHighlightAll.Checked);
            }

            return findRange;
        }

        // Use the dialog configuration to search for a match.
        private CharacterRange FindWrapper(bool searchUp) {
            if (searchUp) {
                _goNext = false;
                return Manager.FindPrevious(GetQuery(), chkWrap.Checked);
            }
            else {
                _goNext = true;
                return Manager.FindNext(GetQuery(), chkWrap.Checked);
            }
        }

        // Process find/replace next/previous:
        // - Update history
        // - Run search and navigate to first result
        // - Update status text
        private void ProcessFindReplace(Func<bool, CharacterRange> findReplace, Action addMru, bool searchUp) {
            txtFind.BackColor = SystemColors.Window;
            string statusText = string.Empty;
            if (txtFind.Text != string.Empty) {     
                statusText = Manager.RunFindReplace(findReplace, addMru, searchUp);
                if (!_toolItem) {
                    Manager.EnsureVisible(Bounds);
                }
            }
            else {
                Manager.Clear();
            }
            UpdateStatus(statusText);
        }

        // Updates the status text
        internal void UpdateStatus(string text) {
            if (text != null) {
                lblStatus.Text = text;
            }
            if ((txtFind.Text != string.Empty) && (Manager.CurrentResults.Count == 0)) {
                txtFind.BackColor = Color.LightPink;
            }
        }

        #endregion Methods
    }
}