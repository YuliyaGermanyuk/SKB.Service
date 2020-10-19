using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKB.Service.Cards;
using SKB.Base;
using SKB.Base.Ref;

namespace SKB.Service
{
    /// <summary>
    /// Позиция калькуляции стоимости сервисного обслуживания
    /// </summary>
    public class CalculationItem
    {
        /// <summary>
        /// Название прибора.
        /// </summary>
        public string DeviceName = "";
        /// <summary>
        /// ID записи прибора.
        /// </summary>
        public string DeviceID = "";
        /// <summary>
        /// Название сборочного узла.
        /// </summary>
        public string BlockName = "";
        /// <summary>
        /// ID записи сборочного узла.
        /// </summary>
        public string BlockID = "";
        /// <summary>
        /// Наименование ремонтной работы.
        /// </summary>
        public string WorkName = "";
        /// <summary>
        /// ID записи ремонтной работы.
        /// </summary>
        public string WorkID = "";
        /// <summary>
        /// Флаг доработки.
        /// </summary>
        public bool Improvement = false;
        /// <summary>
        /// Цена за одну работу.
        /// </summary>
        public decimal Price = 0;
        /// <summary>
        /// Количество работ.
        /// </summary>
        public int Count = 0;
        /// <summary>
        /// Общая стоимость работы.
        /// </summary>
        public decimal Cost = 0;
        /// <summary>
        /// Общая стоимость работы.
        /// </summary>
        public bool Include = false;
        /// <summary>
        /// Гарантия аннулирована.
        /// </summary>
        public bool VoidWarranty = false;
        /// <summary>
        /// Удвоена стоимость ремонта.
        /// </summary>
        public bool DoubleCost = false;
        /// <summary>
        /// Описание причины аннулирования гарантии/удвоения стоимости ремонта.
        /// </summary>
        public string DescriptionOfReason = "";
        /// <summary>
        /// Отказ от ремонта.
        /// </summary>
        public bool RefusalToRepair = false;
        /// <summary>
        /// Позиция калькуляции стоимости сервисного обслуживания
        /// </summary>
        public CalculationItem(MyBaseCard Card, string DeviceName, string DeviceID, string BlockName, string BlockID, string WorkName, string WorkID, bool Improvement, int Count, bool Include, 
            bool VoidWarranty, bool DoubleCost, string DescriptionOfReason, bool RefusalToRepair)
        {
            this.DeviceName = DeviceName;
            this.DeviceID = DeviceID;
            this.BlockName = BlockName;
            this.BlockID = BlockID;
            this.WorkName = WorkName;
            this.WorkID = WorkID;
            this.Improvement = Improvement;
            this.VoidWarranty = VoidWarranty;
            this.DoubleCost = DoubleCost;
            this.DescriptionOfReason = DescriptionOfReason;
            this.RefusalToRepair = RefusalToRepair;

            switch (this.WorkName)
            {
                case "":
                    this.Price = 0;
                    break;
                case "Доставка до СКБ":
                    this.Price = Card.GetControlValue(RefApplicationCard.MainInfo.DeliveryCost) == null ? 0 : (decimal)Card.GetControlValue(RefApplicationCard.MainInfo.DeliveryCost);
                    break;
                default:
                    if (this.WorkID != "")
                        this.Price = Convert.ToDecimal(ApplicationCard.UniversalCard.GetItemPropertyValue(new Guid(WorkID), "Стоимость (руб/шт)"));
                    break;
            }
            
            //this.Price = this.WorkID != "" ? Convert.ToDecimal(ApplicationCard.UniversalCard.GetItemPropertyValue(new Guid(WorkID), "Стоимость (руб/шт)")) : 0;

            this.Count = Count;
            this.Cost = Price * Count;
            this.Include = Include;
        }
    }
}
