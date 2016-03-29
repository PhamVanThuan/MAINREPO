using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2InstanceManager.Misc;
using SAHL.X2InstanceManager.Items;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmMove : Form
    {
        EngineConnector engine = null;
        string pName, wName, SessionID;
        Int64 InstanceID;
        public frmMove(Int64 InstanceID, string WorkflowName, string ProcName, string EngineURL)
        {
            this.Text = string.Format("Move instance:{0}", InstanceID);
            InitializeComponent();
            this.InstanceID = InstanceID;
            this.engine = new EngineConnector(WorkflowName, ProcName, EngineURL);
        }

        private void frmMove_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("Move instance:{0}", InstanceID);
            SAHL.X2.Framework.DataSets.X2 ds = new SAHL.X2.Framework.DataSets.X2();
            DBMan.GetPotentialStatesForInstance(ds, InstanceID);
            foreach (SAHL.X2.Framework.DataSets.X2.StateRow dr in ds.State.Rows)
            {
                StateItem item = new StateItem(dr);
                li.Items.Add(item);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            StateItem item = li.SelectedItem as StateItem;
            if (null != item)
            {
                DBMan.UpdateInstanceState(item.ID, InstanceID);

                var resp = engine.RecalcSecurity(InstanceID, "");
                if (null == resp)
                {
                    MessageBox.Show("Done.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show(string.Format("Error recalculating security. Instance may not appear on correct worklists"));
                }
            }
        }
    }
}