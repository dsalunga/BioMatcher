namespace BioMatcher.BenchMark
{
    partial class FormMain
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
            this.cmdInitialize = new System.Windows.Forms.Button();
            this.lblLogs = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtThreads = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdBenchmarkTask = new System.Windows.Forms.Button();
            this.chkBreakOnMarch = new System.Windows.Forms.CheckBox();
            this.cmdVerify = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.cmdBenchmarkSeed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdInitialize
            // 
            this.cmdInitialize.Location = new System.Drawing.Point(147, 12);
            this.cmdInitialize.Name = "cmdInitialize";
            this.cmdInitialize.Size = new System.Drawing.Size(75, 23);
            this.cmdInitialize.TabIndex = 0;
            this.cmdInitialize.Text = "Initialize";
            this.cmdInitialize.UseVisualStyleBackColor = true;
            this.cmdInitialize.Click += new System.EventHandler(this.cmdInitialize_Click);
            // 
            // lblLogs
            // 
            this.lblLogs.AutoSize = true;
            this.lblLogs.Location = new System.Drawing.Point(11, 90);
            this.lblLogs.Name = "lblLogs";
            this.lblLogs.Size = new System.Drawing.Size(33, 13);
            this.lblLogs.TabIndex = 2;
            this.lblLogs.Text = "Logs:";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(14, 106);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(439, 271);
            this.txtStatus.TabIndex = 3;
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(68, 15);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(73, 20);
            this.txtCount.TabIndex = 4;
            this.txtCount.Text = "10000";
            // 
            // txtThreads
            // 
            this.txtThreads.Location = new System.Drawing.Point(293, 44);
            this.txtThreads.Name = "txtThreads";
            this.txtThreads.Size = new System.Drawing.Size(44, 20);
            this.txtThreads.TabIndex = 6;
            this.txtThreads.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(238, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Threads:";
            // 
            // cmdBenchmarkTask
            // 
            this.cmdBenchmarkTask.Enabled = false;
            this.cmdBenchmarkTask.Location = new System.Drawing.Point(378, 8);
            this.cmdBenchmarkTask.Name = "cmdBenchmarkTask";
            this.cmdBenchmarkTask.Size = new System.Drawing.Size(75, 23);
            this.cmdBenchmarkTask.TabIndex = 8;
            this.cmdBenchmarkTask.Text = "Benchmark";
            this.cmdBenchmarkTask.UseVisualStyleBackColor = true;
            this.cmdBenchmarkTask.Click += new System.EventHandler(this.cmdBenchmarkTask_Click);
            // 
            // chkBreakOnMarch
            // 
            this.chkBreakOnMarch.AutoSize = true;
            this.chkBreakOnMarch.Location = new System.Drawing.Point(343, 46);
            this.chkBreakOnMarch.Name = "chkBreakOnMarch";
            this.chkBreakOnMarch.Size = new System.Drawing.Size(102, 17);
            this.chkBreakOnMarch.TabIndex = 9;
            this.chkBreakOnMarch.Text = "Break on Match";
            this.chkBreakOnMarch.UseVisualStyleBackColor = true;
            this.chkBreakOnMarch.CheckedChanged += new System.EventHandler(this.chkBreakOnMarch_CheckedChanged);
            // 
            // cmdVerify
            // 
            this.cmdVerify.Location = new System.Drawing.Point(378, 77);
            this.cmdVerify.Name = "cmdVerify";
            this.cmdVerify.Size = new System.Drawing.Size(75, 23);
            this.cmdVerify.TabIndex = 10;
            this.cmdVerify.Text = "Clear";
            this.cmdVerify.UseVisualStyleBackColor = true;
            this.cmdVerify.Click += new System.EventHandler(this.cmdVerify_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Samples:";
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(270, 81);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(109, 17);
            this.chkSave.TabIndex = 12;
            this.chkSave.Text = "Save Tempalates";
            this.chkSave.UseVisualStyleBackColor = true;
            // 
            // cmdBenchmarkSeed
            // 
            this.cmdBenchmarkSeed.Enabled = false;
            this.cmdBenchmarkSeed.Location = new System.Drawing.Point(259, 8);
            this.cmdBenchmarkSeed.Name = "cmdBenchmarkSeed";
            this.cmdBenchmarkSeed.Size = new System.Drawing.Size(113, 23);
            this.cmdBenchmarkSeed.TabIndex = 13;
            this.cmdBenchmarkSeed.Text = "Benchmark (Seed)";
            this.cmdBenchmarkSeed.UseVisualStyleBackColor = true;
            this.cmdBenchmarkSeed.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 389);
            this.Controls.Add(this.cmdBenchmarkSeed);
            this.Controls.Add(this.chkSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdVerify);
            this.Controls.Add(this.chkBreakOnMarch);
            this.Controls.Add(this.cmdBenchmarkTask);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtThreads);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblLogs);
            this.Controls.Add(this.cmdInitialize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "FingerPrint Benchmark";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdInitialize;
        private System.Windows.Forms.Label lblLogs;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.TextBox txtThreads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdBenchmarkTask;
        private System.Windows.Forms.CheckBox chkBreakOnMarch;
        private System.Windows.Forms.Button cmdVerify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.Button cmdBenchmarkSeed;
    }
}

