using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.XML;

namespace SAHL.X2Designer.Forms
{
    public partial class frmRetrieve : Form
    {
        private bool _cancelClose = false;

        #region properties

        public string DatabaseServer
        {
            get
            {
                return cbxEnvironment.Text;
            }
        }

        #endregion properties

        public frmRetrieve(string currentMapName)
        {
            InitializeComponent();
        }

        private void frmRetrieve_Load(object sender, EventArgs e)
        {
            foreach (string sname in Properties.Settings.Default.X2Environments)
            {
                cbxEnvironment.Items.Add(sname);
            }
        }

        private void frmRetrieve_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelClose)
            {
                e.Cancel = true;
                _cancelClose = false;
            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (cbxEnvironment.Text.Length == -1)
            {
                MessageBox.Show("Must select an environment to retrieve from", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _cancelClose = true;
                return;
            }
        }
    }
}