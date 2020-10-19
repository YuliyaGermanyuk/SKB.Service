using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SKB.Base;
using SKB.Base.Synchronize;
using SKB.Base.Ref;
using DocsVision.BackOffice.ObjectModel;

namespace SKB.Service
{
    /// <summary>
    /// Измение в строке таблицы карточки "Заявка на сервисное обслуживание".
    /// </summary>
    internal class DevicesTableChange : MyRowChange
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
        /// Поле «Только ДК».
        /// </summary>
        public ChangingValue<Boolean> AC { get; private set; }
        /// <summary>
        /// Поле «Датчики».
        /// </summary>
        public ChangingValue<String> Sensors { get; private set; }
        /// <summary>
        /// Изменен файл протокола.
        /// </summary>
        public Boolean ProtocolIsChanged { get; set; }
        /// <summary>
        /// Изменен файл сертификата.
        /// </summary>
        public Boolean CertificateIsChanged { get; set; }
        /// <summary>
        /// Строка изменена.
        /// </summary>
        public override Boolean IsChanged
        {
            get
            {
                return DeviceId.IsChanged || DeviceNumberId.IsChanged || AC.IsChanged || Sensors.IsChanged || ProtocolIsChanged || CertificateIsChanged;
            }
        }
        DevicesTableChange(Guid RowId) : base(RowId) { }
        public static explicit operator DevicesTableChange(BaseCardProperty Row)
        {
            DevicesTableChange Change = new DevicesTableChange(Row[RefCertificateCreationCard.Devices.Id].ToGuid());
            Change.DeviceId = new ChangingValue<Guid>(Row[RefCertificateCreationCard.Devices.DeviceTypeId].ToGuid());
            Change.DeviceNumberId = new ChangingValue<Guid>(Row[RefCertificateCreationCard.Devices.DeviceNumberID].ToGuid());
            Change.AC = new ChangingValue<Boolean>((Boolean)Row[RefCertificateCreationCard.Devices.AC]);
            Change.Sensors = new ChangingValue<String>(Row[RefCertificateCreationCard.Devices.AdditionalWares] as String);
            Change.ProtocolIsChanged = false;
            Change.CertificateIsChanged = false;
            return Change;
        }
    }
}
