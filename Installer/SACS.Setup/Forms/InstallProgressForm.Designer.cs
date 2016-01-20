namespace SACS.Setup.Forms
{
    partial class InstallProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallProgressForm));
            this.CancelInstallButton = new System.Windows.Forms.Button();
            this.InstallProgressBar = new System.Windows.Forms.ProgressBar();
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CancelInstallButton
            // 
            this.CancelInstallButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelInstallButton.Enabled = false;
            this.CancelInstallButton.Location = new System.Drawing.Point(312, 124);
            this.CancelInstallButton.Name = "CancelInstallButton";
            this.CancelInstallButton.Size = new System.Drawing.Size(75, 23);
            this.CancelInstallButton.TabIndex = 0;
            this.CancelInstallButton.Text = "Cancel";
            this.CancelInstallButton.UseVisualStyleBackColor = true;
            // 
            // InstallProgressBar
            // 
            this.InstallProgressBar.Location = new System.Drawing.Point(12, 95);
            this.InstallProgressBar.Name = "InstallProgressBar";
            this.InstallProgressBar.Size = new System.Drawing.Size(375, 23);
            this.InstallProgressBar.TabIndex = 1;
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Padding = new System.Windows.Forms.Padding(20);
            this.CaptionLabel.Size = new System.Drawing.Size(399, 92);
            this.CaptionLabel.TabIndex = 2;
            this.CaptionLabel.Tag = "";
            // 
            // InstallProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelInstallButton;
            this.ClientSize = new System.Drawing.Size(399, 157);
            this.ControlBox = false;
            this.Controls.Add(this.CaptionLabel);
            this.Controls.Add(this.InstallProgressBar);
            this.Controls.Add(this.CancelInstallButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallProgressForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Installing...";
            this.Load += new System.EventHandler(this.InstallProgressForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelInstallButton;
        private System.Windows.Forms.ProgressBar InstallProgressBar;
        private System.Windows.Forms.Label CaptionLabel;
    }
}