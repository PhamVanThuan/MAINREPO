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
    public partial class frmCreateNewInstance : Form
    {
        string WFName, PName;
        EngineConnector engine = null;
        Dictionary<string, string> FieldInputs = new Dictionary<string, string>();
        public frmCreateNewInstance(string PName, string WName, string EngineConn)
        {
            InitializeComponent();
            this.Text = "Create Case";
            this.PName = PName;
            this.WFName = WName;
            this.engine = new EngineConnector(WFName, PName, EngineConn);
        }

        private void frmCreateNewInstance_Load(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = string.Empty;
                string SessionID = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(Error))
                {
                    MessageBox.Show(string.Format("Unable to login:{0}{1}", Environment.NewLine, Error));
                    return;
                }
                string CreateActivity = txtCreate.Text;
                engine.SetADUser(txtUser.Text);
                int GenericKey = Convert.ToInt32(txtKeyVal.Text);
                string GenericKeyName = txtKeyName.Text;
                Int64 InstanceID = engine.CreateCase(SessionID, WFName, GenericKey, GenericKeyName, ref Error, CreateActivity);
                txtIID.Text = InstanceID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}