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
    static class VerificationCertificate
    {
        
        public static bool Verify(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, bool IsNewDevice = false)
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

            // Наличие в справочнике номера в госреестре
            if (ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Номер в Госреестре") == null)
            { ErrorText = ErrorText + " - Не удалось определить номер в Госреестре для указанного прибора.\n"; }

            // Наличие в справочнике средств поверки
            //if (ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Средства калибровки") == null)
            if (CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, true, false) == "-")
            { ErrorText = ErrorText + " - Не удалось определить средства поверки для указанного прибора.\n"; }

            // Наличие в справочнике межповерочного интервала
            if (CalibrationLib.GetVerificationInterval(ApplicationCard.UniversalCard, DeviceTypeID) == 0)
            { ErrorText = ErrorText + " - Не удалось определить межповерочный интервал для указанного прибора.\n"; }

            // Наличие сведений об условиях калибровки на указанную дату калибровки
            ErrorText = ErrorText + CalibrationLib.CheckCalibrationJournal(CardScript, Context, VerificationDate);


            //** Проверка родительских документов **//

            TablesCreator Creator = new TablesCreator();
            string DeviceErrorText = "";

            // Проверка протокола поверки прибора
            //if (Creator.CertificateDeviceTablesCollection.Any(r => r.DeviceTypes.Any(s => s == DeviceTypeName) && r.ParentDocumentCategory == CertificateTableLook.DocumentCategory.VerificationProtocol))
            //{
                DeviceErrorText = CalibrationLib.CheckDocument(CardScript.Session, DeviceCard, StartDateOfService, CalibrationLib.VerificationProtocolCategoryID, IsNewDevice);
                if (DeviceErrorText != "")
                { ErrorText = ErrorText + " - Не удалось найти данные о поверке для " + DeviceTypeName + " " + DeviceNumber + ":\n" + DeviceErrorText; }
            //}

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
                XtraMessageBox.Show("Не удалось сформировать 'Свидетельство о поверке'. Обнаружены следующие ошибки:\n\n" + ErrorText + 
                    "\nОбратитесь к администратору системы.", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Создать свидетельство о поверке.
        /// </summary>
        /// <param name="CardScript"> Скрипт.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка прибора.</param>
        /// <param name="Client"> Идентификатор клиента.</param>
        /// <param name="CalibrationDate"> Дата проведения поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата передачи на поверку.</param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки.</param>
        /// <param name="NewDevice"> Метка нового прибора.</param>
        /// <returns></returns>
        public static CardData Create(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string VerifySerialNumber, bool NewDevice = false)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;

            string PreviousVerifySerialNumber = "-";
            // Получаем последнее свидетельство о поверке
            CardData PreviousVerificationCertificate = CalibrationLib.GetDocumentCard(CardScript.Session, DeviceCard, CalibrationLib.VerificationCertificateCategoryID);
            if (PreviousVerificationCertificate != null)
            {
                try
                {
                    if (PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber) != null)
                    {
                        String OldSerialNumber = PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber).ToString();
                        PreviousVerifySerialNumber = OldSerialNumber == "" ? PreviousVerifySerialNumber : OldSerialNumber;
                    }
                }
                catch
                { PreviousVerifySerialNumber = "-"; }
            }

            WordprocessingDocument Certificate = CalibrationLib.GetVerificationCertificateTemplate(CardScript.Session);
            // Заполнение данных сертификата
            FillData(Certificate, CardScript, Context, DeviceCard, ClientName, CalibrationDate, AdditionalWaresList, StartDateOfService, PreviousVerifySerialNumber);
            // Сохранение изменений
            Certificate.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Certificate.Close();
            // Создание карточки сертификата
            CardData NewFileCard = CalibrationLib.NewFileCard(Context, CardScript.Session, CalibrationLib.VerificationCertificateCategoryID, CalibrationLib.TempFolder + "\\" + "Свидетельство о поверке.docx",
                CalibrationDate, DeviceCard, ClientName, VerifySerialNumber, false, NewDevice);
            // Прикрепление карточки сертификата к карточке паспорта прибора
            CalibrationLib.AttachFileToCard(CardScript.Session, DeviceCard, NewFileCard);
            
            return NewFileCard;
        }
        /// <summary>
        /// Переформировать свидетельство о поверке.
        /// </summary>
        /// <param name="FileCard"> Карточка файла.</param>
        /// <param name="CardScript"> Скрипт.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка прибора.</param>
        /// <param name="Client"> Идентификатор клиента.</param>
        /// <param name="CalibrationDate"> Дата проведения поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата передачи на поверку.</param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки.</param>
        public static void ReFill(CardData FileCard, ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime CalibrationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string VerifySerialNumber)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;

            string PreviousVerifySerialNumber = "-";
            // Получаем последнее свидетельство о поверке
            CardData PreviousVerificationCertificate = CalibrationLib.GetDocumentCard(CardScript.Session, DeviceCard, CalibrationLib.VerificationCertificateCategoryID);
            if (PreviousVerificationCertificate != null)
            {
                if (PreviousVerificationCertificate.Id == FileCard.Id)
                { PreviousVerifySerialNumber = "-"; }
                else
                {
                    try
                    {
                        if (PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber) != null)
                        {
                            String OldSerialNumber = PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber).ToString();
                            PreviousVerifySerialNumber = OldSerialNumber == "" ? PreviousVerifySerialNumber : OldSerialNumber;
                        }
                    }
                    catch
                    { PreviousVerifySerialNumber = "-"; }
                }
            }

            WordprocessingDocument Certificate = CalibrationLib.GetVerificationCertificateTemplate(CardScript.Session);
            // Заполнение данных сертификата
            FillData(Certificate, CardScript, Context, DeviceCard, ClientName, CalibrationDate, AdditionalWaresList, StartDateOfService, PreviousVerifySerialNumber);
            // Сохранение изменений
            Certificate.MainDocumentPart.Document.Save();
            // Закрытие сертификата
            Certificate.Close();
            // Замена файла в карточке сертификата
            CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "\\" + "Свидетельство о поверке.docx", CalibrationDate, DeviceCard, ClientName, VerifySerialNumber, 
                false, CalibrationLib.VerificationCertificateCategoryID);
            return;
        }
        /// <summary>
        /// Заполнить данные свидетельства о поверке.
        /// </summary>
        /// <param name="Certificate"> Файл свидетельства о поверке.</param>
        /// <param name="CardScript">Скрипт.</param>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="DeviceCard">Карточка прибора.</param>
        /// <param name="ClientName">Название клиента.</param>
        /// <param name="VerificationDate">Дата поверки.</param>
        /// <param name="AdditionalWaresList">Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService">Дата поступления на поверку.</param>
        /// <param name="PreviousVerifySerialNumber">Дата поступления на поверку.</param>
        public static void FillData(WordprocessingDocument Certificate, ScriptClassBase CardScript, ObjectContext Context,
            CardData DeviceCard, string ClientName, DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string PreviousVerifySerialNumber)
        {
            // Формирование сертификата
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();
            DeviceNumber = DeviceNumber.Contains("/") == true ? DeviceNumber.Replace("/", " (") + " г)" : DeviceNumber;

            // Данные для заполнения закладочных полей
            string FullDeviceType = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название").ToString() + " " + DeviceTypeName + ",";
            string VerificationDateString = VerificationDate.ToLongDateString();
            int VerificationInterval = CalibrationLib.GetVerificationInterval(ApplicationCard.UniversalCard, DeviceTypeID);
            string ValidUntil =  VerificationDate.AddMonths(VerificationInterval).AddDays(-1).ToLongDateString();
            string VerificationMethods = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Методика поверки").ToString();
            string VerificationScope = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Объем поверки") == null ? "в полном объеме" : ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Объем поверки").ToString();
            string Suitability = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Пригодность к применению") == null ? "" : " " + ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Пригодность к применению").ToString();
            string RegNum = "ГР №" + ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Номер в Госреестре").ToString();
            string AdditionalWares = (AdditionalWaresList.Count > 0 ? "c датчиками " : "-") + AdditionalWaresList.Select(r => 
            ApplicationCard.UniversalCard.GetItemName(r.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid()) + " № " + (r.GetDeviceNumber().Contains("/") == true ? r.GetDeviceNumber().Replace("/", " (") + " г)" : r.GetDeviceNumber())).Aggregate(AdditionalWaresList.Count == 2 ? " и " : ", ");
            string VerificationMeans = CalibrationLib.GetListOfMeasures(CardScript.Session, DeviceTypeID, false, true);
            // ФИО текущего сотрудника определяется из справочника сотрудников (строка отображения)
            string StaffName = Context.GetCurrentEmployee().FirstName.Substring(0,1) + "." + Context.GetCurrentEmployee().MiddleName.Substring(0, 1) + ". " + Context.GetCurrentEmployee().LastName;
            // ФИО руководителя отдела Метрологической лаборатории определяется из справочника сотрудников по должности
            StaffEmployee MetrologicalLabManagerEmployee = Context.GetEmployeeByPosition(SKB.Base.Ref.RefServiceCard.Roles.MetrologicalLabManager);
            string ManagerName = MetrologicalLabManagerEmployee != null ? MetrologicalLabManagerEmployee.FirstName.Substring(0,1) + "." + MetrologicalLabManagerEmployee.MiddleName.Substring(0,1) + ". " + MetrologicalLabManagerEmployee.LastName : "";

            // Условия поверки
            string TempPath = "";
            WordprocessingDocument ParentDocument = CalibrationLib.GetDocument(CardScript.Session, DeviceCard, CalibrationLib.VerificationProtocolCategoryID, out TempPath);
            Table ParentTable = ParentDocument == null ? null : ParentDocument.GetTable("при следующих значениях влияющих факторов:");

            List<string> Collection = new List<string>();
            if (ParentTable != null)
            {
                for (int i = 1; i < ParentTable.Elements<TableRow>().Count(); i++)
                { Collection.Add(ParentTable.GetCellValue(i, 1) + ": " + ParentTable.GetCellValue(i, 2) + " " + ParentTable.GetCellValue(i, 3) + " "); }
            }
            string Factors = Collection.Count() > 0 ? Collection.Aggregate() : "";
            string Factors2 = "";
            if (Factors.Length > 27)
            {
                int i = Factors.Substring(0, 27).LastIndexOf(" ");
                if (i > 0)
                {
                    Factors2 = Factors.Substring(i);
                    Factors = Factors.Substring(0, i);
                }
            }

            // Заполнение закладочный полей
            FillBookmarks(Certificate, ValidUntil, FullDeviceType, RegNum, AdditionalWares, PreviousVerifySerialNumber, DeviceNumber, VerificationMethods, VerificationScope, Suitability, VerificationMeans, Factors, Factors2, VerificationDateString, StaffName, ManagerName);

            // Добавление дополнительных данных
            return;
        }
        /// <summary>
        /// Заполнение закладочных полей свидетельства о поверке.
        /// </summary>
        /// <param name="Certificate">Документ свидетельства о поверке.</param>
        /// <param name="ValidUntil">Действителен до.</param>
        /// <param name="DeviceType">Тип прибора.</param>
        /// <param name="RegNum">Номер регистрации в гос. реестре.</param>
        /// <param name="AdditionalWares">Перечень доп. изделий.</param>
        /// <param name="VerifySerialNumber">Серийный номер поверки.</param>
        /// <param name="DeviceNumber">Заводской номер прибора.</param>
        /// <param name="VerificationMethods">Методика поверки.</param>
        /// <param name="VerificationScope">Объем проводимой поверки.</param>
        /// <param name="Suitability">Пригодность к применению.</param>
        /// <param name="VerificationMeans">Средства поверки.</param>
        /// <param name="Factors">Значимые условия окружающей среды (часть 1).</param>
        /// <param name="Factors2">Значимые условия окружающей среды (часть 2).</param>
        /// <param name="VerificationDate">Дата проведения поверки.</param>
        /// <param name="StaffName">Поверитель.</param>
        /// <param name="ManagerName">Начальник метрологической лаборатории.</param>
        static void FillBookmarks(WordprocessingDocument Certificate, string ValidUntil, string DeviceType, string RegNum, string AdditionalWares, string VerifySerialNumber, string DeviceNumber, 
            string VerificationMethods, string VerificationScope, string Suitability, string VerificationMeans, string Factors, string Factors2, string VerificationDate, string StaffName, string ManagerName)
        {

            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Certificate.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            /* Запись закладочных полей */
            if (BookmarkDic.Keys.Any(r => r == "ValidUntil")) BookmarkDic["ValidUntil"].WriteText(ValidUntil);
            if (BookmarkDic.Keys.Any(r => r == "DeviceType")) BookmarkDic["DeviceType"].WriteText(DeviceType);
            if (BookmarkDic.Keys.Any(r => r == "RegNum")) BookmarkDic["RegNum"].WriteText(RegNum);
            if (BookmarkDic.Keys.Any(r => r == "AdditionalWares")) BookmarkDic["AdditionalWares"].WriteText(AdditionalWares);
            if (BookmarkDic.Keys.Any(r => r == "VerifySerialNumber")) BookmarkDic["VerifySerialNumber"].WriteText(VerifySerialNumber);
            if (BookmarkDic.Keys.Any(r => r == "DeviceNumber")) BookmarkDic["DeviceNumber"].WriteText(DeviceNumber);
            if (BookmarkDic.Keys.Any(r => r == "VerificationMethods")) BookmarkDic["VerificationMethods"].WriteText(VerificationMethods);
            if (BookmarkDic.Keys.Any(r => r == "VerificationScope")) BookmarkDic["VerificationScope"].WriteText(VerificationScope);
            if (BookmarkDic.Keys.Any(r => r == "Suitability")) BookmarkDic["Suitability"].WriteText(Suitability);
            if (BookmarkDic.Keys.Any(r => r == "VerificationMeans")) BookmarkDic["VerificationMeans"].WriteText(VerificationMeans);
            if (BookmarkDic.Keys.Any(r => r == "Factors")) BookmarkDic["Factors"].WriteText(Factors);
            if (BookmarkDic.Keys.Any(r => r == "Factors2")) BookmarkDic["Factors2"].WriteText(Factors2);
            if (BookmarkDic.Keys.Any(r => r == "VerifyDate")) BookmarkDic["VerifyDate"].WriteText(VerificationDate);
            if (BookmarkDic.Keys.Any(r => r == "VerifyDate2")) BookmarkDic["VerifyDate2"].WriteText(VerificationDate);
            if (BookmarkDic.Keys.Any(r => r == "StaffName")) BookmarkDic["StaffName"].WriteText(StaffName);
            if (BookmarkDic.Keys.Any(r => r == "StaffName2")) BookmarkDic["StaffName2"].WriteText(StaffName);
            if (BookmarkDic.Keys.Any(r => r == "ManagerName")) BookmarkDic["ManagerName"].WriteText(ManagerName);
        }
    }
}
