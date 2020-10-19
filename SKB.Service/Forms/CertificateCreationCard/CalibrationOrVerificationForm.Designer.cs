namespace SKB.Service.Forms.CertificateCreationCard
{
    partial class CalibrationOrVerificationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationOrVerificationForm));
            this.VerificationButton = new DevExpress.XtraEditors.SimpleButton();
            this.CalibrationButton = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // VerificationButton
            // 
            this.VerificationButton.Location = new System.Drawing.Point(1, 79);
            this.VerificationButton.Name = "VerificationButton";
            this.VerificationButton.Size = new System.Drawing.Size(298, 70);
            this.VerificationButton.TabIndex = 2;
            this.VerificationButton.Text = "Заявка на поверку";
            this.VerificationButton.Click += new System.EventHandler(this.VerificationButton_Click);
            // 
            // CalibrationButton
            // 
            this.CalibrationButton.Location = new System.Drawing.Point(1, 3);
            this.CalibrationButton.Name = "CalibrationButton";
            this.CalibrationButton.Size = new System.Drawing.Size(298, 70);
            this.CalibrationButton.TabIndex = 1;
            this.CalibrationButton.Text = "Заявка на калибровку";
            this.CalibrationButton.Click += new System.EventHandler(this.CalibrationButton_Click);
            // 
            // CalibrationOrVerificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.VerificationButton);
            this.Controls.Add(this.CalibrationButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalibrationOrVerificationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выберите тип заявки:";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CalibrationOrVerificationForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton VerificationButton;
        private DevExpress.XtraEditors.SimpleButton CalibrationButton;
    }
}