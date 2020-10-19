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
using DocsVision.Platform.ObjectManager.SearchModel;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.CardLib;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.TakeOffice.Cards.Constants;

using SKB.Base;
using SKB.Service.Cards;
using Newtonsoft.Json;

namespace SKB.Service.CalibrationDocs
{
    class CalibrationProtocol
    {
        static Dictionary<string, string> FilesDic = new Dictionary<string, string>()
        {
            {"ПКВ/М7", "ПКВ_М7_2_№ХХ ГГГГ.cpd"},
            {"ПКВ/У3.0", "ПКВ_У3.0_№ХХ ГГГГ.cpd"},
            {"ПКВ/У3.1", "ПКВ_У3.1_№ХХ ГГГГ.cpd"},
            {"ПКВ/У3.0-01", "ПКВ_У3.0-01_№ХХ ГГГГ.cpd"},
            {"МИКО-2.3", "МИКО-2.3_№ХХ_ГГГГ.mcd"},
            {"МИКО-7", "МИКО-7_№ХХ_ГГГГ.mcd"},
            {"МИКО-8", "МИКО-8_№ХХ_ГГГГ.mcd"},
            {"МИКО-21", "МИКО-21_№ХХ_ГГГГ.mcd"}          
        };

        public static bool Verify(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsNewDevice = false)
        {
            string ErrorText = "";

            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();

            // Наличие в справочнике средств калибровки
            //if (ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Средства калибровки") == null)
            if (CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, true, false) == "-")
            { ErrorText = ErrorText + " - Не удалось определить средства калибровки для указанного прибора.\n"; }

            // Наличие сведений об условиях калибровки на указанную дату калибровки
            ErrorText = ErrorText + CalibrationLib.CheckCalibrationJournal(CardScript, Context, CalibrationDate);

            //** Проверка родительских документов **//

            TablesCreator Creator = new TablesCreator();
            string DeviceErrorText = "";

            // Проверка протокола калибровки прибора
            if (Creator.ProtokolDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.CalibrationProtocol))
            {
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.CalibrationProtocolCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные о калибровке для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
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
                XtraMessageBox.Show("Не удалось сформировать 'Протокол калибровки'. Обнаружены следующие ошибки:\n\n" + ErrorText +
                    "\nОбратитесь к администратору системы.", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static CardData Create(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService)
        {
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string ClientName = Client.Equals(Guid.Empty) ? "СКБ ЭП" : Context.GetObject<PartnersCompany>(Client).Name;
            WordprocessingDocument Protocol = CalibrationLib.GetCalibrationProtocolTemplate(CardScript.Session, DeviceTypeName);
            // Заполнение данных протокола
            FillData(Protocol, CardScript, Context, DeviceCard, CalibrationDate, AdditionalWaresList, StartDateOfService);
            // Сохранение изменений
            Protocol.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Protocol.Close();
            // Создание карточки протокола калибровки
            CardData ProtocolFileCard = CalibrationLib.NewFileCard(Context, CardScript.Session, CalibrationLib.CalibrationProtocolCategoryID,
                CalibrationLib.TempFolder + "\\" + "Протокол калибровки " + DeviceTypeName.Replace("/", "-") + ".docm", CalibrationDate, DeviceCard, ClientName, "", true);
            // Прикрепление карточки протокола калировки к карточке паспорта прибора
            CalibrationLib.AttachFileToCard(CardScript.Session, DeviceCard, ProtocolFileCard);
            return ProtocolFileCard;
        }

        public static void ReFill(CardData FileCard, ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService)
        {
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string ClientName = Client.Equals(Guid.Empty) ? "СКБ ЭП" : Context.GetObject<PartnersCompany>(Client).Name;

            WordprocessingDocument Protocol = CalibrationLib.GetCalibrationProtocolTemplate(CardScript.Session, DeviceTypeName);
            // Заполнение данных протокола
            FillData(Protocol, CardScript, Context, DeviceCard, CalibrationDate, AdditionalWaresList, StartDateOfService);
            // Сохранение изменений
            Protocol.MainDocumentPart.Document.Save();
            // Закрытие протокола
            Protocol.Close();
            // Замена файла в карточке протокола
            CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "\\" + "Протокол калибровки " + DeviceTypeName.Replace("/", "-") + ".docm", CalibrationDate, DeviceCard,
                ClientName, "", true);
            return;
        }
        /// <summary>
        /// Заполнение данных протокола
        /// </summary>
        /// <param name="Protocol">Документ протокола.</param>
        /// <param name="CardScript">Скрипт карточки.</param>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="DeviceCard">Карточка прибора.</param>
        /// <param name="CalibrationDate">Дата калибровки.</param>
        /// <param name="AdditionalWaresList">Перечень доп. изделий.</param>
        /// <param name="StartDateOfService">Дата начала сервисного обслуживания.</param>
        public static void FillData(WordprocessingDocument Protocol, ScriptClassBase CardScript, ObjectContext Context,
            CardData DeviceCard, DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService)
        {
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DisplayDeviceTypeName;
            if (CalibrationLib.AdditionalWaresList.Any(r => r == DeviceTypeName))
            { DisplayDeviceTypeName = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название").ToString(); }
            switch (DeviceTypeName)
            {
                case "ПКР-2М":
                    DisplayDeviceTypeName = "ПКР-2";
                    break;
                case "ПКВ/У3.0-01":
                    DisplayDeviceTypeName = "ПКВ/У3.0";
                    break;
                default:
                    DisplayDeviceTypeName = DeviceTypeName;
                    break;
            }
            string DeviceNumber = DeviceCard.GetDeviceNumber();
            // Данные для заполнения закладочных полей
            string CalibrationDateString = CalibrationDate.ToLongDateString();
            //string CalibrationMeans = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Средства калибровки").ToString();
            string CalibrationMeans = CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, true, false) + ".";
            string FileName = CalibrationLib.MeasuringDataList.Any(r => r == DeviceTypeName) ? CalibrationLib.GetDocumentCard(CardScript.Session, DeviceCard, CalibrationLib.MeasuringDataCategoryID).Sections[CardFile.MainInfo.ID].FirstRow.GetString(CardFile.MainInfo.FileName) : "";
            string StaffName = Context.GetCurrentEmployee().DisplayString;

            // Заполнение закладочный полей
            string Temperature;
            string Humidity;
            string AtmospherePressure;
            CalibrationLib.GetCalibrationConditions(CardScript.Session, Context, CalibrationDate, out Temperature, out Humidity, out AtmospherePressure);
            FillBookmarks(Protocol, DeviceCard, DisplayDeviceTypeName, DeviceNumber, CalibrationDateString, CalibrationMeans, StaffName, Temperature, Humidity, AtmospherePressure, FileName);
            /*TablesCreator Creator = new TablesCreator();
            
            // Добавление таблиц с результатами испытаний датчиков
            //Random RandomValue = new Random();
            if (AdditionalWaresList != null)
            {
                foreach (CardData Ware in AdditionalWaresList)
                {
                    Guid WareTypeID = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
                    string WareTypeName = ApplicationCard.UniversalCard.GetItemName(WareTypeID);

                    if (CalibrationLib.SensorsList.Any(r => r == WareTypeName))
                    {
                        AddSensorTables(Protocol, CardScript.Session, Creator, WareTypeName, Ware, StartDateOfService);
                    }
                }
            }*/


            //** Заполнение данных о метрологических характеристиках **//
            TablesCreator Creator = new TablesCreator();
            // Добавление таблиц метрологических характеристик прибора
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
                        AddSensorTables(Protocol, CardScript.Session, Creator, WareTypeName, Ware, StartDateOfService);
                    }
                }
            }

            return;
        }

        /*static string GetCalibrationMeans(ScriptClassBase CardScript, Guid DeviceTypeID)
        {
            SearchQuery searchQuery = CardScript.Session.CreateSearchQuery();
            // Поиск по типу карточки    
            CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(MyHelper.RefType_EA);
            // Поиск по секции
            SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(new Guid("{5A8CC505-7A0B-4CAB-84DD-8C23F26A90D3}"));
            // Поиск по значению поля
            sectionQuery.ConditionGroup.Conditions.AddNew("TypeServiceFact", FieldType.Int, ConditionOperation.IsNotNull);
            // Получение текста запроса
            string query = searchQuery.GetXml(null, true);
            // Выполнение запроса
            CardDataCollection coll = session.CardManager.FindCards(query);
            Logger.Debug("Выполнили поисковый запрос.");
            string CalibrationMeans = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Средства калибровки").ToString();
        }*/
        /// <summary>
        /// Добавить в протокол таблицу с данными измерений прибора.
        /// </summary>
        /// <param name="Protocol">Документ протокола.</param>
        /// <param name="Session">Пользовательская сессия.</param>
        /// <param name="Creator">Объект создателя таблиц.</param>
        /// <param name="DeviceType">Тип прибора.</param>
        /// <param name="DeviceCard">Карточка прибора.</param>
        /// <param name="StartDateOfService">Дата начала сервисного обслуживания.</param>
        static void AddDeviceTables(WordprocessingDocument Protocol, UserSession Session, TablesCreator Creator, string DeviceType, CardData DeviceCard,
            DateTime StartDateOfService)
        {
            IEnumerable<ProtocolTableLook> TablesCollection = Creator.ProtokolDeviceTablesCollection.Where(r => r.DeviceTypes.Any(s => s == DeviceType));
            if (TablesCollection.Count() > 0)
            {
                OpenXmlElement IncertElement = Protocol.GetTable("Дата").ElementsBefore().Last();
                switch (DeviceType)
                {
                    case "ПКВ/М7":
                        IncertElement = Protocol.GetTable("Определение основной абсолютной погрешности измерений силы постоянного электрического тока каналами ВКЛ и ОТКЛ силового коммутатора").ElementsAfter().First();
                        break;
                    case "ПКВ/У3.0":
                        IncertElement = Protocol.GetTable("Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора").ElementsAfter().First();
                        break;
                    case "ПКВ/У3.1":
                        IncertElement = Protocol.GetTable("Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора").ElementsAfter().First();
                        break;
                    case "ПКВ/У3.0-01":
                        IncertElement = Protocol.GetTable("Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора").ElementsAfter().First();
                        break;
                    case "МИКО-2.2":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-2.3":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-7":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-8":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-10":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                    case "МИКО-21":
                        IncertElement = Protocol.GetTable("Определение метрологических характеристик").ElementsAfter().First();
                        break;
                }

                Dictionary<String, Table> TablesList = new Dictionary<String, Table>();
                foreach (ProtocolTableLook CurrentTable in TablesCollection)
                {
                    
                    string ParentDocumentPath = "";
                    //string TempPath = "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case ProtocolTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocumentPath = CalibrationLib.GetDocumentPath(Session, DeviceCard, CalibrationLib.AcceptanceTestingProtocolCategoryID);
                            break;
                        case ProtocolTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocumentPath = CalibrationLib.GetDocumentPath(Session, DeviceCard, CalibrationLib.CalibrationProtocolCategoryID);
                            break;
                        case ProtocolTableLook.DocumentCategory.MeasuringData:
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
        /// <summary>
        /// Добавить в протокол таблицу с данными измерений датчика.
        /// </summary>
        /// <param name="Protocol">Документ протокола</param>
        /// <param name="Session">пользовательская сессия.</param>
        /// <param name="Creator">Объект создателя таблиц.</param>
        /// <param name="SensorType">Тип датчика.</param>
        /// <param name="SensorCard">Карточка датчика.</param>
        /// <param name="StartDateOfService">Дата начала сервисного обслуживания.</param>
        static void AddSensorTables(WordprocessingDocument Protocol, UserSession Session, TablesCreator Creator, string SensorType, CardData SensorCard,
            DateTime StartDateOfService)
        {
            string SensorNumber = SensorCard.GetDeviceNumber();
            IEnumerable<ProtocolTableLook> TablesCollection = Creator.ProtokolSensorTablesCollection.Where(r => r.DeviceTypes.Any(s => s == SensorType));
            if (TablesCollection.Count() == 0)
            { XtraMessageBox.Show("Для датчика '" + SensorType + "' не найден шаблон таблицы. Обратитесь к системному администратору."); }
            else
            {
                foreach (ProtocolTableLook CurrentTable in TablesCollection)
                {
                    WordprocessingDocument ParentDocument;
                    string TempPath = "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case ProtocolTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.AcceptanceTestingProtocolCategoryID, out TempPath);
                            break;
                        case ProtocolTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.CalibrationProtocolCategoryID, out TempPath);
                            break;
                        case ProtocolTableLook.DocumentCategory.MeasuringData:
                            ParentDocument = CalibrationLib.GetDocument(Session, SensorCard, CalibrationLib.MeasuringDataCategoryID, out TempPath);
                            break;
                        default:
                            ParentDocument = null;
                            break;
                    }

                    Table NewSensorTable = CurrentTable.GetSensorsTable(ParentDocument, SensorNumber);

                    /*if (Protocol.MainDocumentPart.Document.Body.Elements<Table>().Count() > 0)
                    {
                        List<OpenXmlElement> NewList = Protocol.MainDocumentPart.Document.Body.Elements().ToList();
                        int Index = NewList.Count - 4;
                        NewList.Insert(Index, CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 11));
                        NewList.Insert(Index, (OpenXmlElement)NewSensorTable);
                        Protocol.MainDocumentPart.Document.Body.RemoveAllChildren();
                        foreach (OpenXmlElement Element in NewList)
                        { Protocol.MainDocumentPart.Document.Body.Append(Element); }
                    }
                    else
                    {
                        SdtBlock Block = Protocol.MainDocumentPart.Document.Body.Elements<SdtBlock>().First();
                        SdtContentBlock ContentBlock = Block.Elements<SdtContentBlock>().First();
                        List<OpenXmlElement> NewList = ContentBlock.Elements().ToList();
                        int Index = NewList.Count - 4;
                        NewList.Insert(Index, CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 11));
                        NewList.Insert(Index, (OpenXmlElement)NewSensorTable);
                        ContentBlock.RemoveAllChildren();
                        foreach (OpenXmlElement Element in NewList)
                        { ContentBlock.Append(Element); }
                    }*/
                    AddNewTable(Protocol, NewSensorTable, Protocol.GetTable("Дата").ElementsBefore().Last());
                    if (ParentDocument != null) 
                    { 
                        ParentDocument.Close();
                        File.Delete(TempPath);
                    }
                }
            }
            return;
        }
        /// <summary>
        /// Добавить новую таблицу в документ
        /// </summary>
        /// <param name="Protocol">Документ протокола.</param>
        /// <param name="NewTable">Объект таблицы.</param>
        /// <param name="InsertElement">Объект документа, перед которым требуется вставить таблицу.</param>
        static void AddNewTable(WordprocessingDocument Protocol, Table NewTable, OpenXmlElement InsertElement)
        {
            InsertElement.InsertBeforeSelf(CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 11));
            InsertElement.InsertBeforeSelf((OpenXmlElement)NewTable);

            /*if (Protocol.MainDocumentPart.Document.Body.Elements<Table>().Count() > 0)
            {
                
                List<OpenXmlElement> NewList = Protocol.MainDocumentPart.Document.Body.Elements().ToList();
                int Index = NewList.Count - 4;
                NewList.Insert(Index, CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 11));
                NewList.Insert(Index, (OpenXmlElement)NewTable);
                Protocol.MainDocumentPart.Document.Body.RemoveAllChildren();
                foreach (OpenXmlElement Element in NewList)
                { Protocol.MainDocumentPart.Document.Body.Append(Element); }

                
            }
            else
            {
                SdtBlock Block = Protocol.MainDocumentPart.Document.Body.Elements<SdtBlock>().First();
                SdtContentBlock ContentBlock = Block.Elements<SdtContentBlock>().First();
                List<OpenXmlElement> NewList = ContentBlock.Elements().ToList();
                int Index = NewList.Count - 4;
                NewList.Insert(Index, CalibrationLib.NewParagraph(-113, -133, JustificationValues.Left, 11));
                NewList.Insert(Index, (OpenXmlElement)NewTable);
                ContentBlock.RemoveAllChildren();
                foreach (OpenXmlElement Element in NewList)
                { ContentBlock.Append(Element); }
            }*/
        }

        static void FillBookmarks(WordprocessingDocument Certificate, CardData DeviceCard, string DeviceType, string DeviceNumber,
            string CalibrationDate, string CalibrationMeans, string StaffName, string Temperature, string Humidity, string AtmospherePressure, string FileName)
        {
            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Certificate.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            /* Запись закладочных полей */
            BookmarkDic["DeviceType"].WriteText(DeviceType);
            BookmarkDic["DeviceNumber"].WriteText(DeviceNumber);
            BookmarkDic["CalibrationDate"].WriteText(CalibrationDate);
            BookmarkDic["StaffName"].WriteText(StaffName);
            BookmarkDic["CalibrationMeans"].WriteText(CalibrationMeans);
            BookmarkDic["Temperature"].WriteText(Temperature);
            BookmarkDic["Humidity"].WriteText(Humidity);
            BookmarkDic["AtmospherePressure"].WriteText(AtmospherePressure);
        }


        

    }
}
