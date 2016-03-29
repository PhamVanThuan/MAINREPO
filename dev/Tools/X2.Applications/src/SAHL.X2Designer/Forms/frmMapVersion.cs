using System;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmMapVersion : Form
    {
        public frmMapVersion()
        {
            InitializeComponent();
        }

        private void frmChangeMapVersion_Load(object sender, EventArgs e)
        {
            if (lblCurrentVersion.Text != "UNKNOWN")
                txtNewVersion.Text = lblCurrentVersion.Text;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        }
    }
}