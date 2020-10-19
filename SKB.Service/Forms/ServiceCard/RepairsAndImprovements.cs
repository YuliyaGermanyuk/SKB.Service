using System;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SearchModel;
using DocsVision.Platform.ObjectManager.Metadata;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.WinForms.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Controls;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.Platform.CardHost;
using DocsVision.TakeOffice.Cards.Constants;
using SKB.Base;
using RKIT.MyMessageBox;
using SKB.Base.Ref;

namespace SKB.Service.Forms.ServiceCard
{
    /// <summary>
    /// Форма редактирования таблицы "Описание неисправностей".
    /// </summary>
    public partial class RepairsAndImprovements : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Карточка сервисного обслуживания.
        /// </summary>
        public MyBaseCard Card;
        /// <summary>
        /// Карточка Универсального справочника.
        /// </summary>
        public CardData UniversalCard;
        /// <summary>
        /// Тип прибора.
        /// </summary>
        public string DeviceType;
        /// <summary>
        /// ID паспорта прибора.
        /// </summary>
        public string DeviceID;
        /// <summary>
        /// Заводской номер прибора.
        /// </summary>
        public string DeviceNumber;
        /// <summary>
        /// ID сборочного узла.
        /// </summary>
        public string BlockID;
        /// <summary>
        /// Секция "Наименование сборочного узла".
        /// </summary>
        public string Block;
        /// <summary>
        /// ID класса неисправности.
        /// </summary>
        public string KindID;
        /// <summary>
        /// Класс неисправности.
        /// </summary>
        public string Kind;
        /// <summary>
        /// Описание неисправности.
        /// </summary>
        public string Description;
        /// <summary>
        /// Способ устранения неисправности.
        /// </summary>
        public string WayToSolve;
        /// <summary>
        /// Комментарий.
        /// </summary>
        public string Comments;
        /// <summary>
        /// Перечень ремонтных работ.
        /// </summary>
        public ArrayList Works;
        /// <summary>
        /// Перечень ремонтных работ (текст).
        /// </summary>
        public string WorksList = "";
        /// <summary>
        /// Замена.
        /// </summary>
        public string Replacement = "";
        /// <summary>
        /// ID замены.
        /// </summary>
        public string ReplacementID = "";
        /// <summary>
        /// Действие со старым изделием.
        /// </summary>
        public string OldProductAction = "";
        /// <summary>
        /// Таблица "Ремонтные работы и доработки".
        /// </summary>
        public System.Data.DataTable Table;
        /// <summary>
        /// Принять изменения.
        /// </summary>
        public bool Acceptance;
        /// <summary>
        /// .
        /// </summary>
        bool f;
        /// <summary>
        ///  Справочник классификации неисправностей.
        /// </summary>
        public string ClassificationDic =  "{0EAD99A9-5838-4C0C-9CE5-D1D483430C7A}";
        /// <summary>
        ///  Форма описания неисправностей.
        /// </summary>
        public RepairsAndImprovements(MyBaseCard Card, CardData UniversalCard, string DeviceID, string DeviceNumber, string pDeviceType, string pBlockID, string pKindID, 
            string pDescription, string pWayToSolve, string pComments, ArrayList pWorks, string pReplacementID = "", string pOldValueAction = "")
        {
            this.Card = Card;
            this.DeviceType = pDeviceType;
            this.DeviceID = DeviceID;
            this.DeviceNumber = DeviceNumber;
            this.InitializeComponent();
            this.BlockID = pBlockID;
            this.Block = BlockID == "" ? "" : UniversalCard.GetItemName(pBlockID.ToGuid());
            this.KindID = pKindID;
            this.Kind = KindID == "" ? "" : UniversalCard.GetItemName(pKindID.ToGuid());
            this.Description = pDescription;
            this.WayToSolve = pWayToSolve;
            this.Comments = pComments;
            this.Works = pWorks;
            this.UniversalCard = UniversalCard;
            this.ReplacementID = pReplacementID;
            this.Replacement = pReplacementID == "" ? "" : Card.CardScript.Session.CardManager.GetCardData(new Guid(pReplacementID)).Description;
            this.OldProductAction = pOldValueAction;

            this.TBlock.Text = Block;
            this.TKind.Text = Kind;
            this.TDescription.Text = Description;
            this.TWayToSolve.Text = WayToSolve;
            this.TComments.Text = Comments;
            this.TReplacement.Text = Replacement;
            this.TOldProductAction.EditValue = OldProductAction;

            Guid SalesManagerID = Card.GetControlValue(RefServiceCard.MainInfo.Manager).ToGuid();
            
            if (SalesManagerID.Equals(Card.Context.GetObjectRef(Card.Context.GetCurrentEmployee()).Id) || (bool)Card.CardScript.Session.Properties["IsAdmin"].Value || 
                Card.Context.GetObjectRef(GetEmployeeByGroup(Card.Context, RefApplicationCard.Roles.ServiceManagerGroup)).Id.Equals(Card.Context.GetObjectRef(Card.Context.GetCurrentEmployee()).Id))
            {
                this.TResult.OptionsColumn.ReadOnly = false;
                foreach (Work Row in Works)
                {
                    int WorksType = (int)UniversalCard.GetItemPropertyValue(new Guid(Row.WorkID), "Тип работ");
                    if ((WorksType == 4)) // && (Row.Result == Work.NegotiationResult.Perform.GetDescription()))
                    {
                        this.TReplacement.Properties.ReadOnly = false;
                        this.TOldProductAction.Properties.ReadOnly = false;
                    }
                }
            }

            this.Text = this.Text + " " + DeviceNumber;

            Table = new System.Data.DataTable();
            Table.Columns.Add("TW");
            Table.Columns.Add("TWID");
            Table.Columns.Add("TC", typeof(int));
            Table.Columns.Add("TI", typeof(bool));
            Table.Columns.Add("TP", typeof(string));
            Table.Columns.Add("TPID", typeof(string));
            Table.Columns.Add("TL", typeof(decimal));
            Table.Columns.Add("TED", typeof(DateTime));
            Table.Columns.Add("TR", typeof(string));
            object[] Parametr = new object[9];
            foreach (Work Row in Works)
            {
                Parametr[0] = Row.WorkName;
                Parametr[1] = Row.WorkID;
                Parametr[2] = Row.Count;
                Parametr[3] = Row.Improvements;
                Parametr[4] = Row.Performer;
                Parametr[5] = Row.PerformerID;
                Parametr[6] = Row.FactLaboriousness;
                if (Row.EndDate != null) Parametr[7] = (DateTime)Row.EndDate;
                Parametr[8] = Row.Result;
                Table.Rows.Add(Parametr);
            }
            gridControl1.DataSource = Table;
        }
        /// <summary>
        ///  Выбор сборочного узла.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TBlock_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string BlocksDictionary = FindBlocksDictionary(UniversalCard, DeviceType);
            if (BlocksDictionary == "")
                return;
            object activateParam = new string[] { "{DD20BF9B-90F8-4D9A-9553-5B5F17AD724E}", BlockID, FindBlocksDictionary(UniversalCard, DeviceType) };
            object result = Card.CardScript.CardFrame.CardHost.SelectFromCard(new Guid("{B2A438B7-8BB3-4B13-AF6E-F2F8996E148B}"), "Выберите сборочный узел", activateParam);
            if (result != null)
            {
                BlockID = result.ToString();
                Block = UniversalCard.GetItemName(BlockID.ToGuid());
                this.TBlock.Text = Block;
            }
        }
        /// <summary>
        ///  Выбор классификации неисправности.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TKind_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            object activateParam = new string[] { "{DD20BF9B-90F8-4D9A-9553-5B5F17AD724E}", KindID, ClassificationDic}; 
            object result = Card.CardScript.CardFrame.CardHost.SelectFromCard(new Guid("{B2A438B7-8BB3-4B13-AF6E-F2F8996E148B}"), "Выберите вид неисправности", activateParam);
            if (result != null)
            {
                KindID = result.ToString();
                Kind = UniversalCard.GetItemName(KindID.ToGuid());
                this.TKind.Text = Kind;
            }
        }
        /// <summary>
        ///  Выбор вида ремонтных работ и доработок.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit1_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int[] index = gridView1.GetSelectedRows();
            bool Improvement;
            string WorksTypeID = gridView1.GetRowCellDisplayText(index[0], "TWID").ToString();
            object activateParam = new string[] { "{DD20BF9B-90F8-4D9A-9553-5B5F17AD724E}", WorksTypeID, "{43A6DA44-899C-47D8-9567-2185E05D8524}" };
            object result = Card.CardScript.CardFrame.CardHost.SelectFromCard(new Guid("{B2A438B7-8BB3-4B13-AF6E-F2F8996E148B}"), "Выберите вид ремонтных работ или доработку", activateParam);

            if (result != null)
            {
                // Проверка выбранной записи
                RowData WorkRow = UniversalCard.GetItemRow(result.ToGuid());
                int WorksType = (int)WorkRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Тип работ'").GetInt32("Value");
                RowDataCollection WorksDevices = WorkRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Приборы'").ChildSections[RefUniversal.SelectedValues.ID].Rows;

                if (WorksType == 3)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Выбранная вами позиция не является ремонтной работой или доработкой. Изменения внесены не будут.");
                    return;
                }
                if (!WorksDevices.Any(r => r.GetString(RefUniversal.SelectedValues.SelectedValue) == DeviceType))
                {
                    string DeviceTypeName = UniversalCard.GetItemName(new Guid(DeviceType));
                    DevExpress.XtraEditors.XtraMessageBox.Show("Выбранная вами позиция не относится к прибору " + DeviceTypeName + ". Изменения внесены не будут.");
                    return;
                }

                // Дбавление выбранной записи
                Improvement = WorksType == 2 ? true : false;
                foreach (int i in index)
                {
                    gridView1.SetRowCellValue(i, "TW", UniversalCard.GetItemName(result.ToGuid()));
                    gridView1.SetRowCellValue(i, "TWID", result.ToString());
                    gridView1.SetRowCellValue(i, "TI", Improvement);
                    gridView1.SetRowCellValue(i, "TR", Work.NegotiationResult.NotAgreed.GetDescription());
                }
                Table.AcceptChanges();
                WorksList = GetWorksList(Table);
            }
        }
        /// <summary>
        ///  Выбор исполнителя.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit2_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int[] index = gridView1.GetSelectedRows();
            string PerformerID = gridView1.GetRowCellDisplayText(index[0], "TPID").ToString();
            Object[] activateParams = new Object[] { 
                RefStaff.Employees.ID.ToString("B").ToUpper(), 
                PerformerID, 
                false, String.Empty, false, false, false };
            object result = Card.CardScript.CardFrame.CardHost.SelectFromCard(RefStaff.ID, "Выберите исполнителя ремонтных работ", activateParams);

            if (result != null)
            {
                foreach (int i in index)
                {
                    gridView1.SetRowCellValue(i, "TP", Card.Context.GetEmployeeDisplay(new Guid(result.ToString())));
                    gridView1.SetRowCellValue(i, "TPID", result.ToString());
                }
                Table.AcceptChanges();
                WorksList = GetWorksList(Table);
            }
        }
        /// <summary>
        ///  Удаление ремонтной работы/доработки.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int[] index;
            index = gridView1.GetSelectedRows();

            foreach (int i in index)
            {
                Table.Rows.RemoveAt(i);
            }
            WorksList = GetWorksList(Table);
        }
        /// <summary>
        ///  Добавление ремонтной работы/доработки.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            bool Improvement;
            object activateParam = new object[] { "{DD20BF9B-90F8-4D9A-9553-5B5F17AD724E}", "", "{43A6DA44-899C-47D8-9567-2185E05D8524}", null, null, true};
            VbaCollection result = (VbaCollection)Card.CardScript.CardFrame.CardHost.SelectFromCard(new Guid("{B2A438B7-8BB3-4B13-AF6E-F2F8996E148B}"), "Выберите вид ремонтных работ или доработку", activateParam);

            if (result != null)
            {
                foreach (object r in result)
                {
                    // Проверка выбранной записи
                    bool Verify = true;
                    RowData WorkRow = UniversalCard.GetItemRow(r.ToGuid());
                    int WorksType = (int)WorkRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Тип работ'").GetInt32(RefUniversal.Properties.Value);
                    RowDataCollection WorksDevices = WorkRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Приборы'").ChildSections[RefUniversal.SelectedValues.ID].Rows;
                    if (WorksType == 3)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Позиция '" + WorkRow.GetString(RefUniversal.Item.Name) + "' не является ремонтной работой или доработкой. Она не будет добавлена в перечень ремонтных работ.");
                        Verify = false;
                    }

                    if (!WorksDevices.Any(row => row.GetString(RefUniversal.SelectedValues.SelectedValue).ToGuid().Equals(DeviceType.ToGuid())))
                    {
                        string DeviceTypeName = UniversalCard.GetItemName(new Guid(DeviceType));
                        DevExpress.XtraEditors.XtraMessageBox.Show("Позиция '" + WorkRow.GetString(RefUniversal.Item.Name) + "' не относится к прибору " + DeviceTypeName + ". Она не будет добавлена в перечень ремонтных работ.");
                        Verify = false;
                    }

                    if (Verify)
                    {
                        // Добавление выбранной записи
                        Improvement = WorksType == 2 ? true : false;
                        object[] Parametr = new object[9];
                        Parametr[0] = UniversalCard.GetItemName(r.ToGuid());
                        Parametr[1] = r.ToString();
                        Parametr[2] = 1;
                        Parametr[3] = Improvement;
                        Parametr[4] = Card.Context.GetCurrentEmployee().DisplayName;
                        Parametr[5] = Card.Context.GetObjectRef(Card.Context.GetCurrentEmployee()).Id.ToString();
                        Parametr[6] = 0;
                        Parametr[7] = null;
                        Parametr[8] = Work.NegotiationResult.NotAgreed.GetDescription();
                        DataRow NewRow = Table.Rows.Add(Parametr);
                    }
                }
            }
        }
        /// <summary>
        ///  Сохранение изменений.
        /// </summary>
        private bool SaveChanges()
        {
            bool IsReplacement = false;
            bool NoAction = false;
            ArrayList PropertyList = new ArrayList();
            if (BlockID == "") { PropertyList.Add("Сборочный узел"); }
            if (Description == "") { PropertyList.Add("Описание причин неисправности"); }
            if (KindID == "") { PropertyList.Add("Классификация неисправности"); }
            if (WayToSolve == "") { PropertyList.Add("Способ устранения"); }

            for (int i = 0; i<gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "TWID").ToString() == "")
                {
                    PropertyList.Add("Вид работ");
                    break;
                }
                else
                {
                    int WorksType = (int)UniversalCard.GetItemPropertyValue(new Guid(gridView1.GetRowCellValue(i, "TWID").ToString()), "Тип работ");
                    if ((WorksType == 4) && (gridView1.GetRowCellValue(i, "TR").ToString() == Work.NegotiationResult.Perform.GetDescription()))
                    { 
                        IsReplacement = true;
                        if ((TOldProductAction.EditValue == null) || (TOldProductAction.EditValue.ToString() == ""))
                        { NoAction = true; }
                    }
                }
                if (gridView1.GetRowCellValue(i, "TC") == null || (int)gridView1.GetRowCellValue(i, "TC") <= 0)
                {
                    PropertyList.Add("Количество");
                    break;
                }
            }
            if (PropertyList.Count > 0)
            {
                string Text = "";
                for (int j = 0; j < PropertyList.Count - 1; j++)
                {
                    Text = Text + "- \"" + PropertyList[j] + "\";\n";
                }
                Text = Text + "- \"" + PropertyList[PropertyList.Count - 1] + "\".";

                MyMessageBox.Show("Для сохранения необходимо заполнить следующие поля:\n" + Text, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (NoAction)
                {
                    MyMessageBox.Show("Ошибка! Данное изделие должно быть заменено на новое. Укажите, какие действия необходимо совершить со старым изделием.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if ((!IsReplacement) && (TReplacement.Text != ""))
                {
                    MyMessageBox.Show("Ошибка! Данное изделие должно быть заменено на новое. Выберите соответствующую ремонтную рабту из справочника.","Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                Works.Clear();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    Card.WriteLog("Получаем дату");
                    Work W;
                    W = new Work(UniversalCard, gridView1.GetRowCellValue(i, "TWID").ToString(), (int)gridView1.GetRowCellValue(i, "TC"), gridView1.GetRowCellValue(i, "TP").ToString(),
                        gridView1.GetRowCellValue(i, "TPID").ToString(), (decimal)gridView1.GetRowCellValue(i, "TL"), GetEndDate(gridView1.GetRowCellValue(i, "TED")), gridView1.GetRowCellValue(i, "TR").ToString());
                    Works.Add(W);
                }
                WorksList = GetWorksList(Table);
                return true;
            }
        }
        /// <summary>
        ///  Подтверждение сохранения изменений.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            bool Verify = SaveChanges();
            if (Verify == true)
            {
                Acceptance = true;
                f = true;
                this.Close();
            }
        }
        /// <summary>
        ///  Отмена внесенных изменений.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Acceptance = false;
            f = true;
            this.Close();
        }
        /// <summary>
        ///  Закрытие формы.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void RepairsAndImprovements_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f == false)
            {
                DialogResult result = MyMessageBox.Show("Сохранить изменения?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool Verify = SaveChanges();
                    if (Verify == true)
                    {Acceptance = true;}
                    else
                    {e.Cancel = true;}
                }
                else
                {Acceptance = false;}
            }
        }
        /// <summary>
        ///  Изменение описания неисправности.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TDescription_EditValueChanged(object sender, EventArgs e)
        {
            Description = this.TDescription.Text;
        }
        /// <summary>
        ///  Изменение способа решения проблемы.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TWayToSolve_EditValueChanged(object sender, EventArgs e)
        {
            WayToSolve = this.TWayToSolve.Text;
            
        }
        /// <summary>
        ///  Изменение комментария.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TComments_EditValueChanged(object sender, EventArgs e)
        {
            Comments = TComments.Text;
        }
        /// <summary>
        ///  Формирование перечня ремонтных работ.
        /// </summary>
        /// <param name="T"> Таблица ресонтных работ.</param>
        private string GetWorksList(System.Data.DataTable T)
        {
            StringCollection SC = new StringCollection(); 
            string b = "";
            foreach(DataRow Row in T.Rows)
            {
                b = Row[0].ToString() + " (" + Row[2].ToString() + ")";
                SC.Add(b);
            }
            if (SC.Count > 0)
            {
                string[] c = new string[SC.Count];
                SC.CopyTo(c, 0);
                return string.Join("; ", c);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        ///  Поиск справочника сборочных узлов по типу прибора.
        /// </summary>
        /// <param name="UniversalCard"> Универсальный справочник.</param>
        /// <param name="DeviceType"> Тип прибора.</param>
        public static string FindBlocksDictionary(CardData UniversalCard, string DeviceType)
        {
            SectionData Items = UniversalCard.Sections[RefUniversal.Item.ID];
            RowData Item = Items.GetRow(new Guid(DeviceType));

            if (Item == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось найти прибор в 'Справочнике приборов и комплектующих'. Обратитесь к системному администратору.");
                return "";
            }
            else
            {
                string DicID = "";
                string DeviceName = Item.GetString("Name");
                RowData ItemsType = UniversalCard.Sections[RefUniversal.ItemType.ID].FindRow("@Name = 'Сборочные узлы и детали на комплектацию'");

                foreach (RowData Row in ItemsType.ChildRows)
                {
                    if (Row.GetString("Name") == DeviceName)
                    {
                        DicID = Row.GetString("RowID");
                        break;
                    }
                }
                if (DicID == "")
                { DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось найти 'Справочник сборочных узлов' для прибора '" + DeviceName + "'. Обратитесь к системному администратору."); }
                return DicID;
            }
        }
        /// <summary>
        ///  Перечень прибора и всех доп. изделий.
        /// </summary>
        /*public ArrayList FindAllDevices()
        {
            ArrayList DevicesList = new ArrayList();
            string Device = Card.GetControlValue(RefServiceCard.MainInfo.DeviceCardID) == null ? "" :
                Card.GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString();
            this.Card.WriteLog("Прибор: " + Device);
            DevicesList.Add(Device);

            GridView Grid_AdditionalWaresList = Card.ICardControl.GetGridView(RefServiceCard.AdditionalWaresList.Alias);
            ITableControl Table_AdditionalWaresList = Card.ICardControl.FindPropertyItem<ITableControl>(RefServiceCard.AdditionalWaresList.Alias);
            this.Card.WriteLog("Ищем датчики. Кол-во: " + Table_AdditionalWaresList.RowCount.ToString());
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Device = UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString();
                //DeviceName = UniversalCard.GetItemPropertyValue(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid(), "Наименование прибора").ToString();
                //if (!DevicesList.Contains(DeviceName))
                DevicesList.Add(Device);
                this.Card.WriteLog("Датчик: " + Device);
            }
            return DevicesList;
        }*/

        private void TReplacement_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!this.TReplacement.Properties.ReadOnly)
            {
                Guid DevicePassportTypeID = new Guid("{42826E25-AD0E-4D9C-8B18-CD88E6796972}");     // Тип - "Паспорт прибора"
                Guid DevicePassportFolderID = new Guid("{DDE96A16-438B-46DB-AC70-B477769E2124}");   // Папка - "[01] ПАСПОРТА ПРИБОРОВ"
                SearchQuery searchQuery = Card.CardScript.Session.CreateSearchQuery();
                searchQuery.CombineResults = ConditionGroupOperation.And;

                CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(CardOrd.ID);
                SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(CardOrd.MainInfo.ID);
                sectionQuery.Operation = SectionQueryOperation.And;
                sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
                sectionQuery.ConditionGroup.Conditions.AddNew("Type", FieldType.RefId, ConditionOperation.Equals, DevicePassportTypeID);

                sectionQuery = typeQuery.SectionQueries.AddNew(CardOrd.Properties.ID);
                sectionQuery.Operation = SectionQueryOperation.And;
                sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
                sectionQuery.ConditionGroup.Conditions.AddNew(CardOrd.Properties.Name, FieldType.String, ConditionOperation.Equals, "Состояние прибора");
                sectionQuery.ConditionGroup.Conditions.AddNew(CardOrd.Properties.Value, FieldType.Int, ConditionOperation.Equals, 3);

                sectionQuery = typeQuery.SectionQueries.AddNew(CardOrd.Properties.ID);
                sectionQuery.Operation = SectionQueryOperation.And;
                sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
                sectionQuery.ConditionGroup.Conditions.AddNew(CardOrd.Properties.Name, FieldType.String, ConditionOperation.Equals, "Прибор");
                sectionQuery.ConditionGroup.Conditions.AddNew(CardOrd.Properties.Value, FieldType.RefId, ConditionOperation.Equals, new Guid(DeviceType));

                // Получение текста запроса
                searchQuery.Limit = 0;
                string query = searchQuery.GetXml();

                Guid result = Card.CardScript.CardFrame.CardHost.SelectCard("Выберите паспорт изделия для замены:", DevicePassportFolderID, query);
                if (!result.Equals(Guid.Empty))
                {
                    ReplacementID = result.ToString();
                    Replacement = Card.CardScript.Session.CardManager.GetCardData(new Guid(ReplacementID)).Description;
                    this.TReplacement.Text = Replacement;
                }
            }
        }

        private void TOldProductAction_EditValueChanged(object sender, EventArgs e)
        {
            this.OldProductAction = this.TOldProductAction.EditValue.ToString();
        }

        private DateTime? GetEndDate(object EndDate)
        {
            try
            { return (DateTime?)EndDate; }
            catch
            { return null; }
        }
        /// <summary>
        /// Возвращает сотрудника по группе или его заместителя.
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="GroupName">Название группы.</param>
        /// <param name="GetDeputy">Возвращать заместителя при неактивности сотрудника в необходимой должности.</param>
        /// <returns></returns>
        public StaffEmployee GetEmployeeByGroup(ObjectContext Context, String GroupName, Boolean GetDeputy = true)
        {
            Staff staff = Context.GetObject<Staff>(RefStaff.ID);
            StaffGroup Group = staff.Groups.FirstOrDefault<StaffGroup>(r => r.Name == GroupName);

            if (!Group.IsNull())
            {
                List<StaffEmployee> FindedEmployees = Group.Employees.ToList(); //Context.FindObjects<StaffEmployee>(new QueryObject(RefStaff.Employees.Position, Context.GetObjectRef<StaffPosition>(Position).Id)).ToList();
                if (FindedEmployees.Any())
                {
                    List<StaffEmployee> Employees = FindedEmployees.Where(emp => emp.Status != StaffEmployeeStatus.Discharged && emp.Status != StaffEmployeeStatus.Transfered && emp.Status != StaffEmployeeStatus.DischargedNoRestoration).ToList();
                    if (Employees.Any())
                    {
                        StaffEmployee Employee = Employees.FirstOrDefault(emp => emp.Status == StaffEmployeeStatus.Active);
                        if (!Employee.IsNull())
                            return Employee;
                        else if (GetDeputy)
                            return Employees.Select(emp => emp.GetDeputy()).FirstOrDefault(emp => !emp.IsNull());
                    }
                }
            }
            return null;
        }
    }
}