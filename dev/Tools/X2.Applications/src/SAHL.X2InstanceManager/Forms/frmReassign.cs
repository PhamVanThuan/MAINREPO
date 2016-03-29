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
    public partial class frmReassign : Form
    {
        EngineConnector engine = null;
        int ApplicationKey;
        Int64 InstanceID;
        string WorkflowName, ProcName, SessionID, EngineURL;
        

        public frmReassign(Int64 ID, string WorkflowName, string ProcName, string EngineURL)
        {
            this.Text = string.Format("Reassign Instance {0}", ID);
            this.InstanceID = ID;
            this.WorkflowName = WorkflowName;
            this.ProcName = ProcName;
            this.EngineURL = EngineURL;
            InitializeComponent();
            MainForm.SetThreadPrincipal("X2");
        }

        private void btnReassign_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.SetThreadPrincipal("X2");
                // get selected user
                UserObj ad = liReassignTo.SelectedItem as UserObj;
                StringBuilder sb = new StringBuilder();
                // update offerrole and insert new offerrole record.
                sb.AppendFormat("update [2am]..offerrole set generalstatuskey=2 ");
                sb.AppendFormat("where offerroletypekey={0} and offerkey={1}", ad.ORTKey, ApplicationKey);
                DBMan.RunNonQuery(sb.ToString());
                sb = new StringBuilder();
                sb.AppendFormat("insert into [2am]..offerrole values ({0}, {1}, {2}, 1, getdate())", ad.LEKEy, ApplicationKey, ad.ORTKey);
                DBMan.RunNonQuery(sb.ToString());
                
                // update workflowassignment and insert new workflowassignment record.
                sb = new StringBuilder();
                sb.AppendFormat("select s.name,i.* from x2.x2.state s (nolock) ");
                sb.AppendFormat("join x2.x2.instance i (nolock) on i.stateid = s.id where i.id = {0} order by i.id asc", Convert.ToInt64(InstanceID));
                DataSet ds = DBMan.ExecuteSQL(sb.ToString());
                string StateName = Convert.ToString(ds.Tables[0].Rows[0][0]);

                //setup data from instance for use in workflowhistory table
                int StateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"]);
                int ActivityID = ds.Tables[0].Rows[0]["ActivityID"] == DBNull.Value ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["ActivityID"]);
                string CreatorADUserName = Convert.ToString(ds.Tables[0].Rows[0]["CreatorADUserName"]);
                DateTime CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]);
                string CreationDateFormatted = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", CreationDate);
                int Priority = ds.Tables[0].Rows[0]["Priority"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["Priority"]);

                sb = new StringBuilder();
                sb.AppendFormat("update [x2].[x2].workflowassignment set generalstatuskey = 2 ");
                sb.AppendFormat("where instanceid = {0}", Convert.ToInt64(InstanceID));
                DBMan.RunNonQuery(sb.ToString());
                sb = new StringBuilder();
                sb.AppendFormat("exec pr_AssignWorkflowRole {0}, {1}, {2}, '{3}'", Convert.ToInt64(InstanceID), ad.BLAKey, ad.ADUserKey, StateName);
                DBMan.RunNonQuery(sb.ToString());

                // need to update the workflowhistory table too. first get the last row to get all the data which we can use again
                sb = new StringBuilder();
                sb.AppendFormat("select top 1 * from x2.x2.workflowhistory where instanceid = {0} order by id desc", Convert.ToInt64(InstanceID));
                ds = DBMan.ExecuteSQL(sb.ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    StateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"]);
                    ActivityID = Convert.ToInt32(ds.Tables[0].Rows[0]["ActivityID"]);
                    CreatorADUserName = Convert.ToString(ds.Tables[0].Rows[0]["CreatorADUserName"]);
                    CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]);
                    CreationDateFormatted = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", CreationDate);
                    Priority = Convert.ToInt32(ds.Tables[0].Rows[0]["Priority"]);
                }

                sb = new StringBuilder();
                sb.AppendFormat("insert into x2.x2.workflowhistory ");
                if (ActivityID == -1)
                {
                    sb.AppendFormat("(InstanceID, StateID, CreatorADUserName, CreationDate, StateChangeDate, ActivityDate, ADUserName, Priority) ");
                    sb.AppendFormat("values ({0}, {1}, '{2}', '{3}', GetDate(), GetDate(), '{4}', {5})", Convert.ToInt64(InstanceID), StateID, CreatorADUserName, CreationDateFormatted, ad.ADUserName, Priority);
                }
                else
                {
                    sb.AppendFormat("(InstanceID, StateID, ActivityID, CreatorADUserName, CreationDate, StateChangeDate, ActivityDate, ADUserName, Priority) ");
                    sb.AppendFormat("values ({0}, {1}, {2}, '{3}', '{4}', GetDate(), GetDate(), '{5}', {6})", Convert.ToInt64(InstanceID), StateID, ActivityID, CreatorADUserName, CreationDateFormatted, ad.ADUserName, Priority);
                }

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
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        private void frmReassign_Load(object sender, EventArgs e)
        {
            try
            {
                engine = new EngineConnector(WorkflowName, ProcName, EngineURL);
                string Error = string.Empty;
                SessionID = Guid.NewGuid().ToString();

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
                
                MainForm.SetThreadPrincipal("X2");

                // look at the active offerroles and then join back to aduser to get a list of active users for those offerroletypes
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select ");
                sb.AppendLine("a.adusername, ort.description, a.legalentitykey, ort.offerroletypekey, bla.OfferRoleTypeOrganisationStructureMappingKey, a.aduserkey ");
                sb.AppendLine("from x2.x2.instance i ");
                sb.AppendLine("join x2.x2.stateworklist swl on swl.stateid = i.stateid ");
                sb.AppendLine("join x2.x2.securitygroup sg on sg.id = swl.securitygroupid ");
                sb.AppendLine("join [2am]..offerroletype ort on ort.Description = sg.Name ");
                sb.AppendLine("join [2am]..OfferRoleTypeOrganisationStructureMapping bla on bla.OfferRoleTypeKey = ort.OfferRoleTypeKey ");
                sb.AppendLine("join [2am]..UserOrganisationStructure uos on uos.OrganisationStructureKey = bla.OrganisationStructureKey ");
                sb.AppendLine("join [2am]..Aduser a on a.ADUserKey = uos.ADUserKey ");
                sb.AppendFormat("where i.id = {0}", InstanceID);
                sb.AppendLine("order by a.adusername ");

                DataSet ds = DBMan.ExecuteSQL(sb.ToString());
                // if there are no users returned then we can list everyone.
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sb = new StringBuilder();
                    sb.AppendLine("select ");
                    sb.AppendLine("distinct(a.adusername), ort.description, a.legalentitykey, ort.offerroletypekey, bla.OfferRoleTypeOrganisationStructureMappingKey, a.aduserkey ");
                    sb.AppendLine("from [2am]..offerroletype ort ");
                    sb.AppendLine("join [2am]..OfferRoleTypeOrganisationStructureMapping bla on bla.OfferRoleTypeKey = ort.OfferRoleTypeKey ");
                    sb.AppendLine("join [2am]..UserOrganisationStructure uos on uos.OrganisationStructureKey = bla.OrganisationStructureKey ");
                    sb.AppendLine("join [2am]..Aduser a on a.ADUserKey = uos.ADUserKey ");
                    sb.AppendLine("order by a.adusername ");
                    ds = DBMan.ExecuteSQL(sb.ToString());
                }
                List<UserObj> users = new List<UserObj>();
                foreach (DataRow dru in ds.Tables[0].Rows)
                {
                    try{
                        UserObj u = new UserObj(dru["adusername"].ToString(), dru["description"].ToString(), Convert.ToInt32(dru["legalentitykey"]), Convert.ToInt32(dru["offerroletypekey"]), Convert.ToInt32(dru["OfferRoleTypeOrganisationStructureMappingKey"]), Convert.ToInt32(dru["ADUserKey"]));
                    users.Add(u);
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
}