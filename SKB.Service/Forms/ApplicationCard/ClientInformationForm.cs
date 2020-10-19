using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.ObjectManager;
using DocsVision.BackOffice.ObjectModel;

namespace SKB.Service.Forms.ApplicationCard
{
    /// <summary>
    /// Форма вывода информации о клиенте.
    /// </summary>
    public partial class ClientInformationForm : DevExpress.XtraEditors.XtraForm
    {
        ObjectContext Context;
        /// <summary>
        /// Адрес возврата приборов.
        /// </summary>
        public string sReturnAddress;
        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public string sPhone;
        /// <summary>
        /// Email клиента.
        /// </summary>
        public string sEmail;
        /// <summary>
        /// Контактное лицо.
        /// </summary>
        public string sContactName;
        /// <summary>
        /// Инициализирует форму вывода информации о клиенте.
        /// </summary>
        /// <param name="Context">Объектный контекст.</param>
        /// <param name="ClientID">ID клиента.</param>
        /// <param name="pReturnAddress">Адрес возврата приборов.</param>
        /// <param name="pPhone">Контактный телефон.</param>
        /// <param name="pEmail">Email клиента.</param>
        /// <param name="pContactName">Контактное лицо.</param>
        public ClientInformationForm(ObjectContext Context, string ClientID, string pReturnAddress, string pPhone, string pEmail, string pContactName)
        {

            InitializeComponent();
            this.Context = Context;
            sReturnAddress = pReturnAddress;
            sPhone = pPhone;
            sEmail = pEmail;
            sContactName = pContactName;
            PartnersCompany Company = Context.GetObject<PartnersCompany>(new Guid(ClientID));
            PartnersAddresse[] Addresses = Company.Addresses.ToArray();
            PartnersAddresse RowPostalAddress = null;
            PartnersAddresse RowLegalAddress = null;
            foreach (PartnersAddresse Row in Addresses)
            {
                switch (Row.AddressType)
                {
                    case PartnersAddresseAddressType.PostAddress:
                        RowPostalAddress = Row;
                        break;
                    case PartnersAddresseAddressType.LegalAddress:
                        RowLegalAddress = Row;
                        break;
                }
            }

            if (Company != null)
            {
                this.TClientName.Text = Company.Name;
                this.TFullName.Text = Company.FullName;
               
                // Юридический адрес
                this.TLegalAddress.Text = RowLegalAddress == null ? "" : FormattingAddress(RowLegalAddress.ZipCode, RowLegalAddress.Country, RowLegalAddress.City, RowLegalAddress.Address);
                this.TPostalAddress.Text = RowPostalAddress == null ? "" : FormattingAddress(RowPostalAddress.ZipCode, RowPostalAddress.Country, RowPostalAddress.City, RowPostalAddress.Address);
                this.TReturnAddress.Text = pReturnAddress == "" ? this.TPostalAddress.Text : pReturnAddress;
                this.TPhone.Text = pPhone == "" ? Company.Phone : pPhone;
                this.TEmail.Text = pEmail == "" ? Company.Email : pEmail;
                this.TContactName.Text = pContactName;
            }
        }

        internal static string FormattingAddress(string PostalIndex, string Country, string City, string Address)
        {
            StringCollection FullAddress = new StringCollection();

            if ((PostalIndex != "") && (PostalIndex != null)) { FullAddress.Add(PostalIndex); }
            if ((Country != "") && (Country != null)) { FullAddress.Add(Country); }
            if ((Address != "") && (Address != null)) { FullAddress.Add(Address); }
            if ((City != "") && (City != null)) { FullAddress.Add(City); }

            string[] FAddress = new string[FullAddress.Count];
            FullAddress.CopyTo(FAddress, 0);
            return string.Join(", ", FAddress);
        }

        // Сохранение изменений
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            sReturnAddress = this.TReturnAddress.Text;
            sPhone = this.TPhone.Text;
            sEmail = this.TEmail.Text;
            sContactName = this.TContactName.Text;
            this.Close();
        }

        // Отмена изменений
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}