using CodeEditor_Components.SearchTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeEditor_Components
{
    /// <summary>
    /// Class to define the logic for a Find/Replace <see cref="Dialog"/>.
    /// </summary>
    public partial class FindReplaceDialog : Dialog
    {
        #region Fields

        private TextRange _searchRange;
        private Control _menuSource;
        private readonly FindReplace _manager;
        // Storage fields to be shared across tabs
        private string _findText;
        private bool _extended;
        private bool _regex;
        private bool _wrap;
        private bool _searchSelection;
        private bool _matchCase;
        private bool _wholeWord;
        private bool _ignoreCase;
        private bool _singleline;
        private bool _multiline;
        private bool _ignorePatternWhitespace;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="FindReplaceDialog"/>.
        /// </summary>
        /// <param name="manager"><see cref="FindReplace"/> instance that manages this <see cref="FindReplaceDialog"/>.</param>
        public FindReplaceDialog(FindReplace manager) : base() {
            InitializeComponent();
            _extended = rdoExtended.Checked;
            _regex = rdoRegex.Checked;
            _wrap = chkWrap.Checked;
            _searchSelection = chkSearchSelection.Checked;
            _matchCase = chkMatchCase.Checked;
            _wholeWord = chkWholeWord.Checked;
            _ignoreCase = chkIgnoreCase.Checked;
            _singleline = chkSingleline.Checked;
            _multiline = chkMultiline.Checked;
            _ignorePatternWhitespace = chkIgnorePatternWhitespace.Checked;
            Manager = _manager = manager;
        }

        #endregion Constructors

        #region Events & Handlers

        /// <summary>
        /// Triggered when a find all action is performed.
        /// </summary>
        public event EventHandler<FindResultsEventArgs> FindAllResults;

        /// <summary>
        /// Triggered when a replace all action is performed.
        /// </summary>
        public event EventHandler<ReplaceResultsEventArgs> ReplaceAllResults;

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
                chkSearchSelection_Replace.Enabled = true;
            }
            else {
                chkSearchSelection.Enabled = false;
                chkSearchSelection_Replace.Enabled = false;
                chkSearchSelection.Checked = false;
                chkSearchSelection_Replace.Checked = false;
            }

            // Clear old search range because it may be invalid
            _searchRange = new TextRange();

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
        /// <param name="e">Key event data.</param>
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
        /// <param name="e">Key event data.</param>
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
            var findString = txtFind_Replace.Text;
            txtFind_Replace.Text = txtReplace.Text;
            txtReplace.Text = findString;
        }

        // Handle wrap checkbox click.
        private void ChkWrap_CheckedChanged(object sender, EventArgs e) {
            _wrap = (sender as CheckBox).Checked;
        }

        // Handle selection checkbox click.
        private void ChkSearchSelection_CheckedChanged(object sender, EventArgs e) {
            _searchSelection = (sender as CheckBox).Checked;
        }

        // Handle match case checkbox click.
        private void ChkMatchCase_CheckedChanged(object sender, EventArgs e) {
            _matchCase = (sender as CheckBox).Checked;
        }

        // Handle whole word checkbox click.
        private void ChkWholeWord_CheckedChanged(object sender, EventArgs e) {
            _wholeWord = (sender as CheckBox).Checked;
        }

        // Handle multiline checkbox click.
        private void ChkMultiline_CheckedChanged(object sender, EventArgs e) {
            _multiline = (sender as CheckBox).Checked;
        }

        // Handle singeline checkbox click.
        private void ChkSingleline_CheckedChanged(object sender, EventArgs e) {
            _singleline = (sender as CheckBox).Checked;
        }

        // Handle ignore case checkbox click.
        private void ChkIgnoreCase_CheckedChanged(object sender, EventArgs e) {
            _ignoreCase = (sender as CheckBox).Checked;
        }

        // Handle ignore pattern whitespace checkbox click.
        private void ChkIgnorePatternWhitespace_CheckedChanged(object sender, EventArgs e) {
            _ignorePatternWhitespace = (sender as CheckBox).Checked;
        }

        // Handle highlight checkbox click.
        private void ChkHighlight_CheckedChanged(object sender, EventArgs e) {
            if (chkHighlightMatches.Checked) {
                _manager.Highlight(_manager.CurrentResults);
            }
            else {
                _manager.ClearAllHighlights();
            }
        }

        // Handle mark checkbox click.
        private void ChkMark_CheckedChanged(object sender, EventArgs e) {
            if (chkMarkLine.Checked) {
                _manager.Mark(_manager.CurrentResults);
            }
            else {
                _manager.ClearAllMarks();
            }
        }

        #endregion Button

        #region Menus

        // Handle find history menu button click
        private void CmdRecentFind_Click(object sender, EventArgs e) {
            ShowRecentMenu(sender as Button, _manager.FindHistory);
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
            else if (_menuSource == cmdRecentFind_Replace) {
                txtBox = txtFind_Replace;
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
                    if (_menuSource == cmdRecentFind || _menuSource == cmdRecentFind_Replace) {
                        cmdRecentFind.Enabled = false;
                        cmdRecentFind_Replace.Enabled = false;
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
            ShowExtRegexMenu(sender as Button, mnuRegExCharFind);
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
            else if (_menuSource == cmdExtCharAndRegExFind_Replace) {
                txtBox = txtFind_Replace;
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

        // Handle find text changed.
        private void TxtFind_TextChanged(object sender, EventArgs e) {
            _findText = (sender as TextBox).Text;
        }

        // Handle search type radio buttons changed.
        private void RdoSearchType_CheckedChanged(object sender, EventArgs e) {
            // Show the appropriate options panel
            if (rdoRegex.Checked) {
                _regex = true;
                _extended = false;
                pnlRegExOptions.BringToFront();
            }
            else if (rdoExtended.Checked) {
                _extended = true;
                _regex = false;
                pnlStandardOptions.BringToFront();
            }
            else if (rdoStandard.Checked) {
                _regex = _extended = false;
                pnlStandardOptions.BringToFront();
            }

            // Enable/disable extended/regex insertion menu
            cmdExtCharAndRegExFind.Enabled = !rdoStandard.Checked;
            cmdExtCharAndRegExReplace.Enabled = !rdoStandard.Checked;
        }

        // Handle search type radio buttons changed.
        private void RdoSearchTypeReplace_CheckedChanged(object sender, EventArgs e) {
            // Show the appropriate options panel
            if (rdoRegex_Replace.Checked) {
                _regex = true;
                _extended = false;
                pnlRegExOptions_Replace.BringToFront();
            }
            else if (rdoExtended_Replace.Checked) {
                _extended = true;
                _regex = false;
                pnlStandardOptions_Replace.BringToFront();
            }
            else if (rdoStandard_Replace.Checked) {
                _regex = _extended = false;
                pnlStandardOptions_Replace.BringToFront();
            }

            // Enable/disable extended/regex insertion menu
            cmdExtCharAndRegExFind_Replace.Enabled = !rdoStandard_Replace.Checked;
            cmdExtCharAndRegExReplace.Enabled = !rdoStandard_Replace.Checked;
        }

        // Handle find/replace tab changed.
        private void TabAll_Selecting(object sender, TabControlCancelEventArgs e) {
            // Update dialog title and data for shared-pupose controls
            if (e.TabPage == tpgFind) {
                Text = "Find";
                txtFind.Text = txtFind_Replace.Text;
                chkWrap.Checked = chkWrap_Replace.Checked;
                chkSearchSelection.Checked = chkSearchSelection_Replace.Checked;
                rdoExtended.Checked = rdoExtended_Replace.Checked;
                rdoStandard.Checked = rdoStandard_Replace.Checked;
                rdoRegex.Checked = rdoRegex_Replace.Checked;
                chkMatchCase.Checked = chkMatchCase_Replace.Checked;
                chkWholeWord.Checked = chkWholeWord_Replace.Checked;
                chkMultiline.Checked = chkMultiline_Replace.Checked;
                chkSingleline.Checked = chkSingleline_Replace.Checked;
                chkIgnoreCase.Checked = chkIgnoreCase_Replace.Checked;
                chkIgnorePatternWhitespace.Checked = chkIgnorePatternWhitespace_Replace.Checked;
                AcceptButton = btnFindNext;
            }
            else {
                Text = "Replace";
                txtFind_Replace.Text = txtFind.Text;
                chkWrap_Replace.Checked = chkWrap.Checked;
                chkSearchSelection_Replace.Checked = chkSearchSelection.Checked;
                rdoExtended_Replace.Checked = rdoExtended.Checked;
                rdoStandard_Replace.Checked = rdoStandard.Checked;
                rdoRegex_Replace.Checked = rdoRegex.Checked;
                chkMatchCase_Replace.Checked = chkMatchCase.Checked;
                chkWholeWord_Replace.Checked = chkWholeWord.Checked;
                chkMultiline_Replace.Checked = chkMultiline.Checked;
                chkSingleline_Replace.Checked = chkSingleline.Checked;
                chkIgnoreCase_Replace.Checked = chkIgnoreCase.Checked;
                chkIgnorePatternWhitespace_Replace.Checked = chkIgnorePatternWhitespace.Checked;
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

            if (_ignoreCase) {
                ro |= RegexOptions.IgnoreCase;
            }

            if (_ignorePatternWhitespace) {
                ro |= RegexOptions.IgnorePatternWhitespace;
            }

            if (_multiline) {
                ro |= RegexOptions.Multiline;
            }

            if (_singleline) {
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
            _manager.AddFindHistory(_findText);
            cmdRecentFind.Enabled = true;
            cmdRecentFind_Replace.Enabled = true;
        }

        // Adds the replace text to the replace history.
        private void AddReplaceHistory() {
            _manager.AddReplaceHistory(_findText, txtReplace.Text);
            cmdRecentFind.Enabled = true;
            cmdRecentFind_Replace.Enabled = true;
            cmdRecentReplace.Enabled = true;
        }

        // Gets the location of the pop-up menu relative to the given button.
        private Point GetMenuLocation(Button btn) {
            return new Point(btn.ClientRectangle.Location.X + btn.Width / 2, btn.ClientRectangle.Location.Y + btn.Height / 2);
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
                mnuRecent.Show(cmdRecent.PointToScreen(GetMenuLocation(cmdRecent)), ToolStripDropDownDirection.BelowRight);
            }
        }

        // Shows the extended/regular expression instertion menu.
        private void ShowExtRegexMenu(Button cmdExtChar, ContextMenuStrip mnuRegEx) {
            _menuSource = cmdExtChar;
            if (_extended) {
                mnuExtendedChar.Show(cmdExtChar.PointToScreen(GetMenuLocation(cmdExtChar)), ToolStripDropDownDirection.BelowRight);
            }
            else if (_regex) {
                mnuRegEx.Show(cmdExtChar.PointToScreen(GetMenuLocation(cmdExtChar)), ToolStripDropDownDirection.BelowRight);
            }
        }

        // Transforms the given text if necessary to convert e.g. "\r" to the corresponding character.
        private string SanitizeText(string text) {
            if (_extended) {
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
            if (_searchSelection) {
                if (_searchRange.Start == _searchRange.End) {
                    _searchRange = _manager.GetEditorSelectedRange();
                }
            }
            else {
                _searchRange = _manager.GetEditorWholeRange();
            }

            if (_regex) {
                try {
                    return new RegexSearch(_searchRange, _findText, GetRegexOptions());
                }
                catch (ArgumentException ex) {
                    UpdateStatus("Error in Regular Expression: " + ex.Message);
                    return null;
                }
            }
            else {
                return new StringSearch(_searchRange, SanitizeText(_findText), _matchCase, _wholeWord);
            }
        }

        // Use the dialog configuration to search for a match.
        private TextRange FindWrapper(bool searchUp) {
            if (searchUp) {
                return _manager.FindPrevious(GetQuery(), _wrap);
            }
            else {
                return _manager.FindNext(GetQuery(), _wrap);
            }
        }

        // Use the dialog configuration to search for all matches.
        private List<TextRange> FindAllWrapper() {
            var results = _manager.FindAll(GetQuery(), chkMarkLine.Checked, chkHighlightMatches.Checked);
            FindAllResults?.Invoke(this, new FindResultsEventArgs(_manager, results));
            return results;
        }

        // Use the dialog configuration to replace the first match.
        private TextRange ReplaceWrapper(bool searchUp) {
            return _manager.Replace(GetQuery(), SanitizeText(txtReplace.Text), _wrap);
        }

        // Use the dialog configuration to replace all matches.
        private List<TextRange> ReplaceAllWrapper() {
            var results = _manager.ReplaceAll(GetQuery(), SanitizeText(txtReplace.Text));
            ReplaceAllResults?.Invoke(this, new ReplaceResultsEventArgs(_manager, results));
            return results;
        }

        // Process find/replace next/previous:
        // - Update history
        // - Run search and navigate to first result
        // - Update status text
        private void ProcessFindReplace(Func<bool, TextRange> findReplace, Action addMru, bool searchUp) {
            if (_findText == string.Empty) {
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
        private void ProcessFindReplaceAll(Func<List<TextRange>> findReplaceAll, Action addMru, bool replace) {
            if (_findText == string.Empty) {
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

    #region Event Classes

    /// <summary>
    /// Event data for the find all event. 
    /// </summary>
    public class FindResultsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="FindReplace"/> control.
        /// </summary>
        public FindReplace Manager { get; set; }

        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        public List<TextRange> FindAllResults { get; set; }

        /// <summary>
        /// Creates a new <see cref="FindResultsEventArgs"/> instance.
        /// </summary>
        /// <param name="manager">Associated <see cref="FindReplace"/> control.</param>
        /// <param name="findAllResults"><see cref="List{CharacterRange}"/> containing the locations of the found results.</param>
        public FindResultsEventArgs(FindReplace manager, List<TextRange> findAllResults) {
            Manager = manager;
            FindAllResults = findAllResults;
        }
    }

    /// <summary>
    /// Event data for the replace all event.
    /// </summary>
    public class ReplaceResultsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="FindReplace"/> object.
        /// </summary>
        public FindReplace Manager { get; set; }

        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        public List<TextRange> ReplaceAllResults { get; set; }

        /// <summary>
        /// Creates a new <see cref="ReplaceResultsEventArgs"/> instance.
        /// </summary>
        /// <param name="manager">Associated <see cref="FindReplace"/> instance.</param>
        /// <param name="replaceAllResults"><see cref="List{CharacterRange}"/> containing the locations of the replacements.</param>
        public ReplaceResultsEventArgs(FindReplace manager, List<TextRange> replaceAllResults) {
            Manager = manager;
            ReplaceAllResults = replaceAllResults;
        }
    }

    #endregion Event Classes
}