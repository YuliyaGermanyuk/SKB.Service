using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKB.Base.Ref
{
    /// <summary>
    /// Схема карточки "Заявка на выдачу сертификатов о калибровке".
    /// </summary>
    public static class RefCertificateCreationCard
    {
        /// <summary>
        /// Псевдоним карточки.
        /// </summary>
        public const String Alias = "CertificateCreationCard";
        /// <summary>
        /// Название карточки.
        /// </summary>
        public const String Name = "Заявка на выдачу сертификатов о калибровке";
        /// <summary>
        /// Идентификатор типа карточки.
        /// </summary>
        public static readonly Guid ID = new Guid("{93DA5560-1406-4E48-AFA8-6CA890C9B5C7}");
        /// <summary>
        /// Название правила для получения номера.
        /// </summary>
        public const String CalibrationNumberRuleName = "СКБ Заявка на выдачу сертификатов о калибровке";
        /// <summary>
        /// Название правила для получения номера.
        /// </summary>
        public const String VerificationNumberRuleName = "СКБ Заявка на поверку";
        /// <summary>
        /// Команды ленты.
        /// </summary>
        public static class RibbonItems
        {
            /// <summary>
            /// Команда "Отправить".
            /// </summary>
            public const String Send = "Send";
            /// <summary>
            /// Команда "Делегировать".
            /// </summary>
            public const String Delegate = "Delegate";
            /// <summary>
            /// Команда "Завершить".
            /// </summary>
            public const String Complete = "Complete";
            /// <summary>
            /// Команда "Принять".
            /// </summary>
            public const String Accept = "Accept";
            /// <summary>
            /// Команда "Вернуть".
            /// </summary>
            public const String Revoke = "Revoke";
        }
        /// <summary>
        /// Роли карточки.
        /// </summary>
        public static class UserRoles
        {
            /// <summary>
            /// Администратор.
            /// </summary>
            public const String Admin = "Admin";
            /// <summary>
            /// Все.
            /// </summary>
            public const String AllUsers = "AllUsers";
            /// <summary>
            /// Команда "Регистратор".
            /// </summary>
            public const String Creator = "Creator";
            /// <summary>
            /// Сотрудник отдела сбыта и маркетинга.
            /// </summary>
            public const String SalesDepartmentEmployee = "SalesDepartmentEmployee";
            /// <summary>
            /// Текущий исполнитель
            /// </summary>
            public const String Performer = "Performer";
            /// <summary>
            /// Начальник отдела-исполнителя
            /// </summary>
            public const String FAManager = "FAManager";
        }
        /// <summary>
        /// Основная секция карточки.
        /// </summary>
        public static class MainInfo
        {
            /// <summary>
            /// Состояние.
            /// </summary>
            public enum CardState
            {
                /// <summary>
                /// Не начата.
                /// </summary>
                [Description("NotStarted")]
                NotStarted = 0,
                /// <summary>
                /// В работе.
                /// </summary>
                [Description("InWork")]
                InWork = 1,
                /// <summary>
                /// Выполнена.
                /// </summary>
                [Description("Completed")]
                Completed = 2,
                /// <summary>
                /// Закрыта.
                /// </summary>
                [Description("Accepted")]
                Accepted = 3,
            };
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MainInfo";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{7DEF8BCF-AD22-43C7-8D94-22E4945DAD44}");
            /// <summary>
            /// Поле "Номер".
            /// </summary>
            public const String Number = "Number";
            /// <summary>
            /// Поле "Дата регистрации".
            /// </summary>
            public const String CreationDate = "CreationDate";
            /// <summary>
            /// Поле "Регистратор".
            /// </summary>
            public const String Creator = "Creator";
            /// <summary>
            /// Поле "Дата отправки".
            /// </summary>
            public const String SentDate = "SentDate";
            /// <summary>
            /// Поле "Исполнитель".
            /// </summary>
            public const String Performer = "Performer";
            /// <summary>
            /// Поле "Дата завершения".
            /// </summary>
            public const String EndDate = "EndDate";
            /// <summary>
            /// Поле "Состояние".
            /// </summary>
            public const String State = "State";
            /// <summary>
            /// Поле "Комментарий".
            /// </summary>
            public const String Comment = "Comment";
            /// <summary>
            /// Поле "Тип заявки".
            /// </summary>
            public const String ApplicationType = "ApplicationType";
        }
        /// <summary>
        /// Секция "Приборы".
        /// </summary>
        public static class Devices
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Devices";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{B48E94D2-8AF4-497C-8FEC-D9F06B0EE6AC}");
            /// <summary>
            /// Поле "Идентификатор".
            /// </summary>
            public const String Id = "Id";
            /// <summary>
            /// Поле "Идентификатор типа прибора".
            /// </summary>
            public const String DeviceTypeId = "DeviceTypeID";
            /// <summary>
            /// Поле "Заводской номер прибора".
            /// </summary>
            public const String DeviceNumber = "DeviceNumber";
            /// <summary>
            /// Поле "Идентификатор номера прибора".
            /// </summary>
            public const String DeviceNumberID = "DeviceNumberID";
            /// <summary>
            /// Поле "Дополнительные изделия".
            /// </summary>
            public const String AC = "AC";
            /// <summary>
            /// Поле "Только ДК".
            /// </summary>
            public const String AdditionalWares = "AdditionalWares";
            /// <summary>
            /// Поле "Протокол калибровки".
            /// </summary>
            public const String CalibrationProtocol = "CalibrationProtocol";
            /// <summary>
            /// Поле "Сертификат о калибровке".
            /// </summary>
            public const String CalibrationCertificate = "CalibrationCertificate";
            /// <summary>
            /// Поле "Дата проведения калибровки".
            /// </summary>
            public const String CalibrationDate = "CalibrationDate";
            /// <summary>
            /// Поле "Дата поступления на калибровку".
            /// </summary>
            public const String ReceiptDate = "ReceiptDate";
            /// <summary>
            /// Поле "Серийный номер поверки"
            /// </summary>
            public const String VerifySerialNumber = "VerifySerialNumber";
            /// <summary>
            /// Поле "Причины непригодности"
            /// </summary>
            public const String CausesOfUnfitness = "CausesOfUnfitness";
        }
        /// <summary>
        /// Секция "Дополнительные изделия".
        /// </summary>
        public static class AdditionalWaresList
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "AdditionalWaresList";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{01977D59-3BEC-4625-A29F-E942E884C7BC}");
            /// <summary>
            /// Поле "Идентификатор заводского номера изделия".
            /// </summary>
            public const String WaresNumber = "WaresNumber";
            /// <summary>
            /// Поле "Дата передачи в ремонт".
            /// </summary>
            public const String WaresNumberID = "WaresNumberID";
            /// <summary>
            /// Поле "Протокол калибровки".
            /// </summary>
            public const String CalibrationProtocol = "CalibrationProtocol";
            /// <summary>
            /// Поле "Сертификат о калибровке".
            /// </summary>
            public const String CalibrationCertificate = "CalibrationCertificate";
            /// <summary>
            /// Поле "Идентификатор родительской строки".
            /// </summary>
            public const String ParentTableRowId = "ParentTableRowId";
        }
        /// <summary>
        /// Секция "История исполнения".
        /// </summary>
        public static class History
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "History";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{983D57F6-1081-4B49-978F-210030417DDE}");
            /// <summary>
            /// Поле "Дата события".
            /// </summary>
            public const String EventDate = "EventDate";
            /// <summary>
            /// Поле "Инициатор события".
            /// </summary>
            public const String EventCreator = "EventCreator";
            /// <summary>
            /// Поле "Описание события".
            /// </summary>
            public const String EventDescription = "EventDescription";
        }
        /// <summary>
        /// Роли процесса
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// Начальник отдела настройки.
            /// </summary>
            public const String AdjastManager = SKB.Base.MyHelper.PositionName_MSM;
            /// <summary>
            /// Зам. начальника отдела калибровки.
            /// </summary>
            public const String DeputyAdjastManager = SKB.Base.MyHelper.PositionName_ADMD;
        }
    }
}