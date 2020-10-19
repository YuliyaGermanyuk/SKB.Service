using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKB.Base.Ref
{
    /// <summary>
    /// Схема карточки "Сервисное обслуживание прибора".
    /// </summary>
    public static class RefServiceCard
    {
        /// <summary>
        /// Псевдоним карточки.
        /// </summary>
        public const String Alias = "ServicesCard";
        /// <summary>
        /// Название карточки.
        /// </summary>
        public const String Name = "Сервисное обслуживание прибора";
        /// <summary>
        /// Идентификатор типа карточки.
        /// </summary>
        public static readonly Guid ID = new Guid("{C3548925-A7ED-4BD7-BCBB-ED7066D7C81A}");
        /// <summary>
        /// Команды ленты.
        /// </summary>
        public static class RibbonItems
        {
            /// <summary>
            /// Команда "Отправить в ремонт (через отдел сбыта)".
            /// </summary>
            public const String SendToAdjustment = "SendToAdjustment";
            /// <summary>
            /// Команда "Передать в отдел сбыта".
            /// </summary>
            public const String SendToSale = "SendToSale";
            /// <summary>
            /// Команда "Передать на калибровку".
            /// </summary>
            public const String SendToCalibrate = "SendToCalibrate";
            /// <summary>
            /// Команда "Передать на согласование".
            /// </summary>
            public const String SendToAgreement = "SendToAgreement";
            /// <summary>
            /// Команда "Вернуть".
            /// </summary>
            public const String Return = "Return";
            /// <summary>
            /// Команда "Акт передачи".
            /// </summary>
            public const String ActOfLoad = "ActOfLoad";
            /// <summary>
            /// Команда "Передать на техобслуживание".
            /// </summary>
            public const String SendToMaintenance = "SendToMaintenance";
            /// <summary>
            /// Команда "Передать на поверку".
            /// </summary>
            public const String SendToVerification = "SendToVerification";
            /// <summary>
            /// Команда "Согласовать ремонт".
            /// </summary>
            public const String AgreeToRepair = "AgreeToRepair";
            /// <summary>
            /// Команда "Отказ от ремонта".
            /// </summary>
            public const String FailureToRepair = "FailureToRepair";
            /// <summary>
            /// Команда "Согласовать частично".
            /// </summary>
            public const String PartiallyAgreeToRepair = "PartiallyAgreeToRepair";
            /// <summary>
            /// Команда "Завершить".
            /// </summary>
            public const String Complete = "Complete";
            /// <summary>
            /// Команда "Делегировать".
            /// </summary>
            public const String Delegate = "Delegate";
        }
        /// <summary>
        /// Кнопки карточки.
        /// </summary>
        public static class Buttons
        {
            /// <summary>
            /// Кнопка "Негарантийный случай".
            /// </summary>
            public const String NonWarrantyCase = "NonWarrantyCase";
            /// <summary>
            /// Кнопка "Открыть дополнительные изделия".
            /// </summary>
            public const String OpenSensors = "OpenSensors";
            /// <summary>
            /// Кнопка "Обновить протокол калибровки".
            /// </summary>
            public const String RefreshProtocol = "RefreshProtocol";
            /// <summary>
            /// Кнопка "Обновить сертификат о калибровке".
            /// </summary>
            public const String RefreshCertificate = "RefreshCertificate";
            /// <summary>
            /// Кнопка "Создать протокол калибровки".
            /// </summary>
            public const String CreateProtocol = "CreateProtocol";
            /// <summary>
            /// Кнопка "Создать сертификат о калибровке".
            /// </summary>
            public const String CreateCertificate = "CreateCertificate";
            /// <summary>
            /// Кнопка "Обновить протокол поверки".
            /// </summary>
            public const String RefreshVerificationProtocol = "RefreshVerificationProtocol";
            /// <summary>
            /// Кнопка "Обновить свидетельство о поверке".
            /// </summary>
            public const String RefreshVerificationCertificate = "RefreshVerificationCertificate";
            /// <summary>
            /// Кнопка "Создать протокол поверки".
            /// </summary>
            public const String CreateVerificationProtocol = "CreateVerificationProtocol";
            /// <summary>
            /// Кнопка "Создать свидетельство о поверке".
            /// </summary>
            public const String CreateVerificationCertificate = "CreateVerificationCertificate";
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
                /// На техобслуживании.
                /// </summary>
                [Description("Maintenance")]
                Maintenance = 0,
                /// <summary>
                /// На согласовании.
                /// </summary>
                [Description("Consensus")]
                Consensus = 1,
                /// <summary>
                /// В настройке.
                /// </summary>
                [Description("Adjustment")]
                Adjustment = 2,
                /// <summary>
                /// На повторном техобслуживании.
                /// </summary>
                [Description("Remaintenance")]
                Remaintenance = 3,
                /// <summary>
                /// Отказ от ремонта.
                /// </summary>
                [Description("Failure")]
                Failure = 4,
                /// <summary>
                /// Завершено.
                /// </summary>
                [Description("Completed")]
                Completed = 5,
                /// <summary>
                /// На диагностике.
                /// </summary>
                [Description("Diagnostics")]
                Diagnostics = 6,
                /// <summary>
                /// На калибровке.
                /// </summary>
                [Description("Calibration")]
                Calibration = 7,
                /// <summary>
                /// Ожидание оплаты.
                /// </summary>
                [Description("Payment")]
                Payment = 8,
                /// <summary>
                /// На поверке.
                /// </summary>
                [Description("Verification")]
                Verification = 9,
            };
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MainInfo";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{BE562EAA-CD4D-44A5-BED4-044CF06747C3}");
            /// <summary>
            /// Поле "Режим работы".
            /// </summary>
            public const String Mode = "Mode";
            /// <summary>
            /// Поле "Статус".
            /// </summary>
            public const String Status = "Status";
            /// <summary>
            /// Поле "Дата регистрации".
            /// </summary>
            public const String RegistrationDate = "RegistrationDate";
            /// <summary>
            /// Поле "Менеджер сбыта".
            /// </summary>
            public const String Manager = "Manager";
            /// <summary>
            /// Поле "Заводской номер прибора".
            /// </summary>
            public const String DeviceCardID = "DeviceCardID";
            /// <summary>
            /// Поле "Дата поступления прибора".
            /// </summary>
            public const String IncommingDate = "IncomingDate";
            /// <summary>
            /// Поле "Дата выполнения (план)".
            /// </summary>
            public const String DateEndPlan = "DateEndPlan";
            /// <summary>
            /// Поле "Заводские номера дополнительных изделий".
            /// </summary>
            public const String AdditionalWares = "AdditionalWares";
            /// <summary>
            /// Поле "Наименование организации".
            /// </summary>
            public const String Client = "Client";
            /// <summary>
            /// Поле "Дата выполнения (факт)".
            /// </summary>
            public const String DateEndFact = "DateEndFact";
            /// <summary>
            /// Поле "Только комплектующие".
            /// </summary>
            public const String OnlyA = "OnlyA";
            /// <summary>
            /// Поле "Перечень комплектующих".
            /// </summary>
            public const String AList = "AList";
            /// <summary>
            /// Поле "Тип прибора".
            /// </summary>
            public const String DeviceType = "DeviceType";
            /// <summary>
            /// Поле "Ссылки".
            /// </summary>
            public const String Links = "Links";
            /// <summary>
            /// Поле "Файлы".
            /// </summary>
            public const String Files = "Files";
            /// <summary>
            /// Поле "Комментарий заказчика".
            /// </summary>
            public const String Comment = "Comment";
            /// <summary>
            /// Поле "Данные упаковочного листа".
            /// </summary>
            public const String PackedListData = "PackedListData";
            /// <summary>
            /// Поле "Идентификатор упаковочного листа".
            /// </summary>
            public const String PackedListId = "PackedListId";
            /// <summary>
            /// Поле "Специальные условия по СО и ГО".
            /// </summary>
            public const String SpecialConditions = "SpecialConditions";
        }
        /// <summary>
        /// Секция "Калибровка".
        /// </summary>
        public static class Calibration
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Calibration";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{5A8CC505-7A0B-4CAB-84DD-8C23F26A90D3}");
            /// <summary>
            /// Поле "Калибровщик".
            /// </summary>
            public const String Calibrator = "Calibrator";
            /// <summary>
            /// Поле "Дата передачи на калибровку".
            /// </summary>
            public const String CalDateStart = "CalDateStart";
            /// <summary>
            /// Поле "Запрошенный вид сервиса".
            /// </summary>
            public const String ReqTypeService = "ReqTypeService";
            /// <summary>
            /// Поле "Гарантийное обслуживание".
            /// </summary>
            public const String WarrantyService = "WarrantyService";
            /// <summary>
            /// Поле "Негарантийный случай".
            /// </summary>
            public const String NonWarrantyCase = "NonWarrantyCase";
            /// <summary>
            /// Поле "Внешнее проявление проблемы".
            /// </summary>
            public const String DescriptionOfDefects = "DescriptionOfDefects";
            /// <summary>
            /// Поле "Акт дефектации".
            /// </summary>
            public const String ActDefects = "ActDefects";
            /// <summary>
            /// Поле "Фактический вид сервиса".
            /// </summary>
            public const String TypeServiceFact = "TypeServiceFact";
            /// <summary>
            /// Поле "Внешнее проявление проблемы".
            /// </summary>
            public const String Problem = "Problem";
            /// <summary>
            /// Поле "Необходимые доработки".
            /// </summary>
            public const String Improvements = "Improvements";
            /// <summary>
            /// Поле "Результат согласования ремонта".
            /// </summary>
            public const String ResultOfConsensus = "ResultOfConsensus";
            /// <summary>
            /// Поле "Дата окончания калибровки".
            /// </summary>
            public const String CalibDateEnd = "CalibDateEnd";
            /// <summary>
            /// Поле "Требуется помывка".
            /// </summary>
            public const String Wash = "Wash";
            /// <summary>
            /// Поле "Отдел".
            /// </summary>
            public const String Department = "Department";
            /// <summary>
            /// Поле "Трудоемкость диагностики".
            /// </summary>
            public const String DiagnosticsTime = "DiagnosticsTime";
            /// <summary>
            /// Поле "Трудоемкость калибровки".
            /// </summary>
            public const String CalibrationTime = "CalibrationTime";
            /// <summary>
            /// Поле "Калибровка".
            /// </summary>
            public const String DeviceCalibration = "DeviceCalibration";
            /// <summary>
            /// Поле "Поверка".
            /// </summary>
            public const String Verify = "Verify";
            /// <summary>
            /// Поле "Ремонт прибора".
            /// </summary>
            public const String DeviceRepair = "DeviceRepair";
            /// <summary>
            /// Поле "Ремонт комплектующих".
            /// </summary>
            public const String AccessoriesRepair = "AccessoriesRepair";
            /// <summary>
            /// Поле "Протокол калибровки прибора".
            /// </summary>
            public const String CalibrationProtocol = "CalibrationProtocol";
            /// <summary>
            /// Поле "Сертификат о калибровке прибора".
            /// </summary>
            public const String CalibrationCertificate = "CalibrationCertificate";
            /// <summary>
            /// Поле "Протокол поверки прибора".
            /// </summary>
            public const String VerificationProtocol = "VerificationProtocol";
            /// <summary>
            /// Поле "Свидетельство о поверке прибора".
            /// </summary>
            public const String VerificationCertificate = "VerificationCertificate";
            /// <summary>
            /// Поле "Серийный номер поверки".
            /// </summary>
            public const String VerifySerialNumber = "VerifySerialNumber";
            /// <summary>
            /// Поле "Исполнитель калибровки".
            /// </summary>
            public const String CalibrationPerformer = "CalibrationPerformer";
            /// <summary>
            /// Поле "Дата передачи на калибровку".
            /// </summary>
            public const String CalibrationStartDate = "CalibrationStartDate";
            /// <summary>
            /// Поле "Дата проведения калибровки".
            /// </summary>
            public const String CalibrationEndDate = "CalibrationEndDate";
            /// <summary>
            /// Поле "Исполнитель поверки".
            /// </summary>
            public const String VerificationPerformer = "VerificationPerformer";
            /// <summary>
            /// Поле "Дата передачи на поверку".
            /// </summary>
            public const String VerificationStartDate = "VerificationStartDate";
            /// <summary>
            /// Поле "Дата проведения поверки".
            /// </summary>
            public const String VerificationEndDate = "VerificationEndDate";
            /// <summary>
            /// Поле "Причины непригодности".
            /// </summary>
            public const String CausesOfUnfitness = "CausesOfUnfitness";
        }
        /// <summary>
        /// Секция "Настройка".
        /// </summary>
        public static class Adjustment
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Adjustment";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{8D5E410B-11A0-41DD-9DC9-807C05640748}");
            /// <summary>
            /// Поле "Настройщик".
            /// </summary>
            public const String Adjuster = "Adjuster";
            /// <summary>
            /// Поле "Дата передачи в ремонт".
            /// </summary>
            public const String RepDateStart = "RepDateStart";
            /// <summary>
            /// Поле "Дата окончания ремонта".
            /// </summary>
            public const String RepDateEnd = "RepDateEnd";
            /// <summary>
            /// Поле "Время наработки".
            /// </summary>
            public const String OperatingTime = "OperatingTime";
            /// <summary>
            /// Поле "Работник производства".
            /// </summary>
            public const String Worker = "Worker";
            /// <summary>
            /// Поле "Дата передачи комплектующих в ремонт".
            /// </summary>
            public const String AccDateStart = "AccDateStart";
            /// <summary>
            /// Поле "Дата окончания ремонта комплектующих".
            /// </summary>
            public const String AccDateEnd = "AccDateEnd";
            /// <summary>
            /// Поле "Трудоемкость диагностики".
            /// </summary>
            public const String LaboriousnessDiagnostics = "LaboriousnessDiagnostics";
            /// <summary>
            /// Поле "Трудоемкость ремонта".
            /// </summary>
            public const String LaboriousnessRepair = "LaboriousnessRepair";
            /// <summary>
            /// Поле "Гарантия аннулирована".
            /// </summary>
            public const String VoidWarranty = "VoidWarranty";
            /// <summary>
            /// Поле "Удвоить стоимость ремонта".
            /// </summary>
            public const String DoubleCost = "DoubleCost";
            /// <summary>
            /// Поле "Описание причины".
            /// </summary>
            public const String DescriptionOfReason = "DescriptionOfReason";
            /// <summary>
            /// Поле "Комментарий отдела настройки".
            /// </summary>
            public const String Comment = "FrontAdjustmentComment";
            /// <summary>
            /// Поле "Согласованная стоимость ремонта".
            /// </summary>
            public const String AgreedRepairCost = "AgreedRepairCost";
            /// <summary>
            /// Поле "Исполнитель диагностики".
            /// </summary>
            public const String DiagnosticPerformer = "DiagnosticPerformer";
            /// <summary>
            /// Поле "Дата передачи на диагностики".
            /// </summary>
            public const String DiagnosticDateStart = "DiagnosticDateStart";
            /// <summary>
            /// Поле "Дата окончания диагностики".
            /// </summary>
            public const String DiagnosticDateEnd = "DiagnosticDateEnd";

        }
        /// <summary>
        /// Секция "Описание неисправностей".
        /// </summary>
        public static class DescriptionOfFault
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "DescriptionOfFault";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{7ECF1171-74FE-4E22-921D-4088DEF9BF18}");
            /// <summary>
            /// Поле "Блок прибора".
            /// </summary>
            public const String BlockOfDevice = "BlockOfDevice";
            /// <summary>
            /// Поле "Описание причин неисправности".
            /// </summary>
            public const String Description = "Description";
            /// <summary>
            /// Поле "Классификация неисправностей".
            /// </summary>
            public const String FaultСlassification = "FaultСlassification";
            /// <summary>
            /// Поле "Способ устранения".
            /// </summary>
            public const String CorrectiveActions = "CorrectiveActions";
            /// <summary>
            /// Поле "Комментарий".
            /// </summary>
            public const String Comment = "Comment";
            /// <summary>
            /// Поле "ID блока прибора".
            /// </summary>
            public const String BlockOfDeviceID = "BlockOfDeviceID";
            /// <summary>
            /// Поле "ID классификации неисправностей".
            /// </summary>
            public const String FaultСlassificationID = "FaultСlassificationID";
            /// <summary>
            /// Поле "Перечень ремонтных работ".
            /// </summary>
            public const String RepairWorksList = "RepairWorksList";
            /// <summary>
            /// Поле "Идентификатор".
            /// </summary>
            public const String Id = "Id";
            /// <summary>
            /// Поле "Заводской номер".
            /// </summary>
            public const String SerialNumber = "SerialNumber";
            /// <summary>
            /// Поле "ID заводского номера".
            /// </summary>
            public const String SerialNumberID = "SerialNumberID";
            /// <summary>
            /// Поле "Замена".
            /// </summary>
            public const String Replacement = "Replacement";
            /// <summary>
            /// Поле "ID замены".
            /// </summary>
            public const String ReplacementID = "ReplacementID";
            /// <summary>
            /// Поле "Действие со старым изделием".
            /// </summary>
            public const String OldProductAction = "OldProductAction";
        }
        /// <summary>
        /// Секция "Ремонтные работы".
        /// </summary>
        public static class RepairWorks
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "RepairWorks";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{8B2CFBDD-FCF0-4A65-9677-45345E71AC23}");
            /// <summary>
            /// Поле "Вид работ".
            /// </summary>
            public const String WorksType = "WorksType";
            /// <summary>
            /// Поле "Количество".
            /// </summary>
            public const String Amount = "Amount";
            /// <summary>
            /// Поле "Доработка".
            /// </summary>
            public const String Revision = "Revision";
            /// <summary>
            /// Поле "ID вида работ".
            /// </summary>
            public const String WorksTypeID = "WorksTypeID";
            /// <summary>
            /// Поле "ID родительской записи".
            /// </summary>
            public const String ParentTableRowId = "ParentTableRowId";
            /// <summary>
            /// Поле "Исполнитель".
            /// </summary>
            public const String Performer = "Performer";
            /// <summary>
            /// Поле "Трудоемкость (план)".
            /// </summary>
            public const String PlanLaboriousness = "PlanLaboriousness";
            /// <summary>
            /// Поле "Трудоемкость (факт)".
            /// </summary>
            public const String FactLaboriousness = "FactLaboriousness";
            /// <summary>
            /// Поле "Дата завершения".
            /// </summary>
            public const String EndDate = "EndDate";
            /// <summary>
            /// Поле "Результат согласования".
            /// </summary>
            public const String NegotiationResult = "NegotiationResult";
        }
        /// <summary>
        /// Секция "Необходимые доработки".
        /// </summary>
        public static class ImprovementsTable
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "ImprovementsTable";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{C7C0D6AF-2146-47F1-8D65-75FD49752363}");
            /// <summary>
            /// Название доработки.
            /// </summary>
            public const String ImprovementName = "ImprovementName";
            /// <summary>
            /// Идентификатор доработки.
            /// </summary>
            public const String ImprovementID = "ImprovementID";
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
            public static readonly Guid ID = new Guid("{BDB61771-60E6-4AA2-8EEA-3E3590016487}");
            /// <summary>
            /// Заводской номер изделия.
            /// </summary>
            public const String WaresNumber = "WaresNumber";
            /// <summary>
            /// Идентификатор заводского номера изделия.
            /// </summary>
            public const String WaresNumberID = "WaresNumberID";
            /// <summary>
            /// Трудоемкость диагностики.
            /// </summary>
            public const String DiagnosticsTime = "DiagnosticsTime";
            /// <summary>
            /// Трудоемкость калибровки.
            /// </summary>
            public const String CalibrationTime = "CalibrationTime";
            /// <summary>
            /// Поле "Протокол калибровки доп. изделия".
            /// </summary>
            public const String CalibrationProtocol = "CalibrationProtocol";
            /// <summary>
            /// Поле "Сертификат о калибровке доп. изделия".
            /// </summary>
            public const String CalibrationCertificate = "CalibrationCertificate";
        }
        /// <summary>
        /// Роли процесса
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// Начальник отдела настройки.
            /// </summary>
            public const String AdjastManager = "Начальник отдела настройки";
            /// <summary>
            /// Директор производства.
            /// </summary>
            public const String ProdactionManager = "Директор производства";
            /// <summary>
            /// Зам. начальника отдела калибровки.
            /// </summary>
            public const String DeputyCalibrationManager = "Зам. начальника отдела калибровки";
            /// <summary>
            /// Специалист по сбыту.
            /// </summary>
            public const String SalesManager = "Специалист по сбыту";
            /// <summary>
            /// Группа менеджеров по сервисному обслуживанию.
            /// </summary>
            public const String ServiceManagerGroup = "Специалисты по сервисному обслуживанию";
            /// <summary>
            /// Начальник метрологической лабораториии.
            /// </summary>
            public const String MetrologicalLabManager = "Начальник метрологической лаборатории";

        }
        /// <summary>
        /// Параметры бизнес-процесса "Наряд на сервисное обслуживание прибора/комплектующих"
        /// </summary>
        public static class ParametersOfBusinessProcess
        {
            /// <summary>
            /// Карточка наряда на сервисное обслуживание.
            /// </summary>
            public const String ServiceCard = "Карточка CO";
            /// <summary>
            /// Калибровщик.
            /// </summary>
            public const String Calibrator = "Калибровщик";
            /// <summary>
            /// Менеджер по сбыту.
            /// </summary>
            public const String SalesManager = "Менеджер по сбыту";
            /// <summary>
            /// Дата окончания серивсного обслуживания (план).
            /// </summary>
            public const String EndDatePlan = "Дата исполнения (план)";
            /// <summary>
            /// Нужен ремонт?.
            /// </summary>
            public const String NeedsRepair = "Нужен ремонт?";
            /// <summary>
            /// Карточка паспорта прибора.
            /// </summary>
            public const String PassportCard = "Паспорт";
            /// <summary>
            /// Комментарий заказчика.
            /// </summary>
            public const String CommentOfClient = "Комментарий заказчика";
            /// <summary>
            /// Карточка Заявки на СО.
            /// </summary>
            public const String ApplicationCard = "Карточка Заявки";
            /// <summary>
            /// Номер текущего повтора при разблокировке карточки.
            /// </summary>
            public const String NumberOfCurrentRepeat = "Номер текущего повтора";
            /// <summary>
            /// Кол-во повторов при блокировке карточки.
            /// </summary>
            public const String CountOfRepeats = "Кол-во повторов при блокировке";
            /// <summary>
            /// Ошибка.
            /// </summary>
            public const String Error = "Ошибка";
            /// <summary>
            /// Ненадл. эксплуатация.
            /// </summary>
            public const String ImproperUse = "Ненадл. эксплуатация";
            /// <summary>
            /// Описание дефектов, свидетельствующих о ненадлежащей эксплуатации.
            /// </summary>
            public const String DescriptionOfDefects = "Описание дефектов";
            /// <summary>
            /// Результат согласования с клиентом.
            /// </summary>
            public const String NegotiationResult = "Результат согласования";
            /// <summary>
            /// Решение клиента (ремонтировать/не ремонтировать).
            /// </summary>
            public const String Decision = "Решение";
            /// <summary>
            /// Задание на калибровку.
            /// </summary>
            public const String CalibrationTask = "Задание на калибровку";
            /// <summary>
            /// Описание прибора.
            /// </summary>
            public const String DeviceDescription = "Описание прибора";
            /// <summary>
            /// Дата?
            /// </summary>
            public const String RDate = "Дата";
            /// <summary>
            /// Запрошенный вид сервиса
            /// </summary>
            public const String RequestedTypeService  = "Запрошенный вид сервиса";
            /// <summary>
            /// Гарантийный ремонт
            /// </summary>
            public const String WarrantyRepair = "Гарантийный ремонт";
            /// <summary>
            /// Упаковочный лист
            /// </summary>
            public const String PackingList = "Упаковочный лист";
            /// <summary>
            /// Наименование клиента
            /// </summary>
            public const String ClientName = "Клиент";
            /// <summary>
            /// Текущий исполнитель
            /// </summary>
            public const String CurentPerformer = "Текущий исполнитель";
            /// <summary>
            /// Начальник отдела настройки
            /// </summary>
            public const String AdjustManager = "Начальник отдела настройки";
            /// <summary>
            /// Начальник ОТК
            /// </summary>
            public const String ControlManager = "Начальник ОТК";
            /// <summary>
            /// Фактический вид сервиса (числовое обозначение)
            /// </summary>
            public const String FactTypeService = "Факт вид сервиса";
            /// <summary>
            /// Уведомление об отказе от ремонта
            /// </summary>
            public const String RefusalNotice = "Уведомление об отказе от ремонта";
            /// <summary>
            /// Задание на повторную калибровку
            /// </summary>
            public const String RecalibrationTask = "Задание на повторную калибровку";
            /// <summary>
            /// Состояние наряда на СО
            /// </summary>
            public const String State = "Состояние СО";
            /// <summary>
            /// Внешнее проявление проблемы
            /// </summary>
            public const String ExternalManifestationOfProblem = "Внешнее проявление проблемы";
            /// <summary>
            /// Необходимые доработки
            /// </summary>
            public const String Improvements = "Необходимые доработки";
            /// <summary>
            /// Задание на ремонт прибора
            /// </summary>
            public const String RepairTask = "Задание на ремонт прибора";
            /// <summary>
            /// Завершивший исполнитель
            /// </summary>
            public const String EndedPerformer = "Завершивший исполнитель";
            /// <summary>
            /// Только комплектующие
            /// </summary>
            public const String OnlyAccessories = "Только комплектующие";
            /// <summary>
            /// Датчики
            /// </summary>
            public const String Sensors = "Датчики";
            /// <summary>
            /// Фактический вид сервиса
            /// </summary>
            public const String FactTypeServiseList = "Фактический вид сервиса";
            /// <summary>
            /// Файлы заявки
            /// </summary>
            public const String Files = "Файлы заявки";
            /// <summary>
            /// Исполнитель ремонта комплектующих
            /// </summary>
            public const String CompleteRepairPerformer = "Исполнитель ремонта комплектующих";
            /// <summary>
            /// Требуется ремонт прибора
            /// </summary>
            public const String DeviceRepair = "Ремонт прибора";
            /// <summary>
            /// Требуется ремонт комплектующих
            /// </summary>
            public const String CompleteRepair = "Ремонт комплектующих";
            /// <summary>
            /// Задание на ремонт комплектующих
            /// </summary>
            public const String CompleteRepairTask = "Задание на ремонт комплектующих";
            /// <summary>
            /// Задание на диагностику
            /// </summary>
            public const String DiagnostikTask = "Задание на диагностику";
            /// <summary>
            /// Требуется согласование?
            /// </summary>
            public const String NeedsNegotiation = "Требуется согласование?";
            /// <summary>
            /// Поле "Комментарий отдела настройки".
            /// </summary>
            public const String Comment = "Комментарий отдела настройки";
        }
    }
}