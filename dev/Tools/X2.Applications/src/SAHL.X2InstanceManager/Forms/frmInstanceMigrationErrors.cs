using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmInstanceMigrationErrors : Form
    {
        public frmInstanceMigrationErrors(List<ListRequestItem> lstResponses, string errorMessage)
        {
            InitializeComponent();
            PopulateFailureList(lstResponses);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                lblErrorMessage.Text = errorMessage;
            }
            else
            {
                lblErrorMessageHeader.Visible = false;
                lblErrorMessage.Visible = false;
            }

        }

        private void PopulateFailureList(List<ListRequestItem> lstResponses)
        {
            for (int x = 0; x < lstResponses.Count; x++)
            {
                listViewErrors.Items.Add(new ListViewItem(new string[] {lstResponses[x].InstnaceID.ToString()}));
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //SaveFileDialog mDialog = new SaveFileDialog();
            //mDialog.DefaultExt = ".txt";
            //mDialog.Filter = "Text Files | *.txt";
            //mDialog.Title = "Save Failed Instances";            
            //if (mDialog.ShowDialog() == DialogResult.OK)
            //{
            //    StreamWriter mWriter = new StreamWriter(mDialog.FileName);
            //    for (int x = 0; x < listViewErrors.Items.Count; x++)
            //    {
            //        mWriter.WriteLine(listViewErrors.Items[x].Text);
            //    }
            //    mWriter.Close();
            //    mWriter.Dispose();
            //    Close();
            //}

            Close();
        }
    }
}