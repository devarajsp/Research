﻿namespace testK4Wv2
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
            this.button1 = new System.Windows.Forms.Button();
            this.lblLefthand = new System.Windows.Forms.Label();
            this.lblRighthand = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblGestureCommand = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start monitoring";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblLefthand
            // 
            this.lblLefthand.AutoSize = true;
            this.lblLefthand.BackColor = System.Drawing.Color.LightGray;
            this.lblLefthand.Location = new System.Drawing.Point(162, 14);
            this.lblLefthand.Name = "lblLefthand";
            this.lblLefthand.Size = new System.Drawing.Size(47, 13);
            this.lblLefthand.TabIndex = 1;
            this.lblLefthand.Text = "leftHand";
            // 
            // lblRighthand
            // 
            this.lblRighthand.AutoSize = true;
            this.lblRighthand.BackColor = System.Drawing.Color.Red;
            this.lblRighthand.Location = new System.Drawing.Point(693, 9);
            this.lblRighthand.Name = "lblRighthand";
            this.lblRighthand.Size = new System.Drawing.Size(53, 13);
            this.lblRighthand.TabIndex = 2;
            this.lblRighthand.Text = "rightHand";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(180, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 321);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
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
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(2, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 354);
            this.panel1.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(546, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 28);
            this.button2.TabIndex = 7;
            this.button2.Text = "ppt";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 504);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblGestureCommand);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblRighthand);
            this.Controls.Add(this.lblLefthand);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblLefthand;
        private System.Windows.Forms.Label lblRighthand;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblGestureCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
    }
}

