using System;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.CardHost;
using SKB.Base.Ref;
using SKB.Base;
using RKIT.MyMessageBox;

namespace SKB.Service.Forms.ServiceCard
{
    /// <summary>
    ///  Форма "Перечень приборов".
    /// </summary>
    public partial class DeviceList : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  Таблица.
        /// </summary>
        public System.Data.DataTable Table;
        /// <summary>
        ///  Карточка универсального справочника.
        /// </summary>
        CardData UniversalCard;
        /// <summary>
        ///  Перечень приборов.
        /// </summary>
        public ArrayList Devices;
        /// <summary>
        ///  Тип прибора.
        /// </summary>
        public string DeviceTypeID = "";
        /// <summary>
        ///  Тип прибора.
        /// </summary>
        public string DeviceNumberValue = "";
        /// <summary>
        ///  Тип прибора.
        /// </summary>
        public string DevicePassportID = "";
        /// <summary>
        ///  Создание формы "Перечень приборов".
        /// </summary>
        /// <param name="UniversalCard"> Карточка универсального справочника.</param>
        /// <param name="Devices"> Перечень приборов.</param>
        public DeviceList(MyBaseCard Card, CardData UniversalCard, ArrayList Devices)
        {
            this.Devices = Devices;
            this.UniversalCard = UniversalCard;

            InitializeComponent();
            Table = new System.Data.DataTable();
            Table.Columns.Add("DeviceType", typeof(string));
            Table.Columns.Add("DeviceNumber", typeof(string));
            Table.Columns.Add("DevicePassport", typeof(string));
            object[] Parametr = new object[3];

            foreach (string Row in Devices)
            {
                string DeviceType = "";
                string DeviceNumber = "";

                if (Row.ToString() == "")
                {
                    DeviceType = Card.GetControlValue(RefServiceCard.MainInfo.DeviceType).ToString();
                    DeviceNumber = UniversalCard.GetItemName(new Guid(DeviceType));
                }
                else
                {
                    CardData DeviceCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(Row.ToString()));
                    DeviceType = CalibrationDocs.CalibrationLib.GetDeviceTypeID(DeviceCard);
                    DeviceNumber = UniversalCard.GetItemName(new Guid(DeviceType)) + " " + CalibrationDocs.CalibrationLib.GetDeviceNumber(DeviceCard);
                }              

                Parametr[0] = DeviceType;
                Parametr[1] = DeviceNumber;
                Parametr[2] = Row.ToString();
                Table.Rows.Add(Parametr);
            }

            gridControl1.DataSource = Table;
        }
        /// <summary>
        ///  Выбор прибора.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            
            if (gridView1.FocusedRowHandle >= 0)
                if ((gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DeviceType") != null) && (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DeviceType").ToString() != ""))
                {
                    DeviceTypeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DeviceType").ToString();
                    DeviceNumberValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DeviceNumber").ToString();
                    DevicePassportID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DevicePassport").ToString();
                    this.Close();
                }
        }

        private void DeviceList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DeviceTypeID == "")
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}