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
    ///  Форма "Дополнительные изделия"
    /// </summary>
    public partial class AdditionalWares : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  Таблица "Дополнительные изделия"
        /// </summary>
        public System.Data.DataTable Table;
        /// <summary>
        /// Табличный контрол "Дополнительные изделия"
        /// </summary>
        ITableControl T_AW;
        /// <summary>
        ///  Карточка Заявки на сервисное обслуживание
        /// </summary>
        MyBaseCard Card;
        /// <summary>
        ///  Карточка универсального справочника
        /// </summary>
        CardData UniversalCard;
        /// <summary>
        ///  Время диагностики изделия
        /// </summary>
        public ArrayList TimeDiagnostics;
        /// <summary>
        ///  Время калибровки изделия
        /// </summary>
        public ArrayList TimeCalibration;
        /// <summary>
        ///  Принять изменения
        /// </summary>
        public bool Acceptance;
        /// <summary>
        ///  Закрыть
        /// </summary>
        private bool close;
        /// <summary>
        ///  Создать форму "Дополнительные изделия"
        /// </summary>
        /// <param name="Card"> Карточка заявки на сервисное обслуживание.</param>
        /// <param name="UniversalCard"> Карточка универсального справочника.</param>
        /// <param name="Table_AdditionalWaresList"> Контрол Дополнительные комплектующие.</param>
        public AdditionalWares(MyBaseCard Card, CardData UniversalCard, ITableControl Table_AdditionalWaresList)
        {
            T_AW = Table_AdditionalWaresList;
            this.Card = Card;
            this.UniversalCard = UniversalCard;
            TimeDiagnostics = new ArrayList();
            TimeCalibration = new ArrayList();
            Acceptance = false;
            close = false;

            InitializeComponent();
            Table = new System.Data.DataTable();
            Table.Columns.Add("WaresNumber", typeof(string));
            Table.Columns.Add("DiagnosticsTime", typeof(decimal));
            Table.Columns.Add("CalibrationTime", typeof(decimal));
            Table.Columns.Add("CalibrationProtocol", typeof(string));
            Table.Columns.Add("CalibrationCertificate", typeof(string));
            Table.Columns.Add("CalibrationProtocolID", typeof(string));
            Table.Columns.Add("CalibrationCertificateID", typeof(string));
            Table.Columns.Add("WaresNumberID", typeof(string));
            object[] Parametr = new object[8];
            for (int i = 0; i < Table_AdditionalWaresList.RowCount; i++)
            {
                Parametr[0] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumber];
                Parametr[1] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.DiagnosticsTime];
                Parametr[2] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationTime];
                Parametr[3] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol] == null ? "" :
                    Card.CardScript.Session.CardManager.GetCardData(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol].ToGuid()).Description;
                Parametr[4] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate] == null ? "" : 
                    Card.CardScript.Session.CardManager.GetCardData(Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate].ToGuid()).Description;
                Parametr[5] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol] == null ? "" : 
                    Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationProtocol].ToString();
                Parametr[6] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate] == null ? "" : 
                    Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.CalibrationCertificate].ToString();
                Parametr[7] = Table_AdditionalWaresList[i][RefServiceCard.AdditionalWaresList.WaresNumberID].ToString();
                Table.Rows.Add(Parametr);
                
            }
            gridControl1.DataSource = Table;
        }
        /// <summary>
        ///  Подтвержение сохранения изменений.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            Acceptance = true;
            close = true;
            this.Close();
        }
        /// <summary>
        ///  Отмена внесенных изменений.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Acceptance = false;
            close = true;
            this.Close();
        }
        /// <summary>
        ///  Закрытие формы "Дополнительные изделия".
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void AdditionalWares_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                DialogResult result = MyMessageBox.Show("Сохранить изменения?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveChanges();
                    Acceptance = true;
                }
                else
                { Acceptance = false; }
            }
        }
        /// <summary>
        ///  Сохранить изменения.
        /// </summary>
        private void SaveChanges()
        {
            TimeDiagnostics.Clear();
            TimeCalibration.Clear();
            for (int i = 0; i<gridView1.RowCount; i++)
            {
                TimeDiagnostics.Add(gridView1.GetRowCellValue(i, "DiagnosticsTime"));
                TimeCalibration.Add(gridView1.GetRowCellValue(i, "CalibrationTime"));
            }
        }

        private void AdditionalWares_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  Выбор вида ремонтных работ и доработок.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit1_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int RowIndex = gridView1.FocusedRowHandle;
            if (gridView1.FocusedColumn.Name == RefServiceCard.AdditionalWaresList.WaresNumber)
            {
                if (gridView1.GetRowCellValue(RowIndex, "WaresNumberID").ToString() != "")
                {
                    string PassportID = UniversalCard.GetItemPropertyValue(new Guid(gridView1.GetRowCellValue(RowIndex, "WaresNumberID").ToString()), "Паспорт прибора").ToString();
                    CardData PassportCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(PassportID));
                    Card.CardScript.CardFrame.CardHost.ShowCard(PassportCard.Id, ActivateMode.Edit);
                }
            }
        }
        /// <summary>
        ///  Открытие файла "Протокол калибровки".
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit2_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int RowIndex = gridView1.FocusedRowHandle;
            if (gridView1.FocusedColumn.Name == RefServiceCard.AdditionalWaresList.CalibrationProtocol)
            {
                if (gridView1.GetRowCellValue(RowIndex, "CalibrationProtocolID").ToString() != "")
                {
                    CardData ProtocolCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(gridView1.GetRowCellValue(RowIndex, "CalibrationProtocolID").ToString()));
                    Card.CardScript.CardFrame.CardHost.ShowCard(ProtocolCard.Id, ActivateMode.Edit);
                }
            }
        }
        /// <summary>
        ///  Открытие файла "Сертификата о калибровке".
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void repositoryItemButtonEdit3_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int RowIndex = gridView1.FocusedRowHandle;
            if (gridView1.FocusedColumn.Name == RefServiceCard.AdditionalWaresList.CalibrationCertificate)
            {
                if (gridView1.GetRowCellValue(RowIndex, "CalibrationCertificateID").ToString() != "")
                {
                    CardData CertificateCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(gridView1.GetRowCellValue(RowIndex, "CalibrationCertificateID").ToString()));
                    Card.CardScript.CardFrame.CardHost.ShowCard(CertificateCard.Id, ActivateMode.Edit);
                }
            }
        }
    }
}