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
    static class NoticeOfUnfitness
    {
        /// <summary>
        /// Проверка возможности создания "Извещения о непригодности"
        /// </summary>
        /// <param name="CardScript"> Скрипт карточки.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка паспорта прибора.</param>
        /// <param name="Client"> Наименование клиента.</param>
        /// <param name="VerificationDate"> Дата проведения поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата передачи прибора на поверку.</param>
        /// <param name="IsNewDevice"> Новый прибор.</param>
        /// <returns></returns>
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
                XtraMessageBox.Show("Не удалось сформировать 'Извещение о непригодности'. Обнаружены следующие ошибки:\n\n" + ErrorText + 
                    "\nОбратитесь к администратору системы.", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Создание "Извещения о неригодности"
        /// </summary>
        /// <param name="CardScript"> Скрипт карточки.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка паспорта прибора.</param>
        /// <param name="Client"> Наименование клиента.</param>
        /// <param name="VerificationDate"> Дата проведения поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата передачи прибора на поверку.</param>
        /// <param name="VerifySerialNumber"> Серийный номер поверки.</param>
        /// <param name="CausesOfUnfitness"> Причина непригодности.</param>
        /// <param name="IsNewDevice"> Новый прибор.</param>
        /// <returns></returns>
        public static CardData Create(ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string VerifySerialNumber, string CausesOfUnfitness, bool IsNewDevice = false)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;

            // Серия и номер знака предыдущей поверки
            string PreviousVerifySerialNumber = "отсутствует";
            // Получаем последнее свидетельство о поверке
            CardData PreviousVerificationCertificate = CalibrationLib.GetDocumentCard(CardScript.Session, DeviceCard, CalibrationLib.VerificationCertificateCategoryID);
            try
            {
                if (PreviousVerificationCertificate.Id != null)
                {

                    if (PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber) != null)
                    {
                        String OldSerialNumber = PreviousVerificationCertificate.GetDocumentProperty(CalibrationLib.DocumentProperties.VerifySerialNumber).ToString();
                        PreviousVerifySerialNumber = OldSerialNumber == "" ? PreviousVerifySerialNumber : OldSerialNumber;
                    }
                }
            }
            catch
            { PreviousVerifySerialNumber = "отсутствует"; }

            WordprocessingDocument Document = CalibrationLib.GetNoticeOfUnfitnessTemplate(CardScript.Session);
            // Заполнение данных Извещения
            FillData(Document, CardScript, Context, DeviceCard, ClientName, VerificationDate, AdditionalWaresList, StartDateOfService, PreviousVerifySerialNumber, CausesOfUnfitness);
            // Сохранение изменений
            Document.MainDocumentPart.Document.Save();
            // Закрытие Извещения
            Document.Close();
            // Создание карточки Извещения
            CardData NewFileCard = CalibrationLib.NewFileCard(Context, CardScript.Session, CalibrationLib.NoticeOfUnfitnessCategoryID, CalibrationLib.TempFolder + "\\" + "Извещение о непригодности.docx", VerificationDate,
                DeviceCard, ClientName, VerifySerialNumber, false, IsNewDevice);
            // Прикрепление карточки Извещения к карточке паспорта прибора
            CalibrationLib.AttachFileToCard(CardScript.Session, DeviceCard, NewFileCard);

            return NewFileCard;
        }
        /// <summary>
        /// Обновление "Извещения о непригодности".
        /// </summary>
        /// <param name="FileCard"> Карточка файла, содержащая "Извещение о непригодности".</param>
        /// <param name="CardScript"> Скрипт карточки.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка паспорта прибора.</param>
        /// <param name="Client"> Наименование клиента.</param>
        /// <param name="VerificationDate"> Дата проведения поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата передачи прибора на поверку.</param>
        /// <param name="CausesOfUnfitness"> Причина непригодности.</param>
        /// <returns></returns>
        public static void ReFill(CardData FileCard, ScriptClassBase CardScript, ObjectContext Context, CardData DeviceCard, Guid Client,
            DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string CausesOfUnfitness)
        {
            string ClientName = Client.Equals(Guid.Empty) ? "ООО \"СКБ ЭП\"" : Context.GetObject<PartnersCompany>(Client).Name;
            // Серия и номер знака предыдущей поверки
            string PreviousVerifySerialNumber = "отсутствует";
            // Получаем последнее свидетельство о поверке
            CardData PreviousVerificationCertificate = CalibrationLib.GetDocumentCard(CardScript.Session, DeviceCard, CalibrationLib.VerificationCertificateCategoryID);
            if (PreviousVerificationCertificate != null)
            {
                if (PreviousVerificationCertificate.Id == FileCard.Id)
                { PreviousVerifySerialNumber = "отсутствует"; }
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
                    { PreviousVerifySerialNumber = "отсутствует"; }
                }
            }

            WordprocessingDocument Document = CalibrationLib.GetNoticeOfUnfitnessTemplate(CardScript.Session);
            // Заполнение данных Извещения
            FillData(Document, CardScript, Context, DeviceCard, ClientName, VerificationDate, AdditionalWaresList, StartDateOfService, PreviousVerifySerialNumber, CausesOfUnfitness);
            // Сохранение изменений
            Document.MainDocumentPart.Document.Save();
            // Закрытие Извещения
            Document.Close();
            // Замена файла в карточке Извещения
            CalibrationLib.RefreshFileCard(Context, CardScript, FileCard, CalibrationLib.TempFolder + "\\" + "Извещение о непригодности.docx", VerificationDate, DeviceCard, ClientName, "отсутствует", false, 
                CalibrationLib.NoticeOfUnfitnessCategoryID);
            return;
        }
        /// <summary>
        /// Заполнение файла "Извещение о непригодности"
        /// </summary>
        /// <param name="Document"> Редактируемый документ.</param>
        /// <param name="CardScript"> Скрипт карточки.</param>
        /// <param name="Context"> Объектный контекст.</param>
        /// <param name="DeviceCard"> Карточка прибора.</param>
        /// <param name="ClientName"> Название клиента.</param>
        /// <param name="VerificationDate"> Дата поверки.</param>
        /// <param name="AdditionalWaresList"> Перечень дополнительных изделий.</param>
        /// <param name="StartDateOfService"> Дата поступления на поверку.</param>
        /// <param name="PreviousVerifySerialNumber"> Серийный номер предыдущей поверки.</param>
        /// <param name="CausesOfUnfitness"> Серийный номер предыдущей поверки.</param>
        /// <returns></returns>
        public static void FillData(WordprocessingDocument Document, ScriptClassBase CardScript, ObjectContext Context,
            CardData DeviceCard, string ClientName, DateTime VerificationDate, List<CardData> AdditionalWaresList, DateTime StartDateOfService, string PreviousVerifySerialNumber, string CausesOfUnfitness)
        {
            // Формирование Извещения о непригодности
            Guid DeviceTypeID = DeviceCard.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid();
            string DeviceTypeName = ApplicationCard.UniversalCard.GetItemName(DeviceTypeID);
            string DeviceNumber = DeviceCard.GetDeviceNumber();

            // Данные для заполнения закладочных полей
            string FullDeviceType = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Полное название").ToString() + " " + DeviceTypeName + ",";
            string VerificationDateString = VerificationDate.ToLongDateString();
            string VerificationMethods = ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Методика поверки").ToString();
            string RegNum = "ГР №" + ApplicationCard.UniversalCard.GetItemPropertyValue(DeviceTypeID, "Номер в Госреестре").ToString();
            string AdditionalWares = (AdditionalWaresList.Count > 0 ? "c датчиками " : "") + AdditionalWaresList.Select(r => ApplicationCard.UniversalCard.GetItemName(r.Sections[CardOrd.Properties.ID].FindRow("@Name = 'Прибор'").GetString(CardOrd.Properties.Value).ToGuid()) + " " + r.GetDeviceNumber()).Aggregate(AdditionalWaresList.Count == 2 ? " и " : ", ");

            // Заполнение закладочный полей
            FillBookmarks(Document, FullDeviceType, RegNum, AdditionalWares, PreviousVerifySerialNumber, DeviceNumber, VerificationMethods, VerificationDateString, CausesOfUnfitness);

            return;
        }
        /// <summary>
        /// Заполнение закладочных полей "Извещения о непригодности"
        /// </summary>
        /// <param name="Document"> Редактируемый документ.</param>
        /// <param name="DeviceType"> Тип прибора.</param>
        /// <param name="RegNum"> Регистрационный номер в Федеральном информационном фонде по ОЕИ.</param>
        /// <param name="AdditionalWares"> Перечень дополнительных изделий.</param>
        /// <param name="VerifySerialNumber"> Серийный номер предыдущей поверки.</param>
        /// <param name="DeviceNumber"> Заводской номер прибора.</param>
        /// <param name="VerificationMethods"> Методика поверки.</param>
        /// <param name="VerificationDate"> Дата поверки.</param>
        /// <param name="CausesOfUnfitness"> Причина непригодности.</param>
        static void FillBookmarks(WordprocessingDocument Document, string DeviceType, string RegNum, string AdditionalWares, string VerifySerialNumber, string DeviceNumber, 
            string VerificationMethods, string VerificationDate, string CausesOfUnfitness)
        {
            //CalibrationLib.ChangeCellsWidth(Certificate.GetTable("СЕРТИФИКАТ О КАЛИБРОВКЕ №"), 3, 0, DeviceType.Length);

            Dictionary<String, BookmarkStart> BookmarkDic = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart Bookmark in Document.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                BookmarkDic[Bookmark.Name] = Bookmark;

            /* Запись закладочных полей */
            BookmarkDic["DeviceType"].WriteText(DeviceType);
            BookmarkDic["RegNum"].WriteText(RegNum);
            BookmarkDic["AdditionalWares"].WriteText(AdditionalWares);
            BookmarkDic["VerifySerialNumber"].WriteText(VerifySerialNumber);
            BookmarkDic["DeviceNumber"].WriteText(DeviceNumber);
            BookmarkDic["VerificationMethods"].WriteText(VerificationMethods);
            BookmarkDic["VerifyDate"].WriteText(VerificationDate);
            BookmarkDic["CausesOfUnfitness"].WriteText(CausesOfUnfitness);
        }
    }
}
