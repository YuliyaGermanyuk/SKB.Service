using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;

using DocsVision.Platform.CardHost;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.WinForms;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

using RKIT.MyMessageBox;
using RKIT.MyReferencesList.Design.Control;

using SKB.Base;
using SKB.Base.Enums;
using SKB.PaymentAndShipment;
using SKB.PaymentAndShipment.Cards;
using SKB.PaymentAndShipment.Ref;
using SKB.Service.Forms.ApplicationCard;
using SKB.Service.Ref;

namespace SKB.Service.Cards
{
    /// <summary>
    /// Карточка "Заявка на сервисное обслуживание".
    /// </summary>
    public class ApplicationCard : MyBaseCard
    {
        #region Properties
        /// <summary>
        /// Таблица "Сервисное обслуживание".
        /// </summary>
        ITableControl Table_Service;
        /// <summary>
        /// Таблица "Дополнительные комплектующие".
        /// </summary>
        ITableControl Table_AddComplete;
        /// <summary>
        /// Таблица "Сервисное обслуживание".
        /// </summary>
        GridView Grid_Service;
        /// <summary>
        /// Таблица "Дополнительные комплектующие".
        /// </summary>
        GridView Grid_AddComplete;
        /// <summary>
        /// Список ссылок.
        /// </summary>
        LinksControlView Control_Links;
        /// <summary>
        /// Список табличных изменений.
        /// </summary>
        List<ServiceTableChange> Dic_Changes;
        #endregion

        /// <summary>
        /// Инициализирует карточку по заданным данным.
        /// </summary>
        /// <param name="ClassBase">Скрипт карточки.</param>
        /// <param name="e">Событие открытия карточки</param>
        public ApplicationCard (ScriptClassBase ClassBase, CardActivatedEventArgs e)
            : base(ClassBase)
        {
            try
            {
                /* Получение рабочих объектов */
                Table_Service = ICardControl.FindPropertyItem<ITableControl>(RefApplicationCard.Service.Alias);
                Table_AddComplete = ICardControl.FindPropertyItem<ITableControl>(RefApplicationCard.AddComplete.Alias);

                Grid_Service = ICardControl.GetGridView(RefApplicationCard.Service.Alias);
                Grid_AddComplete = ICardControl.GetGridView(RefApplicationCard.AddComplete.Alias);

                Control_Links = ICardControl.FindPropertyItem<LinksControlView>(RefApplicationCard.MainInfo.Links);

                /* Получение номера */
                if (GetControlValue(RefApplicationCard.MainInfo.Number).ToGuid().IsEmpty())
                {
                    CurrentNumerator = CardScript.GetNumber(RefApplicationCard.NumberRuleName);
                    CurrentNumerator.Number = Convert.ToInt32(CurrentNumerator.Number).ToString("00000");
                    SetControlValue(RefApplicationCard.MainInfo.Number, Context.GetObjectRef<BaseCardNumber>(CurrentNumerator).Id);
                    WriteLog("Выдали номер: " + CurrentNumerator.Number);
                }
                else
                    CurrentNumerator = Context.GetObject<BaseCardNumber>(GetControlValue(RefApplicationCard.MainInfo.Number));

                /* Заполнение списка изменений */
                RefreshChanges();

                /* Значения по умолчанию */
                if (e.ActivateFlags.HasFlag(ActivateFlags.New))
                    SetControlValue(RefApplicationCard.MainInfo.Status, (Int32)RefApplicationCard.MainInfo.State.Registered);

                if (GetControlValue(RefApplicationCard.MainInfo.Negotiator).ToGuid().IsEmpty())
                    try
                    {
                        StaffEmployee Emp = Context.GetEmployeeByPosition("Начальник отдела настройки");
                        SetControlValue(RefApplicationCard.MainInfo.Negotiator, Context.GetObjectRef(Emp).Id);
                    }
                    catch { MyMessageBox.Show("Не удалось найти ответственного исполнителя!", "Предупржедение", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                /* Привязка методов */
                CardScript.CardControl.CardClosed += CardControl_CardClosed;
                CardScript.CardControl.Saved += CardControl_Saved;
                CardScript.CardControl.Saving += CardControl_Saving;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.ShowClientInfo].ItemClick += ShowClientInfo_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Calculation].ItemClick += Calculation_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintAcceptanceAct].ItemClick += PrintAcceptanceAct_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintDeliveryAct].ItemClick += PrintDeliveryAct_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Calibrate].ItemClick += Calibrate_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Revoke].ItemClick += Revoke_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.CreateAccountCard].ItemClick += CreateAccountCard_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Marketing].ItemClick += Marketing_ItemClick;

                Grid_Service.AddDoubleClickHandler(new Service_DoubleClickDelegate(Service_DoubleClick));
                AddTableHandler(RefApplicationCard.Service.Alias, "AddButtonClicked", "Service_AddButtonClicked");
                AddTableHandler(RefApplicationCard.Service.Alias, "RemoveButtonClicked", "Service_RemoveButtonClicked");

                /* Настройка */
                Customize();
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        #region Delegates

        private delegate void Service_DoubleClickDelegate (Object sender, EventArgs e);

        #endregion

        #region Methods
        /// <summary>
        /// Настройка внешнего вида.
        /// </summary>
        public override void Customize ()
        {
            /* Настройка таблиц */
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_Service.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

            ICardControl.HideTableBarItem(RefApplicationCard.Service.Alias, 2, true);

            /* Настройка полей Оплаты */
            foreach (String S in new String[] { RefApplicationCard.MainInfo.DeliveryCost, RefApplicationCard.MainInfo.Sum, RefApplicationCard.MainInfo.SumNDS })
            {
                SpinEdit Edit = ICardControl.FindPropertyItem<SpinEdit>(S);
                Edit.Properties.Mask.EditMask = "c2";
                Edit.Properties.Mask.UseMaskAsDisplayFormat = true;
            }

            /* Настройка кнопок */
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.ShowClientInfo].Hint = "Показать данные о клиенте.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintAcceptanceAct].Hint = "Печать акта приемки на СО.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintDeliveryAct].Hint = "Печать акта сдачи после СО.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Calibrate].Hint = "Передать на калибровку.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Revoke].Hint = "Отозвать из калибровки.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.CreateAccountCard].Hint = "Сформировать договор/счет сбыта.";
            ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Marketing].Hint = "Передать в отдел сбыта.";
        }
        /// <summary>
        /// Обновляет список изменений.
        /// </summary>
        public override void RefreshChanges ()
        {
            if (Dic_Changes.IsNull())
                Dic_Changes = new List<ServiceTableChange>();
            else
                Dic_Changes.Clear();

            for (Int32 i = 0; i < Table_Service.RowCount; i++)
                Dic_Changes.Add((ServiceTableChange)Table_Service[i]);
        }
        /// <summary>
        /// Обновляет строки таблицы "Дополнительные комплектующие" из-за изменения комплектации.
        /// </summary>
        /// <param name="RowId">Идентификатор родительской строки.</param>
        /// <param name="Rows">Строки таблицы комплектующих.</param>
        private void UpdateACRows (Guid RowId, List<SKB.PaymentAndShipment.Forms.AccountCard.SaleCompleteRow> Rows)
        {
            BaseCardProperty Row;
            for (Int32 i = 0; i < Table_AddComplete.RowCount; i++)
            {
                Row = Table_AddComplete[i];
                if (Row[RefApplicationCard.AddComplete.ParentTableRowId].ToGuid().Equals(RowId) && !Rows.Any(r => r.Name == Row[RefApplicationCard.AddComplete.Name].ToString()))
                    Table_AddComplete.RemoveRow(CardScript.BaseObject, i--);
            }
            for (Int32 i = 0; i < Rows.Count; i++)
            {
                Boolean Flag = true;
                for (Int32 j = 0; j < Table_AddComplete.RowCount; j++)
                {
                    Row = Table_AddComplete[j];
                    if (Row[RefApplicationCard.AddComplete.ParentTableRowId].ToGuid().Equals(RowId) && Rows[i].Name == Row[RefApplicationCard.AddComplete.Name].ToString())
                    {
                        Row[RefApplicationCard.AddComplete.Count] = Rows[i].Count;
                        Table_AddComplete.RefreshRow(Table_AddComplete.RowCount - 1);
                        Flag = false;
                    }
                }
                if (Flag)
                {
                    Table_AddComplete.AddRow(CardScript.BaseObject);
                    Row = Table_AddComplete[Table_AddComplete.RowCount - 1];
                    Row[RefApplicationCard.AddComplete.ParentTableRowId] = RowId;
                    Row[RefApplicationCard.AddComplete.Id] = Rows[i].Id;
                    Row[RefApplicationCard.AddComplete.Name] = Rows[i].Name;
                    Row[RefApplicationCard.AddComplete.Code] = Rows[i].Code;
                    Row[RefApplicationCard.AddComplete.Count] = Rows[i].Count;
                    Row[RefApplicationCard.AddComplete.Ordered] = Rows[i].Ordered;
                    Row[RefApplicationCard.AddComplete.Comment] = Rows[i].Comment;
                    Table_AddComplete.RefreshRow(Table_AddComplete.RowCount - 1);
                }
            }
        }
        #endregion

        #region Event Handlers

        private void Calculation_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Calibrate_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void CardControl_CardClosed (Object sender, EventArgs e)
        {
            try
            {
                /* Отвязка методов */
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Marketing].ItemClick -= Marketing_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.CreateAccountCard].ItemClick -= CreateAccountCard_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Revoke].ItemClick -= Revoke_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Calibrate].ItemClick -= Calibrate_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintDeliveryAct].ItemClick -= PrintDeliveryAct_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.PrintAcceptanceAct].ItemClick -= PrintAcceptanceAct_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.Calculation].ItemClick -= Calculation_ItemClick;
                ICardControl.RibbonControl.Items[RefApplicationCard.RibbonItems.ShowClientInfo].ItemClick -= ShowClientInfo_ItemClick;
                CardScript.CardControl.Saving -= CardControl_Saving;
                CardScript.CardControl.Saved -= CardControl_Saved;
                CardScript.CardControl.CardClosed -= CardControl_CardClosed;

                if (FolderCard.GetShortcuts(CardScript.CardData.Id).Count == 0)
                {
                    try { CardScript.ReleaseNumber(CurrentNumerator.NumericPart); }
                    catch { WriteLog("Не удалось освободить номер!"); }
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void CardControl_Saved (Object sender, EventArgs e)
        {
            try
            {
                StringBuilder Digest = new StringBuilder("Заявка №");
                if (CurrentNumerator.IsNull())
                    Digest.Append("<не указан>");
                else
                    Digest.Append(CurrentNumerator.Number);

                CardScript.UpdateDescription(Digest.ToString());
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void CardControl_Saving (Object sender, CancelEventArgs e)
        {
            try
            {
                /* Переформирование файлов */
                for (Int32 i = 0; i < Table_Service.RowCount; i++)
                {
                    BaseCardProperty Row = Table_Service[i];
                    ServiceTableChange Change = Dic_Changes.Find(Row[RefApplicationCard.Service.Id].ToGuid());
                    if (Change.FileIsChanged || Row[RefApplicationCard.Service.PackedListID].ToGuid().IsEmpty())
                    {
                        Guid FileId = Row[RefApplicationCard.Service.PackedListID].ToGuid();
                        AccountCard.FillPackFile(Context, UniversalCard.GetItemName(Row[RefApplicationCard.Service.DeviceID].ToGuid()), Row[RefApplicationCard.Service.PackedListData] as String, ref FileId);
                        Row[RefApplicationCard.Service.PackedListID] = FileId;
                        Table_Service.RefreshRow(i);
                    }
                    if (Change.DeviceNumberId.IsChanged)
                    {
                        if (!Change.DeviceNumberId.OldValue.IsEmpty())
                            UniversalCard.GetItemRow(Change.DeviceNumberId.OldValue).SetDeviceState(DeviceState.Operating);
                        if (!Change.DeviceNumberId.NewValue.IsEmpty())
                            UniversalCard.GetItemRow(Change.DeviceNumberId.NewValue).SetDeviceState(DeviceState.OnTheWay);
                    }
                    if (Change.Sensors.IsChanged)
                    {
                        if(!String.IsNullOrEmpty(Change.Sensors.OldValue))
                            foreach (String Sensor in Change.Sensors.OldValue.Split(';'))
                                UniversalCard.GetSensorRow(Sensor).SetDeviceState(DeviceState.Operating);
                        if (!String.IsNullOrEmpty(Change.Sensors.NewValue))
                            foreach (String Sensor in Change.Sensors.NewValue.Split(';'))
                                UniversalCard.GetSensorRow(Sensor).SetDeviceState(DeviceState.OnTheWay);
                    }
                }

                /* Синхронизация */
                if (Dic_Changes.IsChanged())
                {
                    ReferenceList RefList = Context.GetObject<ReferenceList>(GetControlValue(RefApplicationCard.MainInfo.Links).ToGuid());
                    List<CardData> AccountCards = new List<CardData>();
                    /* Получение карточек */
                    foreach (ReferenceListReference Link in RefList.References)
                    {
                        if (Link.CardType.Equals(RefAccountCard.ID))
                        {
                            CardData AccountCard = CardScript.Session.CardManager.GetCardData(Link.Card);
                            CardLock CardLock = CardScript.GetCardLock(AccountCard);
                            if (!CardLock.IsFree)
                                throw new MyException("Договор/счет «" + CardLock.CardDescription + "» заблокирован " + (CardLock.IsMine ? "вами!" : "пользователем «" + CardLock.AccountName + "»!"));
                            AccountCards.Add(AccountCard);
                        }
                    }
                    for (Int32 i = 0; i < AccountCards.Count; i++)
                    {
                        RefList = Context.GetObject<ReferenceList>(AccountCards[i].Sections[RefAccountCard.MainInfo.ID].FirstRow.GetGuid(RefAccountCard.MainInfo.LinkListId));

                        List<CardData> ShipmentTasks = new List<CardData>(),
                            CompleteTasks = new List<CardData>();

                        /* Получение карточек */
                        foreach (ReferenceListReference Link in RefList.References)
                        {
                            if (Link.CardType.Equals(RefShipmentCard.ID))
                            {
                                CardData ShipmentTask = CardScript.Session.CardManager.GetCardData(Link.Card);
                                CardLock CardLock = CardScript.GetCardLock(ShipmentTask);
                                if (!CardLock.IsFree)
                                    throw new MyException("Задание на отгрузку «" + CardLock.CardDescription + "» заблокировано " + (CardLock.IsMine ? "вами!" : "пользователем «" + CardLock.AccountName + "»!"));
                                ShipmentTasks.Add(ShipmentTask);
                                CardData CompleteTask = CardScript.Session.CardManager.GetCardData(ShipmentTask.Sections[RefShipmentCard.MainInfo.ID].FirstRow.GetGuid(RefShipmentCard.MainInfo.CompleteTaskId).ToGuid());
                                CardLock = CardScript.GetCardLock(CompleteTask);
                                if (!CardLock.IsFree)
                                    throw new MyException("Задание на комплектацию «" + CardLock.CardDescription + "» заблокировано " + (CardLock.IsMine ? "вами!" : "пользователем «" + CardLock.AccountName + "»!"));
                                CompleteTasks.Add(CompleteTask);
                            }
                        }

                        if (AccountCards[i].InUpdate)
                            AccountCards[i].CancelUpdate();

                        RowDataCollection AccountServiceRows = AccountCards[i].Sections[RefAccountCard.Service.ID].Rows;
                        RowDataCollection AccountAddCompleteRows = AccountCards[i].Sections[RefAccountCard.AddComplete.ID].Rows;

                        for (Int32 j = 0; j < Table_Service.RowCount; j++)
                        {
                            BaseCardProperty Row = Table_Service[i];
                            ServiceTableChange Change = Dic_Changes.Find(Row[RefApplicationCard.Service.Id].ToGuid());
                            RowData ServiceRow = AccountServiceRows.Find(RefAccountCard.Service.Id, Change.RowId);
                            if (Change.IsChanged && !ServiceRow.IsNull())
                            {
                                if (Change.Warranty.IsChanged)
                                    ServiceRow.SetBoolean(RefAccountCard.Service.Warranty, Change.Warranty.NewValue);
                                if (Change.DeviceId.IsChanged)
                                    ServiceRow.SetGuid(RefAccountCard.Service.DeviceId, Change.DeviceId.NewValue);
                                if (Change.AC.IsChanged)
                                    ServiceRow.SetBoolean(RefAccountCard.Service.AC, Change.AC.NewValue);
                                if (Change.FileIsChanged)
                                {
                                    ServiceRow.SetString(RefAccountCard.Service.ACList, Row[RefApplicationCard.Service.ACList] as String);
                                    ServiceRow.SetString(RefAccountCard.Service.PackedListData, Row[RefApplicationCard.Service.PackedListData] as String);
                                    ServiceRow.SetGuid(RefAccountCard.Service.PackedListId, Row[RefApplicationCard.Service.PackedListData].ToGuid());

                                    RowDataCollection AddCompleteRows = AccountAddCompleteRows.Filter("@" + RefAccountCard.AddComplete.ParentTableRowId + " = '" + Change.RowId.ToString().ToUpper() + "'");
                                    List<String> AccountComlete = AddCompleteRows.Select(r => r.GetString(RefAccountCard.AddComplete.Name)).ToList();
                                    List<String> ApplicationComlete = new List<String>();
                                    for (Int32 k = 0; k < Table_AddComplete.RowCount; k++)
                                        if (Table_AddComplete[k][RefApplicationCard.AddComplete.ParentTableRowId].ToGuid().Equals(Change.RowId))
                                            ApplicationComlete.Add(Table_AddComplete[k][RefApplicationCard.AddComplete.Name] as String);

                                    for (Int32 k = 0; k < AddCompleteRows.Count; k++)
                                        if (!ApplicationComlete.Contains(AddCompleteRows[k].GetString(RefAccountCard.AddComplete.Name)))
                                            AccountCards[i].Sections[RefAccountCard.AddComplete.ID].DeleteRow(AddCompleteRows[k].Id);

                                    for (Int32 k = 0; k < Table_AddComplete.RowCount; k++)
                                        if (Table_AddComplete[k][RefApplicationCard.AddComplete.ParentTableRowId].ToGuid().Equals(Change.RowId))
                                        {
                                            RowData AddCompleteRow = AddCompleteRows.Find(RefAccountCard.AddComplete.Name, Table_AddComplete[k][RefApplicationCard.AddComplete.Name] as String);
                                            if (AddCompleteRow.IsNull())
                                            {
                                                AddCompleteRow = AccountCards[i].Sections[RefAccountCard.AddComplete.ID].Rows.AddNew();
                                                AddCompleteRow.SetInt32(RefAccountCard.AddComplete.Ordered, 0);
                                            }
                                            AddCompleteRow.SetInt32(RefAccountCard.AddComplete.Count, (Int32?)Table_AddComplete[k][RefApplicationCard.AddComplete.Count]);
                                            AddCompleteRow.SetGuid(RefAccountCard.AddComplete.Id, Table_AddComplete[k][RefApplicationCard.AddComplete.Id].ToGuid());
                                            AddCompleteRow.SetString(RefAccountCard.AddComplete.Name, Table_AddComplete[k][RefApplicationCard.AddComplete.Name] as String);
                                            AddCompleteRow.SetString(RefAccountCard.AddComplete.Code, Table_AddComplete[k][RefApplicationCard.AddComplete.Code] as String);
                                        }
                                }
                            }
                        }

                        for (Int32 j = 0; j < ShipmentTasks.Count; j++)
                        {
                            if (ShipmentTasks[j].LockStatus == LockStatus.Free && CompleteTasks[j].LockStatus == LockStatus.Free)
                            {
                                if (ShipmentTasks[j].InUpdate)
                                    ShipmentTasks[j].CancelUpdate();
                                if (CompleteTasks[j].InUpdate)
                                    CompleteTasks[j].CancelUpdate();

                                RowDataCollection ShipmentDevicesRows = ShipmentTasks[j].Sections[RefShipmentCard.Devices.ID].Rows;
                                RowDataCollection CompleteDevicesRows = CompleteTasks[j].Sections[RefCompleteCard.Devices.ID].Rows;

                                foreach (RowData DevicesRow in ShipmentDevicesRows)
                                {
                                    ServiceTableChange Change = Dic_Changes.Find(DevicesRow.GetGuid(RefShipmentCard.Devices.AccountCardRowId).ToGuid());
                                    if (!Change.IsNull() && Change.IsChanged)
                                    {
                                        RowDataCollection CompleteRows = CompleteDevicesRows.Filter("@" + RefCompleteCard.Devices.ShipmentTaskRowId + " = '" + DevicesRow.GetGuid(RefShipmentCard.Devices.Id).ToString().ToUpper() + "'");
                                        /* Изменение связных полей */
                                        if (Change.Warranty.IsChanged)
                                        {
                                            DevicesRow.SetBoolean(RefShipmentCard.Devices.Warranty, Change.Warranty.NewValue);
                                            foreach (RowData CompleteRow in CompleteRows)
                                                CompleteRow.SetBoolean(RefCompleteCard.Devices.Warranty, Change.Warranty.NewValue);
                                        }
                                        if (Change.AC.IsChanged)
                                        {
                                            DevicesRow.SetBoolean(RefShipmentCard.Devices.AC, Change.AC.NewValue);
                                            foreach (RowData CompleteRow in CompleteRows)
                                                CompleteRow.SetBoolean(RefCompleteCard.Devices.AC, Change.AC.NewValue);
                                        }
                                        if (Change.DeviceId.IsChanged)
                                        {
                                            DevicesRow.SetGuid(RefShipmentCard.Devices.DeviceId, Change.DeviceId.NewValue);
                                            foreach (RowData CompleteRow in CompleteRows)
                                                CompleteRow.SetGuid(RefCompleteCard.Devices.DeviceId, Change.DeviceId.NewValue);
                                        }
                                        if (Change.FileIsChanged)
                                        {
                                            DevicesRow.SetBoolean(RefShipmentCard.Devices.IsChanged, Change.FileIsChanged);
                                            for (Int32 k = 0; k < Table_Service.RowCount; k++)
                                                if (Table_Service[k][RefApplicationCard.Service.Id].ToGuid().Equals(Change.RowId))
                                                    DevicesRow.SetGuid(RefShipmentCard.Devices.TemplatePackListId, Table_Service[k][RefApplicationCard.Service.PackedListID].ToGuid());
                                        }

                                        /* Очистка связных полей */
                                        if (Change.DeviceId.IsChanged || Change.AC.IsChanged)
                                        {
                                            DevicesRow.SetString(RefShipmentCard.Devices.DeviceNumbers, null);
                                            DevicesRow.SetString(RefShipmentCard.Devices.PartyNumbers, null);
                                            DevicesRow.SetString(RefShipmentCard.Devices.Coupons, null);

                                            foreach (RowData CompleteRow in CompleteRows)
                                                try
                                                {
                                                    Guid DeviceNumberId = CompleteRow.GetObject(RefCompleteCard.Devices.DeviceNumberId).ToGuid();
                                                    /* Очистка приборов */
                                                    if (!DeviceNumberId.IsEmpty())
                                                        try { UniversalCard.GetItemRow(DeviceNumberId).UpdateDeviceInformation(UniversalCard, CompleteRow.GetBoolean(RefCompleteCard.Devices.AS)); }
                                                        catch (MyException Ex)
                                                        {
                                                            switch (Ex.ErrorCode)
                                                            {
                                                                case -1:
                                                                    WriteLog("Ошибка: предвиденное исключение" + "\r\n" + Ex.Message + "\r\n" + Ex.StackTrace);
                                                                    WriteLog("Ошибка: предвиденное исключение" + "\r\n" + Ex.InnerException.Message + "\r\n" + Ex.InnerException.StackTrace);
                                                                    break;
                                                                case 0:
                                                                    WriteLog("Ошибка: предвиденное исключение" + "\r\n" + Ex.Message + "\r\n" + Ex.StackTrace);
                                                                    break;
                                                                case 1:
                                                                    String[] ss = Ex.Message.Split('\t');
                                                                    WriteLog("Ошибка: предвиденное исключение" + "\r\n" + (ss.Length >= 2 ? "В карточке " + ss[1] + " отсутствует поле: " + ss[0] : Ex.Message) + "\r\n" + Ex.StackTrace);
                                                                    WriteLog("Ошибка: предвиденное исключение" + "\r\n" + Ex.InnerException.Message + "\r\n" + Ex.InnerException.StackTrace);
                                                                    break;
                                                            }
                                                        }

                                                    CompleteRow.SetGuid(RefCompleteCard.Devices.DeviceNumberId, Guid.Empty);
                                                    CompleteRow.SetString(RefCompleteCard.Devices.DeviceNumber, null);
                                                    CompleteRow.SetGuid(RefCompleteCard.Devices.CouponId, Guid.Empty);
                                                }
                                                catch (Exception Ex) { WriteLog("Ошибка: непредвиденное исключение" + "\r\n" + Ex.Message + "\r\n" + Ex.StackTrace); }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (ArgumentNullException Ex)
            {
                MyMessageBox.Show("Поле \"" + Ex.ParamName + "\" обязательно к заполнению!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            catch (MyException Ex)
            {
                MyMessageBox.Show(Ex.Message + Environment.NewLine + "Синхронизация связных карточек не произойдет.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void CreateAccountCard_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
                IReferenceListService ReferenceListService = Context.GetService<IReferenceListService>();
                ReferenceList RefList = Context.GetObject<ReferenceList>(Control_Links.ReferenceListID);
                if (RefList.References.Any(r => r.CardType.Equals(RefAccountCard.ID)))
                    CardScript.CardFrame.CardHost.ShowCardModal(RefList.References.First(r => r.CardType.Equals(RefAccountCard.ID)).Card, ActivateMode.Edit);

                if (Table_Service.RowCount > 0)
                    throw new MyException(1);

                if (!CardScript.SaveCard())
                    throw new MyException(0);

                /* Формирование задания на отгрузку */
                CardData AccountData = Context.CreateCard(RefAccountCard.ID);
                AccountData.BeginUpdate();

                RowData MainInfoRow = AccountData.Sections[RefAccountCard.MainInfo.ID].FirstRow;
                SectionData Service = AccountData.Sections[RefAccountCard.Service.ID];

                MainInfoRow.SetGuid(RefAccountCard.MainInfo.RegisterId, Context.GetCurrentUser());
                MainInfoRow.SetGuid(RefAccountCard.MainInfo.ContractSubjectId, AccountCard.Item_Subject_Service.ToGuid());
                MainInfoRow.SetGuid(RefAccountCard.MainInfo.ClientId, GetControlValue(RefApplicationCard.MainInfo.Client).ToGuid());

                
                Guid? RefListId = MainInfoRow.GetGuid(RefAccountCard.MainInfo.LinkListId);
                RefList = RefListId.HasValue ? Context.GetObject<ReferenceList>(RefListId.Value) : ReferenceListService.CreateReferenceList();

                if (!RefList.References.Any(RefRef => RefRef.Card.Equals(CardScript.CardData.Id)))
                {
                    ReferenceListService.CreateReference(RefList, null, CardScript.CardData.Id, RefApplicationCard.ID, false);
                    MainInfoRow.SetGuid(RefAccountCard.MainInfo.LinkListId, Context.GetObjectRef<ReferenceList>(RefList).Id);
                }

                RefList.References.First(RefRef => RefRef.Card.Equals(CardScript.CardData.Id)).LinkDescription = DateTime.Now.ToString("Создано dd.MM.yyyy HH:mm");
                Context.SaveObject(RefList);

                for (Int32 i = 0; i < Table_Service.RowCount; i++)
                {
                    BaseCardProperty Row = Table_Service[i];
                    RowData ServiceRow = Service.Rows.AddNew();
                    ServiceRow.SetGuid(RefAccountCard.Service.Id, Row[RefApplicationCard.Service.Id].ToGuid());
                    ServiceRow.SetGuid(RefAccountCard.Service.DeviceId, Row[RefApplicationCard.Service.DeviceID].ToGuid());
                    ServiceRow.SetBoolean(RefAccountCard.Service.AC, (Boolean)Row[RefApplicationCard.Service.Verify]);
                    ServiceRow.SetBoolean(RefAccountCard.Service.Verify, (Boolean)Row[RefApplicationCard.Service.Verify]);
                    ServiceRow.SetBoolean(RefAccountCard.Service.Repair, (Boolean)Row[RefApplicationCard.Service.Repair]);
                    ServiceRow.SetBoolean(RefAccountCard.Service.Calibrate, (Boolean)Row[RefApplicationCard.Service.Calibrate]);
                    ServiceRow.SetBoolean(RefAccountCard.Service.Delivery, false);
                    ServiceRow.SetInt32(RefAccountCard.Service.Count, 1);
                    ServiceRow.SetInt32(RefAccountCard.Service.Shipped, 0);
                    ServiceRow.SetInt32(RefAccountCard.Service.ToShip, 1);
                    ServiceRow.SetString(RefAccountCard.Service.ACList, Row[RefApplicationCard.Service.ACList]);
                    ServiceRow.SetString(RefAccountCard.Service.PackedListData, Row[RefApplicationCard.Service.PackedListData]);
                    ServiceRow.SetGuid(RefAccountCard.Service.PackedListId, Row[RefApplicationCard.Service.PackedListID].ToGuid());
                    ServiceRow.SetBoolean(RefAccountCard.Service.Warranty, (Boolean)Row[RefApplicationCard.Service.WarrantyServices]);
                }

                AccountData.EndUpdate();

                if (CardScript.CardFrame.CardHost.ShowCardModal(AccountData.Id, ActivateMode.Edit, ActivateFlags.New))
                {
                    Control_Links.AddRef(AccountData.Id, DateTime.Now.ToString("Создано dd.MM.yyyy HH:mm"), true, false);
                    MyHelper.SaveCard(CardScript);
                }
                else
                {
                    AccountData.ForceUnlock();
                    CardScript.Session.CardManager.DeleteCard(AccountData.Id, true);
                }
            }
            catch (MyException Ex)
            {
                switch (Ex.ErrorCode)
                {
                    case 1: MyMessageBox.Show("Таблица «Сервисное обслуживание» не заполнена!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); break;
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Marketing_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void PrintAcceptanceAct_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void PrintDeliveryAct_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Revoke_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Service_AddButtonClicked (Object sender, EventArgs e)
        {
            try
            {
                ServiceForm Form = !Dic_Changes.Any() ? new ServiceForm(Context, UniversalCard, CardScript.CardFrame.CardHost, Guid.Empty, Convert.ToDateTime(GetControlValue(RefApplicationCard.MainInfo.RegDate)))
                    : new ServiceForm(Context, UniversalCard, CardScript.CardFrame.CardHost, Guid.Empty,
                        Convert.ToDateTime(GetControlValue(RefApplicationCard.MainInfo.RegDate)),
                        Dic_Changes.Select(ch => ch.DeviceNumberId.NewValue).ToList(),
                        Dic_Changes.SelectMany(ch => String.IsNullOrEmpty(ch.Sensors.NewValue) ? new List<String>() : ch.Sensors.NewValue.Split(';').ToList()).ToList());
                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        BaseCardProperty Row = Table_Service[Table_Service.FocusedRowIndex];
                        Guid RowId = Guid.NewGuid();
                        Row[RefApplicationCard.Service.Id] = RowId;
                        Row[RefApplicationCard.Service.DeviceID] = Form.DeviceId;

                        Row[RefApplicationCard.Service.DeviceNumber] = Form.DeviceNumber;
                        Row[RefApplicationCard.Service.DeviceNumberID] = Form.DeviceNumberId;

                        Row[RefApplicationCard.Service.AC] = Form.AC;

                        Row[RefApplicationCard.Service.Sensors] = Form.Sensors;

                        Row[RefApplicationCard.Service.Verify] = Form.Verify;
                        Row[RefApplicationCard.Service.Repair] = Form.Repair;
                        Row[RefApplicationCard.Service.Calibrate] = Form.Calibrate;
                        Row[RefApplicationCard.Service.Wash] = Form.Wash;

                        Row[RefApplicationCard.Service.WarrantyServices] = Form.Warranty;

                        Row[RefApplicationCard.Service.ACList] = Form.ACList;
                        Row[RefApplicationCard.Service.Comments] = Form.Comment;

                        Row[RefApplicationCard.Service.PackedListData] = Form.CData;
                        Row[RefApplicationCard.Service.PackedListID] = Guid.Empty;
                        Table_Service.RefreshRow(Table_Service.FocusedRowIndex);

                        ServiceTableChange Change = (ServiceTableChange)Row;
                        Change.FileIsChanged = true;
                        Dic_Changes.Add(Change);

                        UpdateACRows(RowId, Form.ACRows.Select(i => (SKB.PaymentAndShipment.Forms.AccountCard.SaleCompleteRow)i).ToList());
                        break;
                    default:
                        Table_Service.RemoveRow(CardScript.BaseObject, Table_Service.FocusedRowIndex);
                        break;
                }
            }
            catch (MyException) { Table_Service.RemoveRow(CardScript.BaseObject, Table_Service.FocusedRowIndex); }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Service_DoubleClick (Object sender, EventArgs e)
        {
            try
            {
                BaseCardProperty Row = Table_Service[Table_Service.FocusedRowIndex];
                Guid RowId = Row[RefApplicationCard.Service.Id].ToGuid();
                ServiceForm Form = !Dic_Changes.Any(ch => ch.RowId != RowId) ? new ServiceForm(Context, UniversalCard, CardScript.CardFrame.CardHost, Row[RefApplicationCard.Service.DeviceID].ToGuid(), Convert.ToDateTime(GetControlValue(RefApplicationCard.MainInfo.RegDate)))
                    : new ServiceForm(Context, UniversalCard, CardScript.CardFrame.CardHost, Row[RefApplicationCard.Service.DeviceID].ToGuid(),
                        Convert.ToDateTime(GetControlValue(RefApplicationCard.MainInfo.RegDate)),
                        Dic_Changes.Select(ch => ch.DeviceNumberId.NewValue).ToList(),
                        Dic_Changes.SelectMany(ch => String.IsNullOrEmpty(ch.Sensors.NewValue) ? new List<String>() : ch.Sensors.NewValue.Split(';').ToList()).ToList());

                Form.DeviceNumberId = Row[RefApplicationCard.Service.DeviceNumberID].ToGuid();
                Form.DeviceNumber = Row[RefApplicationCard.Service.DeviceNumber] as String;
               
                Form.AC = (Boolean)Row[RefApplicationCard.Service.AC];

                Form.Sensors = Row[RefApplicationCard.Service.Sensors] as String;

                Form.Verify = (Boolean)Row[RefApplicationCard.Service.Verify];
                Form.Repair = (Boolean)Row[RefApplicationCard.Service.Repair];
                Form.Calibrate = (Boolean)Row[RefApplicationCard.Service.Calibrate];
                Form.Wash = (Boolean)Row[RefApplicationCard.Service.Wash];

                Form.Warranty = (Boolean)Row[RefApplicationCard.Service.WarrantyServices];

                Form.ACList = Row[RefApplicationCard.Service.ACList] as String;
                Form.Comment = Row[RefApplicationCard.Service.Comments] as String;

                String OldCData = Row[RefApplicationCard.Service.PackedListData] as String;
                Form.CData = OldCData;
                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        Row[RefApplicationCard.Service.DeviceID] = Form.DeviceId;

                        Row[RefApplicationCard.Service.DeviceNumber] = Form.DeviceNumber;
                        Row[RefApplicationCard.Service.DeviceNumberID] = Form.DeviceNumberId;

                        Row[RefApplicationCard.Service.AC] = Form.AC;

                        Row[RefApplicationCard.Service.Sensors] = Form.Sensors;

                        Row[RefApplicationCard.Service.Verify] = Form.Verify;
                        Row[RefApplicationCard.Service.Repair] = Form.Repair;
                        Row[RefApplicationCard.Service.Calibrate] = Form.Calibrate;
                        Row[RefApplicationCard.Service.Wash] = Form.Wash;

                        Row[RefApplicationCard.Service.WarrantyServices] = Form.Warranty;

                        Row[RefApplicationCard.Service.ACList] = Form.ACList;
                        Row[RefApplicationCard.Service.Comments] = Form.Comment;

                        Row[RefApplicationCard.Service.PackedListData] = Form.CData;
                        Table_Service.RefreshRow(Table_Service.FocusedRowIndex);
                        Grid_Service.FocusedRowHandle = -1;

                        ServiceTableChange Change = Dic_Changes.Find(RowId);
                        Change.DeviceId.NewValue = Form.DeviceId;
                        Change.FileIsChanged = !OldCData.Equals(Form.CData);
                        Change.Warranty.NewValue = Form.Warranty;
                        Change.AC.NewValue = Form.AC;

                        if (!OldCData.Equals(Form.CData))
                            UpdateACRows(RowId, Form.ACRows.Select(i => (SKB.PaymentAndShipment.Forms.AccountCard.SaleCompleteRow)i).ToList());
                        break;
                    default:
                        Table_Service.RemoveRow(CardScript.BaseObject, Table_Service.FocusedRowIndex);
                        break;
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void Service_RemoveButtonClicked (Object sender, EventArgs e)
        {
            try
            {
                List<Guid> NotExistRowIds = Dic_Changes.Select(ch => ch.RowId).Except(Table_Service.Select(RefApplicationCard.Service.Id)).ToList();
                if (NotExistRowIds.Count > 0)
                {
                    foreach (Guid RowId in NotExistRowIds)
                    {
                        //Dic_NeedChange.Remove(RowId);
                        Dic_Changes.Remove(Dic_Changes.Find(RowId));
                        for (Int32 i = 0; i < Table_AddComplete.RowCount; i++)
                            if (Table_AddComplete[i][RefApplicationCard.AddComplete.ParentTableRowId].ToGuid().Equals(RowId))
                                Table_AddComplete.RemoveRow(CardScript.BaseObject, i--);
                    }
                    WriteLog("Удалены строки из таблицы \"Доп. комплектующие\".");
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        private void ShowClientInfo_ItemClick (Object sender, ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        #endregion
    }
}
