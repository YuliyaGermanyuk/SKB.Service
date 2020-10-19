using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SKB.Service.Forms.CertificateCreationCard
{
    /// <summary>
    /// Строка таблицы редактирования датчиков формы редактирования "Приборы".
    /// </summary>
    public class AdditionalWaresRow : IDataErrorInfo
    {
        private String Error;
        /// <summary>
        /// Датчик корректен.
        /// </summary>
        public Boolean Valid;
        /// <summary>
        /// Тект всплывающей подсказки.
        /// </summary>
        public String ToolTip;
        /// <summary>
        /// Идентификатор старого датчика.
        /// </summary>
        public String _WareID;
        /// <summary>
        /// Старый датчик.
        /// </summary>
        public String OldWareName;
        /// <summary>
        /// Идентификатор нового датчика.
        /// </summary>
        public String WareID { get; set; }
        /// <summary>
        /// Новый датчик.
        /// </summary>
        public String WareName { get; set; }
        /// <summary>
        /// Протокол калибровки.
        /// </summary>
        public String ProtocolName { get; set; }
        /// <summary>
        /// Идентификатор протокола калибровки.
        /// </summary>
        public String ProtocolID { get; set; }
        /// <summary>
        /// Сертификат о калибровке.
        /// </summary>
        public String CertificateName { get; set; }
        /// <summary>
        /// Идентификатор сертификата о калибровке.
        /// </summary>
        public String CertificateID { get; set; }
        /// <summary>
        /// Инициализирует пустую строку изделия.
        /// </summary>
        public AdditionalWaresRow()
        {
            this.Valid = false;
            this.ToolTip = "Не введено значение!";
        }
        /// <summary>
        /// Инициализирует строку изделия.
        /// </summary>
        /// <param name="WareName"> Заводской номер изделия. </param>
        /// <param name="WareID"> Идентификатор заводского номера изделия. </param>
        /// <param name="ToolTip"> Комментарий. </param>
        /// <param name="ProtocolID"> Идентификатор картоки протокола калибровки для изделия. </param>
        /// <param name="ProtocolName"> Название карточки протокола калибровки для изделия. </param>
        /// <param name="CertificateID"> Идентификатор карточки сертификата о калибровке для изделия. </param>
        /// <param name="CertificateName"> Название карточки сертификата о калибровке для изделия. </param>
        public AdditionalWaresRow(String WareName, String WareID, String ToolTip, String ProtocolID, String ProtocolName, String CertificateID, String CertificateName)
        {
            this.Valid = true;
            this.WareName = WareName;
            this.OldWareName = WareName;
            this.WareID = WareID;
            //this._WareID = WareID;
            this.ToolTip = ToolTip;
            this.ProtocolID = ProtocolID;
            this.ProtocolName = ProtocolName;
            this.CertificateID = CertificateID;
            this.CertificateName = CertificateName;
        }
        /// <summary>
        /// Возвращает строковое представление датчика.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return WareName;
        }
        /// <summary>
        /// Проверяет датчик. 
        /// </summary>
        public void Validate()
        {
            Error = Valid ? null : ToolTip;
        }

        /// <summary>
        /// Указывает ошибку для конкретного столбца.
        /// </summary>
        /// <param name="columnName">Имя столбца</param>
        /// <returns></returns>
        String IDataErrorInfo.this[String columnName]
        {
            get { return Error; }
        }
        /// <summary>
        /// Указывает ошибку для всей строки.
        /// </summary>
        String IDataErrorInfo.Error
        {
            get { return String.Empty; }
        }
    }
}