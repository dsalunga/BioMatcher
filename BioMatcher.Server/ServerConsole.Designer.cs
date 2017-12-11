namespace BioMatcher.Server
{
    partial class ServerConsole
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
            this.RichTextBoxConsole = new System.Windows.Forms.RichTextBox();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.chkBreakOnMatch = new System.Windows.Forms.CheckBox();
            this.cmdUpdateCache = new System.Windows.Forms.Button();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // RichTextBoxConsole
            // 
            this.RichTextBoxConsole.Location = new System.Drawing.Point(12, 32);
            this.RichTextBoxConsole.Name = "RichTextBoxConsole";
            this.RichTextBoxConsole.ReadOnly = true;
            this.RichTextBoxConsole.Size = new System.Drawing.Size(423, 496);
            this.RichTextBoxConsole.TabIndex = 4;
            this.RichTextBoxConsole.Text = "";
            // 
            // ButtonStop
            // 
            this.ButtonStop.Enabled = false;
            this.ButtonStop.Location = new System.Drawing.Point(380, 3);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(55, 23);
            this.ButtonStop.TabIndex = 2;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(12, 3);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(91, 23);
            this.ButtonStart.TabIndex = 3;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // chkBreakOnMatch
            // 
            this.chkBreakOnMatch.AutoSize = true;
            this.chkBreakOnMatch.Checked = true;
            this.chkBreakOnMatch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBreakOnMatch.Location = new System.Drawing.Point(109, 7);
            this.chkBreakOnMatch.Name = "chkBreakOnMatch";
            this.chkBreakOnMatch.Size = new System.Drawing.Size(104, 17);
            this.chkBreakOnMatch.TabIndex = 5;
            this.chkBreakOnMatch.Text = "Break On Match";
            this.chkBreakOnMatch.UseVisualStyleBackColor = true;
            this.chkBreakOnMatch.CheckedChanged += new System.EventHandler(this.chkBreakOnMatch_CheckedChanged);
            // 
            // cmdUpdateCache
            // 
            this.cmdUpdateCache.Location = new System.Drawing.Point(283, 3);
            this.cmdUpdateCache.Name = "cmdUpdateCache";
            this.cmdUpdateCache.Size = new System.Drawing.Size(91, 23);
            this.cmdUpdateCache.TabIndex = 6;
            this.cmdUpdateCache.Text = "UpdateCache";
            this.cmdUpdateCache.UseVisualStyleBackColor = true;
            this.cmdUpdateCache.Click += new System.EventHandler(this.cmdUpdateCache_Click);
            // 
            // chkVerbose
            // 
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Checked = true;
            this.chkVerbose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVerbose.Location = new System.Drawing.Point(212, 7);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Size = new System.Drawing.Size(65, 17);
            this.chkVerbose.TabIndex = 7;
            this.chkVerbose.Text = "Verbose";
            this.chkVerbose.UseVisualStyleBackColor = true;
            this.chkVerbose.CheckedChanged += new System.EventHandler(this.chkVerbose_CheckedChanged);
            // 
            // ServerConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 540);
            this.Controls.Add(this.chkVerbose);
            this.Controls.Add(this.cmdUpdateCache);
            this.Controls.Add(this.chkBreakOnMatch);
            this.Controls.Add(this.RichTextBoxConsole);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonStart);
            this.Name = "ServerConsole";
            this.Text = "FingerprintServer";
            this.Load += new System.EventHandler(this.ServerConsole_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxConsole;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.CheckBox chkBreakOnMatch;
        private System.Windows.Forms.Button cmdUpdateCache;
        private System.Windows.Forms.CheckBox chkVerbose;
    }
}