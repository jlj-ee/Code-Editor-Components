namespace ScintillaNET_Components
{
	partial class FindReplaceDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExNew;
            this.tabAll = new System.Windows.Forms.TabControl();
            this.tpgFind = new System.Windows.Forms.TabPage();
            this.grpFindAll = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.chkHighlightMatches = new System.Windows.Forms.CheckBox();
            this.chkMarkLine = new System.Windows.Forms.CheckBox();
            this.pnlSearchType = new System.Windows.Forms.Panel();
            this.chkWrap = new System.Windows.Forms.CheckBox();
            this.rdoStandard = new System.Windows.Forms.RadioButton();
            this.rdoRegex = new System.Windows.Forms.RadioButton();
            this.lblSearchType = new System.Windows.Forms.Label();
            this.rdoExtended = new System.Windows.Forms.RadioButton();
            this.chkSearchSelection = new System.Windows.Forms.CheckBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.pnlStandardOptions = new System.Windows.Forms.Panel();
            this.chkWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.pnlRegexpOptions = new System.Windows.Forms.Panel();
            this.chkSingleline = new System.Windows.Forms.CheckBox();
            this.chkMultiline = new System.Windows.Forms.CheckBox();
            this.chkIgnorePatternWhitespace = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.pnlFindNav = new System.Windows.Forms.Panel();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.pnlFind = new System.Windows.Forms.Panel();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.cmdRecentFind = new System.Windows.Forms.Button();
            this.cmdExtCharAndRegExFind = new System.Windows.Forms.Button();
            this.tpgReplace = new System.Windows.Forms.TabPage();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.pnlReplaceNav = new System.Windows.Forms.Panel();
            this.btnReplaceNext = new System.Windows.Forms.Button();
            this.pnlReplace = new System.Windows.Forms.Panel();
            this.lblReplace = new System.Windows.Forms.Label();
            this.cmdRecentReplace = new System.Windows.Forms.Button();
            this.cmdExtCharAndRegExReplace = new System.Windows.Forms.Button();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuExtendedChar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExtCharNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExtCharLF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExtCharCR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExtCharTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExtCharNull = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecent = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mnuRegExCharFind = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemRegExAnyCh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExDig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExWhite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExSet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRegExLF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExCR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExNull = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRegExAnyNum = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExOnePlus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExZeroPlus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRegExCap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegExNameCap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRegExStart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuRegExCharReplace = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemGroupEntire = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroup9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGroupNamed = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemRegExNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tabAll.SuspendLayout();
            this.tpgFind.SuspendLayout();
            this.grpFindAll.SuspendLayout();
            this.pnlSearchType.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.pnlStandardOptions.SuspendLayout();
            this.pnlRegexpOptions.SuspendLayout();
            this.pnlFindNav.SuspendLayout();
            this.pnlFind.SuspendLayout();
            this.tpgReplace.SuspendLayout();
            this.pnlReplaceNav.SuspendLayout();
            this.pnlReplace.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mnuExtendedChar.SuspendLayout();
            this.mnuRegExCharFind.SuspendLayout();
            this.mnuRegExCharReplace.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItemRegExNew
            // 
            toolStripMenuItemRegExNew.Name = "toolStripMenuItemRegExNew";
            toolStripMenuItemRegExNew.Size = new System.Drawing.Size(260, 22);
            toolStripMenuItemRegExNew.Tag = "\\r\\n";
            toolStripMenuItemRegExNew.Text = "\\r\\n Windows New Line";
            // 
            // tabAll
            // 
            this.tabAll.Controls.Add(this.tpgFind);
            this.tabAll.Controls.Add(this.tpgReplace);
            this.tabAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAll.Location = new System.Drawing.Point(0, 0);
            this.tabAll.Name = "tabAll";
            this.tabAll.SelectedIndex = 0;
            this.tabAll.Size = new System.Drawing.Size(489, 284);
            this.tabAll.TabIndex = 0;
            this.tabAll.TabStop = false;
            this.tabAll.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabAll_Selecting);
            // 
            // tpgFind
            // 
            this.tpgFind.Controls.Add(this.grpFindAll);
            this.tpgFind.Controls.Add(this.pnlSearchType);
            this.tpgFind.Controls.Add(this.grpOptions);
            this.tpgFind.Controls.Add(this.pnlFindNav);
            this.tpgFind.Controls.Add(this.pnlFind);
            this.tpgFind.Location = new System.Drawing.Point(4, 22);
            this.tpgFind.Name = "tpgFind";
            this.tpgFind.Padding = new System.Windows.Forms.Padding(3);
            this.tpgFind.Size = new System.Drawing.Size(481, 258);
            this.tpgFind.TabIndex = 0;
            this.tpgFind.Text = "Find";
            this.tpgFind.UseVisualStyleBackColor = true;
            // 
            // grpFindAll
            // 
            this.grpFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpFindAll.Controls.Add(this.btnClear);
            this.grpFindAll.Controls.Add(this.btnFindAll);
            this.grpFindAll.Controls.Add(this.chkHighlightMatches);
            this.grpFindAll.Controls.Add(this.chkMarkLine);
            this.grpFindAll.Location = new System.Drawing.Point(8, 188);
            this.grpFindAll.Name = "grpFindAll";
            this.grpFindAll.Size = new System.Drawing.Size(209, 65);
            this.grpFindAll.TabIndex = 5;
            this.grpFindAll.TabStop = false;
            this.grpFindAll.Text = "Find All";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(116, 37);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "C&lear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Location = new System.Drawing.Point(116, 13);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(88, 23);
            this.btnFindAll.TabIndex = 2;
            this.btnFindAll.Text = "Find &All";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.BtnFindAll_Click);
            // 
            // chkHighlightMatches
            // 
            this.chkHighlightMatches.AutoSize = true;
            this.chkHighlightMatches.Checked = true;
            this.chkHighlightMatches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHighlightMatches.Location = new System.Drawing.Point(6, 37);
            this.chkHighlightMatches.Name = "chkHighlightMatches";
            this.chkHighlightMatches.Size = new System.Drawing.Size(110, 17);
            this.chkHighlightMatches.TabIndex = 1;
            this.chkHighlightMatches.Text = "&Highlight Matches";
            this.chkHighlightMatches.UseVisualStyleBackColor = true;
            this.chkHighlightMatches.CheckedChanged += new System.EventHandler(this.ChkMarkHighlight_CheckedChanged);
            // 
            // chkMarkLine
            // 
            this.chkMarkLine.AutoSize = true;
            this.chkMarkLine.Location = new System.Drawing.Point(6, 20);
            this.chkMarkLine.Name = "chkMarkLine";
            this.chkMarkLine.Size = new System.Drawing.Size(71, 17);
            this.chkMarkLine.TabIndex = 0;
            this.chkMarkLine.Text = "&Mark Line";
            this.chkMarkLine.UseVisualStyleBackColor = true;
            this.chkMarkLine.CheckedChanged += new System.EventHandler(this.ChkMarkHighlight_CheckedChanged);
            // 
            // pnlSearchType
            // 
            this.pnlSearchType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearchType.BackColor = System.Drawing.Color.White;
            this.pnlSearchType.Controls.Add(this.chkWrap);
            this.pnlSearchType.Controls.Add(this.rdoStandard);
            this.pnlSearchType.Controls.Add(this.rdoRegex);
            this.pnlSearchType.Controls.Add(this.lblSearchType);
            this.pnlSearchType.Controls.Add(this.rdoExtended);
            this.pnlSearchType.Controls.Add(this.chkSearchSelection);
            this.pnlSearchType.Location = new System.Drawing.Point(0, 59);
            this.pnlSearchType.Name = "pnlSearchType";
            this.pnlSearchType.Size = new System.Drawing.Size(480, 42);
            this.pnlSearchType.TabIndex = 6;
            // 
            // chkWrap
            // 
            this.chkWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWrap.AutoSize = true;
            this.chkWrap.Checked = true;
            this.chkWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrap.Location = new System.Drawing.Point(367, 2);
            this.chkWrap.Name = "chkWrap";
            this.chkWrap.Size = new System.Drawing.Size(52, 17);
            this.chkWrap.TabIndex = 3;
            this.chkWrap.Text = "&Wrap";
            this.chkWrap.UseVisualStyleBackColor = true;
            // 
            // rdoStandard
            // 
            this.rdoStandard.AutoSize = true;
            this.rdoStandard.Checked = true;
            this.rdoStandard.Location = new System.Drawing.Point(19, 21);
            this.rdoStandard.Name = "rdoStandard";
            this.rdoStandard.Size = new System.Drawing.Size(69, 17);
            this.rdoStandard.TabIndex = 0;
            this.rdoStandard.TabStop = true;
            this.rdoStandard.Text = "&Standard";
            this.rdoStandard.UseVisualStyleBackColor = true;
            this.rdoStandard.CheckedChanged += new System.EventHandler(this.RdoSearchType_CheckedChanged);
            // 
            // rdoRegex
            // 
            this.rdoRegex.AutoSize = true;
            this.rdoRegex.Location = new System.Drawing.Point(239, 21);
            this.rdoRegex.Name = "rdoRegex";
            this.rdoRegex.Size = new System.Drawing.Size(117, 17);
            this.rdoRegex.TabIndex = 2;
            this.rdoRegex.Text = "Regular &Expression";
            this.rdoRegex.UseVisualStyleBackColor = true;
            this.rdoRegex.CheckedChanged += new System.EventHandler(this.RdoSearchType_CheckedChanged);
            // 
            // lblSearchType
            // 
            this.lblSearchType.AutoSize = true;
            this.lblSearchType.Location = new System.Drawing.Point(3, 2);
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.Size = new System.Drawing.Size(67, 13);
            this.lblSearchType.TabIndex = 0;
            this.lblSearchType.Text = "Search Type";
            // 
            // rdoExtended
            // 
            this.rdoExtended.AutoSize = true;
            this.rdoExtended.Location = new System.Drawing.Point(94, 21);
            this.rdoExtended.Name = "rdoExtended";
            this.rdoExtended.Size = new System.Drawing.Size(139, 17);
            this.rdoExtended.TabIndex = 1;
            this.rdoExtended.Text = "E&xtended (\\n, \\r, \\t, \\0)";
            this.rdoExtended.UseVisualStyleBackColor = true;
            this.rdoExtended.CheckedChanged += new System.EventHandler(this.RdoSearchType_CheckedChanged);
            // 
            // chkSearchSelection
            // 
            this.chkSearchSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSearchSelection.AutoSize = true;
            this.chkSearchSelection.Location = new System.Drawing.Point(367, 21);
            this.chkSearchSelection.Name = "chkSearchSelection";
            this.chkSearchSelection.Size = new System.Drawing.Size(105, 17);
            this.chkSearchSelection.TabIndex = 4;
            this.chkSearchSelection.Text = "Search Selection";
            this.chkSearchSelection.UseVisualStyleBackColor = true;
            // 
            // grpOptions
            // 
            this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptions.BackColor = System.Drawing.Color.White;
            this.grpOptions.Controls.Add(this.pnlStandardOptions);
            this.grpOptions.Controls.Add(this.pnlRegexpOptions);
            this.grpOptions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpOptions.Location = new System.Drawing.Point(0, 103);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(481, 77);
            this.grpOptions.TabIndex = 7;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // pnlStandardOptions
            // 
            this.pnlStandardOptions.Controls.Add(this.chkWholeWord);
            this.pnlStandardOptions.Controls.Add(this.chkMatchCase);
            this.pnlStandardOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStandardOptions.Location = new System.Drawing.Point(3, 17);
            this.pnlStandardOptions.Name = "pnlStandardOptions";
            this.pnlStandardOptions.Size = new System.Drawing.Size(475, 57);
            this.pnlStandardOptions.TabIndex = 0;
            // 
            // chkWholeWord
            // 
            this.chkWholeWord.AutoSize = true;
            this.chkWholeWord.Location = new System.Drawing.Point(10, 26);
            this.chkWholeWord.Name = "chkWholeWord";
            this.chkWholeWord.Size = new System.Drawing.Size(85, 17);
            this.chkWholeWord.TabIndex = 1;
            this.chkWholeWord.Text = "Whole Wor&d";
            this.chkWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(10, 3);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 0;
            this.chkMatchCase.Text = "Match &Case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // pnlRegexpOptions
            // 
            this.pnlRegexpOptions.Controls.Add(this.chkSingleline);
            this.pnlRegexpOptions.Controls.Add(this.chkMultiline);
            this.pnlRegexpOptions.Controls.Add(this.chkIgnorePatternWhitespace);
            this.pnlRegexpOptions.Controls.Add(this.chkIgnoreCase);
            this.pnlRegexpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRegexpOptions.Location = new System.Drawing.Point(3, 17);
            this.pnlRegexpOptions.Name = "pnlRegexpOptions";
            this.pnlRegexpOptions.Size = new System.Drawing.Size(475, 57);
            this.pnlRegexpOptions.TabIndex = 1;
            // 
            // chkSingleline
            // 
            this.chkSingleline.AutoSize = true;
            this.chkSingleline.Location = new System.Drawing.Point(10, 26);
            this.chkSingleline.Name = "chkSingleline";
            this.chkSingleline.Size = new System.Drawing.Size(70, 17);
            this.chkSingleline.TabIndex = 10;
            this.chkSingleline.Text = "Singleline";
            this.chkSingleline.UseVisualStyleBackColor = true;
            // 
            // chkMultiline
            // 
            this.chkMultiline.AutoSize = true;
            this.chkMultiline.Location = new System.Drawing.Point(101, 26);
            this.chkMultiline.Name = "chkMultiline";
            this.chkMultiline.Size = new System.Drawing.Size(64, 17);
            this.chkMultiline.TabIndex = 11;
            this.chkMultiline.Text = "Multiline";
            this.chkMultiline.UseVisualStyleBackColor = true;
            // 
            // chkIgnorePatternWhitespace
            // 
            this.chkIgnorePatternWhitespace.AutoSize = true;
            this.chkIgnorePatternWhitespace.Location = new System.Drawing.Point(101, 3);
            this.chkIgnorePatternWhitespace.Name = "chkIgnorePatternWhitespace";
            this.chkIgnorePatternWhitespace.Size = new System.Drawing.Size(156, 17);
            this.chkIgnorePatternWhitespace.TabIndex = 9;
            this.chkIgnorePatternWhitespace.Text = "I&gnore Pattern Whitespace";
            this.chkIgnorePatternWhitespace.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Location = new System.Drawing.Point(10, 3);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(85, 17);
            this.chkIgnoreCase.TabIndex = 8;
            this.chkIgnoreCase.Text = "&Ignore Case";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // pnlFindNav
            // 
            this.pnlFindNav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFindNav.Controls.Add(this.btnFindPrevious);
            this.pnlFindNav.Controls.Add(this.btnFindNext);
            this.pnlFindNav.Location = new System.Drawing.Point(367, 194);
            this.pnlFindNav.Name = "pnlFindNav";
            this.pnlFindNav.Size = new System.Drawing.Size(110, 56);
            this.pnlFindNav.TabIndex = 3;
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindPrevious.Location = new System.Drawing.Point(2, 3);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(107, 23);
            this.btnFindPrevious.TabIndex = 2;
            this.btnFindPrevious.Text = "Find &Previous";
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.BtnFindPrevious_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(2, 31);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(107, 23);
            this.btnFindNext.TabIndex = 0;
            this.btnFindNext.Text = "Find &Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.BtnFindNext_Click);
            // 
            // pnlFind
            // 
            this.pnlFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFind.Controls.Add(this.txtFind);
            this.pnlFind.Controls.Add(this.lblFind);
            this.pnlFind.Controls.Add(this.cmdRecentFind);
            this.pnlFind.Controls.Add(this.cmdExtCharAndRegExFind);
            this.pnlFind.Location = new System.Drawing.Point(1, 0);
            this.pnlFind.Name = "pnlFind";
            this.pnlFind.Size = new System.Drawing.Size(479, 29);
            this.pnlFind.TabIndex = 0;
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Location = new System.Drawing.Point(59, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(370, 21);
            this.txtFind.TabIndex = 0;
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.BackColor = System.Drawing.Color.White;
            this.lblFind.Location = new System.Drawing.Point(26, 4);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(27, 13);
            this.lblFind.TabIndex = 6;
            this.lblFind.Text = "&Find";
            // 
            // cmdRecentFind
            // 
            this.cmdRecentFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRecentFind.BackgroundImage = global::ScintillaNET_Components.Properties.Resources.history;
            this.cmdRecentFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdRecentFind.Enabled = false;
            this.cmdRecentFind.Location = new System.Drawing.Point(430, 3);
            this.cmdRecentFind.Name = "cmdRecentFind";
            this.cmdRecentFind.Size = new System.Drawing.Size(23, 23);
            this.cmdRecentFind.TabIndex = 4;
            this.toolTip.SetToolTip(this.cmdRecentFind, "Show a list of recent search strings");
            this.cmdRecentFind.UseVisualStyleBackColor = true;
            this.cmdRecentFind.Click += new System.EventHandler(this.CmdRecentFind_Click);
            // 
            // cmdExtCharAndRegExFind
            // 
            this.cmdExtCharAndRegExFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExtCharAndRegExFind.Enabled = false;
            this.cmdExtCharAndRegExFind.Location = new System.Drawing.Point(453, 3);
            this.cmdExtCharAndRegExFind.Name = "cmdExtCharAndRegExFind";
            this.cmdExtCharAndRegExFind.Size = new System.Drawing.Size(23, 23);
            this.cmdExtCharAndRegExFind.TabIndex = 5;
            this.cmdExtCharAndRegExFind.Text = ">";
            this.toolTip.SetToolTip(this.cmdExtCharAndRegExFind, "Show a list of extended characters");
            this.cmdExtCharAndRegExFind.UseVisualStyleBackColor = true;
            this.cmdExtCharAndRegExFind.Click += new System.EventHandler(this.CmdExtendedCharFind_Click);
            // 
            // tpgReplace
            // 
            this.tpgReplace.Controls.Add(this.btnSwap);
            this.tpgReplace.Controls.Add(this.btnReplaceAll);
            this.tpgReplace.Controls.Add(this.pnlReplaceNav);
            this.tpgReplace.Controls.Add(this.pnlReplace);
            this.tpgReplace.Location = new System.Drawing.Point(4, 22);
            this.tpgReplace.Name = "tpgReplace";
            this.tpgReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tpgReplace.Size = new System.Drawing.Size(481, 258);
            this.tpgReplace.TabIndex = 1;
            this.tpgReplace.Text = "Replace";
            this.tpgReplace.UseVisualStyleBackColor = true;
            // 
            // btnSwap
            // 
            this.btnSwap.BackgroundImage = global::ScintillaNET_Components.Properties.Resources.swap;
            this.btnSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSwap.ForeColor = System.Drawing.Color.Transparent;
            this.btnSwap.Location = new System.Drawing.Point(26, 19);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(24, 24);
            this.btnSwap.TabIndex = 7;
            this.btnSwap.UseVisualStyleBackColor = false;
            this.btnSwap.Click += new System.EventHandler(this.BtnSwap_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReplaceAll.Location = new System.Drawing.Point(8, 225);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(107, 23);
            this.btnReplaceAll.TabIndex = 5;
            this.btnReplaceAll.Text = "Replace &All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.BtnReplaceAll_Click);
            // 
            // pnlReplaceNav
            // 
            this.pnlReplaceNav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReplaceNav.Controls.Add(this.btnReplaceNext);
            this.pnlReplaceNav.Location = new System.Drawing.Point(253, 194);
            this.pnlReplaceNav.Name = "pnlReplaceNav";
            this.pnlReplaceNav.Size = new System.Drawing.Size(110, 56);
            this.pnlReplaceNav.TabIndex = 6;
            // 
            // btnReplaceNext
            // 
            this.btnReplaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplaceNext.Location = new System.Drawing.Point(1, 30);
            this.btnReplaceNext.Name = "btnReplaceNext";
            this.btnReplaceNext.Size = new System.Drawing.Size(107, 23);
            this.btnReplaceNext.TabIndex = 0;
            this.btnReplaceNext.Text = "&Replace";
            this.btnReplaceNext.UseVisualStyleBackColor = true;
            this.btnReplaceNext.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // pnlReplace
            // 
            this.pnlReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReplace.Controls.Add(this.lblReplace);
            this.pnlReplace.Controls.Add(this.cmdRecentReplace);
            this.pnlReplace.Controls.Add(this.cmdExtCharAndRegExReplace);
            this.pnlReplace.Controls.Add(this.txtReplace);
            this.pnlReplace.Location = new System.Drawing.Point(1, 31);
            this.pnlReplace.Name = "pnlReplace";
            this.pnlReplace.Size = new System.Drawing.Size(479, 29);
            this.pnlReplace.TabIndex = 1;
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(8, 12);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(45, 13);
            this.lblReplace.TabIndex = 5;
            this.lblReplace.Text = "&Replace";
            // 
            // cmdRecentReplace
            // 
            this.cmdRecentReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRecentReplace.BackgroundImage = global::ScintillaNET_Components.Properties.Resources.history;
            this.cmdRecentReplace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdRecentReplace.Enabled = false;
            this.cmdRecentReplace.Location = new System.Drawing.Point(430, 3);
            this.cmdRecentReplace.Name = "cmdRecentReplace";
            this.cmdRecentReplace.Size = new System.Drawing.Size(23, 23);
            this.cmdRecentReplace.TabIndex = 3;
            this.toolTip.SetToolTip(this.cmdRecentReplace, "Show a list of recent search strings");
            this.cmdRecentReplace.UseVisualStyleBackColor = true;
            this.cmdRecentReplace.Click += new System.EventHandler(this.CmdRecentReplace_Click);
            // 
            // cmdExtCharAndRegExReplace
            // 
            this.cmdExtCharAndRegExReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExtCharAndRegExReplace.Enabled = false;
            this.cmdExtCharAndRegExReplace.Location = new System.Drawing.Point(453, 3);
            this.cmdExtCharAndRegExReplace.Name = "cmdExtCharAndRegExReplace";
            this.cmdExtCharAndRegExReplace.Size = new System.Drawing.Size(23, 23);
            this.cmdExtCharAndRegExReplace.TabIndex = 4;
            this.cmdExtCharAndRegExReplace.Text = ">";
            this.toolTip.SetToolTip(this.cmdExtCharAndRegExReplace, "Show a list of extended characters");
            this.cmdExtCharAndRegExReplace.UseVisualStyleBackColor = true;
            this.cmdExtCharAndRegExReplace.Click += new System.EventHandler(this.CmdExtendedCharReplace_Click);
            // 
            // txtReplace
            // 
            this.txtReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReplace.Location = new System.Drawing.Point(59, 4);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(370, 21);
            this.txtReplace.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 284);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(489, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // mnuExtendedChar
            // 
            this.mnuExtendedChar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExtCharNew,
            this.toolStripMenuItemExtCharLF,
            this.toolStripMenuItemExtCharCR,
            this.toolStripMenuItemExtCharTab,
            this.toolStripMenuItemExtCharNull});
            this.mnuExtendedChar.Name = "mnuExtendedChar";
            this.mnuExtendedChar.ShowImageMargin = false;
            this.mnuExtendedChar.Size = new System.Drawing.Size(175, 114);
            this.mnuExtendedChar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MnuExtRegExChar_ItemClicked);
            // 
            // toolStripMenuItemExtCharNew
            // 
            this.toolStripMenuItemExtCharNew.Name = "toolStripMenuItemExtCharNew";
            this.toolStripMenuItemExtCharNew.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemExtCharNew.Tag = "\\r\\n";
            this.toolStripMenuItemExtCharNew.Text = "\\r\\n Windows New Line";
            // 
            // toolStripMenuItemExtCharLF
            // 
            this.toolStripMenuItemExtCharLF.Name = "toolStripMenuItemExtCharLF";
            this.toolStripMenuItemExtCharLF.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemExtCharLF.Tag = "\\n";
            this.toolStripMenuItemExtCharLF.Text = "\\n Line Feed";
            // 
            // toolStripMenuItemExtCharCR
            // 
            this.toolStripMenuItemExtCharCR.Name = "toolStripMenuItemExtCharCR";
            this.toolStripMenuItemExtCharCR.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemExtCharCR.Tag = "\\r";
            this.toolStripMenuItemExtCharCR.Text = "\\r Carriage Return";
            // 
            // toolStripMenuItemExtCharTab
            // 
            this.toolStripMenuItemExtCharTab.Name = "toolStripMenuItemExtCharTab";
            this.toolStripMenuItemExtCharTab.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemExtCharTab.Tag = "\\t";
            this.toolStripMenuItemExtCharTab.Text = "\\t Tab";
            // 
            // toolStripMenuItemExtCharNull
            // 
            this.toolStripMenuItemExtCharNull.Name = "toolStripMenuItemExtCharNull";
            this.toolStripMenuItemExtCharNull.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemExtCharNull.Tag = "\\0";
            this.toolStripMenuItemExtCharNull.Text = "\\0 Null Character";
            // 
            // mnuRecent
            // 
            this.mnuRecent.Name = "mnuRecentFindF";
            this.mnuRecent.ShowImageMargin = false;
            this.mnuRecent.Size = new System.Drawing.Size(36, 4);
            this.mnuRecent.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MnuRecent_ItemClicked);
            // 
            // mnuRegExCharFind
            // 
            this.mnuRegExCharFind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRegExAnyCh,
            this.toolStripMenuItemRegExAlpha,
            this.toolStripMenuItemRegExDig,
            this.toolStripMenuItemRegExWhite,
            this.toolStripMenuItemRegExSet,
            this.toolStripSeparator4,
            toolStripMenuItemRegExNew,
            this.toolStripMenuItemRegExLF,
            this.toolStripMenuItemRegExCR,
            this.toolStripMenuItemRegExTab,
            this.toolStripMenuItemRegExNull,
            this.toolStripSeparator1,
            this.toolStripMenuItemRegExAnyNum,
            this.toolStripMenuItemRegExOnePlus,
            this.toolStripMenuItemRegExZeroPlus,
            this.toolStripMenuItemFew,
            this.toolStripSeparator2,
            this.toolStripMenuItemRegExCap,
            this.toolStripMenuItemRegExNameCap,
            this.toolStripSeparator3,
            this.toolStripMenuItemRegExStart,
            this.toolStripMenuItemEnd});
            this.mnuRegExCharFind.Name = "mnuRegExChar";
            this.mnuRegExCharFind.ShowImageMargin = false;
            this.mnuRegExCharFind.Size = new System.Drawing.Size(261, 424);
            this.mnuRegExCharFind.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MnuExtRegExChar_ItemClicked);
            // 
            // toolStripMenuItemRegExAnyCh
            // 
            this.toolStripMenuItemRegExAnyCh.Name = "toolStripMenuItemRegExAnyCh";
            this.toolStripMenuItemRegExAnyCh.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExAnyCh.Tag = ".";
            this.toolStripMenuItemRegExAnyCh.Text = ". Any Character";
            // 
            // toolStripMenuItemRegExAlpha
            // 
            this.toolStripMenuItemRegExAlpha.Name = "toolStripMenuItemRegExAlpha";
            this.toolStripMenuItemRegExAlpha.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExAlpha.Tag = "\\w";
            this.toolStripMenuItemRegExAlpha.Text = "\\w Alphanumeric (\\W Non-Aplhat)";
            // 
            // toolStripMenuItemRegExDig
            // 
            this.toolStripMenuItemRegExDig.Name = "toolStripMenuItemRegExDig";
            this.toolStripMenuItemRegExDig.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExDig.Tag = "\\d";
            this.toolStripMenuItemRegExDig.Text = "\\d Digit (\\D Non-Digit)";
            // 
            // toolStripMenuItemRegExWhite
            // 
            this.toolStripMenuItemRegExWhite.Name = "toolStripMenuItemRegExWhite";
            this.toolStripMenuItemRegExWhite.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExWhite.Tag = "\\s";
            this.toolStripMenuItemRegExWhite.Text = "\\s Whitespace (\\S Non-Whitespace)";
            // 
            // toolStripMenuItemRegExSet
            // 
            this.toolStripMenuItemRegExSet.Name = "toolStripMenuItemRegExSet";
            this.toolStripMenuItemRegExSet.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExSet.Tag = "[a-zA-Z]";
            this.toolStripMenuItemRegExSet.Text = "[a-zA-Z] Specified Set ([^a...] Not in Set)";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItemRegExLF
            // 
            this.toolStripMenuItemRegExLF.Name = "toolStripMenuItemRegExLF";
            this.toolStripMenuItemRegExLF.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExLF.Tag = "\\n";
            this.toolStripMenuItemRegExLF.Text = "\\n Line Feed";
            // 
            // toolStripMenuItemRegExCR
            // 
            this.toolStripMenuItemRegExCR.Name = "toolStripMenuItemRegExCR";
            this.toolStripMenuItemRegExCR.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExCR.Tag = "\\r";
            this.toolStripMenuItemRegExCR.Text = "\\r Carriage Return";
            // 
            // toolStripMenuItemRegExTab
            // 
            this.toolStripMenuItemRegExTab.Name = "toolStripMenuItemRegExTab";
            this.toolStripMenuItemRegExTab.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExTab.Tag = "\\t";
            this.toolStripMenuItemRegExTab.Text = "\\t Tab";
            // 
            // toolStripMenuItemRegExNull
            // 
            this.toolStripMenuItemRegExNull.Name = "toolStripMenuItemRegExNull";
            this.toolStripMenuItemRegExNull.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExNull.Tag = "\\0";
            this.toolStripMenuItemRegExNull.Text = "\\0 Null Character";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItemRegExAnyNum
            // 
            this.toolStripMenuItemRegExAnyNum.Name = "toolStripMenuItemRegExAnyNum";
            this.toolStripMenuItemRegExAnyNum.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExAnyNum.Tag = "*";
            this.toolStripMenuItemRegExAnyNum.Text = "* Any Number of Repetitions";
            // 
            // toolStripMenuItemRegExOnePlus
            // 
            this.toolStripMenuItemRegExOnePlus.Name = "toolStripMenuItemRegExOnePlus";
            this.toolStripMenuItemRegExOnePlus.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExOnePlus.Tag = "+";
            this.toolStripMenuItemRegExOnePlus.Text = "+ One or More Repetitions";
            // 
            // toolStripMenuItemRegExZeroPlus
            // 
            this.toolStripMenuItemRegExZeroPlus.Name = "toolStripMenuItemRegExZeroPlus";
            this.toolStripMenuItemRegExZeroPlus.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExZeroPlus.Tag = "?";
            this.toolStripMenuItemRegExZeroPlus.Text = "? Zero or More Repetitions";
            // 
            // toolStripMenuItemFew
            // 
            this.toolStripMenuItemFew.Name = "toolStripMenuItemFew";
            this.toolStripMenuItemFew.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemFew.Tag = "?";
            this.toolStripMenuItemFew.Text = "? As Few as Possible (i.e. *?, +? or ??)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItemRegExCap
            // 
            this.toolStripMenuItemRegExCap.Name = "toolStripMenuItemRegExCap";
            this.toolStripMenuItemRegExCap.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExCap.Tag = "()";
            this.toolStripMenuItemRegExCap.Text = "( ) Numbered Capture";
            // 
            // toolStripMenuItemRegExNameCap
            // 
            this.toolStripMenuItemRegExNameCap.Name = "toolStripMenuItemRegExNameCap";
            this.toolStripMenuItemRegExNameCap.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExNameCap.Tag = "(?<Name>)";
            this.toolStripMenuItemRegExNameCap.Text = "(?<Name>) Named Capture";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItemRegExStart
            // 
            this.toolStripMenuItemRegExStart.Name = "toolStripMenuItemRegExStart";
            this.toolStripMenuItemRegExStart.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemRegExStart.Tag = "^";
            this.toolStripMenuItemRegExStart.Text = "^ Beginning of String or Line";
            // 
            // toolStripMenuItemEnd
            // 
            this.toolStripMenuItemEnd.Name = "toolStripMenuItemEnd";
            this.toolStripMenuItemEnd.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemEnd.Tag = "$";
            this.toolStripMenuItemEnd.Text = "$ End of String or Line";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // mnuRegExCharReplace
            // 
            this.mnuRegExCharReplace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGroupEntire,
            this.toolStripMenuItemGroup1,
            this.toolStripMenuItemGroup2,
            this.toolStripMenuItemGroup3,
            this.toolStripMenuItemGroup4,
            this.toolStripMenuItemGroup5,
            this.toolStripMenuItemGroup6,
            this.toolStripMenuItemGroup7,
            this.toolStripMenuItemGroup8,
            this.toolStripMenuItemGroup9,
            this.toolStripMenuItemGroupNamed});
            this.mnuRegExCharReplace.Name = "mnuRegExCharReplace";
            this.mnuRegExCharReplace.Size = new System.Drawing.Size(199, 246);
            this.mnuRegExCharReplace.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MnuExtRegExChar_ItemClicked);
            // 
            // toolStripMenuItemGroupEntire
            // 
            this.toolStripMenuItemGroupEntire.Name = "toolStripMenuItemGroupEntire";
            this.toolStripMenuItemGroupEntire.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroupEntire.Tag = "$&";
            this.toolStripMenuItemGroupEntire.Text = "$&& Entire Match";
            // 
            // toolStripMenuItemGroup1
            // 
            this.toolStripMenuItemGroup1.Name = "toolStripMenuItemGroup1";
            this.toolStripMenuItemGroup1.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup1.Tag = "$1";
            this.toolStripMenuItemGroup1.Text = "$1 Numbered Group #1";
            // 
            // toolStripMenuItemGroup2
            // 
            this.toolStripMenuItemGroup2.Name = "toolStripMenuItemGroup2";
            this.toolStripMenuItemGroup2.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup2.Tag = "$2";
            this.toolStripMenuItemGroup2.Text = "$2 Numbered Group #2";
            // 
            // toolStripMenuItemGroup3
            // 
            this.toolStripMenuItemGroup3.Name = "toolStripMenuItemGroup3";
            this.toolStripMenuItemGroup3.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup3.Tag = "$3";
            this.toolStripMenuItemGroup3.Text = "$3 Numbered Group #3";
            // 
            // toolStripMenuItemGroup4
            // 
            this.toolStripMenuItemGroup4.Name = "toolStripMenuItemGroup4";
            this.toolStripMenuItemGroup4.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup4.Tag = "$4";
            this.toolStripMenuItemGroup4.Text = "$4 Numbered Group #4";
            // 
            // toolStripMenuItemGroup5
            // 
            this.toolStripMenuItemGroup5.Name = "toolStripMenuItemGroup5";
            this.toolStripMenuItemGroup5.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup5.Tag = "$5";
            this.toolStripMenuItemGroup5.Text = "$5 Numbered Group #5";
            // 
            // toolStripMenuItemGroup6
            // 
            this.toolStripMenuItemGroup6.Name = "toolStripMenuItemGroup6";
            this.toolStripMenuItemGroup6.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup6.Tag = "$6";
            this.toolStripMenuItemGroup6.Text = "$6 Numbered Group #6";
            // 
            // toolStripMenuItemGroup7
            // 
            this.toolStripMenuItemGroup7.Name = "toolStripMenuItemGroup7";
            this.toolStripMenuItemGroup7.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup7.Tag = "$7";
            this.toolStripMenuItemGroup7.Text = "$7 Numbered Group #7";
            // 
            // toolStripMenuItemGroup8
            // 
            this.toolStripMenuItemGroup8.Name = "toolStripMenuItemGroup8";
            this.toolStripMenuItemGroup8.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup8.Tag = "$8";
            this.toolStripMenuItemGroup8.Text = "$8 Numbered Group #8";
            // 
            // toolStripMenuItemGroup9
            // 
            this.toolStripMenuItemGroup9.Name = "toolStripMenuItemGroup9";
            this.toolStripMenuItemGroup9.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroup9.Tag = "$9";
            this.toolStripMenuItemGroup9.Text = "$9 Numbered Group #9";
            // 
            // toolStripMenuItemGroupNamed
            // 
            this.toolStripMenuItemGroupNamed.Name = "toolStripMenuItemGroupNamed";
            this.toolStripMenuItemGroupNamed.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItemGroupNamed.Tag = "${Name}";
            this.toolStripMenuItemGroupNamed.Text = "${Name} Named Group";
            // 
            // FindReplaceDialog
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 306);
            this.Controls.Add(this.tabAll);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 345);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(505, 345);
            this.Name = "FindReplaceDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find";
            this.tabAll.ResumeLayout(false);
            this.tpgFind.ResumeLayout(false);
            this.grpFindAll.ResumeLayout(false);
            this.grpFindAll.PerformLayout();
            this.pnlSearchType.ResumeLayout(false);
            this.pnlSearchType.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.pnlStandardOptions.ResumeLayout(false);
            this.pnlStandardOptions.PerformLayout();
            this.pnlRegexpOptions.ResumeLayout(false);
            this.pnlRegexpOptions.PerformLayout();
            this.pnlFindNav.ResumeLayout(false);
            this.pnlFind.ResumeLayout(false);
            this.pnlFind.PerformLayout();
            this.tpgReplace.ResumeLayout(false);
            this.pnlReplaceNav.ResumeLayout(false);
            this.pnlReplace.ResumeLayout(false);
            this.pnlReplace.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mnuExtendedChar.ResumeLayout(false);
            this.mnuRegExCharFind.ResumeLayout(false);
            this.mnuRegExCharReplace.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabPage tpgFind;
		internal System.Windows.Forms.TabControl tabAll;
		private System.Windows.Forms.Label lblSearchType;
		private System.Windows.Forms.RadioButton rdoRegex;
		private System.Windows.Forms.RadioButton rdoStandard;
		private System.Windows.Forms.GroupBox grpOptions;
		private System.Windows.Forms.Panel pnlRegexpOptions;
		private System.Windows.Forms.Panel pnlStandardOptions;
		private System.Windows.Forms.CheckBox chkWholeWord;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.CheckBox chkMultiline;
		private System.Windows.Forms.CheckBox chkIgnorePatternWhitespace;
		private System.Windows.Forms.CheckBox chkIgnoreCase;
		private System.Windows.Forms.CheckBox chkSingleline;
		private System.Windows.Forms.Button btnFindPrevious;
		private System.Windows.Forms.Button btnFindNext;
		private System.Windows.Forms.CheckBox chkWrap;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnFindAll;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.GroupBox grpFindAll;
		internal System.Windows.Forms.CheckBox chkSearchSelection;
		private System.Windows.Forms.RadioButton rdoExtended;
		private System.Windows.Forms.Button cmdExtCharAndRegExFind;
		private System.Windows.Forms.ContextMenuStrip mnuExtendedChar;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtCharTab;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtCharLF;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtCharCR;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtCharNull;
		internal System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.Button cmdRecentFind;
		private System.Windows.Forms.ContextMenuStrip mnuRecent;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip mnuRegExCharFind;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtCharNew;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExAnyCh;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExAlpha;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExDig;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExWhite;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExSet;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExAnyNum;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExOnePlus;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExZeroPlus;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFew;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExCap;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExNameCap;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExStart;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEnd;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExLF;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExCR;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExTab;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegExNull;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ContextMenuStrip mnuRegExCharReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroupEntire;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroup9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGroupNamed;
        private System.Windows.Forms.Panel pnlSearchType;
        private System.Windows.Forms.TabPage tpgReplace;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Button cmdExtCharAndRegExReplace;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.Button btnReplaceNext;
        private System.Windows.Forms.Button cmdRecentReplace;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.Panel pnlReplace;
        private System.Windows.Forms.Panel pnlFind;
        private System.Windows.Forms.Panel pnlFindNav;
        private System.Windows.Forms.Panel pnlReplaceNav;
        private System.Windows.Forms.Button btnSwap;
        internal System.Windows.Forms.CheckBox chkHighlightMatches;
        internal System.Windows.Forms.CheckBox chkMarkLine;
    }
}