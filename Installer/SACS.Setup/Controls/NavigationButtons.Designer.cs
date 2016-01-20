namespace SACS.Setup.Controls
{
    partial class NavigationButtons
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
            this.CancelWizardButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CancelWizardButton
            // 
            this.CancelWizardButton.Location = new System.Drawing.Point(169, 3);
            this.CancelWizardButton.Name = "CancelWizardButton";
            this.CancelWizardButton.Size = new System.Drawing.Size(75, 23);
            this.CancelWizardButton.TabIndex = 0;
            this.CancelWizardButton.Text = "Cancel";
            this.CancelWizardButton.UseVisualStyleBackColor = true;
            this.CancelWizardButton.Click += new System.EventHandler(this.CancelWizardButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(88, 3);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(7, 3);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 2;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // NavigationButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.CancelWizardButton);
            this.Name = "NavigationButtons";
            this.Size = new System.Drawing.Size(252, 31);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelWizardButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button BackButton;
    }
}
