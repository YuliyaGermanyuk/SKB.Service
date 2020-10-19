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
    public partial class CalibrationOrVerificationForm : XtraForm
    {

        public bool? IsCalibration;
        /// <summary>
        /// Инициализирует форму делегирования.
        /// </summary>
        /// <param name="CardHost">Хост карточек.</param>
        /// <param name="Context">Объектный контекст.</param>
        public CalibrationOrVerificationForm()
        {
            InitializeComponent();

            IsCalibration = null;
        }

        private void CalibrationButton_Click(Object sender, EventArgs e)
        {
            try
            {
                this.IsCalibration = true;
                this.Close();
            }
            catch (MyException Ex)
            {
                MyMessageBox.Show(Ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
        private void VerificationButton_Click(Object sender, EventArgs e)
        {
            try
            {
                this.IsCalibration = false;
                this.Close();
            }
            catch (MyException Ex)
            {
                MyMessageBox.Show(Ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }

        private void CalibrationOrVerificationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}