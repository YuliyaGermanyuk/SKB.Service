using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsVision.Platform.ObjectManager;
using SKB.Base;

namespace SKB.Service
{
    /// <summary>
    /// Работа по сервисному обслуживанию.
    /// </summary>
    public class Work
    {
        /// <summary>
        /// ID записи в справочнике ремонтных работ и доработок.
        /// </summary>
        public string WorkID = "";
        /// <summary>
        /// Название работы по сервисному обслуживанию.
        /// </summary>
        public string WorkName = "";
        /// <summary>
        /// Количество исполненных работ.
        /// </summary>
        public int Count = 1;
        /// <summary>
        /// Является ли данная работа доработкой.
        /// </summary>
        public bool Improvements = false;
        /// <summary>
        /// ФИО исполнителя работы.
        /// </summary>
        public string Performer = "";
        /// <summary>
        /// ID исполнителя работы.
        /// </summary>
        public string PerformerID = "";
        /// <summary>
        /// Фактическая трудоемкость.
        /// </summary>
        public decimal FactLaboriousness = 0;
        /// <summary>
        /// Дата окончания работы.
        /// </summary>
        public DateTime? EndDate;
        /// <summary>
        /// Результат согласования.
        /// </summary>
        public enum NegotiationResult
        {
            /// <summary>
            /// Не согласовано.
            /// </summary>
            [Description("Не согласовано")]
            NotAgreed = 1,
            /// <summary>
            /// Выполнить.
            /// </summary>
            [Description("Выполнить")]
            Perform = 2,
            /// <summary>
            /// Не выполнять.
            /// </summary>
            [Description("Не выполнять")]
            NotPerform = 3,
        };
        /// <summary>
        /// Результат согласования.
        /// </summary>
        public string Result;
        /// <summary>
        /// Новая работа.
        /// </summary>
        public Work(CardData UniversalCard, string pWorkID, int pCount, string Performer, string PerformerID, decimal FactLaboriousness, DateTime? EndDate, string Result = "Не согласовано")
        {
            this.WorkID = pWorkID;
            this.WorkName = pWorkID == "" ? "" : UniversalCard.GetItemName(pWorkID.ToGuid());
            this.Count = pCount;
            this.Improvements = pWorkID == "" ? false : UniversalCard.GetItemRow(pWorkID.ToGuid()).ChildSections[UniversalCard.Type.AllSections["Properties"].Id].FindRow("@Name = 'Тип работ'").GetString("Value") == "2" ? true : false;
            this.Performer = Performer;
            this.PerformerID = PerformerID;
            this.FactLaboriousness = FactLaboriousness;
            //if (!EndDate.IsNull())
            //    this.EndDate = (DateTime)EndDate;
            this.EndDate = EndDate;
            this.Result = Result;
        }
    }
}
