using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.CardHost;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services.Entities.KindSetting;

using RKIT.MyMessageBox;

using SKB.Base;
using SKB.Base.Dictionary;

namespace SKB.Service.Forms.CertificateCreationCard
{
    /// <summary>
    /// Форма простого делегирования.
    /// </summary>
    public partial class DelegateForm : XtraForm
    {
        ICardHost CardHost;
        ObjectContext Context;

        /// <summary>
        /// Сотрудник, которому делегируется задание (Делегат).
        /// </summary>
        public StaffEmployee Delegate
        {
            get
            {
                return Edit_Delegate.Tag.ToGuid().IsEmpty() ? null : Context.GetObject<StaffEmployee>(Edit_Delegate.Tag.ToGuid());
            }
        }
        /// <summary>
        /// Комментарий для делегата.
        /// </summary>
        public String Comment
        {
            get
            {
                return Edit_Comment.EditValue as String;
            }
        }
        /// <summary>
        /// Инициализирует форму делегирования.
        /// </summary>
        /// <param name="CardHost">Хост карточек.</param>
        /// <param name="Context">Объектный контекст.</param>
        public DelegateForm(ICardHost CardHost, ObjectContext Context)
        {
            InitializeComponent();

            this.CardHost = CardHost;
            this.Context = Context;
        }

        private void Edit_Delegate_ButtonPressed(Object sender, ButtonPressedEventArgs e)
        {
            SelectForm Form = new SelectForm("Выберите сотрудника для делегирования...", CardHost, Context, SelectionType.StaffEmployee, Guid.Empty, Edit_Delegate.Text);
            switch (Form.ShowDialog())
            {
                case DialogResult.OK:
                    Edit_Delegate.EditValue = Form.SelectedItem.Name;
                    Edit_Delegate.Tag = Form.SelectedItem.Id;
                    break;
            }
        }

        private void Button_Click(Object sender, EventArgs e)
        {
            try
            {
                switch ((sender as SimpleButton).DialogResult)
                {
                    case DialogResult.OK:
                        if (Delegate.IsNull())
                            throw new MyException("Не выбран делегат!");
                        if (String.IsNullOrWhiteSpace(Comment))
                            throw new MyException("Не указан комментарий!");
                        break;
                }
                DialogResult = (sender as SimpleButton).DialogResult;
            }
            catch (MyException Ex)
            {
                MyMessageBox.Show(Ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}