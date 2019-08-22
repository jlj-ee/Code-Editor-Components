namespace ScintillaNET_Components
{
    #region Using Directives

    using ScintillaNET_Components.SearchTypes;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    #endregion Using Directives

    /// <summary>
    /// Class to define the logic for a Find/Replace <see cref="Dialog"/>.
    /// </summary>
    public partial class FindReplaceDialog : Dialog
    {
        #region Fields

        private CharacterRange _searchRange;
        private Control _menuSource;
        private FindReplace _manager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="FindReplaceDialog"/>.
        /// </summary>
        /// <param name="manager"><see cref="FindReplace"/> instance that manages this <see cref="FindReplaceDialog"/>.</param>
        public FindReplaceDialog(FindReplace manager) : base() {
            InitializeComponent();

            Manager = _manager = manager;
        }

        #endregion Constructors

        #region Events & Handlers

        /// <summary>
        /// Triggered when a find all action is performed.
        /// </summary>
        public event FindAllResultsEventHandler FindAllResults;

        /// <summary>
        /// Handler for the find all action event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="FindAllResults">The found results.</param>
        public delegate void FindAllResultsEventHandler(object sender, FindResultsEventArgs FindAllResults);

        /// <summary>
        /// Handler for the replace all action event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="FindAllResults">The found results.</param>
        public delegate void ReplaceAllResultsEventHandler(object sender, ReplaceResultsEventArgs FindAllResults);

        /// <summary>
        /// Triggered when a replace all action is performed.
        /// </summary>
        public event ReplaceAllResultsEventHandler ReplaceAllResults;

        #region Dialog

        /// <summary>
        /// Handle activation of the <see cref="FindReplaceDialog"/> and raise the <see cref="Form.Activated"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnActivated(EventArgs e) {
            // Make dialog fully opaque
            Opacity = 1.0;
            if (_manager.EditorHasSelection()) {
                chkSearchSelection.Enabled = true;
            }
            else {
                chkSearchSelection.Enabled = false;
                chkSearchSelection.Checked = false;
            }

            // Clear old search range because it may be invalid
            _searchRange = new CharacterRange();

            lblStatus.Text = string.Empty;
            statusStrip.Refresh();

            MoveDialogAwayFromSelection();

            base.OnActivated(e);
        }

        /// <summary>
        /// Handle deactivation of the <see cref="FindReplaceDialog"/> and raise the <see cref="Form.Deactivate"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnDeactivate(EventArgs e) {
            // Make dialog semi-transparent
            Opacity = 0.7;
            if (_manager.CurrentResults != null) {
                lblStatus.Text = _manager.CurrentResults.Count + " matches";
                statusStrip.Refresh();
            }

            base.OnDeactivate(e);
        }

        /// <summary>
        /// Handle hiding of <see cref="FindReplaceDialog"/> and raise the <see cref="Control.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnVisibleChanged(EventArgs e) {
            if (!Visible && (_manager != null)) {
                _manager.Clear();
            }
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Handle key presses on the <see cref="FindReplaceDialog"/> and raise the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The key info about the key(s) pressed.</param>
        protected override void OnKeyDown(KeyEventArgs e) {
            // Raise KeyPressed event so it can be handled externally
            KeyPressed?.Invoke(this, e);

            // Handle dialog-specific keys
            if (e.KeyCode == Keys.Escape) {
                Hide();
            }
            // Workaround because AcceptButton property does not properly handle this in all external containers
            else if (e.KeyCode == Keys.Enter) {
                AcceptButton.PerformClick();
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Event that occurs when a key is pressed in the <see cref="FindReplaceDialog"/>.
        /// </summary>
        public event KeyPressedHandler KeyPressed;

        /// <summary>
        /// Represents the method that will handle key presses on the <see cref="FindReplaceDialog"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The key info about the key(s) pressed.</param>
        public delegate void KeyPressedHandler(object sender, KeyEventArgs e);

        #endregion Dialog

        #region Buttons

        // Handle clear find all button click.
        private void BtnClear_Click(object sender, EventArgs e) {
            // Delete all markers and remove all highlighting
            // TODO: Raise event for FindAllResultsPanel?
            _manager.ClearAllMarks();
            _manager.ClearAllHighlights();
        }

        // Handle find all button click.
        private void BtnFindAll_Click(object sender, EventArgs e) {
            ProcessFindReplaceAll(FindAllWrapper, AddFindHistory, false);
        }

        // Handle find next butotn click.
        private void BtnFindNext_Click(object sender, EventArgs e) {
            GoToNextResult();
        }

        // Handle find previous button click.
        private void BtnFindPrevious_Click(object sender, EventArgs e) {
            GoToPreviousResult();
        }

        // Handle replace all button click.
        private void BtnReplaceAll_Click(object sender, EventArgs e) {
            ProcessFindReplaceAll(ReplaceAllWrapper, AddReplaceHistory, true);
        }

        // Handle replace next button click.
        private void BtnReplace_Click(object sender, EventArgs e) {
            ProcessFindReplace(ReplaceWrapper, AddReplaceHistory, false);
        }

        // Handle swap button click.
        private void BtnSwap_Click(object sender, EventArgs e) {
            var findString = txtFind.Text;
            txtFind.Text = txtReplace.Text;
            txtReplace.Text = findString;
        }

        // Handle mark/highlight checkbox click.
        private void ChkMarkHighlight_CheckedChanged(object sender, EventArgs e) {
            _manager.UpdateHighlights = true;
        }

        #endregion Button

        #region Menus

        // Handle find history menu button click
        private void CmdRecentFind_Click(object sender, EventArgs e) {
            ShowRecentMenu(cmdRecentFind, _manager.FindHistory);
        }

        // Handle replace history menu button click
        private void CmdRecentReplace_Click(object sender, EventArgs e) {
            ShowRecentMenu(cmdRecentReplace, _manager.ReplaceHistory);
        }

        // Handle history menu item click
        private void MnuRecent_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            TextBox txtBox = null;
            List<string> mru = new List<string>();
            if (_menuSource == cmdRecentFind) {
                txtBox = txtFind;
                mru = _manager.FindHistory;
            }
            else if (_menuSource == cmdRecentReplace) {
                txtBox = txtReplace;
                mru = _manager.ReplaceHistory;
            }
            if (txtBox != null) {
                if (e.ClickedItem.Text == "Clear History") {
                    // CLear the history list and disable the history control
                    mru.Clear();
                    if (_menuSource == cmdRecentFind || _menuSource == cmdRecentFind) {
                        cmdRecentFind.Enabled = false;
                    }
                    else {
                        _menuSource.Enabled = false;
                    }
                }
                else {
                    // Replace the text with the history item
                    txtBox.Text = e.ClickedItem.Tag.ToString();
                }
            }
        }

        // Handle extended/regex insert (find) menu button click
        private void CmdExtendedCharFind_Click(object sender, EventArgs e) {
            ShowExtRegexMenu(cmdExtCharAndRegExFind, mnuRegExCharFind);
        }

        // Handle extended/regex insert (replace) menu button click
        private void CmdExtendedCharReplace_Click(object sender, EventArgs e) {
            ShowExtRegexMenu(cmdExtCharAndRegExReplace, mnuRegExCharReplace);
        }

        // Handle extended/regex menu item click
        private void MnuExtRegExChar_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            TextBox txtBox = null;
            if (_menuSource == cmdExtCharAndRegExFind) {
                txtBox = txtFind;
            }
            else if (_menuSource == cmdExtCharAndRegExReplace) {
                txtBox = txtReplace;
            }
            if (txtBox != null) {
                // Insert the string value held in the menu items Tag field (\t, \n, etc.)
                txtBox.SelectedText = e.ClickedItem.Tag.ToString();
                // For the named group, select "Name" to easily edit
                if (e.ClickedItem.Tag.ToString() == "${Name}") {
                    txtBox.SelectionStart -= 5;
                    txtBox.SelectionLength = 4;
                    txtBox.Select();
                }
            }
        }

        #endregion Menus

        #region Navigation

        // Handle search type radio buttons changed
        private void RdoSearchType_CheckedChanged(object sender, EventArgs e) {
            // Show the appropriate options panel
            if (rdoRegex.Checked) {
                pnlRegexpOptions.BringToFront();
            }
            else {
                pnlStandardOptions.BringToFront();
            }

            // Enable/disable extended/regex insertion menu
            cmdExtCharAndRegExFind.Enabled = !rdoStandard.Checked;
            cmdExtCharAndRegExReplace.Enabled = !rdoStandard.Checked;
        }

        // Handle find/replace tab changed
        private void TabAll_Selecting(object sender, TabControlCancelEventArgs e) {
            // Update dialog title and move shared controls to the appropriate page
            if (e.TabPage == tpgFind) {
                Text = "Find";
                pnlSearchType.Parent = tpgFind;
                grpOptions.Parent = tpgFind;
                pnlFind.Parent = tpgFind;
                pnlFindNav.Parent = tpgFind;
                AcceptButton = btnFindNext;
            }
            else {
                Text = "Replace";
                pnlSearchType.Parent = tpgReplace;
                grpOptions.Parent = tpgReplace;
                pnlFind.Parent = tpgReplace;
                pnlFindNav.Parent = tpgReplace;
                AcceptButton = btnReplaceNext;
            }
        }

        #endregion Navigation

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Returns the regular expression configuration from the <see cref="FindReplaceDialog"/>.
        /// </summary>
        /// <returns><see cref="RegexOptions"/> flag with all of the selected options.</returns>
        private RegexOptions GetRegexOptions() {
            RegexOptions ro = RegexOptions.None;

            if (chkIgnoreCase.Checked) {
                ro |= RegexOptions.IgnoreCase;
            }

            if (chkIgnorePatternWhitespace.Checked) {
                ro |= RegexOptions.IgnorePatternWhitespace;
            }

            if (chkMultiline.Checked) {
                ro |= RegexOptions.Multiline;
            }

            if (chkSingleline.Checked) {
                ro |= RegexOptions.Singleline;
            }

            return ro;
        }

        /// <summary>
        /// Navigates to the next matched search result.
        /// </summary>
        private void GoToNextResult() {
            ProcessFindReplace(FindWrapper, AddFindHistory, false);
        }

        /// <summary>
        /// Navigates to the previous matched search result.
        /// </summary>
        private void GoToPreviousResult() {
            ProcessFindReplace(FindWrapper, AddFindHistory, true);
        }

        // Adds the find text to the find history.
        private void AddFindHistory() {
            _manager.AddFindHistory(txtFind.Text);
            if (!cmdRecentFind.Enabled) {
                cmdRecentFind.Enabled = true;
            }
        }

        // Adds the replace text to the replace history.
        private void AddReplaceHistory() {
            _manager.AddReplaceHistory(txtFind.Text, txtReplace.Text);
            if (!cmdRecentFind.Enabled) {
                cmdRecentFind.Enabled = true;
            }
            if (!cmdRecentReplace.Enabled) {
                cmdRecentReplace.Enabled = true;
            }
        }

        // Shows the history menu.
        private void ShowRecentMenu(Button cmdRecent, List<string> history) {
            _menuSource = cmdRecent;
            mnuRecent.Items.Clear();
            foreach (var item in history) {
                ToolStripItem newItem = mnuRecent.Items.Add(item);
                newItem.Tag = item;
            }
            if (history.Count > 0) {
                mnuRecent.Items.Add("-");
                mnuRecent.Items.Add("Clear History");
                mnuRecent.Show(cmdRecent.PointToScreen(cmdRecent.ClientRectangle.Location));
            }
        }

        // Shows the extended/regular expression instertion menu.
        private void ShowExtRegexMenu(Button cmdExtChar, ContextMenuStrip mnuRegEx) {
            _menuSource = cmdExtChar;
            if (rdoExtended.Checked) {
                mnuExtendedChar.Show(cmdExtChar.PointToScreen(cmdExtChar.ClientRectangle.Location));
            }
            else if (rdoRegex.Checked) {
                mnuRegEx.Show(cmdExtChar.PointToScreen(cmdExtChar.ClientRectangle.Location));
            }
        }

        // Transforms the given text if necessary to convert e.g. "\r" to the corresponding character.
        private string SanitizeText(string text) {
            if (rdoExtended.Checked) {
                string transformed = text;
                char nullChar = (char)0;
                char cr = (char)13;
                char lf = (char)10;
                char tab = (char)9;

                transformed = transformed.Replace("\\r\\n", Environment.NewLine);
                transformed = transformed.Replace("\\r", cr.ToString());
                transformed = transformed.Replace("\\n", lf.ToString());
                transformed = transformed.Replace("\\t", tab.ToString());
                transformed = transformed.Replace("\\0", nullChar.ToString());

                return transformed;
            }
            else {
                return text;
            }
        }

        // Returns a search object representing the search query
        private Search GetQuery() {
            if (chkSearchSelection.Checked) {
                if (_searchRange.cpMin == _searchRange.cpMax) {
                    _searchRange = _manager.GetEditorSelectedRange();
                }
            }
            else {
                _searchRange = _manager.GetEditorWholeRange();
            }

            if (rdoRegex.Checked) {
                try {
                    return new RegexSearch(_searchRange, txtFind.Text, GetRegexOptions());
                }
                catch (ArgumentException ex) {
                    UpdateStatus("Error in Regular Expression: " + ex.Message);
                    return null;
                }
            }
            else {
                return new StringSearch(_searchRange, SanitizeText(txtFind.Text), chkMatchCase.Checked, chkWholeWord.Checked);
            }
        }

        // Use the dialog configuration to search for a match.
        private CharacterRange FindWrapper(bool searchUp) {
            if (searchUp) {
                return _manager.FindPrevious(GetQuery(), chkWrap.Checked);
            }
            else {
                return _manager.FindNext(GetQuery(), chkWrap.Checked);
            }
        }

        // Use the dialog configuration to search for all matches.
        private List<CharacterRange> FindAllWrapper() {
            var results = _manager.FindAll(GetQuery(), chkMarkLine.Checked, chkHighlightMatches.Checked);
            FindAllResults?.Invoke(this, new FindResultsEventArgs(_manager, results));
            return results;
        }

        // Use the dialog configuration to replace the first match.
        private CharacterRange ReplaceWrapper(bool searchUp) {
            return _manager.Replace(GetQuery(), SanitizeText(txtReplace.Text), chkWrap.Checked);
        }

        // Use the dialog configuration to replace all matches.
        private List<CharacterRange> ReplaceAllWrapper() {
            var results = _manager.ReplaceAll(GetQuery(), SanitizeText(txtReplace.Text), false, false);
            ReplaceAllResults?.Invoke(this, new ReplaceResultsEventArgs(_manager, results));
            return results;
        }

        // Process find/replace next/previous:
        // - Update history
        // - Run search and navigate to first result
        // - Update status text
        private void ProcessFindReplace(Func<bool, CharacterRange> findReplace, Action addMru, bool searchUp) {
            if (txtFind.Text == string.Empty) {
                return;
            }

            string statusText = _manager.RunFindReplace(findReplace, addMru, searchUp);
            MoveDialogAwayFromSelection();
            UpdateStatus(statusText);
        }

        // Process find/replace all:
        // - Update history
        // - Run search
        // - Update status text
        private void ProcessFindReplaceAll(Func<List<CharacterRange>> findReplaceAll, Action addMru, bool replace) {
            if (txtFind.Text == string.Empty) {
                return;
            }

            string statusText = _manager.RunFindReplaceAll(findReplaceAll, addMru, replace);
            UpdateStatus(statusText);
        }

        // Updates the status text
        internal void UpdateStatus(string text) {
            if (text != null) {
                lblStatus.Text = text;
                statusStrip.Refresh();
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// Event data for the find all event. 
    /// </summary>
    public class FindResultsEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="FindResultsEventArgs"/> instance.
        /// </summary>
        /// <param name="manager">Associated <see cref="FindReplace"/> control.</param>
        /// <param name="findAllResults"><see cref="List{CharacterRange}"/> containing the locations of the found results.</param>
        public FindResultsEventArgs(FindReplace manager, List<CharacterRange> findAllResults) {
            Manager = manager;
            FindAllResults = findAllResults;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="FindReplace"/> control.
        /// </summary>
        public FindReplace Manager { get; set; }

        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        public List<CharacterRange> FindAllResults { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Event data for the replace all event.
    /// </summary>
    public class ReplaceResultsEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="ReplaceResultsEventArgs"/> instance.
        /// </summary>
        /// <param name="manager">Associated <see cref="FindReplace"/> instance.</param>
        /// <param name="replaceAllResults"><see cref="List{CharacterRange}"/> containing the locations of the replacements.</param>
        public ReplaceResultsEventArgs(FindReplace manager, List<CharacterRange> replaceAllResults) {
            Manager = manager;
            ReplaceAllResults = replaceAllResults;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="FindReplace"/> object.
        /// </summary>
        public FindReplace Manager { get; set; }

        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        public List<CharacterRange> ReplaceAllResults { get; set; }

        #endregion Properties
    }
}