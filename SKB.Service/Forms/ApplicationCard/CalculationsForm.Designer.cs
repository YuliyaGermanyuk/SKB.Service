using System;
using SKB.Base;

namespace SKB.Service.Forms.ApplicationCard
{
    partial class CalculationsForm
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
        /// 
        private delegate void Calculation_DoubleClickDelegate(Object sender, EventArgs e);
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationsForm));
            this.NDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.CostWithoutNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.CostWithNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.CostNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.NDS_Label = new DevExpress.XtraEditors.LabelControl();
            this.CostWithoutNDS_Label = new DevExpress.XtraEditors.LabelControl();
            this.CostWithNDS_Label = new DevExpress.XtraEditors.LabelControl();
            this.CostNDS_Label = new DevExpress.XtraEditors.LabelControl();
            this.CalculationFile_Value = new DevExpress.XtraEditors.ButtonEdit();
            this.CalculationGridControl = new DevExpress.XtraGrid.GridControl();
            this.CalculationGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DeviceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BlockName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WorkName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Improvements = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Price = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Cost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Include = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Comment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FullCost = new DevExpress.XtraEditors.GroupControl();
            this.FullCostNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.FullCostWithNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.FullCostWithoutNDS_Value = new DevExpress.XtraEditors.SpinEdit();
            this.IncludeInAccount = new DevExpress.XtraEditors.GroupControl();
            this.CalculationFile_Label = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.NDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostWithoutNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostWithNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationFile_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCost)).BeginInit();
            this.FullCost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostWithNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostWithoutNDS_Value.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IncludeInAccount)).BeginInit();
            this.IncludeInAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // NDS_Value
            // 
            this.NDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NDS_Value.Location = new System.Drawing.Point(90, 448);
            this.NDS_Value.Name = "NDS_Value";
            this.NDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.NDS_Value.Size = new System.Drawing.Size(68, 20);
            this.NDS_Value.TabIndex = 0;
            this.NDS_Value.EditValueChanged += new System.EventHandler(this.TNDS_EditValueChanged);
            // 
            // CostWithoutNDS_Value
            // 
            this.CostWithoutNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostWithoutNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CostWithoutNDS_Value.Location = new System.Drawing.Point(169, 30);
            this.CostWithoutNDS_Value.Name = "CostWithoutNDS_Value";
            this.CostWithoutNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.CostWithoutNDS_Value.Properties.ReadOnly = true;
            this.CostWithoutNDS_Value.Size = new System.Drawing.Size(122, 20);
            this.CostWithoutNDS_Value.TabIndex = 4;
            // 
            // CostWithNDS_Value
            // 
            this.CostWithNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostWithNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CostWithNDS_Value.Location = new System.Drawing.Point(169, 58);
            this.CostWithNDS_Value.Name = "CostWithNDS_Value";
            this.CostWithNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.CostWithNDS_Value.Properties.ReadOnly = true;
            this.CostWithNDS_Value.Size = new System.Drawing.Size(122, 20);
            this.CostWithNDS_Value.TabIndex = 5;
            // 
            // CostNDS_Value
            // 
            this.CostNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CostNDS_Value.Location = new System.Drawing.Point(389, 30);
            this.CostNDS_Value.Name = "CostNDS_Value";
            this.CostNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.CostNDS_Value.Properties.ReadOnly = true;
            this.CostNDS_Value.Size = new System.Drawing.Size(106, 20);
            this.CostNDS_Value.TabIndex = 6;
            // 
            // NDS_Label
            // 
            this.NDS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NDS_Label.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NDS_Label.Location = new System.Drawing.Point(30, 451);
            this.NDS_Label.Name = "NDS_Label";
            this.NDS_Label.Size = new System.Drawing.Size(54, 13);
            this.NDS_Label.TabIndex = 1;
            this.NDS_Label.Text = "НДС (%):";
            // 
            // CostWithoutNDS_Label
            // 
            this.CostWithoutNDS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostWithoutNDS_Label.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CostWithoutNDS_Label.Location = new System.Drawing.Point(26, 33);
            this.CostWithoutNDS_Label.Name = "CostWithoutNDS_Label";
            this.CostWithoutNDS_Label.Size = new System.Drawing.Size(128, 13);
            this.CostWithoutNDS_Label.TabIndex = 2;
            this.CostWithoutNDS_Label.Text = "Сумма без НДС (руб.):";
            // 
            // CostWithNDS_Label
            // 
            this.CostWithNDS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostWithNDS_Label.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CostWithNDS_Label.Location = new System.Drawing.Point(26, 61);
            this.CostWithNDS_Label.Name = "CostWithNDS_Label";
            this.CostWithNDS_Label.Size = new System.Drawing.Size(114, 13);
            this.CostWithNDS_Label.TabIndex = 3;
            this.CostWithNDS_Label.Text = "Сумма с НДС (руб.):";
            // 
            // CostNDS_Label
            // 
            this.CostNDS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CostNDS_Label.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CostNDS_Label.Location = new System.Drawing.Point(309, 33);
            this.CostNDS_Label.Name = "CostNDS_Label";
            this.CostNDS_Label.Size = new System.Drawing.Size(65, 13);
            this.CostNDS_Label.TabIndex = 3;
            this.CostNDS_Label.Text = "НДС (руб.):";
            // 
            // CalculationFile_Value
            // 
            this.CalculationFile_Value.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CalculationFile_Value.Location = new System.Drawing.Point(311, 448);
            this.CalculationFile_Value.Name = "CalculationFile_Value";
            this.CalculationFile_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.CalculationFile_Value.Properties.ReadOnly = true;
            this.CalculationFile_Value.Size = new System.Drawing.Size(760, 20);
            this.CalculationFile_Value.TabIndex = 7;
            this.CalculationFile_Value.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CalculationFileButton_ButtonPressed);
            this.CalculationFile_Value.DoubleClick += new System.EventHandler(this.CalculationFile_DoubleClick);
            // 
            // CalculationGridControl
            // 
            this.CalculationGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CalculationGridControl.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CalculationGridControl.Location = new System.Drawing.Point(18, 22);
            this.CalculationGridControl.MainView = this.CalculationGridView;
            this.CalculationGridControl.Name = "CalculationGridControl";
            this.CalculationGridControl.Size = new System.Drawing.Size(1053, 407);
            this.CalculationGridControl.TabIndex = 8;
            this.CalculationGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CalculationGridView});
            // 
            // CalculationGridView
            // 
            this.CalculationGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DeviceName,
            this.BlockName,
            this.WorkName,
            this.Improvements,
            this.Price,
            this.Count,
            this.Cost,
            this.Include,
            this.Comment});
            this.CalculationGridView.GridControl = this.CalculationGridControl;
            this.CalculationGridView.Name = "CalculationGridView";
            this.CalculationGridView.OptionsView.ShowGroupPanel = false;
            this.CalculationGridView.AddDoubleClickHandler(new Calculation_DoubleClickDelegate(Calculation_DoubleClick));
            // 
            // DeviceName
            // 
            this.DeviceName.Caption = "Наименование прибора";
            this.DeviceName.FieldName = "DeviceName";
            this.DeviceName.MinWidth = 15;
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.OptionsColumn.ReadOnly = true;
            this.DeviceName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.DeviceName.Visible = true;
            this.DeviceName.VisibleIndex = 0;
            this.DeviceName.Width = 128;
            // 
            // BlockName
            // 
            this.BlockName.Caption = "Сборочный узел";
            this.BlockName.FieldName = "BlockName";
            this.BlockName.MinWidth = 15;
            this.BlockName.Name = "BlockName";
            this.BlockName.OptionsColumn.ReadOnly = true;
            this.BlockName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.BlockName.Visible = true;
            this.BlockName.VisibleIndex = 1;
            this.BlockName.Width = 172;
            // 
            // WorkName
            // 
            this.WorkName.Caption = "Вид работ";
            this.WorkName.FieldName = "WorkName";
            this.WorkName.Name = "WorkName";
            this.WorkName.OptionsColumn.ReadOnly = true;
            this.WorkName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.WorkName.Visible = true;
            this.WorkName.VisibleIndex = 2;
            this.WorkName.Width = 198;
            // 
            // Improvements
            // 
            this.Improvements.Caption = "Доработка";
            this.Improvements.FieldName = "Improvements";
            this.Improvements.Name = "Improvements";
            this.Improvements.OptionsColumn.ReadOnly = true;
            this.Improvements.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.Improvements.Visible = true;
            this.Improvements.VisibleIndex = 3;
            this.Improvements.Width = 77;
            // 
            // Price
            // 
            this.Price.Caption = "Цена";
            this.Price.FieldName = "Price";
            this.Price.Name = "Price";
            this.Price.OptionsColumn.ReadOnly = true;
            this.Price.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Price.Visible = true;
            this.Price.VisibleIndex = 4;
            // 
            // Count
            // 
            this.Count.Caption = "Кол-во";
            this.Count.FieldName = "Count";
            this.Count.Name = "Count";
            this.Count.OptionsColumn.ReadOnly = true;
            this.Count.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Count.Visible = true;
            this.Count.VisibleIndex = 5;
            this.Count.Width = 56;
            // 
            // Cost
            // 
            this.Cost.Caption = "Стоимость";
            this.Cost.FieldName = "Cost";
            this.Cost.Name = "Cost";
            this.Cost.OptionsColumn.ReadOnly = true;
            this.Cost.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Cost.Visible = true;
            this.Cost.VisibleIndex = 6;
            this.Cost.Width = 70;
            // 
            // Include
            // 
            this.Include.Caption = "Включено в счет";
            this.Include.FieldName = "Include";
            this.Include.Name = "Include";
            this.Include.OptionsColumn.ReadOnly = true;
            this.Include.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.Include.Visible = true;
            this.Include.VisibleIndex = 7;
            this.Include.Width = 77;
            // 
            // Comment
            // 
            this.Comment.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.Comment.AppearanceCell.Options.UseForeColor = true;
            this.Comment.Caption = "Комментарий";
            this.Comment.FieldName = "Comment";
            this.Comment.Name = "Comment";
            this.Comment.OptionsColumn.ReadOnly = true;
            this.Comment.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.Comment.Visible = true;
            this.Comment.VisibleIndex = 8;
            this.Comment.Width = 182;
            // 
            // FullCost
            // 
            this.FullCost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FullCost.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.FullCost.Appearance.Options.UseBackColor = true;
            this.FullCost.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FullCost.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.FullCost.AppearanceCaption.Options.UseFont = true;
            this.FullCost.AppearanceCaption.Options.UseForeColor = true;
            this.FullCost.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FullCost.Controls.Add(this.FullCostNDS_Value);
            this.FullCost.Controls.Add(this.labelControl1);
            this.FullCost.Controls.Add(this.labelControl2);
            this.FullCost.Controls.Add(this.FullCostWithNDS_Value);
            this.FullCost.Controls.Add(this.labelControl3);
            this.FullCost.Controls.Add(this.FullCostWithoutNDS_Value);
            this.FullCost.Location = new System.Drawing.Point(560, 485);
            this.FullCost.Name = "FullCost";
            this.FullCost.Size = new System.Drawing.Size(511, 87);
            this.FullCost.TabIndex = 9;
            this.FullCost.Text = "Стоимость всех выполненных работ:";
            // 
            // FullCostNDS_Value
            // 
            this.FullCostNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FullCostNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.FullCostNDS_Value.Location = new System.Drawing.Point(387, 26);
            this.FullCostNDS_Value.Name = "FullCostNDS_Value";
            this.FullCostNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.FullCostNDS_Value.Properties.ReadOnly = true;
            this.FullCostNDS_Value.Size = new System.Drawing.Size(107, 20);
            this.FullCostNDS_Value.TabIndex = 12;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl1.Location = new System.Drawing.Point(313, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 13);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "НДС (руб.):";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl2.Location = new System.Drawing.Point(26, 61);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 13);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "Сумма с НДС (руб.):";
            // 
            // FullCostWithNDS_Value
            // 
            this.FullCostWithNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FullCostWithNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.FullCostWithNDS_Value.Location = new System.Drawing.Point(169, 58);
            this.FullCostWithNDS_Value.Name = "FullCostWithNDS_Value";
            this.FullCostWithNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.FullCostWithNDS_Value.Properties.ReadOnly = true;
            this.FullCostWithNDS_Value.Size = new System.Drawing.Size(122, 20);
            this.FullCostWithNDS_Value.TabIndex = 11;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl3.Location = new System.Drawing.Point(26, 30);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(128, 13);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Сумма без НДС (руб.):";
            // 
            // FullCostWithoutNDS_Value
            // 
            this.FullCostWithoutNDS_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FullCostWithoutNDS_Value.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.FullCostWithoutNDS_Value.Location = new System.Drawing.Point(169, 27);
            this.FullCostWithoutNDS_Value.Name = "FullCostWithoutNDS_Value";
            this.FullCostWithoutNDS_Value.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.FullCostWithoutNDS_Value.Properties.ReadOnly = true;
            this.FullCostWithoutNDS_Value.Size = new System.Drawing.Size(122, 20);
            this.FullCostWithoutNDS_Value.TabIndex = 10;
            // 
            // IncludeInAccount
            // 
            this.IncludeInAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IncludeInAccount.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IncludeInAccount.Appearance.Options.UseFont = true;
            this.IncludeInAccount.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IncludeInAccount.AppearanceCaption.Options.UseFont = true;
            this.IncludeInAccount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.IncludeInAccount.Controls.Add(this.CostNDS_Value);
            this.IncludeInAccount.Controls.Add(this.CostNDS_Label);
            this.IncludeInAccount.Controls.Add(this.CostWithNDS_Label);
            this.IncludeInAccount.Controls.Add(this.CostWithNDS_Value);
            this.IncludeInAccount.Controls.Add(this.CostWithoutNDS_Label);
            this.IncludeInAccount.Controls.Add(this.CostWithoutNDS_Value);
            this.IncludeInAccount.Location = new System.Drawing.Point(18, 485);
            this.IncludeInAccount.Name = "IncludeInAccount";
            this.IncludeInAccount.Size = new System.Drawing.Size(511, 87);
            this.IncludeInAccount.TabIndex = 10;
            this.IncludeInAccount.Text = "Стоимость работ, включенных в счет:";
            // 
            // CalculationFile_Label
            // 
            this.CalculationFile_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CalculationFile_Label.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CalculationFile_Label.Location = new System.Drawing.Point(187, 451);
            this.CalculationFile_Label.Name = "CalculationFile_Label";
            this.CalculationFile_Label.Size = new System.Drawing.Size(117, 13);
            this.CalculationFile_Label.TabIndex = 11;
            this.CalculationFile_Label.Text = "Бланк калькуляции:";
            // 
            // CalculationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1090, 593);
            this.Controls.Add(this.CalculationFile_Label);
            this.Controls.Add(this.IncludeInAccount);
            this.Controls.Add(this.FullCost);
            this.Controls.Add(this.CalculationGridControl);
            this.Controls.Add(this.NDS_Value);
            this.Controls.Add(this.NDS_Label);
            this.Controls.Add(this.CalculationFile_Value);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalculationsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Калькуляция";
            ((System.ComponentModel.ISupportInitialize)(this.NDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostWithoutNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostWithNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationFile_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CalculationGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCost)).EndInit();
            this.FullCost.ResumeLayout(false);
            this.FullCost.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostWithNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullCostWithoutNDS_Value.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IncludeInAccount)).EndInit();
            this.IncludeInAccount.ResumeLayout(false);
            this.IncludeInAccount.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit NDS_Value;
        private DevExpress.XtraEditors.SpinEdit CostWithoutNDS_Value;
        private DevExpress.XtraEditors.SpinEdit CostWithNDS_Value;
        private DevExpress.XtraEditors.SpinEdit CostNDS_Value;
        private DevExpress.XtraEditors.LabelControl NDS_Label;
        private DevExpress.XtraEditors.LabelControl CostWithoutNDS_Label;
        private DevExpress.XtraEditors.LabelControl CostWithNDS_Label;
        private DevExpress.XtraEditors.LabelControl CostNDS_Label;
        private DevExpress.XtraEditors.ButtonEdit CalculationFile_Value;
        private DevExpress.XtraGrid.GridControl CalculationGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CalculationGridView;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceName;
        private DevExpress.XtraGrid.Columns.GridColumn BlockName;
        private DevExpress.XtraGrid.Columns.GridColumn WorkName;
        private DevExpress.XtraGrid.Columns.GridColumn Improvements;
        private DevExpress.XtraGrid.Columns.GridColumn Price;
        private DevExpress.XtraGrid.Columns.GridColumn Count;
        private DevExpress.XtraGrid.Columns.GridColumn Cost;
        private DevExpress.XtraGrid.Columns.GridColumn Include;
        private DevExpress.XtraEditors.GroupControl FullCost;
        private DevExpress.XtraEditors.SpinEdit FullCostNDS_Value;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit FullCostWithNDS_Value;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit FullCostWithoutNDS_Value;
        private DevExpress.XtraEditors.GroupControl IncludeInAccount;
        private DevExpress.XtraEditors.LabelControl CalculationFile_Label;
        private DevExpress.XtraGrid.Columns.GridColumn Comment;
    }
}