﻿namespace SACS.Setup.Controls
{
    partial class ConfigureControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureControl));
            this.label1 = new System.Windows.Forms.Label();
            this.TooltipLabel = new System.Windows.Forms.Label();
            this.ServerErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ConfigureTabControl = new System.Windows.Forms.TabControl();
            this.ServerTabPage = new System.Windows.Forms.TabPage();
            this.DeployScriptsButton = new System.Windows.Forms.Button();
            this.DatabaseLocationTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.RefreshAccountButton = new System.Windows.Forms.Button();
            this.ServerApplyButton = new System.Windows.Forms.Button();
            this.ServerCancelButton = new System.Windows.Forms.Button();
            this.AppConfigLabel = new System.Windows.Forms.Label();
            this.ServerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ServiceAccountChangeButton = new System.Windows.Forms.Button();
            this.ServiceAccountLabel = new System.Windows.Forms.Label();
            this.ServiceAccountTextBox = new System.Windows.Forms.TextBox();
            this.WindowsTabPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.ServerErrorProvider)).BeginInit();
            this.ConfigureTabControl.SuspendLayout();
            this.ServerTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Configure SACS";
            // 
            // TooltipLabel
            // 
            this.TooltipLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TooltipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TooltipLabel.Location = new System.Drawing.Point(0, 411);
            this.TooltipLabel.Name = "TooltipLabel";
            this.TooltipLabel.Size = new System.Drawing.Size(584, 59);
            this.TooltipLabel.TabIndex = 5;
            this.TooltipLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ServerErrorProvider
            // 
            this.ServerErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ServerErrorProvider.ContainerControl = this;
            // 
            // ConfigureTabControl
            // 
            this.ConfigureTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfigureTabControl.Controls.Add(this.ServerTabPage);
            this.ConfigureTabControl.Controls.Add(this.WindowsTabPage);
            this.ConfigureTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigureTabControl.Location = new System.Drawing.Point(7, 23);
            this.ConfigureTabControl.Name = "ConfigureTabControl";
            this.ConfigureTabControl.SelectedIndex = 0;
            this.ConfigureTabControl.Size = new System.Drawing.Size(574, 385);
            this.ConfigureTabControl.TabIndex = 8;
            // 
            // ServerTabPage
            // 
            this.ServerTabPage.Controls.Add(this.DeployScriptsButton);
            this.ServerTabPage.Controls.Add(this.DatabaseLocationTextBox);
            this.ServerTabPage.Controls.Add(this.DatabaseLabel);
            this.ServerTabPage.Controls.Add(this.RefreshAccountButton);
            this.ServerTabPage.Controls.Add(this.ServerApplyButton);
            this.ServerTabPage.Controls.Add(this.ServerCancelButton);
            this.ServerTabPage.Controls.Add(this.AppConfigLabel);
            this.ServerTabPage.Controls.Add(this.ServerPropertyGrid);
            this.ServerTabPage.Controls.Add(this.ServiceAccountChangeButton);
            this.ServerTabPage.Controls.Add(this.ServiceAccountLabel);
            this.ServerTabPage.Controls.Add(this.ServiceAccountTextBox);
            this.ServerTabPage.Location = new System.Drawing.Point(4, 24);
            this.ServerTabPage.Name = "ServerTabPage";
            this.ServerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ServerTabPage.Size = new System.Drawing.Size(566, 357);
            this.ServerTabPage.TabIndex = 0;
            this.ServerTabPage.Text = "Server and container";
            this.ServerTabPage.UseVisualStyleBackColor = true;
            // 
            // DeployScriptsButton
            // 
            this.DeployScriptsButton.Location = new System.Drawing.Point(423, 38);
            this.DeployScriptsButton.Name = "DeployScriptsButton";
            this.DeployScriptsButton.Size = new System.Drawing.Size(138, 23);
            this.DeployScriptsButton.TabIndex = 10;
            this.DeployScriptsButton.Text = "Deploy Scripts";
            this.DeployScriptsButton.UseVisualStyleBackColor = true;
            this.DeployScriptsButton.Click += new System.EventHandler(this.DeployScriptsButton_Click);
            this.DeployScriptsButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.DeployScriptsButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // DatabaseLocationTextBox
            // 
            this.DatabaseLocationTextBox.Location = new System.Drawing.Point(120, 39);
            this.DatabaseLocationTextBox.Name = "DatabaseLocationTextBox";
            this.DatabaseLocationTextBox.ReadOnly = true;
            this.DatabaseLocationTextBox.Size = new System.Drawing.Size(296, 21);
            this.DatabaseLocationTextBox.TabIndex = 9;
            this.DatabaseLocationTextBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.DatabaseLocationTextBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(39, 42);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(63, 15);
            this.DatabaseLabel.TabIndex = 8;
            this.DatabaseLabel.Text = "Database:";
            this.DatabaseLabel.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.DatabaseLabel.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // RefreshAccountButton
            // 
            this.RefreshAccountButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshAccountButton.Image")));
            this.RefreshAccountButton.Location = new System.Drawing.Point(423, 11);
            this.RefreshAccountButton.Name = "RefreshAccountButton";
            this.RefreshAccountButton.Size = new System.Drawing.Size(24, 23);
            this.RefreshAccountButton.TabIndex = 7;
            this.RefreshAccountButton.UseVisualStyleBackColor = true;
            this.RefreshAccountButton.Click += new System.EventHandler(this.RefreshAccountButton_Click);
            this.RefreshAccountButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.RefreshAccountButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServerApplyButton
            // 
            this.ServerApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerApplyButton.Location = new System.Drawing.Point(404, 328);
            this.ServerApplyButton.Name = "ServerApplyButton";
            this.ServerApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ServerApplyButton.TabIndex = 6;
            this.ServerApplyButton.Text = "Apply";
            this.ServerApplyButton.UseVisualStyleBackColor = true;
            this.ServerApplyButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerApplyButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServerCancelButton
            // 
            this.ServerCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerCancelButton.Location = new System.Drawing.Point(485, 328);
            this.ServerCancelButton.Name = "ServerCancelButton";
            this.ServerCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ServerCancelButton.TabIndex = 5;
            this.ServerCancelButton.Text = "Cancel";
            this.ServerCancelButton.UseVisualStyleBackColor = true;
            this.ServerCancelButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServerCancelButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // AppConfigLabel
            // 
            this.AppConfigLabel.AutoSize = true;
            this.AppConfigLabel.Location = new System.Drawing.Point(35, 66);
            this.AppConfigLabel.Name = "AppConfigLabel";
            this.AppConfigLabel.Size = new System.Drawing.Size(67, 15);
            this.AppConfigLabel.TabIndex = 4;
            this.AppConfigLabel.Text = "App.config:";
            this.AppConfigLabel.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.AppConfigLabel.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServerPropertyGrid
            // 
            this.ServerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerPropertyGrid.Location = new System.Drawing.Point(120, 66);
            this.ServerPropertyGrid.Name = "ServerPropertyGrid";
            this.ServerPropertyGrid.Size = new System.Drawing.Size(440, 256);
            this.ServerPropertyGrid.TabIndex = 3;
            // 
            // ServiceAccountChangeButton
            // 
            this.ServiceAccountChangeButton.Location = new System.Drawing.Point(453, 11);
            this.ServiceAccountChangeButton.Name = "ServiceAccountChangeButton";
            this.ServiceAccountChangeButton.Size = new System.Drawing.Size(108, 23);
            this.ServiceAccountChangeButton.TabIndex = 2;
            this.ServiceAccountChangeButton.Text = "Open Services";
            this.ServiceAccountChangeButton.UseVisualStyleBackColor = true;
            this.ServiceAccountChangeButton.Click += new System.EventHandler(this.ServiceAccountChangeButton_Click);
            this.ServiceAccountChangeButton.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServiceAccountChangeButton.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServiceAccountLabel
            // 
            this.ServiceAccountLabel.AutoSize = true;
            this.ServiceAccountLabel.Location = new System.Drawing.Point(6, 15);
            this.ServiceAccountLabel.Name = "ServiceAccountLabel";
            this.ServiceAccountLabel.Size = new System.Drawing.Size(96, 15);
            this.ServiceAccountLabel.TabIndex = 0;
            this.ServiceAccountLabel.Text = "Service Account:";
            this.ServiceAccountLabel.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServiceAccountLabel.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // ServiceAccountTextBox
            // 
            this.ServiceAccountTextBox.Location = new System.Drawing.Point(120, 12);
            this.ServiceAccountTextBox.Name = "ServiceAccountTextBox";
            this.ServiceAccountTextBox.ReadOnly = true;
            this.ServiceAccountTextBox.Size = new System.Drawing.Size(296, 21);
            this.ServiceAccountTextBox.TabIndex = 1;
            this.ServiceAccountTextBox.MouseEnter += new System.EventHandler(this.Tooltip_MouseEnter);
            this.ServiceAccountTextBox.MouseLeave += new System.EventHandler(this.Tooltip_MouseLeave);
            // 
            // WindowsTabPage
            // 
            this.WindowsTabPage.Location = new System.Drawing.Point(4, 24);
            this.WindowsTabPage.Name = "WindowsTabPage";
            this.WindowsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.WindowsTabPage.Size = new System.Drawing.Size(601, 357);
            this.WindowsTabPage.TabIndex = 1;
            this.WindowsTabPage.Text = "Windows Console";
            this.WindowsTabPage.UseVisualStyleBackColor = true;
            // 
            // ConfigureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ConfigureTabControl);
            this.Controls.Add(this.TooltipLabel);
            this.Controls.Add(this.label1);
            this.Name = "ConfigureControl";
            this.Size = new System.Drawing.Size(584, 470);
            this.VisibleChanged += new System.EventHandler(this.ConfigureControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ServerErrorProvider)).EndInit();
            this.ConfigureTabControl.ResumeLayout(false);
            this.ServerTabPage.ResumeLayout(false);
            this.ServerTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TooltipLabel;
        private System.Windows.Forms.ErrorProvider ServerErrorProvider;
        private System.Windows.Forms.TabControl ConfigureTabControl;
        private System.Windows.Forms.TabPage ServerTabPage;
        private System.Windows.Forms.PropertyGrid ServerPropertyGrid;
        private System.Windows.Forms.Button ServiceAccountChangeButton;
        private System.Windows.Forms.Label ServiceAccountLabel;
        private System.Windows.Forms.TextBox ServiceAccountTextBox;
        private System.Windows.Forms.TabPage WindowsTabPage;
        private System.Windows.Forms.Label AppConfigLabel;
        private System.Windows.Forms.Button ServerApplyButton;
        private System.Windows.Forms.Button ServerCancelButton;
        private System.Windows.Forms.Button RefreshAccountButton;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.TextBox DatabaseLocationTextBox;
        private System.Windows.Forms.Button DeployScriptsButton;
    }
}
