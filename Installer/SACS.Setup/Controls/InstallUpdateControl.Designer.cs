namespace SACS.Setup.Controls
{
    partial class InstallUpdateControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.ServerGroupBox = new System.Windows.Forms.GroupBox();
            this.ServerVersionLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ServerBrowseButton = new System.Windows.Forms.Button();
            this.ServerInstallButton = new System.Windows.Forms.Button();
            this.ServerCompleteLabel = new System.Windows.Forms.Label();
            this.ServerLocationTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerSkipCheckBox = new System.Windows.Forms.CheckBox();
            this.TooltipLabel = new System.Windows.Forms.Label();
            this.ServerFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.WindowsConsoleGroupBox = new System.Windows.Forms.GroupBox();
            this.WindowsIncludeStartCheckBox = new System.Windows.Forms.CheckBox();
            this.WindowsVersionLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.WindowsBrowseButton = new System.Windows.Forms.Button();
            this.WindowsInstallButton = new System.Windows.Forms.Button();
            this.WindowsCompleteLabel = new System.Windows.Forms.Label();
            this.WindowsLocationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WindowsSkipCheckBox = new System.Windows.Forms.CheckBox();
            this.WindowsFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ServerGroupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.WindowsConsoleGroupBox.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Install / Update SACS";
            // 
            // ServerGroupBox
            // 
            this.ServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerGroupBox.Controls.Add(this.ServerVersionLabel);
            this.ServerGroupBox.Controls.Add(this.flowLayoutPanel1);
            this.ServerGroupBox.Controls.Add(this.ServerLocationTextBox);
            this.ServerGroupBox.Controls.Add(this.label2);
            this.ServerGroupBox.Controls.Add(this.ServerSkipCheckBox);
            this.ServerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerGroupBox.Location = new System.Drawing.Point(7, 23);
            this.ServerGroupBox.Name = "ServerGroupBox";
            this.ServerGroupBox.Size = new System.Drawing.Size(557, 113);
            this.ServerGroupBox.TabIndex = 1;
            this.ServerGroupBox.TabStop = false;
            this.ServerGroupBox.Text = "Server and container";
            // 
            // ServerVersionLabel
            // 
            this.ServerVersionLabel.AutoSize = true;
            this.ServerVersionLabel.Location = new System.Drawing.Point(70, 49);
            this.ServerVersionLabel.Name = "ServerVersionLabel";
            this.ServerVersionLabel.Size = new System.Drawing.Size(104, 15);
            this.ServerVersionLabel.TabIndex = 4;
            this.ServerVersionLabel.Text = "Existing Version: -";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.ServerBrowseButton);
            this.flowLayoutPanel1.Controls.Add(this.ServerInstallButton);
            this.flowLayoutPanel1.Controls.Add(this.ServerCompleteLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(70, 67);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(481, 38);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // ServerBrowseButton
            // 
            this.ServerBrowseButton.Location = new System.Drawing.Point(3, 3);
            this.ServerBrowseButton.Name = "ServerBrowseButton";
            this.ServerBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.ServerBrowseButton.TabIndex = 0;
            this.ServerBrowseButton.Text = "Browse";
            this.ServerBrowseButton.UseVisualStyleBackColor = true;
            this.ServerBrowseButton.Click += new System.EventHandler(this.ServerBrowseButton_Click);
            this.ServerBrowseButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerBrowseButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServerInstallButton
            // 
            this.ServerInstallButton.Location = new System.Drawing.Point(84, 3);
            this.ServerInstallButton.Name = "ServerInstallButton";
            this.ServerInstallButton.Size = new System.Drawing.Size(97, 23);
            this.ServerInstallButton.TabIndex = 1;
            this.ServerInstallButton.Text = "Install Now";
            this.ServerInstallButton.UseVisualStyleBackColor = true;
            this.ServerInstallButton.Click += new System.EventHandler(this.ServerInstallButton_Click);
            this.ServerInstallButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerInstallButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServerCompleteLabel
            // 
            this.ServerCompleteLabel.AutoSize = true;
            this.ServerCompleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerCompleteLabel.Location = new System.Drawing.Point(187, 0);
            this.ServerCompleteLabel.Name = "ServerCompleteLabel";
            this.ServerCompleteLabel.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.ServerCompleteLabel.Size = new System.Drawing.Size(153, 23);
            this.ServerCompleteLabel.TabIndex = 2;
            this.ServerCompleteLabel.Text = "Installation Complete";
            this.ServerCompleteLabel.Visible = false;
            // 
            // ServerLocationTextBox
            // 
            this.ServerLocationTextBox.Location = new System.Drawing.Point(70, 25);
            this.ServerLocationTextBox.Name = "ServerLocationTextBox";
            this.ServerLocationTextBox.ReadOnly = true;
            this.ServerLocationTextBox.Size = new System.Drawing.Size(481, 21);
            this.ServerLocationTextBox.TabIndex = 2;
            this.ServerLocationTextBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerLocationTextBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location:";
            // 
            // ServerSkipCheckBox
            // 
            this.ServerSkipCheckBox.AutoSize = true;
            this.ServerSkipCheckBox.BackColor = System.Drawing.SystemColors.Window;
            this.ServerSkipCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerSkipCheckBox.Location = new System.Drawing.Point(450, 0);
            this.ServerSkipCheckBox.Name = "ServerSkipCheckBox";
            this.ServerSkipCheckBox.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ServerSkipCheckBox.Size = new System.Drawing.Size(101, 19);
            this.ServerSkipCheckBox.TabIndex = 0;
            this.ServerSkipCheckBox.Tag = "";
            this.ServerSkipCheckBox.Text = "Skip this part";
            this.ServerSkipCheckBox.UseVisualStyleBackColor = false;
            this.ServerSkipCheckBox.CheckedChanged += new System.EventHandler(this.SeverSkipCheckBox_CheckedChanged);
            this.ServerSkipCheckBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerSkipCheckBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // TooltipLabel
            // 
            this.TooltipLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TooltipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TooltipLabel.Location = new System.Drawing.Point(0, 347);
            this.TooltipLabel.Name = "TooltipLabel";
            this.TooltipLabel.Size = new System.Drawing.Size(567, 59);
            this.TooltipLabel.TabIndex = 2;
            this.TooltipLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // WindowsConsoleGroupBox
            // 
            this.WindowsConsoleGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsConsoleGroupBox.Controls.Add(this.WindowsIncludeStartCheckBox);
            this.WindowsConsoleGroupBox.Controls.Add(this.WindowsVersionLabel);
            this.WindowsConsoleGroupBox.Controls.Add(this.flowLayoutPanel2);
            this.WindowsConsoleGroupBox.Controls.Add(this.WindowsLocationTextBox);
            this.WindowsConsoleGroupBox.Controls.Add(this.label3);
            this.WindowsConsoleGroupBox.Controls.Add(this.WindowsSkipCheckBox);
            this.WindowsConsoleGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowsConsoleGroupBox.Location = new System.Drawing.Point(7, 142);
            this.WindowsConsoleGroupBox.Name = "WindowsConsoleGroupBox";
            this.WindowsConsoleGroupBox.Size = new System.Drawing.Size(557, 135);
            this.WindowsConsoleGroupBox.TabIndex = 3;
            this.WindowsConsoleGroupBox.TabStop = false;
            this.WindowsConsoleGroupBox.Text = "Windows Management Console";
            // 
            // WindowsIncludeStartCheckBox
            // 
            this.WindowsIncludeStartCheckBox.AutoSize = true;
            this.WindowsIncludeStartCheckBox.Checked = true;
            this.WindowsIncludeStartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WindowsIncludeStartCheckBox.Enabled = false;
            this.WindowsIncludeStartCheckBox.Location = new System.Drawing.Point(73, 65);
            this.WindowsIncludeStartCheckBox.Name = "WindowsIncludeStartCheckBox";
            this.WindowsIncludeStartCheckBox.Size = new System.Drawing.Size(173, 19);
            this.WindowsIncludeStartCheckBox.TabIndex = 11;
            this.WindowsIncludeStartCheckBox.Text = "Include start menu shortcut";
            this.WindowsIncludeStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // WindowsVersionLabel
            // 
            this.WindowsVersionLabel.AutoSize = true;
            this.WindowsVersionLabel.Location = new System.Drawing.Point(70, 49);
            this.WindowsVersionLabel.Name = "WindowsVersionLabel";
            this.WindowsVersionLabel.Size = new System.Drawing.Size(104, 15);
            this.WindowsVersionLabel.TabIndex = 10;
            this.WindowsVersionLabel.Text = "Existing Version: -";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.Controls.Add(this.WindowsBrowseButton);
            this.flowLayoutPanel2.Controls.Add(this.WindowsInstallButton);
            this.flowLayoutPanel2.Controls.Add(this.WindowsCompleteLabel);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(70, 88);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(481, 38);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // WindowsBrowseButton
            // 
            this.WindowsBrowseButton.Location = new System.Drawing.Point(3, 3);
            this.WindowsBrowseButton.Name = "WindowsBrowseButton";
            this.WindowsBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.WindowsBrowseButton.TabIndex = 0;
            this.WindowsBrowseButton.Text = "Browse";
            this.WindowsBrowseButton.UseVisualStyleBackColor = true;
            this.WindowsBrowseButton.Click += new System.EventHandler(this.WindowsBrowseButton_Click);
            this.WindowsBrowseButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.WindowsBrowseButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // WindowsInstallButton
            // 
            this.WindowsInstallButton.Location = new System.Drawing.Point(84, 3);
            this.WindowsInstallButton.Name = "WindowsInstallButton";
            this.WindowsInstallButton.Size = new System.Drawing.Size(97, 23);
            this.WindowsInstallButton.TabIndex = 1;
            this.WindowsInstallButton.Text = "Install Now";
            this.WindowsInstallButton.UseVisualStyleBackColor = true;
            this.WindowsInstallButton.Click += new System.EventHandler(this.WindowsInstallButton_Click);
            this.WindowsInstallButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.WindowsInstallButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // WindowsCompleteLabel
            // 
            this.WindowsCompleteLabel.AutoSize = true;
            this.WindowsCompleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowsCompleteLabel.Location = new System.Drawing.Point(187, 0);
            this.WindowsCompleteLabel.Name = "WindowsCompleteLabel";
            this.WindowsCompleteLabel.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.WindowsCompleteLabel.Size = new System.Drawing.Size(153, 23);
            this.WindowsCompleteLabel.TabIndex = 2;
            this.WindowsCompleteLabel.Text = "Installation Complete";
            this.WindowsCompleteLabel.Visible = false;
            // 
            // WindowsLocationTextBox
            // 
            this.WindowsLocationTextBox.Location = new System.Drawing.Point(70, 26);
            this.WindowsLocationTextBox.Name = "WindowsLocationTextBox";
            this.WindowsLocationTextBox.ReadOnly = true;
            this.WindowsLocationTextBox.Size = new System.Drawing.Size(481, 21);
            this.WindowsLocationTextBox.TabIndex = 8;
            this.WindowsLocationTextBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.WindowsLocationTextBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Location:";
            // 
            // WindowsSkipCheckBox
            // 
            this.WindowsSkipCheckBox.AutoSize = true;
            this.WindowsSkipCheckBox.BackColor = System.Drawing.SystemColors.Window;
            this.WindowsSkipCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowsSkipCheckBox.Location = new System.Drawing.Point(450, 1);
            this.WindowsSkipCheckBox.Name = "WindowsSkipCheckBox";
            this.WindowsSkipCheckBox.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.WindowsSkipCheckBox.Size = new System.Drawing.Size(101, 19);
            this.WindowsSkipCheckBox.TabIndex = 5;
            this.WindowsSkipCheckBox.Tag = "";
            this.WindowsSkipCheckBox.Text = "Skip this part";
            this.WindowsSkipCheckBox.UseVisualStyleBackColor = false;
            this.WindowsSkipCheckBox.CheckedChanged += new System.EventHandler(this.WindowsSkipCheckBox_CheckedChanged);
            this.WindowsSkipCheckBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.WindowsSkipCheckBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // InstallUpdateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WindowsConsoleGroupBox);
            this.Controls.Add(this.TooltipLabel);
            this.Controls.Add(this.ServerGroupBox);
            this.Controls.Add(this.label1);
            this.Name = "InstallUpdateControl";
            this.Size = new System.Drawing.Size(567, 406);
            this.VisibleChanged += new System.EventHandler(this.InstallUpdateControl_VisibleChanged);
            this.ServerGroupBox.ResumeLayout(false);
            this.ServerGroupBox.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.WindowsConsoleGroupBox.ResumeLayout(false);
            this.WindowsConsoleGroupBox.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox ServerGroupBox;
        private System.Windows.Forms.CheckBox ServerSkipCheckBox;
        private System.Windows.Forms.Label TooltipLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ServerBrowseButton;
        private System.Windows.Forms.TextBox ServerLocationTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog ServerFolderBrowserDialog;
        private System.Windows.Forms.Button ServerInstallButton;
        private System.Windows.Forms.Label ServerCompleteLabel;
        private System.Windows.Forms.Label ServerVersionLabel;
        private System.Windows.Forms.GroupBox WindowsConsoleGroupBox;
        private System.Windows.Forms.CheckBox WindowsSkipCheckBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button WindowsBrowseButton;
        private System.Windows.Forms.Button WindowsInstallButton;
        private System.Windows.Forms.Label WindowsCompleteLabel;
        private System.Windows.Forms.TextBox WindowsLocationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label WindowsVersionLabel;
        private System.Windows.Forms.FolderBrowserDialog WindowsFolderBrowserDialog;
        private System.Windows.Forms.CheckBox WindowsIncludeStartCheckBox;
    }
}
