using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.BackOffice.CardLib.CardDefs;

using DocsVision.Platform.CardHost;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.WinForms;
using DocsVision.Platform.ObjectManager.SearchModel;
using DocsVision.Platform.ObjectManager.Metadata;
using DocsVision.TakeOffice.Cards.Constants;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

using RKIT.MyMessageBox;
using RKIT.MyReferencesList.Design.Control;
using RKIT.MyFilesList.Design.Control;

using SKB.Base;
using SKB.Base.Enums;
using SKB.PaymentAndShipment;
using SKB.PaymentAndShipment.Cards;
using SKB.Service.Forms.CertificateCreationCard;
using SKB.Base.Ref;
using SKB.Service.Cards;
using SKB.Service.CalibrationDocs;

using DVDocument = DocsVision.BackOffice.ObjectModel.Document;

namespace SKB.Service.Cards
{
    /// <summary>
    /// Карточка "Заявка на выдачу сертификатов о калибровке".
    /// </summary>
    public class CertificateCreationCard : MyBaseCard
    {
        #region Properties
        /// <summary>
        /// Таблица "Приборы".
        /// </summary>
        ITableControl Table_Devices;
        /// <summary>
        /// Таблица "Дополнительные изделия".
        /// </summary>
        ITableControl Table_AdditionalWaresList;
        /// <summary>
        /// Таблица "История исполнения".
        /// </summary>
        ITableControl Table_History;
        /// <summary>
        /// Таблица "Приборы".
        /// </summary>
        GridView Grid_Devices;
        /// <summary>
        /// Таблица "Дополнительные изделия".
        /// </summary>
        GridView Grid_AdditionalWaresList;
        /// <summary>
        /// Таблица "История исполнения".
        /// </summary>
        GridView Grid_History;
        /// <summary>
        /// Список табличных изменений.
        /// </summary>
        List<DevicesTableChange> Dic_Changes;
        /// <summary>
        /// Бизнес-календарь СКБ.
        /// </summary>
        const string BusinessCalendarID = "{2DFC601B-451C-E411-A309-00155D016943}";
        /// <summary>
        /// Генератор случайных чисел.
        /// </summary>
        Random RandomValue = new Random();
        /// <summary>
        /// Создатель таблиц.
        /// </summary>
        TablesCreator Creator = new TablesCreator();
        Int32 ClickTicks = 0;
        /// <summary>
        /// Роль текущего пользователя
        /// </summary>
        String CurrentUserRole
        {
            get 
            {
                IStaffService StaffService = Context.GetService<IStaffService>();
                StaffEmployee CurrentEmployee = StaffService.GetCurrentEmployee();
                Guid CurrentEmployeeID = Context.GetObjectRef<StaffEmployee>(CurrentEmployee).Id;

                // Администратор
                if (StaffService.FindEmployeeGroups(CurrentEmployee).Any(r => r.Name == "DocsVision Administrators"))
                    return RefCertificateCreationCard.UserRoles.Admin;
                // Регистратор
                if (CurrentEmployeeID.Equals(GetControlValue(RefCertificateCreationCard.MainInfo.Creator).ToGuid()))
                    return RefCertificateCreationCard.UserRoles.Creator;
                // Начальник отдела калибровки и настройки аппаратуры
                if (StaffService.FindEmployeeGroups(CurrentEmployee).Any(r => r.Name == "FA Manager"))
                    return RefCertificateCreationCard.UserRoles.FAManager;
                // Текущий исполнитель
                if (CurrentEmployeeID.Equals(GetControlValue(RefCertificateCreationCard.MainInfo.Performer).ToGuid()))
                    return RefCertificateCreationCard.UserRoles.Performer;
                // Сотрудник отдела сбыта и маркетинга
                if (StaffService.FindEmployeeGroups(CurrentEmployee).Any(r => r.Name == "Sales and Marketing"))
                    return RefCertificateCreationCard.UserRoles.SalesDepartmentEmployee;
                if (StaffService.FindEmployeeGroups(CurrentEmployee).Any(r => r.Name == "SM manager"))
                    return RefCertificateCreationCard.UserRoles.SalesDepartmentEmployee;
                // Прочие сотрудники
                return RefCertificateCreationCard.UserRoles.AllUsers;
            }
        }
        /// <summary>
        /// Тип заявки Калибровка
        /// </summary>
        bool IsCalibration;
        #endregion
        /// <summary>
        /// Инициализирует карточку по заданным данным.
        /// </summary>
        /// <param name="ClassBase">Скрипт карточки.</param>
        /// <param name="e">Событие открытия карточки</param>
        public CertificateCreationCard(ScriptClassBase ClassBase, CardActivatedEventArgs e)
            : base(ClassBase)
        {
            try
            {
                /*if (this.CardScript.CardControl.ActivateFlags.HasFlag(ActivateFlags.New))
                {
                    CalibrationOrVerificationForm COrVForm = new CalibrationOrVerificationForm();
                    COrVForm.ShowDialog();
                    if (COrVForm.IsCalibration == null)
                    { CardScript.CardFrame.CardHost.CloseCards(); }
                    else
                    {
                        if ((bool)COrVForm.IsCalibration)
                        {
                            this.IsCalibration = true;
                            CardScript.UpdateDescription("Заявка на калибровку");
                        }
                        else
                        {
                            this.IsCalibration = false;
                            CardScript.UpdateDescription("Заявка на поверку");
                        }
                    }

                }
                else
                {
                    if (CardScript.CardData.Description.IndexOf("Заявка на калибровку") >= 0)
                    {
                        this.IsCalibration = true;
                        CardScript.UpdateDescription(CardScript.CardData.Description);
                    }
                    else
                    {
                        this.IsCalibration = false;
                        CardScript.UpdateDescription(CardScript.CardData.Description);
                    }
                }*/
                if (GetControlValue(RefCertificateCreationCard.MainInfo.ApplicationType) == null)
                {
                    this.IsCalibration = true;
                    SetControlValue(RefCertificateCreationCard.MainInfo.ApplicationType, 0);
                }
                else
                {
                    switch ((Int32)GetControlValue(RefCertificateCreationCard.MainInfo.ApplicationType))
                    {
                        case 0:
                            this.IsCalibration = true;
                            break;
                        case 1:
                            this.IsCalibration = false;
                            break;
                    }
                    CardScript.UpdateDescription(CardScript.CardData.Description);
                }


                /* Получение рабочих объектов */
                Table_Devices = ICardControl.FindPropertyItem<ITableControl>(RefCertificateCreationCard.Devices.Alias);
                Table_AdditionalWaresList = ICardControl.FindPropertyItem<ITableControl>(RefCertificateCreationCard.AdditionalWaresList.Alias);
                Table_History = ICardControl.FindPropertyItem<ITableControl>(RefCertificateCreationCard.History.Alias);

                Grid_Devices = ICardControl.GetGridView(RefCertificateCreationCard.Devices.Alias);
                Grid_AdditionalWaresList = ICardControl.GetGridView(RefCertificateCreationCard.AdditionalWaresList.Alias);
                Grid_History = ICardControl.GetGridView(RefCertificateCreationCard.History.Alias);
                
                /* Получение номера */
                if (GetControlValue(RefCertificateCreationCard.MainInfo.Number).ToGuid().IsEmpty())
                {
                    if (IsCalibration)
                        CurrentNumerator = CardScript.GetNumber(RefCertificateCreationCard.CalibrationNumberRuleName);
                    else
                        CurrentNumerator = CardScript.GetNumber(RefCertificateCreationCard.VerificationNumberRuleName);
                    CurrentNumerator.Number = Convert.ToInt32(CurrentNumerator.Number).ToString("00000");
                    SetControlValue(RefCertificateCreationCard.MainInfo.Number, Context.GetObjectRef<BaseCardNumber>(CurrentNumerator).Id);
                    WriteLog("Выдали номер: " + CurrentNumerator.Number);
                }
                else
                {
                    CurrentNumerator = Context.GetObject<BaseCardNumber>(GetControlValue(RefCertificateCreationCard.MainInfo.Number));
                }

                /* Заполнение списка изменений */
                RefreshChanges();

                /* Значения по умолчанию */
                //if (e.ActivateFlags.HasFlag(ActivateFlags.New))
                //    SetControlValue(RefCertificateCreationCard.MainInfo.State, (Int32)RefCertificateCreationCard.MainInfo.CardState.NotStarted);

                if (GetControlValue(RefCertificateCreationCard.MainInfo.Performer).ToGuid().IsEmpty())
                {
                    StaffEmployee Emp;

                    Emp = Context.GetEmployeeByPosition(RefCertificateCreationCard.Roles.AdjastManager, true);
                    SetControlValue(RefCertificateCreationCard.MainInfo.Performer, Context.GetObjectRef(Emp).Id);
                    
                    /*catch
                    {
                        try
                        {
                            Emp = Context.GetEmployeeByPosition(RefCertificateCreationCard.Roles.DeputyAdjastManager, false);
                            SetControlValue(RefCertificateCreationCard.MainInfo.Performer, Context.GetObjectRef(Emp).Id);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности '" + RefCertificateCreationCard.Roles.AdjastManager + "' или '" + RefCertificateCreationCard.Roles.DeputyAdjastManager + "'!",
                                "Предупржедение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }*/
                }
                /* Привязка методов */
                CardScript.CardControl.CardClosed += CardControl_CardClosed;
                CardScript.CardControl.Saved += CardControl_Saved;
                CardScript.CardControl.Saving += CardControl_Saving;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Send].ItemClick += Send_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Delegate].ItemClick += Delegate_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Complete].ItemClick += Complete_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Accept].ItemClick += Accept_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Revoke].ItemClick += Revoke_ItemClick;

                AddNewDoubleClickHandler();
                //Grid_Devices.AddDoubleClickHandler(new Devices_DoubleClickDelegate(Devices_DoubleClick));
                AddTableHandler(RefCertificateCreationCard.Devices.Alias, "AddButtonClicked", "Devices_AddButtonClicked");
                AddTableHandler(RefCertificateCreationCard.Devices.Alias, "RemoveButtonClicked", "Devices_RemoveButtonClicked");
                
                /* Настройка */
                Customize();
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        #region Delegates

        private delegate void Devices_DoubleClickDelegate(Object sender, EventArgs e);
        

        #endregion

        #region Methods
        /// <summary>
        /// Настройка внешнего вида.
        /// </summary>
        public override void Customize ()
        {
            /* Настройка таблиц */
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_Devices.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

            ICardControl.HideTableBarItem(RefCertificateCreationCard.Devices.Alias, 2, true);

            /* Настройка кнопок */
            ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Send].Hint = "Отправить заявку в отдел калибровки и настройки аппаратуры.";
            ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Delegate].Hint = "Делегировать исполнение работы другому сотруднику.";
            ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Complete].Hint = "Завершить работы по заявке.";
            ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Accept].Hint = "Принять работы по заявке.";
            ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Revoke].Hint = "Вернуть заявку в предыдущее состояние.";
        }
        

        
        public void AddNewDoubleClickHandler()
        {
            Grid_Devices.MouseDown += Devices_MouseDown;
            Grid_Devices.ShownEditor += Devices_ShownEditor;
        }
        public void RemoveNewDoubleClickHandler()
        {
            Grid_Devices.MouseDown -= Devices_MouseDown;
            Grid_Devices.ShownEditor -= Devices_ShownEditor;
        }

        private void Devices_MouseDown(object s, MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Left)
                return;
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo Info = Grid_Devices.CalcHitInfo(args.X, args.Y);
            ClickTicks = Info.RowHandle >= 0 ? Environment.TickCount : 0;
        }
        private void Devices_ShownEditor(object s, EventArgs args)
        {
            if ((Environment.TickCount - ClickTicks) < SystemInformation.DoubleClickTime)
            {
                ClickTicks = 0;
                BaseEdit Editor = Grid_Devices.ActiveEditor;
                Editor.MouseDown += Editor_MouseDown;
            }
        }
        private void Editor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (Environment.TickCount - ClickTicks<SystemInformation.DoubleClickTime)
                new Devices_DoubleClickDelegate(Devices_DoubleClick).DynamicInvoke(sender, e);
            ClickTicks = Environment.TickCount;
        }
    /// <summary>
    /// Обновляет список изменений.
    /// </summary>
    public override void RefreshChanges()
        {
            if (Dic_Changes.IsNull())
                Dic_Changes = new List<DevicesTableChange>();
            else
                Dic_Changes.Clear();

            for (Int32 i = 0; i < Table_Devices.RowCount; i++)
                Dic_Changes.Add((DevicesTableChange)Table_Devices[i]);
        }
        /// <summary>
        /// Получает запись датчика в универсальном справочнике.
        /// </summary>
        /// <param name="GotNumber">Заводской номер.</param>
        /// <param name="Device">Наименование прибора.</param>
        public RowData GetDeviceRow(String GotNumber, String Device)
        {
            String Number = null, Year = null;
            if (GotNumber.Contains("/"))
            {
                Year = GotNumber.Split('/')[1];
                Number = GotNumber.Split('/')[0];
            }
            else
                Number = GotNumber;

            SectionData Items = UniversalCard.Sections[RefUniversal.Item.ID];
            SectionQuery Query = CardScript.Session.CreateSectionQuery();
            Query.ConditionGroup.Operation = ConditionGroupOperation.And;
            Query.ConditionGroup.Conditions.AddNew("Name", FieldType.Unistring, ConditionOperation.Contains, String.Format("{0} № {1}/{2}", Device, Number, Year));

            RowDataCollection Found = Items.FindRows(Query.GetXml());
            return Found.Count == 1 ? Found[0] : null;
        }
        /// <summary>
        /// Обновляет строки таблицы "Дополнительные изделия".
        /// </summary>
        /// <param name="RowId">Идентификатор родительской строки.</param>
        /// <param name="Rows">Строки таблицы дополнительных изделий.</param>
        private void UpdateAWRows(Guid RowId, BindingList<AdditionalWaresRow> Rows)
        {
            BaseCardProperty Table_AdditionalWaresListRow;
            WriteLog("Таблица доп. изделий.");
            for (Int32 i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Table_AdditionalWaresListRow = Table_AdditionalWaresList[i];
                WriteLog("Строка " + i);
                if (Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.ParentTableRowId].ToGuid().Equals(RowId))
                {
                    Table_AdditionalWaresList.RemoveRow(CardScript.BaseObject, i--);
                    WriteLog("Удаление строки " + i);
                }
            }
            for (Int32 i = 0; i < Rows.Count; i++)
            {
                {
                    
                    Table_AdditionalWaresListRow = Table_AdditionalWaresList.AddRow(CardScript.BaseObject);
                    WriteLog("Добавление строки " + i);
                    Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.ParentTableRowId] = RowId;
                    WriteLog("Родительская строка (ID).");
                    Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.WaresNumber] = Rows[i].WareName;
                    WriteLog("Название изделия.");
                    Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.WaresNumberID] = Rows[i].WareID;
                    WriteLog("Идентификатор изделия.");
                    Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationProtocol] = Rows[i].ProtocolID;
                    WriteLog("Протокол калибровки.");
                    Table_AdditionalWaresListRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate] = Rows[i].CertificateID;
                    WriteLog("Сертификат о калибровке.");
                    Table_AdditionalWaresList.RefreshRow(Table_AdditionalWaresList.RowCount - 1);
                    WriteLog("Обновление таблицы доп. изделий.");
                }
            }
        }
        /// <summary>
        /// Вычисление даты проведения калибровки.
        /// </summary>
        /// <param name="BusinessCalendarID">ID Бизнес-календаря СКБ.</param>
        /// <param name="CurrentDate">Дата начала отсчета.</param>
        public DateTime GetCalibrationDate(string BusinessCalendarID, DateTime CurrentDate)
        {
            ICalendarService calendarService = this.Context.GetService<ICalendarService>();

            double StorageTime = 240;                                 // срок хранения - 20 рабочих дней (240 рабочих часов)
            double RandomTime = (double)RandomValue.Next(0, 80);      // случайная погрешность - 10 рабочих дней (80 рабочих часов)

            DateTime CalibrationDate = calendarService.GetStartDate(new Guid(BusinessCalendarID), CurrentDate, StorageTime + RandomTime);

            while (CalibrationLib.CheckCalibrationJournal(CardScript, Context, CalibrationDate) != "")
            {CalibrationDate = calendarService.GetEndDate(new Guid(BusinessCalendarID), CalibrationDate, 8);}

            return CalibrationDate;
        }
        /// <summary>
        /// Вычисление даты поступления на калибровку.
        /// </summary>
        /// <param name="BusinessCalendarID">ID Бизнес-календаря СКБ.</param>
        /// <param name="CalibrationDate">Дата проведения калибровки.</param>
        public DateTime GetReceiptDate(string BusinessCalendarID, DateTime CalibrationDate)
        {
            ICalendarService calendarService = this.Context.GetService<ICalendarService>();

            double CalibrationTime = 120;                             // срок калибровки - 15 рабочих дней (120 рабочих часов)
            double RandomTime = (double)RandomValue.Next(0, 80);      // случайная погрешность - 10 рабочих дней (80 рабочих часов)

            DateTime ReceiptDate = calendarService.GetStartDate(new Guid(BusinessCalendarID), CalibrationDate, CalibrationTime + RandomTime);
            return ReceiptDate;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Команда "Отправить".
        /// </summary>
        private void Send_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // ============================
                // === Выполняются проверки ===
                // ============================

                // Если в таблице "Приборы" нет ни одной записи, отправка отменяется.
                if (Table_Devices.RowCount == 0)
                {
                    MyMessageBox.Show("Укажите хотя бы один прибор, для которого требуется выполнить заявку.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если не заполнено поле "Исполнитель", отправка отменяется.
                if (GetControlValue(RefCertificateCreationCard.MainInfo.Performer) == null)
                {
                    MyMessageBox.Show("Укажите исполнителя данной заявки.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если в таблице "Приборы" позиция отмечена в поле "Только ДК", то обязательно требуется указать доп. изделия.
                string MessageText = "";
                for (int i = 0; i < Table_Devices.RowCount; i++)
                {
                    if (((bool)Table_Devices[i][RefCertificateCreationCard.Devices.AC] == true) &&
                        ((Table_Devices[i][RefCertificateCreationCard.Devices.AdditionalWares] == null) || (Table_Devices[i][RefCertificateCreationCard.Devices.AdditionalWares].ToString() == "")))
                    {
                        string DeviceName = UniversalCard.GetItemName(new Guid(Table_Devices[i][RefCertificateCreationCard.Devices.DeviceTypeId].ToString()));
                        MessageText = MessageText + "Укажите дополнительные изделия для прибора " + DeviceName + ".\n";
                    }
                }
                if (MessageText != "")
                {
                    MyMessageBox.Show(MessageText, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ===============================
                // === Осуществляется отправка ===
                // ===============================

                SetControlValue(RefCertificateCreationCard.MainInfo.SentDate, DateTime.Now);
                SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.InWork); 
                CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.InWork.GetDescription());          
                CardScript.SaveCard();
                MyMessageBox.Show("Заявка успешно отправлена.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CardScript.CardFrame.CardHost.CloseCards();
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Команда "Делегировать".
        /// </summary>
        private void Delegate_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // ====================================
                // === Осуществляется делегирование ===
                // ====================================

                DelegateForm NewDelegateForm = new DelegateForm(CardScript.CardFrame.CardHost, Context);
                NewDelegateForm.ShowDialog();
                if (NewDelegateForm.DialogResult == DialogResult.OK)
                {
                    string Comment = GetControlValue(RefCertificateCreationCard.MainInfo.Comment) == null ? "" : GetControlValue(RefCertificateCreationCard.MainInfo.Comment).ToString();
                    string CommentToDelegate = DateTime.Now.ToString() + ". Заявка делегирована сотруднику '" + NewDelegateForm.Delegate.DisplayName + "'. Комментарий начальника отдела настройки: " + NewDelegateForm.Comment;
                    Comment = Comment == "" ? CommentToDelegate : Comment + "\n" + CommentToDelegate;

                    SetControlValue(RefCertificateCreationCard.MainInfo.Comment, Comment);
                    SetControlValue(RefCertificateCreationCard.MainInfo.Performer, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                    CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();
                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Команда "Завершить".
        /// </summary>
        private void Complete_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // ============================
                // === Выполняются проверки ===
                // ============================

                // Если в таблице "Приборы" позиция не отмечена в поле "Только ДК", то обязательно требуется сформировать сертификат о калибровке.
                string MessageText = "";
                for (int i = 0; i < Table_Devices.RowCount; i++)
                {
                    if ((!(bool)Table_Devices[i][RefCertificateCreationCard.Devices.AC]) &&
                        ((Table_Devices[i][RefCertificateCreationCard.Devices.CalibrationCertificate] == null) || (Table_Devices[i][RefCertificateCreationCard.Devices.CalibrationCertificate].ToGuid().Equals(Guid.Empty))))
                    {
                        string DeviceName = UniversalCard.GetItemName(new Guid(Table_Devices[i][RefCertificateCreationCard.Devices.DeviceTypeId].ToString())) + " " +
                            Table_Devices[i][RefCertificateCreationCard.Devices.DeviceNumber].ToString();
                        MessageText = MessageText == "" ? DeviceName : MessageText + ", " + DeviceName;
                    }
                }

                if (IsCalibration)
                {
                    if (MessageText != "")
                    {
                        if (IsAdmin)
                        {
                            if (MyMessageBox.Show("Внимание! Не все приборы данной заявки имеют сертификаты о калибровке. Продолжить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                return;
                        }
                        else
                        {
                            MyMessageBox.Show("Для завершения заявки необходимо создать сертификаты о калибровке для " + MessageText + ".", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Если в таблице "Дополнительные изделия" тип прибора предусматривает создание отдельного сертификата о калибровке, то обязательно требуется сформировать сертификат.
                    /*MessageText = "";
                    for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                    {
                        RowData WaresRow = UniversalCard.GetItemRow(new Guid(Table_AdditionalWaresList[i][RefCertificateCreationCard.AdditionalWaresList.WaresNumberID].ToString()));
                        Guid WaresTypeId = UniversalCard.GetItemPropertyValue(WaresRow.Id, "Наименование прибора").ToGuid();
                        string WaresTypeName = UniversalCard.GetItemName(WaresTypeId);
                        bool NeedCertificate = Creator.CertificateDeviceTablesCollection.Any(r => r.DeviceTypes.Any(m => m == WaresTypeName));

                        if ((NeedCertificate == true) && ((Table_AdditionalWaresList[i][RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate] == null)
                            || (Table_AdditionalWaresList[i][RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate].ToGuid().Equals(Guid.Empty))))
                        {
                            string WaresNumber = Table_AdditionalWaresList[i][RefCertificateCreationCard.AdditionalWaresList.WaresNumber].ToString();
                            MessageText = MessageText == "" ? WaresNumber : MessageText + ", " + WaresNumber;
                        }
                    }
                    if (MessageText != "")
                    {
                        MyMessageBox.Show("Для завершения заявки необходимо создать сертификаты о калибровке для " + MessageText + ".", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }*/
                }
                else
                {
                    if (MessageText != "")
                    {
                        if (IsAdmin)
                        {
                            if (MyMessageBox.Show("Внимание! Не все приборы данной заявки имеют свидетельство о поверке. Продолжить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                return;
                        }
                        else
                        {
                            MyMessageBox.Show("Для завершения заявки необходимо создать свидетельство о поверке для " + MessageText + ".", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                // ========================================
                // === Осуществляется завершение заявки ===
                // ========================================
                
                SetControlValue(RefCertificateCreationCard.MainInfo.EndDate, DateTime.Now);
                SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.Completed);
                CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.Completed.GetDescription());
                CardScript.SaveCard();
                MyMessageBox.Show("Заявка успешно завершена.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CardScript.CardFrame.CardHost.CloseCards();
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Команда "Принять".
        /// </summary>
        private void Accept_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // ======================================
                // === Осуществляется принятие заявки ===
                // ======================================
                SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.Accepted);
                CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.Accepted.GetDescription());
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Команда "Вернуть".
        /// </summary>
        private void Revoke_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // =====================================
                // === Осуществляется возврат заявки ===
                // =====================================

                int CurrentState = (int)GetControlValue(RefCertificateCreationCard.MainInfo.State);
                switch (CurrentState)
                {
                    case (int)RefCertificateCreationCard.MainInfo.CardState.InWork:
                        SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.NotStarted);
                        CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.NotStarted.GetDescription());
                        break;
                    case (int)RefCertificateCreationCard.MainInfo.CardState.Completed:
                        SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.InWork);
                        CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.InWork.GetDescription());
                        break;
                    case (int)RefCertificateCreationCard.MainInfo.CardState.Accepted:
                        SetControlValue(RefCertificateCreationCard.MainInfo.State, RefCertificateCreationCard.MainInfo.CardState.Completed);
                        CardScript.ChangeState(RefCertificateCreationCard.MainInfo.CardState.Completed.GetDescription());
                        break;
                }
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Закрытие заявки.
        /// </summary>
        private void CardControl_CardClosed(Object sender, EventArgs e)
        {
            try
            {
                /* Отвязка методов */
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Send].ItemClick -= Send_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Delegate].ItemClick -= Delegate_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Complete].ItemClick -= Complete_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Accept].ItemClick -= Accept_ItemClick;
                ICardControl.RibbonControl.Items[RefCertificateCreationCard.RibbonItems.Revoke].ItemClick -= Revoke_ItemClick;
                CardScript.CardControl.Saving -= CardControl_Saving;
                CardScript.CardControl.Saved -= CardControl_Saved;
                CardScript.CardControl.CardClosed -= CardControl_CardClosed;

                Dic_Changes.Clear();
                Dic_Changes = null;
                

                RemoveNewDoubleClickHandler();

                if (FolderCard.GetShortcuts(CardScript.CardData.Id).Count == 0)
                {
                    try { CardScript.ReleaseNumber(CurrentNumerator.NumericPart); }
                    catch { WriteLog("Не удалось освободить номер!"); }
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Сохранение заявки.
        /// </summary>
        private void CardControl_Saved(Object sender, EventArgs e)
        {
            try
            {
                StringBuilder Digest;
                if (IsCalibration)
                    Digest = new StringBuilder("Заявка на калибровку №");
                else
                    Digest = new StringBuilder("Заявка на поверку №");
                if (CurrentNumerator.IsNull())
                    Digest.Append("<не указан>");
                else
                    Digest.Append(CurrentNumerator.Number);
                WriteLog("Формируем дайджест");
                DateTime CreationDate = (DateTime)GetControlValue(RefCertificateCreationCard.MainInfo.CreationDate);
                WriteLog("Дата регистрации карточки: " + CreationDate.ToShortDateString());
                Digest.Append(" от " + CreationDate.ToShortDateString());
                CardScript.UpdateDescription(Digest.ToString());
                WriteLog("Сформировали дайджест:" + Digest.ToString());
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Перед сохранением заявки.
        /// </summary>
        private void CardControl_Saving(Object sender, CancelEventArgs e)
        {
            
        }
        /// <summary>
        /// Добавление новой строки в таблицу "Приборы".
        /// </summary>
        private void Devices_AddButtonClicked(Object sender, EventArgs e)
        {
            try
            {
                DateTime SentDate = Convert.ToDateTime(GetControlValue(RefCertificateCreationCard.MainInfo.SentDate));
                int CurrentState = (int)GetControlValue(RefCertificateCreationCard.MainInfo.State);

                DevicesForm Form = !Dic_Changes.Any() ? new DevicesForm(this, UniversalCard, CurrentUserRole, CurrentState, Guid.Empty, SentDate, IsCalibration, "", "")
                    : new DevicesForm(this, UniversalCard, CurrentUserRole, CurrentState, Guid.Empty, SentDate, IsCalibration, "", "",
                        Dic_Changes.Select(ch => ch.DeviceNumberId.NewValue).ToList(),
                        Dic_Changes.SelectMany(ch => String.IsNullOrEmpty(ch.Sensors.NewValue) ? new List<String>() : ch.Sensors.NewValue.Split(';').ToList()).ToList());

                //DevicesForm Form = new DevicesForm(this, UniversalCard,  CurrentUserRole, CurrentState, Guid.Empty, SentDate);

                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        BaseCardProperty Row = Table_Devices[Table_Devices.FocusedRowIndex];
                        Guid RowId = Guid.NewGuid();
                        Row[RefCertificateCreationCard.Devices.Id] = RowId;
                        Row[RefCertificateCreationCard.Devices.DeviceTypeId] = Form.DeviceId;
                        Row[RefCertificateCreationCard.Devices.DeviceNumber] = Form.DeviceNumber;
                        Row[RefCertificateCreationCard.Devices.DeviceNumberID] = Form.DeviceNumberId;
                        Row[RefCertificateCreationCard.Devices.AC] = Form.AC;
                        Row[RefCertificateCreationCard.Devices.AdditionalWares] = Form.AdditionalWares;

                        UpdateAWRows(RowId, Form.AdditionalWaresList);
                        
                        Table_Devices.RefreshRow(Table_Devices.FocusedRowIndex);

                        DevicesTableChange Change = (DevicesTableChange)Row;
                        Dic_Changes.Add(Change);
                        break;
                    default:
                        Table_Devices.RemoveRow(CardScript.BaseObject, Table_Devices.FocusedRowIndex);
                        break;
                }
            }
            catch (MyException) { Table_Devices.RemoveRow(CardScript.BaseObject, Table_Devices.FocusedRowIndex); }
        }
        /// <summary>
        /// Двойной клик по строке таблицы "Приборы".
        /// </summary>
        private void Devices_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                BaseCardProperty Row = Table_Devices[Table_Devices.FocusedRowIndex];
                Int32 SelectedRowIndex = Grid_Devices.GetSelectedRows()[0];
                Guid RowId = Row[RefCertificateCreationCard.Devices.Id].ToGuid();
                Guid DeviceId = Row[RefCertificateCreationCard.Devices.DeviceTypeId].ToGuid();
                string DeviceName = UniversalCard.GetItemName(DeviceId);
                DateTime SentDate = Convert.ToDateTime(GetControlValue(RefCertificateCreationCard.MainInfo.SentDate));
                int CurrentState = (int)GetControlValue(RefCertificateCreationCard.MainInfo.State);
                string VerifySerialNumber = Row[RefCertificateCreationCard.Devices.VerifySerialNumber] == null ? "" : Row[RefCertificateCreationCard.Devices.VerifySerialNumber].ToString();
                string CausesOfUnfitness = Row[RefCertificateCreationCard.Devices.CausesOfUnfitness] == null ? "" : Row[RefCertificateCreationCard.Devices.CausesOfUnfitness].ToString();

                DevicesForm Form = !Dic_Changes.Any(ch => ch.RowId != RowId) ? new DevicesForm(this, UniversalCard, CurrentUserRole, CurrentState, DeviceId, SentDate, IsCalibration, VerifySerialNumber, CausesOfUnfitness)
                       : new DevicesForm(this, UniversalCard, CurrentUserRole, CurrentState, DeviceId, SentDate, IsCalibration, VerifySerialNumber, CausesOfUnfitness,
                           Dic_Changes.Where(ch => ch.RowId != RowId).Select(ch => ch.DeviceNumberId.NewValue).ToList(),
                           Dic_Changes.Where(ch => ch.RowId != RowId).SelectMany(ch => String.IsNullOrEmpty(ch.Sensors.NewValue) ? new List<String>() : ch.Sensors.NewValue.Split(';').ToList()).ToList());

                Form.DeviceNumberId     = Row[RefCertificateCreationCard.Devices.DeviceNumberID].ToGuid();
                Form.DeviceNumber       = Row[RefCertificateCreationCard.Devices.DeviceNumber] as String;
                Form.OldDeviceNumber    = Row[RefCertificateCreationCard.Devices.DeviceNumber] as String;
                Form.AC                 = (Boolean)Row[RefCertificateCreationCard.Devices.AC];
                BindingList<AdditionalWaresRow> WaresList = new BindingList<AdditionalWaresRow>();
                for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                {
                    BaseCardProperty WaresRow = Table_AdditionalWaresList[i];
                    if (WaresRow[RefCertificateCreationCard.AdditionalWaresList.ParentTableRowId].ToGuid().Equals(Row[RefCertificateCreationCard.Devices.Id].ToGuid()))
                    {
                        string WaresNumber = WaresRow[RefCertificateCreationCard.AdditionalWaresList.WaresNumber].ToString();
                        string WaresNumberID = WaresRow[RefCertificateCreationCard.AdditionalWaresList.WaresNumberID].ToString();
                        string Tooltip = UniversalCard.GetItemName(WaresRow[RefCertificateCreationCard.AdditionalWaresList.WaresNumberID]);
                        string ProtocolID = !WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationProtocol].IsNull() && !WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationProtocol].ToGuid().Equals(Guid.Empty)
                            ? WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationProtocol].ToString() : "";
                        string ProtocolName = ProtocolID == "" ? "" : CardScript.Session.CardManager.GetCardData(new Guid(ProtocolID)).Description;
                        string CertificateID = !WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate].IsNull() && !WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate].ToGuid().Equals(Guid.Empty)
                            ? WaresRow[RefCertificateCreationCard.AdditionalWaresList.CalibrationCertificate].ToString() : "";
                        string CertificateName = CertificateID == "" ? "" : CardScript.Session.CardManager.GetCardData(new Guid(CertificateID)).Description;

                        WaresList.Add(new AdditionalWaresRow(WaresNumber, WaresNumberID, Tooltip, ProtocolID, ProtocolName, CertificateID, CertificateName));
                    }
                }
                Form.AdditionalWaresList = WaresList;

                // Определение даты проведения калибровки и даты поступления на калибровку.
                /*if (SentDate != null && !SentDate.Equals(new DateTime(1, 1, 1)))
                {
                    if ((Row[RefCertificateCreationCard.Devices.CalibrationDate] == null) || (Row[RefCertificateCreationCard.Devices.ReceiptDate] == null))
                    {
                        DateTime CalibrationDate = GetCalibrationDate(BusinessCalendarID, SentDate);
                        DateTime ReceiptDate = GetReceiptDate(BusinessCalendarID, CalibrationDate);
                        Form.CalibrationDate = CalibrationDate;
                        Form.ReceiptDate = ReceiptDate;
                    }
                    else
                    {
                        Form.CalibrationDate = Convert.ToDateTime(Row[RefCertificateCreationCard.Devices.CalibrationDate]);
                        Form.ReceiptDate = Convert.ToDateTime(Row[RefCertificateCreationCard.Devices.ReceiptDate]);
                    }
                }*/

                if ((Row[RefCertificateCreationCard.Devices.CalibrationDate] != null) || (Row[RefCertificateCreationCard.Devices.ReceiptDate] != null))
                {
                    Form.CalibrationDate = Convert.ToDateTime(Row[RefCertificateCreationCard.Devices.CalibrationDate]);
                    Form.ReceiptDate = Convert.ToDateTime(Row[RefCertificateCreationCard.Devices.ReceiptDate]);
                }

                if (Row[RefCertificateCreationCard.Devices.DeviceNumberID] != null && !Row[RefCertificateCreationCard.Devices.DeviceNumberID].Equals(Guid.Empty))
                {
                    if ((Row[RefCertificateCreationCard.Devices.CalibrationProtocol] != null) && (!Row[RefCertificateCreationCard.Devices.CalibrationProtocol].ToGuid().Equals(Guid.Empty)))
                    {Form.ProtocolId = Row[RefCertificateCreationCard.Devices.CalibrationProtocol].ToGuid();}

                    if ((Row[RefCertificateCreationCard.Devices.CalibrationCertificate] != null) && (!Row[RefCertificateCreationCard.Devices.CalibrationCertificate].ToGuid().Equals(Guid.Empty)))
                    { Form.CertificateId = Row[RefCertificateCreationCard.Devices.CalibrationCertificate].ToGuid(); }
                }
                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        WriteLog("Регистрация данных.");
                        Row[RefCertificateCreationCard.Devices.DeviceTypeId]            = Form.DeviceId;
                        WriteLog("Тип прибора (ID).");
                        Row[RefCertificateCreationCard.Devices.DeviceNumber]            = Form.DeviceNumber;
                        WriteLog("Номер прибора (текст).");
                        Row[RefCertificateCreationCard.Devices.DeviceNumberID]          = Form.DeviceNumberId;
                        WriteLog("Номер прибора (ID).");
                        Row[RefCertificateCreationCard.Devices.AC]                      = Form.AC;
                        WriteLog("Только ДК.");
                        Row[RefCertificateCreationCard.Devices.AdditionalWares]         = Form.AdditionalWares;
                        WriteLog("Перечень доп. изделий.");
                        Row[RefCertificateCreationCard.Devices.CalibrationProtocol]     = Form.ProtocolId;
                        WriteLog("Протокол калибровки.");
                        Row[RefCertificateCreationCard.Devices.CalibrationCertificate]  = Form.CertificateId;
                        WriteLog("Серийный номер поверки.");
                        Row[RefCertificateCreationCard.Devices.VerifySerialNumber] = IsCalibration ? "" : Form.VerifySerialNumber;
                        WriteLog("Причины неисправности.");
                        Row[RefCertificateCreationCard.Devices.CausesOfUnfitness] = IsCalibration ? "" : Form.CausesOfUnfitness;
                        WriteLog("Сертификат о калибровке.");
                        if ((Form.CalibrationDate != null) && (Form.CalibrationDate != DateTime.MinValue))
                            Row[RefCertificateCreationCard.Devices.CalibrationDate] = Form.CalibrationDate;
                        WriteLog("Дата калибровки.");
                        if ((Form.ReceiptDate != null) && (Form.CalibrationDate != DateTime.MinValue))
                            Row[RefCertificateCreationCard.Devices.ReceiptDate] = Form.ReceiptDate;
                        WriteLog("Дата передачи на калибровку.");
                        UpdateAWRows(RowId, Form.AdditionalWaresList);
                        WriteLog("Обновление перечня доп. изделий.");
                        Table_Devices.RefreshRow(Table_Devices.FocusedRowIndex);
                        WriteLog("Обновление табличного контрола.");
                        Grid_Devices.FocusedRowHandle = -1;
                        WriteLog("Смена фокуса.");

                        DevicesTableChange Change = Dic_Changes.Find(RowId);
                        WriteLog("Сохранение изменений.");
                        Change.DeviceId.NewValue = Form.DeviceId;
                        WriteLog("Идентификатор измененного прибора.");
                        Change.DeviceNumberId.NewValue = Form.DeviceNumberId;
                        WriteLog("Идентификатор номера измененного прибора.");
                        Change.AC.NewValue = Form.AC;
                        WriteLog("Изменение ДК.");
                        Change.Sensors.NewValue = Form.AdditionalWares;
                        WriteLog("Изменение доп. комплектации.");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                WriteLog(Ex.Message);
                CallError(Ex);
            }
        }
        /// <summary>
        /// Удаление строки таблицы "Приборы".
        /// </summary>
        private void Devices_RemoveButtonClicked(Object sender, EventArgs e)
        {
            try
            {
                List<Guid> NotExistRowIds = Dic_Changes.Select(ch => ch.RowId).Except(Table_Devices.Select(RefCertificateCreationCard.Devices.Id)).ToList();
                if (NotExistRowIds.Count > 0)
                {
                    foreach (Guid RowId in NotExistRowIds)
                    {
                        for (Int32 i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                            if (Table_AdditionalWaresList[i][RefCertificateCreationCard.AdditionalWaresList.ParentTableRowId].ToGuid().Equals(RowId))
                                Table_AdditionalWaresList.RemoveRow(CardScript.BaseObject, i--);
                    }
                    WriteLog("Удалены строки из таблицы \"Доп. комплектующие\".");
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        #endregion
    }
}
