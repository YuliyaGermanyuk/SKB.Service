using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using DevExpress.XtraEditors;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.TakeOffice.Cards.Constants;

using SKB.Base;
using SKB.Service.Cards;

using Newtonsoft.Json;

namespace SKB.Service.CalibrationDocs
{
    class VerificationProtocol
    {
        public static bool Verify(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsNewDevice = false)
        {
            string ErrorText = "";

            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();

            // Наличие в системе средств поверки
            if (CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, false, true) == "-")
            { ErrorText = ErrorText + " - Не удалось определить средства поверки для указанного прибора.\n"; }

            // Наличие сведений об условиях поверки на указанную дату поверки
            ErrorText = ErrorText + CalibrationLib.CheckCalibrationJournal(CardScript, Context, VerificationDate);

            //** Проверка родительских документов **//

            TablesCreator Creator = new TablesCreator();
            string DeviceErrorText = "";

            // Проверка протокола приемосдаточных испытаний
            if (Creator.ProtokolDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.CalibrationProtocol))
            {
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.CalibrationProtocolCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные о приемосдаточных испытаниях для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
            }
            
            // Проверка данных измерений для некоторых приборов
            DeviceErrorText = "";
            if (CalibrationLib.MeasuringDataList.Any(r => r == DeviceTypeName))
            {
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.MeasuringDataCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные измерений для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
            }

            // Проверка данных измерений для некоторых приборов
            DeviceErrorText = "";
            if (Creator.ProtokolDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.MeasuringData))
            {
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.MeasuringDataCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные измерений для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
            }

            // Проверка протоколов приемосдаточных испытаний для датчиков
            if ((AdditionalWaresList != null) && (AdditionalWaresList.Count() > 0))
            {
                foreach (CardData Ware in AdditionalWaresList)
                {
                    Guid WareTypeID = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
                    string WareTypeName = ApplicationCard.UniversalCard.GetItemName(WareTypeID);

                    if (CalibrationLib.SensorsList.Any(r => r == WareTypeName))
                    {
                        string WareNumber = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Заводской номер прибора'").GetString(CardOrd.Properties.Value).ToString();
                        string WareYear = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = '/Год прибора'").GetString(CardOrd.Properties.Value).ToString();
                        WareNumber = WareNumber.Length == 4 ? WareNumber : WareNumber + "/" + WareYear;

                        string SensorsErrorText = CalibrationLib.CheckDocument(CardScript.Session, Ware, StartDateOfService, CalibrationLib.AcceptanceTestingProtocolCategoryID, IsNewDevice);
                        if (SensorsErrorText != "")
                        { ErrorText = ErrorText + " - Не удалось найти данные о приемосдаточных испытаниях для " + WareTypeName + " " + WareNumber + ":\n" + SensorsErrorText; }
                    }
                }
            }
            if (ErrorText == "")
            {
                return true;
            }
            else
            {
                XtraMessageBox.Show("Не удалось сформировать 'Протокол поверки'. Обнаружены следующие ошибки:\n\n" + ErrorText +
                    "\nОбратитесь к администратору системы.", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static CardData Create(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsPrimaryVerification)
        {
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;
            WordprocessingDocument Protocol = CalibrationLib.GetVerificationProtocolTemplate(CardScript.Session, DeviceTypeName);
            // Заполнение данных протокола
            FillData(Protocol, CardScript, Context, DeviceCard, VerificationDate, ClientName, AdditionalWaresList, StartDateOfService, IsPrimaryVerification);
            // Сохранение изменений
            Protocol.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Protocol.Close();
            // Создание карточки протокола калибровки
            CardData ProtocolFileCard = CalibrationLib.NewFileCard(Context, CardScript.Session, CalibrationLib.VerificationProtocolCategoryID,
                CalibrationLib.TempFolder + "\\" + "Протокол поверки " + DeviceTypeName.Replace("/", "-") + ".docm", VerificationDate, DeviceCard, ClientName, "", false);
            // Прикрепление карточки протокола калировки к карточке паспорта прибора
            CalibrationLib.AttachFileToCard(CardScript.Session, DeviceCard, ProtocolFileCard);
            return ProtocolFileCard;
        }

        public static void ReFill(CardData FileCard, ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsPrimaryVerification)
        {
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;

            WordprocessingDocument Protocol = CalibrationLib.GetVerificationProtocolTemplate(CardScript.Session, DeviceTypeName);
            // Заполнение данных протокола
            FillData(Protocol, CardScript, Context, DeviceCard, VerificationDate, ClientName, AdditionalWaresList, StartDateOfService, IsPrimaryVerification);
            // Сохранение изменений
            Protocol.MainDocumentPart.Document.Save();
            // Закрытие протокола
            Protocol.Close();
            // Замена файла в карточке протокола
            //CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "\\" + "Протокол поверки " + DeviceTypeName.Replace("/", "-") + ".docm", VerificationDate, DeviceCard,
            //    ClientName, "", false);
            CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "Протокол поверки " + DeviceTypeName.Replace("/", "-") + ".docm", VerificationDate, DeviceCard,
                ClientName, "", false);
            return;
        }
        /// <summary>
        /// Заполнение данных Протокола поверки.
        /// </summary>
        /// <param name="Protocol">Документ протокола поверки.</param>
        /// <param name="CardScript">Скрипт карточки.</param>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="DeviceCard">Карточка прибора.</param>
        /// <param name="VerificationDate">Дата проведения поверки.</param>
        /// <param name="ClientName">Наименование клиента.</param>
        /// <param name="AdditionalWaresList">Перечень доп. изделий.</param>
        /// <param name="StartDateOfService">Дата поступления на серивсное обслуживание.</param>
        /// <param name="IsPrimaryVerification">Флаг первичной поверки.</param>
        public static void FillData(WordprocessingDocument Protocol, ScriptClassBase CardScript, ObjectContext Context,
            CardData DeviceCard, DateTime VerificationDate, string ClientName, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsPrimaryVerification)
        {
            // ====== Получение информации для заполнения закладочных полей в протоколе поверки ===== //

            // Идентификатор типа прибора определяется из карточки "Паспорта прибора"
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            // Тип поверки определяется из входных данных (если прибор новый или после ремонта, то тип поверки - "первичная", если прибор клиента после калибровки/поверки - то тип поверки - "периодическая")
            string VerificationType = IsPrimaryVerification ? "ПЕРВИЧНОЙ" : "ПЕРИОДИЧЕСКОЙ";
            // Краткое название прибора определяется из универсального справочника типа "Приборы и комплектующие"
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            // Полное официальное название прибора определяется из универсального справочника типа "Приборы и комплектующие"
            string DisplayDeviceTypeName = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название").ToString() + " " + DeviceTypeName;
            // Перечень дополнительных изделий определяется из входного параметра AdditionalWaresList
            string AdditionalWares = (AdditionalWaresList.Count > 0 ? "c датчиками " : "") + AdditionalWaresList.Select(r => 
            ApplicationCard.UniversalCard.GetItemName(r.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid()) + " № " + (r.GetDeviceNumber().Contains("/") == true ? r.GetDeviceNumber().Replace("/", " (") + " г)" : r.GetDeviceNumber())).Aggregate(AdditionalWaresList.Count == 2 ? " и " : ", ");
            // Заводской номер прибора определяется из карточки "Паспорта прибора"
            string DeviceNumber = DeviceCard.GetDeviceNumber() + " " + AdditionalWares;
            // Дата проведения поверки определяется из входного параметра VerificationDate
            string VerificationDateString = VerificationDate.ToLongDateString();
            // ФИО текущего сотрудника определяется из справочника сотрудников (строка отображения)
            string StaffName = Context.GetCurrentEmployee().DisplayString;
            // ФИО руководителя отдела Метрологической лаборатории определяется из справочника сотрудников по должности
            string ManagerName = Context.GetEmployeeByPosition(SKB.Base.Ref.RefServiceCard.Roles.MetrologicalLabManager) != null ? Context.GetEmployeeByPosition(SKB.Base.Ref.RefServiceCard.Roles.MetrologicalLabManager).DisplayString : "";
            // Методика поверки определяется из универсального справочника типа "Пиборы и комплектующие"
            string VerificationMethods = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Методика поверки").ToString();
            // Дата выпуска прибора определяется из карточки "Паспорта прибора" (если в истории прибора есть строка о поступлени прибора на калибровку, то берется дата поступления на калибровку, если нет - то берется год прибора).
            string DateOfIssue = DeviceCard.GetDeviceDateOfIssue();

            // Перечень эталонов определяется из карточек "Оборудование" (отбираются все карточки "Оборудование", в которых указано, что данное оборудование является эталоном и используется для поверки текущего типа прибора).
            // Перечень эталонов разбивается на две части по количеству символов (для вставки в разные строки протокола поверки).
            string VerificationMeans = "";
            string VerificationMeans2 = "";
            CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, false, true, 4, out VerificationMeans, out VerificationMeans2);
            // Перечень вспомогательных средств поверки определяется из карточек "Оборудование" (отбираются все карточки "Оборудование", в которых указано, что данное оборудование является вспомогательным средством поверки и используется для поверки текущего типа прибора)
            // Перечень вспомогательных средств поверки разбивается на две части по количеству символов (для вставки в разные строки протокола поверки).
            string AuxiliaryMeshuring = "";
            string AuxiliaryMeshuring2 = "";
            CalibrationLib.GetListOfAuxiliaryMeasures(CardScript.Session, DeviceTypeID, false, true, 3, out AuxiliaryMeshuring, out AuxiliaryMeshuring2);
            
            // Значения влияющих факторов (температура, влажность, атмосферное давление) определяются из конструктора справочников, из узла "Журнал условий поверки".
            // Если прибор = МИКО-21, то значение атмосферного давления корректируется.
            // Определяются только в режиме авторматического заполнения протокола поверки.
            string Temperature;
            string Humidity;
            string AtmospherePressure;
            CalibrationLib.GetCalibrationConditions(CardScript.Session, Context, VerificationDate, out Temperature, out Humidity, out AtmospherePressure);
            if (DeviceTypeName == "МИКО-21") AtmospherePressure = Math.Round(Convert.ToDouble(AtmospherePressure)/7.501, 1).ToString();

            // ===== Заполнение закладочных полей ===== //
            FillBookmarks(Protocol, DeviceCard, VerificationType, VerificationDateString, DisplayDeviceTypeName, DeviceNumber, ClientName, DateOfIssue, VerificationMethods, VerificationMeans, VerificationMeans2,
                AuxiliaryMeshuring, AuxiliaryMeshuring2, Temperature, Humidity, AtmospherePressure, StaffName, ManagerName);

            // ===== Заполнение данных о метрологических характеристиках прибора ===== //
            
            // Определение режима заполнения протоколов поверки для текущего типа прибора
            int FormMode =  (Int32)ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Формирование протокола поверки");

            // Заполнение данных о метрологических характеристиках осуществляется только в автоматическом режиме заполнения протокола поверки
            // Если режим заполнения - автоматический, то добавляем таблицы с данными метрологических характеристик
            if (FormMode == 2)
            {
                // Получение объекта создателя таблиц
                TablesCreator Creator = new TablesCreator();
                // Добавление таблиц метрологических характеристик прибора (информация о таблицах и их содержимом получается из объекта создателя таблиц)
                AddDeviceTables(Protocol, CardScript.Session, Creator, DeviceTypeName, DeviceCard, StartDateOfService);
                
                // Добавление таблиц с результатами испытаний датчиков
                if (AdditionalWaresList != null)
                {
                    foreach (CardData Ware in AdditionalWaresList)
                    {
                        Guid WareTypeID = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
                        string WareTypeName = ApplicationCard.UniversalCard.GetItemName(WareTypeID);

                        if (CalibrationLib.SensorsList.Any(r => r == WareTypeName))
                        {
                            AddSensorTables(Protocol, CardScript.Session, Creator, WareTypeName, Ware, StartDateOfService, DeviceTypeName);
                        }
                    }
                }
            }
            return;
        }

        static void AddDeviceTables(WordprocessingDocument Protocol, UserSession Session, TablesCreator Creator, string DeviceType, CardData DeviceCard,
            DateTime StartDateOfService)
        {
            // Определение точек вставки таблиц для протоколов поверки разных типов приборов
            IEnumerable<VerificationProtocolTableLook> TablesCollection = Creator.VerifyProtokolDeviceTablesCollection.Where(r => r.DeviceTypes.Any(s => s == DeviceType));
            if (TablesCollection.Count() > 0)
            {
                OpenXmlElement IncertElement = Protocol.GetTable("Заключение:").ElementsBefore().Last();
                switch (DeviceType)
                {
                    case "ПКВ/М7":
                        IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений интервалов времени").ElementsBefore().Last();
                        break;
                    case "ПКВ/У3.0":
                        IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений интервалов времени").ElementsBefore().Last();
                        break;
                    case "ПКВ/У3.1":
                        IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений интервалов времени").ElementsBefore().Last();
                        break;
                    case "ПКВ/У3.0-01":
                        IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений интервалов времени").ElementsBefore().Last();
                        break;
                    /*case "МИКО-2.2":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-2.3":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;*/
                    case "МИКО-7":
                        IncertElement = Protocol.GetTable("7. Проверка ограничения выходной мощности").ElementsBefore().Last();
                        break;
                    /*case "МИКО-21":
                        IncertElement = Protocol.GetTable("Заключение:").ElementsAfter().First();
                        break;*/
                }
                //OpenXmlElement IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений силы постоянного электрического тока каналами ВКЛ и ОТКЛ силового коммутатора").ElementsAfter().First();
                Dictionary<String, Table> TablesList = new Dictionary<String, Table>();
                foreach (VerificationProtocolTableLook CurrentTable in TablesCollection)
                {
                    
                    //WordprocessingDocument ParentDocument;
                    string ParentDocumentPath = "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case VerificationProtocolTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocumentPath = CalibrationLib.GetDocumentPath(Session, DeviceCard, CalibrationLib.AcceptanceTestingProtocolCategoryID);
                            break;
                        case VerificationProtocolTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocumentPath = CalibrationLib.GetDocumentPath(Session, DeviceCard, CalibrationLib.CalibrationProtocolCategoryID);
                            break;
                        case VerificationProtocolTableLook.DocumentCategory.MeasuringData:
                            ParentDocumentPath = CalibrationLib.GetDocumentPath(Session, DeviceCard, CalibrationLib.MeasuringDataCategoryID);
                            break;
                        default:
                            ParentDocumentPath = "";
                            break;
                    }

                    if (ParentDocumentPath.EndsWith(".docx"))
                    {
                        WordprocessingDocument ParentDocument = WordprocessingDocument.Open(ParentDocumentPath, true);

                        if (TablesList.Any(r => r.Key == CurrentTable.TableName))
                        {
                            CurrentTable.AdditionDeviceTable(ParentDocument, TablesList.First(r => r.Key == CurrentTable.TableName).Value);
                        }
                        else
                        { TablesList.Add(CurrentTable.TableName, CurrentTable.GetDeviceTable(ParentDocument)); }

                        if (ParentDocument != null)
                            ParentDocument.Close();
                    }
                    if (ParentDocumentPath.EndsWith(".json"))
                    {
                        string fileContent = File.ReadAllText(ParentDocumentPath);
                        RootObject MeashureData = JsonConvert.DeserializeObject<RootObject>(fileContent);
                        TablesList.Add(CurrentTable.TableName, CurrentTable.GetDeviceTable(MeashureData));
                    }
                    File.Delete(ParentDocumentPath);
                }

                foreach (KeyValuePair<String, Table> T in TablesList)
                {
                    AddNewTable(Protocol, T.Value, IncertElement);
                }
            }
        }

        static void AddSensorTables(WordprocessingDocument Protocol, UserSession Session, TablesCreator Creator, string SensorType, CardData SensorCard,
            DateTime StartDateOfService, string DeviceTypeName)
        {
            string SensorNumber = SensorCard.GetDeviceNumber();
            IEnumerable<VerificationProtocolTableLook> TablesCollection = Creator.VerificationProtokolSensorTablesCollection.Where(r => r.DeviceTypes.Any(s => s == SensorType));
            if (TablesCollection.Count() == 0)
            { XtraMessageBox.Show("Для датчика '" + SensorType + "' не найден шаблон таблицы. Обратитесь к системному администратору."); }
            else
            {
                foreach (VerificationProtocolTableLook CurrentTable in TablesCollection)
                {
                    WordprocessingDocument ParentDocument;
                    string TempPath = "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case VerificationProtocolTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.AcceptanceTestingProtocolCategoryID, out TempPath);
                            break;
                        case VerificationProtocolTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.CalibrationProtocolCategoryID, out TempPath);
                            break;
                        case VerificationProtocolTableLook.DocumentCategory.MeasuringData:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.MeasuringDataCategoryID, out TempPath);
                            break;
                        default:
                            ParentDocument = null;
                            break;
                    }

                    Table NewSensorTable = CurrentTable.GetSensorsTable(ParentDocument, SensorNumber, DeviceTypeName);

                    AddNewTable(Protocol, NewSensorTable, Protocol.GetTable("Заключение:").ElementsBefore().Last());
                    if (ParentDocument != null) 
                    { 
                        ParentDocument.Close();
                        File.Delete(TempPath);
                    }
                }
            }
            return;
        }
        static void AddNewTable(WordprocessingDocument Protocol, Table NewTable, OpenXmlElement InsertElement)
        {
            InsertElement.InsertBeforeSelf(CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 10));
            InsertElement.InsertBeforeSelf((OpenXmlElement)NewTable);
        }
        /// <summary>
        /// Заполнение закладочных полей Протокола поверки
        /// </summary>
        /// <param name="VerifyDocument">Документ протокола поверки.</param>
        /// <param name="DeviceCard">Карточка прибора.</param>
        /// <param name="VerificationType">Тип поверки (первичная/периодическая)</param>
        /// <param name="VerificationDate">Дата проведения поверки.</param>
        /// <param name="DeviceType">Тип прибора.</param>
        /// <param name="DeviceNumber">Заводской номер прибора.</param>
        /// <param name="ClientName">Наименование клиента.</param>
        /// <param name="DateOfIssue">Дата выпуска прибора (дата поступления на калибровку).</param>
        /// <param name="VerificationMethods">Методика проведения поверки.</param>
        /// <param name="VerificationMeans">Перечень эталонов (часть 1).</param>
        /// <param name="VerificationMeans2">Перечень эталонов (часть 2).</param>
        /// <param name="AuxiliaryMeshuring">Вспомогательные средства поверки (часть 1).</param>
        /// <param name="AuxiliaryMeshuring2">Вспомогательные средства поверки (часть 2).</param>
        /// <param name="Temperature">Температура.</param>
        /// <param name="Humidity">Влажность.</param>
        /// <param name="AtmospherePressure">Атмосферное давление.</param>
        /// <param name="StaffName">ФИО поверителя.</param>
        /// <param name="ManagerName">ФИО руководителя метрологической лаборатории.</param>
        static void FillBookmarks(WordprocessingDocument VerifyDocument, CardData DeviceCard, string VerificationType, string VerificationDate, string DeviceType, string DeviceNumber,
            string ClientName, string DateOfIssue, string VerificationMethods, string VerificationMeans, string VerificationMeans2, string AuxiliaryMeshuring, string AuxiliaryMeshuring2, string Temperature, string Humidity, 
            string AtmospherePressure, string StaffName, string ManagerName)
        {
               Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in VerifyDocument.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            Dictionary<String, BookmarkStart> HeaderBookmarkDic = new Dictionary<String, BookmarkStart>();

            foreach(HeaderPart header in VerifyDocument.MainDocumentPart.HeaderParts)
                foreach (BookmarkStart Bookmark in header.RootElement.Descendants<BookmarkStart>())
                    HeaderBookmarkDic[Bookmark.Name] = Bookmark;

            /* Запись закладочных полей */
            if (HeaderBookmarkDic.Keys.Any(r => r == "VerifyType"))     HeaderBookmarkDic["VerifyType"].WriteText(VerificationType);
            if (HeaderBookmarkDic.Keys.Any(r => r == "VerifyDate"))     HeaderBookmarkDic["VerifyDate"].WriteText(VerificationDate);
            if (BookmarkDic.Keys.Any(r => r == "DeviceType"))           BookmarkDic["DeviceType"].WriteText(DeviceType);
            if (BookmarkDic.Keys.Any(r => r == "DeviceNumber"))         BookmarkDic["DeviceNumber"].WriteText(DeviceNumber);
            if (BookmarkDic.Keys.Any(r => r == "Client"))               BookmarkDic["Client"].WriteText(ClientName);
            if (BookmarkDic.Keys.Any(r => r == "DateOfIssue"))          BookmarkDic["DateOfIssue"].WriteText(DateOfIssue);
            if (BookmarkDic.Keys.Any(r => r == "Document"))             BookmarkDic["Document"].WriteText(VerificationMethods);
            if (BookmarkDic.Keys.Any(r => r == "Meshuring"))            BookmarkDic["Meshuring"].WriteText(VerificationMeans);
            if (BookmarkDic.Keys.Any(r => r == "Meshuring2"))           BookmarkDic["Meshuring2"].WriteText(VerificationMeans2);
            if (BookmarkDic.Keys.Any(r => r == "AuxiliaryMeshuring"))   BookmarkDic["AuxiliaryMeshuring"].WriteText(AuxiliaryMeshuring);
            if (BookmarkDic.Keys.Any(r => r == "AuxiliaryMeshuring2"))  BookmarkDic["AuxiliaryMeshuring2"].WriteText(AuxiliaryMeshuring2);
            if (BookmarkDic.Keys.Any(r => r == "Temperature"))          BookmarkDic["Temperature"].WriteText(Temperature);
            if (BookmarkDic.Keys.Any(r => r == "Humidity"))             BookmarkDic["Humidity"].WriteText(Humidity);
            if (BookmarkDic.Keys.Any(r => r == "AtmospherePressure"))   BookmarkDic["AtmospherePressure"].WriteText(AtmospherePressure);
            if (BookmarkDic.Keys.Any(r => r == "StaffName"))            BookmarkDic["StaffName"].WriteText(StaffName);
            if (BookmarkDic.Keys.Any(r => r == "ManagerName"))          BookmarkDic["ManagerName"].WriteText(ManagerName);
            if (BookmarkDic.Keys.Any(r => r == "VerifyDate2"))          BookmarkDic["VerifyDate2"].WriteText(VerificationDate);
            if (BookmarkDic.Keys.Any(r => r == "VerifyDate3"))          BookmarkDic["VerifyDate3"].WriteText(VerificationDate);
        }
    }
}
