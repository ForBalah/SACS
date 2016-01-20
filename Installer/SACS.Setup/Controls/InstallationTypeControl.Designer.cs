namespace SACS.Setup.Controls
{
    partial class InstallationTypeControl
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
            this.InstallRadioButton = new System.Windows.Forms.RadioButton();
            this.ConfigureRadioButton = new System.Windows.Forms.RadioButton();
            this.UninstallRadioButton = new System.Windows.Forms.RadioButton();
            this.TooltipLabel = new System.Windows.Forms.Label();
            this.InstallationGroupBox = new System.Windows.Forms.GroupBox();
            this.InstallationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the type of installation";
            // 
            // InstallRadioButton
            // 
            this.InstallRadioButton.AutoSize = true;
            this.InstallRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstallRadioButton.Location = new System.Drawing.Point(19, 19);
            this.InstallRadioButton.Name = "InstallRadioButton";
            this.InstallRadioButton.Size = new System.Drawing.Size(148, 19);
            this.InstallRadioButton.TabIndex = 1;
            this.InstallRadioButton.TabStop = true;
            this.InstallRadioButton.Text = "Install / Upgrade SACS";
            this.InstallRadioButton.UseVisualStyleBackColor = true;
            this.InstallRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            this.InstallRadioButton.MouseEnter += new System.EventHandler(this.InstallRadioButton_MouseEnter);
            this.InstallRadioButton.MouseLeave += new System.EventHandler(this.RadioButton_MouseLeave);
            // 
            // ConfigureRadioButton
            // 
            this.ConfigureRadioButton.AutoSize = true;
            this.ConfigureRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigureRadioButton.Location = new System.Drawing.Point(19, 45);
            this.ConfigureRadioButton.Name = "ConfigureRadioButton";
            this.ConfigureRadioButton.Size = new System.Drawing.Size(112, 19);
            this.ConfigureRadioButton.TabIndex = 2;
            this.ConfigureRadioButton.TabStop = true;
            this.ConfigureRadioButton.Text = "Configure SACS";
            this.ConfigureRadioButton.UseVisualStyleBackColor = true;
            this.ConfigureRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            this.ConfigureRadioButton.MouseEnter += new System.EventHandler(this.ConfigureRadioButton_MouseEnter);
            this.ConfigureRadioButton.MouseLeave += new System.EventHandler(this.RadioButton_MouseLeave);
            // 
            // UninstallRadioButton
            // 
            this.UninstallRadioButton.AutoSize = true;
            this.UninstallRadioButton.Enabled = false;
            this.UninstallRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UninstallRadioButton.Location = new System.Drawing.Point(19, 71);
            this.UninstallRadioButton.Name = "UninstallRadioButton";
            this.UninstallRadioButton.Size = new System.Drawing.Size(107, 19);
            this.UninstallRadioButton.TabIndex = 3;
            this.UninstallRadioButton.TabStop = true;
            this.UninstallRadioButton.Text = "Uninstall SACS";
            this.UninstallRadioButton.UseVisualStyleBackColor = true;
            this.UninstallRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            this.UninstallRadioButton.MouseEnter += new System.EventHandler(this.UninstallRadioButton_MouseEnter);
            this.UninstallRadioButton.MouseLeave += new System.EventHandler(this.RadioButton_MouseLeave);
            // 
            // TooltipLabel
            // 
            this.TooltipLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TooltipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TooltipLabel.Location = new System.Drawing.Point(0, 388);
            this.TooltipLabel.Name = "TooltipLabel";
            this.TooltipLabel.Size = new System.Drawing.Size(493, 47);
            this.TooltipLabel.TabIndex = 4;
            this.TooltipLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // InstallationGroupBox
            // 
            this.InstallationGroupBox.Controls.Add(this.InstallRadioButton);
            this.InstallationGroupBox.Controls.Add(this.ConfigureRadioButton);
            this.InstallationGroupBox.Controls.Add(this.UninstallRadioButton);
            this.InstallationGroupBox.Location = new System.Drawing.Point(7, 42);
            this.InstallationGroupBox.Name = "InstallationGroupBox";
            this.InstallationGroupBox.Size = new System.Drawing.Size(193, 109);
            this.InstallationGroupBox.TabIndex = 5;
            this.InstallationGroupBox.TabStop = false;
            // 
            // InstallationTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InstallationGroupBox);
            this.Controls.Add(this.TooltipLabel);
            this.Controls.Add(this.label1);
            this.Name = "InstallationTypeControl";
            this.Size = new System.Drawing.Size(493, 435);
            this.VisibleChanged += new System.EventHandler(this.InstallationTypeControl_VisibleChanged);
            this.InstallationGroupBox.ResumeLayout(false);
            this.InstallationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton InstallRadioButton;
        private System.Windows.Forms.RadioButton ConfigureRadioButton;
        private System.Windows.Forms.RadioButton UninstallRadioButton;
        private System.Windows.Forms.Label TooltipLabel;
        private System.Windows.Forms.GroupBox InstallationGroupBox;
    }
}
