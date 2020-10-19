using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.WinForms;

using DocsVision.TakeOffice.Cards.Constants;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

using RKIT.MyMessageBox;
using RKIT.MyReferencesList.Design.Control;
using RKIT.MyFilesList.Design.Control;
using RKIT.MyCollectionControl.Design.Control;

using SKB.Base;
using SKB.Service.Forms.ServiceCard;
using SKB.Base.Ref;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using SKB.PaymentAndShipment.Forms.AccountCard;
using SKB.Service.CalibrationDocs;

namespace SKB.Service.Cards
{
    /// <summary>
    /// Карточка "Сервисное обслуживание прибора".
    /// </summary>
    public class ServicesCard : MyBaseCard
    {
        #region Properties
        /// <summary>
        /// Таблица "Описание неисправностей".
        /// </summary>
        ITableControl Table_Description;
        /// <summary>
        /// Таблица "Ремонтные работы".
        /// </summary>
        ITableControl Table_RepairWorks;
        /// <summary>
        /// Таблица "Необходимые доработки".
        /// </summary>
        ITableControl Table_Improvements;
        /// <summary>
        /// Таблица "Дополнительные изделия".
        /// </summary>
        ITableControl Table_AdditionalWaresList;
        /// <summary>
        /// Таблица "Описание неисправностей".
        /// </summary>
        GridView Grid_Description;
        /// <summary>
        /// Таблица "Ремонтные работы".
        /// </summary>
        GridView Grid_RepairWorks;
        /// <summary>
        /// Таблица "Необходимые доработки".
        /// </summary>
        GridView Grid_Improvements;
        /// <summary>
        /// Таблица "Дополнительные изделия".
        /// </summary>
        GridView Grid_AdditionalWaresList;
        /// <summary>
        /// "Гарантия аннулирована".
        /// </summary>
        CheckEdit Check_VoidWarranty;
        /// <summary>
        /// "Удвоить стоимость ремонта".
        /// </summary>
        CheckEdit Check_DoubleCost;
        /// <summary>
        /// "Удвоить стоимость ремонта".
        /// </summary>
        TextEdit Check_DescriptionOfReason;
        /// <summary>
        /// Список ссылок.
        /// </summary>
        LinksControlView Control_Links;
        /// <summary>
        /// Список ссылок.
        /// </summary>
        ItemControlView Control_Files;
        /// <summary>
        /// "Сохранение изменений".
        /// </summary>
        bool SaveChanges;
        /// <summary>
        /// Необходимые доработки.
        /// </summary>
        CollectionControlView Control_Collection;
        /// <summary>
        /// Шаблон Акта передачи приборов и комплектующих.
        /// </summary>
        string ActLoadTemplate = "{618D04AE-97AD-E411-8B0D-00155D016943}";
        /// <summary>
        /// Идентификатор файла шаблона УЛ.
        /// </summary>
        public readonly static Guid RefPackFileTemplate = new Guid("{A5FFE996-3DAF-E311-9DAB-00155D016900}");
        #endregion
        /// <summary>
        /// Инициализирует карточку по заданным данным.
        /// </summary>
        /// <param name="ClassBase">Скрипт карточки.</param>
        /// <param name="e">Событие открытия карточки</param>
        public ServicesCard(ScriptClassBase ClassBase, CardActivatedEventArgs e)
            : base(ClassBase)
        {
            try
            {
                /* Получение рабочих объектов */
                Table_Description = ICardControl.FindPropertyItem<ITableControl>(RefServiceCard.DescriptionOfFault.Alias);
                Table_RepairWorks = ICardControl.FindPropertyItem<ITableControl>(RefServiceCard.RepairWorks.Alias);
                Table_AdditionalWaresList = ICardControl.FindPropertyItem<ITableControl>(RefServiceCard.AdditionalWaresList.Alias);
                Grid_Description = ICardControl.GetGridView(RefServiceCard.DescriptionOfFault.Alias);
                Grid_RepairWorks = ICardControl.GetGridView(RefServiceCard.RepairWorks.Alias);
                Grid_AdditionalWaresList = ICardControl.GetGridView(RefServiceCard.AdditionalWaresList.Alias);
                Control_Collection = ICardControl.FindPropertyItem<CollectionControlView>(RefServiceCard.Calibration.Improvements);

                Control_Links = ICardControl.FindPropertyItem<LinksControlView>(RefServiceCard.MainInfo.Links);
                Control_Files = ICardControl.FindPropertyItem<ItemControlView>(RefServiceCard.MainInfo.Files);

                Check_VoidWarranty = ICardControl.FindPropertyItem<CheckEdit>(RefServiceCard.Adjustment.VoidWarranty);
                Check_DoubleCost = ICardControl.FindPropertyItem<CheckEdit>(RefServiceCard.Adjustment.DoubleCost);
                Check_DescriptionOfReason = ICardControl.FindPropertyItem<TextEdit>(RefServiceCard.Adjustment.DescriptionOfReason);

                /* Привязка методов */
                if (!IsReadOnly)
                {
                    CardScript.CardControl.CardClosed -= CardControl_CardClosed;
                    CardScript.CardControl.CardClosed += CardControl_CardClosed;
                    CardScript.CardControl.Saved -= CardControl_Saved;
                    CardScript.CardControl.Saved += CardControl_Saved;
                    CardScript.CardControl.Saving -= CardControl_Saving;
                    CardScript.CardControl.Saving += CardControl_Saving;

                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].ItemClick -= SendToAdjustment_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].ItemClick += SendToAdjustment_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToCalibrate].ItemClick -= SendToCalibrate_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToCalibrate].ItemClick += SendToCalibrate_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToSale].ItemClick -= SendToSale_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToSale].ItemClick += SendToSale_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAgreement].ItemClick -= SendToAgreement_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAgreement].ItemClick += SendToAgreement_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.ActOfLoad].ItemClick -= ActOfLoad_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.ActOfLoad].ItemClick += ActOfLoad_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].ItemClick -= Return_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].ItemClick += Return_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToMaintenance].ItemClick -= SendToMaintenance_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToMaintenance].ItemClick += SendToMaintenance_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToVerification].ItemClick -= SendToVerification_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToVerification].ItemClick += SendToVerification_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.AgreeToRepair].ItemClick -= AgreeToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.AgreeToRepair].ItemClick += AgreeToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.FailureToRepair].ItemClick -= FailureToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.FailureToRepair].ItemClick += FailureToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.PartiallyAgreeToRepair].ItemClick -= PartiallyAgreeToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.PartiallyAgreeToRepair].ItemClick += PartiallyAgreeToRepair_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Complete].ItemClick -= Complete_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Complete].ItemClick += Complete_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Delegate].ItemClick -= Delegate_ItemClick;
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Delegate].ItemClick += Delegate_ItemClick;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.OpenSensors).Click -= OpenSensors_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.OpenSensors).Click += OpenSensors_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshProtocol).Click -= RefreshProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshProtocol).Click += RefreshProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshCertificate).Click -= RefreshCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshCertificate).Click += RefreshCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateProtocol).Click -= CreateProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateProtocol).Click += CreateProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateCertificate).Click -= CreateCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateCertificate).Click += CreateCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationProtocol).Click -= RefreshVerificationProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationProtocol).Click += RefreshVerificationProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationCertificate).Click -= RefreshVerificationCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationCertificate).Click += RefreshVerificationCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationProtocol).Click -= CreateVerificationProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationProtocol).Click += CreateVerificationProtocol_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationCertificate).Click -= CreateVerificationCertificate_Click;
                    ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationCertificate).Click += CreateVerificationCertificate_Click;

                    Grid_Description.AddDoubleClickHandler(new Description_DoubleClickDelegate(Description_DoubleClick));

                    // Кнопки таблицы "Описание неисправностей"
                    ICardControl.RemoveTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4);
                    ICardControl.RemoveTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 3);
                    ICardControl.AddTableBarItem(RefServiceCard.DescriptionOfFault.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddDescriptionOfFaultButton_ItemClick;
                    ICardControl.AddTableBarItem(RefServiceCard.DescriptionOfFault.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveDescriptionOfFaultButton_ItemClick;
                }

                /* Настройка */
                Customize();
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        #region Delegates

        private delegate void Description_DoubleClickDelegate(Object sender, EventArgs e);

        #endregion

        #region Methods
        /// <summary>
        /// Настройка внешнего вида.
        /// </summary>
        public override void Customize ()
        {
            // Настройка внешнего вида таблиц

            #region Таблица "Описание неисправностей"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_Description.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 0, true);
            ICardControl.HideTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 1, true);
            ICardControl.HideTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 2, true);

            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_Description.RowCount == 0) ICardControl.DisableTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefServiceCard.DescriptionOfFault.Alias))
            {
                ICardControl.DisableTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4, true);
            }
            #endregion

            #region Настройка кнопок 
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].Hint = "Передать в ремонт (через отдел сбыта)";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToCalibrate].Hint = "Передать в калибровку";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToSale].Hint = "Передать в отдел сбыта";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAgreement].Hint = "Передать на согласование";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на предыдущий этап";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToMaintenance].Hint = "Передать на техобслуживание";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToVerification].Hint = "Передать на поверку";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.AgreeToRepair].Hint = "Ремонтные работы согласованы клиентом";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.FailureToRepair].Hint = "Клиент отказался от ремонта";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.PartiallyAgreeToRepair].Hint = "Ремонтные работы согласованы частично";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Complete].Hint = "Завершить наряд на сервисное обслуживание";
            ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Delegate].Hint = "Делегировать исполнение другому сотруднику";

            WriteLog("Определение текущего состояния Наряда на СО...");
            int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
            switch (CurrentState)
            {
                // Текущее состояние = "На техобслуживании" и "На повторном техобслуживании"
                case (int)RefServiceCard.MainInfo.State.Maintenance:
                case (int)RefServiceCard.MainInfo.State.Remaintenance:
                    WriteLog("Текущее состояния Наряда на СО = 'На техобслуживании'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].Hint = "Передать в ремонт";
                    break;
                // Текущее состояние = "На диагносике"
                case (int)RefServiceCard.MainInfo.State.Diagnostics:
                    WriteLog("Текущее состояния Наряда на СО = 'На диагностике'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на техобслуживание";
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].Hint = "Передать в ремонт (без согласования с отделом сбыта)";
                    break;
                // Текущее состояние = "На согласовании"
                case (int)RefServiceCard.MainInfo.State.Consensus:
                    WriteLog("Текущее состояния Наряда на СО = 'На согласовании'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на диагностику";
                    break;
                // Текущее состояние = "В ремонте"
                case (int)RefServiceCard.MainInfo.State.Adjustment:
                    WriteLog("Текущее состояния Наряда на СО = 'В ремонте'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на диагностику (потребуется повторное согласование со сбытом)";
                    break;
                // Текущее состояние = "На калибровке"
                case (int)RefServiceCard.MainInfo.State.Calibration:
                    WriteLog("Текущее состояния Наряда на СО = 'На калибровке'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на техобслуживание";
                    break;
                // Текущее состояние = "На поверке"
                case (int)RefServiceCard.MainInfo.State.Verification:
                    WriteLog("Текущее состояния Наряда на СО = 'На поверке'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на ожидание оплаты";
                    break;
                // Текущее состояние = "Ожидание оплаты"
                case (int)RefServiceCard.MainInfo.State.Payment:
                    WriteLog("Текущее состояния Наряда на СО = 'Ожидание оплаты'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на техобслуживание";
                    break;
                // Текущее состояние = "Завершено"
                case (int)RefServiceCard.MainInfo.State.Completed:
                    WriteLog("Текущее состояния Наряда на СО = 'Выполнено'...");
                    if ((bool)GetControlValue(RefServiceCard.Calibration.Verify))
                        ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на ожидание оплаты";
                    else
                        if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration))
                        ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на калибровку";
                    else
                        ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на техобслуживание";
                    break;
                // Текущее состояние = "Отказ от ремонта"
                case (int)RefServiceCard.MainInfo.State.Failure:
                    WriteLog("Текущее состояния Наряда на СО = 'Отказ от ремонта'...");
                    ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].Hint = "Вернуть на согласование";
                    break;
            }
                    #endregion
            }
        /// <summary>
        /// Получение записи универсального справочника по названию.
        /// </summary>
        /// <param name="UniversalCard">Универсальный справочник.</param>
        /// <param name="ItemName">Название записи.</param>
        public static string GetItemID(CardData UniversalCard, string ItemName)
        {
            SectionData Items = UniversalCard.Sections[RefUniversal.Item.ID];

            RowData Item = Items.FindRow("@Name = '" + ItemName + "'");
            if (Item == null)
            {
                MyMessageBox.Show("Не удалось найти в справочнике запись:" + ItemName + " . Обратитесь к системному администратору.", ItemName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }
            else
            {
                return Item.GetString("RowID");
            }
        }
        /// <summary>
        /// Получение даты.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static DateTime? GetNullableDateTime(object Value)
        {
            try
            { return (DateTime)Value; }
            catch
            { return null; }
        }
        /// <summary>
        ///  Получение перечня приборов и всех доп. изделий.
        /// </summary>
        public ArrayList FindAllDevices()
        {
            ArrayList DevicesList = new ArrayList();
            bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
            string Device = OnlyAccessories ? "" : GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString();
            WriteLog("Прибор: " + Device);
            DevicesList.Add(Device);

            GridView Grid_AdditionalWaresList = ICardControl.GetGridView(RefServiceCard.AdditionalWaresList.Alias);
            ITableControl Table_AdditionalWaresList = ICardControl.FindPropertyItem<ITableControl>(RefServiceCard.AdditionalWaresList.Alias);
            WriteLog("Ищем датчики. Кол-во: " + Table_AdditionalWaresList.RowCount.ToString());
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Device = UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString();
                //DeviceName = UniversalCard.GetItemPropertyValue(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid(), "Наименование прибора").ToString();
                //if (!DevicesList.Contains(DeviceName))
                DevicesList.Add(Device);
                WriteLog("Датчик: " + Device);
            }
            return DevicesList;
        }
        /// <summary>
        /// Проверка выбранных действий.
        /// </summary>
        /// <param name="Actions">Действия: Ремонт, Калибровка, Поверка.</param>
        /// <returns></returns>
        public String CheckActions(params Boolean[] Actions)
        {
            if (Actions != null)
            {
                if (Actions.Length >= 3)
                {
                    Int32 Code = 0;
                    if (Actions[0] || Actions[1]) // Ремонт
                        Code += 4;
                    if (Actions[2]) // Калибровка
                        Code += 2;
                    if (Actions[3]) // Поверка
                        Code += 1;
                    switch (Code)
                    {
                        // Не выбрано ни одного действия
                        case 0: return "Выберите необходимый вид сервисного обслуживания в поле «Требуется вид сервиса».";
                        // Поверка
                        case 1:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") < 0)
                                return "Очистите поле \"Поверка\" (данный вид сервиса не заказан клиентом).";
                            else
                                if (!NeedVerificationProtocol())
                                return "Необходимо указать вид сервиса 'Калибровка', т.к. данный прибор поверяется в сторонней организации.";
                            break;
                        // Калибровка
                        case 2:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") >= 0)
                                return "Заполните поле \"Поверка\" (данный вид сервиса заказан клиентом).";
                            break;
                        // Калибровка + Поверка
                        case 3:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") < 0)
                                return "Очистите поле \"Поверка\" (данный вид сервиса не заказан клиентом).";
                            break;
                        // Ремонт
                        case 4:
                            return "После ремонта обязательно должна осуществляться калибровка или поверка!"
                            + Environment.NewLine + "Выберите данный вид сервиса в поле «Требуется вид сервиса».";
                        // Поверка + Ремонт
                        case 5:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") < 0)
                                return "Очистите поле \"Поверка\" (данный вид сервиса не заказан клиентом).";
                            else
                                if (!NeedVerificationProtocol())
                                    return "Необходимо указать вид сервиса 'Калибровка', т.к. данный прибор поверяется в сторонней организации.";
                            break;
                        // Калибровка + Ремонт
                        case 6:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") >= 0)
                                return "Заполните поле \"Поверка\" (данный вид сервиса заказан клиентом).";
                            break;
                        // Калибровка + Поверка + ремонт
                        case 7:
                            if (GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("поверка") < 0)
                                return "Очистите поле \"Поверка\" (данный вид сервиса не заказан клиентом).";
                            break;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Форматирование типа сервисного обслуживания
        /// </summary>
        /// <param name="DeviceRepair">Требуется ремонт прибора.</param>
        /// <param name="AccessoriesRepair">Требуется ремонт комплектующих.</param>
        /// <param name="Calibrate">Требуется калибровка.</param>
        /// <param name="Verify">Требуется поверка.</param>
        public static string FormattingService(bool DeviceRepair, bool AccessoriesRepair, bool Calibrate, bool Verify)
        {
            StringCollection Services = new StringCollection();
            if (DeviceRepair == true) { Services.Add("ремонт прибора"); }
            if (AccessoriesRepair == true) { Services.Add("ремонт комплектующих"); }
            if (Calibrate == true) { Services.Add("калибровка"); }
            if (Verify == true) { Services.Add("поверка"); }

            string[] Serv = new string[Services.Count];
            Services.CopyTo(Serv, 0);
            return string.Join(", ", Serv);
        }
        /// <summary>
        /// Проверка заполнения трудоемкости диагностики дополнительных изделий.
        /// </summary>
        public static string CheckAdditionalWares(ITableControl Table_AdditionalWaresList, string FildName, MyBaseCard Card)
        {
            string Message = "";
            if (Table_AdditionalWaresList.RowCount > 0)
            {
                for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                {
                    //Card.WriteLog("Трудоемкость диагностики датчика: " + Table_AdditionalWaresList[i][FildName]);
                    //if (Table_AdditionalWaresList[i][FildName] == null || Table_AdditionalWaresList[i][FildName] == "")
                    if (Table_AdditionalWaresList[i][FildName].IsNull())
                    {
                        Card.WriteLog("Трудоемкость диагностики датчика нулевая");
                        Message = Message == "" ? Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumber].ToString() : Message + ", " + Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumber].ToString();
                    }
                }
            }
            return Message;
        }
        /// <summary>
        /// Формирование "Акта приемки на сервисное обслуживание".
        /// </summary>
        /// <param name="Template">Шаблон "Акта передачи".</param>
        /// <param name="CurrentDevice">Прибор.</param>
        /// <param name="ClientID">Идентификатор клиента.</param>
        /// <param name="ApplicationNumber">Номер заявки.</param>
        /// <param name="ApplicationDate">Дата поступления заявки.</param>
        /// <param name="FileListID">Идентификатор карточки списка файлов.</param>
        public Guid FillingDeedOfLoad(string Template, Device CurrentDevice, string ClientID, string ApplicationNumber, DateTime ApplicationDate, string FileListID)
        {
            string FileName = "Акт передачи";
            string Path = @"C:\Tmp\";
            string Format = ".doc";
            string TempPath = Path + FileName + Format;
            if (!System.IO.Directory.Exists(Path))
            { System.IO.Directory.CreateDirectory(Path); }

            CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(Template));
            VersionedFileCard VersionedFile = (VersionedFileCard)CardScript.Session.CardManager.GetCard((Guid)FileCard.Sections[FileCard.Type.Sections["MainInfo"].Id].FirstRow.GetGuid("FileID"));
            VersionedFile.CurrentVersion.Download(TempPath);

            Microsoft.Office.Interop.Word.Application objWord;
            objWord = new Microsoft.Office.Interop.Word.Application();
            Object wMissing = System.Reflection.Missing.Value;
            Object wTrue = true;
            Object wFalse = false;
            object wPath = Path + FileName + Format;
            Microsoft.Office.Interop.Word.Document objDoc = objWord.Documents.Add(ref wPath, ref wMissing, ref wTrue, ref wFalse);

            object bf;
            Microsoft.Office.Interop.Word.Table T = objDoc.Tables[2];

            string DeviceName = CurrentDevice.DeviceNumber == "" ? CurrentDevice.DeviceType + " №          " : CurrentDevice.DeviceNumber.Remove(CurrentDevice.DeviceNumber.IndexOf(" из партии "));
            string DeviceNumber = CurrentDevice.OnlyAccessories == false ? DeviceName + ". Комплектация: " : "Комплектующие для " + CurrentDevice.DeviceType + ": ";
            string CalibrateCertificate = CurrentDevice.Calibrate == true && CurrentDevice.OnlyAccessories == false ? "; Сертификат о калибровке" + CurrentDevice.DeviceType + " (1)" : "";
            string SensorsCalibrateCertificate = CurrentDevice.Calibrate == true && Table_AdditionalWaresList.RowCount > 0
                ? "; Сертификат о калибровке датчика (" + Table_AdditionalWaresList.RowCount + ")" : "";
            string VerificationCertificate = CurrentDevice.Verify == true ? "; Свидетельство о поверке (1)" : "";
            string AdditionalWares = CurrentDevice.Sensors != "" ? ". Заводские номера дополнительных изделий: " + CurrentDevice.Sensors : "";

            T.Rows.Add(ref wMissing);
            T.Cell(2, 1).Range.Text = "1";
            T.Cell(2, 2).Range.Text = DeviceNumber + CurrentDevice.Accessories + CalibrateCertificate + SensorsCalibrateCertificate + VerificationCertificate + AdditionalWares + ".";
            T.Cell(2, 3).Range.Text = "1";

            bf = "f1";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = ApplicationNumber;
            bf = "f2";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = ApplicationDate.ToShortDateString();
            bf = "f3";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = Context.GetObject<PartnersCompany>(ClientID.ToGuid()).Name;
            bf = "f4";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = Context.GetCurrentEmployee().DisplayName;
            bf = "f5";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = DateTime.Today.ToShortDateString();
            bf = "f7";
            objDoc.Bookmarks.get_Item(ref bf).Range.Text = Context.GetCurrentEmployee().PositionName;

            objDoc.SaveAs(ref wPath, ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing,
                        ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing,
                        ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing);
            object sc = WdSaveOptions.wdDoNotSaveChanges;
            object of = WdOriginalFormat.wdOriginalDocumentFormat;
            objWord.Quit(ref sc, ref of, ref wMissing);

            IReferenceListService ReferenceListService = Context.GetService<IReferenceListService>();
            ReferenceList FileList = Context.GetObject<ReferenceList>(FileListID.ToGuid());

            int k = Control_Files.FindFileByName(FileName + Format);
            if (k >= 0)
            {
                Control_Files.RemoveFile(k);
            }

            return Control_Files.AddFile(TempPath, true);
        }
        /// <summary>
        /// Обновляет строки таблицы "Ремонтне работы и доработки" (по родительской строке таблицы "Описание неисправностей").
        /// </summary>
        /// <param name="RowId">Идентификатор родительской строки.</param>
        /// <param name="Rows">Новый перечень ремонтных работ и доработок.</param>
        private void UpdateRepairWorks(Guid RowId, ArrayList Rows)
        {
            BaseCardProperty Table_RepairWorksRow;
            decimal SummTime = 0;

            for (Int32 i = 0; i < Table_RepairWorks.RowCount; i++)
            {
                Table_RepairWorksRow = Table_RepairWorks[i];
                if (Table_RepairWorksRow[RefServiceCard.RepairWorks.ParentTableRowId].ToGuid().Equals(RowId))
                    Table_RepairWorks.RemoveRow(CardScript.BaseObject, i--);
                else
                    SummTime = SummTime + (decimal)Table_RepairWorksRow[RefServiceCard.RepairWorks.FactLaboriousness];
            }
            foreach (Work Row in Rows)
            {
                Table_RepairWorksRow = Table_RepairWorks.AddRow(CardScript.BaseObject);
                Table_RepairWorksRow[RefServiceCard.RepairWorks.ParentTableRowId] = RowId;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.WorksType] = Row.WorkName;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.WorksTypeID] = Row.WorkID;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.Amount] = Row.Count;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.Revision] = Row.Improvements;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.Performer] = Row.PerformerID;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.FactLaboriousness] = Row.FactLaboriousness;
                WriteLog("Записываем дату");
                if (Row.EndDate.IsNull())
                    Table_RepairWorksRow[RefServiceCard.RepairWorks.EndDate] = null; //Row.EndDate;
                else
                    Table_RepairWorksRow[RefServiceCard.RepairWorks.EndDate] = (DateTime)Row.EndDate;
                Table_RepairWorksRow[RefServiceCard.RepairWorks.NegotiationResult] = Row.Result;
                Table_RepairWorks.RefreshRow(Table_RepairWorks.RowCount - 1);
                SummTime = SummTime + Row.FactLaboriousness;
            }
            SetControlValue(RefServiceCard.Adjustment.LaboriousnessRepair, SummTime);
        }
        /// <summary>
        /// Обновляет все строки таблицы "Ремонтне работы и доработки".
        /// </summary>
        private void UpdateRepairWorks()
        {
            BaseCardProperty Table_RepairWorksRow;
            BaseCardProperty Table_DescriptionRow;
            decimal SummTime = 0;

            for (Int32 i = 0; i < Table_RepairWorks.RowCount; i++)
            {
                bool Delete = true;
                Table_RepairWorksRow = Table_RepairWorks[i];

                for (Int32 j = 0; j < Table_Description.RowCount; j++)
                {
                    Table_DescriptionRow = Table_Description[j];
                    if (Table_RepairWorksRow[RefServiceCard.RepairWorks.ParentTableRowId].ToGuid().Equals(Table_DescriptionRow[RefServiceCard.DescriptionOfFault.Id].ToGuid()))
                        Delete = false;
                }
                if (Delete)
                    Table_RepairWorks.RemoveRow(CardScript.BaseObject, i--);
                else
                    SummTime = SummTime + (decimal)Table_RepairWorksRow[RefServiceCard.RepairWorks.FactLaboriousness];
            }
            SetControlValue(RefServiceCard.Adjustment.LaboriousnessRepair, SummTime);
        }
        /// <summary>
        /// Получение перечня ремонтных работ.
        /// </summary>
        /// <param name="ParentTableRowID">ID строки в таблице "Описание неисправностей".</param>
        private ArrayList GetWorks(Guid ParentTableRowID)
        {
            ArrayList Result = new ArrayList();
            BaseCardProperty Table_RepairWorksRow;
            for (Int32 i = 0; i < Table_RepairWorks.RowCount; i++)
            {
                Table_RepairWorksRow = Table_RepairWorks[i];
                if (Table_RepairWorksRow[RefServiceCard.RepairWorks.ParentTableRowId].ToGuid().Equals(ParentTableRowID))
                {
                    WriteLog("WorksTypeID");
                    string WorksTypeID = Table_RepairWorksRow[RefServiceCard.RepairWorks.WorksTypeID].ToString();
                    WriteLog("Amount");
                    int Amount = (int)Table_RepairWorksRow[RefServiceCard.RepairWorks.Amount];
                    WriteLog("Performer");
                    string Performer = Context.GetEmployeeDisplay(new Guid(Table_RepairWorksRow[RefServiceCard.RepairWorks.Performer].ToString()));
                    WriteLog("PerformerID");
                    string PerformerID = Table_RepairWorksRow[RefServiceCard.RepairWorks.Performer].ToString();
                    WriteLog("FactLaboriousness");
                    decimal FactLaboriousness = Math.Round((decimal)Table_RepairWorksRow[RefServiceCard.RepairWorks.FactLaboriousness], 2);
                    WriteLog("EndDate");
                    DateTime? EndDate = (DateTime?)Table_RepairWorksRow[RefServiceCard.RepairWorks.EndDate]; // (DateTime)Table_RepairWorksRow[RefServiceCard.RepairWorks.EndDate];
                    WriteLog("NegotiationResult");
                    string NegotiationResult = Table_RepairWorksRow[RefServiceCard.RepairWorks.NegotiationResult].ToString();
                    WriteLog("Получаем список работ");

                    Work NewWork = new Work(UniversalCard, WorksTypeID, Amount, Performer, PerformerID, FactLaboriousness, EndDate, NegotiationResult);
                    WriteLog("Добавляем новую работу");
                    Result.Add(NewWork);
                }
            }
            return Result;
        }
        /// <summary>
        /// Закрыть задание.
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="EndDateFildName"></param>
        private void TaskClose(string TaskID, string EndDateFildName)
        {
            CardData cTask = CardScript.Session.CardManager.GetCardData(new Guid(TaskID));
            if (EndDateFildName != "")
                SetControlValue(EndDateFildName, DateTime.Now);
            if (cTask.LockStatus != LockStatus.Free)
            {
                cTask.ForceUnlock();
                cTask.EndUpdate();
            }
            cTask.PlaceLock();
            if (cTask.InUpdate == false)
            { cTask.BeginUpdate(); }
            cTask.Sections[cTask.Type.Sections["Performing"].Id].FirstRow.SetInt32("TaskState", 5); //смена статуса задания

            //Перенос ярлыка в папку "Завершенные"
            StaffEmployee Emp = Context.GetCurrentEmployee();
            //FoldersFolder PersonalFolder = Emp.PersonalFolder;
            CardData StaffDictionaryCard = CardScript.Session.CardManager.GetCardData(DocsVision.Platform.Cards.Constants.RefStaff.ID);
            SectionData EmployeesSection = StaffDictionaryCard.Sections[DocsVision.Platform.Cards.Constants.RefStaff.Employees.ID];

            RowData EmployeeRow = EmployeesSection.GetRow(Context.GetObjectRef(Emp).Id);
            Guid PersonalFolderId = EmployeeRow.GetGuid(DocsVision.Platform.Cards.Constants.RefStaff.Employees.PersonalFolder).ToGuid();
            Folder PF = FolderCard.GetFolder(PersonalFolderId);
            Folder Completed = null;
            if (PF.Folders["Завершенные"] == null)
            { Completed = PF.Folders.AddNew("Завершенные"); }
            else
            { Completed = PF.Folders["Завершенные"]; }
            DocsVision.Platform.ObjectManager.SystemCards.ShortcutCollection MyShortcuts = FolderCard.GetShortcuts(cTask.Id);
            if (MyShortcuts.Count > 0)
            {
                foreach (DocsVision.Platform.ObjectManager.SystemCards.Shortcut MyShortcut in MyShortcuts)
                {
                    try
                    {
                        if (FolderCard.GetFolder(MyShortcut.FolderId).Name != "Последние")
                        { MyShortcut.Move(Completed.Id); }
                    }
                    catch { }
                }
            }
            cTask.EndUpdate();
            cTask.RemoveLock();
        }
        /// <summary>
        /// Формирование файла упаковочного листа.
        /// </summary>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="DeviceName">Наименование типа прибора.</param>
        /// <param name="ACData">Данные комплектации.</param>
        /// <param name="FileId">Идентификатор формируемого файла.</param>
        public static void FillPackFile(ObjectContext Context, String DeviceName, String ACData, ref Guid FileId)
        {
            /* Вспомогательные сервисы */
            IVersionedFileCardService VersionedFileCardService = Context.GetService<IVersionedFileCardService>();
            IDocumentService DocumentService = Context.GetService<IDocumentService>();

            /* Получение шаблона */
            String FilePath = System.IO.Path.GetTempPath() + "Упаковочный лист " + DeviceName.Replace("/", ".") + ".docx";
            VersionedFileCard FileVersionCard = (VersionedFileCard)Context.GetService<UserSession>().CardManager.GetCard(RefPackFileTemplate);
            FileVersionCard.CurrentVersion.Download(FilePath);

            WordprocessingDocument Document = WordprocessingDocument.Open(FilePath, true);
            Body objBody = Document.MainDocumentPart.Document.Body;

            /* Преобразовали данные */
            IEnumerable<DeviceCompleteRow> Rows = String.IsNullOrWhiteSpace(ACData) ? new DeviceCompleteRow[0] : ACData.Split('\n').Select(DataRow => (DeviceCompleteRow)DataRow).Where(Row => Row.Count > 0);

            DocumentFormat.OpenXml.Wordprocessing.Table WorkTable = objBody.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(1);

            DeviceCompleteRow[] StandartRows = Rows.Where(Row => Row.TableType == 0).ToArray(),
                AdditionalRows = Rows.Where(Row => Row.TableType != 0).ToArray();

            FillCompleteTable(objBody, "Упакованы следующие изделия", WorkTable, StandartRows);

            WorkTable = objBody.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ElementAt(StandartRows.Length > 0 ? 2 : 1);

            FillCompleteTable(objBody, "Дополнительная комплектация", WorkTable, AdditionalRows);

            Document.MainDocumentPart.Document.Save();
            Document.Close();
            Document = null;

            WordWatermark.AddWatermarkInDoc(FilePath, "Предварительный");
            VersionedFileCard FileCard;
            if (FileId.IsEmpty())
            {
                FileCard = VersionedFileCardService.CreateCard(FilePath);
                FileId = FileCard.Id;
            }
            else
            {
                FileCard = VersionedFileCardService.OpenCard(FileId);
                FileCard.ForceUnlock();

                if (FileCard.CheckedOut)
                    FileCard.UndoCheckout();

                VersionedFileCardService.Upload(FileCard, FilePath, Context.GetCurrentUser());
            }
        }
        /// <summary>
        /// Заполнение для указанного документа таблицы комплектации прибора.
        /// </summary>
        /// <param name="objBody">Документ.</param>
        /// <param name="Caption">Заголовок таблицы.</param>
        /// <param name="WorkTable">Таблица.</param>
        /// <param name="Rows">Строки таблицы.</param>
        private static void FillCompleteTable(Body objBody, String Caption, DocumentFormat.OpenXml.Wordprocessing.Table WorkTable, DeviceCompleteRow[] Rows)
        {
            if (Rows.Length > 0)
            {
                for (Int32 i = 0; i < Rows.Length; i++)
                {
                    TableRow Row = new TableRow();
                    Row.Append(MyHelper.GenerateXMLTableCell((i + 1).ToString(), JustificationValues.Center));
                    Row.Append(MyHelper.GenerateXMLTableCell(Rows[i].Code));
                    Row.Append(MyHelper.GenerateXMLTableCell(Rows[i].Name));
                    Row.Append(MyHelper.GenerateXMLTableCell(Rows[i].Count.ToString(), JustificationValues.Center));
                    Row.Append(MyHelper.GenerateXMLTableCell(String.Empty));

                    WorkTable.Append(Row);
                }
            }
            else
            {
                DocumentFormat.OpenXml.Wordprocessing.Paragraph Selected =
                objBody.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                    .Where(item => item.Descendants<Run>().Count() != 0)
                    .Where(item => item.Descendants<Run>().FirstOrDefault().Descendants<Text>().Count() != 0)
                    .Where(item => item.Descendants<Run>().FirstOrDefault().Descendants<Text>().FirstOrDefault()
                        .Text == Caption)
                        .FirstOrDefault();
                if (Selected != null)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Paragraph PreviousSibling = Selected.PreviousSibling<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                    if (PreviousSibling != null) { PreviousSibling.Remove(); }

                    Selected.Remove();

                    DocumentFormat.OpenXml.Wordprocessing.Paragraph NextSibling = Selected.NextSibling<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                    if (NextSibling != null)
                        NextSibling.Remove();
                }
                WorkTable.Remove();
            }
        }
        /// <summary>
        /// Добавляет в упаковочный лист сертификаты о калибровке для приборов и доп. изделий.
        /// </summary>
        /// <param name="CData">Данные упаковочного листа.</param>
        /// <param name="DeviceName">Название прибора.</param>
        /// <param name="AdditionalWares">Перечень дополнительных изделий.</param>
        /// <param name="OA">Только комплектующие.</param>
        /// <param name="Calibration">Только комплектующие.</param>
        /// <param name="Verification">Только комплектующие.</param>
        private void AddCompleteSertificates(ref String CData, String DeviceName, List<string> AdditionalWares, bool OA, bool Calibration, bool Verification)
        {
            
            List<DeviceCompleteRow> Rows = CData.Split('\n').Select(DataRow => (DeviceCompleteRow)DataRow).ToList();
            RowData Devices = UniversalCard.Sections[RefUniversal.ItemType.ID].GetRow(MyHelper.RefItem_Devices);
            RowData DeviceCompleteInfo = Devices.AllChildRows.Find(RefUniversal.ItemType.Name, DeviceName);

            if (!DeviceCompleteInfo.IsNull())
            {
                SubSectionData DeviceItems = DeviceCompleteInfo.ChildSections[RefUniversal.Item.ID];
                IEnumerable<DeviceCompleteRow> SortedRows = DeviceItems.Rows.OrderBy(item => item.GetInt32(RefUniversal.Item.Order)).Select(r => (DeviceCompleteRow)r);
                DeviceCompleteRow Certificate;
                if (!OA)
                {
                    if (Calibration && GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString().IndexOf("калибровка") >= 0)
                    {
                        
                        Certificate = Rows.FirstOrDefault(r => r.Name == "Сертификат о калибровке " + DeviceName && r.TableType == 1);
                        if (Certificate.IsNull() || Certificate.Count.IsNull() || Certificate.Count == 0)
                        {
                            DeviceCompleteRow Row = SortedRows.FirstOrDefault(r => r.Name == "Сертификат о калибровке " + DeviceName);
                            if (!Row.IsNull())
                            {
                                Row.TableType = 1;
                                Rows.Add(Row);
                            }
                            else
                            {
                                MessageBox.Show("Не найдена позиция \"Сертификат о калибровке\" в упаковочном листе прибора" + DeviceName);
                                return;
                            }
                        }
                    }
                    if (Verification)
                    {
                        Certificate = Rows.FirstOrDefault(r => r.Name == "Свидетельство о поверке" && r.TableType == 1);
                        if (Certificate.IsNull() || Certificate.Count.IsNull() || Certificate.Count == 0)
                        {
                            DeviceCompleteRow Row = SortedRows.FirstOrDefault(r => r.Name == "Свидетельство о поверке");
                            if (!Row.IsNull())
                            {
                                Row.TableType = 1;
                                Rows.Add(Row);
                            }
                            else
                            {
                                MessageBox.Show("Не найдена позиция \"Свидетельство о поверке\" в упаковочном листе прибора " + DeviceName);
                                return;
                            }
                        }
                    }
                }

                // Раньше выписывались отдельные сертификаты на доп. изделия, потом это убрали, т.к. доп. изделия не являются средствами измерения.
                /*foreach (string AdditionalWaresType in AdditionalWares)
                {
                    Certificate = Rows.FirstOrDefault(r => r.Name == "Сертификат о калибровке " + AdditionalWaresType && r.TableType == 1);
                    if (Certificate.IsNull() || Certificate.Count.IsNull() || Certificate.Count == 0)
                    {
                        DeviceCompleteRow Row = SortedRows.FirstOrDefault(r => r.Name == "Сертификат о калибровке " + AdditionalWaresType);
                        if (!Row.IsNull())
                        {
                            Row.TableType = 1;
                            Rows.Add(Row);
                        }
                        else
                        {
                            //MessageBox.Show("Не найдена позиция \"Сертификат о калибровке " + AdditionalWaresType + "\"");
                            return;
                        }
                    }
                }*/
            }
            CData = Rows.Select(r => r.ToString()).Aggregate((a, b) => a + "\n" + b);
        }
        /// <summary>
        /// Корректирует список доп. изделий в упаковочном листе.
        /// </summary>
        /// <param name="CData">Данные упаковочного листа.</param>
        /// <param name="DeviceName">Название прибора.</param>
        /// <param name="AdditionalWares">Перечень дополнительных изделий.</param>
        /// <param name="OA">Только комплектующие.</param>
        private void SensorsListCorrection(ref String CData, String DeviceName, List<string> AdditionalWares, bool OA)
        {
            List<DeviceCompleteRow> Rows = CData.Split('\n').Select(DataRow => (DeviceCompleteRow)DataRow).ToList();
            RowData Devices = UniversalCard.Sections[RefUniversal.ItemType.ID].GetRow(MyHelper.RefItem_Devices);
            RowData DeviceCompleteInfo = Devices.AllChildRows.Find(RefUniversal.ItemType.Name, DeviceName);

            if (!DeviceCompleteInfo.IsNull())
            {
                SubSectionData DeviceItems = DeviceCompleteInfo.ChildSections[RefUniversal.Item.ID];
                IEnumerable<DeviceCompleteRow> SortedRows = DeviceItems.Rows.OrderBy(item => item.GetInt32(RefUniversal.Item.Order)).Select(r => (DeviceCompleteRow)r);

                for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                {
                    Guid WaresNumberID = new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString());
                    Guid WaresCardID = new Guid(UniversalCard.GetItemPropertyValue(WaresNumberID, "Паспорт прибора").ToString());
                    string WaresTypeID = UniversalCard.GetItemPropertyValue(WaresNumberID, "Наименование прибора").ToString();
                    string WaresTypeName = UniversalCard.GetItemName(new Guid(WaresTypeID));
                    string WaresCodeID = UniversalCard.GetItemPropertyValue(new Guid(WaresTypeID), "Код СКБ").ToString();
                    string WaresCode = UniversalCard.GetItemName(new Guid(WaresCodeID));

                    // Определяем, была ли замена датчика
                    bool NeedReplace;         // Требуется замена датчика
                    bool RefusalToReplace;    // Отказ от замены датчика
                    bool RefusalToRepair;     // Отказ от ремонта датчика
                    string ReplacementID;     // ID паспорта нового датчика, предназначенного для замены неремонтопригодного изделия
                    string OldProductAction;  // Действие со старым изделием (списать/вернуть клиенту)

                    // Получаем данные о результатах ремонта датчика
                    ResultOfRepairForDevice(WaresCardID, out RefusalToRepair, out NeedReplace, out RefusalToReplace, out ReplacementID, out OldProductAction);

                    // При отказе от ремонта датчика
                    if (RefusalToRepair)
                    {
                        // Если с клиентом согласована замена датчика на новый
                        if (NeedReplace && !RefusalToReplace)
                        {
                            if (OldProductAction == "Вернуть клиенту")
                            {
                                IEnumerable<DeviceCompleteRow> WaresComplete = Rows.Where(r => r.Code.IndexOf(WaresCode) >= 0 && r.TableType == 1);
                                foreach (DeviceCompleteRow CompleteRow in WaresComplete)
                                {
                                    int CurrentCount = CompleteRow.Count == null ? 0 : (int)CompleteRow.Count;
                                    CompleteRow.Count = CurrentCount + 1;
                                }
                            }
                        }
                    }

                    // При отсутствии отказа от ремонта датчика
                    if (!RefusalToRepair)
                    {
                        // Если с клиентом согласована замена датчика на новый
                        if (NeedReplace && !RefusalToReplace)
                        {
                            if (OldProductAction == "Вернуть клиенту")
                            {
                                IEnumerable<DeviceCompleteRow> WaresComplete = Rows.Where(r => r.Code.IndexOf(WaresCode) >= 0 && r.TableType == 1);
                                foreach (DeviceCompleteRow CompleteRow in WaresComplete)
                                {
                                    int CurrentCount = CompleteRow.Count == null ? 0 : (int)CompleteRow.Count;
                                    CompleteRow.Count = CurrentCount + 1;
                                }
                            }
                        }
                    }
                }
            }
            CData = Rows.Select(r => r.ToString()).Aggregate((a, b) => a + "\n" + b);
        }
        /// <summary>
        /// Возвращает перечень изделий, пригодных для калибровки, из заданного списка
        /// </summary>
        /// <param name="WaresList">Перечень изделий, среди которых осуществляется поиск.</param>
        private List<CardData> GetWaresListForCalibration(string[] WaresList)
        {
            // Перечень датчиков
            List<CardData> AdditionalWaresList = new List<CardData>();

            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Guid WaresNumberID = new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString());

                Guid WaresCardID = new Guid(UniversalCard.GetItemPropertyValue(WaresNumberID, "Паспорт прибора").ToString());

                string WaresTypeID = UniversalCard.GetItemPropertyValue(WaresNumberID, "Наименование прибора").ToString();

                string WaresTypeName = UniversalCard.GetItemName(new Guid(WaresTypeID));

                // Проверка, входит ли доп. изделие в перечень датчиков, которые включаются в сертификаты о калибровке
                if (WaresList.Any(r => r == WaresTypeName))
                {
                    // Определяем, была ли замена датчика
                    bool NeedReplace;         // Требуется замена датчика
                    bool RefusalToReplace;    // Отказ от замены датчика
                    bool RefusalToRepair;     // Отказ от ремонта датчика
                    string ReplacementID;     // ID паспорта нового датчика, предназначенного для замены неремонтопригодного изделия
                    string OldProductAction;  // Действие со старым изделием (списать/вернуть клиенту)

                    // Получаем данные о результатах ремонта датчика
                    ResultOfRepairForDevice(WaresCardID, out RefusalToRepair, out NeedReplace, out RefusalToReplace, out ReplacementID, out OldProductAction);

                    // При отказе от ремонта датчика
                    if (RefusalToRepair)
                    {
                        // Если с клиентом согласована замена датчика на новый
                        if (NeedReplace && !RefusalToReplace)
                        { AdditionalWaresList.Add(CardScript.Session.CardManager.GetCardData(new Guid(ReplacementID))); }
                    }

                    // При отсутствии отказа от ремонта датчика
                    if (!RefusalToRepair)
                    {
                        // Если с клиентом согласована замена датчика на новый
                        if (NeedReplace && !RefusalToReplace)
                        { AdditionalWaresList.Add(CardScript.Session.CardManager.GetCardData(new Guid(ReplacementID))); }
                        if (!NeedReplace)   // Замена датчика не требуется
                        { AdditionalWaresList.Add(CardScript.Session.CardManager.GetCardData(WaresCardID)); }
                    }
                }
            }
            return AdditionalWaresList;
        }
        /// <summary>
        /// Возвращает данные о результатах ремонта изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        /// <param name="RefusalToRepair">Отказ от ремонта изделия.</param>
        /// <param name="NeedReplace">Требуется замена изделия на новое.</param>
        /// <param name="RefusalToReplace">Отказ от замены изделия.</param>
        /// <param name="ReplacementID">ID паспорта изделия-замены.</param>
        /// <param name="OldProductAction">Выбранное действие для старого изделия.</param>
        private void ResultOfRepairForDevice(Guid WaresCardID, out bool RefusalToRepair, out bool NeedReplace, out bool RefusalToReplace, out string ReplacementID, out string OldProductAction)
        {
            // Определяем, была ли замена датчика
            NeedReplace = false;
            RefusalToReplace = false;
            RefusalToRepair = false;
            ReplacementID = "";
            OldProductAction = "";

            for (int j = 0; j < Table_Description.RowCount; j++)
            {
                string SerialNumber = Table_Description[j][RefServiceCard.DescriptionOfFault.SerialNumberID].ToString();
                Guid SerialNumberID = SerialNumber == "" ? Guid.Empty : new Guid(SerialNumber);
                if (SerialNumberID.Equals(WaresCardID))
                {
                    for (int k = 0; k < Table_RepairWorks.RowCount; k++)
                    {
                        int WorkType = (int)UniversalCard.GetItemPropertyValue(new Guid(Table_RepairWorks[k][RefServiceCard.RepairWorks.WorksTypeID].ToString()), "Тип работ");
                        string Result = Table_RepairWorks[k][RefServiceCard.RepairWorks.NegotiationResult].ToString();
                        if (WorkType == 4)      // Тип ремонтной работы - "Замена"
                        {
                            NeedReplace = true;
                            if (Result == "Не выполнять")
                            { RefusalToReplace = true; }

                            ReplacementID = Table_Description[j][RefServiceCard.DescriptionOfFault.ReplacementID].ToString();
                            OldProductAction = Table_Description[j][RefServiceCard.DescriptionOfFault.OldProductAction].ToString();
                        }
                        if (WorkType == 1)      // Тип ремонтной работы - "Ремонт"
                        {
                            if (Result == "Не выполнять")
                            { RefusalToRepair = true; }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Возвращает ID какрточки файла протокола калибровки для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        private string GetAdditionalProtocol(Guid WaresCardID)
        {
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Guid WaresPassportID = new Guid(UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString());

                if (WaresPassportID.Equals(WaresCardID))
                { return Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol].ToString(); }
            }
            return "";
        }
        /// <summary>
        /// Возвращает ID какрточки файла сертификата о калибровке для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        private string GetAdditionalCertificate(Guid WaresCardID)
        {

            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                string WaresPassportID = UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString();
                if (WaresPassportID == WaresCardID.ToString())
                { return Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate].ToString(); }
            }
            return "";
        }
        /// <summary>
        /// Задает ID какрточки файла протокола калибровки для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        /// <param name="ProtocolCardID">ID карточки файла протокола калибровки.</param>
        private void SetAdditionalProtocol(Guid WaresCardID, Guid ProtocolCardID)
        {
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                string WaresPassportID = UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString();
                if (WaresPassportID == WaresCardID.ToString())
                { Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol] = ProtocolCardID; }
            }
        }
        /// <summary>
        /// Задает ID какрточки файла сертификата о калибровке для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        /// <param name="CertificateCardID">ID карточки файла сертификата о калибровке.</param>
        private void SetAdditionalCertificate(Guid WaresCardID, Guid CertificateCardID)
        {
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                string WaresPassportID = UniversalCard.GetItemPropertyValue(new Guid(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString()), "Паспорт прибора").ToString();
                if (WaresPassportID == WaresCardID.ToString())
                { Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate] = CertificateCardID; }
            }
        }
        /// <summary>
        /// Определяет, требуется ли для данного прибора в обязательном порядке формирование протокола калибровки.
        /// </summary>
        private bool NeedCalibrationProtocol()
        {
            string DeviceTypeID = GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString();
            RowData DeviceTypeRow = UniversalCard.GetItemRow(new Guid(DeviceTypeID));
            string DeviceTypeName = UniversalCard.GetItemName(new Guid(DeviceTypeID));

            RowData ItemsType = UniversalCard.GetItemTypeRow(new Guid(DeviceTypeRow.GetString("ParentRowID")));
            IEnumerable<RowData> FindDictionary = ItemsType.ChildRows.Where(r => r.GetString(RefUniversal.ItemType.Name) == DeviceTypeName);

            foreach (RowData Row in FindDictionary)
            {
                if (Row.ChildSections[RefUniversal.Item.ID].Rows.Any(r => r.GetString(RefUniversal.Item.Name) == "Свидетельство о поверке"))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Определяет, требуется ли для данного прибора в обязательном порядке формирование протокола поверки.
        /// </summary>
        private bool NeedVerificationProtocol()
        {
            if (UniversalCard.GetItemPropertyValue(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid(), "Формирование протокола поверки") != null)
                return true;
            return false;
        }
        /// <summary>
        /// Поиск идентификатора состояния по номеру.
        /// </summary>
        /// <param name="Card"> Карточка. </param>
        /// <param name="StateName"> Название состояния. </param>
        /// <returns></returns>
        private string GetState(CardData Card, string StateName)
        {
            CardData RefState = CardScript.Session.CardManager.GetCardData(new Guid("{443F55F0-C8AB-4DD3-BCBD-5328C7C9D385}"));

            SectionData CardKindStateSettings = RefState.Sections[RefState.Type.AllSections["CardKindStateSettings"].Id];
            SectionData System = Card.Sections[Card.Type.AllSections["System"].Id];
            string Kind = System.FirstRow.GetString("Kind").ToString();

            RowData KindState = null;
            foreach (RowData Row in CardKindStateSettings.Rows)
            {
                if (Row.GetString("Kind").ToString() == Kind)
                { KindState = Row; }
            }
            SubSectionData States = KindState.ChildSections[RefState.Type.AllSections["States"].Id];
            RowData State = States.FindRow("@DefaultName = '" + StateName + "'");
            return State.GetString("RowID").ToString();
        }

        /// <summary>
        /// Изменение состояния прибора
        /// </summary>
        /// <param name="DeviceCard"> Карточка "Паспорта прибора".</param>
        /// <param name="StateNumber"> Числовое обозначение состояния прибора.</param>
        /// <param name="StateName"> Текстовое обозначение состояния прибора.</param>
        private void ChangeStateDevice(CardData DeviceCard, int StateNumber, string StateName)
        {
            try
            {
                WriteLog("Производится изменение состояния прибора " + DeviceCard.Description + " в карточке Паспорта...");
                RowData DeviceStateRow = DeviceCard.Sections[CardOrd.Properties.ID].Rows.First(r => r.GetString(CardOrd.Properties.Name) == "Состояние прибора");
                DeviceStateRow.SetInt32(CardOrd.Properties.Value, StateNumber);
                DeviceStateRow.SetString(CardOrd.Properties.DisplayValue, StateName);

                WriteLog("Производится изменение состояния прибора " + DeviceCard.Description + " в универсальном справочнике...");
                RowData DeviceItemRow = DeviceCard.Sections[CardOrd.Properties.ID].Rows.First(r => r.GetString(CardOrd.Properties.Name) == "Запись в справочнике");
                RowData ItemRow = UniversalCard.GetItemRow((Guid)DeviceItemRow.GetGuid(CardOrd.Properties.Value));
                RowData RowState = ItemRow.ChildSections[0].Rows.First(r => r.GetString("Name") == "Статус");
                RowState.SetInt32("Value", StateNumber);
                RowState.SetString("DisplayValue", StateName);
                WriteLog("Состояние прибора " + DeviceCard.Description + " успешно изменено...");
            }
            catch (Exception Ex)
            {
                WriteLog("В процессе изменения состояния прибора " + DeviceCard.Description + " произошла ошибка: " + Ex.Message);
                CallError(Ex);
                MessageBox.Show("Внимание! В процессе изменения состояния прибора " + DeviceCard.Description + " произошла ошибка: " + Ex.Message);
            }
        }
        /// <summary>
        /// Отправка текущего Наряда на СО в сбыт
        /// </summary>
        /// <returns></returns>
        private bool SendToSale(int CurrentWorkOrderState)
        {
            try
            {
                WriteLog("Начата отправка в отдел сбыта...");

                IReferenceListService ReferenceListService = Context.GetService<IReferenceListService>();
                ReferenceList RefList = Context.GetObject<ReferenceList>(Control_Links.ReferenceListID);

                WriteLog("Производится проверка выполнения родительской заявки...");
                if (CurrentWorkOrderState == (int)RefServiceCard.MainInfo.State.Completed || CurrentWorkOrderState == (int)RefServiceCard.MainInfo.State.Failure || CurrentWorkOrderState == (int)RefServiceCard.MainInfo.State.Payment)
                {
                    if (RefList.References.Any(RefRef => RefRef.CardType.Equals(RefApplicationCard.ID)))
                    {
                        WriteLog("Ссылка на родительскую заявку найдена...");
                        Guid ApplicationCardID = RefList.References.First(RefRef => RefRef.CardType.Equals(RefApplicationCard.ID)).Card;
                        CardData ApplicationCard = CardScript.Session.CardManager.GetCardData(ApplicationCardID);

                        if (ApplicationCard.LockStatus == LockStatus.Locked)
                        {
                            WriteLog("Родительская заявка заблокирована...");
                            ApplicationCard.RemoveLock();
                            WriteLog("Выполнена принудительная разблокировка родительской заявки...");
                        }
                        SectionData ServiceSection = ApplicationCard.Sections[RefApplicationCard.Service.ID];
                        bool CloseApplication = true;
                        foreach (RowData CurrentRow in ServiceSection.Rows)
                        {
                            if (CurrentRow.GetGuid(RefApplicationCard.Service.WorkOrderID) != this.CardScript.CardData.Id)
                            {
                                CardData CurrentServiceCard = CardScript.Session.CardManager.GetCardData((Guid)CurrentRow.GetGuid(RefApplicationCard.Service.WorkOrderID));
                                if ((int)CurrentServiceCard.Sections[RefServiceCard.MainInfo.ID].FirstRow.GetInt32(RefServiceCard.MainInfo.Status) != (int)RefServiceCard.MainInfo.State.Completed &&
                                    (int)CurrentServiceCard.Sections[RefServiceCard.MainInfo.ID].FirstRow.GetInt32(RefServiceCard.MainInfo.Status) != (int)RefServiceCard.MainInfo.State.Failure &&
                                    (int)CurrentServiceCard.Sections[RefServiceCard.MainInfo.ID].FirstRow.GetInt32(RefServiceCard.MainInfo.Status) != (int)RefServiceCard.MainInfo.State.Payment)
                                {
                                    WriteLog(CurrentServiceCard + " не завершен, находится в состоянии " + (int)CurrentServiceCard.Sections[RefServiceCard.MainInfo.ID].FirstRow.GetInt32(RefServiceCard.MainInfo.Status) + "...");
                                    CloseApplication = false;
                                }
                            }
                        }

                        if (CloseApplication)
                        {
                            WriteLog("Все работы по заявке выполнены, производится завершение заявки...");
                            ApplicationCard.Sections[RefApplicationCard.MainInfo.ID].FirstRow.SetInt32(RefApplicationCard.MainInfo.Status, (int)RefApplicationCard.MainInfo.State.Performed);
                            ApplicationCard.Sections[RefApplicationCard.MainInfo.ID].FirstRow.SetDateTime(RefApplicationCard.MainInfo.DateEndFact, DateTime.Today);
                            ApplicationCard.Sections[ApplicationCard.Type.AllSections["System"].Id].FirstRow.SetString("State", GetState(ApplicationCard, "Performed"));
                            WriteLog("Родительская заявка завершена...");
                        }
                        else
                        {

                            WriteLog("Родительская заявка остается в работе...");
                        }
                    }
                    else
                    {
                        WriteLog("Ссылка на родительскую заявку НЕ найдена...");
                        return false;
                    }
                }
                else
                {
                    WriteLog("Текущий Наряд на СО еще не завершен, родительская заявка остается в работе...");
                }
                WriteLog("Начато внесение изменений в паспорта приборов...");
                bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);

                ArrayList AllDevices = FindAllDevices();
                if (AllDevices.Count > 0)
                {
                    WriteLog("Начато внесение изменений в паспорта приборов...");
                    foreach (string DeviceCardId in AllDevices)
                    {
                        CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(DeviceCardId));
                        WriteLog("Найдена карточка " + DeviceCard.Description + "...");
                        if (DeviceCard.LockStatus == LockStatus.Locked)
                        {
                            WriteLog("Карточка заблокирована...");
                            DeviceCard.RemoveLock();
                            WriteLog("Выполнена принудительная разблокировка карточки...");
                        }
                        WriteLog("Начато изменение состояния прибора...");
                        ChangeStateDevice(DeviceCard, (int)SKB.Base.Enums.DeviceState.Stocking, SKB.Base.Enums.DeviceState.Stocking.GetDescription());
                    }
                    WriteLog("Внесение изменений в паспорта приборов завершено...");
                }
                else
                {
                    WriteLog("В текущем наряде на СО нет ни одного прибора...");
                }

                WriteLog("Отправка в отдел сбыта завершена...");
                return true;
            }
            catch (Exception Ex)
            {
                WriteLog("При отправке текущего Наряда в сбыт произошла ошибка: " + Ex.Message);
                CallError(Ex);
                MyMessageBox.Show("Внимание! При отправке текущего Наряда в сбыт произошла ошибка: " + Ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Отправка текущего Наряда на СО в сбыт
        /// </summary>
        /// <returns></returns>
        private bool SendToRepair(int CurrentWorkOrderState)
        {
            try
            {
                WriteLog("Начата отправка в ремонт...");

                IReferenceListService ReferenceListService = Context.GetService<IReferenceListService>();
                ReferenceList RefList = Context.GetObject<ReferenceList>(Control_Links.ReferenceListID);

                WriteLog("Начато внесение изменений в паспорта приборов...");
                bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);

                ArrayList AllDevices = FindAllDevices();
                if (AllDevices.Count > 0)
                {
                    WriteLog("Начато внесение изменений в паспорта приборов...");
                    foreach (string DeviceCardId in AllDevices)
                    {
                        CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(DeviceCardId));
                        WriteLog("Найдена карточка " + DeviceCard.Description + "...");
                        if (DeviceCard.LockStatus == LockStatus.Locked)
                        {
                            WriteLog("Карточка заблокирована...");
                            DeviceCard.RemoveLock();
                            WriteLog("Выполнена принудительная разблокировка карточки...");
                        }
                        WriteLog("Начато изменение состояния прибора...");
                        ChangeStateDevice(DeviceCard, (int)SKB.Base.Enums.DeviceState.Repairing, SKB.Base.Enums.DeviceState.Repairing.GetDescription());
                    }
                    WriteLog("Внесение изменений в паспорта приборов завершено...");
                }
                else
                {
                    WriteLog("В текущем наряде на СО нет ни одного прибора...");
                }

                WriteLog("Отправка в ремонт завершена...");
                return true;
            }
            catch (Exception Ex)
            {
                WriteLog("При отправке текущего Наряда в ремонт произошла ошибка: " + Ex.Message);
                CallError(Ex);
                MyMessageBox.Show("Внимание! При отправке текущего Наряда в ремонт произошла ошибка: " + Ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Отправка текущего Наряда на СО на техобслуживание
        /// </summary>
        /// <returns></returns>
        private bool SendToMaintenance(int CurrentWorkOrderState)
        {
            try
            {
                WriteLog("Начата отправка на техобслуживание...");

                IReferenceListService ReferenceListService = Context.GetService<IReferenceListService>();
                ReferenceList RefList = Context.GetObject<ReferenceList>(Control_Links.ReferenceListID);

                WriteLog("Начато внесение изменений в паспорта приборов...");
                bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);

                ArrayList AllDevices = FindAllDevices();
                if (AllDevices.Count > 0)
                {
                    WriteLog("Начато внесение изменений в паспорта приборов...");
                    foreach (string DeviceCardId in AllDevices)
                    {
                        CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(DeviceCardId));
                        WriteLog("Найдена карточка " + DeviceCard.Description + "...");
                        if (DeviceCard.LockStatus == LockStatus.Locked)
                        {
                            WriteLog("Карточка заблокирована...");
                            DeviceCard.RemoveLock();
                            WriteLog("Выполнена принудительная разблокировка карточки...");
                        }
                        WriteLog("Начато изменение состояния прибора...");
                        ChangeStateDevice(DeviceCard, (int)SKB.Base.Enums.DeviceState.ReCalibrating, SKB.Base.Enums.DeviceState.ReCalibrating.GetDescription());
                    }
                    WriteLog("Внесение изменений в паспорта приборов завершено...");
                }
                else
                {
                    WriteLog("В текущем наряде на СО нет ни одного прибора...");
                }

                WriteLog("Отправка на техобслуживание завершена...");
                return true;
            }
            catch (Exception Ex)
            {
                WriteLog("При изменении статуса приборов на 'На калиброке (СО)' произошла ошибка: " + Ex.Message);
                CallError(Ex);
                MyMessageBox.Show("Внимание! При изменении статуса приборов на 'На калиброке (СО)' произошла ошибка: " + Ex.Message);
                return false;
            }
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

        /// <summary>
        /// Определение необходимости согласования.
        /// </summary>
        public bool NeedsNegotiation()
        {
            // ========== Определение необходимости согласования ==========
            WriteLog("Начато определение необходимости согласования...");
            double AgreedRepairCost = GetControlValue(RefServiceCard.Adjustment.AgreedRepairCost) == null ? 0 : Convert.ToDouble(GetControlValue(RefServiceCard.Adjustment.AgreedRepairCost));
            List<BaseCardProperty> RepairWorks = Table_RepairWorks.Select();
            // Если все ремонтные работы согласованы, то согласование не требуется.
            if (!RepairWorks.Any(r => r[RefServiceCard.RepairWorks.NegotiationResult].ToString() == "Не согласовано"))
            {
                WriteLog("Все ремонтные работы согласованы...");
                return false;
            }
            // Если согласование первичное
            if (AgreedRepairCost == 0)
            {
                if (!RepairWorks.Any(r => !(bool)r[RefServiceCard.RepairWorks.Revision]) && !RepairWorks.Any(r =>
                UniversalCard.GetItemPropertyValue(r[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Обязательная доработка") == null ||
                (bool)UniversalCard.GetItemPropertyValue(r[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Обязательная доработка") == false))
                {
                    WriteLog("Среди ремонтных работ выбраны только обязательные доработки...");
                    return false;
                }
            }
            // Если согласование повторное
            else
            {
                // Определяем новую стоимость ремонтных работ
                double NewCost = RepairWorks.Select(r => UniversalCard.GetItemPropertyValue(r[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)") == null ?
                0 : Convert.ToDouble(UniversalCard.GetItemPropertyValue(r[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)"))).Aggregate((a, b) => a + b);
                if (NewCost <= AgreedRepairCost)
                {
                    WriteLog("Стоимость ремонтных работ не превышает согласованную...");
                    return false;
                }
            }
            WriteLog("Требуется провести согласование ремонтных работ...");
            return true;
        }

#endregion

        #region Event Handlers

        #region События пользовательских кнопок
        /// <summary>
        /// Обработчик клика по кнопке, открывающей форму датчиков.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSensors_Click(Object sender, EventArgs e)
        {
            try
            {
                AdditionalWares AWForm = new AdditionalWares(this, UniversalCard, Table_AdditionalWaresList);
                AWForm.ShowDialog();
                if (AWForm.Acceptance)
                {
                    for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                    {
                        Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.DiagnosticsTime] = AWForm.TimeDiagnostics[i];
                        Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationTime] = AWForm.TimeCalibration[i];
                        Table_AdditionalWaresList.RefreshRow(i);
                    }
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Обновить протокол калибровки".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshProtocol_Click(Object sender, EventArgs e)
        {
            try
            {
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.Calibration.CalibrationStartDate);
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;
                
                // Перечень датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);
                // Перечень доп. изделий, для которых нужно сформировать сертификат о калибровке
                List<CardData> AdditionalWaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.AdditionalWaresList);
                
                bool Verify = true;
                // Проверяем возможность обновления "Протокола калировки" для прибора
                if (!(GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    if (!CalibrationDocs.CalibrationProtocol.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                    { Verify = false; }
                }
                // Проверяем возможность обновления "Протокола калировки" для доп. изделий
                foreach (CardData Ware in AdditionalWaresList)
                {
                    if (!CalibrationDocs.CalibrationProtocol.Verify(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService))
                    { Verify = false; }
                }

                if (Verify)
                {
                    // Обновление "Протокола калибровки" для прибора
                    if (!(GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToGuid().Equals(Guid.Empty)) && (!(bool)GetControlValue(RefServiceCard.MainInfo.OnlyA)))
                    {
                        CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToString()));
                        CalibrationDocs.CalibrationProtocol.ReFill(FileCard, CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService);
                    }
                    // Обновление "Протоколов калибровки" для доп. изделий
                    foreach (CardData Ware in AdditionalWaresList)
                    {
                        CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetAdditionalProtocol(Ware.Id)));
                        CalibrationDocs.CalibrationProtocol.ReFill(FileCard, CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService);
                    }
                    CardScript.SaveCard();
                    XtraMessageBox.Show("Данные 'Протокола калибровки' обновлены успешно.");
                }
            }
            
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Обновить сертификат о калибровке".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshCertificate_Click(Object sender, EventArgs e)
        {
            try
            {
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                DateTime StartDateOfCalibration = (DateTime)GetControlValue(RefServiceCard.Calibration.CalibrationStartDate);
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;

                // Перечень датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);
                // Перечень доп. изделий, для которых нужно сформировать сертификат о калибровке
                List<CardData> AdditionalWaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.AdditionalWaresList);

                bool Verify = true;
                // Проверяем возможность обновления "Сертификата о калибровке" для прибора
                if (!(GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    if (!CalibrationDocs.CalibrationCertificate.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                    { Verify = false; }
                }
                // Проверяем возможность обновления "Сертификата о калибровке" для доп. изделий
                foreach (CardData Ware in AdditionalWaresList)
                {
                    if (!CalibrationDocs.CalibrationCertificate.Verify(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService))
                    { Verify = false; }
                }

                if (Verify)
                {
                    // Обновление "Сертификата о калибровке" для прибора
                    if (!(GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                    {
                        CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToString()));
                        CalibrationDocs.CalibrationCertificate.ReFill(FileCard, CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, StartDateOfCalibration);
                    }

                    // Обновление "Сертификата о калибровке" для доп. изделий
                    foreach (CardData Ware in AdditionalWaresList)
                    {
                        CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetAdditionalCertificate(Ware.Id)));
                        CalibrationDocs.CalibrationCertificate.ReFill(FileCard, CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService, StartDateOfCalibration);
                    }
                    CardScript.SaveCard();
                    XtraMessageBox.Show("Данные 'Сертификата о калибровке' обновлены успешно.");
                }
            }
            
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Создать протокол калибровки".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateProtocol_Click(Object sender, EventArgs e)
        {
            try
            {
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;

                // Перечень датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                // Перечень доп. изделий, для которых нужно сформировать сертификат о калибровке
                List<CardData> AdditionalWaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.AdditionalWaresList);
                bool Verify = true;

                // Проверяем возможность создания "Протокола калировки" для прибора
                if ((GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    if (!CalibrationDocs.CalibrationProtocol.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                    { Verify = false; }
                }

                // Проверяем возможность создания "Протокола калировки" для доп. изделий
                foreach (CardData Ware in AdditionalWaresList)
                {
                    if (!CalibrationDocs.CalibrationProtocol.Verify(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService))
                    { Verify = false; }
                }
                if (Verify)
                {
                    // Создание "Протокола калировки" для прибора
                    if ((GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToGuid().Equals(Guid.Empty)) && (!(bool)GetControlValue(RefServiceCard.MainInfo.OnlyA)))
                    {
                        CardData FileCard = CalibrationDocs.CalibrationProtocol.Create(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService);
                        SetControlValue(RefServiceCard.Calibration.CalibrationProtocol, FileCard.Id);
                        //CardScript.CardFrame.CardHost.ShowCard(FileCard.Id, ActivateMode.Edit);
                    }

                    // Создание "Протоколов калибровки" для доп. изделий
                    foreach (CardData Ware in AdditionalWaresList)
                    {
                        CardData FileCard = CalibrationDocs.CalibrationProtocol.Create(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService);
                        SetAdditionalProtocol(Ware.Id, FileCard.Id);
                        //CardScript.CardFrame.CardHost.ShowCard(FileCard.Id, ActivateMode.Edit);
                    }
                    CardScript.SaveCard();
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Создать сертификат о калибровке".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCertificate_Click(Object sender, EventArgs e)
        {
            try
            {
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                DateTime StartDateOfCalibration = (DateTime)GetControlValue(RefServiceCard.Calibration.CalibrationStartDate);
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;

                // Перечень датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);
                // Перечень доп. изделий, для которых нужно сформировать сертификат о калибровке
                List<CardData> AdditionalWaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.AdditionalWaresList);

                bool Verify = true;
                // Проверяем возможность создания "Сертификата о калибровке" для прибора
                if ((GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    if (!CalibrationDocs.CalibrationCertificate.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                    { Verify = false; }
                }

                // Проверяем возможность создания "Сертификата о калибровке" для доп. изделий
                foreach (CardData Ware in AdditionalWaresList)
                {
                    if (!CalibrationDocs.CalibrationCertificate.Verify(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService))
                    { Verify = false; }
                }

                if (Verify)
                {
                    // Создание "Сертификата о калибровке" для прибора
                    if ((GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                    {
                        CardData FileCard = CalibrationDocs.CalibrationCertificate.Create(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, StartDateOfCalibration);
                        SetControlValue(RefServiceCard.Calibration.CalibrationCertificate, FileCard.Id);
                        //CardScript.CardFrame.CardHost.ShowCard(FileCard.Id, ActivateMode.Edit);
                    }

                    // Создание "Сертификатов о калибровке" для доп. изделий
                    foreach (CardData Ware in AdditionalWaresList)
                    {
                        CardData FileCard = CalibrationDocs.CalibrationCertificate.Create(CardScript, Context, Ware, Client, DateTime.Today, null, StartDateOfService, StartDateOfCalibration);
                        SetAdditionalCertificate(Ware.Id, FileCard.Id);
                        //CardScript.CardFrame.CardHost.ShowCard(FileCard.Id, ActivateMode.Edit);
                    }
                    CardScript.SaveCard();
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Обновить протокол поверки".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshVerificationProtocol_Click(Object sender, EventArgs e)
        {
            try
            {
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;

                // Перечень датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                bool Verify = true;
                // Проверяем возможность обновления "Протокола поверки" для прибора
                if (!(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    if (!CalibrationDocs.VerificationProtocol.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                    { Verify = false; }
                }

                if (Verify)
                {
                    // Обновление "Протокола поверки" для прибора
                    if (!(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid().Equals(Guid.Empty)) && (!(bool)GetControlValue(RefServiceCard.MainInfo.OnlyA)))
                    {
                        CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToString()));
                        CalibrationDocs.VerificationProtocol.ReFill(FileCard, CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, false);
                    }
                    
                    CardScript.SaveCard();
                    XtraMessageBox.Show("Данные 'Протокола поверки' обновлены успешно.");
                }
            }

            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Обновить свидетельство о поверке".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshVerificationCertificate_Click(Object sender, EventArgs e)
        {
            try
            {
                if (GetControlValue(RefServiceCard.Calibration.VerificationProtocol) == null || GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid().Equals(Guid.Empty))
                {
                    MyMessageBox.Show("Сформируйте протокол поверки.");
                    return;
                }
                else
                {
                    // Определяем результат поверки (годен/не годен)
                    bool VerifyResult = CalibrationDocs.CalibrationLib.GetVerifyResult(CardScript.Session, CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid()));

                    if (VerifyResult)  // Прибор годен, обновляем Свидетельство о поверке
                    {
                        if (GetControlValue(RefServiceCard.Calibration.VerifySerialNumber) == null || GetControlValue(RefServiceCard.Calibration.VerifySerialNumber).ToString() == "")
                        {
                            MyMessageBox.Show("Укажите серийный номер поверки.");
                            return;
                        }

                        Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                        DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                        bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;
                        string VerifySerialNumber = GetControlValue(RefServiceCard.Calibration.VerifySerialNumber).ToString();


                        // Перечень датчиков
                        List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                        bool Verify = true;
                        // Проверяем возможность обновления "Свидетельства о поверке" для прибора
                        if (!(GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                        {
                            if (!CalibrationDocs.VerificationCertificate.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                            { Verify = false; }
                        }

                        if (Verify)
                        {
                            // Обновление "Свидетельства о поверке" для прибора
                            if (!(GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                            {
                                CardData FileCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToString()));
                                CalibrationDocs.VerificationCertificate.ReFill(FileCard, CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, VerifySerialNumber);
                                // Занесение сформированного документа в протокол поверки
                                string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                string DocumentDate = DocumentDateTime.ToShortDateString();
                                int VerificationInterval = CalibrationLib.GetVerificationInterval(ApplicationCard.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();


                                CardData fileData = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid());
                                RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                VersionedFileCard fileCard = (VersionedFileCard)CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                
                                CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Свидетельство о поверке №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                            }

                            CardScript.SaveCard();
                            XtraMessageBox.Show("Данные 'Свидетельства о поверке' обновлены успешно.");
                        }
                    }
                }        
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Создать протокол поверки".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateVerificationProtocol_Click(Object sender, EventArgs e)
        {
            try
            {
                // Определение идентификатора контрагента
                Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                // Определение даты поступления на поверку
                DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                // Определение метки "Только комплектующие"
                bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                // Определение карточки паспорта прибора
                CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;

                // Определение перечня датчиков
                List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                // Для администратора:
                // Если "Протокол поверки" уже существует и прикреплен к карточке "Паспорта прибора", то предлагаем подгрудить данный "Протокол поверки" в карточку СО (для первичного заполнения базы протоколов поверки). 
                /*if (IsAdmin)
                {
                    // Проверка существования протокола поверки
                    CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(CardScript.Session, CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid()),
                        CalibrationDocs.CalibrationLib.VerificationProtocolCategoryID);
                    if (DocumentCard != null)
                    {
                        switch (MyMessageBox.Show("Для данного прибора уже существует протокол поверки. Обновить дату проведения поверки?", "", MessageBoxButtons.YesNo))
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                DateTime VerificationDate = IsAdmin == true ? (DateTime)GetControlValue(RefServiceCard.Calibration.VerificationStartDate) : DateTime.Today;
                                DateTime ReceiptDate = IsAdmin == true ? (DateTime)GetControlValue(RefServiceCard.Calibration.VerificationStartDate) : DateTime.Today;
                                CalibrationDocs.CalibrationProtocol.ReFill(DocumentCard, CardScript, Context, DeviceCard, Guid.Empty, VerificationDate, SensorsList, ReceiptDate);
                                break;
                            case System.Windows.Forms.DialogResult.No:
                                DateTime VerificationDate2 = (DateTime)DocumentCard.GetDocumentProperty(CalibrationLib.DocumentProperties.StartDate);
                                DateTime ReceiptDate2 = VerificationDate2;
                                break;
                        }
                        SetControlValue(RefServiceCard.Calibration.VerificationProtocol, DocumentCard.Id);
                        return;
                    }
                }*/


                // Проверяем возможность создания "Протокола поверки" для прибора
                bool Verify;
                if ((GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                {
                    DateTime VerificationDate = DateTime.Today;
                    if (CalibrationDocs.VerificationProtocol.Verify(CardScript, Context, DeviceCard, Client, VerificationDate, SensorsList, StartDateOfService))
                    { Verify = true; }
                    else
                    { Verify = false; }
                }
                else
                {
                    if (OnlyAccessories)
                        MyMessageBox.Show("В текущей заявке на сервисное обслуживание не указан прибор.");
                    else
                        MyMessageBox.Show("Протокол поверки уже создан.");
                    Verify = false;
                }

                if (Verify)
                {
                    // Создание "Протокола поверки" для прибора
                    CardData FileCard = CalibrationDocs.VerificationProtocol.Create(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, false);
                    SetControlValue(RefServiceCard.Calibration.VerificationProtocol, FileCard.Id);

                    CardScript.SaveCard();
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик клика по кнопке "Создать сертификат о калибровке".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateVerificationCertificate_Click(Object sender, EventArgs e)
        {
            try
            {
                if (GetControlValue(RefServiceCard.Calibration.VerificationProtocol) == null || GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid().Equals(Guid.Empty))
                {
                    MyMessageBox.Show("Сформируйте протокол поверки.");
                    return;
                }
                else
                {
                    if (IsAdmin)
                    {
                        // Проверка существования протокола поверки
                        CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(CardScript.Session, CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid()),
                            CalibrationDocs.CalibrationLib.VerificationCertificateCategoryID);
                        if (DocumentCard != null)
                        {
                            MyMessageBox.Show("Для данного прибора уже существует свидетельство о поверке.");
                            SetControlValue(RefServiceCard.Calibration.VerificationCertificate, DocumentCard.Id);
                            return;
                        }
                    }

                    // Определяем результат поверки (годен/не годен)
                    bool VerifyResult = CalibrationDocs.CalibrationLib.GetVerifyResult(CardScript.Session, CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid()));

                    if (VerifyResult)  // Прибор годен, формируется Свидетельство о поверке
                    {
                        if (GetControlValue(RefServiceCard.Calibration.VerifySerialNumber) == null || GetControlValue(RefServiceCard.Calibration.VerifySerialNumber).ToString() == "")
                        {
                            MyMessageBox.Show("Укажите серийный номер поверки.");
                            return;
                        }
                        else
                        {
                            Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                            DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.MainInfo.IncommingDate);
                            bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                            CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;
                            string VerifySerialNumber = GetControlValue(RefServiceCard.Calibration.VerifySerialNumber).ToString();

                            // Перечень датчиков
                            List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                            bool Verify = true;
                            // Проверяем возможность создания "Свидетельства о поверке" для прибора
                            if ((GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                            {
                                if (!CalibrationDocs.VerificationCertificate.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                                { Verify = false; }
                            }

                            if (Verify)
                            {
                                // Создание "Свидетельства о поверке" для прибора
                                if ((GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                                {
                                    CardData FileCard = CalibrationDocs.VerificationCertificate.Create(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, VerifySerialNumber);
                                    SetControlValue(RefServiceCard.Calibration.VerificationCertificate, FileCard.Id);

                                    // Занесение сформированного документа в протокол поверки
                                    string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                    DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                    string DocumentDate = DocumentDateTime.ToShortDateString();
                                    int VerificationInterval = CalibrationLib.GetVerificationInterval(ApplicationCard.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                    string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();

                                    CardData fileData = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid());
                                    RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                    VersionedFileCard fileCard = (VersionedFileCard)CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                    fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                    CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Свидетельство о поверке №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                    fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                    System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                                }
                                CardScript.SaveCard();
                            }
                        }
                    }
                    else // Прибор не годен, формируется Извещение о непригодности
                    {
                        // Проверяем возможность создания "Извещения о непригодности" для прибора
                        if (GetControlValue(RefServiceCard.Calibration.CausesOfUnfitness) == null || GetControlValue(RefServiceCard.Calibration.CausesOfUnfitness).ToString() == "")
                        {
                            MyMessageBox.Show("Укажите причины непригодности.");
                            return;
                        }

                        Guid Client = new Guid(GetControlValue(RefServiceCard.MainInfo.Client).ToString());
                        DateTime StartDateOfService = (DateTime)GetControlValue(RefServiceCard.Calibration.VerificationStartDate);
                        bool OnlyAccessories = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        CardData DeviceCard = !OnlyAccessories ? CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString())) : null;
                        string VerifySerialNumber = GetControlValue(RefServiceCard.Calibration.VerifySerialNumber).ToString();
                        string CausesOfUnfitness = GetControlValue(RefServiceCard.Calibration.CausesOfUnfitness).ToString();

                        // Перечень датчиков
                        List<CardData> SensorsList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);
                        
                        bool Verify = true;
                        // Проверяем возможность создания "Извещения о непригодности" для прибора
                        if ((GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                        {
                            if (!CalibrationDocs.NoticeOfUnfitness.Verify(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService))
                            { Verify = false; }
                        }

                        if (Verify)
                        {
                            // Создание "Извещения о непригодности" для прибора
                            if ((GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid().Equals(Guid.Empty)) && (!OnlyAccessories))
                            {
                                CardData FileCard = CalibrationDocs.NoticeOfUnfitness.Create(CardScript, Context, DeviceCard, Client, DateTime.Today, SensorsList, StartDateOfService, VerifySerialNumber, CausesOfUnfitness);
                                SetControlValue(RefServiceCard.Calibration.VerificationCertificate, FileCard.Id);

                                // Занесение сформированного документа в протокол поверки
                                string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                string DocumentDate = DocumentDateTime.ToShortDateString();
                                int VerificationInterval = CalibrationLib.GetVerificationInterval(ApplicationCard.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();

                                CardData fileData = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid());
                                RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                VersionedFileCard fileCard = (VersionedFileCard)CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Извещение о непригодности №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                            }
                            CardScript.SaveCard();
                        }
                    }
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        #endregion

        #region События кнопок на ленте карточки
        /// <summary>
        /// Обработчик события клика по кнопке "Отправить в ремонт".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToAdjustment_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать в ремонт'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);

                switch (CurrentState)
                {
                    #region// Текущее состояние = "На техобслуживании" или "На повторном техобслуживании".
                    case ((int)RefServiceCard.MainInfo.State.Maintenance):
                    case ((int)RefServiceCard.MainInfo.State.Remaintenance):
                        WriteLog("Текущее состояние Наряда на СО = 'На техобслуживании'...");
                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message = CheckActions(DeviceRepair, AccessoriesRepair, Calibration, Verify);
                        if (!String.IsNullOrEmpty(Message))
                        {
                            MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если поле "Внешнее проявление проблемы" не заполнено, то дальнейшие действия прерываются.
                        object Problem = GetControlValue(RefServiceCard.Calibration.Problem);
                        if ((Problem == null) || (Problem.ToString() == ""))
                        {
                            MyMessageBox.Show("Заполните поле \"Внешнее проявление проблемы\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если проставлена галка "Ремонт прибора" и при этом на ремонт пришли только комплектующие, то выводится сообщение об ошибке. 
                        bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (OnlyAccessories == true && DeviceRepair == true)
                        {
                            MyMessageBox.Show("Очистите поле \"Ремонт прибора\" (на сервисное обслуживание поступили только комплектующие).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если проставлена галка "Ремонт прибора" и при этом не указан отдел, ответственный за ремонт прибора, выводится сообщение об ошибке.
                        object Department = GetControlValue(RefServiceCard.Calibration.Department);
                        if ((DeviceRepair) && (Department == null))
                        {
                            MyMessageBox.Show("Укажите в поле \"Ремонт осуществляет\" отдел, который будет осуществлять ремонт прибора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если не проставлены галки "Ремонт прибора" и "Ремонт комплектующих", то выводится сообщение об ошибке.
                        if ((!DeviceRepair) && (!AccessoriesRepair))
                        {
                            MyMessageBox.Show("Для отправки прибора/комплектующих в ремонт через отдел сбыта неоходимо выбрать тип сервиса \"Ремонт прибора\" или \"Ремонт комплектующих\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если на ремонт пришел прибор и при этом не указано фактическое время техобслуживания, то выводится сообщение об ошибке.
                        object DiagnosticsTime = GetControlValue(RefServiceCard.Calibration.DiagnosticsTime);
                        if (!OnlyAccessories && DiagnosticsTime == null)
                        {
                            MyMessageBox.Show("Укажите фактическое время диагностики прибора и комплектующих (кроме датчиков) в поле \"Трудоемкость диагностики\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если на ремонт пришли доп. изделия, и при этом не указано фактическое вреся техобслуживания, то выводится сообщение об ошибке.
                        Message = CheckAdditionalWares(Table_AdditionalWaresList, RefServiceCard.AdditionalWaresList.DiagnosticsTime, this);
                        if (Message != "")
                        {
                            MyMessageBox.Show("Не заполнена трудоемкость диагностики доп. изделий: " + Message + ".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Завершена проверка правильности заполнения полей...");

                        // ========== Определение исполнителя диагностики ==========
                        WriteLog("Начато определение исполнителя диагностики...");
                        StaffEmployee FAManager = null;
                        try { FAManager = Context.GetEmployeeByPosition(RefServiceCard.Roles.AdjastManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.AdjastManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Diagnostics);
                        SetControlValue(RefServiceCard.Calibration.CalibDateEnd, DateTime.Now);
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, Context.GetObjectRef(FAManager).Id);
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticDateStart, DateTime.Now);

                        SendToRepair(CurrentState);

                        CardScript.ChangeState("Diagnostics");

                        /*if (DeviceRepairPerformer != null)
                            SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(DeviceRepairPerformer).Id);
                        if (AccessoriesRepairPerformer != null)
                            SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(AccessoriesRepairPerformer).Id);*/

                        WriteLog("Изменения в текущий Наряд на СО внесены...");

                        MyMessageBox.Show("Передайте прибор и комплектующие в отдел настройки для диагностики.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    #endregion
                    #region// Текущее состояние = "На диагностике".
                    case ((int)RefServiceCard.MainInfo.State.Diagnostics):
                        WriteLog("Текущее состояние Наряда на СО = 'На диагностике'...");
                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify2 = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message2 = CheckActions(DeviceRepair2, AccessoriesRepair2, Calibration2, Verify2);
                        if (!String.IsNullOrEmpty(Message2))
                        {
                            MyMessageBox.Show(Message2, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если проставлена галка "Ремонт прибора" и при этом на ремонт пришли только комплектующие, то выводится сообщение об ошибке. 
                        bool OnlyAccessories2 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (OnlyAccessories2 == true && DeviceRepair2 == true)
                        {
                            MyMessageBox.Show("Очистите поле \"Ремонт прибора\" (на сервисное обслуживание поступили только комплектующие).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если проставлена галка "Ремонт прибора" и при этом не указан отдел, ответственный за ремонт прибора, выводится сообщение об ошибке.
                        object Department2 = GetControlValue(RefServiceCard.Calibration.Department);
                        if ((DeviceRepair2) && (Department2 == null))
                        {
                            MyMessageBox.Show("Укажите в поле \"Ремонт осуществляет\" отдел, который будет осуществлять ремонт прибора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если не проставлены галки "Ремонт прибора" и "Ремонт комплектующих", то выводится сообщение об ошибке.
                        if ((!DeviceRepair2) && (!AccessoriesRepair2))
                        {
                            MyMessageBox.Show("Для отправки прибора/комплектующих в ремонт неоходимо выбрать тип сервиса \"Ремонт прибора\" или \"Ремонт комплектующих\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если в таблице описания неисправностей нет ни одной строки, то выводится сообщение об ошибке.
                        if (Table_Description.RowCount <= 0)
                        {
                            MyMessageBox.Show("В таблице \"Описание неисправностей\" необходимо добавить хотябы одну неисправность.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если трудоемкость диагностики не указана или < 0, то выводится сообщение об ошибке.
                        if ((GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) == null) || ((decimal)GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) <= 0))
                        {
                            MyMessageBox.Show("Заполните поле \"Трудоемкость диагностики\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если для какого-либо сборочного узла не указаны ремонтные работы, выводится сообщение об ошибке.
                        for (int i = 0; i < Table_Description.RowCount; i++)
                        {
                            if ((Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList] == null) || (Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() == ""))
                            {
                                MyMessageBox.Show("Не заполнены ремонтные работы для сборочного узла \"" + Table_Description[i][RefServiceCard.DescriptionOfFault.BlockOfDevice] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        // Если проставлена галка "Аннулировать гарантию" и при этом ремонт не гарантийный, то выводится сообщение об ошибке.
                        bool Warranty = GetControlValue(RefServiceCard.Calibration.WarrantyService) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.WarrantyService);
                        bool VoidWarranty = GetControlValue(RefServiceCard.Adjustment.VoidWarranty) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.VoidWarranty);
                        if ((VoidWarranty) && (!Warranty))
                        {
                            MyMessageBox.Show("Невозможно аннулировать гарантию. Данный прибор не является гарантийным.");
                            return;
                        }
                        // Если проставлена галка "Аннулировать гарантию" или "Аннулировать стоимость ремонта", а причина не указана, то выводится сообщение об ошибке.
                        bool DoubleCost = GetControlValue(RefServiceCard.Adjustment.DoubleCost) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.DoubleCost);
                        string DescriptionOfReason = GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason) == null ? "" : GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason).ToString();
                        if (((VoidWarranty) || (DoubleCost)) && (DescriptionOfReason == ""))
                        {
                            string VoidWarrantyText = VoidWarranty ? "Укажите причину, по которой аннулирована гарантия прибора.\n" : "";
                            string DoubleCostText = DoubleCost ? "Укажите причину, по которой удвоена стоимость ремонта.\n" : "";

                            MyMessageBox.Show(VoidWarrantyText + DoubleCostText);
                            return;
                        }

                        WriteLog("Завершена проверка правильности заполнения полей...");


                        // ========== Определение необходимости согласования ==========
                        WriteLog("Начато определение необходимости согласования...");
                        bool Negotiation = NeedsNegotiation();
                        WriteLog("Завершено определение необходимости согласования...");
                        if (Negotiation)
                        {
                            MyMessageBox.Show("Перед отправкой прибора/комплектующих в ремонт неоходимо провести согласование с отделом сбыта.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителей ремонта ==========
                        WriteLog("Начато определение исполнителей ремонта...");
                        // Определяем, кто выполняет ремонт прибора
                        string DeviceRepairPerformerPosition = "";
                        StaffEmployee DeviceRepairPerformer = null;
                        if (DeviceRepair2)
                        {
                            if ((int)Department2 == 1)
                            { DeviceRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager; }
                            else
                            { DeviceRepairPerformerPosition = RefServiceCard.Roles.AdjastManager; }

                            try { DeviceRepairPerformer = Context.GetEmployeeByPosition(DeviceRepairPerformerPosition); }
                            catch
                            {
                                MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + DeviceRepairPerformerPosition + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            WriteLog("Определен исполнитель ремонта прибора...");
                        }
                        
                        // Определяем, кто выполняет ремонт комплектующих
                        string AccessoriesRepairPerformerPosition = "";
                        StaffEmployee AccessoriesRepairPerformer = null;
                        if (AccessoriesRepair2)
                        {
                            AccessoriesRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager;
                            try { AccessoriesRepairPerformer = Context.GetEmployeeByPosition(AccessoriesRepairPerformerPosition); }
                            catch
                            {
                                MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.ProdactionManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            WriteLog("Определен исполнитель ремонта комплектующих...");
                        }

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Adjustment);
                        if (DeviceRepair2)
                        {
                            SetControlValue(RefServiceCard.Adjustment.RepDateStart, DateTime.Now);
                            SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(DeviceRepairPerformer).Id);
                        }
                        // 
                        if (AccessoriesRepair2)
                        {
                            SetControlValue(RefServiceCard.Adjustment.AccDateStart, DateTime.Now);
                            SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(AccessoriesRepairPerformer).Id);
                        }
                        // Аннулирование гарантии в паспорте прибора
                        if (Warranty)
                        {
                            CardData Passport = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid());
                            if (VoidWarranty)
                                Passport.Sections[Passport.Type.AllSections["Properties"].Id].FindRow("@Name = 'Гарантия аннулирована'").SetBoolean("Value", true);
                            else
                                Passport.Sections[Passport.Type.AllSections["Properties"].Id].FindRow("@Name = 'Гарантийный ремонт'").SetBoolean("Value", true);
                        }

                        SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, DateTime.Now);
                        CardScript.ChangeState("Adjustment");

                        WriteLog("Изменения в текущий Наряд на СО внесены...");

                        MyMessageBox.Show("Наряд на сервисное обслуживание переведен в состояние 'В ремонте'.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        #endregion
                }

                WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards(); 

                /*object Problem = GetControlValue(RefServiceCard.Calibration.Problem);
                bool OA = (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                object Improvements = GetControlValue(RefServiceCard.Calibration.Improvements);
                string ReqTypeService = GetControlValue(RefServiceCard.Calibration.ReqTypeService).ToString();
                object Department = GetControlValue(RefServiceCard.Calibration.Department);
                bool Calibration = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                bool Verify = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                object DiagnosticsTime = GetControlValue(RefServiceCard.Calibration.DiagnosticsTime);

                // Проверка
                if ((Problem == null) || (Problem.ToString() == ""))
                {
                    MyMessageBox.Show("Заполните поле \"Внешнее проявление проблемы\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                String Message = CheckActions(DeviceRepair, AccessoriesRepair, Calibration, Verify);
                if (!String.IsNullOrEmpty(Message))
                {
                    MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ReqTypeService.IndexOf("поверка") >= 0 && Verify == false)
                {
                    MyMessageBox.Show("Заполните поле \"Поверка\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ReqTypeService.IndexOf("поверка") < 0 && Verify == true)
                {
                    MyMessageBox.Show("Очистите поле \"Поверка\" (данный вид сервиса не заказан клиентом).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (OA == true && DeviceRepair == true)
                {
                    MyMessageBox.Show("Очистите поле \"Ремонт прибора\" (на сервисное обслуживание поступили только комплектующие).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if ((DeviceRepair) && (Department == null))
                {
                    MyMessageBox.Show("Укажите в поле \"Ремонт осуществляет\" отдел, который будет осуществлять ремонт прибора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((!DeviceRepair) && (!AccessoriesRepair))
                {
                    MyMessageBox.Show("Для отправки прибора/комплектующих в ремонт через отдел сбыта неоходимо выбрать тип сервиса \"Ремонт прибора\" или \"Ремонт комплектующих\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (OA != true && DiagnosticsTime == null)
                {
                    MyMessageBox.Show("Укажите фактическое время диагностики прибора и комплектующих (кроме датчиков) в поле \"Трудоемкость диагностики\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Message = CheckAdditionalWares(Table_AdditionalWaresList, RefServiceCard.AdditionalWaresList.DiagnosticsTime, this);
                WriteLog("Проверили датчики");
                if (Message != "")
                {
                    MyMessageBox.Show("Не заполнена трудоемкость диагностики доп. изделий: " + Message + ".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string        DeviceRepairPerformerPosition      = "";
                string        AccessoriesRepairPerformerPosition = "";
                StaffEmployee DeviceRepairPerformer              = null;
                StaffEmployee AccessoriesRepairPerformer         = null;
                StaffEmployee FAManager                          = null;

                // Определяем, кто выполняет диагностику
                try { FAManager = Context.GetEmployeeByPosition(RefServiceCard.Roles.AdjastManager); }
                catch
                {
                    MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.AdjastManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Определяем, кто выполняет ремонт прибора
                if (DeviceRepair)
                {
                    if ((int)Department == 1)
                    { DeviceRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager; }
                    else
                    { DeviceRepairPerformerPosition = RefServiceCard.Roles.AdjastManager; }

                    try { DeviceRepairPerformer = Context.GetEmployeeByPosition(DeviceRepairPerformerPosition); }
                    catch
                    {
                        MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + DeviceRepairPerformerPosition + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                // Определяем, кто выполняет ремонт комплектующих
                if (AccessoriesRepair)
                {
                    AccessoriesRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager;
                    try { AccessoriesRepairPerformer = Context.GetEmployeeByPosition(AccessoriesRepairPerformerPosition); }
                    catch
                    {
                        MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.ProdactionManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                WriteLog("Проверки пройдены успешно");
                string ProcessID = CardScript.CardData.Sections[CardScript.CardData.Type.Sections["Processes"].Id].FirstRow.GetString("ProcessID"); //получаем связанный БП
                CardData cProcess = CardScript.Session.CardManager.GetCardData(new Guid(ProcessID));
                SectionData Variables = cProcess.Sections[cProcess.Type.Sections["Variables"].Id];
                WriteLog("Получили БП");
                int State = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                string TaskID = "";
                WriteLog("Получили статус");
                if (State == (int)RefServiceCard.MainInfo.State.Calibration)      //для состояния "На калибровке"
                {
                    WriteLog("Получаем задание на калибровку");
                    TaskID = Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CalibrationTask + "'").GetString("Value").ToString(); //Получаем связанное задание
                }
                if (State == (int)RefServiceCard.MainInfo.State.Remaintenance)      //для состояния "На повторной калибровке"
                {
                    WriteLog("Получаем задание на повторную калибровку");
                    TaskID = Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.RecalibrationTask + "'").GetString("Value").ToString(); //Получаем связанное задание
                }
                WriteLog("Получили связанное задание");

                CardData cTask = CardScript.Session.CardManager.GetCardData(new Guid(TaskID));

                Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.State + "'").SetString("Value", "На диагностике"); //переносим состояние в бп
                Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.ExternalManifestationOfProblem + "'").SetString("Value", Problem.ToString());
                Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.Improvements + "'").SetString("Value", GetControlValue(RefServiceCard.Calibration.Improvements));
                Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.AdjustManager + "'").SetString("Value", null);

                if (DeviceRepairPerformer != null)
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.AdjustManager + "'").SetString("Value", Context.GetObjectRef(DeviceRepairPerformer).Id.ToString());
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.DeviceRepair + "'").SetBoolean("Value", true);
                }
                else
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.AdjustManager + "'").SetString("Value", null);
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.DeviceRepair + "'").SetBoolean("Value", false);
                }
                if (AccessoriesRepairPerformer != null)
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepairPerformer + "'").SetString("Value", Context.GetObjectRef(AccessoriesRepairPerformer).Id.ToString());
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepair + "'").SetBoolean("Value", true);
                }
                else
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepairPerformer + "'").SetString("Value", null);
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepair + "'").SetBoolean("Value", false);
                }

                Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.FactTypeServiseList + "'").SetString("Value", FormattingService(DeviceRepair, AccessoriesRepair, Calibration, Verify));
                Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.Calibrator + "'").SetString("Value", Context.GetObjectRef(FAManager).Id.ToString());
                WriteLog("Передали данные в БП");

                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Diagnostics); //Заполняем реквизиты карточки
                SetControlValue(RefServiceCard.Calibration.CalibDateEnd, DateTime.Now);

                if (DeviceRepairPerformer != null)
                    SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(DeviceRepairPerformer).Id);
                if (AccessoriesRepairPerformer != null)
                    SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(AccessoriesRepairPerformer).Id);

                CardScript.ChangeState("Diagnostics");  //смена состояния карточки СО	
                if (cTask.LockStatus != LockStatus.Free)
                {
                    cTask.ForceUnlock();
                    cTask.EndUpdate();
                }
                WriteLog("Заполнили реквизиты карточки");

                cTask.PlaceLock();
                if (cTask.InUpdate == false)
                { cTask.BeginUpdate(); }
                cTask.Sections[cTask.Type.Sections["Performing"].Id].FirstRow.SetInt32("TaskState", 5); //смена статуса задания
                WriteLog("Сменили состояние задания");

                //Перенос ярлыка в папку "Завершенные"
                StaffEmployee Emp = Context.GetCurrentEmployee();
                //FoldersFolder PersonalFolder = Emp.PersonalFolder;
                CardData StaffDictionaryCard = CardScript.Session.CardManager.GetCardData(DocsVision.Platform.Cards.Constants.RefStaff.ID);
                SectionData EmployeesSection = StaffDictionaryCard.Sections[DocsVision.Platform.Cards.Constants.RefStaff.Employees.ID];

                RowData EmployeeRow = EmployeesSection.GetRow(Context.GetObjectRef(Emp).Id);
                Guid PersonalFolderId = EmployeeRow.GetGuid(DocsVision.Platform.Cards.Constants.RefStaff.Employees.PersonalFolder).ToGuid();
                Folder PF = FolderCard.GetFolder(PersonalFolderId);
                Folder Completed = null;
                if (PF.Folders["Завершенные"] == null)
                { Completed = PF.Folders.AddNew("Завершенные"); }
                else
                { Completed = PF.Folders["Завершенные"]; }
                DocsVision.Platform.ObjectManager.SystemCards.ShortcutCollection MyShortcuts = FolderCard.GetShortcuts(cTask.Id);
                if (MyShortcuts.Count > 0)
                {
                    foreach (DocsVision.Platform.ObjectManager.SystemCards.Shortcut MyShortcut in MyShortcuts)
                    {
                        try {
                                if (FolderCard.GetFolder(MyShortcut.FolderId).Name != "Последние")
                                { MyShortcut.Move(Completed.Id); }
                            }
                        catch { }
                    }
                }
                WriteLog("Перенесли ярлык");

                cTask.EndUpdate();
                cTask.RemoveLock();
                MyMessageBox.Show("Передайте прибор и комплектующие в отдел настройки для диагностики.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();*/
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Отправить на калибровку".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToCalibrate_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать в на калибровку'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                switch (CurrentState)
                {
                    #region // Текущее состояние = "На техобслуживании"
                    case (int)RefServiceCard.MainInfo.State.Maintenance:
                        WriteLog("Текущее состояние наряда на СО = 'На техобслуживании'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message = CheckActions(DeviceRepair, AccessoriesRepair, Calibration, Verify);
                        if (!String.IsNullOrEmpty(Message))
                        {
                            MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //Если поле "Внешнее проявление проблемы" не заполнено, то дальнейшие действия прерываются.
                        object Problem = GetControlValue(RefServiceCard.Calibration.Problem);
                        if ((Problem == null) || (Problem.ToString() == ""))
                        {
                            MyMessageBox.Show("Заполните поле \"Внешнее проявление проблемы\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если в группе полей "Требуется вид сервиса" указан ремонт приборов и/или комплектующих, то дальнейшие действия прерываются.
                        string Improvements = GetControlValue(RefServiceCard.Calibration.Improvements) == null ? "" : GetControlValue(RefServiceCard.Calibration.Improvements).ToString();
                        if ((DeviceRepair) || (AccessoriesRepair))   //Выходим, если требуется ремонт
                        {
                            MyMessageBox.Show("Вы не можете передать на калибровку прибор, которыму требуется ремонт. Передайте прибор/комплектующие на ремонт через отдел сбыта или измените требуемый вид сервиса.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // Если в поле "Необходимые доработки" указан текст, то дальнейшие действия прерываются.
                            if (Improvements != "")
                            {
                                MyMessageBox.Show("Вы не можете передать на калибровку данный прибор. Для него требуется выполнить доработки, описанные в поле \"Необходимые доработки\". Передайте прибор на диагностику.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Если в поле "Требуется вид сервиса" не указана калибровка, то дальнейшие действия прерываются. 
                        if (!Calibration)
                        {
                            MyMessageBox.Show("Для данного прибора не указан требуемый вид сервиса 'Калибровка'", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителей калибровки ==========
                        WriteLog("Начато определение исполнителей калибровки...");
                        StaffEmployee CalibrationPerformer = null;
                        try
                        {
                            CalibrationPerformer = Context.GetEmployeeByPosition(RefServiceCard.Roles.MetrologicalLabManager);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.MetrologicalLabManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель калибровки определен: " + CalibrationPerformer.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.Calibration.CalibDateEnd, DateTime.Now);

                        WriteLog("Установлена дата завершения техобслуживания...");
                        SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, DateTime.Now);
                        SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, Context.GetObjectRef(CalibrationPerformer).Id);
                        WriteLog("Установлены параметры исполнения калибровки...");

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Calibration);
                        CardScript.ChangeState("Calibration");
                        WriteLog("Текущий Наряд на СО переведен в состояние 'На калибровке'...");

                        // ========== Запуск метода отправки в калибровку ==========
                        WriteLog("Производится запуск метода отправки Наряда на СО на калибровку...");


                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                    #region // Текущее состояние = "На повторном техобслуживании"
                    case (int)RefServiceCard.MainInfo.State.Remaintenance:
                        WriteLog("Текущее состояние наряда на СО = 'На повторном техобслуживании'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify2 = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message2 = CheckActions(DeviceRepair2, AccessoriesRepair2, Calibration2, Verify2);
                        if (!String.IsNullOrEmpty(Message2))
                        {
                            MyMessageBox.Show(Message2, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если не заполнено фактическое время, затраченное на техобслуживание прибора, то дальнейшие действия прерываются.
                        object CalibrationTime = GetControlValue(RefServiceCard.Calibration.CalibrationTime);
                        bool OnlyAccessories2 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (!OnlyAccessories2)
                        {
                            if ((CalibrationTime == null) || ((decimal)CalibrationTime == 0))
                            {
                                MyMessageBox.Show("Укажите фактическое время техобслуживания прибора и комплектующих после ремонта (кроме датчиков) в поле \"Трудоемкость калибровки\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        // Если не заполнено фактическое время, затраченное на техобслуживание доп. изделий, то дальнейшие действия прерываются.
                        Message2 = CheckAdditionalWares(Table_AdditionalWaresList, RefServiceCard.AdditionalWaresList.CalibrationTime, this);
                        if (Message2 != "")
                        {
                            MyMessageBox.Show("Не заполнена трудоемкость техобслуживания после ремонта доп. изделий: " + Message2 + ".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если в поле "Требуется вид сервиса" указана калибровка, и в текущем Наряде на СО имеется прибор, то дальнейшие действия прерываются. 
                        if (!Calibration2)
                        {
                            MyMessageBox.Show("Для данного прибора не указан требуемый вид сервиса 'Калибровка'", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителей калибровки ==========
                        WriteLog("Начато определение исполнителей калибровки...");
                        StaffEmployee CalibrationPerformer2 = null;
                        try
                        {
                            CalibrationPerformer2 = Context.GetEmployeeByPosition(RefServiceCard.Roles.MetrologicalLabManager);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.MetrologicalLabManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель калибровки определен: " + CalibrationPerformer2.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.Calibration.CalibDateEnd, DateTime.Now);

                        WriteLog("Установлена дата завершения техобслуживания...");
                        SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, DateTime.Now);
                        SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, Context.GetObjectRef(CalibrationPerformer2).Id);
                        WriteLog("Установлены параметры исполнения калибровки...");

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Calibration);
                        CardScript.ChangeState("Calibration");
                        WriteLog("Текущий Наряд на СО переведен в состояние 'На калибровке'...");

                        // ========== Запуск метода отправки в калибровку ==========
                        WriteLog("Производится запуск метода отправки Наряда на СО в калибровку...");
                       

                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                    #region// Текущее состояние = "Ожидание оплаты" или "Завершено"
                    case (int)RefServiceCard.MainInfo.State.Payment:
                    case (int)RefServiceCard.MainInfo.State.Completed:
                        WriteLog("Текущее состояние наряда на СО = 'Ожидание оплаты' или 'Завершено'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration3 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify3 = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair3 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair3 = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message3 = CheckActions(DeviceRepair3, AccessoriesRepair3, Calibration3, Verify3);
                        if (!String.IsNullOrEmpty(Message3))
                        {
                            MyMessageBox.Show(Message3, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если в поле "Требуется вид сервиса" не указана калибровка, то дальнейшие действия прерываются. 
                        if (!Calibration3)
                        {
                            MyMessageBox.Show("Для данного прибора не указан требуемый вид сервиса 'Калибровка'", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителей калибровки ==========
                        WriteLog("Начато определение исполнителей калибровки...");
                        StaffEmployee CalibrationPerformer3 = null;
                        try
                        {
                            CalibrationPerformer3 = Context.GetEmployeeByPosition(RefServiceCard.Roles.MetrologicalLabManager);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.MetrologicalLabManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель калибровки определен: " + CalibrationPerformer3.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                       
                        SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, DateTime.Now);
                        SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, Context.GetObjectRef(CalibrationPerformer3).Id);
                        WriteLog("Установлены параметры исполнения калибровки...");

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Calibration);
                        CardScript.ChangeState("Calibration");
                        WriteLog("Текущий Наряд на СО переведен в состояние 'На калибровке'...");

                        // ========== Запуск метода отправки в калибровку ==========
                        WriteLog("Производится запуск метода отправки Наряда на СО на калибровку...");


                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                }

                WriteLog("Завершена обработка кнопки 'Передать на калибровку'...");
            }
            /*
            try
            {
                // Проверка правильности заполнения данных
                WriteLog("Отправка на калибровку. Проверка правильности заполнения данных.");
                
                if (!IsAdmin)
                {
                    if (Table_Description.RowCount <= 0)
                    {
                        MyMessageBox.Show("В таблице \"Описание неисправностей\" необходимо добавить хотябы одну неисправность.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < Table_Description.RowCount; i++)
                    {
                        if ((Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList] == null) || (Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() == ""))
                        {
                            MyMessageBox.Show("Не заполнены ремонтные работы для сборочного узла \"" + Table_Description[i][RefServiceCard.DescriptionOfFault.BlockOfDevice] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                    {
                        if (Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate].IsNull())
                        {
                            MyMessageBox.Show("Не заполнена фактическая дата окончания ремонтной работы \"" + Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksType] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                // Поиск незаполненных полей в таблице "Ремонтные работы".
                ArrayList PropertyList = new ArrayList();
                for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                {
                    if (Table_RepairWorks[i][RefServiceCard.RepairWorks.NegotiationResult].ToString() != "Не выполнять")
                    {
                        if (Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] == null || (decimal)Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] <= 0)
                        {
                            PropertyList.Add("Трудоемкость");
                            break;
                        }

                        if (Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer] == null || Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer].ToString() == "")
                        {
                            PropertyList.Add("Исполнитель");
                            break;
                        }

                        if ((Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate] == null) || (((DateTime)Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate]).Equals(DateTime.MinValue)))
                        {
                            PropertyList.Add("Дата завершения");
                            break;
                        }
                    }
                }
                if (!IsAdmin)
                {
                    if (PropertyList.Count > 0)
                    {
                        string ErrorText = PropertyList.ToArray().Select(r => "- \"" + r.ToString() + "\"").Aggregate((currentValue, nextValue) => currentValue + ";\n" + nextValue) + ".";
                        //for (int j = 0; j < PropertyList.Count - 1; j++)
                        //{
                        //    Text = Text + "- \"" + PropertyList[j] + "\";\n";
                        //}
                        //Text = Text + "- \"" + PropertyList[PropertyList.Count - 1] + "\".";

                        MyMessageBox.Show("Для отправки на калибровку необходимо заполнить следующие поля:\n" + ErrorText, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                WriteLog("Отправка на калибровку. Проверяем исполнителей.");
                Guid DeviceRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Adjuster) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Adjuster).ToGuid();
                Guid AccessoriesRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Worker) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Worker).ToGuid();
                if ((!DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                    (!AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                    (!IsAdmin))
                {
                    MyMessageBox.Show("Вы не являетесь исполнителем ремонта прибора или комплектующих. Отправка на калибровку невозможна.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                WriteLog("Отправка на калибровку. Проверяем получателя");
                StaffEmployee Calibrator = null;
                try
                {
                    Calibrator = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager);
                }
                catch
                {
                    MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                WriteLog("Отмечаем выполненные доработки");
                bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                if (!OnlyAccessories)
                {
                    CardData Passport = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid());
                    if (Passport.LockStatus != LockStatus.Free)
                        Passport.ForceUnlock();
                    WriteLog("Получили пасспорт прибора");
                    SectionData Properties = Passport.Sections[Passport.Type.AllSections["Properties"].Id];
                    SubSectionData ParentDocuments = Properties.FindRow("@Name = 'Документ'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                    SubSectionData Completed = Properties.FindRow("@Name = 'Выполнено'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                    SubSectionData CompleteDate = Properties.FindRow("@Name = 'Дата выполнения'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                    WriteLog("Получили свойства паспорта");
                    WriteLog("Проверям перечень ремонтных работ...");
                    for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                    {
                        WriteLog(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksType].ToString());
                        if (Table_RepairWorks[i][RefServiceCard.RepairWorks.NegotiationResult].ToString() != "Не выполнять")
                        {
                            if (((decimal)Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] > 0) && ((bool)Table_RepairWorks[i][RefServiceCard.RepairWorks.Revision]))
                            {
                                WriteLog("Ставим отметку о выполнении.");
                                Guid ParentDocument = UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Документ") == null ? Guid.Empty :
                                    UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Документ").ToGuid();
                                WriteLog("Получили родительское Разрешение.");
                                if (!ParentDocument.Equals(Guid.Empty))
                                {
                                    WriteLog("Родительское Разрешение существует.");
                                    //RowData RowParentDocument = ParentDocuments.Rows.First(row => row.GetGuid("SelectedValue").ToGuid().Equals(ParentDocument));
                                    RowData RowParentDocument = ParentDocuments.Rows.Find("SelectedValue", ParentDocument);
                                    WriteLog("Произвели поиск доработки в паспорте прибора.");
                                    if (RowParentDocument != null)
                                    {
                                        WriteLog("Доработка существует.");
                                        int Order = (int)RowParentDocument.GetInt32("Order");
                                        RowData RowCompleted = Completed.FindRow("@Order = '" + Order + "'");
                                        RowData RowCompleteDate = CompleteDate.FindRow("@Order = '" + Order + "'");
                                        RowCompleted.SetBoolean("SelectedValue", true);
                                        RowCompleteDate.SetDateTime("SelectedValue", DateTime.Today);
                                        WriteLog("Внесли отметку об исполнении.");
                                    }
                                    else
                                    {
                                        WriteLog("Такой доработки в паспорте не существует.");
                                    }
                                }
                            }

                            if (Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer] == null || Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer].ToString() == "")
                            {
                                PropertyList.Add("Исполнитель");
                                break;
                            }

                            if (Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate] == null)
                            {
                                PropertyList.Add("Дата завершения");
                                break;
                            }
                        }
                    }
                }

                
                WriteLog("Отправка на калибровку. Получаем БП.");
                string ProcessID = CardScript.CardData.Sections[CardScript.CardData.Type.Sections["Processes"].Id].FirstRow.GetString("ProcessID");  //получаем связанный БП
                CardData cProcess = CardScript.Session.CardManager.GetCardData(new Guid(ProcessID));
                SectionData Variables = cProcess.Sections[cProcess.Type.Sections["Variables"].Id];
                string TaskID = "";
                WriteLog("Отправка на калибровку. Получаем свойства карточки.");
                //DateTime RepDateStart = DeviceRepairPerformerID.Equals(Guid.Empty) ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.RepDateStart);
                DateTime? RepDateStart = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.RepDateStart));
                //DateTime AccDateStart = AccessoriesRepairPerformerID.Equals(Guid.Empty) ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.AccDateStart);
                DateTime? AccDateStart = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.AccDateStart));
                //DateTime RepDateEnd = GetControlValue(RefServiceCard.Adjustment.RepDateEnd) == null ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.RepDateEnd);
                DateTime? RepDateEnd = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.RepDateEnd));
                //DateTime AccDateEnd = GetControlValue(RefServiceCard.Adjustment.AccDateEnd) == null ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.AccDateEnd);
                DateTime? AccDateEnd = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.AccDateEnd));

                WriteLog("Отправка на калибровку. Получаем задание.");
                if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) || 
                    (!DeviceRepairPerformerID.Equals(Guid.Empty) && IsAdmin))
                {
                    if ((!RepDateEnd.IsNull()) && (!RepDateStart.IsNull()) && (RepDateEnd >= RepDateStart))
                    {
                        MyMessageBox.Show("Прибор уже передан на повторную калибровку.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) == null)
                    {
                        MyMessageBox.Show("Укажите трудоемкость диагностики неисправности прибора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    WriteLog("Отправка на калибровку. Получаем задание на ремонт прибора.");
                    TaskID = Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.RepairTask + "'").GetString("Value").ToString(); //Получаем связанное задание
                    WriteLog("Отправка на калибровку. Закрываем задание на ремонт прибора.");
                    TaskClose(TaskID, RefServiceCard.Adjustment.RepDateEnd);
                    RepDateEnd = DateTime.Now;
                }

                if (AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) ||
                    (!AccessoriesRepairPerformerID.Equals(Guid.Empty) && IsAdmin))
                {
                    if ((!AccDateEnd.IsNull()) && (!AccDateStart.IsNull()) && (AccDateEnd >= AccDateStart))
                    {
                        MyMessageBox.Show("Комплектующие уже переданы на повторную калибровку.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    WriteLog("Отправка на калибровку. Получаем задание на ремонт комплектующих.");
                    TaskID = Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepairTask + "'").GetString("Value").ToString(); //Получаем связанное задание
                    WriteLog("Отправка на калибровку. Закрываем задание на ремонт комплектующих.");
                    TaskClose(TaskID, RefServiceCard.Adjustment.AccDateEnd);
                    AccDateEnd = DateTime.Now;
                }
                WriteLog("Отправка на калибровку. Определяем, завершен ли ремонт");

                //RepDateEnd = GetControlValue(RefServiceCard.Adjustment.RepDateEnd) == null ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.RepDateEnd);
                //AccDateEnd = GetControlValue(RefServiceCard.Adjustment.AccDateEnd) == null ? new DateTime() : (DateTime)GetControlValue(RefServiceCard.Adjustment.AccDateEnd);
                if (((!RepDateStart.IsNull()) && (RepDateEnd.IsNull() || RepDateEnd < RepDateStart)) ||
                    ((!AccDateStart.IsNull()) && (AccDateEnd.IsNull() || AccDateEnd < AccDateStart)))
                {
                    WriteLog("наряд остается в ремонте");
                }
                else
                {
                    WriteLog("наряд отправляется на калибровку");
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.State + "'").SetString("Value", "На повторной калибровке"); //переносим состояние в бп
                    Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.Calibrator + "'").SetString("Value", Context.GetObjectRef(Calibrator).Id.ToString());

                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance); //Заполняем реквизиты карточки
                    //SetControlValue(RefServiceCard.Calibration.CalDateStart, DateTime.Now);
                    CardScript.ChangeState("Recalibration");//смена состояния карточки СО
                }

                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();
            }*/
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик кнопки "Отправить на согласование".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToAgreement_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать на согласование'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);

                // ========== Проверка заполнения полей Наряда на СО ==========
                WriteLog("Начата проверка правильности заполнения полей...");
                //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                bool Calibration2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                bool Verify2 = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                bool DeviceRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                bool AccessoriesRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                String Message2 = CheckActions(DeviceRepair2, AccessoriesRepair2, Calibration2, Verify2);
                if (!String.IsNullOrEmpty(Message2))
                {
                    MyMessageBox.Show(Message2, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если проставлена галка "Ремонт прибора" и при этом на ремонт пришли только комплектующие, то выводится сообщение об ошибке. 
                bool OnlyAccessories2 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                if (OnlyAccessories2 == true && DeviceRepair2 == true)
                {
                    MyMessageBox.Show("Очистите поле \"Ремонт прибора\" (на сервисное обслуживание поступили только комплектующие).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если проставлена галка "Ремонт прибора" и при этом не указан отдел, ответственный за ремонт прибора, выводится сообщение об ошибке.
                object Department2 = GetControlValue(RefServiceCard.Calibration.Department);
                if ((DeviceRepair2) && (Department2 == null))
                {
                    MyMessageBox.Show("Укажите в поле \"Ремонт осуществляет\" отдел, который будет осуществлять ремонт прибора.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если не проставлены галки "Ремонт прибора" и "Ремонт комплектующих", то выводится сообщение об ошибке.
                if ((!DeviceRepair2) && (!AccessoriesRepair2))
                {
                    MyMessageBox.Show("Для отправки прибора/комплектующих в ремонт неоходимо выбрать тип сервиса \"Ремонт прибора\" или \"Ремонт комплектующих\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если в таблице описания неисправностей нет ни одной строки, то выводится сообщение об ошибке.
                if (Table_Description.RowCount <= 0)
                {
                    MyMessageBox.Show("В таблице \"Описание неисправностей\" необходимо добавить хотябы одну неисправность.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если трудоемкость диагностики не указана или < 0, то выводится сообщение об ошибке.
                if ((GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) == null) || ((decimal)GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) <= 0))
                {
                    MyMessageBox.Show("Заполните поле \"Трудоемкость диагностики\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если для какого-либо сборочного узла не указаны ремонтные работы, выводится сообщение об ошибке.
                for (int i = 0; i < Table_Description.RowCount; i++)
                {
                    if ((Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList] == null) || (Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() == ""))
                    {
                        MyMessageBox.Show("Не заполнены ремонтные работы для сборочного узла \"" + Table_Description[i][RefServiceCard.DescriptionOfFault.BlockOfDevice] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                // Если проставлена галка "Аннулировать гарантию" и при этом ремонт не гарантийный, то выводится сообщение об ошибке.
                bool Warranty = GetControlValue(RefServiceCard.Calibration.WarrantyService) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.WarrantyService);
                bool VoidWarranty = GetControlValue(RefServiceCard.Adjustment.VoidWarranty) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.VoidWarranty);
                if ((VoidWarranty) && (!Warranty))
                {
                    MyMessageBox.Show("Невозможно аннулировать гарантию. Данный прибор не является гарантийным.");
                    return;
                }
                // Если проставлена галка "Аннулировать гарантию" или "Аннулировать стоимость ремонта", а причина не указана, то выводится сообщение об ошибке.
                bool DoubleCost = GetControlValue(RefServiceCard.Adjustment.DoubleCost) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.DoubleCost);
                string DescriptionOfReason = GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason) == null ? "" : GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason).ToString();
                if (((VoidWarranty) || (DoubleCost)) && (DescriptionOfReason == ""))
                {
                    string VoidWarrantyText = VoidWarranty ? "Укажите причину, по которой аннулирована гарантия прибора.\n" : "";
                    string DoubleCostText = DoubleCost ? "Укажите причину, по которой удвоена стоимость ремонта.\n" : "";

                    MyMessageBox.Show(VoidWarrantyText + DoubleCostText);
                    return;
                }
                WriteLog("Завершена проверка правильности заполнения полей...");

                // ========== Определение необходимости согласования ==========
                WriteLog("Начато определение необходимости согласования...");
                bool Negotiation = NeedsNegotiation();
                WriteLog("Завершено определение необходимости согласования...");
                if (!Negotiation)
                {
                    if (MyMessageBox.Show("Согласование с отделом сбыта не требуется. Все равно отправить на согласование?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }

                // ========== Определение исполнителей согласования ==========
                WriteLog("Начато определение исполнителей согласования...");
                StaffEmployee Manager = null;
                try
                {
                    Manager = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup);
                }
                catch
                {
                    MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                WriteLog("Исполнитель согласования определен: " + Manager.DisplayName + "...");

                // ========== Внесение изменений в текущий Наряд на СО ==========
                WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager).Id);
                switch (CurrentState)
                {
                    case (int)RefServiceCard.MainInfo.State.Diagnostics:
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, DateTime.Now);
                        break;
                    case (int)RefServiceCard.MainInfo.State.Adjustment:
                        if (DeviceRepair2)
                            SetControlValue(RefServiceCard.Adjustment.RepDateEnd, DateTime.Now);
                        if (AccessoriesRepair2)
                            SetControlValue(RefServiceCard.Adjustment.AccDateEnd, DateTime.Now);
                        break;
                }
                // Аннулирование гарантии в паспорте прибора
                if (VoidWarranty)
                {
                    CardData Passport = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid());
                    Passport.Sections[Passport.Type.AllSections["Properties"].Id].FindRow("@Name = 'Гарантия аннулирована'").SetBoolean("Value", true);
                }

                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Consensus); //Заполняем реквизиты карточки
                CardScript.ChangeState("Consensus"); //смена состояния карточки СО
                WriteLog("Изменения в текущий Наряд на СО внесены...");

                WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();

                /*WriteLog("Отправка на согласование. Начинаем проверку");
                if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                {
                    if (Table_Description.RowCount <= 0)
                    {
                        MyMessageBox.Show("В таблице \"Описание неисправностей\" необходимо добавить хотябы одну неисправность.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Проверка
                    if ((GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) == null) || ((decimal)GetControlValue(RefServiceCard.Adjustment.LaboriousnessDiagnostics) <= 0))
                    {
                        MyMessageBox.Show("Заполните поле \"Трудоемкость диагностики\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    for (int i = 0; i < Table_Description.RowCount; i++)
                    {
                        if ((Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList] == null) || (Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() == ""))
                        {
                            MyMessageBox.Show("Не заполнены ремонтные работы для сборочного узла \"" + Table_Description[i][RefServiceCard.DescriptionOfFault.BlockOfDevice] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                // Определяем, требуется ли согласование с отделом сбыта
                WriteLog("Определяем, требуется ли согласование с отделом сбыта");
                bool NeedsNegotiation = false;
                for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                {
                    WriteLog(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksType].ToString());
                    if ((bool)Table_RepairWorks[i][RefServiceCard.RepairWorks.Revision] == true)
                    {
                        WriteLog("Доработка");

                        bool MandatoryRevision = UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Обязательная доработка") == null ? false :
                            (bool)UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Обязательная доработка");
                        WriteLog("Получили свойство Обязательная доработка");
                        if (!MandatoryRevision)
                        { NeedsNegotiation = true; }
                        WriteLog("Оценили необходимость согласования");
                    }
                    else
                    { NeedsNegotiation = true; }
                }
                WriteLog("Закончили оценку необходимости согласования");

                bool Warranty = GetControlValue(RefServiceCard.Calibration.WarrantyService) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.WarrantyService);
                bool VoidWarranty = GetControlValue(RefServiceCard.Adjustment.VoidWarranty) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.VoidWarranty);
                bool DoubleCost = GetControlValue(RefServiceCard.Adjustment.DoubleCost) == null ? false : (bool)GetControlValue(RefServiceCard.Adjustment.DoubleCost);
                string DescriptionOfReason = GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason) == null ? "" : GetControlValue(RefServiceCard.Adjustment.DescriptionOfReason).ToString();

                if ((VoidWarranty) && (!Warranty))
                {
                    MyMessageBox.Show("Невозможно аннулировать гарантию. Данный прибор не является гарантийным.");
                    return;
                }

                //if ((DoubleCost) && (Warranty))
                //{
                //     MyMessageBox.Show("Невозможно удвоить стоимость ремонта. Данный прибор является гарантийным. Если прибор эксплуатировался ненадлежащим образом, аннулируйте гарантию на данный прибор.");
                //    return;
                //}

                if (((VoidWarranty) || (DoubleCost)) && (DescriptionOfReason == ""))
                {
                    string VoidWarrantyText = VoidWarranty ? "Укажите причину, по которой аннулирована гарантия прибора.\n" : "";
                    string DoubleCostText = DoubleCost ? "Укажите причину, по которой удвоена стоимость ремонта.\n" : "";

                    MyMessageBox.Show(VoidWarrantyText + DoubleCostText);
                    return;
                }

                if (VoidWarranty)
                {
                    CardData Passport = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid());
                    Passport.Sections[Passport.Type.AllSections["Properties"].Id].FindRow("@Name = 'Гарантия аннулирована'").SetBoolean("Value", true);
                }

                WriteLog("Отправка на согласование. Проверяем получателя");
                StaffEmployee Manager = null;
                try
                {
                    Manager = Context.GetEmployeeByPosition(RefServiceCard.Roles.SalesManager);
                }
                catch
                {
                    MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.SalesManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                WriteLog("Отправка на согласование. Получаем БП.");
                string ProcessID = CardScript.CardData.Sections[CardScript.CardData.Type.Sections["Processes"].Id].FirstRow.GetString("ProcessID");  //получаем связанный БП
                CardData cProcess = CardScript.Session.CardManager.GetCardData(new Guid(ProcessID));
                SectionData Variables = cProcess.Sections[cProcess.Type.Sections["Variables"].Id];

                string TaskID = "";
                WriteLog("Отправка на согласование. Получаем свойства карточки.");

                WriteLog("Отправка на согласование. Получаем задание на диагностику.");
                TaskID = Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.DiagnostikTask + "'").GetString("Value").ToString(); //Получаем связанное задание
                WriteLog("Отправка на согласование. Закрываем задание на диагностику.");
                TaskClose(TaskID, "");

                WriteLog("наряд отправляется на согласование");
                Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.State + "'").SetString("Value", "На согласовании"); //переносим состояние в бп
                Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.SalesManager + "'").SetString("Value", Context.GetObjectRef(Manager).Id.ToString());
                

                // Обновляем данные о ремонте
                bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);

                string DeviceRepairPerformerPosition = "";
                string AccessoriesRepairPerformerPosition = "";
                StaffEmployee DeviceRepairPerformer = null;
                StaffEmployee AccessoriesRepairPerformer = null;
                StaffEmployee FAManager = null;
                object Department = GetControlValue(RefServiceCard.Calibration.Department);

                if (AccessoriesRepair)
                {
                    AccessoriesRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager;
                    try { AccessoriesRepairPerformer = Context.GetEmployeeByPosition(AccessoriesRepairPerformerPosition); }
                    catch
                    {
                        MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.ProdactionManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (DeviceRepair)
                {
                    if ((int)Department == 1)
                    { DeviceRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager; }
                    else
                    { DeviceRepairPerformerPosition = RefServiceCard.Roles.AdjastManager; }

                    try { DeviceRepairPerformer = Context.GetEmployeeByPosition(DeviceRepairPerformerPosition); }
                    catch
                    {
                        MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + DeviceRepairPerformerPosition + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (DeviceRepairPerformer != null)
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.AdjustManager + "'").SetString("Value", Context.GetObjectRef(DeviceRepairPerformer).Id.ToString());
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.DeviceRepair + "'").SetBoolean("Value", true);
                }
                else
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.AdjustManager + "'").SetString("Value", null);
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.DeviceRepair + "'").SetBoolean("Value", false);
                }
                if (AccessoriesRepairPerformer != null)
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepairPerformer + "'").SetString("Value", Context.GetObjectRef(AccessoriesRepairPerformer).Id.ToString());
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepair + "'").SetBoolean("Value", true);
                }
                else
                {
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepairPerformer + "'").SetString("Value", null);
                    Variables.FindRow("@Name = '" + RefServiceCard.ParametersOfBusinessProcess.CompleteRepair + "'").SetBoolean("Value", false);
                }
                
                WriteLog("Передаем данные о необходимости согласования в БП");
                Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.NeedsNegotiation + "'").SetBoolean("Value", NeedsNegotiation);
                WriteLog("Заносим в БП данные о результате согласования");
                if (!NeedsNegotiation)
                    Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.NegotiationResult + "'").SetString("Value", "Согласование с отделом сбыта не требуется.");

                if ((VoidWarranty) || (DoubleCost))
                {
                    Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.ImproperUse + "'").SetBoolean("Value", true);
                    Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.DescriptionOfDefects + "'").SetString("Value", DescriptionOfReason);
                }
                Variables.FindRow("@Name='" + RefServiceCard.ParametersOfBusinessProcess.Comment + "'").SetString("Value", GetControlValue(RefServiceCard.Adjustment.Comment) == null ? "" : GetControlValue(RefServiceCard.Adjustment.Comment).ToString());
                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Consensus); //Заполняем реквизиты карточки
                CardScript.ChangeState("Consensus"); //смена состояния карточки СО

                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();*/
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик кнопки "Вернуть в работу".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_ItemClick(Object sender, ItemClickEventArgs e)
        {

            WriteLog("Начата обработка кнопки 'Вернуть'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                switch (CurrentState)
                {
                    #region // Текущее состояние = "На диагносике"
                    case (int)RefServiceCard.MainInfo.State.Diagnostics:
                        WriteLog("Текущее состояния Наряда на СО = 'На диагностике'...");

                        // ========== Определение исполнителя калибровки ==========
                        WriteLog("Начато определение исполнителя техобслуживания...");
                        StaffEmployee Calibrator = null;
                        try { Calibrator = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на техобслуживание...");
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, null);
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticDateStart, null);
                        SetControlValue(RefServiceCard.Calibration.CalibDateEnd, null);
                        SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator).Id);

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Maintenance);
                        CardScript.ChangeState("Maintenance");
                        SendToMaintenance(CurrentState);
                        WriteLog("Наряд на СО возвращен на техобслуживание...");
                        break;
                    #endregion
                    #region // Текущее состояние = "На согласовании"
                    case (int)RefServiceCard.MainInfo.State.Consensus:
                        WriteLog("Текущее состояния Наряда на СО = 'На согласовании'...");

                        // ========== Определение исполнителя диагностики ==========
                        WriteLog("Начато определение исполнителя диагностики...");
                        StaffEmployee DiagnosticPerformer = null;
                        try { DiagnosticPerformer = Context.GetEmployeeByPosition(RefServiceCard.Roles.AdjastManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.AdjastManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на диагностику...");
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, Context.GetObjectRef(DiagnosticPerformer).Id);
                        SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, null);

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Diagnostics);
                        CardScript.ChangeState("Diagnostics");
                        WriteLog("Наряд на СО возвращен на диагностику...");

                        break;
                    #endregion
                    #region// Текущее состояние = "В ремонте"
                    case (int)RefServiceCard.MainInfo.State.Adjustment:
                        WriteLog("Текущее состояния Наряда на СО = 'В ремонте'...");

                        // ========== Определение исполнителя диагностики ==========
                        WriteLog("Начато определение исполнителя диагностики...");
                        StaffEmployee DiagnosticPerformer2 = null;
                        try { DiagnosticPerformer2 = Context.GetEmployeeByPosition(RefServiceCard.Roles.AdjastManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.AdjastManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителей ремонтных работ ==========
                        WriteLog("Начато определение исполнителей ремонтных работ...");
                        Guid DeviceRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Adjuster) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Adjuster).ToGuid();
                        Guid AccessoriesRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Worker) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Worker).ToGuid();
                        // Если текущий пользователь не является исполнителем ремонтных работ, то отправка на техобслуживание невозможна.
                        if ((!DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                            (!AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                            (!IsAdmin))
                        {
                            MyMessageBox.Show("Вы не являетесь исполнителем ремонта прибора или комплектующих. Возврат на диагностику невозможен.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Начато определение, возможен ли возврат...");
                        bool Return = false;
                        // Если текущий пользователь - исполнитель ремонта приборов и ремонта комплектующих
                        if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) && AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                        {
                            Return = true;
                        }
                        else
                        {
                            // Если текущий пользователь - исполнитель ремонта прибора
                            if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) && GetControlValue(RefServiceCard.Adjustment.Worker).IsNull())
                            {
                                Return = true;
                            }
                            // Если текущий пользователь - исполнитель ремонта комплектующих
                            if (AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) && GetControlValue(RefServiceCard.Adjustment.Adjuster).IsNull())
                            {
                                Return = true;
                            }
                            if (IsAdmin)
                                Return = true;
                        }

                        if (Return)
                        {
                            // ========== Внесение изменений в Наряд на СО ==========
                            WriteLog("Начат возврат Наряда на СО на диагностику...");
                            SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, Context.GetObjectRef(DiagnosticPerformer2).Id);
                            SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, null);

                            SetControlValue(RefServiceCard.Adjustment.Worker, null);
                            SetControlValue(RefServiceCard.Adjustment.Adjuster, null);
                            SetControlValue(RefServiceCard.Adjustment.AccDateStart, null);
                            SetControlValue(RefServiceCard.Adjustment.AccDateEnd, null);
                            SetControlValue(RefServiceCard.Adjustment.RepDateStart, null);
                            SetControlValue(RefServiceCard.Adjustment.RepDateEnd, null);

                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Diagnostics);
                            Context.ChangeState(CardScript.CardData, "Diagnostics");
                            WriteLog("Наряд на СО возвращен на диагностику...");
                        }
                        else
                        {
                            MyMessageBox.Show("Вы не являетесь единственным исполнителем данного Наряда на СО. Возврат на диагностику возможен только после согласования с остальными исполнителями. Обратитесь к администратору.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    #endregion
                    #region // Текущее состояние = "На калировке"
                    case (int)RefServiceCard.MainInfo.State.Calibration:
                        WriteLog("Текущее состояния Наряда на СО = 'На калибровке'...");
                        
                        // ========== Определение исполнителя диагностики ==========
                        WriteLog("Начато определение исполнителя техобслуживания...");
                        StaffEmployee Calibrator2 = null;
                        try { Calibrator2 = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на техобслуживание...");

                        if (GetControlValue(RefServiceCard.Calibration.CalibrationEndDate).IsNull())
                        {
                            SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, null);
                            SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, null);
                        }
                        else
                        {
                            SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, ((DateTime)GetControlValue(RefServiceCard.Calibration.CalibrationEndDate)).AddHours(-1));
                        }

                        SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator2).Id);
                        if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair) || (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair))
                        {
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);
                            CardScript.ChangeState("Remaintenance");
                        }
                        else
                        {
                            SetControlValue(RefServiceCard.Calibration.CalibDateEnd, null);
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Maintenance);
                            CardScript.ChangeState("Maintenance");
                        }
                        SendToMaintenance(CurrentState);
                        WriteLog("Наряд на СО возвращен на техобслуживание...");

                        break;
                    #endregion
                    #region// Текущее состояние = "На поверке"
                    case (int)RefServiceCard.MainInfo.State.Verification:
                        WriteLog("Текущее состояния Наряда на СО = 'На поверке'...");
                        
                        // ========== Определение менеджера по сервисному обслуживанию ==========
                        WriteLog("Начато определение менеджера по сервисному обслуживанию...");
                        StaffEmployee Manager = null;
                        try { Manager = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на ожидание оплаты...");

                        if (GetControlValue(RefServiceCard.Calibration.VerificationEndDate).IsNull())
                        {
                            SetControlValue(RefServiceCard.Calibration.VerificationPerformer, null);
                            SetControlValue(RefServiceCard.Calibration.VerificationStartDate, null);
                        }
                        else
                        {
                            SetControlValue(RefServiceCard.Calibration.VerificationStartDate, ((DateTime)GetControlValue(RefServiceCard.Calibration.VerificationEndDate)).AddHours(-1));
                        }

                        SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager).Id);

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Payment);
                        CardScript.ChangeState("Payment");
                        WriteLog("Наряд на СО возвращен на ожидание оплаты...");

                        break;
                    #endregion
                    #region// Текущее состояние = "Ожидание оплаты"
                    case (int)RefServiceCard.MainInfo.State.Payment:
                        WriteLog("Текущее состояния Наряда на СО = 'Ожидание оплаты'...");
                        
                        // ========== Определение исполнителя диагностики ==========
                        WriteLog("Начато определение исполнителя техобслуживания...");
                        StaffEmployee Calibrator3 = null;
                        try { Calibrator3 = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на техобслуживание...");

                        SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator3).Id);
                        if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair) || (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair))
                        {
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);
                            CardScript.ChangeState("Remaintenance");
                        }
                        else
                        {
                            SetControlValue(RefServiceCard.Calibration.CalibDateEnd, null);
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Maintenance);
                            CardScript.ChangeState("Maintenance");
                        }
                        SendToMaintenance(CurrentState);
                        WriteLog("Наряд на СО возвращен на техобслуживание...");
                        break;
                    #endregion
                    #region // Текущее состояние = "Завершено"
                    case (int)RefServiceCard.MainInfo.State.Completed:
                        WriteLog("Текущее состояния Наряда на СО = 'Выполнено'...");

                        if ((bool)GetControlValue(RefServiceCard.Calibration.Verify))
                        {
                            WriteLog("Начат возврат Наряда на СО на ожидание оплаты...");
                            // ========== Определение менеджера о сервисному обслуживанию ==========
                            WriteLog("Начато определение менеджера по сервисному обслуживанию...");
                            StaffEmployee Manager3 = null;
                            try { Manager3 = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup); }
                            catch
                            {
                                MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            // ========== Внесение изменений в Наряд на СО ==========
                            WriteLog("Начат возврат Наряда на СО на согласование...");
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager3).Id);

                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Payment);
                            CardScript.ChangeState("Payment");
                            WriteLog("Наряд на СО возвращен на ожидание оплаты...");
                        }
                        else
                        {
                            if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration))
                            {
                                WriteLog("Начат возврат Наряда на СО на калибровку...");
                                // ========== Определение исполнителя калибровки ==========
                                WriteLog("Начато определение исполнителя калибровки...");
                                StaffEmployee CalibrationPerformer = null;
                                try { CalibrationPerformer = Context.GetEmployeeByPosition(RefServiceCard.Roles.MetrologicalLabManager); }
                                catch
                                {
                                    MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.MetrologicalLabManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                // ========== Внесение изменений в Наряд на СО ==========
                                WriteLog("Начат возврат Наряда на СО на калибровку...");
                                SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, Context.GetObjectRef(CalibrationPerformer).Id);
                                SetControlValue(RefServiceCard.Calibration.CalibrationStartDate, DateTime.Now);
                                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Calibration);

                                CardScript.ChangeState("Calibration");
                                WriteLog("Наряд на СО возвращен на ожидание оплаты...");
                            }
                            else
                            {
                                WriteLog("Начат возврат Наряда на СО на техобслуживание...");
                                // ========== Определение исполнителя калибровки ==========
                                WriteLog("Начато определение исполнителя техобслуживания...");
                                StaffEmployee Calibrator4 = null;
                                try { Calibrator4 = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager); }
                                catch
                                {
                                    MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator4).Id);
                                // Если был ремонт, то возвращаем на повторное техобслуживание
                                if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair) || (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair))
                                {
                                    // ========== Внесение изменений в Наряд на СО ==========
                                    WriteLog("Начат возврат Наряда на СО на повторное техобслуживание...");
                                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);

                                    CardScript.ChangeState("Remaintenance");
                                    WriteLog("Наряд на СО возвращен на повторное техобслуживание...");
                                }
                                // Если ремонта не было, то возвращаем на первичное техобслуживание
                                else
                                {
                                    // ========== Внесение изменений в Наряд на СО ==========
                                    WriteLog("Начат возврат Наряда на СО на первичное техобслуживание...");
                                    
                                    SetControlValue(RefServiceCard.Calibration.CalibDateEnd, null);
                                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Maintenance);

                                    CardScript.ChangeState("Maintenance");
                                    WriteLog("Наряд на СО возвращен на повторное техобслуживание...");
                                }
                                SendToMaintenance(CurrentState);
                            }
                        }
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, null);
                        break;
                    #endregion
                    #region // Текущее состояние = "Отказ от ремонта"
                    case (int)RefServiceCard.MainInfo.State.Failure:
                        WriteLog("Текущее состояния Наряда на СО = 'Отказ от ремонта'...");

                        // ========== Определение исполнителя согласования ==========
                        WriteLog("Начато определение исполнителя согласования...");
                        StaffEmployee Manager2 = null;
                        try { Manager2 = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // ========== Внесение изменений в Наряд на СО ==========
                        WriteLog("Начат возврат Наряда на СО на согласование...");

                        SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager2).Id);
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, null);

                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Consensus);
                        CardScript.ChangeState("Consensus");
                        WriteLog("Наряд на СО возвращен на согласование...");
                        SendToRepair(CurrentState);
                        break;
                    #endregion
                }

                WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();
            }
            catch (MyException Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик кнопки "Передать в сбыт".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToSale_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать в сбыт'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                switch (CurrentState)
                {
                    #region // Текущее состояние = "На техобслуживании"
                    case (int)RefServiceCard.MainInfo.State.Maintenance:
                        WriteLog("Текущее состояние наряда на СО = 'На техобслуживании'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message = CheckActions(DeviceRepair, AccessoriesRepair, Calibration, Verify);
                        if (!String.IsNullOrEmpty(Message))
                        {
                            MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //Если поле "Внешнее проявление проблемы" не заполнено, то дальнейшие действия прерываются.
                        object Problem = GetControlValue(RefServiceCard.Calibration.Problem);
                        if ((Problem == null) || (Problem.ToString() == ""))
                        {
                            MyMessageBox.Show("Заполните поле \"Внешнее проявление проблемы\".", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если в группе полей "Требуется вид сервиса" указан ремонт приборов и/или комплектующих, то дальнейшие действия прерываются.
                        string Improvements = GetControlValue(RefServiceCard.Calibration.Improvements) == null ? "" : GetControlValue(RefServiceCard.Calibration.Improvements).ToString();
                        if ((DeviceRepair) || (AccessoriesRepair))   //Выходим, если требуется ремонт
                        {
                            MyMessageBox.Show("Вы не можете передать в сбыт прибор/комплектующие, которым требуется ремонт. Передайте прибор/комплектующие на ремонт через отдел сбыта или измените требуемый вид сервиса.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // Если в поле "Необходимые доработки" указан текст, то дальнейшие действия прерываются.
                            if (Improvements != "")
                            {
                                MyMessageBox.Show("Вы не можете передать в сбыт данный прибор. Для него требуется выполнить доработки, описанные в поле \"Необходимые доработки\". Передайте прибор на диагностику.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Если в поле "Требуется вид сервиса" указана калибровка, и в текущем Наряде на СО имеется прибор, то дальнейшие действия прерываются. 
                        bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (Calibration && !OnlyAccessories)
                        {
                            MyMessageBox.Show("Для данного прибора требуется осуществить калибровку. Передайте прибор на калибровку в метрологическую лабораторию.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Переформирование Упаковочного листа для текущего Наряда на СО ==========
                        WriteLog("Начато переформирование Упаковочного листа...");
                        // Определение перечня доп. изделий
                        List<string> AdditionalWares = new List<string>();
                        for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                        {
                            Guid AdditionalWareID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid();
                            Guid DeviceTypeID = UniversalCard.GetItemPropertyValue(AdditionalWareID, "Наименование прибора").ToGuid();
                            AdditionalWares.Add(UniversalCard.GetItemName(DeviceTypeID));
                        }
                        WriteLog("Выполнен поиск доп. изделий...");
                        // Переформирование упаковочного листа
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                            String CData = GetControlValue(RefServiceCard.MainInfo.PackedListData).ToString();
                            String DeviceName = UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid());

                            AddCompleteSertificates(ref CData, DeviceName, AdditionalWares, OnlyAccessories, Calibration, Verify);
                            SensorsListCorrection(ref CData, DeviceName, AdditionalWares, OnlyAccessories);
                            WriteLog("Выполнена корректировка данных Упаковочного листа...");
                            Guid FileId = GetControlValue(RefServiceCard.MainInfo.PackedListId) != null ? GetControlValue(RefServiceCard.MainInfo.PackedListId).ToGuid() : Guid.Empty;

                            FillPackFile(Context, UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType)), CData, ref FileId);
                            SetControlValue(RefServiceCard.MainInfo.PackedListId, FileId);
                            WriteLog("Сформирован новый Упаковочный лист...");
                        //}

                        // ========== Определение исполнителей согласования ==========
                        WriteLog("Начато определение исполнителей согласования...");
                        StaffEmployee Manager3 = null;
                        try
                        {
                            Manager3 = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель согласования определен: " + Manager3.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Today.Date);
                        WriteLog("Установлена дата завершения ремонта...");

                        // Если для прибора требуется поверка, то отправляем его на ожидание оплаты
                        if (Verify)
                        {
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager3).Id);
                            WriteLog("Установлен исполнитель...");
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Payment);                                
                            CardScript.ChangeState("Payment");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Ожидание оплаты'...");
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Payment);
                        }
                        // Если для прибора не требуется поверка, то отправляем его в сбыт для отправки клиенту
                        else
                        {
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Completed);
                            CardScript.ChangeState("Completed");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Завершено'...");
                            SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Now);
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Completed);
                        }

                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                    #region // Текущее состояние = "На повторном техобслуживании"
                    case (int)RefServiceCard.MainInfo.State.Remaintenance:
                        

                        WriteLog("Текущее состояние наряда на СО = 'На повторном техобслуживании'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                        bool Calibration2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify2 = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        bool DeviceRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                        bool AccessoriesRepair2 = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                        String Message2 = CheckActions(DeviceRepair2, AccessoriesRepair2, Calibration2, Verify2);
                        if (!String.IsNullOrEmpty(Message2))
                        {
                            MyMessageBox.Show(Message2, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если не заполнено фактическое время, затраченное на техобслуживание прибора, то дальнейшие действия прерываются.
                        object CalibrationTime = GetControlValue(RefServiceCard.Calibration.CalibrationTime);
                        bool OnlyAccessories2 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (!OnlyAccessories2)
                        {
                            if ((CalibrationTime == null) || ((decimal)CalibrationTime == 0))
                            {
                                MyMessageBox.Show("Укажите фактическое время техобслуживания прибора и комплектующих после ремонта (кроме датчиков) в поле \"Трудоемкость калибровки\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        // Если не заполнено фактическое время, затраченное на техобслуживание доп. изделий, то дальнейшие действия прерываются.
                        Message2 = CheckAdditionalWares(Table_AdditionalWaresList, RefServiceCard.AdditionalWaresList.CalibrationTime, this);
                        if (Message2 != "")
                        {
                            MyMessageBox.Show("Не заполнена трудоемкость техобслуживания после ремонта доп. изделий: " + Message2 + ".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если в поле "Требуется вид сервиса" указана калибровка, и в текущем Наряде на СО имеется прибор, то дальнейшие действия прерываются. 
                        if (Calibration2 && !OnlyAccessories2)
                        {
                            MyMessageBox.Show("Для данного прибора требуется провести калибровку. Передайте прибор на калибровку в метрологическую лабораторию.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Переформирование Упаковочного листа для текущего Наряда на СО ==========
                        WriteLog("Начато переформирование Упаковочного листа...");
                        // Определение перечня доп. изделий
                        List<string> AdditionalWares2 = new List<string>();
                        for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                        {
                            Guid AdditionalWareID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid();
                            Guid DeviceTypeID = UniversalCard.GetItemPropertyValue(AdditionalWareID, "Наименование прибора").ToGuid();
                            AdditionalWares2.Add(UniversalCard.GetItemName(DeviceTypeID));
                        }
                        WriteLog("Выполнен поиск доп. изделий...");
                        // Переформирование упаковочного листа
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                        String CData2 = GetControlValue(RefServiceCard.MainInfo.PackedListData).ToString();
                        String DeviceName2 = UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid());

                        AddCompleteSertificates(ref CData2, DeviceName2, AdditionalWares2, OnlyAccessories2, Calibration2, Verify2);
                        SensorsListCorrection(ref CData2, DeviceName2, AdditionalWares2, OnlyAccessories2);
                        WriteLog("Выполнена корректировка данных Упаковочного листа...");
                        Guid FileId2 = GetControlValue(RefServiceCard.MainInfo.PackedListId) != null ? GetControlValue(RefServiceCard.MainInfo.PackedListId).ToGuid() : Guid.Empty;

                        FillPackFile(Context, UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType)), CData2, ref FileId2);
                        SetControlValue(RefServiceCard.MainInfo.PackedListId, FileId2);
                        WriteLog("Сформирован новый Упаковочный лист...");
                        //}

                        // ========== Определение исполнителей согласования ==========
                        WriteLog("Начато определение исполнителей согласования...");
                        StaffEmployee Manager = null;
                        try
                        {
                            Manager = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель согласования определен: " + Manager.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Today.Date);
                        WriteLog("Установлена дата завершения ремонта...");
                       

                        // Если для прибора требуется поверка, то отправляем его на ожидание оплаты
                        if (Verify2)
                        {
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager).Id);
                            WriteLog("Установлен исполнитель...");
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Payment);
                            CardScript.ChangeState("Payment");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Ожидание оплаты'...");
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Payment);
                        }
                        // Если для прибора не требуется поверка, то отправляем его в сбыт для отправки клиенту
                        else
                        {
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Completed);
                            CardScript.ChangeState("Completed");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Завершено'...");
                            SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Now);
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Completed);
                        }

                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                    #region// Текущее состояние = "На калибровке"
                    case (int)RefServiceCard.MainInfo.State.Calibration:
                        WriteLog("Текущее состояние наряда на СО = 'На калибровке'...");

                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        // Проверяем, требуется ли перед отправкой сформировать Протокол калибровки прибора
                        WriteLog("Начата проверка Протокола калибровки...");
                        bool OnlyAccessories3 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (!OnlyAccessories3) // && !(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        {
                            CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString()));
                            Guid CalibrationProtocolID = GetControlValue(RefServiceCard.Calibration.CalibrationProtocol) == null ? Guid.Empty :
                                GetControlValue(RefServiceCard.Calibration.CalibrationProtocol).ToGuid();
                            if (NeedCalibrationProtocol())
                            {
                                if ((CalibrationProtocolID.Equals(Guid.Empty)) ||
                                    (!CalibrationDocs.CalibrationLib.TestFileFilds(CardScript.Session, CardScript.Session.CardManager.GetCardData(CalibrationProtocolID))))
                                {
                                    XtraMessageBox.Show("Требуется создать и заполнить 'Протокол калибровки'.");
                                    return;
                                }
                            }
                        }

                        // Проверяем, требуется ли перед отправкой сформировать "Сертификат о калибровке"
                        WriteLog("Начата проверка Сертификата о калибровке...");
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                        if (!OnlyAccessories3)
                        {
                            Guid CalibrationCertificateID = GetControlValue(RefServiceCard.Calibration.CalibrationCertificate) == null ? Guid.Empty :
                                GetControlValue(RefServiceCard.Calibration.CalibrationCertificate).ToGuid();
                            if (CalibrationCertificateID.Equals(Guid.Empty))
                            {
                                XtraMessageBox.Show("Требуется создать и заполнить 'Сертификат о калибровке'.");
                                return;
                            }
                        }
                        // Раньше для доп. изделий формировались собственные сертификаты, потом это отменили, т.к. доп изделия не являются средствами изменения.
                        /*else
                        {
                            List<string> FindWares = new List<string>();
                            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                            {
                                Guid AdditionalWareID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid();
                                Guid DeviceTypeID = UniversalCard.GetItemPropertyValue(AdditionalWareID, "Наименование прибора").ToGuid();
                                string DeviceTypeName = UniversalCard.GetItemName(DeviceTypeID);
                                if (CalibrationDocs.CalibrationLib.AdditionalWaresList.Any(r => r == DeviceTypeName))
                                {
                                    Guid WaresCalibrationCertificateID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate] == null ? Guid.Empty :
                                Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate].ToGuid();
                                    if (WaresCalibrationCertificateID.Equals(Guid.Empty))
                                    { FindWares.Add(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumber].ToString()); }
                                }
                            }
                            if (FindWares.Count() > 0)
                            {
                                string WaresList = FindWares.Aggregate((currentvalue, nextvalue) => currentvalue + "\n - " + nextvalue);
                                XtraMessageBox.Show("Требуется создать и заполнить 'Сертификат о калибровке' для следущих изделий:" + WaresList);
                                return;
                            }
                        }*/
                        //}
                        // ========== Переформирование Упаковочного листа для текущего Наряда на СО ==========
                        WriteLog("Начато переформирование Упаковочного листа...");
                        bool Calibration3 = GetControlValue(RefServiceCard.Calibration.DeviceCalibration) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify3 = GetControlValue(RefServiceCard.Calibration.Verify) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        // Определение перечня доп. изделий
                        List<string> AdditionalWares3 = new List<string>();
                        for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                        {
                            Guid AdditionalWareID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid();
                            Guid DeviceTypeID = UniversalCard.GetItemPropertyValue(AdditionalWareID, "Наименование прибора").ToGuid();
                            AdditionalWares3.Add(UniversalCard.GetItemName(DeviceTypeID));
                        }
                        WriteLog("Выполнен поиск доп. изделий...");
                        // Переформирование упаковочного листа
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                        String CData3 = GetControlValue(RefServiceCard.MainInfo.PackedListData).ToString();
                        String DeviceName3 = UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid());

                        AddCompleteSertificates(ref CData3, DeviceName3, AdditionalWares3, OnlyAccessories3, Calibration3, Verify3);
                        SensorsListCorrection(ref CData3, DeviceName3, AdditionalWares3, OnlyAccessories3);
                        WriteLog("Выполнена корректировка данных Упаковочного листа...");
                        Guid FileId3 = GetControlValue(RefServiceCard.MainInfo.PackedListId) != null ? GetControlValue(RefServiceCard.MainInfo.PackedListId).ToGuid() : Guid.Empty;

                        FillPackFile(Context, UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType)), CData3, ref FileId3);
                        SetControlValue(RefServiceCard.MainInfo.PackedListId, FileId3);
                        WriteLog("Сформирован новый Упаковочный лист...");
                        //}

                        // ========== Определение исполнителей согласования ==========
                        WriteLog("Начато определение исполнителей согласования...");
                        StaffEmployee Manager2 = null;
                        try
                        {
                            Manager2 = GetEmployeeByGroup(Context, RefServiceCard.Roles.ServiceManagerGroup);
                        }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.ServiceManagerGroup + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Исполнитель согласования определен: " + Manager2.DisplayName + "...");

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.Calibration.CalibrationEndDate, DateTime.Now);
                        WriteLog("Установлена дата завершения калибровки...");

                        // Если для прибора требуется поверка, то отправляем его на ожидание оплаты
                        if (Verify3)
                        {
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(Manager2).Id);
                            WriteLog("Установлен исполнитель...");
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Payment);
                            CardScript.ChangeState("Payment");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Ожидание оплаты'...");
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Payment);
                        }
                        // Если для прибора не требуется поверка, то отправляем его в сбыт для отправки клиенту
                        else
                        {
                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Completed);
                            SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Now);
                            CardScript.ChangeState("Completed");
                            WriteLog("Текущий Наряд на СО переведен в состояние 'Завершено'...");
                            WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                            SendToSale((int)RefServiceCard.MainInfo.State.Completed);
                        }

                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                    #region// Текущее состояние = "На поверке"
                    case (int)RefServiceCard.MainInfo.State.Verification:
                        WriteLog("Текущее состояние наряда на СО = 'На поверке'...");
                        
                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        // Проверяем, требуется ли перед отправкой сформировать Протокол поверки прибора
                        WriteLog("Начата проверка Протокола поверки...");
                        bool OnlyAccessories4 = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (!OnlyAccessories4) // && !(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        {
                            CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString()));
                            Guid VerificationProtocolID = GetControlValue(RefServiceCard.Calibration.VerificationProtocol) == null ? Guid.Empty :
                                GetControlValue(RefServiceCard.Calibration.VerificationProtocol).ToGuid();
                            if (NeedVerificationProtocol())
                            {
                                if ((VerificationProtocolID.Equals(Guid.Empty)) ||
                                    (!CalibrationDocs.CalibrationLib.TestFileFilds(CardScript.Session, CardScript.Session.CardManager.GetCardData(VerificationProtocolID))))
                                {
                                    XtraMessageBox.Show("Требуется создать и заполнить 'Протокол поверки'.");
                                    return;
                                }
                            }
                        }

                        // Проверяем, требуется ли перед отправкой сформировать "Сертификат о калибровке"
                        WriteLog("Начата проверка Свидетельства о поверке...");
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                        if (!OnlyAccessories4)
                        {
                            Guid VerificationCertificateID = GetControlValue(RefServiceCard.Calibration.VerificationCertificate) == null ? Guid.Empty :
                                GetControlValue(RefServiceCard.Calibration.VerificationCertificate).ToGuid();
                            if (VerificationCertificateID.Equals(Guid.Empty))
                            {
                                XtraMessageBox.Show("Требуется создать 'Свидетельство о поверке' или 'Извещение о непригодности'.");
                                return;
                            }
                        }

                        // ========== Переформирование Упаковочного листа для текущего Наряда на СО ==========
                        WriteLog("Начато переформирование Упаковочного листа...");
                        bool Calibration4 = GetControlValue(RefServiceCard.Calibration.DeviceCalibration) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                        bool Verify4 = GetControlValue(RefServiceCard.Calibration.Verify) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                        // Определение перечня доп. изделий
                        List<string> AdditionalWares4 = new List<string>();
                        for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
                        {
                            Guid AdditionalWareID = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToGuid();
                            Guid DeviceTypeID = UniversalCard.GetItemPropertyValue(AdditionalWareID, "Наименование прибора").ToGuid();
                            AdditionalWares4.Add(UniversalCard.GetItemName(DeviceTypeID));
                        }
                        WriteLog("Выполнен поиск доп. изделий...");
                        // Переформирование упаковочного листа
                        //if (!(bool)CardScript.Session.Properties["IsAdmin"].Value)
                        //{
                        String CData4 = GetControlValue(RefServiceCard.MainInfo.PackedListData).ToString();
                        String DeviceName4 = UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid());

                        AddCompleteSertificates(ref CData4, DeviceName4, AdditionalWares4, OnlyAccessories4, Calibration4, Verify4);
                        SensorsListCorrection(ref CData4, DeviceName4, AdditionalWares4, OnlyAccessories4);
                        WriteLog("Выполнена корректировка данных Упаковочного листа...");
                        Guid FileId4 = GetControlValue(RefServiceCard.MainInfo.PackedListId) != null ? GetControlValue(RefServiceCard.MainInfo.PackedListId).ToGuid() : Guid.Empty;

                        FillPackFile(Context, UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType)), CData4, ref FileId4);
                        SetControlValue(RefServiceCard.MainInfo.PackedListId, FileId4);
                        WriteLog("Сформирован новый Упаковочный лист...");
                        //}

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                        SetControlValue(RefServiceCard.Calibration.VerificationEndDate, DateTime.Now);
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Now);
                        WriteLog("Установлена дата завершения поверки...");

                        // Отправляем Наряд на СО в сбыт для отправки клиенту
                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Completed);
                        CardScript.ChangeState("Completed");
                        WriteLog("Текущий Наряд на СО переведен в состояние 'Завершено'...");

                        // ========== Запуск метода отправки в сбыт ==========
                        WriteLog("Производится запуск метода отправки Наряда на СО в сбыт...");
                        SendToSale((int)RefServiceCard.MainInfo.State.Completed);

                        // ========== Сохранение карточки Наряда на СО ==========
                        WriteLog("Производится сохранение карточки Наряда на СО...");
                        CardScript.SaveCard();
                        CardScript.CardFrame.CardHost.CloseCards();
                        break;
                    #endregion
                }

                WriteLog("Завершена обработка кнопки 'Передать в сбыт'...");
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик кнопки "Акт приемки"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActOfLoad_ItemClick(Object sender, ItemClickEventArgs e)
        {

            string DeviceNumber = GetControlValue(RefServiceCard.MainInfo.DeviceCardID) == null ? "" : GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToString();

            if (DeviceNumber != "")
            {
                CardData Passport = CardScript.Session.CardManager.GetCardData(new Guid(DeviceNumber));
                DeviceNumber = Passport.Sections[Passport.Type.Sections["Properties"].Id].FindRow("@Name = 'Запись в справочнике'").GetString("Value");
            }

            string DeviceType = GetControlValue(RefServiceCard.MainInfo.DeviceType) == null ? "" : GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString();
            bool OnlyA = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
            bool DeviceRepair = GetControlValue(RefServiceCard.Calibration.DeviceRepair) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
            bool DeviceCalibration = GetControlValue(RefServiceCard.Calibration.DeviceCalibration) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
            bool Verify = GetControlValue(RefServiceCard.Calibration.Verify) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.Verify);
            bool WarrantyService = GetControlValue(RefServiceCard.Calibration.WarrantyService) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.WarrantyService);
            string AList = GetControlValue(RefServiceCard.MainInfo.AList) == null ? "" : GetControlValue(RefServiceCard.MainInfo.AList).ToString();
            string AdditionalWares = GetControlValue(RefServiceCard.MainInfo.AdditionalWares) == null ? "" : GetControlValue(RefServiceCard.MainInfo.AdditionalWares).ToString();

            ReferenceList RefList = Context.GetObject<ReferenceList>(GetControlValue(RefServiceCard.MainInfo.Links).ToGuid());
            CardData ApplicationCard = CardScript.Session.CardManager.GetCardData(RefList.References.First(Ref => Ref.CardType.Equals(RefApplicationCard.ID)).Card);
            string ApplicationNumber = ApplicationCard.Sections[ApplicationCard.Type.Sections["Numbers"].Id].FirstRow.GetString("Number");
            DateTime ApplicationDate = (DateTime)ApplicationCard.Sections[RefApplicationCard.MainInfo.ID].FirstRow.GetDateTime(RefApplicationCard.MainInfo.RegDate);
            string ClientID = GetControlValue(RefServiceCard.MainInfo.Client).ToString();

            string FileCardID = GetControlValue(RefServiceCard.MainInfo.Files).ToString();

            Device CurrentDevice = new Device(this, DeviceNumber, DeviceType, OnlyA, DeviceRepair, DeviceCalibration, Verify, WarrantyService, AList, "", "", false, AdditionalWares, CardScript.CardData.Id.ToString());
            Guid Act = FillingDeedOfLoad(ActLoadTemplate, CurrentDevice, ClientID, ApplicationNumber, ApplicationDate, FileCardID);

            IVersionedFileCardService VersionedFileCardService = Context.GetService<IVersionedFileCardService>();
            Process.Start(VersionedFileCardService.Download(VersionedFileCardService.OpenCard(Act)));
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Отправить на техобслуживание".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToMaintenance_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать на техобслуживание'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);

                        WriteLog("Текущее состояние Наряда на СО = 'В ремонте'...");
                        // ========== Проверка заполнения полей Наряда на СО ==========
                        WriteLog("Начата проверка правильности заполнения полей...");
                        // Если в таблице неисправностей нет ни одной строки, то выводится сообщение об ошибке.
                        if (Table_Description.RowCount <= 0)
                        {
                            MyMessageBox.Show("В таблице \"Описание неисправностей\" необходимо добавить хотябы одну неисправность.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Если для какого-то узла не заполнены ремонтные работы, то выводится сообщение об ошибке.
                        for (int i = 0; i < Table_Description.RowCount; i++)
                        {
                            if ((Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList] == null) || (Table_Description[i][RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() == ""))
                            {
                                MyMessageBox.Show("Не заполнены ремонтные работы для сборочного узла \"" + Table_Description[i][RefServiceCard.DescriptionOfFault.BlockOfDevice] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        // Если не заполнена фактическая дата окончания ремонтной работы, то выводится сообщение об ошибке.
                        for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                        {
                            if (Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate].IsNull())
                            {
                                MyMessageBox.Show("Не заполнена фактическая дата окончания ремонтной работы \"" + Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksType] + "\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        // Если в таблице "Ремонтные работы" не заполнены поля, то выводится сообщение об ошибке.
                        ArrayList PropertyList = new ArrayList();
                        for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                        {
                            if (Table_RepairWorks[i][RefServiceCard.RepairWorks.NegotiationResult].ToString() != "Не выполнять")
                            {
                                if (Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] == null || (decimal)Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] <= 0)
                                {
                                    PropertyList.Add("Трудоемкость");
                                    break;
                                }

                                if (Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer] == null || Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer].ToString() == "")
                                {
                                    PropertyList.Add("Исполнитель");
                                    break;
                                }

                                if ((Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate] == null) || (((DateTime)Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate]).Equals(DateTime.MinValue)))
                                {
                                    PropertyList.Add("Дата завершения");
                                    break;
                                }
                            }
                        }
                        if (PropertyList.Count > 0)
                        {
                            string ErrorText = PropertyList.ToArray().Select(r => "- \"" + r.ToString() + "\"").Aggregate((currentValue, nextValue) => currentValue + ";\n" + nextValue) + ".";
                            MyMessageBox.Show("Для отправки на калибровку необходимо заполнить следующие поля:\n" + ErrorText, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Завершена проверка правильности заполнения полей...");

                        // ========== Определение исполнителей ремонтных работ ==========
                        WriteLog("Начато определение исполнителей ремонтных работ...");
                        Guid DeviceRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Adjuster) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Adjuster).ToGuid();
                        Guid AccessoriesRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Worker) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Worker).ToGuid();
                        // Если текущий пользователь не является исполнителем ремонтных работ, то отправка на техобслуживание невозможна.
                        if ((!DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                            (!AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id)) &&
                            (!IsAdmin))
                        {
                            MyMessageBox.Show("Вы не являетесь исполнителем ремонта прибора или комплектующих. Отправка на тех. обслуживание невозможна.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Определение исполнителя калибровки ==========
                        WriteLog("Начато определение исполнителя калибровки...");
                        StaffEmployee Calibrator = null;
                        try { Calibrator = Context.GetEmployeeByPosition(RefServiceCard.Roles.DeputyCalibrationManager); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.DeputyCalibrationManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ========== Внесение изменений в текущий Наряд на СО ==========
                        WriteLog("Начато внесение изменений в текущий Наряд на СО...");

                        WriteLog("Производится отметка выполненных доработок в паспорте прибора...");
                        // Если среди выполненных реомнтных работ есть доработки, то отметка об их выполнении проставляется в паспорт прибора.
                        bool OnlyAccessories = GetControlValue(RefServiceCard.MainInfo.OnlyA) == null ? false : (bool)GetControlValue(RefServiceCard.MainInfo.OnlyA);
                        if (!OnlyAccessories)
                        {
                            CardData Passport = CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid());
                            if (Passport.LockStatus != LockStatus.Free)
                                Passport.ForceUnlock();
                            WriteLog("Получен паспорт прибора...");
                            SectionData Properties = Passport.Sections[Passport.Type.AllSections["Properties"].Id];
                            SubSectionData ParentDocuments = Properties.FindRow("@Name = 'Документ'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                            SubSectionData Completed = Properties.FindRow("@Name = 'Выполнено'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                            SubSectionData CompleteDate = Properties.FindRow("@Name = 'Дата выполнения'").ChildSections[Passport.Type.AllSections["SelectedValues"].Id];
                            WriteLog("Получены свойства паспорта...");
                            WriteLog("Проверяется перечень ремонтных работ...");
                            for (int i = 0; i < Table_RepairWorks.RowCount; i++)
                            {
                                WriteLog(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksType].ToString());
                                if (Table_RepairWorks[i][RefServiceCard.RepairWorks.NegotiationResult].ToString() != "Не выполнять")
                                {
                                    if (((decimal)Table_RepairWorks[i][RefServiceCard.RepairWorks.FactLaboriousness] > 0) && ((bool)Table_RepairWorks[i][RefServiceCard.RepairWorks.Revision]))
                                    {
                                        WriteLog("Проставляется отметка о выполнении...");
                                        Guid ParentDocument = UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Документ") == null ? Guid.Empty :
                                            UniversalCard.GetItemPropertyValue(Table_RepairWorks[i][RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Документ").ToGuid();
                                        WriteLog("Получено родительское 'Разрешение об изменении'...");
                                        if (!ParentDocument.Equals(Guid.Empty))
                                        {
                                            WriteLog("Родительское 'Разрешение об изменении' существует...");
                                            RowData RowParentDocument = ParentDocuments.Rows.Find("SelectedValue", ParentDocument);
                                            WriteLog("Произведен поиск доработки в паспорте прибора...");
                                            if (RowParentDocument != null)
                                            {
                                                WriteLog("Доработка существует...");
                                                int Order = (int)RowParentDocument.GetInt32("Order");
                                                RowData RowCompleted = Completed.FindRow("@Order = '" + Order + "'");
                                                RowData RowCompleteDate = CompleteDate.FindRow("@Order = '" + Order + "'");
                                                RowCompleted.SetBoolean("SelectedValue", true);
                                                RowCompleteDate.SetDateTime("SelectedValue", DateTime.Today);
                                                WriteLog("Внесена отметка об исполнении...");
                                            }
                                            else
                                            {
                                                WriteLog("Указанной доработки в паспорте не существует...");
                                            }
                                        }
                                    }
                                    // Если для ремонтной работы не указан исполнитель и дата завершения, то выводится сообщение об ошибке.
                                    if (Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer] == null || Table_RepairWorks[i][RefServiceCard.RepairWorks.Performer].ToString() == "")
                                    {
                                        PropertyList.Add("Исполнитель");
                                        break;
                                    }

                                    if (Table_RepairWorks[i][RefServiceCard.RepairWorks.EndDate] == null)
                                    {
                                        PropertyList.Add("Дата завершения");
                                        break;
                                    }
                                }
                            }
                        }

                        WriteLog("Производится определение, завершен ли ремонт...");
                        DateTime? RepDateStart = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.RepDateStart));
                        DateTime? AccDateStart = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.AccDateStart));
                        DateTime? RepDateEnd = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.RepDateEnd));
                        DateTime? AccDateEnd = GetNullableDateTime(GetControlValue(RefServiceCard.Adjustment.AccDateEnd));

                        // Если текущий пользователь - исполнитель ремонта приборов и ремонта комплектующих
                        if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) && AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                        {
                            SetControlValue(RefServiceCard.Adjustment.RepDateEnd, DateTime.Now);
                            SetControlValue(RefServiceCard.Adjustment.AccDateEnd, DateTime.Now);
                            SetControlValue(RefServiceCard.Calibration.CalDateStart, DateTime.Now);

                            SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);                                                                                             
                            CardScript.ChangeState("Remaintenance");
                            SendToMaintenance(CurrentState);
                            MyMessageBox.Show("Передайте прибор и комплектующие в калибровочную лабораторию на техобслуживание.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CardScript.CardFrame.CardHost.CloseCards();
                        }
                        else
                        {
                            // Если текущий пользователь - исполнитель ремонта прибора
                            if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                            {
                                SetControlValue(RefServiceCard.Adjustment.RepDateEnd, DateTime.Now);

                                if ((GetControlValue(RefServiceCard.Adjustment.Worker).IsNull()) || GetControlValue(RefServiceCard.Adjustment.Worker).ToGuid().Equals(Guid.Empty) || (!AccDateEnd.IsNull() && (AccDateEnd > AccDateStart)))
                                {
                                    SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator).Id);
                                    SetControlValue(RefServiceCard.Calibration.CalDateStart, DateTime.Now);
                                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);
                                    CardScript.ChangeState("Remaintenance");
                                    SendToMaintenance(CurrentState);
                                    MyMessageBox.Show("Передайте прибор в калибровочную лабораторию на техобслуживание.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CardScript.CardFrame.CardHost.CloseCards();
                                }
                                else
                                {
                                    MyMessageBox.Show("Исполнитель ремонта комплектующих еще не завершил свои работы. Даный наряд на сервисное обслуживание будет передан на техобслуживание только после того, как исполнитель ремонта комплектующих завершит свои работы.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                } 
                            }
                            // Если текущий пользователь - исполнитель ремонта комплектующих
                            if (AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                            {
                                SetControlValue(RefServiceCard.Adjustment.AccDateEnd, DateTime.Now);

                                if ((GetControlValue(RefServiceCard.Adjustment.Adjuster).IsNull()) || GetControlValue(RefServiceCard.Adjustment.Adjuster).ToGuid().Equals(Guid.Empty) || (!RepDateEnd.IsNull() && (RepDateEnd > RepDateStart)))
                                {
                                    SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(Calibrator).Id);
                                    SetControlValue(RefServiceCard.Calibration.CalDateStart, DateTime.Now);
                                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);
                                    CardScript.ChangeState("Remaintenance");
                                    SendToMaintenance(CurrentState);
                                    MyMessageBox.Show("Передайте комплектующие в калибровочную лабораторию на техобслуживание.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CardScript.CardFrame.CardHost.CloseCards();
                                }
                                else
                                {
                                    MyMessageBox.Show("Исполнитель ремонта прибора еще не завершил свои работы. Даный наряд на сервисное обслуживание будет передан на техобслуживание только после того, как исполнитель ремонта прибора завершит свои работы.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            // Если текущий пользователь - админ
                            if (IsAdmin)
                            {
                                if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair))
                                    SetControlValue(RefServiceCard.Adjustment.RepDateEnd, DateTime.Now);
                                if ((bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair))
                                    SetControlValue(RefServiceCard.Adjustment.AccDateEnd, DateTime.Now);
                                SetControlValue(RefServiceCard.Calibration.CalDateStart, DateTime.Now);

                                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Remaintenance);
                                CardScript.ChangeState("Remaintenance");
                                SendToMaintenance(CurrentState);
                                MyMessageBox.Show("Передайте прибор и комплектующие в калибровочную лабораторию на техобслуживание.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CardScript.CardFrame.CardHost.CloseCards();
                            }
                }

                        WriteLog("Изменения в текущий Наряд на СО внесены...");
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Отправить на поверку".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToVerification_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Передать на поверку'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);

                WriteLog("Текущее состояние наряда на СО = 'Ожидание оплаты' или 'Завершено'...");

                // ========== Проверка заполнения полей Наряда на СО ==========
                WriteLog("Начата проверка правильности заполнения полей...");
                //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                bool Calibration = (bool)GetControlValue(RefServiceCard.Calibration.DeviceCalibration);
                bool Verify = (bool)GetControlValue(RefServiceCard.Calibration.Verify);
                bool DeviceRepair = (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                bool AccessoriesRepair = (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                String Message = CheckActions(DeviceRepair, AccessoriesRepair, Calibration, Verify);
                if (!String.IsNullOrEmpty(Message))
                {
                    MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Если в поле "Требуется вид сервиса" не указана калибровка, то дальнейшие действия прерываются. 
                if (!Verify)
                {
                    MyMessageBox.Show("Для данного прибора не указан требуемый вид сервиса 'Поверка'", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Если поверка прибора осуществляется в сторонней организации, то выводится сообщение об ошибке.
                if (!NeedVerificationProtocol())
                {
                    MyMessageBox.Show("Для данного прибора поверка осуществляется в сторонней организации, а не в СКБ ЭП.\n" +
                        "Отправьте прибор вместе с 'Сертификатом о калибровке' в стороннюю организацию для осуществления поверки.\n" +
                        "'Сертификат о калибровке' находится на вкладке 'Калибровка'.\n" +
                        "Если 'Сертификат о калибровке' на вкладке 'Калбровка' отсутствует, передайте прибор на калибровку в метрологическую лабораторию.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ========== Определение исполнителей калибровки ==========
                WriteLog("Начато определение исполнителей поверки...");
                StaffEmployee VerificationPerformer = null;
                try
                {
                    VerificationPerformer = Context.GetEmployeeByPosition(RefServiceCard.Roles.MetrologicalLabManager);
                }
                catch
                {
                    MyMessageBox.Show("Не удалось найти сотрудника в группе \"" + RefServiceCard.Roles.MetrologicalLabManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                WriteLog("Исполнитель поверки определен: " + VerificationPerformer.DisplayName + "...");

                // ========== Внесение изменений в текущий Наряд на СО ==========
                WriteLog("Начато внесение изменений в текущий Наряд на СО...");

                SetControlValue(RefServiceCard.Calibration.VerificationStartDate, DateTime.Now);
                SetControlValue(RefServiceCard.Calibration.VerificationPerformer, Context.GetObjectRef(VerificationPerformer).Id);
                WriteLog("Установлены параметры исполнения поверки...");

                SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Verification);
                CardScript.ChangeState("Verification");
                WriteLog("Текущий Наряд на СО переведен в состояние 'На поверке'...");

                // ========== Запуск метода отправки на поверку ==========
                WriteLog("Производится запуск метода отправки Наряда на СО на поверку...");


                // ========== Сохранение карточки Наряда на СО ==========
                WriteLog("Производится сохранение карточки Наряда на СО...");
                CardScript.SaveCard();
                CardScript.CardFrame.CardHost.CloseCards();

                WriteLog("Завершена обработка кнопки 'Передать на поверку'...");
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Согласовать ремонт".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgreeToRepair_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Согласовать ремонт'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                WriteLog("Текущее состояние наряда на СО = 'На согласовании'...");

                // ========== Проверка заполнения полей Наряда на СО ==========
                WriteLog("Начата проверка правильности заполнения полей...");
                //Если поле "Результат согласования" не заполнено, то дальнейшие действия прерываются.
                if (GetControlValue(RefServiceCard.Calibration.ResultOfConsensus) == null || GetControlValue(RefServiceCard.Calibration.ResultOfConsensus).ToString() == "")
                {
                    MyMessageBox.Show("Укажите в поле 'Результат согласования' отчет о согласовании ремонта с клиентом.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // ========== Определение исполнителей ремонта ==========
                    WriteLog("Начато определение исполнителей ремонта...");
                    // Определяем, кто выполняет ремонт прибора
                    string DeviceRepairPerformerPosition = "";
                    StaffEmployee DeviceRepairPerformer = null;
                    bool DeviceRepair = GetControlValue(RefServiceCard.Calibration.DeviceRepair) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                    bool AccessoriesRepair = GetControlValue(RefServiceCard.Calibration.AccessoriesRepair) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                    object Department = GetControlValue(RefServiceCard.Calibration.Department);
                    if (DeviceRepair)
                    {
                        if ((int)Department == 1)
                        { DeviceRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager; }
                        else
                        { DeviceRepairPerformerPosition = RefServiceCard.Roles.AdjastManager; }

                        try { DeviceRepairPerformer = Context.GetEmployeeByPosition(DeviceRepairPerformerPosition); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + DeviceRepairPerformerPosition + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Определен исполнитель ремонта прибора...");
                    }

                    // Определяем, кто выполняет ремонт комплектующих
                    string AccessoriesRepairPerformerPosition = "";
                    StaffEmployee AccessoriesRepairPerformer = null;
                    if (AccessoriesRepair)
                    {
                        AccessoriesRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager;
                        try { AccessoriesRepairPerformer = Context.GetEmployeeByPosition(AccessoriesRepairPerformerPosition); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.ProdactionManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Определен исполнитель ремонта комплектующих...");
                    }

                    // ========== Внесение изменений в текущий Наряд на СО ==========
                    WriteLog("Начато внесение изменений в текущий Наряд на СО...");
                    double AgreedRepairCost = 0;
                    foreach (BaseCardProperty Row in Table_RepairWorks.Select())
                    {
                        Row[RefServiceCard.RepairWorks.NegotiationResult] = "Выполнить";
                        double Cost = UniversalCard.GetItemPropertyValue(Row[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)") == null ? 0 : 
                            Convert.ToDouble(UniversalCard.GetItemPropertyValue(Row[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)"));
                        AgreedRepairCost = AgreedRepairCost + Cost;
                    }
                    SetControlValue(RefServiceCard.Adjustment.AgreedRepairCost, AgreedRepairCost);

                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Adjustment);
                    if (DeviceRepair)
                    {
                        SetControlValue(RefServiceCard.Adjustment.RepDateStart, DateTime.Now);
                        SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(DeviceRepairPerformer).Id);
                    }
                    // 
                    if (AccessoriesRepair)
                    {
                        SetControlValue(RefServiceCard.Adjustment.AccDateStart, DateTime.Now);
                        SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(AccessoriesRepairPerformer).Id);
                    }

                    CardScript.ChangeState("Adjustment");

                    WriteLog("Изменения в текущий Наряд на СО внесены...");

                    MyMessageBox.Show("Наряд на СО успешно отправлен в ремонт.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                    //CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();

                }
            }
            catch (Exception Ex)
            {
                CallError(Ex);
            }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Отказ от ремонта".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FailureToRepair_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Отказ от ремонта'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                WriteLog("Текущее состояние наряда на СО = 'На согласовании'...");

                // ========== Проверка заполнения полей Наряда на СО ==========
                WriteLog("Начата проверка правильности заполнения полей...");
                //Если группа полей "Требуется вид сервиса" заполнена неверно, то дальнейшие действия прерываются.
                if (GetControlValue(RefServiceCard.Calibration.ResultOfConsensus) == null || GetControlValue(RefServiceCard.Calibration.ResultOfConsensus).ToString() == "")
                {
                    MyMessageBox.Show("Укажите в поле 'Результат согласования' отчет о согласовании ремонта с клиентом.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // ========== Определение исполнителя диагностики ==========
                    WriteLog("Начато определение исполнителя диагностики...");
                    StaffEmployee FAManager = null;
                    try { FAManager = Context.GetEmployeeByPosition(RefServiceCard.Roles.AdjastManager); }
                    catch
                    {
                        MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.AdjastManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ========== Внесение изменений в текущий Наряд на СО ==========
                    WriteLog("Начато внесение изменений в текущий Наряд на СО...");

                    foreach (BaseCardProperty Row in Table_RepairWorks.Select())
                        Row[RefServiceCard.RepairWorks.NegotiationResult] = "Не выполнять";

                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Failure);
                    SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, Context.GetObjectRef(FAManager).Id);
                    SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, null);
                    WriteLog("Начата ние изменений в текущий Наряд на СО...");
                    CardScript.ChangeState("Failure");
                    SendToSale((int)GetControlValue(RefServiceCard.MainInfo.Status));                 

                    WriteLog("Изменения в текущий Наряд на СО внесены...");

                    MyMessageBox.Show("Наряд на СО завершен, приборы/комплектующие будут переданы в отдел сбыта для отправки клиенту.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                    //CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();

                }
            }
            catch (Exception Ex)
            {
                CallError(Ex);
            }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Согласовать частично".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartiallyAgreeToRepair_ItemClick(Object sender, ItemClickEventArgs e)
        {
            WriteLog("Начата обработка кнопки 'Согласовать ремонт'...");
            try
            {
                WriteLog("Определение текущего состояния Наряда на СО...");
                int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                WriteLog("Текущее состояние наряда на СО = 'На согласовании'...");

                // ========== Проверка заполнения полей Наряда на СО ==========
                WriteLog("Начата проверка правильности заполнения полей...");
                //Если поле "Результат согласования" не заполнено, то дальнейшие действия прерываются.
                if (GetControlValue(RefServiceCard.Calibration.ResultOfConsensus) == null || GetControlValue(RefServiceCard.Calibration.ResultOfConsensus).ToString() == "")
                {
                    MyMessageBox.Show("Укажите в поле 'Результат согласования' отчет о согласовании ремонта с клиентом.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    foreach (BaseCardProperty Row in Table_RepairWorks.Select())
                    {
                        if (Row[RefServiceCard.RepairWorks.NegotiationResult].ToString() == "Не согласовано")
                        {
                            MyMessageBox.Show("В таблице 'Описание неисправностей' (которая расположена на вкладке 'Настройки') для каждой ремонтной работы укажите требуемое действие: 'Выполнить ремонт'/'Не выполнять ремонт'.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // ========== Определение исполнителей ремонта ==========
                    WriteLog("Начато определение исполнителей ремонта...");
                    // Определяем, кто выполняет ремонт прибора
                    string DeviceRepairPerformerPosition = "";
                    StaffEmployee DeviceRepairPerformer = null;
                    bool DeviceRepair = GetControlValue(RefServiceCard.Calibration.DeviceRepair) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair);
                    bool AccessoriesRepair = GetControlValue(RefServiceCard.Calibration.AccessoriesRepair) == null ? false : (bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair);
                    object Department = GetControlValue(RefServiceCard.Calibration.Department);
                    if (DeviceRepair)
                    {
                        if ((int)Department == 1)
                        { DeviceRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager; }
                        else
                        { DeviceRepairPerformerPosition = RefServiceCard.Roles.AdjastManager; }

                        try { DeviceRepairPerformer = Context.GetEmployeeByPosition(DeviceRepairPerformerPosition); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + DeviceRepairPerformerPosition + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Определен исполнитель ремонта прибора...");
                    }

                    // Определяем, кто выполняет ремонт комплектующих
                    string AccessoriesRepairPerformerPosition = "";
                    StaffEmployee AccessoriesRepairPerformer = null;
                    if (AccessoriesRepair)
                    {
                        AccessoriesRepairPerformerPosition = RefServiceCard.Roles.ProdactionManager;
                        try { AccessoriesRepairPerformer = Context.GetEmployeeByPosition(AccessoriesRepairPerformerPosition); }
                        catch
                        {
                            MyMessageBox.Show("Не удалось найти сотрудника в должности \"" + RefServiceCard.Roles.ProdactionManager + "\". Обратитесь к Администратору системы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        WriteLog("Определен исполнитель ремонта комплектующих...");
                    }

                    // ========== Внесение изменений в текущий Наряд на СО ==========
                    WriteLog("Начато внесение изменений в текущий Наряд на СО...");

                    double AgreedRepairCost = 0;
                    foreach (BaseCardProperty Row in Table_RepairWorks.Select())
                    {
                        if (Row[RefServiceCard.RepairWorks.NegotiationResult].ToString() == "Выполнить")
                        {
                            double Cost = UniversalCard.GetItemPropertyValue(Row[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)") == null ? 0 :
                                Convert.ToDouble(UniversalCard.GetItemPropertyValue(Row[RefServiceCard.RepairWorks.WorksTypeID].ToGuid(), "Стоимость (руб/шт)"));
                            AgreedRepairCost = AgreedRepairCost + Cost;
                        }
                    }
                    SetControlValue(RefServiceCard.Adjustment.AgreedRepairCost, AgreedRepairCost);

                    SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Adjustment);
                    if (DeviceRepair)
                    {
                        SetControlValue(RefServiceCard.Adjustment.RepDateStart, DateTime.Now);
                        SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(DeviceRepairPerformer).Id);
                    }
                    // 
                    if (AccessoriesRepair)
                    {
                        SetControlValue(RefServiceCard.Adjustment.AccDateStart, DateTime.Now);
                        SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(AccessoriesRepairPerformer).Id);
                    }

                    CardScript.ChangeState("Adjustment");

                    WriteLog("Изменения в текущий Наряд на СО внесены...");

                    MyMessageBox.Show("Наряд на СО успешно отправлен в ремонт.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //WriteLog("Начато сохранение изменений в текущем Наряде на СО...");
                    //CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();

                }
            }
            catch (Exception Ex)
            {
                CallError(Ex);
            }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Завершить".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Complete_ItemClick(Object sender, ItemClickEventArgs e)
        {
            int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
            switch (CurrentState)
            {
                case (int)RefServiceCard.MainInfo.State.Failure:
                    SetControlValue(RefServiceCard.Adjustment.DiagnosticDateEnd, DateTime.Now);
                    CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();
                    break;
                case (int)RefServiceCard.MainInfo.State.Payment:
                    if (NeedVerificationProtocol())
                    {
                        MyMessageBox.Show("Для данного прибора заказана поверка, которая производится на территории СКБ ЭП. Отправьте прибор на поверку, если клиент уже произвел оплату.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        SendToSale(CurrentState);
                        SetControlValue(RefServiceCard.MainInfo.Status, RefServiceCard.MainInfo.State.Completed);
                        SetControlValue(RefServiceCard.MainInfo.DateEndFact, DateTime.Now);
                        CardScript.ChangeState("Completed");
                        CardScript.CardFrame.CardHost.CloseCards();
                    }
                    break;
            }
        }
        /// <summary>
        /// Обработчик события клика по кнопке "Делегировать".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delegate_ItemClick(Object sender, ItemClickEventArgs e)
        {
            try
            {
                // ====================================
                // === Осуществляется делегирование ===
                // ====================================

                Forms.CertificateCreationCard.DelegateForm NewDelegateForm = new Forms.CertificateCreationCard.DelegateForm(CardScript.CardFrame.CardHost, Context);
                NewDelegateForm.ShowDialog();
                if (NewDelegateForm.DialogResult == DialogResult.OK)
                {
                    int CurrentState = (int)GetControlValue(RefServiceCard.MainInfo.Status);
                    switch (CurrentState)
                    {
                        case (int)RefServiceCard.MainInfo.State.Maintenance:
                            SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Diagnostics:
                            SetControlValue(RefServiceCard.Adjustment.DiagnosticPerformer, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Consensus:
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Adjustment:
                            // ========== Определение исполнителей ремонтных работ ==========
                            WriteLog("Начато определение исполнителей ремонтных работ...");
                            Guid DeviceRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Adjuster) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Adjuster).ToGuid();
                            Guid AccessoriesRepairPerformerID = GetControlValue(RefServiceCard.Adjustment.Worker) == null ? Guid.Empty : GetControlValue(RefServiceCard.Adjustment.Worker).ToGuid();
                            // Если текущий пользователь - исполнитель ремонта приборов и ремонта комплектующих
                            if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id) && AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                            {
                                SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                                SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            }
                            else
                            {
                                // Если текущий пользователь - исполнитель ремонта прибора
                                if (DeviceRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                                {
                                    SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                                }
                                // Если текущий пользователь - исполнитель ремонта комплектующих
                                if (AccessoriesRepairPerformerID.Equals(Context.GetObjectRef(Context.GetCurrentEmployee()).Id))
                                {
                                    SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                                }
                                // Если текущий пользователь - админ
                                if (IsAdmin)
                                {
                                    if ((bool)GetControlValue(RefServiceCard.Calibration.DeviceRepair))
                                        SetControlValue(RefServiceCard.Adjustment.Adjuster, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                                    if ((bool)GetControlValue(RefServiceCard.Calibration.AccessoriesRepair))
                                        SetControlValue(RefServiceCard.Adjustment.Worker, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                                }
                            }
                            break;
                        case (int)RefServiceCard.MainInfo.State.Remaintenance:
                            SetControlValue(RefServiceCard.Calibration.Calibrator, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Calibration:
                            SetControlValue(RefServiceCard.Calibration.CalibrationPerformer, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Payment:
                            SetControlValue(RefServiceCard.MainInfo.Manager, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                        case (int)RefServiceCard.MainInfo.State.Verification:
                            SetControlValue(RefServiceCard.Calibration.VerificationPerformer, Context.GetObjectRef(NewDelegateForm.Delegate).Id);
                            break;
                    }
                    CardScript.SaveCard();
                    CardScript.CardFrame.CardHost.CloseCards();
                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }


        #endregion

        #region События карточки
        /// <summary>
        /// Обработчик закрытия карточки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_CardClosed(Object sender, EventArgs e)
        {
            try
            {
                /* Отвязка методов */
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].ItemClick -= SendToAdjustment_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToCalibrate].ItemClick -= SendToCalibrate_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToSale].ItemClick -= SendToSale_ItemClick;
                CardScript.CardControl.Saving -= CardControl_Saving;
                CardScript.CardControl.Saved -= CardControl_Saved;
                CardScript.CardControl.CardClosed -= CardControl_CardClosed;


                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAdjustment].ItemClick -= SendToAdjustment_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToCalibrate].ItemClick -= SendToCalibrate_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToSale].ItemClick -= SendToSale_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.SendToAgreement].ItemClick -= SendToAgreement_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.ActOfLoad].ItemClick -= ActOfLoad_ItemClick;
                ICardControl.RibbonControl.Items[RefServiceCard.RibbonItems.Return].ItemClick -= Return_ItemClick;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.OpenSensors).Click -= OpenSensors_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshProtocol).Click -= RefreshProtocol_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshCertificate).Click -= RefreshCertificate_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateProtocol).Click -= CreateProtocol_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateCertificate).Click -= CreateCertificate_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationProtocol).Click -= RefreshVerificationProtocol_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.RefreshVerificationCertificate).Click -= RefreshVerificationCertificate_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationProtocol).Click -= CreateVerificationProtocol_Click;
                ICardControl.FindPropertyItem<SimpleButton>(RefServiceCard.Buttons.CreateVerificationCertificate).Click -= CreateVerificationCertificate_Click;

                CardScript.CardControl.Saving -= CardControl_Saving;
                CardScript.CardControl.Saved -= CardControl_Saved;
                CardScript.CardControl.CardClosed -= CardControl_CardClosed;

                ICardControl.RemoveTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4);
                ICardControl.RemoveTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 3);
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик события сохранения карточки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_Saved(Object sender, EventArgs e)
        {
            try
            {
                string Description = "Наряд на СО";
                if ((bool)GetControlValue(RefServiceCard.MainInfo.OnlyA) == true)
                {
                    Description = Description + " комплектующих для " + UniversalCard.GetItemName(GetControlValue(RefServiceCard.MainInfo.DeviceType).ToGuid()) + " от " + ((DateTime)GetControlValue(RefServiceCard.MainInfo.RegistrationDate)).ToShortDateString();
                }
                else
                {
                    Description = Description + " " + CardScript.Session.CardManager.GetCardData(GetControlValue(RefServiceCard.MainInfo.DeviceCardID).ToGuid()).Description + " от " + ((DateTime)GetControlValue(RefServiceCard.MainInfo.RegistrationDate)).ToShortDateString();
                }
                CardScript.UpdateDescription(Description);
                WriteLog("Сохранение карточки завершено. Текущее состояние заявки: " + GetControlValue(RefApplicationCard.MainInfo.Status).ToString());
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик события сохранения карточки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_Saving(Object sender, CancelEventArgs e)
        {
            try
            {
                if (SaveChanges == true)
                {
                    UpdateRepairWorks();
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        #endregion

        #region События таблиц карточки
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Описание неисправностей".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDescriptionOfFaultButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                ArrayList NewWorks = new ArrayList();
                //string DeviceType = GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString();

                WriteLog("Выбираем изделие");
                string DeviceID = "";
                string DeviceType = "";
                string DeviceNumber = "";

                ArrayList DevicesList = FindAllDevices();
                if (DevicesList.Count == 0)
                    return;
                if (DevicesList.Count == 1)
                {
                    WriteLog("Изделий для выбора: 1");
                    DeviceID = DevicesList[0].ToString();
                    if (DeviceID == "")
                    {
                        DeviceType = GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString();
                        DeviceNumber = DeviceType;
                    }
                    else
                    {
                        CardData DeviceCard = CardScript.Session.CardManager.GetCardData(new Guid(DeviceID));
                        DeviceType = CalibrationDocs.CalibrationLib.GetDeviceTypeID(DeviceCard);
                        DeviceNumber = UniversalCard.GetItemName(new Guid(DeviceType)) + " " + CalibrationDocs.CalibrationLib.GetDeviceNumber(DeviceCard);
                    }
                }
                else
                {
                    WriteLog("Изделий для выбора: " + DevicesList.Count.ToString());
                    DeviceList DLForm = new DeviceList(this, UniversalCard, DevicesList);
                    if (DLForm.ShowDialog() == DialogResult.OK)
                    {
                        DeviceID = DLForm.DevicePassportID;
                        DeviceType = DLForm.DeviceTypeID;
                        DeviceNumber = DLForm.DeviceNumberValue;
                    }
                    else
                    {
                        Table_Description.RemoveRow(CardScript.BaseObject, Table_Description.RowCount - 1);
                        return;
                    }
                }

                RepairsAndImprovements RForm = new RepairsAndImprovements(this, UniversalCard, DeviceID, DeviceNumber, DeviceType, "", "", "", "", "", NewWorks);
                RForm.ShowDialog();
                if (RForm.Acceptance == true)
                {
                    BaseCardProperty NewRow = Table_Description.AddRow(CardScript.BaseObject);
                    //BaseCardProperty Row = Table_Description[Table_Description.RowCount - 1];
                    NewRow[RefServiceCard.DescriptionOfFault.BlockOfDevice] = RForm.Block;
                    NewRow[RefServiceCard.DescriptionOfFault.BlockOfDeviceID] = RForm.BlockID;
                    NewRow[RefServiceCard.DescriptionOfFault.FaultСlassification] = RForm.Kind;
                    NewRow[RefServiceCard.DescriptionOfFault.FaultСlassificationID] = RForm.KindID;
                    NewRow[RefServiceCard.DescriptionOfFault.Description] = RForm.Description;
                    NewRow[RefServiceCard.DescriptionOfFault.CorrectiveActions] = RForm.WayToSolve;
                    NewRow[RefServiceCard.DescriptionOfFault.RepairWorksList] = RForm.WorksList;
                    NewRow[RefServiceCard.DescriptionOfFault.Comment] = RForm.Comments;
                    NewRow[RefServiceCard.DescriptionOfFault.Id] = Guid.NewGuid();
                    NewRow[RefServiceCard.DescriptionOfFault.SerialNumber] = RForm.DeviceNumber;
                    NewRow[RefServiceCard.DescriptionOfFault.SerialNumberID] = RForm.DeviceID;
                    NewRow[RefServiceCard.DescriptionOfFault.Replacement] = RForm.Replacement;
                    NewRow[RefServiceCard.DescriptionOfFault.ReplacementID] = RForm.ReplacementID;
                    NewRow[RefServiceCard.DescriptionOfFault.OldProductAction] = RForm.OldProductAction;
                    Table_Description.RefreshRows();
                    UpdateRepairWorks(NewRow[RefServiceCard.DescriptionOfFault.Id].ToGuid(), RForm.Works);
                    ICardControl.DisableTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4, false);
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Описание неисправностей".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDescriptionOfFaultButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                Table_Description.RemoveRow(CardScript.BaseObject, Table_Description.FocusedRowItem);
                if (Table_Description.RowCount == 0) ICardControl.DisableTableBarItem(RefServiceCard.DescriptionOfFault.Alias, 4, true);
                SaveChanges = true;
            }
            catch { }
        }
        /// <summary>
        /// Обработчик двойного клика по строке таблицы "Описание неисправностей".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Description_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                BaseCardProperty Row = Table_Description[Table_Description.FocusedRowIndex];
                ArrayList List = new ArrayList();

                if ((Row[RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() != "") && (Row[RefServiceCard.DescriptionOfFault.RepairWorksList].ToString() != null))
                {
                    List = GetWorks(Row[RefServiceCard.DescriptionOfFault.Id].ToGuid());
                }

                // Если не указан соответствующий прибор
                if (Row[RefServiceCard.DescriptionOfFault.SerialNumberID] == null)
                {
                    ArrayList DevicesList = FindAllDevices();
                    if (DevicesList.Count == 0)
                    {
                        XtraMessageBox.Show("Не найдено ни одного прибора для выбора.");
                        return;
                    }
                    else 
                    {
                        WriteLog("Изделий для выбора: " + DevicesList.Count.ToString());
                        DeviceList DLForm = new DeviceList(this, UniversalCard, DevicesList);
                        if (DLForm.ShowDialog() == DialogResult.OK)
                        {
                            Row[RefServiceCard.DescriptionOfFault.SerialNumberID] = DLForm.DevicePassportID;
                            Row[RefServiceCard.DescriptionOfFault.SerialNumber] = DLForm.DeviceNumberValue;
                            Row[RefServiceCard.DescriptionOfFault.ReplacementID] = "";
                            Row[RefServiceCard.DescriptionOfFault.Replacement] = "";
                            Row[RefServiceCard.DescriptionOfFault.OldProductAction] = "";
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                string DeviceType = Row[RefServiceCard.DescriptionOfFault.SerialNumberID].ToString() == "" ? GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString() :
                    CalibrationDocs.CalibrationLib.GetDeviceTypeID(CardScript.Session.CardManager.GetCardData(new Guid(Row[RefServiceCard.DescriptionOfFault.SerialNumberID].ToString())));
                RepairsAndImprovements RForm = new RepairsAndImprovements(this, UniversalCard, Row[RefServiceCard.DescriptionOfFault.SerialNumberID].ToString(),
                    Row[RefServiceCard.DescriptionOfFault.SerialNumber].ToString(), DeviceType, Row[RefServiceCard.DescriptionOfFault.BlockOfDeviceID].ToString(),
                    Row[RefServiceCard.DescriptionOfFault.FaultСlassificationID].ToString(), Row[RefServiceCard.DescriptionOfFault.Description].ToString(),
                    Row[RefServiceCard.DescriptionOfFault.CorrectiveActions].ToString(), Row[RefServiceCard.DescriptionOfFault.Comment].ToString(), List,
                    Row[RefServiceCard.DescriptionOfFault.ReplacementID].ToString(), Row[RefServiceCard.DescriptionOfFault.OldProductAction].ToString());
                RForm.ShowDialog();
                if (RForm.Acceptance == true)
                {
                    BaseCardProperty EditRow = Table_Description[Table_Description.FocusedRowIndex];
                    EditRow[RefServiceCard.DescriptionOfFault.BlockOfDevice] = RForm.Block;
                    EditRow[RefServiceCard.DescriptionOfFault.BlockOfDeviceID] = RForm.BlockID;
                    EditRow[RefServiceCard.DescriptionOfFault.FaultСlassification] = RForm.Kind;
                    EditRow[RefServiceCard.DescriptionOfFault.FaultСlassificationID] = RForm.KindID;
                    EditRow[RefServiceCard.DescriptionOfFault.Description] = RForm.Description;
                    EditRow[RefServiceCard.DescriptionOfFault.CorrectiveActions] = RForm.WayToSolve;
                    EditRow[RefServiceCard.DescriptionOfFault.RepairWorksList] = RForm.WorksList;
                    EditRow[RefServiceCard.DescriptionOfFault.Comment] = RForm.Comments;
                    Row[RefServiceCard.DescriptionOfFault.SerialNumber] = RForm.DeviceNumber;
                    Row[RefServiceCard.DescriptionOfFault.SerialNumberID] = RForm.DeviceID;
                    Row[RefServiceCard.DescriptionOfFault.Replacement] = RForm.Replacement;
                    Row[RefServiceCard.DescriptionOfFault.ReplacementID] = RForm.ReplacementID;
                    Row[RefServiceCard.DescriptionOfFault.OldProductAction] = RForm.OldProductAction;
                    Table_Description.RefreshRow(Table_Description.FocusedRowIndex);
                    UpdateRepairWorks(Row[RefServiceCard.DescriptionOfFault.Id].ToGuid(), RForm.Works);
                }
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        #endregion

        #endregion


    }
}