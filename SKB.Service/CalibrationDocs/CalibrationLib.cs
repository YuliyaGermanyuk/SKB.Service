using System;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.XtraEditors;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using CardLib = DocsVision.Platform.Cards.Constants;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.ObjectManager.SearchModel;
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.CardLib;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.TakeOffice.Cards.Constants;

using SKB.Base;
using SKB.Base.Ref;
using SKB.Service.Cards;
using DocsVision.Platform.ObjectManager.Metadata;
using System.Globalization;

namespace SKB.Service.CalibrationDocs
{
    /// <summary>
    /// Библиотека методов и констант метрологической лаборатории.
    /// </summary>
    public static class CalibrationLib
    {
        // ********************************
        // ***** Категории документов *****
        // ********************************
        #region
        /// <summary>
        /// Идентификатор категории "ДИ – Данные измерений"
        /// </summary>
        public const string MeasuringDataCategoryID = "{7CD55E06-7BA9-467D-8A3A-89AEC914B5BF}";
        /// <summary>
        /// Идентификатор категории "ПК - Протоколы приемосдаточных испытаний"
        /// </summary>
        public const string AcceptanceTestingProtocolCategoryID = "{937151F3-A501-4DE0-991E-59594D73CBE2}";
        /// <summary>
        /// Идентификатор категории "ПР – Протокол калибровки"
        /// </summary>
        public const string CalibrationProtocolCategoryID = "{3F290A81-39F4-4DEA-83B4-B26F1B569B73}";
        /// <summary>
        /// Идентификатор категории "ПР – Протокол поверки"
        /// </summary>
        public const string VerificationProtocolCategoryID = "{CD7A90AB-4BD5-4F15-B5F7-A695911E594F}";
        /// <summary>
        /// Идентификатор категории "СК - Сертификат о калибровке"
        /// </summary>
        public const string CalibrationCertificateCategoryID = "{03B8BE3B-5ED3-4C9A-9E7D-4C34513A2F37}";
        /// <summary>
        /// Идентификатор категории "СВ - Свидетельство о поверке
        /// </summary>
        public const string VerificationCertificateCategoryID = "{991867CF-8D3E-4A3F-B319-8AB8CDF63739}";
        /// <summary>
        /// Идентификатор категории "ИН - Извещение о непригодности
        /// </summary>
        public const string NoticeOfUnfitnessCategoryID = "{2E4DCCDB-0008-465D-8A7F-A4EEEF3C6E5E}";
        #endregion

        // *******************
        // ***** Шаблоны *****
        // *******************
        #region
        /// <summary>
        /// Идентификаток карточки-шаблона сертификата о калибровке
        /// </summary>
        public const string CertificateTemplateID = "{47860023-54E7-E511-8DE4-00248C0EA807}";
        /// <summary>
        /// Идентификаток карточки-шаблона свидетельства о поверке
        /// </summary>
        public const string VerificationCertificateTemplateID = "{2554D05E-83B8-E711-A205-00248C0EA807}";
        /// <summary>
        /// Идентификаток карточки-шаблона Извещения о непригодности
        /// </summary>
        public const string NoticeOfUnfitnessTemplateID = "{E8FB0ED3-83B8-E711-A205-00248C0EA807}";
        #endregion

        // **********************
        // ***** Нумераторы *****
        // **********************
        #region
        /// <summary>
        /// Название нумератора для сертификатов о калибровке новых приборов.
        /// </summary>
        public const string NewCalibrationCertificateRuleNumbering = "СКБ Сертификат о калибровке (новые приборы)";
        /// <summary>
        /// Название нумератора для сертификатов о калибровке ремонтных приборов.
        /// </summary>
        public const string RepairCalibrationCertificateRuleNumbering = "СКБ Сертификат о калибровке (ремонтные приборы)";
        /// <summary>
        /// Название нумератора для протоколов калибровки приборов.
        /// </summary>
        public const string CalibrationProtocolRuleNumbering = "СКБ Протокол калибровки";
        /// <summary>
        /// Название нумератора для протоколов поверки приборов.
        /// </summary>
        public const string VerificationProtocolRuleNumbering = "СКБ Протокол поверки";
        /// <summary>
        /// Название нумератора для свидетельств о поверке приборов.
        /// </summary>
        public const string VerificationCertificateRuleNumbering = "СКБ Свидетельство о поверке";
        /// <summary>
        /// Название нумератора для извещений о непригодности.
        /// </summary>
        public const string NoticeOfUnfitnessRuleNumbering = "СКБ Извещение о непригодности";
        #endregion

        // ***************************
        // ***** Перечни изделий *****
        // ***************************
        #region
        /// <summary>
        /// Перечень датчиков
        /// </summary>
        public static string[] SensorsList = new string[] { "ДП12", "ДП21", "ДП22" };
        /// <summary>
        /// Перечень доп. изделий, требующих создания отдельного сертификата
        /// </summary>
        public static string[] AdditionalWaresList = new string[] { }; // { "ТК-021", "ТК-026" };
        /// <summary>
        /// Перечень приборов, для которых данные измерений хранятся в отдельном файле
        /// </summary>
        public static string[] MeasuringDataList = new string[] { "МИКО-2.2", "МИКО-2.3", "МИКО-7", "МИКО-7М", "МИКО-7МА", "МИКО-8", "МИКО-8М", "МИКО-8МА", "МИКО-9", "МИКО-9А", "МИКО-21", "МИКО-10", "ПКВ/М7", "ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01" };
        #endregion

        // *********************************
        // ***** Дополнительные данные *****
        // *********************************
        #region
        /// <summary>
        /// Название справочника условий поверки
        /// </summary>
        public const string JournalName = "Журнал условий поверки";
        /// <summary>
        /// Номер кабинета поверки
        /// </summary>
        public const Int32 CabinetNumber = 238;
        /// <summary>
        /// Путь к временной папке
        /// </summary>
        public static string TempFolder = System.IO.Path.GetTempPath();

        /// <summary>
        /// Свойства документа калибровочной лаборатории
        /// </summary>
        public static class DocumentProperties
        {
            /// <summary>
            /// Свойство "Номер".
            /// </summary>
            public const String DocumentNumber = "Номер";
            /// <summary>
            /// Свойство "Дата начала испытаний".
            /// </summary>
            public const String StartDate = "Дата начала испытаний";
            /// <summary>
            /// Свойство "Тип прибора".
            /// </summary>
            public const String DeviceType = "Тип прибора";
            /// <summary>
            /// Свойство "Заводской номер прибора".
            /// </summary>
            public const String DeviceNumber = "Заводской номер прибора";
            /// <summary>
            /// Свойство "Клиент".
            /// </summary>
            public const String Client = "Клиент";
            /// <summary>
            /// Свойство "Серийный номер поверки"
            /// </summary>
            public const String VerifySerialNumber = "Серийный номер поверки";
        }

        /// <summary>
        /// Получение перечня эталонов для калибровки/поверки прибора.
        /// </summary>
        /// <param name="session">Пользовательская сессия.</param>
        /// <param name="DeviceTypeId">Идентификатор типа прибора в универсальном справочнике.</param>
        /// <param name="ForCalibration">Эталоны, используемые для калибровки.</param>
        /// <param name="ForVerify">Эталоны, используемые для поверки.</param>
        /// <returns></returns>
        public static String GetListOfMeasures(UserSession session, Guid DeviceTypeId, Boolean ForCalibration, Boolean ForVerify)
        {
            SearchQuery searchQuery = session.CreateSearchQuery();
            searchQuery.CombineResults = ConditionGroupOperation.And;
            // Тип карточки "Оборудование".
            CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(MyHelper.RefType_EQ);
            // Секция "Метрологическая лаборатория".
            SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(RefEquipmentCard.MetrologicalLaboratory.ID);
            sectionQuery.Operation = SectionQueryOperation.And;
            sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Статус "Эталон".
            sectionQuery.ConditionGroup.Conditions.AddNew(RefEquipmentCard.MetrologicalLaboratory.Status, FieldType.Int, ConditionOperation.Equals, RefEquipmentCard.MetrologicalLaboratory.CardStatus.Gauge);
            // Секция "Используется для приборов".
            SectionQuery sectionQuery2 = typeQuery.SectionQueries.AddNew(RefEquipmentCard.UsedForDevices.ID);
            sectionQuery2.Operation = SectionQueryOperation.And;
            sectionQuery2.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Тип прибора: DeviceTypeId.
            sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Device, FieldType.RefId, ConditionOperation.Equals, DeviceTypeId);
            // Используется для калибровки.
            if (ForCalibration)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Calibration, FieldType.Bool, ConditionOperation.Equals, true);
            // Используется для поверки.
            if (ForVerify)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Verify, FieldType.Bool, ConditionOperation.Equals, true);

            searchQuery.Limit = 0;
            string query = searchQuery.GetXml(null, true);
            CardDataCollection CardCollection = session.CardManager.FindCards(query);

            if (CardCollection.Count == 0)
                return "-";
            else
                return CardCollection.Select(r => r.Sections[RefEquipmentCard.MetrologicalLaboratory.ID].FirstRow.GetString(RefEquipmentCard.MetrologicalLaboratory.DisplayNameInDocuments)).Distinct().Where(r => !String.IsNullOrEmpty(r)).OrderBy(r => r).Aggregate("; ");
        }
        /// <summary>
        /// Получение перечня эталонов для калибровки/поверки прибора.
        /// </summary>
        /// <param name="session">Пользовательская сессия.</param>
        /// <param name="DeviceTypeId">Идентификатор типа прибора в универсальном справочнике.</param>
        /// <param name="ForCalibration">Эталоны, используемые для калибровки.</param>
        /// <param name="ForVerify">Эталоны, используемые для поверки.</param>
        /// <returns></returns>
        public static void GetListOfMeasures(UserSession session, Guid DeviceTypeId, Boolean ForCalibration, Boolean ForVerify, Int32 FirstListLength, out String ListOfMeasures, out String ListOfMeasures2)
        {
            SearchQuery searchQuery = session.CreateSearchQuery();
            searchQuery.CombineResults = ConditionGroupOperation.And;
            // Тип карточки "Оборудование".
            CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(MyHelper.RefType_EQ);
            // Секция "Метрологическая лаборатория".
            SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(RefEquipmentCard.MetrologicalLaboratory.ID);
            sectionQuery.Operation = SectionQueryOperation.And;
            sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Статус "Эталон".
            sectionQuery.ConditionGroup.Conditions.AddNew(RefEquipmentCard.MetrologicalLaboratory.Status, FieldType.Int, ConditionOperation.Equals, RefEquipmentCard.MetrologicalLaboratory.CardStatus.Gauge);
            // Секция "Используется для приборов".
            SectionQuery sectionQuery2 = typeQuery.SectionQueries.AddNew(RefEquipmentCard.UsedForDevices.ID);
            sectionQuery2.Operation = SectionQueryOperation.And;
            sectionQuery2.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Тип прибора: DeviceTypeId.
            sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Device, FieldType.RefId, ConditionOperation.Equals, DeviceTypeId);
            // Используется для калибровки.
            if (ForCalibration)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Calibration, FieldType.Bool, ConditionOperation.Equals, true);
            // Используется для поверки.
            if (ForVerify)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Verify, FieldType.Bool, ConditionOperation.Equals, true);

            searchQuery.Limit = 0;
            string query = searchQuery.GetXml(null, true);
            CardDataCollection CardCollection = session.CardManager.FindCards(query);

            if (CardCollection.Count == 0)
            {
                ListOfMeasures = "";
                ListOfMeasures2 = "";
            }
            else
            {
                IEnumerable<String> IEListOfMeasures = CardCollection.Select(r => r.Sections[RefEquipmentCard.MetrologicalLaboratory.ID].FirstRow.GetString(RefEquipmentCard.MetrologicalLaboratory.DisplayNameInDocuments)).Distinct().Where(r => !String.IsNullOrEmpty(r)).OrderBy(r => r);
                if (IEListOfMeasures.Count() > FirstListLength)
                {
                    ListOfMeasures = IEListOfMeasures.Take(FirstListLength).Aggregate("; ") + ";";
                    ListOfMeasures2 = IEListOfMeasures.Skip(FirstListLength).Aggregate("; ");
                }
                else 
                {
                    ListOfMeasures = IEListOfMeasures.Aggregate("; ");
                    ListOfMeasures2 = "";
                }
            }
        }
        /// <summary>
        /// Получение перечня вспомогательного оборудования для калибровки/поверки прибора.
        /// </summary>
        /// <param name="session">Пользовательская сессия.</param>
        /// <param name="DeviceTypeId">Идентификатор типа прибора в универсальном справочнике.</param>
        /// <param name="ForCalibration">Вспомогательное оборудование, используемые для калибровки.</param>
        /// <param name="ForVerify">Эталоны, используемые для поверки.</param>
        /// <returns></returns>
        public static void GetListOfAuxiliaryMeasures(UserSession session, Guid DeviceTypeId, Boolean ForCalibration, Boolean ForVerify, Int32 FirstListLength, out String ListOfAuxiliaryMeasures, out String ListOfAuxiliaryMeasures2)
        {
            SearchQuery searchQuery = session.CreateSearchQuery();
            searchQuery.CombineResults = ConditionGroupOperation.And;
            // Тип карточки "Оборудование".
            CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(MyHelper.RefType_EQ);
            // Секция "Метрологическая лаборатория".
            SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(RefEquipmentCard.MetrologicalLaboratory.ID);
            sectionQuery.Operation = SectionQueryOperation.And;
            sectionQuery.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Статус - не "Эталон".
            sectionQuery.ConditionGroup.Conditions.AddNew(RefEquipmentCard.MetrologicalLaboratory.Status, FieldType.Int, ConditionOperation.NotEquals, RefEquipmentCard.MetrologicalLaboratory.CardStatus.Gauge);
            // Секция "Используется для приборов".
            SectionQuery sectionQuery2 = typeQuery.SectionQueries.AddNew(RefEquipmentCard.UsedForDevices.ID);
            sectionQuery2.Operation = SectionQueryOperation.And;
            sectionQuery2.ConditionGroup.Operation = ConditionGroupOperation.And;
            // Тип прибора: DeviceTypeId.
            sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Device, FieldType.RefId, ConditionOperation.Equals, DeviceTypeId);
            // Используется для калибровки.
            if (ForCalibration)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Calibration, FieldType.Bool, ConditionOperation.Equals, true);
            // Используется для поверки.
            if (ForVerify)
                sectionQuery2.ConditionGroup.Conditions.AddNew(RefEquipmentCard.UsedForDevices.Verify, FieldType.Bool, ConditionOperation.Equals, true);

            searchQuery.Limit = 0;
            string query = searchQuery.GetXml(null, true);
            CardDataCollection CardCollection = session.CardManager.FindCards(query);

            if (CardCollection.Count == 0)
            {
                ListOfAuxiliaryMeasures = "";
                ListOfAuxiliaryMeasures2 = "";
            }
            else
            {
                IEnumerable<String> IEListOfMeasures = CardCollection.Select(r => r.Sections[RefEquipmentCard.MetrologicalLaboratory.ID].FirstRow.GetString(RefEquipmentCard.MetrologicalLaboratory.DisplayNameInDocuments)).Distinct().Where(r => !String.IsNullOrEmpty(r)).OrderBy(r => r);
                if (IEListOfMeasures.Count() > FirstListLength)
                {
                    ListOfAuxiliaryMeasures = IEListOfMeasures.Take(FirstListLength).Aggregate("; ") + ";";
                    ListOfAuxiliaryMeasures2 = IEListOfMeasures.Skip(FirstListLength).Aggregate("; ");
                }
                else
                {
                    ListOfAuxiliaryMeasures = IEListOfMeasures.Aggregate("; ");
                    ListOfAuxiliaryMeasures2 = "";
                }
            }
        }
        /// <summary>
        /// Получение межповерочного интервала.
        /// </summary>
        /// <param name="UniversalCard">Карточка универсального справочника.</param>
        /// <param name="DeviceTypeId">Идентификатор типа прибора.</param>
        /// <returns></returns>
        public static int GetVerificationInterval(CardData UniversalCard, Guid DeviceTypeId)
        {
            return UniversalCard.GetItemPropertyValue(DeviceTypeId, "Межповерочный интервал (мес)") == null ? 0 : (int)UniversalCard.GetItemPropertyValue(DeviceTypeId, "Межповерочный интервал (мес)");
        }
        /// <summary>
        /// Проверка заполнения журнала калибровки на конкретную дату.
        /// </summary>
        /// <param name="CardScript"> Скрипт карточки. </param>
        /// <param name="Context"> Объектный контекст. </param>
        /// <param name="CalibrationDate"> Дата проведения калибровки. </param>
        /// <returns></returns>
        public static string CheckCalibrationJournal(ScriptClassBase CardScript, ObjectContext Context, DateTime CalibrationDate)
        {
            string ErrorText = "";
            IBaseUniversalService baseUniversalService = Context.GetService<IBaseUniversalService>();
            BaseUniversal baseUniversal = Context.GetObject<BaseUniversal>(RefBaseUniversal.ID);

            if (!baseUniversal.ItemTypes.Any(r => r.Name == JournalName))
            { ErrorText = ErrorText + " - Не найден '" + JournalName + "'.\n"; }

            BaseUniversalItemType JournalItemType = baseUniversal.ItemTypes.First(r => r.Name == JournalName);
            if (!JournalItemType.Items.Any(r => r.Name == "Каб. №" + CabinetNumber + ". Условия на " + CalibrationDate.ToShortDateString()))
            { ErrorText = ErrorText + " - Не заданы условия калибровки/поверки в каб. №" + CabinetNumber + " на дату '" + CalibrationDate.ToShortDateString() + "'.\n"; }

            if (ErrorText == "")
            {
                BaseUniversalItem FindItem = JournalItemType.Items.First(r => r.Name == "Каб. №" + CabinetNumber + ". Условия на " + CalibrationDate.ToShortDateString());
                BaseUniversalItemCard itemCard = baseUniversalService.OpenOrCreateItemCard(FindItem);

                CardData itemCardData = CardScript.Session.CardManager.GetCardData(Context.GetObjectRef<BaseUniversalItemCard>(itemCard).Id);
                SectionData CalibrationConditionsSection = itemCardData.Sections[itemCardData.Type.Sections[RefBaseUniversalItemCard.CalibrationConditions.Alias].Id];
                RowData CalibrationConditionsRow = CalibrationConditionsSection.FirstRow;
                if (CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Temperature) == null)
                { ErrorText = ErrorText + " - Не задана температура в каб. №" + CabinetNumber + " на дату '" + CalibrationDate.ToShortDateString() + "'.\n"; }

                if (CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Humidity) == null)
                { ErrorText = ErrorText + " - Не задана относительная влажность в каб. №" + CabinetNumber + " на дату '" + CalibrationDate.ToShortDateString() + "'.\n"; }

                if (CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Pressure) == null)
                { ErrorText = ErrorText + " - Не задано атмосферное давление в каб. №" + CabinetNumber + " на дату '" + CalibrationDate.ToShortDateString() + "'.\n"; }
            }
            return ErrorText;
        }
        /// <summary>
        /// Получение условий калибровки на конкретную дату.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="Context"> Объектный контекст. </param>
        /// <param name="CalibrationDate"> Дата калибровки. </param>
        /// <param name="Temperature"> Температура. </param>
        /// <param name="Humidity"> Относительная влажность. </param>
        /// <param name="AtmospherePressure"> Атмосферное давление. </param>
        public static void GetCalibrationConditions(UserSession userSession, ObjectContext Context, DateTime CalibrationDate, out string Temperature, out string Humidity, out string AtmospherePressure)
        {
            IBaseUniversalService baseUniversalService = Context.GetService<IBaseUniversalService>();

            BaseUniversal baseUniversal = Context.GetObject<BaseUniversal>(RefBaseUniversal.ID);
            BaseUniversalItemType JournalItemType = baseUniversal.ItemTypes.First(r => r.Name == JournalName);
            BaseUniversalItem FindItem = JournalItemType.Items.First(r => r.Name == "Каб. №" + CabinetNumber + ". Условия на " + CalibrationDate.ToShortDateString());
            BaseUniversalItemCard itemCard = baseUniversalService.OpenOrCreateItemCard(FindItem);

            CardData itemCardData = userSession.CardManager.GetCardData(Context.GetObjectRef<BaseUniversalItemCard>(itemCard).Id);
            SectionData CalibrationConditionsSection = itemCardData.Sections[itemCardData.Type.Sections[RefBaseUniversalItemCard.CalibrationConditions.Alias].Id];
            RowData CalibrationConditionsRow = CalibrationConditionsSection.FirstRow;

            Temperature = CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Temperature).ToString();
            Humidity = CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Humidity).ToString();
            AtmospherePressure = CalibrationConditionsRow.GetDecimal(RefBaseUniversalItemCard.CalibrationConditions.Pressure).ToString();

            return;
        }
        /// <summary>
        /// Проверка документа определенной категории на соответствие требованиям.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="cardData"> Карточка паспорта прибора. </param>
        /// <param name="StartDateOfService"> Дата поступления на калибровку. </param>
        /// <param name="CategoryID"> Идентификатор категории документа. </param>
        /// <param name="IsNewDevice"> Признак нового прибора. </param>
        /// <returns></returns>
        public static string CheckDocument(UserSession userSession, CardData cardData, DateTime StartDateOfService, string CategoryID, bool IsNewDevice)
        {
            string ErrorText = "";

            Guid FilesID = (Guid)cardData.Sections[CardOrd.MainInfo.ID].FirstRow.GetGuid(CardOrd.MainInfo.FilesID);
            CardData cardFiles = userSession.CardManager.GetCardData(FilesID);
            IEnumerable<CardData> Files = cardFiles.Sections[FileList.FileReferences.ID].Rows.Select(r => userSession.CardManager.GetCardData((Guid)r.GetGuid(FileList.FileReferences.CardFileID)));
            IEnumerable<CardData> Documents = Files.Where(r => (r.Sections[CardFile.Categories.ID].Rows.Count > 0) && ((Guid)r.Sections[CardFile.Categories.ID].FirstRow.GetGuid(CardFile.Categories.CategoryID) == new Guid(CategoryID)));

            if (Documents.Count() == 0) // Не найдено ни одного документа
            {
                ErrorText = ErrorText + "   - Не найдено ни одного документа.\n";
                return ErrorText;
            }

            Documents = Documents.Where(r => r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'") != null);
            if (Documents.Count() == 0) // Не найдено ни одного документа
            {
                ErrorText = ErrorText + "   - Не найдено ни одного документа.\n";
                return ErrorText;
            }

            DateTime LastDocumentsDate = Documents.Max(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value));
            if ((!IsNewDevice) && (LastDocumentsDate < StartDateOfService)) // Не найдено ни одного протокола ПСИ, созданного после поступления прибора на ремонт
            {
                ErrorText = ErrorText + "   - Не найдено ни одного документа, созданного после поступления прибора на ремонт.\n";
                return ErrorText;
            }

            CardData LastDocument = Documents.Where(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value)
                == LastDocumentsDate).First<CardData>();
            Guid LastDocumentCategoryID = LastDocument.Sections[CardFile.Categories.ID].FirstRow.GetGuid(CardFile.Categories.CategoryID).ToGuid();
            string DocumentName = LastDocument.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);

            // Для документа типа "ПР - Протокол калибровки" обязательно расширение ".docm"
            if ((LastDocumentCategoryID == new Guid(CalibrationProtocolCategoryID)) && (!DocumentName.EndsWith(".docm")))
            {
                ErrorText = ErrorText + "   - Расширение протокола неверно.\n";
                return ErrorText;
            }

            // Проверка заполнения реквизитов в протоколе калировки
            if (LastDocumentCategoryID == new Guid(CalibrationProtocolCategoryID))
            {
                if (!TestFileFilds(userSession, LastDocument))
                { ErrorText = ErrorText + "   - В протоколе ПСИ не заполены требуемые реквизиты\n"; }
            }

            // Проверка заполнения реквизитов в протоколе поверки
            if (LastDocumentCategoryID == new Guid(VerificationProtocolCategoryID))
            {
                if (!TestFileFilds(userSession, LastDocument))
                { ErrorText = ErrorText + "   - В протоколе поверки не заполены требуемые реквизиты\n"; }
            }

            return ErrorText;
        }
        /// <summary>
        /// Получение документа определенной категории.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="cardData"> Карточка паспорта прибора. </param>
        /// <param name="CategoryID"> Идентификатор категории документа. </param>
        /// <param name="TempPath"> Путь к временной папке. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetDocument(UserSession userSession, CardData cardData, string CategoryID, out string TempPath)
        {
            Guid FilesID = (Guid)cardData.Sections[CardOrd.MainInfo.ID].FirstRow.GetGuid(CardOrd.MainInfo.FilesID);
            CardData cardFiles = userSession.CardManager.GetCardData(FilesID);
            IEnumerable<CardData> Files = cardFiles.Sections[FileList.FileReferences.ID].Rows.Select(r => userSession.CardManager.GetCardData((Guid)r.GetGuid(FileList.FileReferences.CardFileID)));
            IEnumerable<CardData> Documents = Files.Where(r => ((Guid)r.Sections[CardFile.Categories.ID].FirstRow.GetGuid(CardFile.Categories.CategoryID) == new Guid(CategoryID)));

            Documents = Documents.Where(r => r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'") != null);

            DateTime LastDocumentsDate = Documents.Max(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value));
            CardData LastDocument = Documents.Where(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value)
                == LastDocumentsDate).First<CardData>();
            string DocumentName = LastDocument.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);
            
            string TempFolder = System.IO.Path.GetTempPath();
            //TempPath = TempFolder + "\\" + DocumentName;
            TempPath = TempFolder + DocumentName;

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)LastDocument.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument DocumentFile = WordprocessingDocument.Open(TempPath, true);

            return DocumentFile;
        }
        /// <summary>
        /// Получение документа определенной категории.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="cardData"> Карточка паспорта прибора. </param>
        /// <param name="CategoryID"> Идентификатор категории документа. </param>
        /// <returns></returns>
        public static string GetDocumentPath(UserSession userSession, CardData cardData, string CategoryID)
        {
            string TempPath = "";
            Guid FilesID = (Guid)cardData.Sections[CardOrd.MainInfo.ID].FirstRow.GetGuid(CardOrd.MainInfo.FilesID);
            CardData cardFiles = userSession.CardManager.GetCardData(FilesID);
            IEnumerable<CardData> Files = cardFiles.Sections[FileList.FileReferences.ID].Rows.Select(r => userSession.CardManager.GetCardData((Guid)r.GetGuid(FileList.FileReferences.CardFileID)));
            IEnumerable<CardData> Documents = Files.Where(r => ((Guid)r.Sections[CardFile.Categories.ID].FirstRow.GetGuid(CardFile.Categories.CategoryID) == new Guid(CategoryID)));

            Documents = Documents.Where(r => r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'") != null);

            DateTime LastDocumentsDate = Documents.Max(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value));
            CardData LastDocument = Documents.Where(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value)
                == LastDocumentsDate).First<CardData>();
            string DocumentName = LastDocument.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);

            string TempFolder = System.IO.Path.GetTempPath();
            //TempPath = TempFolder + "\\" + DocumentName;
            TempPath = TempFolder + DocumentName;

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)LastDocument.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            return TempPath;
        }
        /// <summary>
        /// Получение карточки файла документа определенной категории.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="Passport"> Карточка паспорта прибора. </param>
        /// <param name="CategoryID"> Идентификатор категории документа. </param>
        /// <returns></returns>
        public static CardData GetDocumentCard(UserSession userSession, CardData Passport, string CategoryID)
        {
            Guid FilesID = (Guid)Passport.Sections[CardOrd.MainInfo.ID].FirstRow.GetGuid(CardOrd.MainInfo.FilesID);
            CardData cardFiles = userSession.CardManager.GetCardData(FilesID);
            IEnumerable<CardData> Files = cardFiles.Sections[FileList.FileReferences.ID].Rows.Select(r => userSession.CardManager.GetCardData((Guid)r.GetGuid(FileList.FileReferences.CardFileID)));
            IEnumerable<CardData> Documents = Files.Where(r => ((Guid)r.Sections[CardFile.Categories.ID].FirstRow.GetGuid(CardFile.Categories.CategoryID) == new Guid(CategoryID)));
            Documents = Documents.Count() > 0 ? Documents.Where(r => r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'") != null) : Documents;

            if (Documents.Count() > 0)
            {
                DateTime LastDocumentsDate = Documents.Max(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value));
                CardData LastDocument = Documents.Where(r => (DateTime)r.Sections[CardFile.Properties.ID].FindRow("@Name = 'Дата начала испытаний'").GetDateTime(CardFile.Properties.Value)
                == LastDocumentsDate).First<CardData>();
                return LastDocument;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получение текстового представления заводского номера прибора (формат: [Номер]/[Год] или [Номер][Литера]).
        /// </summary>
        /// <param name="DeviceCard"> Карточка паспорта прибора. </param>
        /// <returns></returns>
        public static string GetDeviceNumber(this CardData DeviceCard)
        {
            string DeviceNumber = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Заводской номер прибора'").GetString(CardOrd.Properties.Value).ToString();
            string DeviceYear = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = '/Год прибора'").GetString(CardOrd.Properties.Value).ToString();
            DeviceNumber = DeviceNumber.Length >= 4 ? DeviceNumber : DeviceNumber + "/" + DeviceYear;
            return DeviceNumber;
        }
        /// <summary>
        /// Получение заводского номера и года выпуска прибора.
        /// </summary>
        /// <param name="DeviceCard"></param>
        /// <param name="DeviceNumber"></param>
        /// <param name="DeviceYear"></param>
        public static void GetDeviceNumber(this CardData DeviceCard, out string DeviceNumber, out string DeviceYear)
        {
            DeviceNumber = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Заводской номер прибора'").GetString(CardOrd.Properties.Value).ToString();
            DeviceYear = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = '/Год прибора'").GetString(CardOrd.Properties.Value).ToString();
            return;
        }
        /// <summary>
        /// Получение типа прибора.
        /// </summary>
        /// <param name="DeviceCard"> Картока паспорта прибора. </param>
        /// <returns></returns>
        public static string GetDeviceTypeID(this CardData DeviceCard)
        {
            string DeviceType = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToString();
            return DeviceType;
        }
        /// <summary>
        /// Получение текстового представления даты выпуска прибора (для нового прибора - дата поступления на склад, для старого прибора - год).
        /// </summary>
        /// <param name="DeviceCard"> Карточка паспорта прибора. </param>
        /// <returns></returns>
        public static string GetDeviceDateOfIssue(this CardData DeviceCard)
        {
            RowData ActionDate = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Дата начала'");
            RowData ActionDescription = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Действие с прибором'");

            if (ActionDate.ChildSections[CardOrd.SelectedValues.ID].Rows.Count > 0)
            {
                RowData CurrentActionDate = ActionDate.ChildSections[CardOrd.SelectedValues.ID].FirstRow;
                RowData CurrentActionDescription = ActionDescription.ChildSections[CardOrd.SelectedValues.ID].FindRow("@Order = '" + CurrentActionDate.GetInt32(CardOrd.SelectedValues.Order) + "'");
                if (CurrentActionDescription != null && CurrentActionDescription.GetString(CardOrd.SelectedValues.SelectedValue) == "Принят на калибровку")
                {
                    return ((DateTime)CurrentActionDate.GetDateTime(CardOrd.SelectedValues.SelectedValue)).ToShortDateString() + "г.";
                }
            }
            return DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = '/Год прибора'").GetString(CardOrd.Properties.Value) + "г.";
        }
        /// <summary>
        /// Получение шаблона протокола калибровки.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="DeviceType"> Тип прибора. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetCalibrationProtocolTemplate(UserSession userSession, string DeviceType)
        {
            CardData UniversalReference = userSession.CardManager.GetDictionaryData(RefUniversal.ID);
            RowData DevicesType = UniversalReference.Sections[RefUniversal.ItemType.ID].FindRow("@Name = 'Приборы и комплектующие'");
            RowData DeviceTypeRow = DevicesType.ChildSections[RefUniversal.Item.ID].FindRow("@Name = '" + DeviceType + "'");
            RowData ProtocolProperty = DeviceTypeRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Протокол калибровки'");
            CardData ProtocolTemplate = userSession.CardManager.GetCardData((Guid)ProtocolProperty.GetGuid(RefUniversal.Properties.Value));

            string TempFolder = System.IO.Path.GetTempPath();
            string TempPath = TempFolder + "\\" + "Протокол калибровки " + DeviceType.Replace("/", "-") + ".docm";
            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)ProtocolTemplate.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);
            return ProtocolDocument;
        }
        /// <summary>
        /// Получение шаблона протокола поверки.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="DeviceType"> Тип прибора. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetVerificationProtocolTemplate(UserSession userSession, string DeviceType)
        {
            CardData UniversalReference = userSession.CardManager.GetDictionaryData(RefUniversal.ID);
            RowData DevicesType = UniversalReference.Sections[RefUniversal.ItemType.ID].FindRow("@Name = 'Приборы и комплектующие'");
            RowData DeviceTypeRow = DevicesType.ChildSections[RefUniversal.Item.ID].FindRow("@Name = '" + DeviceType + "'");
            int FormMode = (Int32)DeviceTypeRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Формирование протокола поверки'").GetInt32(RefUniversal.Properties.Value);
            RowData ProtocolProperty = FormMode == 1 ? DeviceTypeRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Протокол поверки (ручное заполнение)'") :
                DeviceTypeRow.ChildSections[RefUniversal.Properties.ID].FindRow("@Name = 'Протокол поверки (автоматическое заполнение)'");
            CardData ProtocolTemplate = userSession.CardManager.GetCardData((Guid)ProtocolProperty.GetGuid(RefUniversal.Properties.Value));

            string TempFolder = System.IO.Path.GetTempPath();
            string TempPath = TempFolder + "\\" + "Протокол поверки " + DeviceType.Replace("/", "-") + ".docm";
            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)ProtocolTemplate.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);
            return ProtocolDocument;
        }
        /// <summary>
        /// Получение шаблона сертификата о калибровке
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetCalibrationCertificateTemplate(UserSession userSession)
        {
            CardData CertificateTemplate = userSession.CardManager.GetCardData(new Guid(CertificateTemplateID));
            string TempPath = TempFolder + "\\" + "Сертификат о калибровке.docx";

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)CertificateTemplate.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);
            return ProtocolDocument;
        }
        /// <summary>
        /// Получение шаблона свидетельства о поверке
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetVerificationCertificateTemplate(UserSession userSession)
        {
            CardData CertificateTemplate = userSession.CardManager.GetCardData(new Guid(VerificationCertificateTemplateID));
            string TempPath = TempFolder + "\\" + "Свидетельство о поверке.docx";

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)CertificateTemplate.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument CertificateDocument = WordprocessingDocument.Open(TempPath, true);
            return CertificateDocument;
        }
        /// <summary>
        /// Получение шаблона извещения о непригодности
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <returns></returns>
        public static WordprocessingDocument GetNoticeOfUnfitnessTemplate(UserSession userSession)
        {
            CardData NoticeOfUnfitnessTemplate = userSession.CardManager.GetCardData(new Guid(NoticeOfUnfitnessTemplateID));
            string TempPath = TempFolder + "\\" + "Извещение о непригодности.docx";

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)NoticeOfUnfitnessTemplate.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument NoticeDocument = WordprocessingDocument.Open(TempPath, true);
            return NoticeDocument;
        }
        #endregion

        // *********************************
        // ***** Работа с нумераторами *****
        // *********************************
        #region
        /// <summary>
        /// Получить номер сертификата о калибровке для нового прибора.
        /// </summary>
        /// <param name="Context"> Объектный контекст. </param>
        /// <param name="CalibrationDate"> Дата проведения калибровки </param>
        /// 
        public static string GetNewCalibrationCertificateNumber(this ObjectContext Context, DateTime CalibrationDate)
        {
            return "1-" + Context.GetNumber(NewCalibrationCertificateRuleNumbering, true, "Y" + CalibrationDate.Year).ToString() + "/" + CalibrationDate.Year.ToString().Substring(2, 2);
        }
        /// <summary>
        /// Получить номер сертификата о калибровке для ремонтного прибора.
        /// </summary>
        /// <param name="Context"> Объектный контекст.</param>
        /// <returns></returns>
        public static string GetRepairCalibrationCertificateNumber(this ObjectContext Context)
        {
            return "2-" + Context.GetNumber(RepairCalibrationCertificateRuleNumbering, true).ToString() + "/" + DateTime.Today.Year.ToString().Substring(2, 2);
        }
        /// <summary>
        /// Получить номер протокола калибровки для прибора.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetCalibrationProtocolNumber(this ObjectContext Context)
        {
            return Context.GetNumber(CalibrationProtocolRuleNumbering, true).ToString();
        }
        /// <summary>
        /// Получить номер протокола поверки для прибора.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetVerificationProtocolNumber(this ObjectContext Context)
        {
            return Context.GetNumber(VerificationProtocolRuleNumbering, false).ToString();
        }
        /// <summary>
        /// Получить номер свидетельства о поверке.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetVerificationCertificateNumber(this ObjectContext Context)
        {
            return Context.GetNumber(VerificationCertificateRuleNumbering, false).ToString();
        }
        /// <summary>
        /// Получить номер извещения о непригодности.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetNoticeOfUnfitnessNumber(this ObjectContext Context)
        {
            return Context.GetNumber(NoticeOfUnfitnessRuleNumbering, true).ToString();
        }

        /// <summary>
        /// Возвращает номер, выданный для указанного Нумератора из Справочника нумераторов.
        /// </summary>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="NumeratorName">Название нумератора.</param>
        /// <param name="UseZones">Учитывать зоны номеров.</param>
        /// <param name="ZoneName">Название зоны номеров.</param>
        /// <returns></returns>
        public static Int32 GetNumber(this ObjectContext Context, String NumeratorName, Boolean UseZones = false, string ZoneName = "")
        {
            Guid NumberId;
            return Context.GetNumber(NumeratorName, out NumberId, UseZones, ZoneName);
        }
        /// <summary>
        /// Возвращает номер, выданный для указанного Нумератора из Справочника нумераторов.
        /// </summary>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="NumeratorName">Название нумератора.</param>
        /// <param name="NumberId">Идентификатор, выданного номера.</param>
        /// <param name="UseZones">Учитывать зоны номеров.</param>
        ///  <param name="ZoneName">Название зоны номеров.</param>
        /// <returns></returns>
        public static Int32 GetNumber(this ObjectContext Context, String NumeratorName, out Guid NumberId, Boolean UseZones = false, string ZoneName = "")
        {
            UserSession Session = Context.GetService<UserSession>();
            CardData NumeratorsData = Session.CardManager.GetCardData(MyHelper.RefNumerators);
            RowData NumeratorRow = NumeratorsData.Sections[MyHelper.RefNumeratorsNumerators].Rows.Find("Name", NumeratorName);
            NumberId = Guid.Empty;
            if (!NumeratorRow.IsNull())
            {
                Guid NumeratorId = NumeratorRow.GetGuid("NumeratorID") ?? Guid.Empty;
                if (!NumeratorId.IsEmpty())
                {
                    NumeratorCard Numerator = (NumeratorCard)Session.CardManager.GetCard(NumeratorId);
                    ZoneName = ZoneName == "" ? "Y" + DateTime.Now.Year : ZoneName;
                    NumeratorZone Zone = UseZones ? Numerator.Zones.FirstOrDefault(z => z.Name == ZoneName) : Numerator.Zones.FirstOrDefault();
                    if (Zone.IsNull())
                        Zone = Numerator.Zones.AddNew(ZoneName);
                    return Zone.GetNumber(Context.GetCurrentUser(), true, out NumberId);
                }
            }
            return 0;
        }
        #endregion

        // ***************************************
        // ***** Работа с файлами документов *****
        // ***************************************
        #region
        /// <summary>
        /// Поиск свойства в документе
        /// </summary>
        /// <param name="Document"> Карточка документа </param>
        /// <param name="PropertyName"> Название свойства. </param>
        public static object GetDocumentProperty(this CardData Document, string PropertyName)
        {
            try
            {
                return Document.Sections[CardFile.Properties.ID].FindRow("@Name = '" + PropertyName + "'").GetObject(CardFile.Properties.Value);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Изменение свойства в документе
        /// </summary>
        /// <param name="Document"> Карточка документа </param>
        /// <param name="PropertyName"> Название свойства. </param>
        /// <param name="PropertyValue"> Значение свойства. </param>
        public static void SetDocumentProperty(this CardData Document, string PropertyName, string PropertyValue)
        {
            try
            {
                if (Document.Sections[CardFile.Properties.ID].FindRow("@Name = '" + PropertyName + "'").IsNull())
                {
                    RowData NewRow = Document.Sections[CardFile.Properties.ID].Rows.AddNew();
                    NewRow.SetString(CardFile.Properties.Name, PropertyName);
                    NewRow.SetInt32(CardFile.Properties.ParamType, 0);
                    NewRow.SetString(CardFile.Properties.Value, PropertyValue);
                    NewRow.SetString(CardFile.Properties.DisplayValue, PropertyValue);
                }
                else
                {
                    RowData FindRow = Document.Sections[CardFile.Properties.ID].FindRow("@Name = '" + PropertyName + "'");
                    FindRow.SetString(CardFile.Properties.Value, PropertyValue);
                    FindRow.SetString(CardFile.Properties.DisplayValue, PropertyValue);
                }
                
            }
            catch (MyException Ex)
            {
                XtraMessageBox.Show("Внимание! При записи свойства '" + PropertyName + "' в документ произошла ошибка: " + Ex.Message);
            }
        }
        /// <summary>
        /// Запись номера в документ
        /// </summary>
        /// <param name="FullDocumentName"> Полный путь к документу. </param>
        /// <param name="Number"> Номер документа. </param>
        public static void SetDocumentNumber(string FullDocumentName, string Number)
        {
            WordprocessingDocument Document = WordprocessingDocument.Open(FullDocumentName, true);
            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Document.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            if (BookmarkDic.Keys.Any(r => r == "Number"))
            {
                BookmarkDic["Number"].WriteText(Number);
            }
            else
            {
                Dictionary<String, BookmarkStart> HeaderBookmarkDic = new Dictionary<String, BookmarkStart>();
                foreach (HeaderPart header in Document.MainDocumentPart.HeaderParts)
                    foreach (BookmarkStart Bookmark in header.RootElement.Descendants<BookmarkStart>())
                        HeaderBookmarkDic[Bookmark.Name] = Bookmark;
                if (HeaderBookmarkDic.Keys.Any(r => r == "Number")) HeaderBookmarkDic["Number"].WriteText(Number);
            }
            Document.MainDocumentPart.Document.Save();
            Document.Close();
        }
        /// <summary>
        /// Запись связанного документа в документ
        /// </summary>
        /// <param name="FullDocumentName"> Полный путь к документу. </param>
        /// <param name="ResultDocumentName"> Название связанного документа. </param>
        /// <param name="DocumentTerm"> Срок действия связанного документа. </param>
        public static void SetResultDocumentName(string FullDocumentName, string ResultDocumentName, string DocumentTerm)
        {
            WordprocessingDocument Document = WordprocessingDocument.Open(FullDocumentName, true);
            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Document.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            if (BookmarkDic.Keys.Any(r => r == "ResultDocument"))
            {
                BookmarkDic["ResultDocument"].WriteText(ResultDocumentName);
            }
            if (BookmarkDic.Keys.Any(r => r == "DocumentTerm"))
            {
                BookmarkDic["DocumentTerm"].WriteText(DocumentTerm);
            }

            Document.MainDocumentPart.Document.Save();
            Document.Close();
        }
        /// <summary>
        /// формирование имени сертификата о калибровке
        /// </summary>
        /// <param name="CertificateNumber"> Номер сертификата о калибровке </param>
        /// <returns></returns>
        public static string GetCalibrationCertificateName(string CertificateNumber)
        {
            //return TempFolder + "\\" + "Сертификат о калибровке № " + CertificateNumber.Replace("/", "-") + ".docx";
            return TempFolder + "Сертификат о калибровке № " + CertificateNumber.Replace("/", "-") + ".docx";
        }
        /// <summary>
        /// формирование имени свидетельства о поверке
        /// </summary>
        /// <param name="CertificateNumber"> Номер свидетельства о поверке </param>
        /// <returns></returns>
        public static string GetVerificationCertificateName(string CertificateNumber)
        {
            //return TempFolder + "\\" + "Свидетельство о поверке № " + CertificateNumber.Replace("/", "-") + ".docx";
            return TempFolder + "Свидетельство о поверке № " + CertificateNumber.Replace("/", "-") + ".docx";
        }
        /// <summary>
        /// Формирование имени протокола калибровки
        /// </summary>
        /// <param name="ProtocolNumber"> Номер протокола калибровки. </param>
        /// <returns></returns>
        public static string GetCalibrationProtocolName(string ProtocolNumber)
        {
            //return TempFolder + "\\" + "Протокол калибровки № " + ProtocolNumber + ".docm"; 
            return TempFolder + "Протокол калибровки № " + ProtocolNumber + ".docm";
        }
        /// <summary>
        /// Формирование имени протокола поверки
        /// </summary>
        /// <param name="ProtocolNumber"> Номер протокола поверки. </param>
        /// <returns></returns>
        public static string GetVerificationProtocolName(string ProtocolNumber)
        {
            //return TempFolder + "\\" + "Протокол поверки № " + ProtocolNumber + ".docm";
            return TempFolder + "Протокол поверки № " + ProtocolNumber + ".docm";
        }
        /// <summary>
        /// Формирование имени извещения о непригодности
        /// </summary>
        /// <param name="NoticeNumber"> Номер извещения о непригодности. </param>
        /// <returns></returns>
        public static string GetNoticeOfUnfitnessName(string NoticeNumber)
        {
            //return TempFolder + "\\" + "Извещение о непригодности № " + NoticeNumber + ".docх";
            return TempFolder + "Извещение о непригодности № " + NoticeNumber + ".docх";
        }
        /// <summary>
        /// Создание карточки файла для документа
        /// </summary>
        /// <param name="Context"> Объектный контекст. </param>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="CategoryID"> Идентификатор категории. </param>
        /// <param name="FileFullName"> Полное имя файла. </param>
        /// <param name="CalibrationDate"> Дата проведения калибровки. </param>
        /// <param name="DeviceCard"> Карточка паспорта прибора. </param>
        /// <param name="ClientName"> Наименование клиента. </param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки. </param>
        /// <param name="IsCalibration"> Калибровка. </param>
        /// <param name="NewDevice"> Новый прибор. </param>
        /// <returns></returns>
        public static CardData NewFileCard(ObjectContext Context, UserSession userSession, string CategoryID, string FileFullName, DateTime CalibrationDate, CardData DeviceCard, string ClientName, 
            string VerifySerialNumber, bool IsCalibration, bool NewDevice = false)
        {
            CardData fileData = userSession.CardManager.CreateCardData(CardFile.ID);
            string FileNumber = "";
            string NewFileName = "";
            switch (CategoryID)
            {
                case CalibrationLib.CalibrationCertificateCategoryID:
                    FileNumber = NewDevice ? Context.GetNewCalibrationCertificateNumber(CalibrationDate) : Context.GetRepairCalibrationCertificateNumber();
                    NewFileName = CalibrationLib.GetCalibrationCertificateName(FileNumber);
                    break;
                case CalibrationLib.CalibrationProtocolCategoryID:
                    FileNumber = Context.GetCalibrationProtocolNumber();
                    NewFileName = CalibrationLib.GetCalibrationProtocolName(FileNumber);
                    break;
                case CalibrationLib.VerificationProtocolCategoryID:
                    FileNumber = Context.GetVerificationProtocolNumber();
                    NewFileName = CalibrationLib.GetVerificationProtocolName(FileNumber);
                    break;
                case CalibrationLib.VerificationCertificateCategoryID:
                    FileNumber = Context.GetVerificationCertificateNumber();
                    NewFileName = CalibrationLib.GetVerificationCertificateName(FileNumber);
                    break;
                case CalibrationLib.NoticeOfUnfitnessCategoryID:
                    FileNumber = Context.GetNoticeOfUnfitnessNumber();
                    NewFileName = CalibrationLib.GetNoticeOfUnfitnessName(FileNumber);
                    break;
            }

            CalibrationLib.SetDocumentNumber(FileFullName, FileNumber);

            File.Move(FileFullName, NewFileName);

            VersionedFileCard fileCard = (VersionedFileCard)userSession.CardManager.CreateCard(CardLib.VersionedFileCard.ID);
            fileCard.Initialize(NewFileName, Guid.Empty, false, true);

            // Выгрузка файла на файловый сервер
            ExtensionMethod method = userSession.ExtensionManager.GetExtensionMethod("UploadExtension", "ArchivingFile");
            method.Parameters.AddNew("FileCard", 0).Value = fileCard.Id.ToString();
            method.Parameters.AddNew("ArchivePath", 0).Value = Path.Combine(MyHelper.GetArchiveDeletePath(userSession), "Протоколы калибровки");
            method.Execute();
            // Удаление файла
            File.Delete(NewFileName);

            RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].Rows.AddNew();
            SectionData PropertiesSection = fileData.Sections[CardFile.Properties.ID];
            SectionData CategoriesSection = fileData.Sections[CardFile.Categories.ID];

            MainInfoRow.BeginUpdate();
            MainInfoRow.SetGuid(CardFile.MainInfo.FileID, fileCard.Id);
            MainInfoRow.SetString(CardFile.MainInfo.FileName, fileCard.CurrentVersion.Name);
            MainInfoRow.SetGuid(CardFile.MainInfo.Author, fileCard.CurrentVersion.AuthorId);
            MainInfoRow.SetInt32(CardFile.MainInfo.FileSize, fileCard.CurrentVersion.Size);
            MainInfoRow.SetInt32(CardFile.MainInfo.VersioningType, 0);
            MainInfoRow.EndUpdate();

            fileData.Description = "Файл: " + fileCard.Name;

            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = GetDeviceNumber(DeviceCard);

            // Номер документа
            RowData NewProperty = PropertiesSection.Rows.AddNew();
            NewProperty.BeginUpdate();
            NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.DocumentNumber); //"Номер");
            NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
            NewProperty.SetString(CardFile.Properties.Value, FileNumber);
            NewProperty.SetString(CardFile.Properties.DisplayValue, FileNumber);
            NewProperty.EndUpdate();

            // Дата калибровки
            NewProperty = PropertiesSection.Rows.AddNew();
            NewProperty.BeginUpdate();
            NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.StartDate); //"Дата начала испытаний");
            NewProperty.SetInt32(CardFile.Properties.ParamType, 17);
            NewProperty.SetDateTime(CardFile.Properties.Value, CalibrationDate);
            NewProperty.SetString(CardFile.Properties.DisplayValue, CalibrationDate.ToShortDateString());
            NewProperty.EndUpdate();

            // Тип прибора
            NewProperty = PropertiesSection.Rows.AddNew();
            NewProperty.BeginUpdate();
            NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.DeviceType); //"Тип прибора");
            NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
            NewProperty.SetString(CardFile.Properties.Value, DeviceTypeName);
            NewProperty.SetString(CardFile.Properties.DisplayValue, DeviceTypeName);
            NewProperty.EndUpdate();

            // Заводской номер прибора
            NewProperty = PropertiesSection.Rows.AddNew();
            NewProperty.BeginUpdate();
            NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.DeviceNumber);//"Заводской номер прибора");
            NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
            NewProperty.SetString(CardFile.Properties.Value, DeviceNumber);
            NewProperty.SetString(CardFile.Properties.DisplayValue, DeviceNumber);
            NewProperty.EndUpdate();

            // Клиент
            NewProperty = PropertiesSection.Rows.AddNew();
            NewProperty.BeginUpdate();
            NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.Client); //"Клиент");
            NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
            NewProperty.SetString(CardFile.Properties.Value, ClientName);
            NewProperty.SetString(CardFile.Properties.DisplayValue, ClientName);
            NewProperty.EndUpdate();

            // Серийный номер поверки
            if (VerifySerialNumber != "")
            {
                NewProperty = PropertiesSection.Rows.AddNew();
                NewProperty.BeginUpdate();
                NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.VerifySerialNumber); //"Серийный номер поверки");
                NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
                NewProperty.SetString(CardFile.Properties.Value, VerifySerialNumber);
                NewProperty.SetString(CardFile.Properties.DisplayValue, VerifySerialNumber);
                NewProperty.EndUpdate();
            }

            RowData NewCategory = CategoriesSection.Rows.AddNew();
            NewCategory.SetGuid(CardFile.Categories.CategoryID, new Guid(CategoryID));

            fileData.AssignRights();

            return fileData;
        }
        /// <summary>
        /// Обновление данных карточки файла для документа
        /// </summary>
        /// <param name="Context"> Пользовательская сессия. </param>
        /// <param name="CardScript"> Скрипт. </param>
        /// <param name="fileData"> Карточка файла. </param>
        /// <param name="FileFullName"> Полное имя файла. </param>
        /// <param name="CalibrationDate"> Дата проведения калибровки. </param>
        /// <param name="DeviceCard"> Карточка паспорта прибора. </param>
        /// <param name="ClientName"> Наименование клиента. </param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки. </param>
        /// <param name="IsCalibration"> Калибровка. </param>
        /// <param name="NewCategoryID"> Новая категория (при смене категории документа, например, со Свидетельства о поверке на Извещение о непригодности). </param>
        public static void RefreshFileCard(ObjectContext Context, ScriptClassBase CardScript, CardData fileData, string FileFullName, DateTime CalibrationDate, CardData DeviceCard, string ClientName, 
            string VerifySerialNumber, bool IsCalibration, string NewCategoryID = "")
        {
            RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
            SectionData PropertiesSection = fileData.Sections[CardFile.Properties.ID];
            SectionData CategoriesSection = fileData.Sections[CardFile.Categories.ID];

            string FileNumber = PropertiesSection.FindRow("@Name = '" + DocumentProperties.DocumentNumber + "'").GetString(CardFile.Properties.Value);
            string CategoryID = CategoriesSection.FirstRow.GetString(CardFile.Categories.CategoryID).ToString();

            if (NewCategoryID != "")
            {
                if (!NewCategoryID.ToGuid().Equals(CategoryID.ToGuid()))
                {
                    CategoryID = NewCategoryID;
                    CategoriesSection.FirstRow.SetString(CardFile.Categories.CategoryID, new Guid(NewCategoryID));
                    if (NewCategoryID == CalibrationLib.NoticeOfUnfitnessCategoryID)
                    {
                        //Guid OldFileNumberId = MyHelper.GetNumberIdByRuleName(Context, VerificationCertificateRuleNumbering, Convert.ToInt32(FileNumber));
                        //MyHelper.ReleaseNumber(CardScript, OldFileNumberId);
                        FileNumber = Context.GetNoticeOfUnfitnessNumber();
                    }
                    if (NewCategoryID == CalibrationLib.VerificationCertificateCategoryID)
                    {
                        //Guid OldFileNumberId = MyHelper.GetNumberIdByRuleName(Context, NoticeOfUnfitnessRuleNumbering, Convert.ToInt32(FileNumber));
                        //MyHelper.ReleaseNumber(CardScript, OldFileNumberId);
                        FileNumber = Context.GetVerificationCertificateNumber();
                    }
                }
            }

            string NewFileName = "";
            switch (CategoryID)
            {
                case CalibrationLib.CalibrationCertificateCategoryID:
                    NewFileName = CalibrationLib.GetCalibrationCertificateName(FileNumber);
                    break;
                case CalibrationLib.CalibrationProtocolCategoryID:
                    NewFileName = CalibrationLib.GetCalibrationProtocolName(FileNumber);
                    break;
                case CalibrationLib.VerificationProtocolCategoryID:
                    NewFileName = CalibrationLib.GetVerificationProtocolName(FileNumber);
                    break;
                case CalibrationLib.VerificationCertificateCategoryID:
                    NewFileName = CalibrationLib.GetVerificationCertificateName(FileNumber);
                    break;
                case CalibrationLib.NoticeOfUnfitnessCategoryID:
                    NewFileName = CalibrationLib.GetNoticeOfUnfitnessName(FileNumber);
                    break;
            }
            CalibrationLib.SetDocumentNumber(FileFullName, FileNumber);

            File.Move(FileFullName, NewFileName);

            VersionedFileCard fileCard = (VersionedFileCard)CardScript.Session.CardManager.GetCard(new Guid(MainInfoRow.GetString(CardFile.MainInfo.FileID)));
            fileCard.CheckIn(NewFileName, 0, false, true);

            File.Delete(NewFileName);

            fileCard.Name = fileCard.CurrentVersion.Name;
            MainInfoRow.BeginUpdate();
            MainInfoRow.SetGuid(CardFile.MainInfo.FileID, fileCard.Id);
            MainInfoRow.SetString(CardFile.MainInfo.FileName, fileCard.CurrentVersion.Name);
            MainInfoRow.SetGuid(CardFile.MainInfo.Author, fileCard.CurrentVersion.AuthorId);
            MainInfoRow.SetInt32(CardFile.MainInfo.FileSize, fileCard.CurrentVersion.Size);
            MainInfoRow.SetInt32(CardFile.MainInfo.VersioningType, 0);
            MainInfoRow.EndUpdate();

            fileData.Description = "Файл: " + fileCard.Name;

            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = GetDeviceNumber(DeviceCard);

            // Номер документа
            RowData Property = PropertiesSection.FindRow("@Name = '" + DocumentProperties.DocumentNumber + "'");  //Номер документа'");
            Property.BeginUpdate();
            Property.SetString(CardFile.Properties.Value, FileNumber);
            Property.SetString(CardFile.Properties.DisplayValue, FileNumber);
            Property.EndUpdate();

            // Дата калибровки
            Property = PropertiesSection.FindRow("@Name = '"+DocumentProperties.StartDate+"'");  //Дата начала испытаний'");
            Property.BeginUpdate();
            Property.SetDateTime(CardFile.Properties.Value, CalibrationDate);
            Property.SetString(CardFile.Properties.DisplayValue, CalibrationDate.ToShortDateString());
            Property.EndUpdate();

            // Тип прибора
            Property = PropertiesSection.FindRow("@Name = '"+DocumentProperties.DeviceType+"'"); //'Тип прибора'");
            Property.BeginUpdate();
            Property.SetString(CardFile.Properties.Value, DeviceTypeName);
            Property.SetString(CardFile.Properties.DisplayValue, DeviceTypeName);
            Property.EndUpdate();

            // Заводской номер прибора
            Property = PropertiesSection.FindRow("@Name = '"+DocumentProperties.DeviceNumber+"'"); //'Заводской номер прибора'");
            Property.BeginUpdate();
            Property.SetString(CardFile.Properties.Value, DeviceNumber);
            Property.SetString(CardFile.Properties.DisplayValue, DeviceNumber);
            Property.EndUpdate();

            // Клиент
            Property = PropertiesSection.FindRow("@Name = '" + DocumentProperties.Client + "'"); //'Клиент'");
            Property.BeginUpdate();
            Property.SetString(CardFile.Properties.Value, ClientName);
            Property.SetString(CardFile.Properties.DisplayValue, ClientName);
            Property.EndUpdate();

            // Серийный номер поверки
            Property = PropertiesSection.FindRow("@Name = '" + DocumentProperties.VerifySerialNumber + "'"); //'Серийный номер поверки'");
            if (Property != null)
            {
                Property.BeginUpdate();
                Property.SetString(CardFile.Properties.Value, VerifySerialNumber);
                Property.SetString(CardFile.Properties.DisplayValue, VerifySerialNumber);
                Property.EndUpdate();
            }
            else
            {
                if (VerifySerialNumber != "")
                {
                    RowData NewProperty = PropertiesSection.Rows.AddNew();
                    NewProperty.BeginUpdate();
                    NewProperty.SetString(CardFile.Properties.Name, DocumentProperties.VerifySerialNumber); //"Серийный номер поверки");
                    NewProperty.SetInt32(CardFile.Properties.ParamType, 0);
                    NewProperty.SetString(CardFile.Properties.Value, VerifySerialNumber);
                    NewProperty.SetString(CardFile.Properties.DisplayValue, VerifySerialNumber);
                    NewProperty.EndUpdate();
                }
            }

            // Выгрузка файла на файловый сервер
            ExtensionMethod method = CardScript.Session.ExtensionManager.GetExtensionMethod("UploadExtension", "ArchivingFile");
            method.Parameters.AddNew("FileCard", 0).Value = fileCard.Id.ToString();
            method.Parameters.AddNew("ArchivePath", 0).Value = Path.Combine(MyHelper.GetArchiveDeletePath(CardScript.Session), "Протоколы калибровки");
            method.Execute();

            return;
        }
        /// <summary>
        /// Обновление данных карточки файла для документа
        /// </summary>
        /// <param name="fileData"> Карточка файла. </param>
        /// <param name="DocumentNumber"> Номер документа. </param>
        /// <param name="CategoryID"> Идентификатор категории документа. </param>
        /// <param name="StartDate"> Дата документа. </param>
        public static void GetDocumentProperties(CardData fileData, out string DocumentNumber, out string CategoryID, out string StartDate)
        {
            RowData MainInfoRow = fileData.Sections[CardFile.MainInfo.ID].FirstRow;
            SectionData PropertiesSection = fileData.Sections[CardFile.Properties.ID];
            SectionData CategoriesSection = fileData.Sections[CardFile.Categories.ID];

            DocumentNumber = PropertiesSection.FindRow("@Name = '" + DocumentProperties.DocumentNumber + "'").GetString(CardFile.Properties.Value);
            CategoryID = CategoriesSection.FirstRow.GetString(CardFile.Categories.CategoryID).ToString();
            StartDate = ((DateTime)PropertiesSection.FindRow("@Name = '" + DocumentProperties.StartDate + "'").GetDateTime(CardFile.Properties.Value)).ToShortDateString();
        }
        /// <summary>
        /// Прикрепление файла к карточке "Паспорт прибора"
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия. </param>
        /// <param name="Card"> Карточка паспорта прибора. </param>
        /// <param name="FileCard"> Карточка файла. </param>
        public static void AttachFileToCard(UserSession userSession, CardData Card, CardData FileCard)
        {
            RowData MainInfoRow = Card.Sections[CardOrd.MainInfo.ID].FirstRow;
            CardData fileList = MainInfoRow.GetGuid(CardOrd.MainInfo.FilesID) == null ? userSession.CardManager.CreateCardData(FileList.ID) :
                userSession.CardManager.GetCardData((Guid)MainInfoRow.GetGuid(CardOrd.MainInfo.FilesID));

            RowData NewReference = fileList.Sections[FileList.FileReferences.ID].Rows.AddNew();
            NewReference.SetGuid(FileList.FileReferences.CardFileID, FileCard.Id);
        }
        /// <summary>
        /// Проверка свойств документа
        /// </summary>
        /// <param name="Document"> Документ. </param>
        /// <returns></returns>
        public static bool TestDocumentsFilds(this WordprocessingDocument Document)
        {
            List<Table> TableCollection = Document.GetAllTables();
            int tableNumber = 0;
            int RowNumber = 0;
            int CellNumber = 0;
            foreach (Table table in TableCollection)
            {
                tableNumber++;
                IEnumerable<TableRow> TableRowCollection = table.Elements<TableRow>();
                RowNumber = 0;
                foreach (TableRow tableRow in TableRowCollection)
                {
                    RowNumber++;
                    CellNumber = 0;
                    foreach (OpenXmlElement Element in tableRow.Elements())
                    {
                        CellNumber++;
                        if (Element is DocumentFormat.OpenXml.Wordprocessing.SdtCell)
                        {
                            Paragraph paragraph = Element.GetFirstChild<SdtContentCell>().GetFirstChild<TableCell>().GetFirstChild<Paragraph>();
                            string Text = paragraph.Elements<Run>().Select(r => r.GetFirstChild<Text>() != null ? r.GetFirstChild<Text>().Text : "").Aggregate((currentValue, nextValue) => currentValue + nextValue);
                            if ((Text == "Ввод данных") || (Text == ""))
                            {                               
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Получение заключения, указанного в протоколе поверки.
        /// </summary>
        /// <param name="Document"> Документ. </param>
        /// <returns></returns>
        public static bool GetDocumentsResult(this WordprocessingDocument Document)
        {
            Table ResultTable = GetTable(Document, "Заключение:");
            TableRow TableRow = ResultTable.Elements<TableRow>().First();
            int CellNumber = 0;
            foreach (OpenXmlElement Element in TableRow.Elements())
            {
                CellNumber++;
                if (Element is DocumentFormat.OpenXml.Wordprocessing.SdtCell)
                {
                    Paragraph paragraph = Element.GetFirstChild<SdtContentCell>().GetFirstChild<TableCell>().GetFirstChild<Paragraph>();
                    string Text = paragraph.Elements<Run>().Select(r => r.GetFirstChild<Text>() != null ? r.GetFirstChild<Text>().Text : "").Aggregate((currentValue, nextValue) => currentValue + nextValue);
                            if ((Text == "годен") || (Text == "СИ соответствует метрологическим требованиям") || (Text == "Прибор МИКО-10 годен к применению") || (Text == "Прибор МИКО-21 годен к применению"))
                                return true;
                            if ((Text == "не годен") || (Text == "СИ не соответствует метрологическим требованиям") || (Text == "Прибор МИКО-10 не годен к применению") || (Text == "Прибор МИКО-21 не годен к применению"))
                                return false;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Получить значение ячейки таблицы
        /// </summary>
        /// <param name="CurrentTable"> Таблица. </param>
        /// <param name="RowIndex"> Индекс строки. </param>
        /// <param name="ColumnIndex"> Индекс столбца. </param>
        /// <returns></returns>
        public static string GetCellValue(this Table CurrentTable, int RowIndex, int ColumnIndex)
        {
            TableRow CurrentTableRow = CurrentTable.Elements<TableRow>().ElementAt(RowIndex);
            ArrayList CurrentCells = new ArrayList();
            foreach (OpenXmlElement Element in CurrentTableRow.Elements())
            {
                if (Element is DocumentFormat.OpenXml.Wordprocessing.TableCell) CurrentCells.Add(Element);

                if (Element is DocumentFormat.OpenXml.Wordprocessing.SdtCell)
                {
                    CurrentCells.Add(Element.GetFirstChild<SdtContentCell>().GetFirstChild<TableCell>());
                }
            }
            TableCell CurrentTableCell = (TableCell)CurrentCells[ColumnIndex];
            Paragraph CurrentParagraph = CurrentTableCell.GetFirstChild<Paragraph>();
            IEnumerable<Run> Runs = CurrentParagraph.Elements<Run>();
            string Text = "";
            if (Runs.Count() > 0)
            {
                Text = CurrentParagraph.Elements<Run>().Select(r => r.GetFirstChild<Text>() != null ? r.GetFirstChild<Text>().Text : "").Aggregate((currentValue, nextValue) => currentValue + nextValue);
            }
            return Text;
        }
        /// <summary>
        /// Задать значение ячейки таблицы
        /// </summary>
        /// <param name="CurrentTable"> Таблица. </param>
        /// <param name="RowIndex"> Индекс строки. </param>
        /// <param name="ColumnIndex"> Индекс столбца. </param>
        /// <param name="Value"> Новое значение. </param>
        public static void SetCellValue(this Table CurrentTable, int RowIndex, int ColumnIndex, string Value)
        {
            TableRow CurrentTableRow = CurrentTable.Elements<TableRow>().ElementAt(RowIndex);
            ArrayList CurrentCells = new ArrayList();
            foreach (OpenXmlElement Element in CurrentTableRow.Elements())
            {
                if (Element is DocumentFormat.OpenXml.Wordprocessing.TableCell) CurrentCells.Add(Element);

                if (Element is DocumentFormat.OpenXml.Wordprocessing.SdtCell)
                {
                    CurrentCells.Add(Element.GetFirstChild<SdtContentCell>().GetFirstChild<TableCell>());
                }
            }
            TableCell CurrentTableCell = (TableCell)CurrentCells[ColumnIndex];
            Paragraph CurrentParagraph = CurrentTableCell.GetFirstChild<Paragraph>();
            IEnumerable<Run> Runs = CurrentParagraph.Elements<Run>();

            Run run;

            if (Runs.Count() > 0)
            {
                run = Runs.First();
                run.RemoveAllChildren<Text>();

                for (int i = 1; i < Runs.Count(); i++)
                {
                    Runs.ElementAt(i).Remove();
                }
            }
            else
            {
                run = NewRun(12);
                CurrentParagraph.Append(run);
            }
            if (run.RunProperties.FontSize == null) { run.RunProperties.SetFontSize(12); }
            run.RunProperties.SetRunFonts("Times New Roman");
            run.Append(NewText(Value));
            return;
        }
        /// <summary>
        /// Изменение ширины ячейки.
        /// </summary>
        /// <param name="CurrentTable"> Таблица. </param>
        /// <param name="RowIndex"> Индекс строки. </param>
        /// <param name="ColumnIndex"> Индекс столбца. </param>
        /// <param name="SignCount"> Количество символов в ячейке. </param>
        public static void ChangeCellsWidth(this Table CurrentTable, int RowIndex, int ColumnIndex, int SignCount)
        {
            TableRow CurrentTableRow = CurrentTable.Elements<TableRow>().ElementAt(RowIndex);
            ArrayList CurrentCells = new ArrayList();
            foreach (OpenXmlElement Element in CurrentTableRow.Elements())
            {
                if (Element is DocumentFormat.OpenXml.Wordprocessing.TableCell) CurrentCells.Add(Element);

                if (Element is DocumentFormat.OpenXml.Wordprocessing.SdtCell)
                {
                    CurrentCells.Add(Element.GetFirstChild<SdtContentCell>().GetFirstChild<TableCell>());
                }
            }

            TableGrid Grid = CurrentTable.Elements<TableGrid>().First();
            IEnumerable<int> GridColumnsWidth = Grid.Elements<GridColumn>().Select(r => Convert.ToInt32(r.Width.Value));
            int MinWidth = SignCount * 123;

            int CurentWidth = 0;
            int i = 0;
            while (CurentWidth < MinWidth)
            {
                i++;
                CurentWidth = CurentWidth + GridColumnsWidth.ElementAt(i);
            }

            TableCell CurrentTableCell = (TableCell)CurrentCells[ColumnIndex];
            CurrentTableCell.TableCellProperties.GridSpan = new GridSpan() { Val = i };
            TableCell NextTableCell = (TableCell)CurrentCells[ColumnIndex + 1];
            NextTableCell.TableCellProperties.GridSpan = new GridSpan() { Val = GridColumnsWidth.Count() - i };

            return;
        }
        /// <summary>
        /// Получение таблицы.
        /// </summary>
        /// <param name="ParentDocument"> Документ, содержащий таблицу. </param>
        /// <param name="TableName"> Название таблицы (содержится в ячейке таблицы R0C0)</param>
        /// <returns></returns>
        public static Table GetTable(this WordprocessingDocument ParentDocument, string TableName)
        {
            List<Table> TableCollection;
            
            TableCollection = ParentDocument.MainDocumentPart.Document.Body.Elements<Table>().ToList();
            if (TableCollection.Count == 0)
            {
                IEnumerable<SdtBlock> BlockCollection = ParentDocument.MainDocumentPart.Document.Body.Elements<SdtBlock>();
                foreach (SdtBlock Block in BlockCollection)
                {
                    foreach (SdtContentBlock ContentBlock in Block.Elements<SdtContentBlock>().ToList())
                    {
                        foreach (Table table in ContentBlock.Elements<Table>().ToList())
                        { TableCollection.Add(table); }
                    }
                }
            }
            string TitleText;
            Table FindTable = null;
            int i = 0;

            foreach (Table CurrentTable in TableCollection)
            {
                i++;
                Paragraph CurrentParagraph = CurrentTable.GetFirstChild<TableRow>().GetFirstChild<TableCell>().GetFirstChild<Paragraph>();
                if (CurrentParagraph.Elements<Run>().Count() == 0)
                { TitleText = ""; }
                else
                { TitleText = CurrentParagraph.Elements<Run>().Select(r => r.GetFirstChild<Text>() != null ? r.GetFirstChild<Text>().Text : "").Aggregate((currentValue, nextValue) => currentValue + nextValue); }
                if (TitleText == TableName)
                { FindTable = CurrentTable; }
            }

            if (FindTable.IsNull())
            {
                foreach (Table CurrentTable in TableCollection)
                {
                    Paragraph LastParagraphBefore = CurrentTable.ElementsBefore().Last() as Paragraph;
                    if (LastParagraphBefore != null)
                    {
                        if (LastParagraphBefore.Elements<Run>().Count() == 0)
                        { TitleText = ""; }
                        else
                        { TitleText = LastParagraphBefore.Elements<Run>().Select(r => r.GetFirstChild<Text>() != null ? r.GetFirstChild<Text>().Text : "").Aggregate((currentValue, nextValue) => currentValue + nextValue); }
                        if (TitleText == TableName)
                        { FindTable = CurrentTable; }
                    }

                }
            }

            return FindTable;
        }
        /// <summary>
        /// Получение таблицы.
        /// </summary>
        /// <param name="ParentDocument"> Документ, содержащий таблицу. </param>
        /// <param name="TableIndex"> Индекс таблицы. </param>
        /// <returns></returns>
        public static Table GetTable(this WordprocessingDocument ParentDocument, int TableIndex)
        {
            Table FindTable = ParentDocument.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(TableIndex);
            
            return FindTable;
        }
        /// <summary>
        /// Возвращает коллекцию таблиц документа
        /// </summary>
        /// <param name="ParentDocument"> Документ. </param>
        /// <returns></returns>
        public static List<Table> GetAllTables(this WordprocessingDocument ParentDocument)
        {
            List<Table> TableCollection;

            TableCollection = ParentDocument.MainDocumentPart.Document.Body.Elements<Table>().ToList();
            if (TableCollection.Count == 0)
            {
                IEnumerable<SdtBlock> BlockCollection = ParentDocument.MainDocumentPart.Document.Body.Elements<SdtBlock>();
                foreach (SdtBlock Block in BlockCollection)
                {
                    foreach (SdtContentBlock ContentBlock in Block.Elements<SdtContentBlock>().ToList())
                    {
                        foreach (Table table in ContentBlock.Elements<Table>().ToList())
                        { TableCollection.Add(table); }
                    }
                }
            }

            return TableCollection;
        }
        /// <summary>
        /// Добавить новую строку в таблицу
        /// </summary>
        /// <param name="RowHeight"> Высота строки. </param>
        /// <returns></returns>
        public static TableRow NewTableRow(UInt32 RowHeight = 284U)
        {
            TableRow tableRow = new TableRow() { RsidTableRowMarkRevision = "00811F72", RsidTableRowAddition = "008776EE", RsidTableRowProperties = "00D46875" };

            TablePropertyExceptions tablePropertyExceptions = new TablePropertyExceptions();
            TableLook tableLook = new TableLook() { Val = "04A0" };
            tablePropertyExceptions.Append(tableLook);

            TableRowProperties tableRowProperties = new TableRowProperties();
            TableRowHeight tableRowHeight = new TableRowHeight() { Val = (UInt32Value)RowHeight };
            tableRowProperties.Append(tableRowHeight);

            tableRow.Append(tablePropertyExceptions);
            tableRow.Append(tableRowProperties);

            return tableRow;
        }
        /// <summary>
        /// Создать новую таблицу.
        /// </summary>
        /// <param name="ColumnsWidth"> Ширина столбца. </param>
        /// <param name="tableIndentationWidth"> Отступ. </param>
        /// <returns></returns>
        public static Table NewTable(int[] ColumnsWidth, int tableIndentationWidth = -176)
        {
            Table table = new Table();
            TableProperties tableProperties = new TableProperties();
            TableStyle tableStyle = new TableStyle() { Val = "a3" };
            TableWidth tableWidth = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };

            TableIndentation tableIndentation = new TableIndentation() { Width = tableIndentationWidth, Type = TableWidthUnitValues.Dxa };
            TableLayout tableLayout = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook = new TableLook() { Val = "04A0" };

            tableProperties.Append(tableStyle);
            tableProperties.Append(tableWidth);
            tableProperties.Append(tableIndentation);
            tableProperties.Append(tableLayout);
            tableProperties.Append(tableLook);

            TableGrid tableGrid = new TableGrid();
            foreach (int ColumnWidth in ColumnsWidth)
            {
                GridColumn gridColumn = new GridColumn() { Width = ColumnWidth.ToString() };
                tableGrid.Append(gridColumn);
            }

            table.Append(tableProperties);
            table.Append(tableGrid);

            return table;
        }
        /// <summary>
        /// Добавить ячейку в таблицу.
        /// </summary>
        /// <param name="CurrentRow"> Строка таблицы. </param>
        /// <param name="CellWidth"> Ширина ячейки. </param>
        /// <param name="Text"> Текст ячейки. </param>
        /// <param name="LeftIndentation"> Левый отступ. </param>
        /// <param name="RightIndentation"> Правый отступ. </param>
        /// <param name="JustificationValue"> Выравнивание. </param>
        /// <param name="FontSize"> Размер шрифта. </param>
        /// <param name="GridSpan"> Объединение ячеек. </param>
        /// <param name="Bold"> Полужирный шрифт. </param>
        /// <param name="Italic"> Курсив. </param>
        /// <param name="FontName"> Название шрифта. </param>
        /// <returns></returns>
        public static TableCell AddTableCell(this TableRow CurrentRow, int CellWidth, string Text, int LeftIndentation, int RightIndentation, JustificationValues JustificationValue,
            int FontSize, int GridSpan = 1, bool Bold = false, bool Italic = false, int VerticalMerge = 0, string FontName = "Times New Roman")
        {
            TableCell tableCell = new TableCell();
            TableCellProperties tableCellProperties = new TableCellProperties();
            TableCellWidth tableCellWidth = new TableCellWidth() { Width = CellWidth.ToString(), Type = TableWidthUnitValues.Dxa };

            tableCellProperties.Append(tableCellWidth);
            tableCell.Append(tableCellProperties);
            Paragraph paragraph = NewParagraph(LeftIndentation, RightIndentation, JustificationValue, FontSize, Bold, Italic, FontName);
            if (Text.Contains("SuperScript"))
            {
                string Text1 = Text.Substring(0, Text.IndexOf("SuperScript"));
                string Text2 = Text.Substring(Text.IndexOf("]") + 1, Text.Length - Text.IndexOf("]") - 1);
                string SuperScriptText = Text.Substring(Text.IndexOf("[") + 1, Text.IndexOf("]") - Text.IndexOf("[") - 1);
                Run run1 = NewRun(FontSize, Bold, Italic, FontName);
                run1.Append(NewText(Text1));
                paragraph.Append(run1);

                Run run2 = NewRun(FontSize, Bold, Italic, FontName, true);
                run2.Append(NewText(SuperScriptText));
                paragraph.Append(run2);

                Run run3 = NewRun(FontSize, Bold, Italic, FontName);
                run3.Append(NewText(Text2));
                paragraph.Append(run3);
            }
            else
            {
                Run run = NewRun(FontSize, Bold, Italic, FontName);
                run.Append(NewText(Text));
                paragraph.Append(run);
            }
            tableCell.Append(paragraph);
            tableCell.SetGridSpan(GridSpan);
            tableCell.SetVerticalMerge(VerticalMerge);
            tableCell.SetVerticalAlignment(TableVerticalAlignmentValues.Center);
            CurrentRow.Append(tableCell);
            

            return tableCell;
        }
        /// <summary>
        /// Добавить новый параграф.
        /// </summary>
        /// <param name="LeftIndentation"> Левый отступ. </param>
        /// <param name="RightIndentation"> Правый отступ. </param>
        /// <param name="JustificationValue"> Выравнивание. </param>
        /// <param name="FontSize"> Размер шрифта. </param>
        /// <param name="Bold"> Полужирный шрифт. </param>
        /// <param name="Italic"> Курсив. </param>
        /// <param name="FontName"> Название шрифта. </param>
        /// <returns></returns>
        public static Paragraph NewParagraph(int LeftIndentation, int RightIndentation, JustificationValues JustificationValue, int FontSize,
            bool Bold = false, bool Italic = false, string FontName = "Times New Roman")
        {
            Paragraph paragraph = new Paragraph() { RsidParagraphMarkRevision = "00466F5C", RsidParagraphAddition = "00466F5C", RsidParagraphProperties = "00466F5C", RsidRunAdditionDefault = "00466F5C" };
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            Tabs tabs = new Tabs();
            // TabStop tabStop = new TabStop() { Val = TabStopValues.Left, Position = 1832 };
            TabStop tabStop = new TabStop() { Val = TabStopValues.Clear, Position = 851 };
            tabs.Append(tabStop);
            paragraphProperties.Append(tabs);
            AutoSpaceDE autoSpaceDE1 = new AutoSpaceDE();
            AutoSpaceDN autoSpaceDN1 = new AutoSpaceDN();
            Indentation indentation1 = new Indentation() { FirstLine = "0" };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };
            paragraphProperties.Append(autoSpaceDE1);
            paragraphProperties.Append(autoSpaceDN1);
            paragraphProperties.Append(indentation1);
            paragraphProperties.Append(justification1);

            paragraphProperties.SetIndentation(LeftIndentation, RightIndentation);  // Отступ
            paragraphProperties.SetJustification(JustificationValue);               // Выравнивание
            paragraphProperties.SetSpacingBetweenLines(0);                          // Межстрочный интервал

            ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();
            paragraphMarkRunProperties.SetRunFonts(FontName);                       // Шрифт
            paragraphMarkRunProperties.SetBold(Bold);                               // Жирный
            paragraphMarkRunProperties.SetItalic(Italic);                           // Курсив
            paragraphMarkRunProperties.SetFontSize(FontSize);                       // Размер шрифта
            paragraphMarkRunProperties.SetLanguages("en-US");                       // Языки

            paragraphProperties.Append(paragraphMarkRunProperties);
            paragraph.Append(paragraphProperties);
            return paragraph;
        }
        /// <summary>
        /// Добавить новый контейнер
        /// </summary>
        /// <param name="FontSize"> Размер шрифта. </param>
        /// <param name="Bold"> Полужирный шрифт. </param>
        /// <param name="Italic"> Курсив. </param>
        /// <param name="FontName"> Название шрифта. </param>
        ///  <param name="SuperScript"> Надстрочный текст. </param>
        /// <returns></returns>
        public static Run NewRun(int FontSize, bool Bold = false, bool Italic = false, string FontName = "Times New Roman", bool SuperScript = false)
        {
            Run run = new Run() { RsidRunProperties = "00466F5C" };

            RunProperties runProperties = new RunProperties();
            runProperties.SetRunFonts(FontName);
            runProperties.SetFontSize(FontSize);
            runProperties.SetBold(Bold);
            runProperties.SetItalic(Italic);
            runProperties.SetLanguages("en-US");
            if (SuperScript) runProperties.SetSuperScript(SuperScript);
            run.Append(runProperties);
            return run;
        }
        /// <summary>
        /// Добавить новый текст.
        /// </summary>
        /// <param name="Value"> Текст. </param>
        /// <returns></returns>
        public static Text NewText(string Value)
        {
            Text text = new Text();
            text.Text = Value;
            return text;
        }
        /// <summary>
        /// Задать отступ
        /// </summary>
        /// <param name="paragraphProperties"> Свойства параграфа. </param>
        /// <param name="LeftIndentation"> Левый отступ. </param>
        /// <param name="RightIndentation"> Правый отступ. </param>
        public static void SetIndentation(this ParagraphProperties paragraphProperties, int LeftIndentation, int RightIndentation)
        {
            Indentation indentation = new Indentation() { Left = LeftIndentation.ToString(), Right = RightIndentation.ToString() };
            paragraphProperties.Append(indentation);
        }
        /// <summary>
        /// Задать горизонтальное выравнивание.
        /// </summary>
        /// <param name="paragraphProperties"> Свойства параграфа. </param>
        /// <param name="JustificationValue"> Вид выравнивания. </param>
        public static void SetJustification(this ParagraphProperties paragraphProperties, JustificationValues JustificationValue)
        {
            Justification justification = new Justification() { Val = JustificationValue };
            paragraphProperties.Append(justification);
        }
        // <summary>
        /// Задать межстрочный интервал.
        /// </summary>
        /// <param name="paragraphProperties"> Свойства параграфа. </param>
        /// <param name="JustificationValue"> Интервал. </param>
        public static void SetSpacingBetweenLines(this ParagraphProperties paragraphProperties, JustificationValues JustificationValue)
        {
            SpacingBetweenLines spacingBetweenLines = new SpacingBetweenLines() { After = "0" }; // Межстрочный интервал
            paragraphProperties.Append(spacingBetweenLines);
        }
        /// <summary>
        /// Задать шрифт.
        /// </summary>
        /// <param name="paragraphMarkRunProperties"> Свойства контейнера текста параграфа. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetRunFonts(this ParagraphMarkRunProperties paragraphMarkRunProperties, string Value)
        {
            RunFonts runFonts = new RunFonts() { Ascii = Value, HighAnsi = Value, ComplexScript = Value };
            paragraphMarkRunProperties.Append(runFonts);
        }
        /// <summary>
        /// Задать шрифт.
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetRunFonts(this RunProperties runProperties, string Value)
        {
            RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            runProperties.Append(runFonts);
        }
        /// <summary>
        /// Установить полужирный шрифт
        /// </summary>
        /// <param name="paragraphMarkRunProperties"> Свойства контейнера текста параграфа. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetBold(this ParagraphMarkRunProperties paragraphMarkRunProperties, bool Value)
        {
            Bold bold = new Bold() { Val = Value };
            paragraphMarkRunProperties.Append(bold);
        }
        /// <summary>
        /// Установить полужирный шрифт
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста </param>
        /// <param name="Value"> Значение. </param>
        public static void SetBold(this RunProperties runProperties, bool Value)
        {
            Bold bold = new Bold() { Val = Value };
            runProperties.Append(bold);
        }
        /// <summary>
        /// Установить надстрочный текст
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста </param>
        /// <param name="Value"> Значение. </param>
        public static void SetSuperScript(this RunProperties runProperties, bool Value)
        {
            VerticalTextAlignment verticalTextAlignment = new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript };
            runProperties.Append(verticalTextAlignment);
        }
        /// <summary>
        /// Установить курсив.
        /// </summary>
        /// <param name="paragraphMarkRunProperties"> Свойства контейнера текста параграфа. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetItalic(this ParagraphMarkRunProperties paragraphMarkRunProperties, bool Value)
        {
            Italic italic = new Italic() { Val = Value };
            paragraphMarkRunProperties.Append(italic);
        }
        /// <summary>
        /// Установить курсив.
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetItalic(this RunProperties runProperties, bool Value)
        {
            Italic italic = new Italic() { Val = Value };
            runProperties.Append(italic);
        }
        /// <summary>
        /// Установить размер шрифта.
        /// </summary>
        /// <param name="paragraphMarkRunProperties"> Свойства контейнера текста параграфа. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetFontSize(this ParagraphMarkRunProperties paragraphMarkRunProperties, int Value)
        {
            FontSize fontSize = new FontSize() { Val = Convert.ToString(Value * 2) };
            FontSizeComplexScript fontSizeComplexScript = new FontSizeComplexScript() { Val = Convert.ToString(Value * 2) };
            paragraphMarkRunProperties.Append(fontSize);
            paragraphMarkRunProperties.Append(fontSizeComplexScript);
        }
        /// <summary>
        /// Установить размер шрифта.
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetFontSize(this RunProperties runProperties, int Value)
        {
            FontSize fontSize = new FontSize() { Val = Convert.ToString(Value * 2) };
            FontSizeComplexScript fontSizeComplexScript = new FontSizeComplexScript() { Val = Convert.ToString(Value * 2) };
            runProperties.Append(fontSize);
            runProperties.Append(fontSizeComplexScript);
        }
        /// <summary>
        /// Установить язык.
        /// </summary>
        /// <param name="paragraphMarkRunProperties"> Свойства контейнера текста параграфа. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetLanguages(this ParagraphMarkRunProperties paragraphMarkRunProperties, string Value)
        {
            Languages languages = new Languages() { Val = Value };
            paragraphMarkRunProperties.Append(languages);
        }
        /// <summary>
        /// Установить язык.
        /// </summary>
        /// <param name="runProperties"> Свойства контейнера текста. </param>
        /// <param name="Value"> Значение. </param>
        public static void SetLanguages(this RunProperties runProperties, string Value)
        {
            Languages languages = new Languages() { Val = Value };
            runProperties.Append(languages);
        }
        // Границы
        /// <summary>
        /// Установить границы.
        /// </summary>
        /// <param name="tableCell"> Ячейка таблицы. </param>
        /// <param name="topBorderValue"> Верхняя граница. </param>
        /// <param name="leftBorderValue"> Левая граница. </param>
        /// <param name="bottomBorderValue"> Нижняя граница. </param>
        /// <param name="rightBorderValue"> Правая граница. </param>
        public static void SetBorders(this TableCell tableCell, BorderValues topBorderValue, BorderValues leftBorderValue, BorderValues bottomBorderValue, BorderValues rightBorderValue)
        {
            TableCellBorders tableCellBorders = new TableCellBorders();
            TopBorder topBorder = new TopBorder() { Val = topBorderValue };
            LeftBorder leftBorder = new LeftBorder() { Val = leftBorderValue };
            BottomBorder bottomBorder = new BottomBorder() { Val = bottomBorderValue };
            RightBorder rightBorder = new RightBorder() { Val = rightBorderValue };

            tableCellBorders.Append(topBorder);
            tableCellBorders.Append(leftBorder);
            tableCellBorders.Append(bottomBorder);
            tableCellBorders.Append(rightBorder);

            tableCell.TableCellProperties.Append(tableCellBorders);
        }
        // Вертикальное выравнивание
        /// <summary>
        /// Установить вертикальное выравнивание.
        /// </summary>
        /// <param name="tableCell"> Ячейка таблицы. </param>
        /// <param name="VerticalAlignmentValue"> Вид выравнивания. </param>
        public static void SetVerticalAlignment(this TableCell tableCell, TableVerticalAlignmentValues VerticalAlignmentValue)
        {
            TableCellVerticalAlignment tableCellVerticalAlignment = new TableCellVerticalAlignment() { Val = VerticalAlignmentValue };
            tableCell.TableCellProperties.Append(tableCellVerticalAlignment);
        }
        // Объединение
        /// <summary>
        /// Объединить ячейки по горизонтали.
        /// </summary>
        /// <param name="tableCell"> Ячейка таблицы. </param>
        /// <param name="GridSpan"> Количество объединяемых ячеек. </param>
        public static void SetGridSpan(this TableCell tableCell, int GridSpan)
        {
            GridSpan gridSpan = new GridSpan() { Val = GridSpan };
            tableCell.TableCellProperties.Append(gridSpan);
        }
        // Объединение
        /// <summary>
        /// Объединить ячейки по вертикали.
        /// </summary>
        /// <param name="tableCell"> Ячейка таблицы. </param>
        /// <param name="GridSpan"> Количество объединяемых ячеек. </param>
        public static void SetVerticalMerge(this TableCell tableCell, int VerticalMerge)
        {
            switch (VerticalMerge)
            {
                case 1:
                    VerticalMerge verticalMerge1 = new VerticalMerge() { Val = MergedCellValues.Restart };
                    tableCell.TableCellProperties.Append(verticalMerge1);
                    break;
                case 2:
                    VerticalMerge verticalMerge2 = new VerticalMerge();
                    tableCell.TableCellProperties.Append(verticalMerge2);
                    break;
                case 0:
                    break;
            }
        }
        /// <summary>
        /// Добавление текста в закладку.
        /// </summary>
        /// <param name="Bookmark"> Закладка. </param>
        /// <param name="Text"> Текст. </param>
        public static void WriteText(this BookmarkStart Bookmark, String Text)
        {
            try
            {
                var CellText = Bookmark.NextSibling<Run>().GetFirstChild<Text>();
                CellText.Text = Text;
            }
            catch { }
        }

        /// <summary>
        /// Определяет, заполнены ли реквизиты протокола калибровки.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия.</param>
        /// <param name="FileCard"> Карточка файла протокола калибровки.</param>
        public static bool TestFileFilds(UserSession userSession, CardData FileCard)
        {
            string DocumentName = FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);
            string TempFolder = System.IO.Path.GetTempPath();
            //string TempPath = TempFolder + "\\" + DocumentName;
            string TempPath = TempFolder + DocumentName;

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);

            bool Result = ProtocolDocument.TestDocumentsFilds();   // В протоколе ПСИ не заполены требуемые реквизиты
            ProtocolDocument.Close();
            return Result;
        }
        /// <summary>
        /// Определяет результат поверки (прибор годен/не годен).
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия.</param>
        /// <param name="FileCard"> Карточка файла протокола калибровки.</param>
        public static bool GetVerifyResult(UserSession userSession, CardData FileCard)
        {
            XtraMessageBox.Show("1.1");
            string DocumentName = FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);
            XtraMessageBox.Show("1.2");
            string TempFolder = System.IO.Path.GetTempPath();
            XtraMessageBox.Show("1.3");
            string TempPath = TempFolder + DocumentName;
            XtraMessageBox.Show("1.4");
            //string TempPath = TempFolder + "\\" + DocumentName;

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            XtraMessageBox.Show("1.5");
            versionedFileCard.CurrentVersion.Download(TempPath);
            XtraMessageBox.Show("1.6");
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);
            XtraMessageBox.Show("1.7");

            bool Result = ProtocolDocument.GetDocumentsResult();  // Определяется результат поверки прибора: годен/не годен
            XtraMessageBox.Show("1.8");
            ProtocolDocument.Close();
            XtraMessageBox.Show("1.9");
            File.Delete(TempPath);
            XtraMessageBox.Show("1.10");
            return Result;
        }
        /// <summary>
        /// Определяет название результирующего документа.
        /// </summary>
        /// <param name="userSession"> Пользовательская сессия.</param>
        /// <param name="FileCard"> Карточка файла протокола калибровки.</param>
        public static bool GetResultDocumentName(UserSession userSession, CardData FileCard)
        {
            string DocumentName = FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName);
            string TempFolder = System.IO.Path.GetTempPath();
            //string TempPath = TempFolder + "\\" + DocumentName;
            string TempPath = TempFolder + DocumentName;

            VersionedFileCard versionedFileCard = (VersionedFileCard)userSession.CardManager.GetCard((Guid)FileCard.Sections[CardFile.MainInfo.ID].FirstRow.GetGuid(CardFile.MainInfo.FileID));
            versionedFileCard.CurrentVersion.Download(TempPath);
            WordprocessingDocument ProtocolDocument = WordprocessingDocument.Open(TempPath, true);

            bool Result = ProtocolDocument.GetDocumentsResult();  // Определяется результат поверки прибора: годен/не годен
            ProtocolDocument.Close();
            return Result;
        }

    }
    /// <summary>
    /// Касс строки таблицы.
    /// </summary>
    class DescriptionRow
    {
        /// <summary>
        /// Тип строки.
        /// </summary>
        public enum RowsType
        {
            /// <summary>
            /// Заголовок таблицы.
            /// </summary>
            [Description("Заголовок")]
            Title = 0,
            /// <summary>
            /// Шапка таблицы.
            /// </summary>
            [Description("Шапка")]
            Header = 1,
            /// <summary>
            /// Подзаголовок.
            /// </summary>
            [Description("Подзаголовок")]
            Subtitle = 2,
            /// <summary>
            /// Обычная строка.
            /// </summary>
            [Description("Строка")]
            SimpleRow = 3,
        };
        public RowsType Type;
        public int ColumnsCount;
        public string[] Values;

        public DescriptionRow(RowsType Type, string[] Values)
        {
            this.Type = Type;
            this.Values = Values;
            this.ColumnsCount = Values != null ? Values.Count() : 1;
        }
    }
    /// <summary>
    /// Параметр с единицей измерения
    /// </summary>
    public class Unit
    {
        public string UnitsName;
        public decimal Value;
        public Unit(string UnitsName, decimal Value)
        {
            this.UnitsName = UnitsName;
            this.Value = Value;
        }
    }
    /// <summary>
    /// Класс-помощник для работы с различными единицами измерения
    /// </summary>
    public static class UnitsOfMeasure
    {
        /// <summary>
        /// Омы
        /// </summary>
        public enum Ohm
        {
            /// <summary>
            /// Мегаом.
            /// </summary>
            [Description("МОм")]
            MOhm = 0,
            /// <summary>
            /// Килоом.
            /// </summary>
            [Description("кОм")]
            kOhm = 1,
            /// <summary>
            /// Ом.
            /// </summary>
            [Description("Ом")]
            Ohm = 2,
            /// <summary>
            /// Миллиом.
            /// </summary>
            [Description("мОм")]
            mOhm = 3,
            /// <summary>
            /// Микроом.
            /// </summary>
            [Description("мкОм")]
            mkOhm = 4
        };
        /// <summary>
        /// Вольты
        /// </summary>
        public enum Volt
        {
            /// <summary>
            /// Мегавольт.
            /// </summary>
            [Description("МВ")]
            MV = 0,
            /// <summary>
            /// Киловольт.
            /// </summary>
            [Description("кВ")]
            kV = 1,
            /// <summary>
            /// Вольт.
            /// </summary>
            [Description("В")]
            V = 2,
            /// <summary>
            /// Милливольт.
            /// </summary>
            [Description("мВ")]
            mV = 3,
            /// <summary>
            /// Микровольт.
            /// </summary>
            [Description("мкВ")]
            mkV = 4
        };

        /// <summary>
        /// Амперы
        /// </summary>
        public enum Ampere
        {
            /// <summary>
            /// Мегаампер.
            /// </summary>
            [Description("МА")]
            MA = 0,
            /// <summary>
            /// Килоампер.
            /// </summary>
            [Description("кА")]
            kA = 1,
            /// <summary>
            /// Ампер.
            /// </summary>
            [Description("А")]
            A = 2,
            /// <summary>
            /// Миллиампер.
            /// </summary>
            [Description("мА")]
            mA = 3,
            /// <summary>
            /// Микроампер.
            /// </summary>
            [Description("мкА")]
            mkA = 4
        };

        /// <summary>
        /// Таблица соотношения единиц измерений.
        /// </summary>
        static Dictionary<String, decimal> Ratio
        {
            get
            {
                Dictionary<String, decimal> NewRatio = new Dictionary<String, decimal>();
                // Омы
                NewRatio.Add(Ohm.MOhm.GetDescription(), 1000000);
                NewRatio.Add(Ohm.kOhm.GetDescription(), 1000);
                NewRatio.Add(Ohm.Ohm.GetDescription(), 1);
                NewRatio.Add(Ohm.mOhm.GetDescription(), (decimal)0.001);
                NewRatio.Add(Ohm.mkOhm.GetDescription(), (decimal)0.000001);
                // Вольты
                NewRatio.Add(Volt.MV.GetDescription(), 1000000);
                NewRatio.Add(Volt.kV.GetDescription(), 1000);
                NewRatio.Add(Volt.V.GetDescription(), 1);
                NewRatio.Add(Volt.mV.GetDescription(), (decimal)0.001);
                NewRatio.Add(Volt.mkV.GetDescription(), (decimal)0.000001);
                // Амперы
                NewRatio.Add(Ampere.MA.GetDescription(), 1000000);
                NewRatio.Add(Ampere.kA.GetDescription(), 1000);
                NewRatio.Add(Ampere.A.GetDescription(), 1);
                NewRatio.Add(Ampere.mA.GetDescription(), (decimal)0.001);
                NewRatio.Add(Ampere.mkA.GetDescription(), (decimal)0.000001);
                return NewRatio;
            }
        }
        /// <summary>
        /// Получение параметра сопротивления (с указанием единицы изменения).
        /// </summary>
        /// <param name="Value">Строковое обозначение сопротивления.</param>
        /// <param name="Separator">Разделитель целой и дробной части.</param>
        /// <returns></returns>
        public static Unit GetResistanceValue(this string Value, string Separator)
        {
            string UnitsName = "";
            // Омы
            if (Value.IndexOf(" " + Ohm.MOhm.GetDescription()) >= 0) UnitsName = Ohm.MOhm.GetDescription();
            if (Value.IndexOf(" " + Ohm.kOhm.GetDescription()) >= 0) UnitsName = Ohm.kOhm.GetDescription();
            if (Value.IndexOf(" " + Ohm.Ohm.GetDescription()) >= 0) UnitsName = Ohm.Ohm.GetDescription();
            if (Value.IndexOf(" " + Ohm.mOhm.GetDescription()) >= 0) UnitsName = Ohm.mOhm.GetDescription();
            if (Value.IndexOf(" " + Ohm.mkOhm.GetDescription()) >= 0) UnitsName = Ohm.mkOhm.GetDescription();
            // Вольты
            if (Value.IndexOf(" " + Volt.MV.GetDescription()) >= 0) UnitsName = Volt.MV.GetDescription();
            if (Value.IndexOf(" " + Volt.kV.GetDescription()) >= 0) UnitsName = Volt.kV.GetDescription();
            if (Value.IndexOf(" " + Volt.V.GetDescription()) >= 0) UnitsName = Volt.V.GetDescription();
            if (Value.IndexOf(" " + Volt.mV.GetDescription()) >= 0) UnitsName = Volt.mV.GetDescription();
            if (Value.IndexOf(" " + Volt.mkV.GetDescription()) >= 0) UnitsName = Volt.mkV.GetDescription();
            // Амперы
            if (Value.IndexOf(" " + Ampere.MA.GetDescription()) >= 0) UnitsName = Ampere.MA.GetDescription();
            if (Value.IndexOf(" " + Ampere.kA.GetDescription()) >= 0) UnitsName = Ampere.kA.GetDescription();
            if (Value.IndexOf(" " + Ampere.A.GetDescription()) >= 0) UnitsName = Ampere.A.GetDescription();
            if (Value.IndexOf(" " + Ampere.mA.GetDescription()) >= 0) UnitsName = Ampere.mA.GetDescription();
            if (Value.IndexOf(" " + Ampere.mkA.GetDescription()) >= 0) UnitsName = Ampere.mkA.GetDescription();

            if (UnitsName != "")
                return new Unit(UnitsName, Convert.ToDecimal(Value.Substring(0, Value.IndexOf(" " + UnitsName)).Replace(Separator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)));
            else
                return null;
        }
        /// <summary>
        /// Операция сложения двух параметров.
        /// </summary>
        /// <param name="Unit1">Параметр сопротивления 1.</param>
        /// <param name="Unit2">Параметр сопротивления 2.</param>
        /// <param name="ResultUnitsName">Единица изменения, к которой приводится результат.</param>
        /// <returns></returns>
        public static Unit GetAddition(Unit Unit1, Unit Unit2, String ResultUnitsName)
        {
            decimal Value1 = Unit1.Value * Ratio[Unit1.UnitsName];
            decimal Value2 = Unit2.Value * Ratio[Unit2.UnitsName];

            return new Unit(ResultUnitsName, (Value1 + Value2) * Ratio[ResultUnitsName]);
        }
        /// <summary>
        /// Операция вычитания для двух параметров.
        /// </summary>
        /// <param name="Unit1">Параметр сопротивления 1.</param>
        /// <param name="Unit2">Параметр сопротивления 2.</param>
        /// <param name="ResultUnitsName">Единица изменения, к которой приводится результат.</param>
        /// <returns></returns>
        public static Unit GetSubtraction(Unit Unit1, Unit Unit2, String ResultUnitsName)
        {
            decimal Value1 = Unit1.Value * Ratio[Unit1.UnitsName];
            decimal Value2 = Unit2.Value * Ratio[Unit2.UnitsName];

            return new Unit(ResultUnitsName, (Value1 - Value2) * Ratio[ResultUnitsName]);
        }
        /// <summary>
        /// Операция умножения двух параметров.
        /// </summary>
        /// <param name="Unit1">Параметр сопротивления 1.</param>
        /// <param name="Unit2">Параметр сопротивления 2.</param>
        /// <param name="ResultUnitsName">Единица изменения, к которой приводится результат.</param>
        /// <returns></returns>
        public static Unit GetMultiplication(Unit Unit1, Unit Unit2, String ResultUnitsName)
        {
            decimal Value1 = Unit1.Value * Ratio[Unit1.UnitsName];
            decimal Value2 = Unit2.Value * Ratio[Unit2.UnitsName];

            return new Unit(ResultUnitsName, (Value1 * Value2) * Ratio[ResultUnitsName]);
        }
        /// <summary>
        /// Операция деления для двух параметров.
        /// </summary>
        /// <param name="Unit1">Параметр сопротивления 1.</param>
        /// <param name="Unit2">Параметр сопротивления 2.</param>
        /// <param name="ResultUnitsName">Единица изменения, к которой приводится результат.</param>
        /// <returns></returns>
        public static Unit GetDivision(Unit Unit1, Unit Unit2, String ResultUnitsName)
        {
            decimal Value1 = Unit1.Value * Ratio[Unit1.UnitsName];
            decimal Value2 = Unit2.Value * Ratio[Unit2.UnitsName];

            return new Unit(ResultUnitsName, (Value1 * Value2) * Ratio[ResultUnitsName]);
        }
        /// <summary>
        /// Операция вычисления погрешности для двух параметров.
        /// </summary>
        /// <param name="Unit1">Параметр сопротивления 1.</param>
        /// <param name="Unit2">Параметр сопротивления 2.</param>
        /// <param name="ResultUnitsName">Единица изменения, к которой приводится результат.</param>
        /// <returns></returns>
        public static Unit GetRatio(Unit Unit1, Unit Unit2, String ResultUnitsName)
        {
            decimal Value1 = Unit1.Value * Ratio[Unit1.UnitsName];
            decimal Value2 = Unit2.Value * Ratio[Unit2.UnitsName];
            decimal Value3 = Value1 - Value2;
            if (Value2 != 0)
            {
                decimal Value4 = Value3 / Value2;
                return new Unit(ResultUnitsName, Value4 * 100);
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// Класс данных для парсинга содержимого файла ДИ в формате json.
    /// </summary>
    public class RootObject
    {
        /// <summary>
        /// Данные измерений.
        /// </summary>
        public List<Measure> measures { get; set; }
        /// <summary>
        /// Тип прибора.
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// Заводской номер прибора.
        /// </summary>
        public string serial_number { get; set; }
        /// <summary>
        /// Год выпуска прибора.
        /// </summary>
        public int serial_year { get; set; }
        /// <summary>
        /// Температура.
        /// </summary>
        public string temp { get; set; }
        /// <summary>
        /// Получение строки данных измерений по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Measure GetMeasure(Int32 index)
        {
            // Отсеиваем строки с пометкой "ВС" и ищем альтернативную строку для той же точки измерения (в связи с особенностями файла ДИ для МИКО-9(А))
            Measure CurrentMeasure = measures[index];
            if (CurrentMeasure.name.Contains("ВС"))
            { return measures.First(r => (r.actual == CurrentMeasure.actual) && (r.name != CurrentMeasure.name)); }
            else
            { return CurrentMeasure; }
        }
    }
    /// <summary>
    /// Класс данных для парсинга данных измерений из файла ДИ в формате json.
    /// </summary>
    public class Measure
    {
        /// <summary>
        /// Действительное значение.
        /// </summary>
        public string actual { get; set; }
        /// <summary>
        /// Измеренное значение.
        /// </summary>
        public string average { get; set; }
        /// <summary>
        /// Ток.
        /// </summary>
        public string current { get; set; }
        /// <summary>
        /// Погрешность.
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// Точка измерения.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Получение параметра по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string Index(Int32 index)
        {
            switch (index)
            {
                case 0:
                    return actual;
                case 1:
                    return average;
                case 2:
                    return current;
                case 3:
                    return error;
                case 4:
                    return name;
            }
            return "";
        }
    }
}
