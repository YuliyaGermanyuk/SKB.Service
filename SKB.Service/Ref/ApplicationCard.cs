using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKB.Service.Ref
{
    /// <summary>
    /// Схема карточки "Заявка на сервисное обслуживание".
    /// </summary>
    public static class RefApplicationCard
    {
        /// <summary>
        /// Псевдоним карточки.
        /// </summary>
        public const String Alias = "ApplicationCard";
        /// <summary>
        /// Название карточки.
        /// </summary>
        public const String Name = "Заявка на сервисное обслуживание";
        /// <summary>
        /// Идентификатор типа карточки.
        /// </summary>
        public static readonly Guid ID = new Guid("{B963182D-3C2D-4D44-B38F-5B4B96F91932}");
        /// <summary>
        /// Название правила для получения номера.
        /// </summary>
        public const String NumberRuleName = "СКБ Заявка на сервисное обслуживание";
        /// <summary>
        /// Команды ленты.
        /// </summary>
        public static class RibbonItems
        {
            /// <summary>
            /// Команда "Показать данные о клиенте".
            /// </summary>
            public const String ShowClientInfo = "ShowClientInfo";
            /// <summary>
            /// Команда "Калькуляция".
            /// </summary>
            public const String Calculation = "Calculation";
            /// <summary>
            /// Команда "Печать акта приемки на СО".
            /// </summary>
            public const String PrintAcceptanceAct = "PrintAcceptanceAct";
            /// <summary>
            /// Команда "Печать акта сдачи после СО".
            /// </summary>
            public const String PrintDeliveryAct = "PrintDeliveryAct";
            /// <summary>
            /// Команда "Передать на калибровку".
            /// </summary>
            public const String Calibrate = "Calibrate";
            /// <summary>
            /// Команда "Отозвать из калибровки".
            /// </summary>
            public const String Revoke = "Revoke";
            /// <summary>
            /// Команда "Создать "Договор/счет сбыта"".
            /// </summary>
            public const String CreateAccountCard = "CreateAccountCard";
            /// <summary>
            /// Команда "Передать в отдел сбыта".
            /// </summary>
            public const String Marketing = "Marketing";
        }
        /// <summary>
        /// Кнопки карточки.
        /// </summary>
        public static class Buttons
        {
            /// <summary>
            /// Кнопка "Согласовать".
            /// </summary>
            public const String Reconcile = "Reconcile";
        }
        /// <summary>
        /// Основная секция карточки.
        /// </summary>
        public static class MainInfo
        {
            /// <summary>
            /// Статус.
            /// </summary>
            public enum State
            {
                /// <summary>
                /// Зарегистрирована.
                /// </summary>
                [Description("Зарегистрирована")]
                Registered = 0,
                /// <summary>
                /// В работе.
                /// </summary>
                [Description("В работе")]
                Process = 1,
                /// <summary>
                /// Выполнена.
                /// </summary>
                [Description("Выполнена")]
                Performed = 2,
                /// <summary>
                /// Закрыта.
                /// </summary>
                [Description("Закрыта")]
                Сlosed = 3,
            };
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MainInfo";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{87D6AE8E-3A46-4595-9D97-F9D948939006}");
            /// <summary>
            /// Поле "Номер".
            /// </summary>
            public const String Number = "Number";
            /// <summary>
            /// Поле "Статус".
            /// </summary>
            public const String Status = "Status";
            /// <summary>
            /// Поле "Дата регистрации".
            /// </summary>
            public const String RegDate = "RegDate";
            /// <summary>
            /// Поле "Регистратор".
            /// </summary>
            public const String Recorder = "Recorder";
            /// <summary>
            /// Поле "Наименование организации".
            /// </summary>
            public const String Client = "Client";
            /// <summary>
            /// Поле "Дата поступления приборов".
            /// </summary>
            public const String IncomingDate = "IncomingDate";
            /// <summary>
            /// Поле "Дата выполнения (план)".
            /// </summary>
            public const String DateEndPlan = "DateEndPlan";
            /// <summary>
            /// Поле "Дата выполнения (факт)".
            /// </summary>
            public const String DateEndFact = "DateEndFact";
            /// <summary>
            /// Поле "Адрес возврата приборов".
            /// </summary>
            public const String ReturnAddress = "ReturnAddress";
            /// <summary>
            /// Поле "Телефон".
            /// </summary>
            public const String Phone = "Phone";
            /// <summary>
            /// Поле "E-mail".
            /// </summary>
            public const String Email = "Email";
            /// <summary>
            /// Поле "Контактное лицо".
            /// </summary>
            public const String ContactName = "ContactName";
            /// <summary>
            /// Поле "Ссылки".
            /// </summary>
            public const String Links = "Links";
            /// <summary>
            /// Поле "Комментарий заказчика".
            /// </summary>
            public const String Comment = "Comment";
            /// <summary>
            /// Поле "Сканирование заявки".
            /// </summary>
            public const String ApplicationScan = "ApplicationScan";
            /// <summary>
            /// Поле "Срочный ремонт".
            /// </summary>
            public const String UrgetRepairs = "UrgetRepairs";
            /// <summary>
            /// Поле "Передать на калибровку".
            /// </summary>
            public const String TransferToCalibrate = "TransferToCalibrate";
            /// <summary>
            /// Поле "Файлы".
            /// </summary>
            public const String Files = "Files";
            /// <summary>
            /// Поле "Ответственный исполнитель".
            /// </summary>
            public const String Negotiator = "Negotiator";
            /// <summary>
            /// Поле "Результат согласования".
            /// </summary>
            public const String ResultOfCons = "ResultOfCons";
            /// <summary>
            /// Поле "Сумма (без НДС)".
            /// </summary>
            public const String Sum = "Sum";
            /// <summary>
            /// Поле "Сумма (с НДС)".
            /// </summary>
            public const String SumNDS = "SumNDS";
            /// <summary>
            /// Поле "Протокол согласования цены".
            /// </summary>
            public const String Protocol = "Protocol";
            /// <summary>
            /// Поле "Доставка".
            /// </summary>
            public const String Delivery = "Delivery";
            /// <summary>
            /// Поле "Стоимость доставки".
            /// </summary>
            public const String DeliveryCost = "DeliveryCost";
        }
        /// <summary>
        /// Секция "Сервисное обслуживание".
        /// </summary>
        public static class Service
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Service";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{D6FC1B81-0FD2-4EC7-AB0C-965345A95825}");
            /// <summary>
            /// Поле "Заводской номер прибора".
            /// </summary>
            public const String DeviceNumber = "DeviceNumber";
            /// <summary>
            /// Поле "Гарантийное обслуживание".
            /// </summary>
            public const String WarrantyServices = "WarrantyServices";
            /// <summary>
            /// Поле "Идентификатор номера прибора".
            /// </summary>
            public const String DeviceNumberID = "DeviceNumberID";
            /// <summary>
            /// Поле "Идентификатор".
            /// </summary>
            public const String Id = "Id";
            /// <summary>
            /// Поле "Идентификатор прибора".
            /// </summary>
            public const String DeviceID = "DeviceID";
            /// <summary>
            /// Поле "Наименование прибора".
            /// </summary>
            public const String Device_Name = "Device_Name";
            /// <summary>
            /// Поле "С поверкой".
            /// </summary>
            public const String Verify = "Verify";
            /// <summary>
            /// Поле "Ремонт".
            /// </summary>
            public const String Repair = "Repair";
            /// <summary>
            /// Поле "Калибровка".
            /// </summary>
            public const String Calibrate = "Calibrate";
            /// <summary>
            /// Поле "Перечень ДК".
            /// </summary>
            public const String ACList = "ACList";
            /// <summary>
            /// Поле "Комментарии".
            /// </summary>
            public const String Comments = "Comments";
            /// <summary>
            /// Поле "Только ДК".
            /// </summary>
            public const String AC = "AC";
            /// <summary>
            /// Поле "Требуется помывка".
            /// </summary>
            public const String Wash = "Wash";
            /// <summary>
            /// Поле "Датчики".
            /// </summary>
            public const String Sensors = "Sensors";
            /// <summary>
            /// Поле "Идентификатор наряда на СО".
            /// </summary>
            public const String WorkOrderID = "WorkOrderID";
            /// <summary>
            /// Поле "Идентификатор упаковочного листа".
            /// </summary>
            public const String PackedListID = "PackedListID";
            /// <summary>
            /// Поле "Данные упаковочного листа".
            /// </summary>
            public const String PackedListData = "PackedListData";
        }
        /// <summary>
        /// Секция "Дополнительные комплектующие".
        /// </summary>
        public static class AddComplete
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "AddComplete";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{1D1E8BBD-086A-4B42-9D45-F9551BCD161D}");
            /// <summary>
            /// Поле "Родительская запись".
            /// </summary>
            public const String ParentTableRowId = "ParentTableRowId";
            /// <summary>
            /// Поле "Идентификатор".
            /// </summary>
            public const String Id = "Id";
            /// <summary>
            /// Поле "Наименование".
            /// </summary>
            public const String Name = "Name";
            /// <summary>
            /// Поле "Код СКБ".
            /// </summary>
            public const String Code = "Code";
            /// <summary>
            /// Поле "Количество  ДК".
            /// </summary>
            public const String Count = "Count";
            /// <summary>
            /// Поле "Заказано в производстве".
            /// </summary>
            public const String Ordered = "Ordered";
            /// <summary>
            /// Поле "Примечание".
            /// </summary>
            public const String Comment = "Comment";
        }
        /// <summary>
        /// Секция "Калькуляция".
        /// </summary>
        public static class Calculations
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Calculations";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{07F69E6E-ECAD-4256-A4CB-E7A6A032DC9A}");
        }
    }
}
