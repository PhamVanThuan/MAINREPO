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
    public partial class frmAssignOfNinjaNess : Form
    {
        EngineConnector engine = null;
        int ApplicationKey;
        Int64 InstanceID;
        int ORTKey;
        string WorkflowName, ProcName, SessionID, EngineURL, WorkflowStorageKeyName;

        public frmAssignOfNinjaNess(Int64 ID, string WorkflowName, string ProcName, string EngineURL)
        {
            this.Text = string.Format("Reassign Instance {0}", ID);
            this.InstanceID = ID;
            this.WorkflowName = WorkflowName;
            this.ProcName = ProcName;
            this.EngineURL = EngineURL;
            InitializeComponent();
            MainForm.SetThreadPrincipal("X2");
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        private void frmAssignOfNinjaNess_Load(object sender, EventArgs e)
        {
            try
            {
                engine = new EngineConnector(WorkflowName, ProcName, EngineURL);
                string Error = string.Empty;
                SessionID = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(Error))
                {
                    MessageBox.Show(string.Format("Unable to login to the engine{0}{1}", Environment.NewLine, Error));
                    return;
                }
                DataTable dt = DBMan.GetOfferRoleTypeList(WorkflowName);
                liDynamicRoles.DataSource = dt;
                liDynamicRoles.DisplayMember = "Description";
                liDynamicRoles.ValueMember = "OfferRoleTypeKey";

                // retreive the workflow record to determine the StorageTable and StorageKey
                string storageTable = Misc.Misc.FixName(WorkflowName);
                string storageKey = "ApplicationKey";
                DataRow drWorkflow = DBMan.GetX2WorkflowRow(WorkflowName, ProcName);
                if (drWorkflow != null)
                {
                    storageTable = drWorkflow["StorageTable"].ToString();
                    storageKey = drWorkflow["StorageKey"].ToString();
                }

                // get the x2 data row
                DataRow dr = DBMan.GetX2DataRow(InstanceID, storageTable);

                // set the ApplicationKey
                ApplicationKey = Convert.ToInt32(dr[storageKey]);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void liDynamicRoles_Click(object sender, EventArgs e)
        {
            if (null != liDynamicRoles.SelectedValue)
            {
                try
                {
                    MainForm.SetThreadPrincipal("X2");

                    ORTKey = Convert.ToInt32(liDynamicRoles.SelectedValue);

                    StringBuilder sb = new StringBuilder();

                    if (ORTKey == 1) // consultant (life)
                    {
                        sb.AppendLine("select ");
                        sb.AppendLine("a.adusername, 'Consultant' as description, a.legalentitykey, 1 as offerroletypekey ");
                        sb.AppendLine("from [2am]..organisationstructure os ");
                        sb.AppendLine("join [2am]..userorganisationstructure uos on os.organisationstructurekey=uos.organisationstructurekey ");
                        sb.AppendLine("join [2am]..aduser a on uos.aduserkey=a.aduserkey ");
                        sb.AppendLine("where os.OrganisationStructureKey=900 "); // CCC Consultant
                        sb.AppendLine("and a.generalstatuskey=1 ");
                        sb.AppendLine("order by a.adusername ");
                    }
                    else
                    {
                        sb.AppendLine("select ");
                        sb.AppendLine("a.adusername, ort.description, a.legalentitykey, ort.offerroletypekey ");
                        sb.AppendLine("from [2am]..offerroletype ort  ");
                        sb.AppendLine("join [2am]..OfferRoleTypeOrganisationStructureMapping bla on ort.offerroletypekey=bla.offerroletypekey ");
                        sb.AppendLine("join [2am]..organisationstructure os on bla.organisationstructurekey=os.organisationstructurekey ");
                        sb.AppendLine("join [2am]..userorganisationstructure uos on os.organisationstructurekey=uos.organisationstructurekey ");
                        sb.AppendLine("join [2am]..aduser a on uos.aduserkey=a.aduserkey ");
                        sb.AppendLine("where a.generalstatuskey=1 ");
                        sb.AppendFormat("and ort.offerroletypekey={0} ", ORTKey);
                        sb.AppendLine("order by ort.description ");
                    }

                    DataSet ds = DBMan.ExecuteSQL(sb.ToString());
                    List<UserObj> users = new List<UserObj>();
                    foreach (DataRow dru in ds.Tables[0].Rows)
                    {
                        try
                        {
                            //UserObj u = new UserObj(dru["adusername"].ToString(), dru["description"].ToString(), Convert.ToInt32(dru["legalentitykey"]), Convert.ToInt32(dru["offerroletypekey"]));
                            //users.Add(u);
                        }
                        catch { }
                    }
                    liReassignTo.DataSource = users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnReassign_Click(object sender, EventArgs e)
        {
            try
            {

                MainForm.SetThreadPrincipal("X2");
                // get selected user
                UserObj ad = liReassignTo.SelectedItem as UserObj;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update [2am]..offerrole set generalstatuskey=2 ");
                sb.AppendFormat("where offerroletypekey={0} and offerkey={1}", ad.ORTKey, ApplicationKey);
                DBMan.RunNonQuery(sb.ToString());
                sb = new StringBuilder();
                sb.AppendFormat("insert into [2am]..offerrole values ({0}, {1}, {2}, 1, getdate())", ad.LEKEy, ApplicationKey, ad.ORTKey);
                DBMan.RunNonQuery(sb.ToString());
                engine.RecalcSecurity(Convert.ToInt64(InstanceID), "");
                MessageBox.Show("Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void liReassignTo_Click(object sender, EventArgs e)
        {

        }
    }
}