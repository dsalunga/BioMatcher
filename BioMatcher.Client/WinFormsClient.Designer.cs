namespace BioMatcher.Client
{
    partial class WinFormsClient
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
            this.ChatPanel = new System.Windows.Forms.Panel();
            this.cmdReset = new System.Windows.Forms.Button();
            this.txtQueue = new System.Windows.Forms.TextBox();
            this.cmdSendQueue = new System.Windows.Forms.Button();
            this.cmdSendTasks = new System.Windows.Forms.Button();
            this.SignInPanel = new System.Windows.Forms.Panel();
            this.StatusText = new System.Windows.Forms.Label();
            this.SignInButton = new System.Windows.Forms.Button();
            this.cmdGetStatus = new System.Windows.Forms.Button();
            this.ChatPanel.SuspendLayout();
            this.SignInPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RichTextBoxConsole
            // 
            this.RichTextBoxConsole.Location = new System.Drawing.Point(8, 12);
            this.RichTextBoxConsole.Name = "RichTextBoxConsole";
            this.RichTextBoxConsole.ReadOnly = true;
            this.RichTextBoxConsole.Size = new System.Drawing.Size(481, 460);
            this.RichTextBoxConsole.TabIndex = 3;
            this.RichTextBoxConsole.Text = "";
            // 
            // ChatPanel
            // 
            this.ChatPanel.Controls.Add(this.cmdGetStatus);
            this.ChatPanel.Controls.Add(this.cmdReset);
            this.ChatPanel.Controls.Add(this.txtQueue);
            this.ChatPanel.Controls.Add(this.cmdSendQueue);
            this.ChatPanel.Controls.Add(this.cmdSendTasks);
            this.ChatPanel.Controls.Add(this.RichTextBoxConsole);
            this.ChatPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatPanel.Location = new System.Drawing.Point(0, 0);
            this.ChatPanel.Name = "ChatPanel";
            this.ChatPanel.Size = new System.Drawing.Size(501, 513);
            this.ChatPanel.TabIndex = 4;
            this.ChatPanel.Visible = false;
            // 
            // cmdReset
            // 
            this.cmdReset.Location = new System.Drawing.Point(402, 478);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(87, 23);
            this.cmdReset.TabIndex = 8;
            this.cmdReset.Text = "Re-cache";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // txtQueue
            // 
            this.txtQueue.Location = new System.Drawing.Point(191, 481);
            this.txtQueue.Name = "txtQueue";
            this.txtQueue.Size = new System.Drawing.Size(41, 20);
            this.txtQueue.TabIndex = 7;
            this.txtQueue.Text = "10";
            // 
            // cmdSendQueue
            // 
            this.cmdSendQueue.Location = new System.Drawing.Point(110, 478);
            this.cmdSendQueue.Name = "cmdSendQueue";
            this.cmdSendQueue.Size = new System.Drawing.Size(75, 23);
            this.cmdSendQueue.TabIndex = 6;
            this.cmdSendQueue.Text = "Send Queue";
            this.cmdSendQueue.UseVisualStyleBackColor = true;
            this.cmdSendQueue.Click += new System.EventHandler(this.cmdSendQueue_Click);
            // 
            // cmdSendTasks
            // 
            this.cmdSendTasks.Location = new System.Drawing.Point(8, 478);
            this.cmdSendTasks.Name = "cmdSendTasks";
            this.cmdSendTasks.Size = new System.Drawing.Size(87, 23);
            this.cmdSendTasks.TabIndex = 5;
            this.cmdSendTasks.Text = "Send FP";
            this.cmdSendTasks.UseVisualStyleBackColor = true;
            this.cmdSendTasks.Click += new System.EventHandler(this.cmdSendTasks_Click);
            // 
            // SignInPanel
            // 
            this.SignInPanel.Controls.Add(this.StatusText);
            this.SignInPanel.Controls.Add(this.SignInButton);
            this.SignInPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignInPanel.Location = new System.Drawing.Point(0, 0);
            this.SignInPanel.Name = "SignInPanel";
            this.SignInPanel.Size = new System.Drawing.Size(501, 513);
            this.SignInPanel.TabIndex = 4;
            // 
            // StatusText
            // 
            this.StatusText.Location = new System.Drawing.Point(12, 59);
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(477, 13);
            this.StatusText.TabIndex = 6;
            this.StatusText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StatusText.Visible = false;
            // 
            // SignInButton
            // 
            this.SignInButton.Location = new System.Drawing.Point(12, 12);
            this.SignInButton.Name = "SignInButton";
            this.SignInButton.Size = new System.Drawing.Size(75, 23);
            this.SignInButton.TabIndex = 5;
            this.SignInButton.Text = "Connect";
            this.SignInButton.UseVisualStyleBackColor = true;
            this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // cmdGetStatus
            // 
            this.cmdGetStatus.Location = new System.Drawing.Point(321, 478);
            this.cmdGetStatus.Name = "cmdGetStatus";
            this.cmdGetStatus.Size = new System.Drawing.Size(75, 23);
            this.cmdGetStatus.TabIndex = 9;
            this.cmdGetStatus.Text = "Get Status";
            this.cmdGetStatus.UseVisualStyleBackColor = true;
            this.cmdGetStatus.Click += new System.EventHandler(this.cmdGetStatus_Click);
            // 
            // WinFormsClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 513);
            this.Controls.Add(this.ChatPanel);
            this.Controls.Add(this.SignInPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(517, 552);
            this.Name = "WinFormsClient";
            this.Text = "Fingerprint Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinFormsClient_FormClosing);
            this.ChatPanel.ResumeLayout(false);
            this.ChatPanel.PerformLayout();
            this.SignInPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxConsole;
        private System.Windows.Forms.Panel ChatPanel;
        private System.Windows.Forms.Panel SignInPanel;
        private System.Windows.Forms.Button SignInButton;
        private System.Windows.Forms.Label StatusText;
        private System.Windows.Forms.Button cmdSendTasks;
        private System.Windows.Forms.Button cmdSendQueue;
        private System.Windows.Forms.TextBox txtQueue;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.Button cmdGetStatus;

    }
}

