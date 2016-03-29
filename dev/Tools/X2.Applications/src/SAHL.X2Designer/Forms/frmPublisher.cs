using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.XML;

namespace SAHL.X2Designer.Forms
{
    public partial class frmPublisher : Form
    {
        private bool _cancelClose = false;
        private string _currentMapName = "";

        #region properties

        public string DatabaseServer
        {
            get
            {
                return cbxEnvironment.Text;
            }
        }

        #endregion properties

        public frmPublisher(string currentMapName)
        {
            InitializeComponent();
            _currentMapName = currentMapName;
        }

        private void frmPublisher_Load(object sender, EventArgs e)
        {
            foreach (string sname in Properties.Settings.Default.X2Environments)
            {
                cbxEnvironment.Items.Add(sname);
            }
        }

        private void frmPublisher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelClose)
            {
                e.Cancel = true;
                _cancelClose = false;
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (cbxEnvironment.Text.Length == -1)
            {
                MessageBox.Show("Must select an environment to publish to", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _cancelClose = true;
                return;
            }

            if (String.IsNullOrEmpty(_currentMapName) || String.Compare(_currentMapName, "m_view", true) == 0)
            {
                MessageBox.Show("Please load map before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cancelClose = true;
                return;
            }

            btnPublish.DialogResult = DialogResult.OK;
        }
    }

    public class MapListItem
    {
        public string MapName;
        public string Version;
    }
}