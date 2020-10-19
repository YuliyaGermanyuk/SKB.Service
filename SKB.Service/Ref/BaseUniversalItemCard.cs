using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKB.Base.Ref
{
    /// <summary>
    /// Схема карточки "Карточка строки справочника".
    /// </summary>
    public static class RefBaseUniversalItemCard
    {
        /// <summary>
        /// Псевдоним карточки.
        /// </summary>
        public const String Alias = "CardBaseUniversalItem";
        /// <summary>
        /// Название карточки.
        /// </summary>
        public const String Name = "Карточка строки справочника";
        /// <summary>
        /// Идентификатор типа карточки.
        /// </summary>
        public static readonly Guid ID = new Guid("{E898C387-0162-4F37-A93C-13BAA07FF183}");
        /// <summary>
        /// Секция "Условия калибровки".
        /// </summary>
        public static class CalibrationConditions
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "CalibrationConditions";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{BB50AF59-B422-4B8A-95AF-A76E04D704B8}");
            /// <summary>
            /// Поле "Дата".
            /// </summary>
            public const String Date = "Date";
            /// <summary>
            /// Поле "Температура".
            /// </summary>
            public const String Temperature = "Temperature";
            /// <summary>
            /// Поле "Атмосферное давление".
            /// </summary>
            public const String Pressure = "Pressure";
            /// <summary>
            /// Поле "Относительная влажность".
            /// </summary>
            public const String Humidity = "Humidity";
            /// <summary>
            /// Поле "Подпись лица, проводившего измерения".
            /// </summary>
            public const String Employee = "Employee";
            /// <summary>
            /// Поле "Номер кабинета".
            /// </summary>
            public const String CabinetNumber = "CabinetNumber";

        }
    }
}