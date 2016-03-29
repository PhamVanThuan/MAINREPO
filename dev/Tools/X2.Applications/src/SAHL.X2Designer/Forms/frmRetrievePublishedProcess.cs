using System;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmRetrievePublishedProcess : Form
    {
        public frmRetrievePublishedProcess()
        {
            InitializeComponent();
        }

        private void frmRetrievePublishedProcess_Load(object sender, EventArgs e)
        {
            ColumnHeader colProcessID = new ColumnHeader();
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colVersion = new ColumnHeader();

            colProcessID.Text = "Process ID";
            colProcessID.Width = 80;
            colProcessID.TextAlign = HorizontalAlignment.Left;
            colName.Text = "Process Name";
            colName.Width = 180;
            colName.TextAlign = HorizontalAlignment.Left;
            colVersion.Text = "Version";
            colVersion.Width = 200;
            colVersion.TextAlign = HorizontalAlignment.Left;

            listViewProcess.Columns.Add(colProcessID);
            listViewProcess.Columns.Add(colName);
            listViewProcess.Columns.Add(colVersion);

            PopulateList();
        }

        private void PopulateList()
        {
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
        }
    }
}