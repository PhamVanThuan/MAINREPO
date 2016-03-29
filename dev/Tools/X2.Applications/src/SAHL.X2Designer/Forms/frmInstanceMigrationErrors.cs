using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2Designer.Forms
{
    public partial class frmInstanceMigrationErrors : Form
    {
        public frmInstanceMigrationErrors(List<ListRequestItem> lstResponses)
        {
            InitializeComponent();
            PopulateFailureList(lstResponses);
        }

        private void PopulateFailureList(List<ListRequestItem> lstResponses)
        {
            for (int x = 0; x < lstResponses.Count; x++)
            {
                listViewErrors.Items.Add(new ListViewItem(new string[] { lstResponses[x].InstnaceID.ToString() }));
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveFileDialog mDialog = new SaveFileDialog();
            mDialog.DefaultExt = ".txt";
            mDialog.Filter = "Text Files | *.txt";
            mDialog.Title = "Save Failed Instances";
            if (mDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter mWriter = new StreamWriter(mDialog.FileName);
                for (int x = 0; x < listViewErrors.Items.Count; x++)
                {
                    mWriter.WriteLine(listViewErrors.Items[x].Text);
                }
                mWriter.Close();
                mWriter.Dispose();
                Close();
            }
        }
    }
}