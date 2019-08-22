namespace ScintillaNET_Components
{
    partial class IncrementalSearcher
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncrementalSearcher));
            this.txtFind = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.TableLayoutPanel();
            this.chkRegex = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.chkWholeWord = new System.Windows.Forms.CheckBox();
            this.chkWrap = new System.Windows.Forms.CheckBox();
            this.chkHighlightAll = new System.Windows.Forms.CheckBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.brnPrevious = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFind
            // 
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFind.Location = new System.Drawing.Point(136, 3);
            this.txtFind.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.txtFind.MaximumSize = new System.Drawing.Size(135, 20);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(135, 20);
            this.txtFind.TabIndex = 1;
            this.txtFind.TextChanged += new System.EventHandler(this.TxtFind_TextChanged);
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtFind_KeyDown);
            // 
            // panel
            // 
            this.panel.AutoSize = true;
            this.panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.ColumnCount = 9;
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel.Controls.Add(this.chkRegex, 0, 0);
            this.panel.Controls.Add(this.chkMatchCase, 1, 0);
            this.panel.Controls.Add(this.chkWholeWord, 2, 0);
            this.panel.Controls.Add(this.chkWrap, 3, 0);
            this.panel.Controls.Add(this.chkHighlightAll, 4, 0);
            this.panel.Controls.Add(this.txtFind, 5, 0);
            this.panel.Controls.Add(this.btnNext, 7, 0);
            this.panel.Controls.Add(this.brnPrevious, 6, 0);
            this.panel.Controls.Add(this.lblStatus, 8, 0);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.MinimumSize = new System.Drawing.Size(500, 24);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(3);
            this.panel.RowCount = 1;
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel.Size = new System.Drawing.Size(500, 28);
            this.panel.TabIndex = 4;
            // 
            // chkRegex
            // 
            this.chkRegex.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRegex.AutoSize = true;
            this.chkRegex.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRegex.FlatAppearance.BorderSize = 0;
            this.chkRegex.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.chkRegex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRegex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkRegex.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkRegex.Image = global::ScintillaNET_Components.Properties.Resources.regex;
            this.chkRegex.Location = new System.Drawing.Point(5, 3);
            this.chkRegex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.Size = new System.Drawing.Size(22, 20);
            this.chkRegex.TabIndex = 8;
            this.toolTip.SetToolTip(this.chkRegex, "Regular Expression");
            this.chkRegex.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkMatchCase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkMatchCase.FlatAppearance.BorderSize = 0;
            this.chkMatchCase.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.chkMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMatchCase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkMatchCase.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMatchCase.Image = ((System.Drawing.Image)(resources.GetObject("chkMatchCase.Image")));
            this.chkMatchCase.Location = new System.Drawing.Point(31, 3);
            this.chkMatchCase.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(22, 20);
            this.chkMatchCase.TabIndex = 7;
            this.toolTip.SetToolTip(this.chkMatchCase, "Case Sensitive");
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // chkWholeWord
            // 
            this.chkWholeWord.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWholeWord.AutoSize = true;
            this.chkWholeWord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkWholeWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWholeWord.FlatAppearance.BorderSize = 0;
            this.chkWholeWord.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.chkWholeWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWholeWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkWholeWord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWholeWord.Image = global::ScintillaNET_Components.Properties.Resources.whole_word;
            this.chkWholeWord.Location = new System.Drawing.Point(57, 3);
            this.chkWholeWord.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chkWholeWord.Name = "chkWholeWord";
            this.chkWholeWord.Size = new System.Drawing.Size(22, 20);
            this.chkWholeWord.TabIndex = 9;
            this.toolTip.SetToolTip(this.chkWholeWord, "Whole Word");
            this.chkWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkWrap
            // 
            this.chkWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWrap.AutoSize = true;
            this.chkWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkWrap.Checked = true;
            this.chkWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWrap.FlatAppearance.BorderSize = 0;
            this.chkWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.chkWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkWrap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkWrap.Image = global::ScintillaNET_Components.Properties.Resources.wrap;
            this.chkWrap.Location = new System.Drawing.Point(83, 3);
            this.chkWrap.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chkWrap.Name = "chkWrap";
            this.chkWrap.Size = new System.Drawing.Size(22, 20);
            this.chkWrap.TabIndex = 10;
            this.toolTip.SetToolTip(this.chkWrap, "Wrap");
            this.chkWrap.UseVisualStyleBackColor = true;
            // 
            // chkHighlightAll
            // 
            this.chkHighlightAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkHighlightAll.AutoSize = true;
            this.chkHighlightAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkHighlightAll.Checked = true;
            this.chkHighlightAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHighlightAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkHighlightAll.FlatAppearance.BorderSize = 0;
            this.chkHighlightAll.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.chkHighlightAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHighlightAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkHighlightAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkHighlightAll.Image = global::ScintillaNET_Components.Properties.Resources.marker;
            this.chkHighlightAll.Location = new System.Drawing.Point(109, 3);
            this.chkHighlightAll.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chkHighlightAll.Name = "chkHighlightAll";
            this.chkHighlightAll.Size = new System.Drawing.Size(22, 20);
            this.chkHighlightAll.TabIndex = 4;
            this.toolTip.SetToolTip(this.chkHighlightAll, "Highlight All Matches");
            this.chkHighlightAll.UseVisualStyleBackColor = true;
            this.chkHighlightAll.Click += new System.EventHandler(this.BtnHighlightAll_Click);
            // 
            // btnNext
            // 
            this.btnNext.AutoSize = true;
            this.btnNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnNext.Image = global::ScintillaNET_Components.Properties.Resources.next;
            this.btnNext.Location = new System.Drawing.Point(301, 3);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(22, 20);
            this.btnNext.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnNext, "Find Next");
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // brnPrevious
            // 
            this.brnPrevious.AutoSize = true;
            this.brnPrevious.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brnPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.brnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brnPrevious.FlatAppearance.BorderSize = 0;
            this.brnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.brnPrevious.Image = global::ScintillaNET_Components.Properties.Resources.previous;
            this.brnPrevious.Location = new System.Drawing.Point(275, 3);
            this.brnPrevious.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.brnPrevious.Name = "brnPrevious";
            this.brnPrevious.Size = new System.Drawing.Size(22, 20);
            this.brnPrevious.TabIndex = 3;
            this.toolTip.SetToolTip(this.brnPrevious, "Find Previous");
            this.brnPrevious.UseVisualStyleBackColor = true;
            this.brnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(330, 6);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblStatus.MinimumSize = new System.Drawing.Size(135, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatus.Size = new System.Drawing.Size(162, 14);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IncrementalSearcher
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "IncrementalSearcher";
            this.Size = new System.Drawing.Size(500, 28);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button brnPrevious;
        private System.Windows.Forms.TableLayoutPanel panel;
        private System.Windows.Forms.CheckBox chkHighlightAll;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblStatus;
        internal System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.CheckBox chkWrap;
        private System.Windows.Forms.CheckBox chkWholeWord;
        private System.Windows.Forms.CheckBox chkRegex;
        private System.Windows.Forms.CheckBox chkMatchCase;
    }
}
