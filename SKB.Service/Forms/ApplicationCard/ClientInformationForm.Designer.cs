using DevExpress.XtraEditors;
using DevExpress.LookAndFeel.Helpers;

namespace SKB.Service.Forms.ApplicationCard
{
    partial class ClientInformationForm
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
            this.TClientName = new DevExpress.XtraEditors.TextEdit();
            this.LClientName = new DevExpress.XtraEditors.LabelControl();
            this.LFullName = new DevExpress.XtraEditors.LabelControl();
            this.TFullName = new DevExpress.XtraEditors.TextEdit();
            this.LLegalAddress = new DevExpress.XtraEditors.LabelControl();
            this.LPostalAddress = new DevExpress.XtraEditors.LabelControl();
            this.LReturnAddress = new DevExpress.XtraEditors.LabelControl();
            this.LPhone = new DevExpress.XtraEditors.LabelControl();
            this.LEmail = new DevExpress.XtraEditors.LabelControl();
            this.LContactName = new DevExpress.XtraEditors.LabelControl();
            this.TLegalAddress = new DevExpress.XtraEditors.TextEdit();
            this.TPostalAddress = new DevExpress.XtraEditors.TextEdit();
            this.TReturnAddress = new DevExpress.XtraEditors.TextEdit();
            this.TPhone = new DevExpress.XtraEditors.TextEdit();
            this.TEmail = new DevExpress.XtraEditors.TextEdit();
            this.TContactName = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.TClientName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TFullName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TLegalAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TPostalAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TReturnAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TContactName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TClientName
            // 
            this.TClientName.Location = new System.Drawing.Point(194, 15);
            this.TClientName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TClientName.Name = "TClientName";
            this.TClientName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TClientName.Properties.Appearance.Options.UseFont = true;
            this.TClientName.Properties.LookAndFeel.SkinName = "Blue";
            this.TClientName.Properties.ReadOnly = true;
            this.TClientName.Size = new System.Drawing.Size(638, 22);
            this.TClientName.TabIndex = 0;
            // 
            // LClientName
            // 
            this.LClientName.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LClientName.Location = new System.Drawing.Point(14, 18);
            this.LClientName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LClientName.Name = "LClientName";
            this.LClientName.Size = new System.Drawing.Size(142, 16);
            this.LClientName.TabIndex = 1;
            this.LClientName.Text = "Наименование клиента:";
            // 
            // LFullName
            // 
            this.LFullName.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LFullName.Location = new System.Drawing.Point(14, 51);
            this.LFullName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LFullName.Name = "LFullName";
            this.LFullName.Size = new System.Drawing.Size(137, 16);
            this.LFullName.TabIndex = 3;
            this.LFullName.Text = "Полное наименование:";
            // 
            // TFullName
            // 
            this.TFullName.Location = new System.Drawing.Point(194, 48);
            this.TFullName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TFullName.Name = "TFullName";
            this.TFullName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TFullName.Properties.Appearance.Options.UseFont = true;
            this.TFullName.Properties.ReadOnly = true;
            this.TFullName.Size = new System.Drawing.Size(638, 22);
            this.TFullName.TabIndex = 2;
            // 
            // LLegalAddress
            // 
            this.LLegalAddress.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LLegalAddress.Location = new System.Drawing.Point(11, 36);
            this.LLegalAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LLegalAddress.Name = "LLegalAddress";
            this.LLegalAddress.Size = new System.Drawing.Size(123, 16);
            this.LLegalAddress.TabIndex = 4;
            this.LLegalAddress.Text = "Юридический адрес:";
            // 
            // LPostalAddress
            // 
            this.LPostalAddress.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LPostalAddress.Location = new System.Drawing.Point(11, 73);
            this.LPostalAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LPostalAddress.Name = "LPostalAddress";
            this.LPostalAddress.Size = new System.Drawing.Size(101, 16);
            this.LPostalAddress.TabIndex = 5;
            this.LPostalAddress.Text = "Почтовый адрес:";
            // 
            // LReturnAddress
            // 
            this.LReturnAddress.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LReturnAddress.Location = new System.Drawing.Point(11, 108);
            this.LReturnAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LReturnAddress.Name = "LReturnAddress";
            this.LReturnAddress.Size = new System.Drawing.Size(158, 16);
            this.LReturnAddress.TabIndex = 6;
            this.LReturnAddress.Text = "Адрес возврата приборов:";
            // 
            // LPhone
            // 
            this.LPhone.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LPhone.Location = new System.Drawing.Point(11, 39);
            this.LPhone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LPhone.Name = "LPhone";
            this.LPhone.Size = new System.Drawing.Size(58, 16);
            this.LPhone.TabIndex = 7;
            this.LPhone.Text = "Телефон:";
            this.LPhone.UseWaitCursor = true;
            // 
            // LEmail
            // 
            this.LEmail.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LEmail.Location = new System.Drawing.Point(13, 78);
            this.LEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LEmail.Name = "LEmail";
            this.LEmail.Size = new System.Drawing.Size(41, 16);
            this.LEmail.TabIndex = 8;
            this.LEmail.Text = "E-mail:";
            this.LEmail.UseWaitCursor = true;
            // 
            // LContactName
            // 
            this.LContactName.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LContactName.Location = new System.Drawing.Point(13, 115);
            this.LContactName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LContactName.Name = "LContactName";
            this.LContactName.Size = new System.Drawing.Size(104, 16);
            this.LContactName.TabIndex = 9;
            this.LContactName.Text = "Контактное лицо:";
            this.LContactName.UseWaitCursor = true;
            // 
            // TLegalAddress
            // 
            this.TLegalAddress.Location = new System.Drawing.Point(182, 33);
            this.TLegalAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TLegalAddress.Name = "TLegalAddress";
            this.TLegalAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TLegalAddress.Properties.Appearance.Options.UseFont = true;
            this.TLegalAddress.Properties.ReadOnly = true;
            this.TLegalAddress.Size = new System.Drawing.Size(620, 22);
            this.TLegalAddress.TabIndex = 10;
            // 
            // TPostalAddress
            // 
            this.TPostalAddress.Location = new System.Drawing.Point(182, 70);
            this.TPostalAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TPostalAddress.Name = "TPostalAddress";
            this.TPostalAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TPostalAddress.Properties.Appearance.Options.UseFont = true;
            this.TPostalAddress.Properties.ReadOnly = true;
            this.TPostalAddress.Size = new System.Drawing.Size(620, 22);
            this.TPostalAddress.TabIndex = 11;
            // 
            // TReturnAddress
            // 
            this.TReturnAddress.Location = new System.Drawing.Point(182, 105);
            this.TReturnAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TReturnAddress.Name = "TReturnAddress";
            this.TReturnAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TReturnAddress.Properties.Appearance.Options.UseFont = true;
            this.TReturnAddress.Size = new System.Drawing.Size(620, 22);
            this.TReturnAddress.TabIndex = 12;
            // 
            // TPhone
            // 
            this.TPhone.Location = new System.Drawing.Point(182, 36);
            this.TPhone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TPhone.Name = "TPhone";
            this.TPhone.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TPhone.Properties.Appearance.Options.UseFont = true;
            this.TPhone.Size = new System.Drawing.Size(480, 22);
            this.TPhone.TabIndex = 13;
            // 
            // TEmail
            // 
            this.TEmail.Location = new System.Drawing.Point(182, 75);
            this.TEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TEmail.Name = "TEmail";
            this.TEmail.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TEmail.Properties.Appearance.Options.UseFont = true;
            this.TEmail.Size = new System.Drawing.Size(480, 22);
            this.TEmail.TabIndex = 14;
            // 
            // TContactName
            // 
            this.TContactName.Location = new System.Drawing.Point(182, 112);
            this.TContactName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TContactName.Name = "TContactName";
            this.TContactName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TContactName.Properties.Appearance.Options.UseFont = true;
            this.TContactName.Size = new System.Drawing.Size(480, 22);
            this.TContactName.TabIndex = 15;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(706, 370);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(126, 28);
            this.simpleButton1.TabIndex = 18;
            this.simpleButton1.Text = "Отмена";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(706, 327);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(126, 28);
            this.simpleButton2.TabIndex = 19;
            this.simpleButton2.Text = "Сохранить";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.LLegalAddress);
            this.groupControl1.Controls.Add(this.LPostalAddress);
            this.groupControl1.Controls.Add(this.LReturnAddress);
            this.groupControl1.Controls.Add(this.TLegalAddress);
            this.groupControl1.Controls.Add(this.TPostalAddress);
            this.groupControl1.Controls.Add(this.TReturnAddress);
            this.groupControl1.Location = new System.Drawing.Point(12, 83);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(820, 146);
            this.groupControl1.TabIndex = 20;
            this.groupControl1.Text = "Адреса:";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.LPhone);
            this.groupControl2.Controls.Add(this.LEmail);
            this.groupControl2.Controls.Add(this.TContactName);
            this.groupControl2.Controls.Add(this.LContactName);
            this.groupControl2.Controls.Add(this.TEmail);
            this.groupControl2.Controls.Add(this.TPhone);
            this.groupControl2.Location = new System.Drawing.Point(12, 249);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(676, 149);
            this.groupControl2.TabIndex = 21;
            this.groupControl2.Text = "Дополнительная информация:";
            // 
            // ClientInformationForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(847, 422);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.LFullName);
            this.Controls.Add(this.TFullName);
            this.Controls.Add(this.LClientName);
            this.Controls.Add(this.TClientName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ClientInformationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Информация о клиенте";
            ((System.ComponentModel.ISupportInitialize)(this.TClientName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TFullName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TLegalAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TPostalAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TReturnAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TContactName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextEdit TClientName;
        private LabelControl LClientName;
        private LabelControl LFullName;
        private TextEdit TFullName;
        private LabelControl LLegalAddress;
        private LabelControl LPostalAddress;
        private LabelControl LReturnAddress;
        private LabelControl LPhone;
        private LabelControl LEmail;
        private LabelControl LContactName;
        private TextEdit TLegalAddress;
        private TextEdit TPostalAddress;
        private TextEdit TReturnAddress;
        private TextEdit TPhone;
        private TextEdit TEmail;
        private TextEdit TContactName;
        private SimpleButton simpleButton1;
        private SimpleButton simpleButton2;
        private GroupControl groupControl1;
        private GroupControl groupControl2;
    }
}