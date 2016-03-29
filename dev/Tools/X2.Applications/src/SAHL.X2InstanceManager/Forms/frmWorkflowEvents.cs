using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2InstanceManager.Misc;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmWorkflowEvents : Form
    {
        EngineConnector engine;
        string WFName, PName;
        Int64 InstanceID;
        public frmWorkflowEvents(string PName, string WFName, EngineConnector engine, Int64 InstanceID)
        {
            InitializeComponent();
            this.engine = engine;
            this.WFName = WFName;
            this.PName = PName;
            this.InstanceID = InstanceID;
        }

        private void frmWorkflowEvents_Load(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select * from x2.externalactivity ea (nolock) where WorkflowID=( ");
                sb.AppendFormat("select max(id) from x2.workflow (nolock) where name='{0}');", WFName);
                DataSet ds = DBMan.ExecuteSQL(sb.ToString());
                // sort out the dataarow view crap in the listbox
                liEIDs.DataSource = ds.Tables[0];
                liEIDs.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        string SelectedEName;
        int SelectedEID;
        int WID;
        private void liEIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                object o = liEIDs.SelectedItem;
                DataRowView drv = o as DataRowView;
                if (null != drv)
                {
                    SelectedEName = drv.Row["Name"].ToString();
                    SelectedEID = Convert.ToInt32(drv.Row["ID"]);
                    WID = Convert.ToInt32(drv.Row["WorkFlowID"]);
                    txtEXT.Text = SelectedEName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string XML = string.Empty;
                if (txtXML.Text != "")
                    XML = txtXML.Text;
                sb.AppendLine("insert into x2.activeexternalactivity (ExternalActivityID, WorkflowID, ActivatingInstanceID, ActivationTime, ActivityXMLData) ");
                sb.AppendFormat("values ({0}, {1}, {2}, getdate(), ", SelectedEID, WID, InstanceID);
                if (string.IsNullOrEmpty(XML))
                {
                    sb.AppendFormat(" NULL);");
                }
                else
                {
                    sb.AppendFormat(" '{0}');", XML);
                }

                DBMan.RunNonQuery(sb.ToString());
                MessageBox.Show("Inserted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}