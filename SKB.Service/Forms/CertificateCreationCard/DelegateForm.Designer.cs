namespace SKB.Service.Forms.CertificateCreationCard
{
    partial class DelegateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelegateForm));
            this.lc = new DevExpress.XtraLayout.LayoutControl();
            this.Button_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Button_Ok = new DevExpress.XtraEditors.SimpleButton();
            this.Edit_Comment = new DevExpress.XtraEditors.MemoEdit();
            this.Edit_Delegate = new DevExpress.XtraEditors.ButtonEdit();
            this.lcg = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lci_Button_Ok = new DevExpress.XtraLayout.LayoutControlItem();
            this.lci_Button_Cancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lci_Edit_Comment = new DevExpress.XtraLayout.LayoutControlItem();
            this.lci_Edit_Delegate = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.lc)).BeginInit();
            this.lc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_Comment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_Delegate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Button_Ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Button_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Edit_Comment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Edit_Delegate)).BeginInit();
            this.SuspendLayout();
            // 
            // lc
            // 
            this.lc.AllowCustomizationMenu = false;
            this.lc.Controls.Add(this.Button_Cancel);
            this.lc.Controls.Add(this.Button_Ok);
            this.lc.Controls.Add(this.Edit_Comment);
            this.lc.Controls.Add(this.Edit_Delegate);
            this.lc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc.Location = new System.Drawing.Point(0, 0);
            this.lc.Name = "lc";
            this.lc.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(593, 63, 250, 350);
            this.lc.OptionsView.UseDefaultDragAndDropRendering = false;
            this.lc.Root = this.lcg;
            this.lc.Size = new System.Drawing.Size(392, 198);
            this.lc.TabIndex = 0;
            this.lc.Text = "lc";
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Cancel.Location = new System.Drawing.Point(282, 164);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(98, 22);
            this.Button_Cancel.StyleController = this.lc;
            this.Button_Cancel.TabIndex = 9;
            this.Button_Cancel.Text = "Отмена";
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button_Ok
            // 
            this.Button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_Ok.Location = new System.Drawing.Point(171, 164);
            this.Button_Ok.Name = "Button_Ok";
            this.Button_Ok.Size = new System.Drawing.Size(97, 22);
            this.Button_Ok.StyleController = this.lc;
            this.Button_Ok.TabIndex = 8;
            this.Button_Ok.Text = "ОК";
            this.Button_Ok.Click += new System.EventHandler(this.Button_Click);
            // 
            // Edit_Comment
            // 
            this.Edit_Comment.Location = new System.Drawing.Point(12, 68);
            this.Edit_Comment.Name = "Edit_Comment";
            this.Edit_Comment.Size = new System.Drawing.Size(368, 92);
            this.Edit_Comment.StyleController = this.lc;
            this.Edit_Comment.TabIndex = 7;
            // 
            // Edit_Delegate
            // 
            this.Edit_Delegate.Location = new System.Drawing.Point(12, 28);
            this.Edit_Delegate.Name = "Edit_Delegate";
            this.Edit_Delegate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.Edit_Delegate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Edit_Delegate.Size = new System.Drawing.Size(368, 20);
            this.Edit_Delegate.StyleController = this.lc;
            this.Edit_Delegate.TabIndex = 4;
            this.Edit_Delegate.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Edit_Delegate_ButtonPressed);
            // 
            // lcg
            // 
            this.lcg.CustomizationFormText = "lcg";
            this.lcg.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcg.GroupBordersVisible = false;
            this.lcg.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lci_Button_Ok,
            this.lci_Button_Cancel,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.lci_Edit_Comment,
            this.lci_Edit_Delegate});
            this.lcg.Location = new System.Drawing.Point(0, 0);
            this.lcg.Name = "lcg";
            this.lcg.Size = new System.Drawing.Size(392, 198);
            this.lcg.Text = "lcg";
            this.lcg.TextVisible = false;
            // 
            // lci_Button_Ok
            // 
            this.lci_Button_Ok.Control = this.Button_Ok;
            this.lci_Button_Ok.CustomizationFormText = "lci_Button_Ok";
            this.lci_Button_Ok.Location = new System.Drawing.Point(159, 152);
            this.lci_Button_Ok.Name = "lci_Button_Ok";
            this.lci_Button_Ok.Size = new System.Drawing.Size(101, 26);
            this.lci_Button_Ok.Text = "lci_Button_Ok";
            this.lci_Button_Ok.TextSize = new System.Drawing.Size(0, 0);
            this.lci_Button_Ok.TextToControlDistance = 0;
            this.lci_Button_Ok.TextVisible = false;
            // 
            // lci_Button_Cancel
            // 
            this.lci_Button_Cancel.Control = this.Button_Cancel;
            this.lci_Button_Cancel.CustomizationFormText = "lci_Button_Cancel";
            this.lci_Button_Cancel.Location = new System.Drawing.Point(270, 152);
            this.lci_Button_Cancel.Name = "lci_Button_Cancel";
            this.lci_Button_Cancel.Size = new System.Drawing.Size(102, 26);
            this.lci_Button_Cancel.Text = "lci_Button_Cancel";
            this.lci_Button_Cancel.TextSize = new System.Drawing.Size(0, 0);
            this.lci_Button_Cancel.TextToControlDistance = 0;
            this.lci_Button_Cancel.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 152);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(159, 26);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(260, 152);
            this.emptySpaceItem2.MaxSize = new System.Drawing.Size(10, 0);
            this.emptySpaceItem2.MinSize = new System.Drawing.Size(10, 10);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(10, 26);
            this.emptySpaceItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem2.Text = "emptySpaceItem2";
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lci_Edit_Comment
            // 
            this.lci_Edit_Comment.Control = this.Edit_Comment;
            this.lci_Edit_Comment.CustomizationFormText = "Комментарий";
            this.lci_Edit_Comment.Location = new System.Drawing.Point(0, 40);
            this.lci_Edit_Comment.Name = "lci_Edit_Comment";
            this.lci_Edit_Comment.Size = new System.Drawing.Size(372, 112);
            this.lci_Edit_Comment.Text = "Комментарий";
            this.lci_Edit_Comment.TextLocation = DevExpress.Utils.Locations.Top;
            this.lci_Edit_Comment.TextSize = new System.Drawing.Size(67, 13);
            // 
            // lci_Edit_Delegate
            // 
            this.lci_Edit_Delegate.Control = this.Edit_Delegate;
            this.lci_Edit_Delegate.CustomizationFormText = "Кому";
            this.lci_Edit_Delegate.Location = new System.Drawing.Point(0, 0);
            this.lci_Edit_Delegate.Name = "lci_Edit_Delegate";
            this.lci_Edit_Delegate.Size = new System.Drawing.Size(372, 40);
            this.lci_Edit_Delegate.Text = "Кому";
            this.lci_Edit_Delegate.TextLocation = DevExpress.Utils.Locations.Top;
            this.lci_Edit_Delegate.TextSize = new System.Drawing.Size(67, 13);
            // 
            // DelegateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 198);
            this.Controls.Add(this.lc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DelegateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Делегирование";
            ((System.ComponentModel.ISupportInitialize)(this.lc)).EndInit();
            this.lc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Edit_Comment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edit_Delegate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Button_Ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Button_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Edit_Comment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lci_Edit_Delegate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl lc;
        private DevExpress.XtraEditors.SimpleButton Button_Cancel;
        private DevExpress.XtraEditors.SimpleButton Button_Ok;
        private DevExpress.XtraEditors.MemoEdit Edit_Comment;
        private DevExpress.XtraEditors.ButtonEdit Edit_Delegate;
        private DevExpress.XtraLayout.LayoutControlGroup lcg;
        private DevExpress.XtraLayout.LayoutControlItem lci_Button_Ok;
        private DevExpress.XtraLayout.LayoutControlItem lci_Button_Cancel;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem lci_Edit_Comment;
        private DevExpress.XtraLayout.LayoutControlItem lci_Edit_Delegate;
    }
}