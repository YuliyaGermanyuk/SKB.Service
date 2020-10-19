namespace SKB.Service.Forms.ServiceCard
{
    partial class RepairsAndImprovements
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
            this.TBlock = new DevExpress.XtraEditors.ButtonEdit();
            this.TKind = new DevExpress.XtraEditors.ButtonEdit();
            this.LBlock = new DevExpress.XtraEditors.LabelControl();
            this.LKind = new DevExpress.XtraEditors.LabelControl();
            this.TDescription = new DevExpress.XtraEditors.MemoEdit();
            this.TWayToSolve = new DevExpress.XtraEditors.MemoEdit();
            this.TComments = new DevExpress.XtraEditors.MemoEdit();
            this.LDescription = new DevExpress.XtraEditors.LabelControl();
            this.LWayOfSolve = new DevExpress.XtraEditors.LabelControl();
            this.LComments = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TWork = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.TWorkID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.TImprovement = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.TPerformer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.TPerformerID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TLaboriousness = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TEndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.AddButton = new DevExpress.XtraEditors.SimpleButton();
            this.DeleteButton = new DevExpress.XtraEditors.SimpleButton();
            this.OKButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancButton = new DevExpress.XtraEditors.SimpleButton();
            this.LTable = new DevExpress.XtraEditors.LabelControl();
            this.LReplacement = new DevExpress.XtraEditors.LabelControl();
            this.LOldProductAction = new DevExpress.XtraEditors.LabelControl();
            this.TOldProductAction = new DevExpress.XtraEditors.ComboBoxEdit();
            this.TReplacement = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.TBlock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TWayToSolve.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TComments.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TOldProductAction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TReplacement.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // TBlock
            // 
            this.TBlock.Location = new System.Drawing.Point(17, 32);
            this.TBlock.Name = "TBlock";
            this.TBlock.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TBlock.Properties.Appearance.Options.UseFont = true;
            this.TBlock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.TBlock.Size = new System.Drawing.Size(381, 20);
            this.TBlock.TabIndex = 0;
            this.TBlock.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.TBlock_ButtonPressed);
            // 
            // TKind
            // 
            this.TKind.Location = new System.Drawing.Point(404, 32);
            this.TKind.Name = "TKind";
            this.TKind.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TKind.Properties.Appearance.Options.UseFont = true;
            this.TKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.TKind.Size = new System.Drawing.Size(381, 20);
            this.TKind.TabIndex = 1;
            this.TKind.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.TKind_ButtonPressed);
            // 
            // LBlock
            // 
            this.LBlock.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LBlock.Location = new System.Drawing.Point(17, 15);
            this.LBlock.Name = "LBlock";
            this.LBlock.Size = new System.Drawing.Size(100, 13);
            this.LBlock.TabIndex = 2;
            this.LBlock.Text = "Сборочный узел:";
            // 
            // LKind
            // 
            this.LKind.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LKind.Location = new System.Drawing.Point(404, 15);
            this.LKind.Name = "LKind";
            this.LKind.Size = new System.Drawing.Size(192, 13);
            this.LKind.TabIndex = 3;
            this.LKind.Text = "Классификация неисправности:";
            // 
            // TDescription
            // 
            this.TDescription.Location = new System.Drawing.Point(17, 80);
            this.TDescription.Name = "TDescription";
            this.TDescription.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TDescription.Properties.Appearance.Options.UseFont = true;
            this.TDescription.Size = new System.Drawing.Size(381, 138);
            this.TDescription.TabIndex = 6;
            this.TDescription.EditValueChanged += new System.EventHandler(this.TDescription_EditValueChanged);
            // 
            // TWayToSolve
            // 
            this.TWayToSolve.Location = new System.Drawing.Point(404, 80);
            this.TWayToSolve.Name = "TWayToSolve";
            this.TWayToSolve.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TWayToSolve.Properties.Appearance.Options.UseFont = true;
            this.TWayToSolve.Size = new System.Drawing.Size(381, 138);
            this.TWayToSolve.TabIndex = 7;
            this.TWayToSolve.EditValueChanged += new System.EventHandler(this.TWayToSolve_EditValueChanged);
            // 
            // TComments
            // 
            this.TComments.Location = new System.Drawing.Point(17, 491);
            this.TComments.Name = "TComments";
            this.TComments.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TComments.Properties.Appearance.Options.UseFont = true;
            this.TComments.Size = new System.Drawing.Size(654, 58);
            this.TComments.TabIndex = 8;
            this.TComments.EditValueChanged += new System.EventHandler(this.TComments_EditValueChanged);
            // 
            // LDescription
            // 
            this.LDescription.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LDescription.Location = new System.Drawing.Point(17, 63);
            this.LDescription.Name = "LDescription";
            this.LDescription.Size = new System.Drawing.Size(53, 13);
            this.LDescription.TabIndex = 9;
            this.LDescription.Text = "Описание:";
            // 
            // LWayOfSolve
            // 
            this.LWayOfSolve.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LWayOfSolve.Location = new System.Drawing.Point(404, 63);
            this.LWayOfSolve.Name = "LWayOfSolve";
            this.LWayOfSolve.Size = new System.Drawing.Size(101, 13);
            this.LWayOfSolve.TabIndex = 10;
            this.LWayOfSolve.Text = "Способ устранения:";
            // 
            // LComments
            // 
            this.LComments.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LComments.Location = new System.Drawing.Point(17, 472);
            this.LComments.Name = "LComments";
            this.LComments.Size = new System.Drawing.Size(73, 13);
            this.LComments.TabIndex = 11;
            this.LComments.Text = "Комментарии:";
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "Table1";
            this.gridControl1.Location = new System.Drawing.Point(17, 252);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemSpinEdit1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemButtonEdit2,
            this.repositoryItemComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(737, 206);
            this.gridControl1.TabIndex = 12;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TWork,
            this.TWorkID,
            this.TCount,
            this.TImprovement,
            this.TPerformer,
            this.TPerformerID,
            this.TLaboriousness,
            this.TEndDate,
            this.TResult});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ViewCaption = "Ремонтные работы и доработки:";
            // 
            // TWork
            // 
            this.TWork.Caption = "Вид работ";
            this.TWork.ColumnEdit = this.repositoryItemButtonEdit1;
            this.TWork.FieldName = "TW";
            this.TWork.Name = "TWork";
            this.TWork.OptionsColumn.ReadOnly = true;
            this.TWork.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.TWork.Visible = true;
            this.TWork.VisibleIndex = 0;
            this.TWork.Width = 207;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            this.repositoryItemButtonEdit1.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit1_ButtonPressed);
            // 
            // TWorkID
            // 
            this.TWorkID.Caption = "ID вида работ";
            this.TWorkID.FieldName = "TWID";
            this.TWorkID.Name = "TWorkID";
            this.TWorkID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // TCount
            // 
            this.TCount.Caption = "Количество";
            this.TCount.ColumnEdit = this.repositoryItemSpinEdit1;
            this.TCount.FieldName = "TC";
            this.TCount.Name = "TCount";
            this.TCount.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.TCount.Visible = true;
            this.TCount.VisibleIndex = 1;
            this.TCount.Width = 54;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.IsFloatValue = false;
            this.repositoryItemSpinEdit1.Mask.EditMask = "N00";
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // TImprovement
            // 
            this.TImprovement.Caption = "Доработка";
            this.TImprovement.ColumnEdit = this.repositoryItemCheckEdit1;
            this.TImprovement.FieldName = "TI";
            this.TImprovement.Name = "TImprovement";
            this.TImprovement.OptionsColumn.ReadOnly = true;
            this.TImprovement.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.TImprovement.Visible = true;
            this.TImprovement.VisibleIndex = 2;
            this.TImprovement.Width = 54;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // TPerformer
            // 
            this.TPerformer.Caption = "Исполнитель";
            this.TPerformer.ColumnEdit = this.repositoryItemButtonEdit2;
            this.TPerformer.FieldName = "TP";
            this.TPerformer.Name = "TPerformer";
            this.TPerformer.OptionsColumn.ReadOnly = true;
            this.TPerformer.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.TPerformer.Visible = true;
            this.TPerformer.VisibleIndex = 3;
            this.TPerformer.Width = 137;
            // 
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            this.repositoryItemButtonEdit2.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit2_ButtonPressed);
            // 
            // TPerformerID
            // 
            this.TPerformerID.Caption = "ID исполнителя";
            this.TPerformerID.FieldName = "TPID";
            this.TPerformerID.Name = "TPerformerID";
            this.TPerformerID.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.TPerformerID.Width = 50;
            // 
            // TLaboriousness
            // 
            this.TLaboriousness.Caption = "Трудоемкость";
            this.TLaboriousness.FieldName = "TL";
            this.TLaboriousness.Name = "TLaboriousness";
            this.TLaboriousness.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.TLaboriousness.Visible = true;
            this.TLaboriousness.VisibleIndex = 4;
            this.TLaboriousness.Width = 81;
            // 
            // TEndDate
            // 
            this.TEndDate.Caption = "Дата завершения";
            this.TEndDate.FieldName = "TED";
            this.TEndDate.Name = "TEndDate";
            this.TEndDate.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.TEndDate.Visible = true;
            this.TEndDate.VisibleIndex = 5;
            this.TEndDate.Width = 109;
            // 
            // TResult
            // 
            this.TResult.Caption = "Решение клиента";
            this.TResult.ColumnEdit = this.repositoryItemComboBox1;
            this.TResult.FieldName = "TR";
            this.TResult.Name = "TResult";
            this.TResult.OptionsColumn.ReadOnly = true;
            this.TResult.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.TResult.Visible = true;
            this.TResult.VisibleIndex = 6;
            this.TResult.Width = 90;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "Не согласовано",
            "Выполнить",
            "Не выполнять"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // AddButton
            // 
            this.AddButton.Image = global::SKB.Service.Properties.Resources.add;
            this.AddButton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.AddButton.Location = new System.Drawing.Point(756, 252);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(29, 29);
            this.AddButton.TabIndex = 13;
            this.AddButton.Text = "simpleButton1";
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Image = global::SKB.Service.Properties.Resources.delete;
            this.DeleteButton.Location = new System.Drawing.Point(756, 283);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(29, 29);
            this.DeleteButton.TabIndex = 14;
            this.DeleteButton.Text = "simpleButton2";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(682, 493);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(103, 23);
            this.OKButton.TabIndex = 15;
            this.OKButton.Text = "Сохранить";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancButton
            // 
            this.CancButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancButton.Location = new System.Drawing.Point(682, 526);
            this.CancButton.Name = "CancButton";
            this.CancButton.Size = new System.Drawing.Size(103, 23);
            this.CancButton.TabIndex = 16;
            this.CancButton.Text = "Отмена";
            this.CancButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // LTable
            // 
            this.LTable.Location = new System.Drawing.Point(17, 233);
            this.LTable.Name = "LTable";
            this.LTable.Size = new System.Drawing.Size(212, 13);
            this.LTable.TabIndex = 17;
            this.LTable.Text = "Перечень ремонтных работ и доработок:";
            // 
            // LReplacement
            // 
            this.LReplacement.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LReplacement.Location = new System.Drawing.Point(17, 559);
            this.LReplacement.Name = "LReplacement";
            this.LReplacement.Size = new System.Drawing.Size(42, 13);
            this.LReplacement.TabIndex = 19;
            this.LReplacement.Text = "Замена:";
            // 
            // LOldProductAction
            // 
            this.LOldProductAction.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LOldProductAction.Location = new System.Drawing.Point(404, 559);
            this.LOldProductAction.Name = "LOldProductAction";
            this.LOldProductAction.Size = new System.Drawing.Size(158, 13);
            this.LOldProductAction.TabIndex = 20;
            this.LOldProductAction.Text = "Неремонтопригодное изделие:";
            // 
            // TOldProductAction
            // 
            this.TOldProductAction.Location = new System.Drawing.Point(568, 556);
            this.TOldProductAction.Name = "TOldProductAction";
            this.TOldProductAction.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TOldProductAction.Properties.Items.AddRange(new object[] {
            "Списать",
            "Вернуть клиенту"});
            this.TOldProductAction.Properties.ReadOnly = true;
            this.TOldProductAction.Size = new System.Drawing.Size(100, 20);
            this.TOldProductAction.TabIndex = 21;
            this.TOldProductAction.EditValueChanged += new System.EventHandler(this.TOldProductAction_EditValueChanged);
            // 
            // TReplacement
            // 
            this.TReplacement.Location = new System.Drawing.Point(66, 556);
            this.TReplacement.Name = "TReplacement";
            this.TReplacement.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.TReplacement.Properties.ReadOnly = true;
            this.TReplacement.Size = new System.Drawing.Size(332, 20);
            this.TReplacement.TabIndex = 22;
            this.TReplacement.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.TReplacement_ButtonPressed);
            // 
            // RepairsAndImprovements
            // 
            this.AcceptButton = this.OKButton;
            this.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(806, 588);
            this.Controls.Add(this.TReplacement);
            this.Controls.Add(this.TOldProductAction);
            this.Controls.Add(this.LOldProductAction);
            this.Controls.Add(this.LReplacement);
            this.Controls.Add(this.LTable);
            this.Controls.Add(this.CancButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.LComments);
            this.Controls.Add(this.LWayOfSolve);
            this.Controls.Add(this.LDescription);
            this.Controls.Add(this.TComments);
            this.Controls.Add(this.TWayToSolve);
            this.Controls.Add(this.TDescription);
            this.Controls.Add(this.LKind);
            this.Controls.Add(this.LBlock);
            this.Controls.Add(this.TKind);
            this.Controls.Add(this.TBlock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RepairsAndImprovements";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ремонтные работы и доработки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RepairsAndImprovements_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.TBlock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TWayToSolve.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TComments.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TOldProductAction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TReplacement.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit TBlock;
        private DevExpress.XtraEditors.ButtonEdit TKind;
        private DevExpress.XtraEditors.LabelControl LBlock;
        private DevExpress.XtraEditors.LabelControl LKind;
        private DevExpress.XtraEditors.MemoEdit TDescription;
        private DevExpress.XtraEditors.MemoEdit TWayToSolve;
        private DevExpress.XtraEditors.MemoEdit TComments;
        private DevExpress.XtraEditors.LabelControl LDescription;
        private DevExpress.XtraEditors.LabelControl LWayOfSolve;
        private DevExpress.XtraEditors.LabelControl LComments;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn TWorkID;
        private DevExpress.XtraGrid.Columns.GridColumn TCount;
        private DevExpress.XtraGrid.Columns.GridColumn TImprovement;
        /// <summary>
        /// Наименование ремонтной работы.
        /// </summary>
        public DevExpress.XtraGrid.Columns.GridColumn TWork;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.SimpleButton AddButton;
        private DevExpress.XtraEditors.SimpleButton DeleteButton;
        private DevExpress.XtraEditors.SimpleButton OKButton;
        private DevExpress.XtraEditors.SimpleButton CancButton;
        private DevExpress.XtraEditors.LabelControl LTable;
        /// <summary>
        /// Исполнитель работы.
        /// </summary>
        public DevExpress.XtraGrid.Columns.GridColumn TPerformer;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn TPerformerID;
        /// <summary>
        /// Фактическая трудоемкость работы.
        /// </summary>
        public DevExpress.XtraGrid.Columns.GridColumn TLaboriousness;
        /// <summary>
        /// Дата окончания работы.
        /// </summary>
        public DevExpress.XtraGrid.Columns.GridColumn TEndDate;
        private DevExpress.XtraGrid.Columns.GridColumn TResult;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.LabelControl LReplacement;
        private DevExpress.XtraEditors.LabelControl LOldProductAction;
        private DevExpress.XtraEditors.ComboBoxEdit TOldProductAction;
        private DevExpress.XtraEditors.ButtonEdit TReplacement;
    }
}