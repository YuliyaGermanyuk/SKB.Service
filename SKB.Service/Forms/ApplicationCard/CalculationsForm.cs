using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Controls;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.CardHost;
using Microsoft.Office.Interop.Word;
using DocsVision.Platform.ObjectManager.SystemCards;
using SKB.Base;

namespace SKB.Service.Forms.ApplicationCard
{
    /// <summary>
    /// Форма "Калькуляция стоимости сервисного обслуживания".
    /// </summary>
    public partial class CalculationsForm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Карточка Заявки на сервисное обслуживание.
        /// </summary>
        MyBaseCard Card;
        /// <summary>
        /// Суммарная стоимость работ по счету (без НДС).
        /// </summary>
        public decimal Sum = 0;
        /// <summary>
        /// Суммарная стоимость всех выполненных работ (без НДС).
        /// </summary>
        public decimal FullSum = 0;
        /// <summary>
        /// Суммарная стоимость работ по счету (с НДС).
        /// </summary>
        public decimal SumWithNDS = 0;
        /// <summary>
        /// Суммарная стоимость всех выполненных работ (с НДС).
        /// </summary>
        public decimal FullSumWithNDS = 0;
        /// <summary>
        /// Ставка НДС.
        /// </summary>
        public decimal NDS;
        /// <summary>
        /// Сумма НДС по счету.
        /// </summary>
        public decimal SumNDS;
        /// <summary>
        /// Сумма НДС для всех выполненных работ.
        /// </summary>
        public decimal FullSumNDS;
        /// <summary>
        /// ID бланка калькуляции.
        /// </summary>
        public Guid ProtocolID;
        /// <summary>
        /// Изменения.
        /// </summary>
        public ArrayList ChangedRows = new ArrayList();
        /// <summary>
        /// Таблица калькуляции.
        /// </summary>
        public System.Data.DataTable TableItems;

        /// <summary>
        /// Создание формы "Калькуляция стоимости сервисного обслуживания".
        /// </summary>
        /// <param name="Card"> Карточка заявки на сервисное обслуживание.</param>
        /// <param name="NDS"> Ставка НДС.</param>
        /// <param name="ProtocolID"> ID бланка калькуляции.</param>
        /// <param name="Calculation"> Данные калькуляции стоимости сервисного обслуживания.</param>
        public CalculationsForm(MyBaseCard Card, decimal NDS, Guid ProtocolID, ArrayList Calculation)
        {
            this.Card = Card;
            InitializeComponent();
            this.NDS = NDS != 0 ? NDS : Cards.ApplicationCard.GetNDS();
            this.ProtocolID = ProtocolID;
            string Comment = "";

            TableItems = new System.Data.DataTable();
            TableItems.Columns.Add("DeviceName");
            TableItems.Columns.Add("BlockName");
            TableItems.Columns.Add("WorkName");
            TableItems.Columns.Add("Improvements", typeof(bool));
            TableItems.Columns.Add("Price", typeof(decimal));
            TableItems.Columns.Add("Count", typeof(int));
            TableItems.Columns.Add("Cost", typeof(decimal));
            TableItems.Columns.Add("Include", typeof(bool));
            TableItems.Columns.Add("Comment");

            object[] NewItem = new object[9];

            foreach (CalculationItem Item in Calculation)
            {
                NewItem[0] = Item.DeviceName;
                NewItem[1] = Item.BlockName;
                NewItem[2] = Item.WorkName;
                NewItem[3] = Item.Improvement;
                NewItem[4] = Item.Price;
                NewItem[5] = Item.Count;
                NewItem[6] = Item.Cost;
                NewItem[7] = Item.Include;

                Comment = "";
                if (Item.VoidWarranty)
                    Comment = "Гарантия аннулирована! Причина: " + Item.DescriptionOfReason;
                if (Item.DoubleCost)
                    Comment = "Удвоена стоимость ремонта! Причина: " + Item.DescriptionOfReason;
                if (Item.RefusalToRepair)
                    Comment = Comment == "" ? "Отказ от ремонта!" : Comment + " Отказ от ремонта!";
                NewItem[8] = Comment;

                TableItems.Rows.Add(NewItem);

                FullSum = FullSum + (decimal)Item.Cost;
                if (Item.Include)
                    Sum = Sum + (decimal)Item.Cost;
            }

            CalculationGridControl.DataSource = TableItems;

            SumWithNDS =  Sum * (1+NDS/100);
            FullSumWithNDS = FullSum * (1 + NDS / 100);

            this.NDS_Value.EditValue = NDS;
            this.CostWithoutNDS_Value.Value = Math.Round(Sum, 2);
            this.CostWithNDS_Value.Value = Math.Round(SumWithNDS, 2);
            this.CostNDS_Value.Value = Math.Round(Sum * (NDS / 100), 2);

            this.FullCostWithoutNDS_Value.Value = Math.Round(FullSum, 2);
            this.FullCostWithNDS_Value.Value = Math.Round(FullSumWithNDS, 2);
            this.FullCostNDS_Value.Value = Math.Round(FullSum * (NDS / 100), 2);

            if (ProtocolID != null)
            {this.CalculationFile_Value.Text = Card.CardScript.Session.CardManager.GetCardData(ProtocolID).Description;}
        }

        /// <summary>
        ///  Изменение ставки НДС.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void TNDS_EditValueChanged(object sender, EventArgs e)
        {
            NDS = this.NDS_Value.Value;
            SumWithNDS = Math.Round(Sum * (1 + NDS / 100), 2);
            SumNDS = Math.Round(Sum * (NDS / 100), 2);
            FullSumWithNDS = Math.Round(FullSum * (1 + NDS / 100), 2);
            FullSumNDS = Math.Round(FullSum * (NDS / 100), 2);

            this.CostWithNDS_Value.Value = SumWithNDS;
            this.CostNDS_Value.Value = SumNDS;
            this.FullCostWithNDS_Value.Value = FullSumWithNDS;
            this.FullCostNDS_Value.Value = FullSumNDS;
        }
        /// <summary>
        ///  Двойной клик по столбцу "Включено в счет".
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void Calculation_DoubleClick(object sender, EventArgs e)
        {

            if (CalculationGridView.FocusedColumn.Name == this.Include.Name)
            {
                CalculationGridView.SetFocusedRowCellValue(this.Include.Name, !(bool)CalculationGridView.GetFocusedRowCellValue(this.Include.Name));

                Sum = this.CostWithoutNDS_Value.Value;
                decimal Cost = (decimal)CalculationGridView.GetFocusedRowCellValue(CalculationGridView.Columns[this.Cost.Name]);

                if ((bool)CalculationGridView.GetFocusedRowCellValue(CalculationGridView.Columns[this.Include.Name]))
                { Sum += Cost; }
                else
                { Sum -= Cost; }

                this.CostWithoutNDS_Value.Value = Sum;
                this.CostWithNDS_Value.Value = Sum * (1 + (NDS / 100));
                this.CostNDS_Value.Value = Sum * (NDS / 100);
                ChangedRows.Add(CalculationGridView.FocusedRowHandle);
            }
        }
        /// <summary>
        ///  Открытие бланка калькуляции.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void CalculationFileButton_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (ProtocolID != null)
            {
                IVersionedFileCardService VersionedFileCardService = Card.Context.GetService<IVersionedFileCardService>();
                Process.Start(VersionedFileCardService.Download(VersionedFileCardService.OpenCard(ProtocolID)));
            }
        }
        /// <summary>
        ///  Открытие бланка калькуляции.
        /// </summary>
        /// <param name="sender"> sender.</param>
        /// <param name="e"> e.</param>
        private void CalculationFile_DoubleClick(object sender, EventArgs e)
        {
            if (ProtocolID != null)
            {
                IVersionedFileCardService VersionedFileCardService = Card.Context.GetService<IVersionedFileCardService>();
                Process.Start(VersionedFileCardService.Download(VersionedFileCardService.OpenCard(ProtocolID)));
            }
        }
    }
}
