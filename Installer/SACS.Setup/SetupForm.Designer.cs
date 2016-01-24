namespace SACS.Setup
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.MainSetupTabControl = new System.Windows.Forms.TabControl();
            this.WelcomeTabPage = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.InstallationTabPage = new System.Windows.Forms.TabPage();
            this.UninstallTabPage = new System.Windows.Forms.TabPage();
            this.PerformInstallTabPage = new System.Windows.Forms.TabPage();
            this.ConfigureTabPage = new System.Windows.Forms.TabPage();
            this.ChecklistTabPage = new System.Windows.Forms.TabPage();
            this.CompleteTabPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.CompleteLabel = new System.Windows.Forms.Label();
            this.InstallationTypeLabel = new System.Windows.Forms.Label();
            this.InstallUpgradeLabel = new System.Windows.Forms.Label();
            this.ConfigureLabel = new System.Windows.Forms.Label();
            this.UninstallLabel = new System.Windows.Forms.Label();
            this.ReleaseNotesButton = new System.Windows.Forms.Button();
            this.MainNavigationButtons = new SACS.Setup.Controls.NavigationButtons();
            this.MainInstallationTypeControl = new SACS.Setup.Controls.InstallationTypeControl();
            this.MainInstallUpdateControl = new SACS.Setup.Controls.InstallUpdateControl();
            this.configureControl1 = new SACS.Setup.Controls.ConfigureControl();
            this.Checklists = new SACS.Setup.Controls.ChecklistControl();
            this.MainSetupTabControl.SuspendLayout();
            this.WelcomeTabPage.SuspendLayout();
            this.InstallationTabPage.SuspendLayout();
            this.PerformInstallTabPage.SuspendLayout();
            this.ConfigureTabPage.SuspendLayout();
            this.ChecklistTabPage.SuspendLayout();
            this.CompleteTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainSetupTabControl
            // 
            this.MainSetupTabControl.Controls.Add(this.WelcomeTabPage);
            this.MainSetupTabControl.Controls.Add(this.InstallationTabPage);
            this.MainSetupTabControl.Controls.Add(this.UninstallTabPage);
            this.MainSetupTabControl.Controls.Add(this.PerformInstallTabPage);
            this.MainSetupTabControl.Controls.Add(this.ConfigureTabPage);
            this.MainSetupTabControl.Controls.Add(this.ChecklistTabPage);
            this.MainSetupTabControl.Controls.Add(this.CompleteTabPage);
            this.MainSetupTabControl.Location = new System.Drawing.Point(167, 56);
            this.MainSetupTabControl.Name = "MainSetupTabControl";
            this.MainSetupTabControl.SelectedIndex = 0;
            this.MainSetupTabControl.Size = new System.Drawing.Size(605, 457);
            this.MainSetupTabControl.TabIndex = 0;
            this.MainSetupTabControl.TabStop = false;
            this.MainSetupTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.MainSetupTabControl_Selecting);
            this.MainSetupTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.MainSetupTabControl_Selected);
            // 
            // WelcomeTabPage
            // 
            this.WelcomeTabPage.Controls.Add(this.label6);
            this.WelcomeTabPage.Controls.Add(this.label4);
            this.WelcomeTabPage.Controls.Add(this.label3);
            this.WelcomeTabPage.Location = new System.Drawing.Point(4, 22);
            this.WelcomeTabPage.Name = "WelcomeTabPage";
            this.WelcomeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.WelcomeTabPage.Size = new System.Drawing.Size(597, 431);
            this.WelcomeTabPage.TabIndex = 0;
            this.WelcomeTabPage.Text = "Welcome";
            this.WelcomeTabPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 397);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(591, 31);
            this.label6.TabIndex = 3;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(465, 106);
            this.label4.TabIndex = 1;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(448, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Welcome to the SACS Setup and Configuration Wizard";
            // 
            // InstallationTabPage
            // 
            this.InstallationTabPage.Controls.Add(this.MainInstallationTypeControl);
            this.InstallationTabPage.Location = new System.Drawing.Point(4, 22);
            this.InstallationTabPage.Name = "InstallationTabPage";
            this.InstallationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.InstallationTabPage.Size = new System.Drawing.Size(597, 431);
            this.InstallationTabPage.TabIndex = 3;
            this.InstallationTabPage.Text = "Installation Type";
            this.InstallationTabPage.UseVisualStyleBackColor = true;
            // 
            // UninstallTabPage
            // 
            this.UninstallTabPage.Location = new System.Drawing.Point(4, 22);
            this.UninstallTabPage.Name = "UninstallTabPage";
            this.UninstallTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.UninstallTabPage.Size = new System.Drawing.Size(597, 431);
            this.UninstallTabPage.TabIndex = 5;
            this.UninstallTabPage.Text = "Uninstall";
            this.UninstallTabPage.UseVisualStyleBackColor = true;
            // 
            // PerformInstallTabPage
            // 
            this.PerformInstallTabPage.Controls.Add(this.MainInstallUpdateControl);
            this.PerformInstallTabPage.Location = new System.Drawing.Point(4, 22);
            this.PerformInstallTabPage.Name = "PerformInstallTabPage";
            this.PerformInstallTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PerformInstallTabPage.Size = new System.Drawing.Size(597, 431);
            this.PerformInstallTabPage.TabIndex = 4;
            this.PerformInstallTabPage.Text = "Install / Upgrade";
            this.PerformInstallTabPage.UseVisualStyleBackColor = true;
            // 
            // ConfigureTabPage
            // 
            this.ConfigureTabPage.Controls.Add(this.configureControl1);
            this.ConfigureTabPage.Location = new System.Drawing.Point(4, 22);
            this.ConfigureTabPage.Name = "ConfigureTabPage";
            this.ConfigureTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigureTabPage.Size = new System.Drawing.Size(597, 431);
            this.ConfigureTabPage.TabIndex = 6;
            this.ConfigureTabPage.Text = "Configure";
            this.ConfigureTabPage.UseVisualStyleBackColor = true;
            // 
            // ChecklistTabPage
            // 
            this.ChecklistTabPage.Controls.Add(this.Checklists);
            this.ChecklistTabPage.Location = new System.Drawing.Point(4, 22);
            this.ChecklistTabPage.Name = "ChecklistTabPage";
            this.ChecklistTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ChecklistTabPage.Size = new System.Drawing.Size(597, 431);
            this.ChecklistTabPage.TabIndex = 1;
            this.ChecklistTabPage.Text = "Checklist";
            this.ChecklistTabPage.UseVisualStyleBackColor = true;
            // 
            // CompleteTabPage
            // 
            this.CompleteTabPage.Controls.Add(this.ReleaseNotesButton);
            this.CompleteTabPage.Controls.Add(this.label9);
            this.CompleteTabPage.Controls.Add(this.label8);
            this.CompleteTabPage.Controls.Add(this.label7);
            this.CompleteTabPage.Controls.Add(this.label5);
            this.CompleteTabPage.Location = new System.Drawing.Point(4, 22);
            this.CompleteTabPage.Name = "CompleteTabPage";
            this.CompleteTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.CompleteTabPage.Size = new System.Drawing.Size(597, 431);
            this.CompleteTabPage.TabIndex = 2;
            this.CompleteTabPage.Text = "Complete";
            this.CompleteTabPage.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(578, 86);
            this.label9.TabIndex = 3;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Next Steps:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "SACS is now ready for use.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "All done!";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Setup";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(141)))), ((int)(((byte)(188)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 50);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(80, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "SACS";
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.WelcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WelcomeLabel.ForeColor = System.Drawing.Color.Black;
            this.WelcomeLabel.Location = new System.Drawing.Point(0, 78);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Padding = new System.Windows.Forms.Padding(15);
            this.WelcomeLabel.Size = new System.Drawing.Size(165, 46);
            this.WelcomeLabel.TabIndex = 4;
            this.WelcomeLabel.Text = "Welcome";
            // 
            // CompleteLabel
            // 
            this.CompleteLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.CompleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompleteLabel.ForeColor = System.Drawing.Color.Black;
            this.CompleteLabel.Location = new System.Drawing.Point(0, 308);
            this.CompleteLabel.Name = "CompleteLabel";
            this.CompleteLabel.Padding = new System.Windows.Forms.Padding(15);
            this.CompleteLabel.Size = new System.Drawing.Size(165, 46);
            this.CompleteLabel.TabIndex = 8;
            this.CompleteLabel.Text = "Complete";
            // 
            // InstallationTypeLabel
            // 
            this.InstallationTypeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.InstallationTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstallationTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.InstallationTypeLabel.Location = new System.Drawing.Point(0, 124);
            this.InstallationTypeLabel.Name = "InstallationTypeLabel";
            this.InstallationTypeLabel.Padding = new System.Windows.Forms.Padding(15);
            this.InstallationTypeLabel.Size = new System.Drawing.Size(165, 46);
            this.InstallationTypeLabel.TabIndex = 9;
            this.InstallationTypeLabel.Text = "Installation Type";
            // 
            // InstallUpgradeLabel
            // 
            this.InstallUpgradeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.InstallUpgradeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstallUpgradeLabel.ForeColor = System.Drawing.Color.Black;
            this.InstallUpgradeLabel.Location = new System.Drawing.Point(0, 216);
            this.InstallUpgradeLabel.Name = "InstallUpgradeLabel";
            this.InstallUpgradeLabel.Padding = new System.Windows.Forms.Padding(15);
            this.InstallUpgradeLabel.Size = new System.Drawing.Size(165, 46);
            this.InstallUpgradeLabel.TabIndex = 10;
            this.InstallUpgradeLabel.Text = "Install / Upgrade";
            // 
            // ConfigureLabel
            // 
            this.ConfigureLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ConfigureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigureLabel.ForeColor = System.Drawing.Color.Black;
            this.ConfigureLabel.Location = new System.Drawing.Point(0, 262);
            this.ConfigureLabel.Name = "ConfigureLabel";
            this.ConfigureLabel.Padding = new System.Windows.Forms.Padding(15);
            this.ConfigureLabel.Size = new System.Drawing.Size(165, 46);
            this.ConfigureLabel.TabIndex = 11;
            this.ConfigureLabel.Text = "Configure";
            // 
            // UninstallLabel
            // 
            this.UninstallLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.UninstallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UninstallLabel.ForeColor = System.Drawing.Color.Black;
            this.UninstallLabel.Location = new System.Drawing.Point(0, 170);
            this.UninstallLabel.Name = "UninstallLabel";
            this.UninstallLabel.Padding = new System.Windows.Forms.Padding(15);
            this.UninstallLabel.Size = new System.Drawing.Size(165, 46);
            this.UninstallLabel.TabIndex = 12;
            this.UninstallLabel.Text = "Uninstall";
            // 
            // ReleaseNotesButton
            // 
            this.ReleaseNotesButton.Location = new System.Drawing.Point(16, 233);
            this.ReleaseNotesButton.Name = "ReleaseNotesButton";
            this.ReleaseNotesButton.Size = new System.Drawing.Size(127, 23);
            this.ReleaseNotesButton.TabIndex = 4;
            this.ReleaseNotesButton.Text = "View Release Notes";
            this.ReleaseNotesButton.UseVisualStyleBackColor = true;
            this.ReleaseNotesButton.Click += new System.EventHandler(this.ReleaseNotesButton_Click);
            // 
            // MainNavigationButtons
            // 
            this.MainNavigationButtons.CanGoNext = true;
            this.MainNavigationButtons.Location = new System.Drawing.Point(521, 519);
            this.MainNavigationButtons.Name = "MainNavigationButtons";
            this.MainNavigationButtons.Position = SACS.Setup.Controls.NavigationPosition.Start;
            this.MainNavigationButtons.Size = new System.Drawing.Size(247, 31);
            this.MainNavigationButtons.TabIndex = 6;
            this.MainNavigationButtons.Cancel += new System.EventHandler(this.MainNavigationButtons_Cancel);
            this.MainNavigationButtons.Next += new System.ComponentModel.CancelEventHandler(this.MainNavigationButtons_Next);
            this.MainNavigationButtons.Back += new System.ComponentModel.CancelEventHandler(this.MainNavigationButtons_Back);
            // 
            // MainInstallationTypeControl
            // 
            this.MainInstallationTypeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainInstallationTypeControl.Location = new System.Drawing.Point(3, 3);
            this.MainInstallationTypeControl.Name = "MainInstallationTypeControl";
            this.MainInstallationTypeControl.Size = new System.Drawing.Size(591, 425);
            this.MainInstallationTypeControl.TabIndex = 0;
            // 
            // MainInstallUpdateControl
            // 
            this.MainInstallUpdateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainInstallUpdateControl.Location = new System.Drawing.Point(3, 3);
            this.MainInstallUpdateControl.Name = "MainInstallUpdateControl";
            this.MainInstallUpdateControl.ServerInstallCompleted = false;
            this.MainInstallUpdateControl.Size = new System.Drawing.Size(591, 425);
            this.MainInstallUpdateControl.TabIndex = 0;
            this.MainInstallUpdateControl.WindowsInstallCompleted = false;
            // 
            // configureControl1
            // 
            this.configureControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configureControl1.Location = new System.Drawing.Point(3, 3);
            this.configureControl1.Name = "configureControl1";
            this.configureControl1.Size = new System.Drawing.Size(591, 425);
            this.configureControl1.TabIndex = 0;
            // 
            // Checklists
            // 
            this.Checklists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Checklists.Location = new System.Drawing.Point(3, 3);
            this.Checklists.Name = "Checklists";
            this.Checklists.Size = new System.Drawing.Size(591, 425);
            this.Checklists.TabIndex = 0;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.UninstallLabel);
            this.Controls.Add(this.ConfigureLabel);
            this.Controls.Add(this.InstallUpgradeLabel);
            this.Controls.Add(this.InstallationTypeLabel);
            this.Controls.Add(this.CompleteLabel);
            this.Controls.Add(this.MainNavigationButtons);
            this.Controls.Add(this.WelcomeLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MainSetupTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "SetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.MainSetupTabControl.ResumeLayout(false);
            this.WelcomeTabPage.ResumeLayout(false);
            this.WelcomeTabPage.PerformLayout();
            this.InstallationTabPage.ResumeLayout(false);
            this.PerformInstallTabPage.ResumeLayout(false);
            this.ConfigureTabPage.ResumeLayout(false);
            this.ChecklistTabPage.ResumeLayout(false);
            this.CompleteTabPage.ResumeLayout(false);
            this.CompleteTabPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainSetupTabControl;
        private System.Windows.Forms.TabPage WelcomeTabPage;
        private System.Windows.Forms.TabPage ChecklistTabPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label WelcomeLabel;
        private Controls.NavigationButtons MainNavigationButtons;
        private System.Windows.Forms.TabPage CompleteTabPage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label CompleteLabel;
        private Controls.ChecklistControl Checklists;
        private System.Windows.Forms.TabPage InstallationTabPage;
        private System.Windows.Forms.Label InstallationTypeLabel;
        private System.Windows.Forms.TabPage PerformInstallTabPage;
        private System.Windows.Forms.Label InstallUpgradeLabel;
        private Controls.InstallUpdateControl MainInstallUpdateControl;
        private Controls.InstallationTypeControl MainInstallationTypeControl;
        private System.Windows.Forms.TabPage UninstallTabPage;
        private System.Windows.Forms.TabPage ConfigureTabPage;
        private System.Windows.Forms.Label ConfigureLabel;
        private System.Windows.Forms.Label UninstallLabel;
        private Controls.ConfigureControl configureControl1;
        private System.Windows.Forms.Button ReleaseNotesButton;
    }
}

