namespace SACS.Setup.Controls
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
            this.WindowsTabPage = new System.Windows.Forms.TabPage();
            this.WindowsConfigurePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.WindowsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.WindowsApplyButton = new System.Windows.Forms.Button();
            this.WindowsCancelButton = new System.Windows.Forms.Button();
            this.ServerConfigurePanel = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.ServerErrorProvider)).BeginInit();
            this.ConfigureTabControl.SuspendLayout();
            this.ServerTabPage.SuspendLayout();
            this.WindowsTabPage.SuspendLayout();
            this.WindowsConfigurePanel.SuspendLayout();
            this.ServerConfigurePanel.SuspendLayout();
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
            this.ServerTabPage.Controls.Add(this.ServerConfigurePanel);
            this.ServerTabPage.Location = new System.Drawing.Point(4, 24);
            this.ServerTabPage.Name = "ServerTabPage";
            this.ServerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ServerTabPage.Size = new System.Drawing.Size(566, 357);
            this.ServerTabPage.TabIndex = 0;
            this.ServerTabPage.Text = "Server and container";
            this.ServerTabPage.UseVisualStyleBackColor = true;
            // 
            // WindowsTabPage
            // 
            this.WindowsTabPage.Controls.Add(this.WindowsConfigurePanel);
            this.WindowsTabPage.Location = new System.Drawing.Point(4, 24);
            this.WindowsTabPage.Name = "WindowsTabPage";
            this.WindowsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.WindowsTabPage.Size = new System.Drawing.Size(566, 357);
            this.WindowsTabPage.TabIndex = 1;
            this.WindowsTabPage.Text = "Windows Console";
            this.WindowsTabPage.UseVisualStyleBackColor = true;
            // 
            // WindowsConfigurePanel
            // 
            this.WindowsConfigurePanel.Controls.Add(this.label2);
            this.WindowsConfigurePanel.Controls.Add(this.WindowsPropertyGrid);
            this.WindowsConfigurePanel.Controls.Add(this.WindowsApplyButton);
            this.WindowsConfigurePanel.Controls.Add(this.WindowsCancelButton);
            this.WindowsConfigurePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowsConfigurePanel.Location = new System.Drawing.Point(3, 3);
            this.WindowsConfigurePanel.Name = "WindowsConfigurePanel";
            this.WindowsConfigurePanel.Size = new System.Drawing.Size(560, 351);
            this.WindowsConfigurePanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "App.config:";
            // 
            // WindowsPropertyGrid
            // 
            this.WindowsPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsPropertyGrid.Location = new System.Drawing.Point(117, 17);
            this.WindowsPropertyGrid.Name = "WindowsPropertyGrid";
            this.WindowsPropertyGrid.Size = new System.Drawing.Size(440, 302);
            this.WindowsPropertyGrid.TabIndex = 13;
            // 
            // WindowsApplyButton
            // 
            this.WindowsApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsApplyButton.Location = new System.Drawing.Point(401, 325);
            this.WindowsApplyButton.Name = "WindowsApplyButton";
            this.WindowsApplyButton.Size = new System.Drawing.Size(75, 23);
            this.WindowsApplyButton.TabIndex = 12;
            this.WindowsApplyButton.Text = "Apply";
            this.WindowsApplyButton.UseVisualStyleBackColor = true;
            // 
            // WindowsCancelButton
            // 
            this.WindowsCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsCancelButton.Location = new System.Drawing.Point(482, 325);
            this.WindowsCancelButton.Name = "WindowsCancelButton";
            this.WindowsCancelButton.Size = new System.Drawing.Size(75, 23);
            this.WindowsCancelButton.TabIndex = 11;
            this.WindowsCancelButton.Text = "Cancel";
            this.WindowsCancelButton.UseVisualStyleBackColor = true;
            // 
            // ServerConfigurePanel
            // 
            this.ServerConfigurePanel.Controls.Add(this.DeployScriptsButton);
            this.ServerConfigurePanel.Controls.Add(this.DatabaseLocationTextBox);
            this.ServerConfigurePanel.Controls.Add(this.DatabaseLabel);
            this.ServerConfigurePanel.Controls.Add(this.RefreshAccountButton);
            this.ServerConfigurePanel.Controls.Add(this.ServerApplyButton);
            this.ServerConfigurePanel.Controls.Add(this.ServerCancelButton);
            this.ServerConfigurePanel.Controls.Add(this.AppConfigLabel);
            this.ServerConfigurePanel.Controls.Add(this.ServerPropertyGrid);
            this.ServerConfigurePanel.Controls.Add(this.ServiceAccountChangeButton);
            this.ServerConfigurePanel.Controls.Add(this.ServiceAccountLabel);
            this.ServerConfigurePanel.Controls.Add(this.ServiceAccountTextBox);
            this.ServerConfigurePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerConfigurePanel.Location = new System.Drawing.Point(3, 3);
            this.ServerConfigurePanel.Name = "ServerConfigurePanel";
            this.ServerConfigurePanel.Size = new System.Drawing.Size(560, 351);
            this.ServerConfigurePanel.TabIndex = 0;
            // 
            // DeployScriptsButton
            // 
            this.DeployScriptsButton.Location = new System.Drawing.Point(420, 35);
            this.DeployScriptsButton.Name = "DeployScriptsButton";
            this.DeployScriptsButton.Size = new System.Drawing.Size(138, 23);
            this.DeployScriptsButton.TabIndex = 21;
            this.DeployScriptsButton.Text = "Deploy Scripts";
            this.DeployScriptsButton.UseVisualStyleBackColor = true;
            // 
            // DatabaseLocationTextBox
            // 
            this.DatabaseLocationTextBox.Location = new System.Drawing.Point(117, 36);
            this.DatabaseLocationTextBox.Name = "DatabaseLocationTextBox";
            this.DatabaseLocationTextBox.ReadOnly = true;
            this.DatabaseLocationTextBox.Size = new System.Drawing.Size(296, 21);
            this.DatabaseLocationTextBox.TabIndex = 20;
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(36, 39);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(63, 15);
            this.DatabaseLabel.TabIndex = 19;
            this.DatabaseLabel.Text = "Database:";
            // 
            // RefreshAccountButton
            // 
            this.RefreshAccountButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshAccountButton.Image")));
            this.RefreshAccountButton.Location = new System.Drawing.Point(420, 8);
            this.RefreshAccountButton.Name = "RefreshAccountButton";
            this.RefreshAccountButton.Size = new System.Drawing.Size(24, 23);
            this.RefreshAccountButton.TabIndex = 18;
            this.RefreshAccountButton.UseVisualStyleBackColor = true;
            // 
            // ServerApplyButton
            // 
            this.ServerApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerApplyButton.Location = new System.Drawing.Point(401, 325);
            this.ServerApplyButton.Name = "ServerApplyButton";
            this.ServerApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ServerApplyButton.TabIndex = 17;
            this.ServerApplyButton.Text = "Apply";
            this.ServerApplyButton.UseVisualStyleBackColor = true;
            // 
            // ServerCancelButton
            // 
            this.ServerCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerCancelButton.Location = new System.Drawing.Point(482, 325);
            this.ServerCancelButton.Name = "ServerCancelButton";
            this.ServerCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ServerCancelButton.TabIndex = 16;
            this.ServerCancelButton.Text = "Cancel";
            this.ServerCancelButton.UseVisualStyleBackColor = true;
            // 
            // AppConfigLabel
            // 
            this.AppConfigLabel.AutoSize = true;
            this.AppConfigLabel.Location = new System.Drawing.Point(32, 63);
            this.AppConfigLabel.Name = "AppConfigLabel";
            this.AppConfigLabel.Size = new System.Drawing.Size(67, 15);
            this.AppConfigLabel.TabIndex = 15;
            this.AppConfigLabel.Text = "App.config:";
            // 
            // ServerPropertyGrid
            // 
            this.ServerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerPropertyGrid.Location = new System.Drawing.Point(117, 63);
            this.ServerPropertyGrid.Name = "ServerPropertyGrid";
            this.ServerPropertyGrid.Size = new System.Drawing.Size(440, 256);
            this.ServerPropertyGrid.TabIndex = 14;
            // 
            // ServiceAccountChangeButton
            // 
            this.ServiceAccountChangeButton.Location = new System.Drawing.Point(450, 8);
            this.ServiceAccountChangeButton.Name = "ServiceAccountChangeButton";
            this.ServiceAccountChangeButton.Size = new System.Drawing.Size(108, 23);
            this.ServiceAccountChangeButton.TabIndex = 13;
            this.ServiceAccountChangeButton.Text = "Open Services";
            this.ServiceAccountChangeButton.UseVisualStyleBackColor = true;
            // 
            // ServiceAccountLabel
            // 
            this.ServiceAccountLabel.AutoSize = true;
            this.ServiceAccountLabel.Location = new System.Drawing.Point(3, 12);
            this.ServiceAccountLabel.Name = "ServiceAccountLabel";
            this.ServiceAccountLabel.Size = new System.Drawing.Size(96, 15);
            this.ServiceAccountLabel.TabIndex = 11;
            this.ServiceAccountLabel.Text = "Service Account:";
            // 
            // ServiceAccountTextBox
            // 
            this.ServiceAccountTextBox.Location = new System.Drawing.Point(117, 9);
            this.ServiceAccountTextBox.Name = "ServiceAccountTextBox";
            this.ServiceAccountTextBox.ReadOnly = true;
            this.ServiceAccountTextBox.Size = new System.Drawing.Size(296, 21);
            this.ServiceAccountTextBox.TabIndex = 12;
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
            this.WindowsTabPage.ResumeLayout(false);
            this.WindowsConfigurePanel.ResumeLayout(false);
            this.WindowsConfigurePanel.PerformLayout();
            this.ServerConfigurePanel.ResumeLayout(false);
            this.ServerConfigurePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TooltipLabel;
        private System.Windows.Forms.ErrorProvider ServerErrorProvider;
        private System.Windows.Forms.TabControl ConfigureTabControl;
        private System.Windows.Forms.TabPage ServerTabPage;
        private System.Windows.Forms.TabPage WindowsTabPage;
        private System.Windows.Forms.Panel WindowsConfigurePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid WindowsPropertyGrid;
        private System.Windows.Forms.Button WindowsApplyButton;
        private System.Windows.Forms.Button WindowsCancelButton;
        private System.Windows.Forms.Panel ServerConfigurePanel;
        private System.Windows.Forms.Button DeployScriptsButton;
        private System.Windows.Forms.TextBox DatabaseLocationTextBox;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.Button RefreshAccountButton;
        private System.Windows.Forms.Button ServerApplyButton;
        private System.Windows.Forms.Button ServerCancelButton;
        private System.Windows.Forms.Label AppConfigLabel;
        private System.Windows.Forms.PropertyGrid ServerPropertyGrid;
        private System.Windows.Forms.Button ServiceAccountChangeButton;
        private System.Windows.Forms.Label ServiceAccountLabel;
        private System.Windows.Forms.TextBox ServiceAccountTextBox;
    }
}
