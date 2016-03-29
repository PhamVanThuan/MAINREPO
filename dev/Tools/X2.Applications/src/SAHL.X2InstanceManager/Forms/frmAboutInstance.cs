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
    public partial class frmAboutInstance : Form
    {
        EngineConnector engine = null;
        string pName, wName;
        Int64 InstanceID;
        public frmAboutInstance(Int64 InstanceID, string WorkflowName, string ProcName, string EngineURL)
        {
            this.Text = string.Format("About Instance:{0}", InstanceID);
            InitializeComponent();
            this.InstanceID = InstanceID;
            this.engine = new EngineConnector(WorkflowName, ProcName, EngineURL);
        }

        private void frmAboutInstance_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text += " " + InstanceID;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select p.name PN, w.name WN, w.id, w.storagetable ");
                sb.AppendLine("from x2.instance i (nolock) join x2.workflow w (nolock) ");
                sb.AppendLine("on i.workflowid=w.id ");
                sb.AppendLine("join x2.process p (nolock) on w.ProcessID=p.id ");
                sb.AppendFormat("where i.id={0}; ", InstanceID);

                sb.AppendLine("select A.Name, IAS.ADUserName ");
                sb.AppendLine("from x2.instanceActivitySecurity IAS (nolock) ");
                sb.AppendLine("join x2.Activity A (nolock) on IAS.ACtivityID=A.ID ");
                sb.AppendFormat("where IAS.InstanceID={0}; ", InstanceID);

                sb.AppendLine("select WL.ADUserName from x2.Worklist WL (nolock) ");
                sb.AppendFormat("where WL.InstanceID={0}; ", InstanceID);

                DataSet ds = DBMan.ExecuteSQL(sb.ToString());
                string StorageTable = ds.Tables[0].Rows[0]["StorageTable"].ToString();
                wName = ds.Tables[0].Rows[0]["WN"].ToString();
                pName = ds.Tables[0].Rows[0]["PN"].ToString();

                string SQLData = string.Format("select * from x2data.{0} data (nolock) where InstanceID={1}", StorageTable, InstanceID);
                DataSet dsData = DBMan.ExecuteSQL(SQLData);

                gvSec.DataSource = ds.Tables[1];
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    liWL.Items.Add(dr[0].ToString());
                }

                gvData.DataSource = dsData.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void gvSec_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridViewSelectedCellCollection drc = gvSec.SelectedCells;
            string Name = drc[0].Value.ToString();
            txtActivity.Text = Name;
        }

        Dictionary<string, string> FieldInputs = new Dictionary<string, string>();
        private void button1_Click(object sender, EventArgs e)
        {
            string Error = string.Empty;
            string SessionID = Guid.NewGuid().ToString();
            string Activity = txtActivity.Text;
            bool validInput = true;

            if (String.IsNullOrEmpty(Activity))
            {
                MessageBox.Show("Must enter an Activity to perform");
                validInput = false;
            }
            if (String.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                MessageBox.Show("Must enter a user to peform activity");
                validInput = false;
            }

            if (validInput == true)
            {
                engine.SetADUser(txtUser.Text);
                engine.PerformAction(SessionID, InstanceID, Activity, ref Error, FieldInputs);
                if (!string.IsNullOrEmpty(Error))
                {
                    MessageBox.Show(string.Format("Error performing activity:{0}{1}{2}", Activity, Environment.NewLine, Error));
                }
                else
                {
                    MessageBox.Show("Done");
                    FieldInputs = new Dictionary<string, string>();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string Key = txtKey.Text;
            string Value = txtValue.Text;
            FieldInputs.Add(Key, Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmWorkflowEvents frm = new frmWorkflowEvents(pName, wName, engine, InstanceID);
            frm.ShowDialog(this);
            frm.Dispose();
        }
    }
}