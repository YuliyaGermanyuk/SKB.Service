namespace SKB.Service.Forms.ServiceCard
{
    partial class AdditionalWares
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.WaresNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.DiagnosticsTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalibrationTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalibrationProtocol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.CalibrationCertificate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.CalibrationProtocolID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CalibrationCertificateID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WaresNumberID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OKButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancButton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Enabled = false;
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemButtonEdit2,
            this.repositoryItemButtonEdit3});
            this.gridControl1.Size = new System.Drawing.Size(617, 210);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.WaresNumber,
            this.DiagnosticsTime,
            this.CalibrationTime,
            this.CalibrationProtocol,
            this.CalibrationCertificate,
            this.CalibrationProtocolID,
            this.CalibrationCertificateID,
            this.WaresNumberID});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ViewCaption = "Дополнительные изделия";
            // 
            // WaresNumber
            // 
            this.WaresNumber.Caption = "Заводской номер";
            this.WaresNumber.ColumnEdit = this.repositoryItemButtonEdit1;
            this.WaresNumber.FieldName = "WaresNumber";
            this.WaresNumber.Name = "WaresNumber";
            this.WaresNumber.OptionsColumn.ReadOnly = true;
            this.WaresNumber.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.WaresNumber.Visible = true;
            this.WaresNumber.VisibleIndex = 0;
            this.WaresNumber.Width = 126;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            this.repositoryItemButtonEdit1.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit1_ButtonPressed);
            // 
            // DiagnosticsTime
            // 
            this.DiagnosticsTime.Caption = "Тр-ть диагностики";
            this.DiagnosticsTime.FieldName = "DiagnosticsTime";
            this.DiagnosticsTime.Name = "DiagnosticsTime";
            this.DiagnosticsTime.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.DiagnosticsTime.Visible = true;
            this.DiagnosticsTime.VisibleIndex = 1;
            this.DiagnosticsTime.Width = 100;
            // 
            // CalibrationTime
            // 
            this.CalibrationTime.Caption = "Тр-ть калибровки";
            this.CalibrationTime.FieldName = "CalibrationTime";
            this.CalibrationTime.Name = "CalibrationTime";
            this.CalibrationTime.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.CalibrationTime.Visible = true;
            this.CalibrationTime.VisibleIndex = 2;
            this.CalibrationTime.Width = 100;
            // 
            // CalibrationProtocol
            // 
            this.CalibrationProtocol.Caption = "Протокол калибровки";
            this.CalibrationProtocol.ColumnEdit = this.repositoryItemButtonEdit2;
            this.CalibrationProtocol.FieldName = "CalibrationProtocol";
            this.CalibrationProtocol.Name = "CalibrationProtocol";
            this.CalibrationProtocol.OptionsColumn.ReadOnly = true;
            this.CalibrationProtocol.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.CalibrationProtocol.Visible = true;
            this.CalibrationProtocol.VisibleIndex = 3;
            this.CalibrationProtocol.Width = 137;
            // 
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            this.repositoryItemButtonEdit2.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit2_ButtonPressed);
            // 
            // CalibrationCertificate
            // 
            this.CalibrationCertificate.Caption = "Сертификат о калибровке";
            this.CalibrationCertificate.ColumnEdit = this.repositoryItemButtonEdit3;
            this.CalibrationCertificate.FieldName = "CalibrationCertificate";
            this.CalibrationCertificate.Name = "CalibrationCertificate";
            this.CalibrationCertificate.OptionsColumn.ReadOnly = true;
            this.CalibrationCertificate.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.CalibrationCertificate.Visible = true;
            this.CalibrationCertificate.VisibleIndex = 4;
            this.CalibrationCertificate.Width = 136;
            // 
            // repositoryItemButtonEdit3
            // 
            this.repositoryItemButtonEdit3.AutoHeight = false;
            this.repositoryItemButtonEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit3.Name = "repositoryItemButtonEdit3";
            this.repositoryItemButtonEdit3.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit3_ButtonPressed);
            // 
            // CalibrationProtocolID
            // 
            this.CalibrationProtocolID.Caption = "ID протокола калибровки";
            this.CalibrationProtocolID.FieldName = "CalibrationProtocolID";
            this.CalibrationProtocolID.Name = "CalibrationProtocolID";
            this.CalibrationProtocolID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // CalibrationCertificateID
            // 
            this.CalibrationCertificateID.Caption = "ID сертификата о калибровке";
            this.CalibrationCertificateID.FieldName = "CalibrationCertificateID";
            this.CalibrationCertificateID.Name = "CalibrationCertificateID";
            this.CalibrationCertificateID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // WaresNumberID
            // 
            this.WaresNumberID.Caption = "ID Заводского номера";
            this.WaresNumberID.FieldName = "WaresNumberID";
            this.WaresNumberID.Name = "WaresNumberID";
            this.WaresNumberID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(12, 230);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(300, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "Сохранить";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancButton
            // 
            this.CancButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancButton.Location = new System.Drawing.Point(329, 230);
            this.CancButton.Name = "CancButton";
            this.CancButton.Size = new System.Drawing.Size(300, 23);
            this.CancButton.TabIndex = 1;
            this.CancButton.Text = "Отмена";
            this.CancButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AdditionalWares
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(641, 262);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.CancButton);
            this.Controls.Add(this.OKButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdditionalWares";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Дополнительные изделия";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdditionalWares_FormClosing);
            this.Load += new System.EventHandler(this.AdditionalWares_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn WaresNumber;
        private DevExpress.XtraGrid.Columns.GridColumn DiagnosticsTime;
        private DevExpress.XtraGrid.Columns.GridColumn CalibrationTime;
        private DevExpress.XtraEditors.SimpleButton OKButton;
        private DevExpress.XtraEditors.SimpleButton CancButton;
        private DevExpress.XtraGrid.Columns.GridColumn CalibrationProtocol;
        private DevExpress.XtraGrid.Columns.GridColumn CalibrationCertificate;
        private DevExpress.XtraGrid.Columns.GridColumn CalibrationProtocolID;
        private DevExpress.XtraGrid.Columns.GridColumn CalibrationCertificateID;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn WaresNumberID;
    }
}