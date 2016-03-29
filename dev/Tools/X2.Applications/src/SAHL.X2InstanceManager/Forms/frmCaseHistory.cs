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
    public partial class frmCaseHistory : Form
    {
        EngineConnector engine = null;
        Int64 InstanceID;
        public frmCaseHistory(Int64 InstanceID, string WorkflowName, string ProcName, string EngineURL)
        {
            this.Text = string.Format("Instance:{0} HIstory", InstanceID);
            InitializeComponent();
            this.InstanceID = InstanceID;
            this.engine = new EngineConnector(WorkflowName, ProcName, EngineURL);
        }

        private void frmCaseHistory_Load(object sender, EventArgs e)
        {
            DataSet ds = GetCaseAndHistInfo();
            gv.DataSource = ds.Tables[1];
        }

        private DataSet GetCaseAndHistInfo()
        {
            StringBuilder sb = new StringBuilder();
            // Get the Instance REcord
            sb.AppendFormat("select * from x2.instance i ");
            sb.AppendFormat("where i.id={0};", InstanceID);
            // Get the case history
            sb.AppendFormat("select il.* from x2.instancelog il ");
            sb.AppendFormat("where il.InstanceID={0}", InstanceID);
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            return ds;
        }
    }
}