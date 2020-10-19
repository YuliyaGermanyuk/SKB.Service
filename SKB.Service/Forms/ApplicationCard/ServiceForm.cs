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
using DocsVision.Platform.ObjectModel;

using RKIT.MyMessageBox;

using SKB.Base;
using SKB.Base.Enums;
using SKB.PaymentAndShipment;
using SKB.PaymentAndShipment.Forms.AccountCard;
using SKB.PaymentAndShipment.Forms.CompleteCard;

namespace SKB.Service.Forms.ApplicationCard
{
    /// <summary>
    /// Форма редактирования таблицы "Сервисное обслуживание".
    /// </summary>
    public partial class ServiceForm : XtraForm
    {
        BindingList<SensorRow> DataList = new BindingList<SensorRow>();

        ObjectContext Context;
        CardData UniversalCard;
        ICardHost Host;
        List<Guid> BusyDeviceNumberIds;
        List<String> BusySensors;
        DateTime RegDate;

        RowData _Devices;
        RowData Devices { get { return _Devices.IsNull() ? _Devices = UniversalCard.Sections[MyHelper.RefUniversalItemType].GetRow(MyHelper.RefItem_Devices) : _Devices; } }

        String DeviceName
        {
            get { return this.Tag as String; }
            set { this.Text = "Выбранный прибор: " + value; this.Tag = value; }
        }
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
            set
            {
                Edit_DeviceNumber.Text = value;
                OldDeviceNumber = value;
            }
        }
        /// <summary>
        /// Старый номер прибора.
        /// </summary>
        public String OldDeviceNumber
        {
            private set
            {
                if (lci_Edit_DeviceNumber.Text == lci_Edit_DeviceNumber.Name)
                    lci_Edit_DeviceNumber.Text = value;
            }
            get { return lci_Edit_DeviceNumber.Text; }
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
        /// Только ДК.
        /// </summary>
        public Boolean AC
        {
            set
            {
                Edit_AC.Checked = value;
                Edit_DeviceNumber.Enabled = !value;
            }
            get
            {
                return Edit_AC.Checked;
            }
        }
        /// <summary>
        /// Датчики.
        /// </summary>
        public String Sensors
        {
            set
            {
                DataList.Clear();
                if (!String.IsNullOrEmpty(value))
                {
                    foreach (String s in value.Split(';'))
                        try { DataList.Add(new SensorRow(s.Trim(), UniversalCard.GetSensorRow(s).GetString("Name"))); }
                        catch { }
                }
                Control_Sensors.RefreshDataSource();
            }
            get
            {
                return SensorsList.Count > 0 ? SensorsList.Aggregate((a, b) => a + ";" + b) : null;
            }
        }
        /// <summary>
        /// Список датчиков.
        /// </summary>
        public List<String> SensorsList
        {
            get
            {
                return DataList.Any(r => !String.IsNullOrWhiteSpace(r.Sensor) && r.Valid) ? DataList.Where(r => !String.IsNullOrWhiteSpace(r.Sensor) && r.Valid).Select(r => r.Sensor).ToList() : new List<String>();
            }
        }

        /// <summary>
        /// С поверкой.
        /// </summary>
        public Boolean Verify
        {
            get { return Control_Actions.Items[2].CheckState.Equals(CheckState.Checked); }
            set { Control_Actions.Items[2].CheckState = value ? CheckState.Checked : CheckState.Unchecked; }
        }
        /// <summary>
        /// Ремонт.
        /// </summary>
        public Boolean Repair
        {
            get { return Control_Actions.Items[0].CheckState.Equals(CheckState.Checked); }
            set { Control_Actions.Items[0].CheckState = value ? CheckState.Checked : CheckState.Unchecked; }
        }
        /// <summary>
        /// Калибровка.
        /// </summary>
        public Boolean Calibrate
        {
            get { return Control_Actions.Items[1].CheckState.Equals(CheckState.Checked); }
            set { Control_Actions.Items[1].CheckState = value ? CheckState.Checked : CheckState.Unchecked; }
        }
        /// <summary>
        /// Помывка.
        /// </summary>
        public Boolean Wash
        {
            get { return Control_Actions.Items[3].CheckState.Equals(CheckState.Checked); }
            set { Control_Actions.Items[3].CheckState = value ? CheckState.Checked : CheckState.Unchecked; }
        }

        /// <summary>
        /// Гарантия.
        /// </summary>
        public Boolean Warranty
        {
            get { return Edit_Warranty.Checked; }
            set { Edit_Warranty.Checked = value; }
        }
        /// <summary>
        /// Дата окончания.
        /// </summary>
        public DateTime? WarrantyEndDate
        {
            get
            {
                try { return (DateTime?)Edit_WarrantyEndDate.EditValue; }
                catch { return null; }
            }
            set
            {
                Edit_WarrantyEndDate.EditValue = value;
                Warranty = value >= RegDate.Date;
            }
        }
        /// <summary>
        /// Комплектующие.
        /// </summary>
        public String ACList
        {
            get { return Edit_ACList.Text; }
            set { Edit_ACList.Text = value; }
        }
        /// <summary>
        /// Комментарий.
        /// </summary>
        public String Comment
        {
            get { return Edit_Comment.Text; }
            set { Edit_Comment.Text = value; }
        }

        /// <summary>
        /// Данные УЛ.
        /// </summary>
        public String CData
        {
            get { return Edit_ACList.Tag as String; }
            set { Edit_ACList.Tag = value; }
        }
        /// <summary>
        /// Список дополнительных комплектующих.
        /// </summary>
        public List<DeviceCompleteRow> ACRows;
        /// <summary>
        /// Инициализирует форму редактирования таблицы "Сервисное обслуживание".
        /// </summary>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="UniversalCard">Данные Универсального справочника.</param>
        /// <param name="Host">Хост карточек.</param>
        /// <param name="DeviceId">Идентификатор типа прибора.</param>
        /// <param name="RegDate">Дата регистрации заявки.</param>
        /// <param name="BusyDeviceNumbers">Занятые номера приборов.</param>
        /// <param name="BusySensors">Занятые сенсоры.</param>
        public ServiceForm (ObjectContext Context, CardData UniversalCard, ICardHost Host, Guid DeviceId, DateTime RegDate, List<Guid> BusyDeviceNumbers = null, List<String> BusySensors = null)
        {
            InitializeComponent();
            this.Context = Context;
            this.UniversalCard = UniversalCard;
            this.Host = Host;
            this.DeviceId = DeviceId;
            this.BusyDeviceNumberIds = BusyDeviceNumbers ?? new List<Guid>();
            this.BusySensors = BusySensors ?? new List<String>();
            this.RegDate = RegDate;

            if (DeviceId.IsEmpty())
            {
                if (!GetDevice(true))
                    throw new MyException(0);
            }
            else
                DeviceName = Devices.ChildSections[MyHelper.RefUniversalItem].Rows[DeviceId].GetString("Name");
        }

        #region Methods

        private Boolean Check (Boolean Show)
        {
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
                    t.ToolTipIcon = DeviceNumberId.IsEmpty() ? ToolTipIcon.Error : ToolTipIcon.Info;
                    t.Show(DeviceNumberTooltTip, Edit_DeviceNumber, 3000);
                }
            }
            else
            {
                DeviceNumber = "";
                DeviceNumberId = Guid.Empty;
                DeviceNumberTooltTip = "";
                Edit_DeviceNumber.ErrorText = "";
                WarrantyEndDate = null;
            }
            for (Int32 i = 0; i < DataList.Count; i++)
                DataList[i].Validate();

            View_Sensors.RefreshData();
            if (DataList.Any(sensor => !sensor.Valid))
            {
                Int32 i = DataList.IndexOf(DataList.First(sensor => !sensor.Valid));
                ToolTipControllerShowEventArgs Args = Controller_ToolTip.CreateShowArgs();
                Args.SelectedControl = Control_Sensors;
                Args.ToolTipType = ToolTipType.SuperTip;
                Args.SuperTip = new SuperToolTip();
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = DataList[i].Sensor ?? "<Пусто>";
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
            return Check;
        }
        /// <summary>
        /// Проверка выбранных действий.
        /// </summary>
        /// <param name="Actions">Действия: Ремонт, Калибровка, Поверка.</param>
        /// <returns></returns>
        public static String CheckActions (params Boolean[] Actions)
        {
            if (Actions != null)
            {
                if (Actions.Length >= 3)
                {
                    Int32 Code = 0;
                    if (Actions[0]) // Ремонт
                        Code += 4;
                    if (Actions[1]) // Калибровка
                        Code += 2;
                    if (Actions[2]) // Поверка
                        Code += 1;
                    switch (Code)
                    {
                        case 0: return "Выберите необходимый вид сервисного обслуживания в поле «Требуется».";
                        case 1: return "Перед поверкой прибора обязательно должна осуществляться калибровка!"
                                + Environment.NewLine + "Выберите данный вид сервиса в поле «Требуется».";
                        case 4: return "После ремонта прибора обязательно должна осуществляться калибровка!"
                                + Environment.NewLine + "Выберите данный вид сервиса в поле «Требуется».";
                        case 5: return "После ремонта прибора и перед поверкой обязательно должна осуществляться калибровка!"
                                + Environment.NewLine + "Выберите данный вид сервиса в поле «Требуется».";
                    }
                }
            }
            return null;
        }

        private Boolean GetDevice (Boolean IsNeedComplete = false)
        {
            Object[] activateParams = new Object[] { 
                MyHelper.RefUniversalItem.ToString("B").ToUpper(), 
                String.Empty, 
                MyHelper.RefItem_Devices.ToString("B").ToUpper(), 
                false, String.Empty, false };
            Object Id = Host.SelectFromCard(MyHelper.RefUniversal, "Выберите прибор...", activateParams);

            if (Id.IsNull())
                return false;

            RowData DeviceItemRow = Devices.ChildSections[MyHelper.RefUniversalItem].Rows[Id.ToGuid()];

            AC = MyMessageBox.Show("Поступили только комплектующие (без прибора)?", "Комплектация", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            DeviceName = DeviceItemRow.GetString("Name");
            RowData DeviceCompleteInfo = Devices.AllChildRows.Find("Name", DeviceName);
            ACList = String.Empty;
            CData = String.Empty;

            if (!DeviceCompleteInfo.IsNull())
            {
                SubSectionData DeviceItems = DeviceCompleteInfo.ChildSections[MyHelper.RefUniversalItem];
                CompleteForm Form = new CompleteForm(DeviceName, DeviceItems, AC, Verify, true, false);

                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        ACList = Form.ACList;
                        CData = Form.CData;
                        ACRows = Form.ACRows;
                        Button_ChangeComplete.Enabled = true;
                        break;
                    default:
                        Button_ChangeComplete.Enabled = false;
                        if (IsNeedComplete)
                            return false;
                        break;
                }
            }
            else
            {
                ACRows = null;
                Button_ChangeComplete.Enabled = false;
            }

            DeviceId = DeviceItemRow.Id;
            return true;
        }

        private void OpenPassport (String FindNumber, String Device)
        {
            CardData Passport = Cards.ApplicationCard.UniversalCard.GetPassportByDeviceNumber(FindNumber, Device);
            if (!Passport.IsNull())
                Host.ShowCardModal(Passport.Id, ActivateMode.Edit);
        }

        private void RemoveSensor ()
        {
            if (View_Sensors.FocusedRowHandle >= 0)
                if (!String.IsNullOrWhiteSpace(DataList[View_Sensors.FocusedRowHandle].Sensor))
                {
                    if (MyMessageBox.Show("Удалить датчик «" + DataList[View_Sensors.FocusedRowHandle].Sensor + "»?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        View_Sensors.DeleteRow(View_Sensors.FocusedRowHandle);
                }
                else
                    View_Sensors.DeleteRow(View_Sensors.FocusedRowHandle);
        }

        #endregion

        private void Controller_ToolTip_GetActiveObjectInfo (Object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == Control_Sensors)
            {
                GridHitInfo hitInfo = View_Sensors.CalcHitInfo(e.ControlMousePosition);
                if (hitInfo.RowHandle >= 0 && DataList.Count > 0)
                {
                    SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                    toolTipArgs.Title.Text = DataList[hitInfo.RowHandle].Sensor;
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

        private void Edit_DeviceNumber_ButtonPressed (Object sender, ButtonPressedEventArgs e)
        {
            switch (e.Button.Kind)
            {
                case ButtonPredefines.Redo:
                    OpenPassport(DeviceNumber, DeviceName);
                    break;
            }
        }

        private void Edit_DeviceNumber_Validating (Object sender, CancelEventArgs e)
        {

            if (!Edit_AC.Checked && !String.IsNullOrEmpty(Edit_DeviceNumber.Text))
            {
                RowData Row;
                DeviceNumberId = Guid.Empty;
                WarrantyEndDate = null;
                Int32 k = Cards.ApplicationCard.UniversalCard.VerifyDeviceNumber(out Row, DeviceNumber, DeviceName, OldDeviceNumber != DeviceNumber, DeviceState.Shipped, DeviceState.Operating);
                switch (k)
                {
                    case -1:
                        DeviceNumberTooltTip = "Прибор с указанным номером должен быть в состоянии «Отгружен», либо «В эксплуатации»!";
                        Edit_DeviceNumber.Properties.Buttons[0].Enabled = true;
                        break;
                    case 0: DeviceNumberTooltTip = "Прибор с указанным номером не существует!"; break;
                    case 1:
                        if (!BusyDeviceNumberIds.Contains(Row.Id))
                        {
                            DeviceNumberTooltTip = Row.GetString("Name");
                            DeviceNumberId = Row.Id;
                            try { WarrantyEndDate = (DateTime?)Row.GetItemPropertyValue("Дата окончания гарантии"); }
                            catch { }
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

        private void Button_Check_Click (Object sender, EventArgs e)
        {
            Check(true);
        }

        private void Item_AddSensor_ItemClick (Object sender, ItemClickEventArgs e)
        {
            DataList.AddNew();
        }

        private void Item_RemoveSensor_ItemClick (Object sender, ItemClickEventArgs e)
        {
            RemoveSensor();
        }

        private void Item_OpenPassport_ItemClick (Object sender, ItemClickEventArgs e)
        {
            if (View_Sensors.FocusedRowHandle >= 0)
                if (!String.IsNullOrEmpty(DataList[View_Sensors.FocusedRowHandle].Sensor))
                {
                    String[] SensorData = DataList[View_Sensors.FocusedRowHandle].Sensor.Split(' ');
                    if (SensorData.Length >= 2)
                        OpenPassport(SensorData[1], SensorData[0]);
                }
        }

        private void Button_Click (Object sender, EventArgs e)
        {
            if (sender.Equals(Button_OK))
            {
                String Message = CheckActions(Repair, Calibrate, Verify);
                if (!String.IsNullOrEmpty(Message))
                {
                    MyMessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                }
                else
                    DialogResult = Check(false) ? DialogResult.OK : DialogResult.None;
            }
            else
                DialogResult = DialogResult.Cancel;
        }

        private void Button_ChangeType_Click (Object sender, EventArgs e)
        {
            GetDevice();
        }

        private void Button_ChangeComplete_Click (Object sender, EventArgs e)
        {
            RowData DeviceCompleteInfo = Devices.AllChildRows.Find("Name", DeviceName);
            if (!DeviceCompleteInfo.IsNull())
            {
                SubSectionData DeviceItems = DeviceCompleteInfo.ChildSections[MyHelper.RefUniversalItem];
                CompleteForm Form = new CompleteForm(DeviceName, DeviceItems, true, false, false, false, CData);
                switch (Form.ShowDialog())
                {
                    case DialogResult.OK:
                        ACList = Form.ACList;
                        CData = Form.CData;
                        ACRows = Form.ACRows;
                        Button_ChangeComplete.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ACRows = null;
                Button_ChangeComplete.Enabled = false;
            }
        }

        private void View_Sensors_KeyDown (Object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                RemoveSensor();
        }

        private void View_Sensors_ValidatingEditor (Object sender, BaseContainerValidateEditorEventArgs e)
        {
            RowData Row = null;
            if (View_Sensors.FocusedRowHandle >= 0)
            {
                DataList[View_Sensors.FocusedRowHandle].Valid = false;
                switch (UniversalCard.VerifySensor(out Row, e.Value as String, !DataList.Select(r => r._Sensor).Where(s => !String.IsNullOrEmpty(s)).Contains(e.Value as String), DeviceState.Shipped, DeviceState.Operating))
                {
                    case -2: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Не введено значение!";
                        break;
                    case -1: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Датчик с указанным номером должен быть в состоянии «Отгружен», либо «В эксплуатации»!";
                        break;
                    case 0: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Датчик с указанным номером не существует!";
                        break;
                    case 1:
                        if (BusySensors.Contains(e.Value as String) && !DataList.Select(r => r._Sensor).Where(s => !String.IsNullOrEmpty(s)).Contains(e.Value as String))
                            DataList[View_Sensors.FocusedRowHandle].ToolTip = "Датчик с указанным номером уже указан в данной карточке!";
                        else
                        {
                            DataList[View_Sensors.FocusedRowHandle].ToolTip = Row.GetString("Name");
                            DataList[View_Sensors.FocusedRowHandle].Valid = true;
                        }
                        break;
                    default: DataList[View_Sensors.FocusedRowHandle].ToolTip = "Обнаружена неоднозначность (по запросу обнаружено несколько записей)!";
                        break;
                }
            }
        }
    }
}