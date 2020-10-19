using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SKB.Base;
using SKB.Base.Synchronize;
using SKB.Service.Ref;
using DocsVision.BackOffice.ObjectModel;

namespace SKB.Service
{
    /// <summary>
    /// Измение в строке таблицы карточки "Заявка на сервисное обслуживание".
    /// </summary>
    internal class ServiceTableChange : MyRowChange
    {
        /// <summary>
        /// Поле «Прибор».
        /// </summary>
        public ChangingValue<Guid> DeviceId { get; private set; }
        /// <summary>
        /// Поле «Номер прибора».
        /// </summary>
        public ChangingValue<Guid> DeviceNumberId { get; private set; }
        /// <summary>
        /// Поле «Гарантия».
        /// </summary>
        public ChangingValue<Boolean> Warranty { get; private set; }
        /// <summary>
        /// Поле «Только ДК».
        /// </summary>
        public ChangingValue<Boolean> AC { get; private set; }
        /// <summary>
        /// Поле «Датчики».
        /// </summary>
        public ChangingValue<String> Sensors { get; private set; }
        /// <summary>
        /// Изменен файл.
        /// </summary>
        public Boolean FileIsChanged { get; set; }
        /// <summary>
        /// Строка изменена.
        /// </summary>
        public override Boolean IsChanged
        {
            get
            {
                return DeviceId.IsChanged || DeviceNumberId.IsChanged || AC.IsChanged || Warranty.IsChanged || Sensors.IsChanged || FileIsChanged;
            }
        }
        ServiceTableChange (Guid RowId) : base(RowId) { }
        public static explicit operator ServiceTableChange (BaseCardProperty Row)
        {
            ServiceTableChange Change = new ServiceTableChange(Row[RefApplicationCard.Service.Id].ToGuid());
            Change.DeviceId = new ChangingValue<Guid>(Row[RefApplicationCard.Service.DeviceID].ToGuid());
            Change.DeviceNumberId = new ChangingValue<Guid>(Row[RefApplicationCard.Service.DeviceNumberID].ToGuid());
            Change.AC = new ChangingValue<Boolean>((Boolean)Row[RefApplicationCard.Service.AC]);
            Change.Warranty = new ChangingValue<Boolean>((Boolean)Row[RefApplicationCard.Service.WarrantyServices]);
            Change.Sensors = new ChangingValue<String>(Row[RefApplicationCard.Service.Sensors] as String);
            Change.FileIsChanged = false;
            return Change;
        }
    }
}
