using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RKIT.MyMessageBox;

namespace SKB.Service.Forms.ServiceCard
{
    public partial class InputBox : DevExpress.XtraEditors.XtraForm
    {
        public string CommentText = "";

        public InputBox()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (Comment.Text == "")
            {
                MyMessageBox.Show("Введите комментарий.");
                return;
            }
            else
            {
                CommentText = Comment.Text;
                this.Close();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
