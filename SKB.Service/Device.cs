using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.Metadata;
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.TakeOffice.Cards.Constants;
using DocsVision.Platform.ObjectManager.SearchModel;
using SKB.Service.Cards;
using SKB.Base.Ref;
using SKB.Base;
using SKB.PaymentAndShipment;


namespace SKB.Service
{
    /// <summary>
    /// Прибор.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Скрипт карточки.
        /// </summary>
        MyBaseCard Card;
        /// <summary>
        /// Полный заводской номер прибора (в формате "МИКО-1 № 001С/2014 из партии МИКО-1 - 100 - 5/2014").
        /// </summary>
        public string DeviceNumber       = "";
        /// <summary>
        /// Краткое наименование прибора (в формате "МИКО-1 № 001С/2014").
        /// </summary>
        public string DeviceName         = "";
        /// <summary>
        /// Краткий номер прибора (в формате "001С/2014").
        /// </summary>
        public string ShortDeviceNumber  = "";
        /// <summary>
        /// ID карточки паспорта прибора.
        /// </summary>
        public string DevicePassportID     = "";
        /// <summary>
        /// ID записи прибора в справочнике готовых приборов.
        /// </summary>
        public string DeviceItemID = "";
        /// <summary>
        /// Наименование типа прибора.
        /// </summary>
        public string DeviceType         = "";
        /// <summary>
        /// ID записи типа прибора в справочнике "Приборы и комплектующие".
        /// </summary>
        public string DeviceTypeID       = "";
        /// <summary>
        /// Только комплектующие (без прибора).
        /// </summary>
        public bool   OnlyAccessories    = false;
        /// <summary>
        /// Требуется ремонт.
        /// </summary>
        public bool   Repair             = false;
        /// <summary>
        /// Требуется калибровка.
        /// </summary>
        public bool   Calibrate          = false;
        /// <summary>
        /// Требуется поверка.
        /// </summary>
        public bool   Verify             = false;
        /// <summary>
        /// Гарантия действует.
        /// </summary>
        public bool   WarrantyRepair     = false;
        /// <summary>
        /// Гарантия аннулирована.
        /// </summary>
        public bool VoidWarranty         = false;
        /// <summary>
        /// Удвоена стоимость ремонта.
        /// </summary>
        public bool DoubleCost           = false;
        /// <summary>
        /// Описание причины аннулирования гарантии/удвоения стоимости ремонта.
        /// </summary>
        public string DescriptionOfReason = "";
        /// <summary>
        /// Требуется помывка.
        /// </summary>
        public bool   Wash               = false;
        /// <summary>
        /// Комплектующие.
        /// </summary>
        public string Accessories        = "";
        /// <summary>
        /// Комментарии.
        /// </summary>
        public string Comments           = "";
        /// <summary>
        /// Перечень необходимых услуг.
        /// </summary>
        public string Services           = "";
        /// <summary>
        /// Данные упаковочного листа.
        /// </summary>
        public string PackedListData         = "";
        /// <summary>
        /// Документ упаковочного листа.
        /// </summary>
        public string PackedListID = "";
        /// <summary>
        /// ID карточки Наряда на сервисное обслуживание прибора.
        /// </summary>
        public string ServiceCardID      = "";
        /// <summary>
        /// Название карточки Наряда на сервисное обслуживание прибора.
        /// </summary>
        public string ServiceCardName    = "";
        /// <summary>
        /// Перечень дополнительных изделий в текстовом формате.
        /// </summary>
        public string Sensors            = "";
        /// <summary>
        /// Перечень дополнительных изделий.
        /// </summary>
        public ArrayList AdditionalWaresList = new ArrayList();
        /// <summary>
        /// Отказ от ремонта.
        /// </summary>
        public bool RefusalToRepair = false;
        /// <summary>
        /// Необходимые доработки.
        /// </summary>
        public string Improvements = "";
        /// <summary>
        /// Специальные условия по сервисному обслуживанию и гарантийным обязательствам.
        /// </summary>
        public string SpecialConditions = "";
        /// <summary>
        /// Новый прибор.
        /// </summary>
        public Device(MyBaseCard Card, string DNID, string DTID, bool OA, bool R, bool C, bool V, bool WR, string A, string Com, string PL, bool W, string S, string WO)
        {
            this.Card = Card;
            DeviceItemID = DNID;

            if (OA == false)
            {
                DevicePassportID  = DNID == "" ? "" : ApplicationCard.UniversalCard.GetItemPropertyValue(DNID.ToGuid(), "Паспорт прибора").ToString();
                DeviceNumber      = DNID == "" ? "" : ApplicationCard.UniversalCard.GetItemName(DNID.ToGuid());
                ShortDeviceNumber = DNID == "" ? "" : ApplicationCard.UniversalCard.GetItemPropertyValue(DNID.ToGuid(), "Номер прибора").ToString();
                string Year       = DNID == "" ? "" : ApplicationCard.UniversalCard.GetItemPropertyValue(DNID.ToGuid(), "Год прибора").ToString();
                ShortDeviceNumber = ShortDeviceNumber.Length == 4 ? ShortDeviceNumber : ShortDeviceNumber + "/" + Year;
                RowData SpecConditions = Card.CardScript.Session.CardManager.GetCardData(new Guid(DevicePassportID)).Sections[CardOrd.Properties.ID].FindRow("@Name = 'Особые условия по СО и ГО'");
                SpecialConditions = SpecConditions.GetString("Value") != null ? SpecConditions.GetString("Value") : "";
            }
            else
            {
                DevicePassportID  = "";
                DeviceNumber      = "";
            }

            DeviceTypeID    = DTID == "" ? "" : DTID;
            DeviceType      = DTID == "" ? "" : ApplicationCard.UniversalCard.GetItemName(DTID.ToGuid());
            OnlyAccessories = OA;
            Repair          = R;
            Calibrate       = C;
            Verify          = V;
            WarrantyRepair  = WR;
            Wash            = W;
            Accessories     = A == "" ? "" : A;
            Comments        = Com == "" ? "" : Com;
            Services        = FormattingService(this.Repair, this.Calibrate, this.Verify);
            PackedListData  = PL == "" ? "" : PL;
            PackedListID    = "";
            Sensors         = S == "" ? "" : S;
            ServiceCardID   = WO;
            DeviceName = this.DeviceNumber == "" ? "Комплектующие для " + this.DeviceType : this.DeviceType + " №" + this.ShortDeviceNumber;

            if (Sensors != "")
            {
                List<String> SelectedSensors = Sensors.Split(';').Where(s => !String.IsNullOrEmpty(s.Trim())).ToList();
                for (Int32 j = 0; j < SelectedSensors.Count; j++)
                {
                    String[] Sensor = SelectedSensors[j].Trim().Split(' ');
                    if (Sensor.Length >= 2)
                    {
                        RowData CurrentDeviceRow = GetDeviceRow(Sensor[1], Sensor[0]);
                        string CurrentPassportId = CurrentDeviceRow.ChildSections[0].FindRow("@Name = 'Паспорт прибора'").GetString("Value");
                        string CurrentDeviceTypeID = CurrentDeviceRow.ChildSections[0].FindRow("@Name = 'Наименование прибора'").GetString("Value");
                        string CurrentDeviceTypeName = ApplicationCard.UniversalCard.GetItemName(CurrentDeviceTypeID);
                        AdditionalWare NewWare = new AdditionalWare(Card.CardScript, SelectedSensors[j], CurrentPassportId, CurrentDeviceRow.Id.ToString(), CurrentDeviceTypeID, CurrentDeviceTypeName);
                        AdditionalWaresList.Add(NewWare);
                    }
                }
            }

            if(ServiceCardID != "")
            {
                CardData ServiceCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(ServiceCardID));
                RowData Adjustment = ServiceCard.Sections[RefServiceCard.Adjustment.ID].FirstRow;
                RowData MainInfo = ServiceCard.Sections[RefServiceCard.MainInfo.ID].FirstRow;
                this.VoidWarranty = Adjustment.GetBoolean(RefServiceCard.Adjustment.VoidWarranty) == null ? false : (bool)Adjustment.GetBoolean(RefServiceCard.Adjustment.VoidWarranty);
                this.DoubleCost = Adjustment.GetBoolean(RefServiceCard.Adjustment.DoubleCost) == null ? false : (bool)Adjustment.GetBoolean(RefServiceCard.Adjustment.DoubleCost);
                this.DescriptionOfReason = Adjustment.GetString(RefServiceCard.Adjustment.DescriptionOfReason) == null ? "" : Adjustment.GetString(RefServiceCard.Adjustment.DescriptionOfReason).ToString();
                this.RefusalToRepair = (int)MainInfo.GetInt32(RefServiceCard.MainInfo.Status) == (int)RefServiceCard.MainInfo.State.Failure ? true : false;
            }
        }
        /// <summary>
        /// Формирование перечня требуемых услуг.
        /// </summary>
        /// <param name="Repair">Требуется ремонт.</param>
        /// <param name="Calibrate">Требуется калибровка.</param>
        /// <param name="Verify">Требуется поверка.</param>
        internal static string FormattingService(bool Repair, bool Calibrate, bool Verify)
        {
            StringCollection Services = new StringCollection();
            if (Repair == true) { Services.Add("ремонт"); }
            if (Calibrate == true) { Services.Add("калибровка"); }
            if (Verify == true) { Services.Add("поверка"); }

            string[] Serv = new string[Services.Count];
            Services.CopyTo(Serv, 0);
            return string.Join(", ", Serv);
        }
        /// <summary>
        /// получает запись доп. изделия в универсальном справочнике.
        /// </summary>
        /// <param name="GotNumber">Заводской номер.</param>
        /// <param name="Device">Наименование изделия.</param>
        public RowData GetDeviceRow(String GotNumber, String Device)
        {
            String Number = null, Year = null;
            if (GotNumber.Contains("/"))
            {
                Year = GotNumber.Split('/')[1];
                Number = GotNumber.Split('/')[0];
            }
            else
                Number = GotNumber;

            SectionData Items = ApplicationCard.UniversalCard.Sections[RefUniversal.Item.ID];

            SectionQuery Query = Card.CardScript.Session.CreateSectionQuery();
            Query.ConditionGroup.Operation = ConditionGroupOperation.And;
            Query.ConditionGroup.Conditions.AddNew("Name", FieldType.Unistring, ConditionOperation.Contains, String.Format("{0} № {1}/{2}", Device, Number, Year));

            RowDataCollection Found = Items.FindRows(Query.GetXml());
            return Found.Count == 1 ? Found[0] : null;
        }
        /// <summary>
        /// Получение перечня ремонтных работ.
        /// </summary>
        public ArrayList GetRepairWorks()
        {
            ArrayList RepairWorksList = new ArrayList();

            // Если прибор гарантийный и гарантия не аннулирована, не включаем ремонтные работы в калькуляцию
            if ((this.WarrantyRepair) && (!this.VoidWarranty))
            {return RepairWorksList;}

            // Если клиент отказался от ремонта, то не включаем ремонтные работы в калькуляцию
            //if (this.RefusalToRepair)
            //{ return RepairWorksList; }

            CardData SCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(this.ServiceCardID));
            SectionData RepairWorks = SCard.Sections[RefServiceCard.RepairWorks.ID.ToGuid()];

            if ((RepairWorks != null) && (RepairWorks.Rows.Count > 0))
            {
                RowDataCollection RowDescriptions = SCard.Sections[RefServiceCard.DescriptionOfFault.ID.ToGuid()].Rows;
                foreach (RowData Row in RepairWorks.Rows)
                {
                    if (Row.GetString(RefServiceCard.RepairWorks.NegotiationResult) != "Не выполнять")
                    {
                        RowData Description = null;
                        foreach (RowData RowDescription in RowDescriptions)
                        {
                            if (RowDescription.GetGuid(RefServiceCard.DescriptionOfFault.Id) == Row.GetGuid(RefServiceCard.RepairWorks.ParentTableRowId))
                            { Description = RowDescription; }
                        }

                        string BlockName = Description.GetString(RefServiceCard.DescriptionOfFault.BlockOfDevice);
                        string BlockNameID = Description.GetString(RefServiceCard.DescriptionOfFault.BlockOfDeviceID);
                        string WorksType = Row.GetString(RefServiceCard.RepairWorks.WorksType);
                        string WorksTypeID = Row.GetString(RefServiceCard.RepairWorks.WorksTypeID);
                        int Amount = this.DoubleCost == true ? (int)Row.GetDecimal(RefServiceCard.RepairWorks.Amount) * 2 : (int)Row.GetDecimal(RefServiceCard.RepairWorks.Amount);
                        bool Improvement = (bool)Row.GetBoolean(RefServiceCard.RepairWorks.Revision);
                        bool VoidWarranty = this.VoidWarranty;
                        bool DoubleCost = this.DoubleCost;
                        string DescriptionOfReason = this.DescriptionOfReason;
                        bool RefusalToRepair = this.RefusalToRepair;


                        CalculationItem NewItem = new CalculationItem(Card, DeviceName, DeviceItemID, BlockName, BlockNameID, WorksType, WorksTypeID, Improvement, Amount, true, VoidWarranty, DoubleCost,
                            DescriptionOfReason, RefusalToRepair);
                        RepairWorksList.Add(NewItem);
                    }
                }
            }
            return RepairWorksList;
        }
        /// <summary>
        /// Получение перечня калибровочных работ.
        /// </summary>
        public ArrayList GetCalibrateWorks()
        {
            ArrayList CalibrateWorksList = new ArrayList();

            // Если прибор гарантийный и гарантия не аннулирована, не включаем калибровку в калькуляцию
            if ((this.WarrantyRepair) && (!this.VoidWarranty))
            { return CalibrateWorksList; }

            /* Калибровка прибора */
            string WorkName = "";
            if (this.OnlyAccessories)
            { WorkName = "Диагностика комплектующих"; }
            else
            {
                WorkName = "Калибровка " + this.DeviceType;
                if (this.GetAge() > 10)
                    WorkName = WorkName + " (старше 10 лет)";
            }

            string CalibWorkID = ServicesCard.GetItemID(ApplicationCard.UniversalCard, WorkName);
            string BlockName = "";
            string BlockNameID = "";
            string WorksType = WorkName;
            string WorksTypeID = CalibWorkID;
            int Amount = 1;
            bool Improvement = false;
            bool VoidWarranty = this.VoidWarranty;
            bool DoubleCost = false;
            string DescriptionOfReason = this.DescriptionOfReason;
            bool RefusalToRepair = this.RefusalToRepair;

            CalculationItem NewItem = new CalculationItem(Card, DeviceName, DevicePassportID, BlockName, BlockNameID, WorksType, WorksTypeID, Improvement, Amount, true, VoidWarranty, DoubleCost,
                        DescriptionOfReason, RefusalToRepair);
            CalibrateWorksList.Add(NewItem);

            /* Калибровочные работы для дополнительных изделий */
            foreach (AdditionalWare CurrentWare in AdditionalWaresList)
            {
                WorkName = "Калибровка " + CurrentWare.TypeName;
                if (CurrentWare.GetAge() > 10)
                    WorkName = WorkName + " (старше 10 лет)";
                CalibWorkID = ServicesCard.GetItemID(ApplicationCard.UniversalCard, WorkName);

                string WaresBlockName           = "";
                string WaresBlockNameID         = "";
                string WaresWorksType           = WorkName;
                string WaresWorksTypeID         = CalibWorkID;
                int    WaresWorkAmount          = 1;
                bool   WaresImprovement         = false;
                bool   WaresVoidWarranty        = this.VoidWarranty;
                bool   WaresDoubleCost          = false;
                string WaresDescriptionOfReason = this.DescriptionOfReason;
                bool   WaresRefusalToRepair     = this.RefusalToRepair;

                CalculationItem NewWaresItem = new CalculationItem(Card, DeviceName, DevicePassportID, WaresBlockName, WaresBlockNameID, WaresWorksType, WaresWorksTypeID, WaresImprovement, WaresWorkAmount,
                    true, WaresVoidWarranty, WaresDoubleCost, WaresDescriptionOfReason, WaresRefusalToRepair);
                CalibrateWorksList.Add(NewWaresItem);
            }
            return CalibrateWorksList;
        }
        /// <summary>
        /// Получение перечня помывочных работ.
        /// </summary>
        public ArrayList GetWashWorks()
        {
            ArrayList WashWorksList = new ArrayList();

            // Если прибор гарантийный и гарантия не аннулирована, не включаем стоимость помывки в калькуляцию
            if ((this.WarrantyRepair) && (!this.VoidWarranty))
            { return WashWorksList; }

            if (Wash)
            {
                string BlockName   = "";
                string BlockNameID = "";
                string WorksType   = "Помывка " + this.DeviceType;
                string WorksTypeID = ServicesCard.GetItemID(ApplicationCard.UniversalCard, WorksType);
                int Amount         = 1;
                bool Improvement = false;
                bool VoidWarranty = this.VoidWarranty;
                bool DoubleCost = false;
                string DescriptionOfReason = this.DescriptionOfReason;
                bool RefusalToRepair = this.RefusalToRepair;

                CalculationItem NewItem = new CalculationItem(Card, DeviceName, DeviceItemID, BlockName, BlockNameID, WorksType, WorksTypeID, Improvement, Amount, true, VoidWarranty, DoubleCost,
                        DescriptionOfReason, RefusalToRepair);
                WashWorksList.Add(NewItem);
            }

            return WashWorksList;
        }
        /// <summary>
        /// Получение перечня упаковочных работ.
        /// </summary>
        public ArrayList GetPackingWorks()
        {
            ArrayList PackingWorksList = new ArrayList();

            // Если прибор гарантийный и гарантия не аннулирована, не включаем стоимость упаковки в калькуляцию
            if ((this.WarrantyRepair) && (!this.VoidWarranty))
            { return PackingWorksList; }

            string BlockName = "";
            string BlockNameID = "";
            string WorksType = "Упаковка и накладные расходы";
            string WorksTypeID = ServicesCard.GetItemID(ApplicationCard.UniversalCard, WorksType);
            int Amount = 1;
            bool Improvement = false;
            bool VoidWarranty = this.VoidWarranty;
            bool DoubleCost = false;
            string DescriptionOfReason = this.DescriptionOfReason;
            bool RefusalToRepair = this.RefusalToRepair;

            CalculationItem NewItem = new CalculationItem(Card, DeviceName, DeviceItemID, BlockName, BlockNameID, WorksType, WorksTypeID, Improvement, Amount, true, VoidWarranty, DoubleCost,
                        DescriptionOfReason, RefusalToRepair);
            PackingWorksList.Add(NewItem);

            return PackingWorksList;
        }
        /// <summary>
        /// Определение возраста прибора.
        /// </summary>
        public int GetAge()
        {
            int Age = 0;
            DateTime LoadDate = DateTime.MinValue;
            CardData CurrentPassport = Card.CardScript.Session.CardManager.GetCardData(new Guid(this.DevicePassportID));
            SectionData Links = CurrentPassport.Sections[CurrentPassport.Type.AllSections["CardReferences"].Id];
            foreach (RowData Link in Links.Rows)
            {
                if (Link.GetString("Description").IndexOf("Плановая дата отгрузки:") >= 0)
                {
                    CardData LinkCard = Card.CardScript.Session.CardManager.GetCardData(new Guid(Link.GetString("Link")));
                    SectionData Devices = LinkCard.Sections[SKB.Base.Ref.RefCompleteCard.Devices.ID];
                    foreach (RowData Row in Devices.Rows)
                    {
                        if ((Row.GetString(SKB.Base.Ref.RefCompleteCard.Devices.ContractSubjectId) == SKB.PaymentAndShipment.Cards.AccountCard.Item_Subject_Delivery) && (Row.GetString(SKB.Base.Ref.RefCompleteCard.Devices.DeviceNumberId) == this.DeviceItemID))
                        {
                            LoadDate = (DateTime)Row.GetDateTime(SKB.Base.Ref.RefCompleteCard.Devices.FactShipDate);
                        }
                    }
                }
            }

            if (LoadDate == DateTime.MinValue)
            {
                RowData DeviceYear = CurrentPassport.Sections[CurrentPassport.Type.AllSections["Properties"].Id].FindRow("@Name = '/Год прибора'");
                Age = DateTime.Today.Year - (int)DeviceYear.GetInt32("Value");
            }
            else
            {
                TimeSpan T = DateTime.Today - LoadDate;
                Age = (int)(T.Days / 365);
            }

            /*if (LoadDate.GetDateTime("Value") != null)
            {
                TimeSpan T = DateTime.Today - ((DateTime)LoadDate.GetDateTime("Value"));
                Age = (int)(T.Days / 365);
            }
            else
            {
                RowData DeviceYear = CurrentPassport.Sections[CurrentPassport.Type.AllSections["Properties"].Id].FindRow("@Name = '/Год прибора'");
                Age = DateTime.Today.Year - (int)DeviceYear.GetInt32("Value");
            }*/
            return Age;
        }
    }
}
