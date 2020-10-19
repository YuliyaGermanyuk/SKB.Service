using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DocsVision.Platform.CardHost;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.ObjectModel;
using DocsVision.TakeOffice.Cards.Constants;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.ObjectModel.Services;

using RKIT.MyMessageBox;

using SKB.Base;
using SKB.Base.Enums;
using SKB.PaymentAndShipment;
using SKB.PaymentAndShipment.Forms.AccountCard;
using SKB.PaymentAndShipment.Forms.CompleteCard;
using SKB.Base.Ref;
using SKB.Service.CalibrationDocs;

using CardOrd = DocsVision.TakeOffice.Cards.Constants.CardOrd;

namespace SKB.Service.Forms.CertificateCreationCard
{
    /// <summary>
    /// Форма редактирования таблицы "Приборы".
    /// </summary>
    public partial class DevicesForm : XtraForm
    {
        /// <summary>
        /// Дата проведения калибровки.
        /// </summary>
        public DateTime CalibrationDate
        {
            get { return Edit_CalibrationDate.DateTime; }
            set { Edit_CalibrationDate.DateTime = value; }
        }
        /// <summary>
        /// Дата поступления на клаибровку.
        /// </summary>
        public DateTime ReceiptDate
        { 
            get { return Edit_ReceiptDate.DateTime; } 
            set { Edit_ReceiptDate.DateTime = value; } 
        }
        /// <summary>
        /// Данные таблицы "Дополнительные изделия".
        /// </summary>
        public BindingList<AdditionalWaresRow> DataList = new BindingList<AdditionalWaresRow>();
        /// <summary>
        /// Родительская карточка.
        /// </summary>
        MyBaseCard BaseCard;
        /// <summary>
        /// Объектный контекст.
        /// </summary>
        ObjectContext Context;
        /// <summary>
        /// Карточка универсального справочника.
        /// </summary>
        CardData UniversalCard;
        /// <summary>
        /// Хост.
        /// </summary>
        ICardHost Host;
        /// <summary>
        /// Занятые номера приборов.
        /// </summary>
        List<Guid> BusyDeviceNumberIds;
        /// <summary>
        /// Занятые номера доп. изделий.
        /// </summary>
        List<String> BusySensors;
        /// <summary>
        /// Дата отправки заявки на выдачу сертификатов о калибровке.
        /// </summary>
        DateTime SendDate;
        /// <summary>
        /// Тип универсального справочника "Приборы и комплектующие".
        /// </summary>
        RowData _Devices;
        /// <summary>
        /// Тип универсального справочника "Приборы и комплектующие".
        /// </summary>
        RowData Devices { get { return _Devices.IsNull() ? _Devices = UniversalCard.Sections[RefUniversal.ItemType.ID].GetRow(MyHelper.RefItem_Devices) : _Devices; } }
        /// <summary>
        /// Название типа прибора.
        /// </summary>
        String DeviceName
        {
            get { return this.Tag as String; }
            set { this.Text = "Выбранный прибор: " + value; this.Tag = value; }
        }
        /// <summary>
        /// Комментарий к прибору.
        /// </summary>
        String DeviceNumberTooltTip;
        /// <summary>
        /// Идентификатор типа прибора.
        /// </summary>
        public Guid DeviceId { get; set; }
        /// <summary>
        /// Выбранный номер прибора.
        /// </summary>
        public String DeviceNumber
        {
            get { return Edit_DeviceNumber.Text; }
            set { Edit_DeviceNumber.Text = value; }
        }
        /// <summary>
        /// Старый номер прибора.
        /// </summary>
        public String OldDeviceNumber
        {
            set
            {
                lci_Edit_DeviceNumber.Tag = value;
            }
            get { return lci_Edit_DeviceNumber.Tag as String; }
        }
        /// <summary>
        /// Идентификатор строки Универсального справочника с номером прибора.
        /// </summary>
        public Guid DeviceNumberId
        {
            get { return Edit_DeviceNumber.Tag.ToGuid(); }
            set
            {
                Edit_DeviceNumber.Tag = value;
                Edit_DeviceNumber.Properties.Buttons[0].Enabled = !value.IsEmpty();
            }
        }
        /// <summary>
        /// Идентификатор протокола калибровки прибора.
        /// </summary>
        public Guid ProtocolId
        {
            get { return Edit_CalibrationProtocol.Tag.ToGuid(); }
            set
            {
                Edit_CalibrationProtocol.Tag = value;
                Edit_CalibrationProtocol.Text = value.Equals(Guid.Empty) ? "" : BaseCard.CardScript.Session.CardManager.GetCardData(value).Description;
                Edit_CalibrationProtocol.Properties.Buttons[0].Enabled = !value.IsEmpty();
                Edit_CalibrationProtocol.Properties.ReadOnly = true;
                //MessageBox.Show("Протокол калибровки: " + value);
                Button_RefreshProtocol.Enabled = value.Equals(Guid.Empty) ? false : HasDocumentsAccess;
                Button_CreateProtocol.Enabled = value.Equals(Guid.Empty) ? HasDocumentsAccess : false;
            }
        }
        /// <summary>
        /// Идентификатор сертификата о калибровке прибора.
        /// </summary>
        public Guid CertificateId
        {
            get { return Edit_CalibrationCertificate.Tag.ToGuid(); }
            set
            {
                Edit_CalibrationCertificate.Tag = value;
                Edit_CalibrationCertificate.Text = value.Equals(Guid.Empty) ? "" : BaseCard.CardScript.Session.CardManager.GetCardData(value).Description;
                Edit_CalibrationCertificate.Properties.Buttons[0].Enabled = !value.IsEmpty();
                Edit_CalibrationCertificate.Properties.ReadOnly = true;
                Button_RefreshCertificate.Enabled = value.Equals(Guid.Empty) ? false : HasDocumentsAccess;
                Button_CreateCertificate.Enabled = value.Equals(Guid.Empty) ? HasDocumentsAccess : false;
            }
        }
        /// <summary>
        /// Только доп. комплектующие.
        /// </summary>
        public Boolean AC
        {
            set
            {
                // Назначение прав на данные о приборах
                if (value)
                    Edit_DeviceNumber.Enabled = !value;
                else
                {
                    Edit_DeviceNumber.Enabled = !value;
                    Edit_DeviceNumber.Properties.ReadOnly = !HasDeviceAccess;
                    Edit_DeviceNumber.Properties.Buttons[0].Enabled = true;
                }

                // Назначение прав на данные о документах
                //Edit_CalibrationProtocol.Enabled = value ? !value : !value && HasDocumentsAccess;
                Button_CreateProtocol.Enabled = value ? !value : !value && HasDocumentsAccess;
                Button_RefreshProtocol.Enabled = value ? !value : !value && HasDocumentsAccess && Edit_CalibrationProtocol.Text != "";

                //Edit_CalibrationCertificate.Enabled = value ? !value : !value && HasDocumentsAccess;
                Button_CreateCertificate.Enabled = value ? !value : !value && HasDocumentsAccess;
                Button_RefreshCertificate.Enabled = value ? !value : !value && HasDocumentsAccess && Edit_CalibrationCertificate.Text != "";

                // Очищение заводского номера прибора
                if (value &&  !String.IsNullOrEmpty(Edit_DeviceNumber.Text))
                {
                    DeviceNumber = null;
                    DeviceNumberId = Guid.Empty;
                    Edit_DeviceNumber.DoValidate();
                }

                Edit_AC.Checked = value;
            }
            get
            {
                return Edit_AC.Checked;
            }
        }
        /// <summary>
        /// Дополнительные изделия.
        /// </summary>
        public String AdditionalWares
        {
            get
            {
                return DataList.Count > 0 ? DataList.Select(r => r.WareName).Aggregate((a, b) => a + ";" + b) : null;
            }
        }
        /// <summary>
        /// Дополнительные изделия.
        /// </summary>
        public BindingList<AdditionalWaresRow> AdditionalWaresList
        {
            set
            {
                DataList.Clear();
                foreach(AdditionalWaresRow WaresRow in value)
                    DataList.Add(WaresRow);
                Control_Sensors.RefreshDataSource();
            }
            get
            {
                return DataList;
            }
        }
        /// <summary>
        /// Роль текущего польщователя.
        /// </summary>
        public String UserRole;
        /// <summary>
        /// Текущее состояние заявки.
        /// </summary>
        public int CurrentState;
        /// <summary>
        /// Пользователь имеет доступ к информации о приборах
        /// </summary>
        bool HasDeviceAccess;
        /// <summary>
        /// Пользователь имеет доступ к информации о документах
        /// </summary>
        bool HasDocumentsAccess;
        /// <summary>
        /// Серийный номер поверки
        /// </summary>
        public String VerifySerialNumber
        {
            get { return this.Edit_VerifySerialNumber.Text; }
            set { this.Edit_VerifySerialNumber.Text = value; }
        }

        /// <summary>
        /// Причины непригодности
        /// </summary>
        public String CausesOfUnfitness
        {
            get { return this.Edit_CausesOfUnfitness.Text; }
            set { this.Edit_CausesOfUnfitness.Text = value; }
        }

        bool IsCalibration;
        /// <summary>
        /// Инициализирует форму редактирования таблицы "Приборы".
        /// </summary>
        /// <param name="BaseCard"> Родительская карточка. </param>
        /// <param name="UniversalCard"> Данные Универсального справочника. </param>
        /// <param name="UserRole"> Роль текущего пользователя. </param>
        /// <param name="CurrentState"> Текущее состояние заявки. </param>
        /// <param name="DeviceId"> Идентификатор типа прибора. </param>
        /// <param name="SendDate"> Дата отправки заявки. </param>
        /// <param name="IsCalibration"> Калибровка (Да/Нет). </param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки. </param>
        /// <param name="BusyDeviceNumbers"> Занятые номера приборов. </param>
        /// <param name="BusySensors"> Занятые сенсоры. </param>
        /// <param name="CausesOfUnfitness"> Причины непригодности. </param>
        public DevicesForm(MyBaseCard BaseCard, CardData UniversalCard, String UserRole, int CurrentState, Guid DeviceId, DateTime SendDate, bool IsCalibration, String VerifySerialNumber, String CausesOfUnfitness, List<Guid> BusyDeviceNumbers = null, List<String> BusySensors = null)
        {
            InitializeComponent();
            this.BaseCard = BaseCard;
            this.Context = BaseCard.Context;
            this.UniversalCard = UniversalCard;
            this.Host = BaseCard.CardScript.CardFrame.CardHost;
            this.DeviceId = DeviceId;
            this.BusyDeviceNumberIds = BusyDeviceNumbers ?? new List<Guid>();
            this.BusySensors = BusySensors ?? new List<String>();
            this.SendDate = SendDate;
            this.UserRole = UserRole;
            this.CurrentState = CurrentState;
            this.IsCalibration = IsCalibration;
            this.VerifySerialNumber = VerifySerialNumber;
            this.CausesOfUnfitness = CausesOfUnfitness;

            // Определение названия и доступности полей
            if (IsCalibration)
            {
                this.lci_Edit_CreateProtocol.Text = "Протокол калибровки:";
                this.lci_Edit_CreateCertificate.Text = "Сертификат о калибровке:";
                this.lci_ReceiptDate.Text = "Дата поступления на калибровку:";
                this.lci_CalibrationDate.Text = "Дата проведения калибровки:";
                this.Edit_CausesOfUnfitness.Enabled = false;
                this.Edit_VerifySerialNumber.Enabled = false;
            }
            else
            {
                this.lci_Edit_CreateProtocol.Text = "Протокол поверки:";
                this.lci_Edit_CreateCertificate.Text = "Свидетельство о поверке/Извещение о непригодности:";
                this.lci_ReceiptDate.Text = "Дата поступления на поверку:";
                this.lci_CalibrationDate.Text = "Дата проведения поверки:";
                this.Edit_CausesOfUnfitness.Enabled = true;
                this.Edit_VerifySerialNumber.Enabled = true;
            }

            // Пользователь имеет доступ к информации о приборах
            HasDeviceAccess = (UserRole.Equals(RefCertificateCreationCard.UserRoles.Admin)) ||
                                   (UserRole.Equals(RefCertificateCreationCard.UserRoles.Creator) && CurrentState <= (int)RefCertificateCreationCard.MainInfo.CardState.InWork) ||
                                   (UserRole.Equals(RefCertificateCreationCard.UserRoles.SalesDepartmentEmployee) && CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.NotStarted);
            // Пользователь имеет доступ к информации о документах
            HasDocumentsAccess = (UserRole.Equals(RefCertificateCreationCard.UserRoles.Admin)) ||
                                      (UserRole.Equals(RefCertificateCreationCard.UserRoles.Performer) && CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.InWork) ||
                                      (UserRole.Equals(RefCertificateCreationCard.UserRoles.FAManager) && CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.InWork);

            Control_Sensors.DataSource = DataList;

            if (DeviceId.IsEmpty())
            {
                if (!GetDevice())
                    throw new MyException(0);
            }
            else
                DeviceName = UniversalCard.GetItemName(DeviceId);
            SettingFilds();
        }

        #region Methods

        private void SettingFilds()
        {
            // Инфомация о приборах
            Edit_DeviceNumber.Properties.ReadOnly = !HasDeviceAccess;
            Edit_DeviceNumber.Properties.Buttons[0].Enabled = true;
            //Edit_DeviceNumber.Enabled = HasDeviceAccess;
            Button_ChangeType.Enabled = HasDeviceAccess;
            Button_Check.Enabled = HasDeviceAccess;
            View_Sensors.Columns["WareName"].OptionsColumn.ReadOnly = !HasDeviceAccess;
            Item_AddSensor.Enabled = HasDeviceAccess;
            Item_RemoveSensor.Enabled = HasDeviceAccess;

            // Информация о документах
            //Edit_CalibrationProtocol.Enabled = HasDocumentsAccess;
            //Edit_CalibrationCertificate.Enabled = HasDocumentsAccess;
            Button_CreateProtocol.Enabled = HasDocumentsAccess;
            Button_CreateCertificate.Enabled = HasDocumentsAccess;
            //Button_RefreshProtocol.Enabled = HasDocumentsAccess;
            //Button_RefreshCertificate.Enabled = HasDocumentsAccess;
            Item_CreateProtocol.Enabled = HasDocumentsAccess;
            Item_CreateCertificate.Enabled = HasDocumentsAccess;

            if (!IsCalibration)
            {
                Edit_CalibrationCertificate.Width = Edit_CalibrationProtocol.Width - Edit_VerifySerialNumber.Width;
                Edit_VerifySerialNumber.Visible = true;
            }
            else
            {
                Edit_CalibrationCertificate.Width = Edit_CalibrationProtocol.Width;
                Edit_VerifySerialNumber.Visible = false;
            }
        }
        /// <summary>
        /// Проверка.
        /// </summary>
        private Boolean Check (Boolean Show)
        {
            BaseCard.WriteLog("Проверка прибора.");
            // Проверка прибора
            Boolean Check = true;
            if (!Edit_AC.Checked && !String.IsNullOrEmpty(Edit_DeviceNumber.Text))
            {
                if (DeviceNumberId.IsEmpty())
                {
                    Edit_DeviceNumber.ErrorText = DeviceNumberTooltTip;
                    Edit_DeviceNumber.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
                    Check = false;
                }
                if (!Check || Show)
                {
                    ToolTip t = new ToolTip();
                    t.ToolTipTitle = DeviceNumber;
                    if (DeviceNumberId.IsEmpty())
                    {
                        t.ToolTipIcon = ToolTipIcon.Error;
                    }
                    else
                    {
                        t.ToolTipIcon = ToolTipIcon.Info;
                    }
                    t.Show(DeviceNumberTooltTip, Edit_DeviceNumber, 3000);
                }
            }
            else
            {
                DeviceNumber = "";
                DeviceNumberId = Guid.Empty;
                DeviceNumberTooltTip = "";
                Edit_DeviceNumber.ErrorText = "";
            }

            // Проверка доп. изделий
            for (Int32 i = 0; i < DataList.Count; i++)
            {
                DataList[i].Validate();
            }
            View_Sensors.RefreshData();
            if (DataList.Any(sensor => !sensor.Valid))
            {
                Int32 i = DataList.IndexOf(DataList.First(sensor => !sensor.Valid));
                ToolTipControllerShowEventArgs Args = Controller_ToolTip.CreateShowArgs();
                Args.SelectedControl = Control_Sensors;
                Args.ToolTipType = ToolTipType.SuperTip;
                Args.SuperTip = new SuperToolTip();
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = DataList[i].WareName ?? "<Пусто>";

                toolTipArgs.Contents.Text = DataList[i].ToolTip;
                Args.SuperTip.Setup(toolTipArgs);
                Args.IconType = DataList[i].Valid ? ToolTipIconType.Information : ToolTipIconType.Error;
                GridRowInfo RowInfo = (View_Sensors.GetViewInfo() as GridViewInfo).GetGridRowInfo(i);
                if (RowInfo.IsNull())
                    Controller_ToolTip.ShowHint(Args, Control_Sensors);
                else
                    Controller_ToolTip.ShowHint(Args, Control_Sensors.GetLocation() + (Size)RowInfo.TotalBounds.Location + RowInfo.TotalBounds.Size);
                return false;
            }
            BaseCard.WriteLog("Завершили проверку. Результат: " + Check);
            return Check;
        }
        /// <summary>
        /// Выбор прибора из справочника.
        /// </summary>
        private Boolean GetDevice ()
        {
            Object[] activateParams = new Object[] { 
                RefUniversal.Item.ID.ToString("B").ToUpper(), 
                String.Empty, 
                MyHelper.RefItem_Devices.ToString("B").ToUpper(), 
                false, String.Empty, false };
            Object Id = Host.SelectFromCard(RefUniversal.ID, "Выберите прибор...", activateParams);

            if (Id.IsNull())
                return false;

            RowData DeviceItemRow = Devices.ChildSections[RefUniversal.Item.ID].Rows[Id.ToGuid()];
            if (!DeviceId.Equals(DeviceItemRow.Id))
            {
                DeviceId = DeviceItemRow.Id;
                DeviceName = DeviceItemRow.GetString("Name");
                DeviceNumber = null;
                DeviceNumberId = Guid.Empty;
                Edit_DeviceNumber.DoValidate();
            }
            AC = IsCalibration ? MyMessageBox.Show("Выписать сертификат только на доп. изделия (без прибора)?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes : false;
            
            return true;
        }
        /// <summary>
        /// Открыть карточку "Паспорт прибора".
        /// </summary>
        private void OpenPassport (String FindNumber, String Device)
        {
            CardData Passport = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(FindNumber, Device);
            if (!Passport.IsNull())
                Host.ShowCardModal(Passport.Id, ActivateMode.Edit);
        }
        /// <summary>
        /// Удалить доп. изделие.
        /// </summary>
        private void RemoveAdditionalWare ()
        {
            if (View_Sensors.FocusedRowHandle >= 0)
                if (!String.IsNullOrWhiteSpace(DataList[View_Sensors.FocusedRowHandle].WareName))
                {
                    if (MyMessageBox.Show("Удалить изделие «" + DataList[View_Sensors.FocusedRowHandle].WareName + "»?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        View_Sensors.DeleteRow(View_Sensors.FocusedRowHandle);
                }
                else
                    View_Sensors.DeleteRow(View_Sensors.FocusedRowHandle);
        }

        #endregion
        /// <summary>
        /// Сообщения.
        /// </summary>
        private void Controller_ToolTip_GetActiveObjectInfo (Object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == Control_Sensors)
            {
                GridHitInfo hitInfo = View_Sensors.CalcHitInfo(e.ControlMousePosition);
                if (hitInfo.RowHandle >= 0 && DataList.Count > 0)
                {
                    SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                    toolTipArgs.Title.Text = DataList[hitInfo.RowHandle].WareName;
                    toolTipArgs.Contents.Text = DataList[hitInfo.RowHandle].ToolTip;
                    if (!String.IsNullOrEmpty(toolTipArgs.Contents.Text))
                    {
                        e.Info = new ToolTipControlInfo();
                        e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
                        e.Info.ToolTipType = ToolTipType.SuperTip;
                        e.Info.SuperTip = new SuperToolTip();
                        e.Info.SuperTip.Setup(toolTipArgs);
                        e.Info.IconType = DataList[hitInfo.RowHandle].Valid ? ToolTipIconType.Information : ToolTipIconType.Error;
                    }
                }
            }
            else if (e.SelectedControl == Edit_DeviceNumber)
            {
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = DeviceNumber;
                toolTipArgs.Contents.Text = DeviceNumberTooltTip;
                if (!String.IsNullOrEmpty(toolTipArgs.Contents.Text))
                {
                    e.Info = new ToolTipControlInfo();
                    e.Info.Object = Edit_DeviceNumber.ToString();
                    e.Info.ToolTipType = ToolTipType.SuperTip;
                    e.Info.SuperTip = new SuperToolTip();
                    e.Info.SuperTip.Setup(toolTipArgs);
                    e.Info.IconType = DeviceNumberId.IsEmpty() ? ToolTipIconType.Error : ToolTipIconType.Information;
                }
            }
        }
        /// <summary>
        /// Кнопка открытия паспорта прибора.
        /// </summary>
        private void Edit_DeviceNumber_ButtonPressed (Object sender, ButtonPressedEventArgs e)
        {
            switch (e.Button.Kind)
            {
                case ButtonPredefines.Redo:
                    OpenPassport(DeviceNumber, DeviceName);
                    break;
            }
        }
        /// <summary>
        /// Проверка правильности заполнени поля "Заводской номер прибора"
        /// </summary>
        private void Edit_DeviceNumber_Validating (Object sender, CancelEventArgs e)
        {

            if (!Edit_AC.Checked && !String.IsNullOrEmpty(Edit_DeviceNumber.Text))
            {
                RowData Row;
                DeviceNumberId = Guid.Empty;

                Int32 k = Cards.ApplicationCard.UniversalCard.VerifyDeviceNumber(out Row, DeviceNumber, DeviceName, OldDeviceNumber != DeviceNumber, DeviceState.Stocking, DeviceState.Bundling);
                
                switch (k)
                {
                    case -1:
                        if (this.UserRole == RefCertificateCreationCard.UserRoles.Admin)
                        {
                            if (!BusyDeviceNumberIds.Contains(Row.Id))
                            {
                                DeviceNumberTooltTip = Row.GetString("Name");
                                DeviceNumberId = Row.Id;
                                if (OldDeviceNumber != DeviceNumber)
                                {
                                    ProtocolId = Guid.Empty;
                                    CertificateId = Guid.Empty;
                                }
                            }
                            else
                            {
                                DeviceNumberTooltTip = "Прибор с указанным номером должен быть в состоянии «На складе» или «На комплектации»!";
                                Edit_DeviceNumber.Properties.Buttons[0].Enabled = true;
                            }
                        }
                        break;
                    case 0: DeviceNumberTooltTip = "Прибор с указанным номером не существует!"; break;
                    case 1:
                        if (!BusyDeviceNumberIds.Contains(Row.Id))
                        {
                            DeviceNumberTooltTip = Row.GetString("Name");
                            DeviceNumberId = Row.Id;
                            if (OldDeviceNumber != DeviceNumber)
                            {
                                ProtocolId = Guid.Empty;
                                CertificateId = Guid.Empty;
                            }
                        }
                        else
                            DeviceNumberTooltTip = "Прибор с указанным номером уже указан в данной карточке!";
                        break;
                    default: DeviceNumberTooltTip = "Обнаружена неоднозначность (по запросу обнаружено несколько записей)!"; break;
                }
            }
            else
            {
                DeviceNumber = "";
                DeviceNumberId = Guid.Empty;
                DeviceNumberTooltTip = "";
                Edit_DeviceNumber.ErrorText = "";
            }
        }
        /// <summary>
        /// Кнопка "Проверить"
        /// </summary>
        private void Button_Check_Click (Object sender, EventArgs e)
        {
            Check(true);
        }
        /// <summary>
        /// Кнопка "Добавить" в таблице "Дополнительные изделия"
        /// </summary>
        private void Item_AddSensor_ItemClick (Object sender, ItemClickEventArgs e)
        {
            DataList.AddNew();
        }
        /// <summary>
        /// Кнопка "Удалить" в таблице "Дополнительные изделия"
        /// </summary>
        private void Item_RemoveSensor_ItemClick (Object sender, ItemClickEventArgs e)
        {
            RemoveAdditionalWare();
        }
        /// <summary>
        /// Кнопка "Открыть паспорт" в таблице "Дополнительные изделия"
        /// </summary>
        private void Item_OpenPassport_ItemClick (Object sender, ItemClickEventArgs e)
        {
            if (View_Sensors.FocusedRowHandle >= 0)
                if (!String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].WareName))
                {
                    String[] SensorData = DataList[View_Sensors.FocusedRowHandle].WareName.Split(' ');
                    if (SensorData.Length >= 2)
                        OpenPassport(SensorData[1], SensorData[0]);
                }
        }
        /// <summary>
        /// Кнопка "ОК"
        /// </summary>
        private void Button_Click (Object sender, EventArgs e)
        {
            BaseCard.WriteLog("Button_Click");
            if (sender.Equals(Button_OK))
            {
                BaseCard.WriteLog("Button_OK");
                if (!IsCalibration)
                {
                    if (this.Edit_VerifySerialNumber.Text == "" && Edit_CalibrationCertificate.Text != "")
                    {
                        // Определяем результат поверки (годен/не годен)
                        bool VerifyResult = CalibrationLib.GetVerifyResult(BaseCard.CardScript.Session, BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId));

                        if (VerifyResult)
                        {
                            MyMessageBox.Show("Укажите серийный номер поверки.");
                            return;
                        }
                    }
                    if (this.Edit_VerifySerialNumber.Text != "" && Edit_CalibrationCertificate.Text != "")
                    {
                        CardData VerificationCertificateCard = BaseCard.CardScript.Session.CardManager.GetCardData(Edit_CalibrationCertificate.Tag.ToGuid());
                        if (VerificationCertificateCard.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber) != null)
                        {
                            VerificationCertificateCard.SetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber, this.Edit_VerifySerialNumber.Text);
                        }
                    }
                }
                DialogResult = Check(true) ? DialogResult.OK : DialogResult.None;
                BaseCard.WriteLog("DialogResult");
            }
            else
                DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// Кнопка "Изменить тип прибора" в таблице "Дополнительные изделия"
        /// </summary>
        private void Button_ChangeType_Click (Object sender, EventArgs e)
        {
            GetDevice();
        }
        /// <summary>
        /// Событие удаления доп. изделия
        /// </summary>
        private void View_Sensors_KeyDown (Object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                RemoveAdditionalWare();
        }
        /// <summary>
        /// Проверка правильности заполнения таблицы "Дополнительные изделия"
        /// </summary>
        private void View_Sensors_ValidatingEditor (Object sender, BaseContainerValidateEditorEventArgs e)
        {
            RowData Row = null;
            if (View_Sensors.FocusedRowHandle >= 0)
            {
                if (View_Sensors.FocusedColumn.Name == "addWareName")
                {
                    DataList[View_Sensors.FocusedRowHandle].Valid = false;
                    switch (UniversalCard.VerifySensor(out Row, e.Value as String, !DataList.Select(r => r.WareName).Where(s => !String.IsNullOrEmpty(s)).Contains(e.Value as String), DeviceState.Stocking, DeviceState.Bundling))
                    {
                        case -2: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Не введено значение!";
                            break;
                        case -1:
                            if (this.UserRole == RefCertificateCreationCard.UserRoles.Admin)
                            {
                                if (BusySensors.Contains(e.Value as String) && !DataList.Select(r => r.WareName).Where(s => !String.IsNullOrEmpty(s)).Contains(e.Value as String))
                                    DataList[View_Sensors.FocusedRowHandle].ToolTip = "Изделие с указанным номером уже указано в данной карточке!";
                                else
                                {
                                    DataList[View_Sensors.FocusedRowHandle].ToolTip = Row.GetString("Name");
                                    DataList[View_Sensors.FocusedRowHandle].WareID = Row.Id.ToString();
                                    DataList[View_Sensors.FocusedRowHandle].Valid = true;
                                    if (DataList[View_Sensors.FocusedRowHandle].OldWareName != e.Value as String)
                                    {
                                        DataList[View_Sensors.FocusedRowHandle].ProtocolID = "";
                                        DataList[View_Sensors.FocusedRowHandle].ProtocolName = "";
                                        DataList[View_Sensors.FocusedRowHandle].CertificateID = "";
                                        DataList[View_Sensors.FocusedRowHandle].CertificateName = "";
                                    }
                                }
                            }
                            else
                            {
                                DataList[View_Sensors.FocusedRowHandle].ToolTip = "Датчик с указанным номером должен быть в состоянии «На складе» или «На комплектации»!";
                            }
                            break;
                        case 0: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Изделие с указанным номером не существует!";
                            break;
                        case 1:
                            if (BusySensors.Contains(e.Value as String) && !DataList.Select(r => r.WareName).Where(s => !String.IsNullOrEmpty(s)).Contains(e.Value as String))
                                DataList[View_Sensors.FocusedRowHandle].ToolTip = "Изделие с указанным номером уже указано в данной карточке!";
                            else
                            {
                                DataList[View_Sensors.FocusedRowHandle].ToolTip = Row.GetString("Name");
                                DataList[View_Sensors.FocusedRowHandle].WareID = Row.Id.ToString();
                                DataList[View_Sensors.FocusedRowHandle].Valid = true;
                                if (DataList[View_Sensors.FocusedRowHandle].OldWareName != e.Value as String)
                                {
                                    DataList[View_Sensors.FocusedRowHandle].ProtocolID = "";
                                    DataList[View_Sensors.FocusedRowHandle].ProtocolName = "";
                                    DataList[View_Sensors.FocusedRowHandle].CertificateID = "";
                                    DataList[View_Sensors.FocusedRowHandle].CertificateName = "";
                                }
                            }
                            break;
                        default: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Обнаружена неоднозначность (по запросу обнаружено несколько записей)!";
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Кнопка "Создать протокол калибровки/поверки"
        /// </summary>
        private void Button_CreateProtocol_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.NotStarted)
                {
                    MyMessageBox.Show("Передайте заявку в метрологическую лабораторию.");
                    return;
                }
                CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);
                // Перечень датчиков
                List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                // Проверка существования протокола калибровки
                if (IsCalibration)
                {
                    CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.CalibrationProtocolCategoryID);
                    if (DocumentCard != null)
                    {
                        switch (MyMessageBox.Show("Для данного прибора уже существует протокол калибровки. Обновить дату проведения калибровки?", "", MessageBoxButtons.YesNo))
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                CalibrationDate = DateTime.Today;
                                ReceiptDate = GetReceiptDate(CalibrationDate);
                                CalibrationDocs.CalibrationProtocol.ReFill(DocumentCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate);
                                break;
                            case System.Windows.Forms.DialogResult.No:
                                CalibrationDate = (DateTime)DocumentCard.GetDocumentProperty(CalibrationLib.DocumentProperties.StartDate);
                                ReceiptDate = GetReceiptDate(CalibrationDate);
                                break;
                        }
                        ProtocolId = DocumentCard.Id;
                        BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                        return;
                    }

                    bool Verify = true;

                    // Проверяем возможность создания "Протокола калировки" для прибора
                    if ((this.Edit_CalibrationProtocol.Text == null) || (this.Edit_CalibrationProtocol.Text == ""))
                    {
                        DateTime TestDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                        if (!CalibrationDocs.CalibrationProtocol.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, TestDate, WaresList, GetReceiptDate(TestDate), true))
                        { Verify = false; }
                    }

                    if (Verify)
                    {
                        // Создание "Протокола калировки" для прибора
                        if ((this.Edit_CalibrationProtocol.Text == null) || (this.Edit_CalibrationProtocol.Text == ""))
                        {
                            if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                            if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                            CardData FileCard = CalibrationDocs.CalibrationProtocol.Create(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate);
                            ProtocolId = FileCard.Id;
                            BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                        }
                    }
                }
                // Проверка существования протокола поверки
                if (!IsCalibration)
                {
                    CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.VerificationProtocolCategoryID);
                    if (DocumentCard != null)
                    {
                        switch (MyMessageBox.Show("Для данного прибора уже существует протокол поверки. Переформировать?", "", MessageBoxButtons.YesNo))
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                CalibrationDate = DateTime.Today;
                                ReceiptDate = GetReceiptDate(CalibrationDate);
                                CalibrationDocs.VerificationProtocol.ReFill(DocumentCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true);
                                break;
                            case System.Windows.Forms.DialogResult.No:
                                CalibrationDate = (DateTime)DocumentCard.GetDocumentProperty(CalibrationLib.DocumentProperties.StartDate);
                                ReceiptDate = GetReceiptDate(CalibrationDate);
                                break;
                        }
                        ProtocolId = DocumentCard.Id;
                        if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                        return;
                    }

                    bool Verify = true;

                    // Проверяем возможность создания "Протокола поверки" для прибора
                    if ((this.Edit_CalibrationProtocol.Text == null) || (this.Edit_CalibrationProtocol.Text == ""))
                    {
                        DateTime TestDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                        if (!CalibrationDocs.VerificationProtocol.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, TestDate, WaresList, GetReceiptDate(TestDate), true))
                        { Verify = false; }
                    }

                    if (Verify)
                    {
                        // Создание "Протокола калировки" для прибора
                        if ((this.Edit_CalibrationProtocol.Text == null) || (this.Edit_CalibrationProtocol.Text == ""))
                        {
                            if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                            if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                            CardData FileCard = CalibrationDocs.VerificationProtocol.Create(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true);
                            ProtocolId = FileCard.Id;
                            if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                        }
                    }
                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Кнопка "Создать сертификат о калибровке/свидетельство о поверке"
        /// </summary>
        private void Button_CreateCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.NotStarted)
                {
                    MyMessageBox.Show("Передайте заявку в метрологическую лабораторию.");
                    return;
                }
                CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);
                // Перечень датчиков
                List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                // Проверка существования сертификата о калибровке
                if (IsCalibration)
                {
                    CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.CalibrationCertificateCategoryID);
                    if (DocumentCard != null)
                    {
                        MyMessageBox.Show("Для данного прибора уже существует сертификат о калибровке.");
                        if (CalibrationDocs.CalibrationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                        {
                            CalibrationDocs.CalibrationCertificate.ReFill(DocumentCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, ReceiptDate);
                            CertificateId = DocumentCard.Id;
                            BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                        }
                        return;
                    }

                    bool Verify = true;
                    // Проверяем возможность создания "Сертификата о калибровке" для прибора
                    if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                    {
                        DateTime TestDate = (CalibrationDate == null) || (CalibrationDate == DateTime.MinValue) ? CalibrationDate = DateTime.Today : CalibrationDate; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                        if (!CalibrationDocs.CalibrationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, TestDate, WaresList, GetReceiptDate(TestDate), true))
                        { Verify = false; }
                    }

                    if (Verify)
                    {
                        // Создание "Сертификата о калибровке" для прибора
                        if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                        {
                            if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                            if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                            CardData FileCard = CalibrationDocs.CalibrationCertificate.Create(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, ReceiptDate, true);
                            CertificateId = FileCard.Id;
                            BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                        }
                    }
                }
                // Проверка существования свидетельства о поверке
                if (!IsCalibration)
                {
                    if (ProtocolId == null || ProtocolId == Guid.Empty)
                    {
                        MyMessageBox.Show("Сформируйте протокол поверки.");
                    }
                    else
                    {
                        // Определяем результат поверки (годен/не годен)
                        bool VerifyResult = true;
                        if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin)
                        { VerifyResult = CalibrationLib.GetVerifyResult(BaseCard.CardScript.Session, BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId)); }

                        if (VerifyResult)  // Прибор годен, формируется Свидетельство о поверке
                        {
                            // Проверяем, существует ли уже для данного прибора Свидетельство о поверке
                            CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.VerificationCertificateCategoryID);
                            if (DocumentCard != null)
                            {
                                MyMessageBox.Show("Для данного прибора уже существует свидетельство о поверке.");
                                if (CalibrationDocs.VerificationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                {
                                    if (VerifyResult)
                                    {
                                        CertificateId = DocumentCard.Id;
                                        if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                    }
                                }
                                return;
                            }
                            // Проверяем, существует ли уже для данного прибора Извещение о непригодности
                            DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.NoticeOfUnfitnessCategoryID);
                            if (DocumentCard != null)
                            {
                                MyMessageBox.Show("Для данного прибора уже существует Извещение о непригодности.");
                                if (CalibrationDocs.NoticeOfUnfitness.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                {
                                    if (VerifyResult)
                                    {
                                        CertificateId = DocumentCard.Id;
                                        if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                    }
                                }
                                return;
                            }


                            // Проверяем возможность создания "Свидетельства о поверке" для прибора
                            bool Verify = true;
                            if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                            {
                                DateTime TestDate = (CalibrationDate == null) || (CalibrationDate == DateTime.MinValue) ? CalibrationDate = DateTime.Today : CalibrationDate; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                if (!CalibrationDocs.VerificationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, TestDate, WaresList, GetReceiptDate(TestDate), true))
                                { Verify = false; }
                            }

                            if (Verify)
                            {
                                // Создание "Свидетельства о поверке" для прибора
                                if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                                {
                                    if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                    if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);
                                    string VerifySerialNumber = Edit_VerifySerialNumber.Text == null ? "" : Edit_VerifySerialNumber.Text;

                                    CardData FileCard = CalibrationDocs.VerificationCertificate.Create(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, VerifySerialNumber, true);
                                    CertificateId = FileCard.Id;

                                    // Занесение сформированного документа в протокол поверки
                                    string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                    DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                    string DocumentDate = DocumentDateTime.ToShortDateString();
                                    int VerificationInterval = CalibrationLib.GetVerificationInterval(this.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                    string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();

                                    CardData fileData = BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId);
                                    RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                    VersionedFileCard fileCard = (VersionedFileCard)BaseCard.CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                    fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                    CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Свидетельство о поверке №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                    fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                    System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);

                                    if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                                }
                            }
                        }
                        else               // Прибор не годен, формируется Извещение о непригодности
                        {
                            // Проверяем, существует ли уже для данного прибора Извещение о непригодности
                            CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.NoticeOfUnfitnessCategoryID);
                            if (DocumentCard != null)
                            {
                                MyMessageBox.Show("Для данного прибора уже существует извещение о непригодности.");
                                if (CalibrationDocs.NoticeOfUnfitness.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                {
                                    CertificateId = DocumentCard.Id;
                                    BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                }
                                return;
                            }
                            // Проверяем, существует ли уже для данного прибора Свидетельство о поверке
                            DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName), CalibrationLib.VerificationCertificateCategoryID);
                            if (DocumentCard != null)
                            {
                                MyMessageBox.Show("Для данного прибора уже существует свидетельство о поверке.");
                                if (CalibrationDocs.VerificationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                {
                                    if (VerifyResult)
                                    {
                                        CertificateId = DocumentCard.Id;
                                        if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                    }
                                }
                                return;
                            }

                            // Проверяем возможность создания "Извещения о непригодности" для прибора

                            if (this.CausesOfUnfitness == null || this.CausesOfUnfitness == "")
                            {
                                MyMessageBox.Show("Укажите причины непригодности.");
                                return;
                            }

                            bool Verify = true;
                            if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                            {
                                DateTime TestDate = (CalibrationDate == null) || (CalibrationDate == DateTime.MinValue) ? CalibrationDate = DateTime.Today : CalibrationDate; 
                                if (!CalibrationDocs.NoticeOfUnfitness.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, TestDate, WaresList, GetReceiptDate(TestDate), true))
                                { Verify = false; }
                            }

                            if (Verify)
                            {
                                // Создание "Извещения о непригодности" для прибора
                                if ((this.Edit_CalibrationCertificate.Text == null) || (this.Edit_CalibrationCertificate.Text == ""))
                                {
                                    if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today;
                                    if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);
                                    string VerifySerialNumber = Edit_VerifySerialNumber.Text == null ? "" : Edit_VerifySerialNumber.Text;

                                    CardData FileCard = CalibrationDocs.NoticeOfUnfitness.Create(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, 
                                        "отсутствует", CausesOfUnfitness, true );
                                    CertificateId = FileCard.Id;
                                    // Занесение сформированного документа в протокол поверки
                                    string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                    DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                    string DocumentDate = DocumentDateTime.ToShortDateString();
                                    int VerificationInterval = CalibrationLib.GetVerificationInterval(this.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                    string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();

                                    CardData fileData = BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId);
                                    RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                    VersionedFileCard fileCard = (VersionedFileCard)BaseCard.CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                    fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                    CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Извещение о непригодности №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                    fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                    System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                                    if (this.UserRole != RefCertificateCreationCard.UserRoles.Admin) BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Кнопка "Обновить протокол калибровки"
        /// </summary>
        private void Button_RefreshProtocol_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsCalibration)        // Для калибровки
                {
                    if (MyMessageBox.Show("Обновить протокол калибровки?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);

                        // Перечень датчиков
                        List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                        bool Verify = true;
                        // Проверяем возможность обновления "Протокола калировки" для прибора
                        if ((this.Edit_CalibrationProtocol.Text != null) && (this.Edit_CalibrationProtocol.Text != ""))
                        {
                            if (!CalibrationDocs.CalibrationProtocol.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                            { Verify = false; }
                        }

                        if (Verify)
                        {
                            // Обновление "Протокола калибровки" для прибора
                            if ((this.Edit_CalibrationProtocol.Text != null) && (this.Edit_CalibrationProtocol.Text != ""))
                            {
                                CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(this.Edit_CalibrationProtocol.Tag.ToString()));
                                CalibrationDocs.CalibrationProtocol.ReFill(FileCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate);
                            }
                            XtraMessageBox.Show("Данные 'Протокола калибровки' обновлены успешно.");
                        }
                    }
                }
                if (!IsCalibration)    // Для поверки
                {
                    if (MyMessageBox.Show("Обновить протокол поверки?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);

                        // Перечень датчиков
                        List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                        bool Verify = true;
                        // Проверяем возможность обновления "Протокола калировки" для прибора
                        if ((this.Edit_CalibrationProtocol.Text != null) && (this.Edit_CalibrationProtocol.Text != ""))
                        {
                            if (!CalibrationDocs.VerificationProtocol.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                            { Verify = false; }
                        }

                        if (Verify)
                        {
                            // Обновление "Протокола калибровки" для прибора
                            if ((this.Edit_CalibrationProtocol.Text != null) && (this.Edit_CalibrationProtocol.Text != ""))
                            {
                                CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(this.Edit_CalibrationProtocol.Tag.ToString()));
                                CalibrationDocs.VerificationProtocol.ReFill(FileCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true);
                            }
                            XtraMessageBox.Show("Данные 'Протокола поверки' обновлены успешно.");
                        }
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message, "Внимание! Произошло непредвиденное исключение.", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        /// <summary>
        /// Кнопка "Обновить сертификат о калибровке"
        /// </summary>
        private void Button_RefreshCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsCalibration)
                {
                    if (MyMessageBox.Show("Обновить сертификат о калибровке?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);
                        // Перечень датчиков
                        List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                        bool Verify = true;
                        // Проверяем возможность обновления "Сертификата о калибровке" для прибора
                        if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                        {
                            if (!CalibrationDocs.CalibrationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                            { Verify = false; }
                        }

                        if (Verify)
                        {
                            // Обновление "Сертификата о калибровке" для прибора
                            if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                            {
                                CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(this.Edit_CalibrationCertificate.Tag.ToString()));
                                CalibrationDocs.CalibrationCertificate.ReFill(FileCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, ReceiptDate);
                            }
                            XtraMessageBox.Show("Данные 'Сертификата о калибровке' обновлены успешно.");
                        }
                    }
                }

                if (!IsCalibration)
                {
                    if (ProtocolId == null || ProtocolId == Guid.Empty)
                    {
                        MyMessageBox.Show("Сформируйте протокол поверки.");
                    }
                    else
                    {
                        MyMessageBox.Show("1");
                        // Определяем результат поверки (годен/не годен)
                        bool VerifyResult = CalibrationLib.GetVerifyResult(BaseCard.CardScript.Session, BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId));
                        MyMessageBox.Show("2");
                        if (VerifyResult)  // Прибор годен, обновляем Свидетельство о поверке
                        {
                            MyMessageBox.Show("3");
                            if (MyMessageBox.Show("Обновить свидетельство о поверке?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                MyMessageBox.Show("4");
                                CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);
                                MyMessageBox.Show("5");
                                // Перечень датчиков
                                List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);
                                MyMessageBox.Show("6");
                                bool Verify = true;
                                // Проверяем возможность обновления "Свидетельства о поверке" для прибора
                                if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                                {
                                    MyMessageBox.Show("7");
                                    if (!CalibrationDocs.VerificationCertificate.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                    {
                                        MyMessageBox.Show("8");
                                        Verify = false;
                                    }
                                }

                                if (Verify)
                                {
                                    MyMessageBox.Show("9");
                                    // Обновление "Свидетельства о поверке" для прибора
                                    if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                                    {
                                        MyMessageBox.Show("10");
                                        CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(this.Edit_CalibrationCertificate.Tag.ToString()));
                                        MyMessageBox.Show("11");
                                        CalibrationDocs.VerificationCertificate.ReFill(FileCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, "отсутствует");
                                        MyMessageBox.Show("12");
                                        CertificateId = FileCard.Id;
                                        MyMessageBox.Show("13");
                                        // Занесение сформированного документа в протокол поверки
                                        string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                        MyMessageBox.Show("14");
                                        DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                        MyMessageBox.Show("15");
                                        string DocumentDate = DocumentDateTime.ToShortDateString();
                                        MyMessageBox.Show("16");
                                        int VerificationInterval = CalibrationLib.GetVerificationInterval(this.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                        MyMessageBox.Show("17");
                                        string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();
                                        MyMessageBox.Show("18");

                                        CardData fileData = BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId);
                                        MyMessageBox.Show("19");
                                        RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                        MyMessageBox.Show("20");
                                        VersionedFileCard fileCard = (VersionedFileCard)BaseCard.CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                        MyMessageBox.Show("21");
                                        fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                        MyMessageBox.Show("22");
                                        CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Свидетельство о поверке №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                        MyMessageBox.Show("23");
                                        fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                        MyMessageBox.Show("24");
                                        System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                                        MyMessageBox.Show("25");
                                    }
                                    XtraMessageBox.Show("Данные 'Свидетельства о поверке' обновлены успешно.");
                                }
                            }
                        }
                        else    // Прибор не годен, обновляем Извещение о непригодности
                        {

                            if (MyMessageBox.Show("Обновить извещение о непригодности?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                CardData DeviceCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(DeviceNumber, DeviceName);
                                // Перечень датчиков
                                List<CardData> WaresList = GetWaresListForCalibration(CalibrationDocs.CalibrationLib.SensorsList);

                                bool Verify = true;
                                if (this.CausesOfUnfitness == null || this.CausesOfUnfitness == "")
                                {
                                    MyMessageBox.Show("Укажите причины непригодности.");
                                    return;
                                }
                                // Проверяем возможность обновления "Извещения о непригодности" для прибора
                                if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                                {
                                    if (!CalibrationDocs.NoticeOfUnfitness.Verify(BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, true))
                                    {
                                        Verify = false;
                                    }
                                }

                                if (Verify)
                                {
                                    // Обновление "Извещения о непригодности" для прибора
                                    if ((this.Edit_CalibrationCertificate.Text != null) && (this.Edit_CalibrationCertificate.Text != ""))
                                    {
                                        CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(this.Edit_CalibrationCertificate.Tag.ToString()));
                                        CalibrationDocs.NoticeOfUnfitness.ReFill(FileCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate, CausesOfUnfitness);
                                        CertificateId = FileCard.Id;
                                        // Занесение сформированного документа в протокол поверки
                                        string DocumentNumber = CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.DocumentNumber).ToString();
                                        DateTime DocumentDateTime = ((DateTime)CalibrationLib.GetDocumentProperty(FileCard, CalibrationLib.DocumentProperties.StartDate));
                                        string DocumentDate = DocumentDateTime.ToShortDateString();
                                        int VerificationInterval = CalibrationLib.GetVerificationInterval(this.UniversalCard, DeviceCard.GetDeviceTypeID().ToGuid());
                                        string ValidUntil = DocumentDateTime.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();

                                        CardData fileData = BaseCard.CardScript.Session.CardManager.GetCardData(ProtocolId);
                                        RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
                                        VersionedFileCard fileCard = (VersionedFileCard)BaseCard.CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
                                        fileCard.CurrentVersion.Download(CalibrationLib.TempFolder + fileCard.Name);
                                        CalibrationLib.SetResultDocumentName(CalibrationLib.TempFolder + fileCard.Name, "Извещение о непригодности №" + DocumentNumber + " от " + DocumentDate, ValidUntil);
                                        fileCard.CheckIn(CalibrationLib.TempFolder + fileCard.Name, 0, false, true);
                                        System.IO.File.Delete(CalibrationLib.TempFolder + fileCard.Name);
                                    }
                                    XtraMessageBox.Show("Данные 'Извещения о непригодности' обновлены успешно.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }

        /// <summary>
        /// Возвращает перечень изделий из заданного списка, пригодных для калибровки
        /// </summary>
        /// <param name="WaresList">Перечень изделий, среди которых осуществляется поиск.</param>
        private List<CardData> GetWaresListForCalibration(string[] WaresList)
        {
            // Перечень датчиков
            List<CardData> AdditionalWaresList = new List<CardData>();

            for (int i = 0; i < DataList.Count; i++)
            {
                string[] WaresData = DataList[i].WareName.Split(' ');

                // Проверка, входит ли доп. изделие в перечень датчиков, которые включаются в сертификаты о калибровке
                if (WaresList.Any(r => r == WaresData[0]))
                { 
                    CardData WaresPassport = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(WaresData[1], WaresData[0]);
                    AdditionalWaresList.Add(WaresPassport); 
                }
            }
            return AdditionalWaresList;
        }
        /// <summary>
        /// Задает ID какрточки файла протокола калибровки для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        /// <param name="ProtocolCardID">ID карточки файла протокола калибровки.</param>
        private void SetAdditionalProtocol(Guid WaresCardID, Guid ProtocolCardID)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                string[] WaresData = DataList[i].WareName.Split(' ');

                CardData WaresPassport = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(WaresData[1], WaresData[0]);
                if(WaresPassport.Id.Equals(WaresCardID))
                {
                    DataList[i].ProtocolID = ProtocolCardID.ToString();
                }
            }
        }
        /// <summary>
        /// Задает ID какрточки файла сертификата о калибровке для доп. изделия.
        /// </summary>
        /// <param name="WaresCardID">ID паспорта изделия.</param>
        /// <param name="CertificateCardID">ID карточки файла сертификата о калибровке.</param>
        private void SetAdditionalCertificate(Guid WaresCardID, Guid CertificateCardID)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                string[] WaresData = DataList[i].WareName.Split(' ');

                CardData WaresPassport = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(WaresData[1], WaresData[0]);
                if (WaresPassport.Id.Equals(WaresCardID))
                {
                    DataList[i].CertificateID = CertificateCardID.ToString();
                }
            }
        }
        /// <summary>
        /// Кнопка "Создать протокол калибровки для доп. изделия".
        /// </summary>
        private void Item_CreateProtocol_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (IsCalibration)
            {
                if (CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.NotStarted)
                {
                    MyMessageBox.Show("Передайте заявку в отдел калибровки и настройки аппаратуры.");
                    return;
                }

                if (View_Sensors.FocusedRowHandle >= 0)
                {
                    if (!String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].WareName))
                    {
                        String[] SensorData = DataList[View_Sensors.FocusedRowHandle].WareName.Split(' ');
                        if (SensorData.Length >= 2)
                        {
                            if (String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].ProtocolID))
                            {
                                /*switch (MyMessageBox.Show("Для доп. изделия " + DataList[View_Sensors.FocusedRowHandle].WareName + " уже существует протокол калибровки. Обновить дату проведения калибровки?", "", MessageBoxButtons.YesNo))
                                {
                                    case System.Windows.Forms.DialogResult.Yes:
                                        CalibrationDate = DateTime.Today;
                                        ReceiptDate = GetReceiptDate(CalibrationDate);
                                        CalibrationDocs.CalibrationProtocol.ReFill(DocumentCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate);
                                        break;
                                    case System.Windows.Forms.DialogResult.No:
                                        CalibrationDate = (DateTime)DocumentCard.GetDocumentProperty(CalibrationLib.DocumentProperties.StartDate);
                                        break;
                                }*/
                                // Создание протокола калибровки
                                if (!CalibrationDocs.CalibrationLib.AdditionalWaresList.Any(r => r == SensorData[0]))   // Для текущего доп. изделия нет шаблона протокола калибровки
                                { return; }

                                CardData WareCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]);

                                // Проверка существования протокола калибровки
                                CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]), CalibrationLib.CalibrationProtocolCategoryID);
                                if (DocumentCard != null)
                                {
                                    switch (MyMessageBox.Show("Для доп. изделия " + DataList[View_Sensors.FocusedRowHandle].WareName + " уже существует протокол калибровки. Обновить дату проведения калибровки?", "", MessageBoxButtons.YesNo))
                                    {
                                        case System.Windows.Forms.DialogResult.Yes:
                                            // Проверяем возможность создания "Протокола калибровки" для доп. изделия
                                            bool IsVerify = true;
                                            DateTime NewTestDate = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                            if (!CalibrationDocs.CalibrationProtocol.Verify(BaseCard.CardScript, Context, WareCard, Guid.Empty, NewTestDate, null, GetReceiptDate(NewTestDate)))
                                            { IsVerify = false; }
                                            if (IsVerify)
                                            {
                                                if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                                if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                                                CalibrationDocs.CalibrationProtocol.ReFill(DocumentCard, BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate);
                                                DataList[View_Sensors.FocusedRowHandle].ProtocolID = DocumentCard.Id.ToString();
                                                DataList[View_Sensors.FocusedRowHandle].ProtocolName = DocumentCard.Description.ToString();
                                                Control_Sensors.Refresh();
                                                BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                            }
                                            //CalibrationDate = DateTime.Today;
                                            //ReceiptDate = GetReceiptDate(CalibrationDate);
                                            //CalibrationDocs.CalibrationProtocol.ReFill(DocumentCard, BaseCard.CardScript, Context, DeviceCard, Guid.Empty, CalibrationDate, WaresList, ReceiptDate);
                                            break;
                                        case System.Windows.Forms.DialogResult.No:
                                            DataList[View_Sensors.FocusedRowHandle].ProtocolID = DocumentCard.Id.ToString();
                                            DataList[View_Sensors.FocusedRowHandle].ProtocolName = DocumentCard.Description.ToString();
                                            Control_Sensors.Refresh();
                                            BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                            break;
                                    }
                                    //MyMessageBox.Show("Для данного прибора уже существует протокол калибровки.");
                                    //DataList[View_Sensors.FocusedRowHandle].ProtocolID = DocumentCard.Id.ToString();
                                    //DataList[View_Sensors.FocusedRowHandle].ProtocolName = DocumentCard.Description.ToString();
                                    //Control_Sensors.Refresh();
                                    BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                    return;
                                }
                                // Проверяем возможность создания "Протокола калибровки" для доп. изделия
                                bool Verify = true;
                                DateTime TestDate = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                if (!CalibrationDocs.CalibrationProtocol.Verify(BaseCard.CardScript, Context, WareCard, Guid.Empty, TestDate, null, GetReceiptDate(TestDate)))
                                { Verify = false; }
                                if (Verify)
                                {
                                    if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today; //DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                    if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                                    CardData FileCard = CalibrationDocs.CalibrationProtocol.Create(BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate);
                                    DataList[View_Sensors.FocusedRowHandle].ProtocolID = FileCard.Id.ToString();
                                    DataList[View_Sensors.FocusedRowHandle].ProtocolName = FileCard.Description.ToString();
                                    Control_Sensors.Refresh();
                                    BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                                }
                            }
                            else
                            {
                                // Обновление протокола калибровки
                                DialogResult result = MyMessageBox.Show("Обновить протокол калибровки?", "", MessageBoxButtons.YesNo);
                                switch (result)
                                {
                                    case System.Windows.Forms.DialogResult.Yes:
                                        CardData WareCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]);
                                        // Проверяем возможность создания "Протокола калибровки" для доп. изделия
                                        bool Verify = true;
                                        DateTime TestDate = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                        if (!CalibrationDocs.CalibrationProtocol.Verify(BaseCard.CardScript, Context, WareCard, Guid.Empty, TestDate, null, GetReceiptDate(TestDate)))
                                        { Verify = false; }
                                        if (Verify)
                                        {
                                            if ((CalibrationDate == null) || (CalibrationDate == DateTime.MinValue)) CalibrationDate = DateTime.Today.DayOfWeek == DayOfWeek.Friday ? DateTime.Today.AddDays(-1) : DateTime.Today;
                                            if ((ReceiptDate == null) || (ReceiptDate == DateTime.MinValue)) ReceiptDate = GetReceiptDate(CalibrationDate);

                                            CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(DataList[View_Sensors.FocusedRowHandle].ProtocolID));
                                            CalibrationDocs.CalibrationProtocol.ReFill(FileCard, BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate);
                                        }
                                        break;
                                    case System.Windows.Forms.DialogResult.No:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Кнопка "Создать сертификат о калибровке для доп. изделия".
        /// </summary>
        private void Item_CreateCertificate_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (IsCalibration)
            {
                if (CurrentState == (int)RefCertificateCreationCard.MainInfo.CardState.NotStarted)
                {
                    MyMessageBox.Show("Передайте заявку в отдел калибровки и настройки аппаратуры.");
                    return;
                }
                if (View_Sensors.FocusedRowHandle >= 0)
                {
                    if (!String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].WareName))
                    {
                        String[] SensorData = DataList[View_Sensors.FocusedRowHandle].WareName.Split(' ');
                        if (SensorData.Length >= 2)
                        {
                            if (String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].CertificateID))
                            {
                                // Создание сертификата о калибовке
                                if (!CalibrationDocs.CalibrationLib.AdditionalWaresList.Any(r => r == SensorData[0]))   // Для текущего доп. изделия нет шаблона сертификата о калибровке
                                { return; }

                                CardData WareCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]);

                                // Проверка существования сертификата о калибровке
                                CardData DocumentCard = CalibrationDocs.CalibrationLib.GetDocumentCard(BaseCard.CardScript.Session, UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]), CalibrationLib.CalibrationCertificateCategoryID);
                                if (DocumentCard != null)
                                {
                                    MyMessageBox.Show("Для данного прибора уже существует сертификат о калибровке.");
                                    DataList[View_Sensors.FocusedRowHandle].CertificateID = DocumentCard.Id.ToString();
                                    DataList[View_Sensors.FocusedRowHandle].CertificateName = DocumentCard.Description.ToString();
                                    Control_Sensors.Refresh();
                                    BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(DocumentCard.Id, ActivateMode.Edit);
                                    return;
                                }

                                // Проверяем возможность создания "Сертификата о калибровке" для доп. изделия
                                bool Verify = true;
                                if (!CalibrationDocs.CalibrationCertificate.Verify(BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate))
                                { Verify = false; }
                                if (Verify)
                                {
                                    CardData FileCard = CalibrationDocs.CalibrationCertificate.Create(BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate, ReceiptDate, true);
                                    DataList[View_Sensors.FocusedRowHandle].CertificateID = FileCard.Id.ToString();
                                    DataList[View_Sensors.FocusedRowHandle].CertificateName = FileCard.Description.ToString();
                                    Control_Sensors.Refresh();
                                    BaseCard.CardScript.CardFrame.CardHost.ShowCardModal(FileCard.Id, ActivateMode.Edit);
                                }
                            }
                            else
                            {
                                // Обновление сертификата о калибровке
                                DialogResult result = MyMessageBox.Show("Обновить сертификат о калибровке?", "", MessageBoxButtons.YesNo);
                                switch (result)
                                {
                                    case System.Windows.Forms.DialogResult.Yes:
                                        CardData WareCard = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(SensorData[1], SensorData[0]);
                                        // Проверяем возможность создания "Сертификата о калибровке" для доп. изделия
                                        bool Verify = true;
                                        if (!CalibrationDocs.CalibrationCertificate.Verify(BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate))
                                        { Verify = false; }
                                        if (Verify)
                                        {
                                            CardData FileCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(DataList[View_Sensors.FocusedRowHandle].CertificateID));
                                            CalibrationDocs.CalibrationCertificate.ReFill(FileCard, BaseCard.CardScript, Context, WareCard, Guid.Empty, CalibrationDate, null, ReceiptDate, ReceiptDate);
                                        }
                                        break;
                                    case System.Windows.Forms.DialogResult.No:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Открытие файла "Протокол калибровки" для доп. изделия.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit1_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int RowIndex = View_Sensors.FocusedRowHandle;
            if (View_Sensors.FocusedColumn.Name == "addProtocolName")
            {
                if (View_Sensors.GetRowCellValue(RowIndex, "ProtocolID").ToString() != "")
                {
                    CardData ProtocolCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(View_Sensors.GetRowCellValue(RowIndex, "ProtocolID").ToString()));
                    BaseCard.CardScript.CardFrame.CardHost.ShowCard(ProtocolCard.Id, ActivateMode.Edit);
                }
            }
        }
        /// <summary>
        ///  Открытие файла "Сертификата о калибровке" для доп. изделия.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit2_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int RowIndex = View_Sensors.FocusedRowHandle;
            if (View_Sensors.FocusedColumn.Name == "addCertificateName")
            {
                if (View_Sensors.GetRowCellValue(RowIndex, "CertificateID").ToString() != "")
                {
                    CardData CertificateCard = BaseCard.CardScript.Session.CardManager.GetCardData(new Guid(View_Sensors.GetRowCellValue(RowIndex, "CertificateID").ToString()));
                    BaseCard.CardScript.CardFrame.CardHost.ShowCard(CertificateCard.Id, ActivateMode.Edit);
                }
            }
        }
        /// <summary>
        /// Открытие файла "Протокола калибровки" для прибора.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Edit_CalibrationProtocol_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if ((Edit_CalibrationProtocol.Text != null) && (Edit_CalibrationProtocol.Text != ""))
            {
                CardData ProtocolCard = BaseCard.CardScript.Session.CardManager.GetCardData(Edit_CalibrationProtocol.Tag.ToGuid());
                BaseCard.CardScript.CardFrame.CardHost.ShowCard(ProtocolCard.Id, ActivateMode.Edit);
            }
        }
        /// <summary>
        /// Открытие файла "Сертификата о калибровки" для прибора.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Edit_CalibrationCertificate_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if ((Edit_CalibrationCertificate.Text != null) && (Edit_CalibrationCertificate.Text != ""))
            {
                CardData CertificateCard = BaseCard.CardScript.Session.CardManager.GetCardData(Edit_CalibrationCertificate.Tag.ToGuid());
                BaseCard.CardScript.CardFrame.CardHost.ShowCard(CertificateCard.Id, ActivateMode.Edit);
            }
        }
        /// <summary>
        /// Вычисление даты поступления на калибровку.
        /// </summary>
        /// <param name="CalibrationDate">Дата проведения калибровки.</param>
        public DateTime GetReceiptDate(DateTime CalibrationDate)
        {
            /*if (IsCalibration)
            {
                Guid BusinessCalendarID = MyHelper.RefCardCalendar_SKB; // "{2DFC601B-451C-E411-A309-00155D016943}";  // Бизнес-календарь СКБ.
                ICalendarService calendarService = this.Context.GetService<ICalendarService>();
                Random RandomValue = new Random();

                double CalibrationTime = 80;                             // срок калибровки - 10 рабочих дней (80 рабочих часов)
                double RandomTime = (double)RandomValue.Next(0, 40);      // случайная погрешность - 5 рабочих дней (40 рабочих часов)

                DateTime ReceiptDate = calendarService.GetStartDate(BusinessCalendarID, CalibrationDate, CalibrationTime + RandomTime);
                return ReceiptDate;
            }
            else
            {*/
                return (DateTime)BaseCard.GetControlValue(RefCertificateCreationCard.MainInfo.SentDate);
            //}
        }
    }
}