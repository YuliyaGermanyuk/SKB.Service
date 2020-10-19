namespace SKB.Service.Forms.ServiceCard
{
    partial class DeviceList
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
            this.DeviceType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DeviceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DevicePassport = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Enabled = false;
            this.gridControl1.Location = new System.Drawing.Point(10, 10);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(240, 150);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DeviceType,
            this.DeviceNumber,
            this.DevicePassport});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsSelection.InvertSelection = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.UseIndicatorForSelection = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // DeviceType
            // 
            this.DeviceType.Caption = "Наименование";
            this.DeviceType.FieldName = "DeviceType";
            this.DeviceType.Name = "DeviceType";
            this.DeviceType.OptionsColumn.ReadOnly = true;
            this.DeviceType.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // DeviceNumber
            // 
            this.DeviceNumber.Caption = "Заводской номер";
            this.DeviceNumber.FieldName = "DeviceNumber";
            this.DeviceNumber.Name = "DeviceNumber";
            this.DeviceNumber.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.DeviceNumber.Visible = true;
            this.DeviceNumber.VisibleIndex = 0;
            // 
            // DevicePassport
            // 
            this.DevicePassport.Caption = "Паспорт прибора";
            this.DevicePassport.FieldName = "DevicePassport";
            this.DevicePassport.Name = "DevicePassport";
            this.DevicePassport.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // DeviceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(260, 170);
            this.Controls.Add(this.gridControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выберите справочник сборочных узлов:";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceList_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceType;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceNumber;
        private DevExpress.XtraGrid.Columns.GridColumn DevicePassport;
    }
}