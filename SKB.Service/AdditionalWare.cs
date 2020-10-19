using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsVision.Platform.ObjectManager;
using DocsVision.BackOffice.WinForms;

namespace SKB.Service
{
    /// <summary>
    /// Дополнительное изделие
    /// </summary>
    class AdditionalWare
    {
        /// <summary>
        /// Скрипт картчоки.
        /// </summary>
        ScriptClassBase CardScript;
        /// <summary>
        /// Название изделия.
        /// </summary>
        public string WareName = "";
        /// <summary>
        /// ID карточки паспорта изделия.
        /// </summary>
        public string WarePassportID = "";
        /// <summary>
        /// ID записи изделия в справочнике готовых приборов.
        /// </summary>
        public string WareItemID = "";
        /// <summary>
        /// ID записи типа изделия в справочнике "Приборы и комплектующие".
        /// </summary>
        public string TypeID = "";
        /// <summary>
        /// Название типа изделия.
        /// </summary>
        public string TypeName = "";
        /// <summary>
        /// Дополнительное изделие.
        /// </summary>
        public AdditionalWare(ScriptClassBase CardScript, string WareName, string WarePassportID, string WareItemID, string TypeID, string TypeName)
        {
            this.CardScript     = CardScript;
            this.WareName       = WareName;
            this.WarePassportID = WarePassportID;
            this.WareItemID     = WareItemID;
            this.TypeID         = TypeID;
            this.TypeName       = TypeName;
        }
        /// <summary>
        /// Определение возраста изделия (количество лет с момента продажи/выпуска).
        /// </summary>
        public int GetAge()
        {
            int Age = 0;
            CardData CurrentPassport = CardScript.Session.CardManager.GetCardData(new Guid(this.WarePassportID));
            RowData LoadDate = CurrentPassport.Sections[CurrentPassport.Type.AllSections["Properties"].Id].FindRow("@Name = 'Дата отправки'");
            if (LoadDate.GetDateTime("Value") != null)
            {
                TimeSpan T = DateTime.Today - ((DateTime)LoadDate.GetDateTime("Value"));
                Age = (int)(T.Days / 365);
            }
            else
            {
                RowData DeviceYear = CurrentPassport.Sections[CurrentPassport.Type.AllSections["Properties"].Id].FindRow("@Name = '/Год прибора'");
                Age = DateTime.Today.Year - (int)DeviceYear.GetInt32("Value");
            }
            return Age;
        }
    }
}
