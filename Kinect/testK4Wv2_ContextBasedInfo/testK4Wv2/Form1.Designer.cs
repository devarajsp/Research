namespace testK4Wv2
{
    partial class Form1
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
            this.lblLefthand = new System.Windows.Forms.Label();
            this.lblRighthand = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblGestureCommand = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLefthand
            // 
            this.lblLefthand.AutoSize = true;
            this.lblLefthand.BackColor = System.Drawing.Color.LightGray;
            this.lblLefthand.Location = new System.Drawing.Point(133, 14);
            this.lblLefthand.Name = "lblLefthand";
            this.lblLefthand.Size = new System.Drawing.Size(47, 13);
            this.lblLefthand.TabIndex = 1;
            this.lblLefthand.Text = "leftHand";
            // 
            // lblRighthand
            // 
            this.lblRighthand.AutoSize = true;
            this.lblRighthand.BackColor = System.Drawing.Color.Red;
            this.lblRighthand.Location = new System.Drawing.Point(535, 14);
            this.lblRighthand.Name = "lblRighthand";
            this.lblRighthand.Size = new System.Drawing.Size(53, 13);
            this.lblRighthand.TabIndex = 2;
            this.lblRighthand.Text = "rightHand";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(186, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 3;
            // 
            // lblGestureCommand
            // 
            this.lblGestureCommand.AutoSize = true;
            this.lblGestureCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.lblGestureCommand.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblGestureCommand.Location = new System.Drawing.Point(282, 14);
            this.lblGestureCommand.Name = "lblGestureCommand";
            this.lblGestureCommand.Size = new System.Drawing.Size(63, 13);
            this.lblGestureCommand.TabIndex = 5;
            this.lblGestureCommand.Text = "Command : ";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(397, 155);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(166, 105);
            this.webBrowser1.TabIndex = 6;
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(906, 2);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(74, 25);
            this.btnRecord.TabIndex = 7;
            this.btnRecord.Text = "Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(754, 2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(137, 27);
            this.btnHelp.TabIndex = 8;
            this.btnHelp.Text = "Start AutoHelp";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 504);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.lblGestureCommand);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblRighthand);
            this.Controls.Add(this.lblLefthand);
            this.Name = "Form1";
            this.Text = "Context Based Info";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLefthand;
        private System.Windows.Forms.Label lblRighthand;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblGestureCommand;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnHelp;
    }
}

