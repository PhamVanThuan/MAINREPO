using System;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class FormDebug : Form
    {
        public FormDebug(string s, bool Updateable)
        {
            InitializeComponent();
            txt.Text = s;
            if (Updateable)
                btnSet.Visible = true;
        }

        private void FormDebug_Load(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            Helpers.XMLPath = txt.Text;
            this.Close();
        }
    }
}