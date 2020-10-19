using System;
using System.IO;
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
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.CardLib;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.TakeOffice.Cards.Constants;

using SKB.Base;
using SKB.Service.Cards;

namespace SKB.Service.CalibrationDocs
{
    static class CalibrationCertificate
    {
        
        public static bool Verify(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsNewDevice = false)
        {
            string ErrorText = "";

            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();

            // Наличие в справочнике полного названия прибора
            if (ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название") == null)
            { ErrorText = ErrorText + " - Не удалось определить полное название прибора.\n"; }

            // Наличие в справочнике методики поверки
            if (ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Методика поверки") == null)
            { ErrorText = ErrorText + " - Не удалось определить методику поверки для указанного прибора.\n"; }

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
            if (Creator.CertificateDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.CalibrationProtocol))
            {
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.CalibrationProtocolCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные о калибровке для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
            }

            // Проверка данных измерений для некоторых приборов
            //if (CalibrationLib.MeasuringDataList.Any(r => r == DeviceTypeName))
            if (Creator.CertificateDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.MeasuringData))
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
                XtraMessageBox.Show("Не удалось сформировать 'Сертификат о калибровке'. Обнаружены следующие ошибки:\n\n" + ErrorText + 
                    "\nОбратитесь к администратору системы.", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
        

        public static CardData Create(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, DateTime StartDateOfCalibration, bool NewDevice = false)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "СКБ ЭП" : Context.GetObject<PartnersCompany>(Client).Name;
            WordprocessingDocument Certificate = CalibrationLib.GetCalibrationCertificateTemplate(CardScript.Session);
            // Заполнение данных сертификата
            FillData(Certificate, CardScript, Context, DeviceCard, ClientName, CalibrationDate, AdditionalWaresList, StartDateOfService, StartDateOfCalibration);
            // Сохранение изменений
            Certificate.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Certificate.Close();
            // Создание карточки сертификата
            CardData NewFileCard = CalibrationLib.NewFileCard(Context, CardScript.Session, CalibrationLib.CalibrationCertificateCategoryID, CalibrationLib.TempFolder + "\\" + "Сертификат о калибровке.docx", CalibrationDate,
                DeviceCard, ClientName, "", true, NewDevice);
            // Прикрепление карточки сертификата к карточке паспорта прибора
            CalibrationLib.AttachFileToCard(CardScript.Session, DeviceCard, NewFileCard);
            
            return NewFileCard;
        }
        public static void ReFill(CardData FileCard, ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, DateTime StartDateOfCalibration)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "СКБ ЭП" : Context.GetObject<PartnersCompany>(Client).Name;

            WordprocessingDocument Certificate = CalibrationLib.GetCalibrationCertificateTemplate(CardScript.Session);
            // Заполнение данных сертификата
            FillData(Certificate, CardScript, Context, DeviceCard, ClientName, CalibrationDate, AdditionalWaresList, StartDateOfService, StartDateOfCalibration);
            // Сохранение изменений
            Certificate.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Certificate.Close();
            // Замена файла в карточке сертификата
            CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "\\" + "Сертификат о калибровке.docx", CalibrationDate, DeviceCard, ClientName, "", true);
            return;
        }
        public static void FillData(WordprocessingDocument Certificate, ScriptClassBase CardScript, ObjectContext Context,
            CardData DeviceCard, string ClientName, DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, DateTime StartDateOfCalibration)
        {
            // Формирование сертификата
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();
            DeviceNumber = DeviceNumber.Contains("/") == true ? DeviceNumber.Replace("/", " (") + " г)" : DeviceNumber;
            string DeviceNumber2;
            switch (DeviceTypeName)
            {
                case "ПКР-2М":
                    DeviceNumber = "ПКР-2" + " № " + DeviceNumber;
                    DeviceNumber2 = "";
                    break;
                case "ПКВ/У3.0-01":
                    DeviceNumber = "ПКВ/У3.0" + " № " + DeviceNumber;
                    DeviceNumber2 = "";
                    break;
                case "ТК-026":
                    DeviceNumber = " № " + DeviceNumber;
                    DeviceNumber2 = "(для ПКВ/М7)";
                    break;
                case "ТК-021":
                    DeviceNumber = " № " + DeviceNumber;
                    DeviceNumber2 = "(для ПКВ/У3)";
                    break;
                default:
                    DeviceNumber = DeviceTypeName + " № " + DeviceNumber;
                    DeviceNumber2 = "";
                    break;
            }
            // Данные для заполнения закладочных полей
            string FullDeviceType = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название").ToString();
            string ReceiptDateString = StartDateOfCalibration.ToLongDateString();
            string CalibrationDateString = CalibrationDate.ToLongDateString();
            string CalibrationMethods = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Методика поверки").ToString();
            //string CalibrationMeans = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Средства калибровки").ToString();
            string CalibrationMeans = CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, true, false) + ".";
            // Условия калибровки
            string Temperature;
            string Humidity;
            string AtmospherePressure;
            CalibrationLib.GetCalibrationConditions(CardScript.Session, Context, CalibrationDate, out Temperature, out Humidity, out AtmospherePressure);
            // Заполнение закладочный полей
            FillBookmarks(Certificate, FullDeviceType, DeviceNumber, DeviceNumber2, ReceiptDateString, ClientName, CalibrationDateString, CalibrationMethods, Temperature, 
                Humidity, AtmospherePressure, CalibrationMeans);
            //** Заполнение данных о метрологических характеристиках **//
            // Добавление заголовка
            AddTitle(Certificate);
            // Добавление таблицы метрологических характеристик прибора
            TablesCreator Creator = new TablesCreator();
            AddDeviceTables(Certificate, CardScript.Session, Creator, DeviceTypeName, DeviceCard, StartDateOfService);
            // Добавление таблиц метрологических характеристик датчиков
            if (AdditionalWaresList != null)
            {
                foreach (CardData Ware in AdditionalWaresList)
                {
                    Guid WareTypeID = Ware.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
                    string WareTypeName = ApplicationCard.UniversalCard.GetItemName(WareTypeID);
                    if (CalibrationLib.SensorsList.Any(r => r == WareTypeName))
                    {
                        string WareNumber = Ware.GetDeviceNumber();
                        AddSensorTables(Certificate, CardScript.Session, Creator, WareTypeName, WareNumber, Ware, StartDateOfService);
                    }
                }
            }
            // Добавление дополнительных данных
            AddAdditionalTables(Certificate, Creator, DeviceTypeName);
            return;
        }

        static void FillBookmarks(WordprocessingDocument Certificate, string DeviceType, string DeviceNumber, string DeviceNumber2, string ReceiptDate, string ClientName,
            string CalibrationDate, string CalibrationMethods, string Temperature, string Humidity, string AtmospherePressure, string CalibrationMeans)
        {
            CalibrationLib.ChangeCellsWidth(Certificate.GetTable("СЕРТИФИКАТ О КАЛИБРОВКЕ №"), 3, 0, DeviceType.Length);

            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Certificate.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            /* Запись закладочных полей */
            BookmarkDic["DeviceType"].WriteText(DeviceType);
            BookmarkDic["DeviceNumber"].WriteText(DeviceNumber);
            BookmarkDic["DeviceNumber2"].WriteText(DeviceNumber2);
            BookmarkDic["ReceiptDate"].WriteText(ReceiptDate);

            /*if (ClientName.Length > 55)
            {
                int Length1 = ClientName.Substring(0, 50).LastIndexOf(" ");
                string Text1 = ClientName.Substring(0, Length1);
                BookmarkDic["ClientName"].WriteText(Text1);

                ClientName = ClientName.Substring(Length1 + 1);
                Paragraph paragraph = (Paragraph)BookmarkDic["ClientName"].Parent;
                TableCell tableCell = (TableCell)paragraph.Parent;
                TableRow tableRow = (TableRow)tableCell.Parent;
                Table table = (Table)tableRow.Parent;
                int RowIndex = table.Elements().ToList().IndexOf(tableRow);

                while (ClientName.Length > 90)
                {
                    int LineLength = ClientName.Substring(0, 90).LastIndexOf(" ");
                    string Text = ClientName.Substring(0, LineLength);
                    ClientName = ClientName.Substring(LineLength + 1);

                    table.InsertLineForClient(RowIndex + 1, Text);
                    RowIndex++;
                }
                table.InsertLineForClient(RowIndex + 1, ClientName);
            }
            else
            {
                BookmarkDic["ClientName"].WriteText(ClientName);
            }*/

            BookmarkDic["CalibrationDate"].WriteText(CalibrationDate);
            BookmarkDic["CalibrationMethods"].WriteText(CalibrationMethods);
            BookmarkDic["Temperature"].WriteText(Temperature);
            BookmarkDic["Humidity"].WriteText(Humidity);
            BookmarkDic["AtmospherePressure"].WriteText(AtmospherePressure);
            BookmarkDic["CalibrationMeans"].WriteText(CalibrationMeans);
        }
        static void AddTitle(WordprocessingDocument Certificate)
        {
            Paragraph paragraph = CalibrationLib.NewParagraph(-284, 141, JustificationValues.Center, 8);
            Run run = CalibrationLib.NewRun(14);
            run.Append(CalibrationLib.NewText("Действительные значения метрологических характеристик"));
            paragraph.Append(run);
            Certificate.MainDocumentPart.Document.Body.Append(paragraph);
            Certificate.MainDocumentPart.Document.Body.Append(CalibrationLib.NewParagraph(0, 0, JustificationValues.Center, 8));
        }
        static void AddDeviceTables(WordprocessingDocument Certificate, UserSession Session, TablesCreator Creator, string DeviceType, CardData DeviceCard, 
            DateTime StartDateOfService)
        {
            //WordprocessingDocument CalibrationProtocol = GetDocument(Session, DeviceCard, StartDateOfService, CalibrationProtocolCategoryID);
            //XtraMessageBox.Show("DeviceType = " + DeviceType);
            IEnumerable<CertificateTableLook> TablesCollection = Creator.CertificateDeviceTablesCollection.Where(r => r.DeviceTypes.Any(s => s == DeviceType));
            if (TablesCollection.Count() == 0)
            { XtraMessageBox.Show("Для прибра '" + DeviceType + "' не найден шаблон таблицы. Обратитесь к системному администратору."); }
            else
            {
                Dictionary<String, Table> TablesList = new Dictionary<String, Table>();
                foreach (CertificateTableLook CurrentTable in TablesCollection)
                {
                    WordprocessingDocument ParentDocument;
                    string TempPath =  "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.AcceptanceTestingProtocolCategoryID, out TempPath);
                            break;
                        case CertificateTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.CalibrationProtocolCategoryID, out TempPath);
                            break;
                        case CertificateTableLook.DocumentCategory.MeasuringData:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.MeasuringDataCategoryID, out TempPath);
                            break;
                        default:
                            ParentDocument = null;
                            break;
                    }
                    if (TablesList.Any(r => r.Key == CurrentTable.TableName))
                    { CurrentTable.AdditionDeviceTable(ParentDocument, TablesList.First(r => r.Key == CurrentTable.TableName).Value); }
                    else
                    { TablesList.Add(CurrentTable.TableName, CurrentTable.GetDeviceTable(ParentDocument)); }
                    //Certificate.MainDocumentPart.Document.Body.Append(CurrentTable.GetDeviceTable(ParentDocument));
                    //Certificate.MainDocumentPart.Document.Body.Append(CalibrationLib.NewParagraph(0, 0, JustificationValues.Left, 10));
                    if (ParentDocument != null) 
                    { 
                        ParentDocument.Close();
                        File.Delete(TempPath);
                    }
                }
                foreach (KeyValuePair<String, Table> T in TablesList)
                {
                    Certificate.MainDocumentPart.Document.Body.Append(T.Value);
                }
            }
        }
        static void AddSensorTables(WordprocessingDocument Certificate, UserSession Session,TablesCreator Creator, string DeviceType, string DeviceNumber, 
            CardData DeviceCard, DateTime StartDateOfService)
        {
            IEnumerable<CertificateTableLook> TablesCollection = Creator.CertificateSensorTablesCollection.Where(r => r.DeviceTypes.Any(s => s == DeviceType));
            if (TablesCollection.Count() == 0)
            { XtraMessageBox.Show("Для датчика '" + DeviceType + "' не найден шаблон таблицы. Обратитесь к системному администратору."); }
            else
            {
                foreach (CertificateTableLook CurrentTable in TablesCollection)
                {
                    WordprocessingDocument ParentDocument;
                    string TempPath = "";
                    switch (CurrentTable.ParentDocumentCategory)
                    {
                        case CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.AcceptanceTestingProtocolCategoryID, out TempPath);
                            break;
                        case CertificateTableLook.DocumentCategory.CalibrationProtocol:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.CalibrationProtocolCategoryID, out TempPath);
                            break;
                        case CertificateTableLook.DocumentCategory.MeasuringData:
                            ParentDocument = CalibrationLib.GetDocument(Session, DeviceCard, CalibrationLib.MeasuringDataCategoryID, out TempPath);
                            break;
                        default:
                            ParentDocument = null;
                            break;
                    }
                    Certificate.MainDocumentPart.Document.Body.Append(CurrentTable.GetSensorsTable(ParentDocument, DeviceNumber));
                        //Certificate.MainDocumentPart.Document.Body.Append(CalibrationLib.NewParagraph(0, 0, JustificationValues.Left, 10));
                    if (ParentDocument != null) 
                    {
                        ParentDocument.Close();
                        File.Delete(TempPath);
                    }
                }
            }
        }
        static void AddAdditionalTables(WordprocessingDocument Certificate, TablesCreator Creator, string DeviceType)
        {
            IEnumerable<CertificateTableLook> TablesCollection = Creator.CertificateAdditionalTablesCollection.Where(r => r.DeviceTypes.Any(s => s == DeviceType));
            if (TablesCollection.Count() > 0)
            {
                foreach (CertificateTableLook CurrentTable in TablesCollection)
                {
                    Certificate.MainDocumentPart.Document.Body.Append(CurrentTable.GetDeviceTable(null));
                        //Certificate.MainDocumentPart.Document.Body.Append(CalibrationLib.NewParagraph(0, 0, JustificationValues.Left, 10));
                }
            }
        }
        static void InsertLineForClient(this Table table, int Index, string Text)
        {
            try
            {
                TableRow newTableRow = CalibrationLib.NewTableRow();
                int CellWidth1 = 10065;
                TableCell TableCell1 = newTableRow.AddTableCell(CellWidth1, Text, -113, -113, JustificationValues.Left, 12, 12);
                TableCell1.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Single, BorderValues.Nil);
                List<OpenXmlElement> NewList = table.Elements().ToList();
                NewList.Insert(Index, newTableRow);
                table.RemoveAllChildren();
                foreach (OpenXmlElement Element in NewList)
                { table.Append(Element); }
            }
            catch { }
        }
        
    }

}
