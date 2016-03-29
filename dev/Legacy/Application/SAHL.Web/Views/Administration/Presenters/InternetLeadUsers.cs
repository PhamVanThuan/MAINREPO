using System;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using IInternetLeadUsers = SAHL.Web.Views.Administration.Interfaces.IInternetLeadUsers;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Attorney Add
    /// </summary>
    public class InternetLeadUsers : SAHLCommonBasePresenter<IInternetLeadUsers>
    {
        private DataTable active_internetleadusers;
        private DataTable inactive_internetleadusers;
        private IInternetRepository internetRepository;
        protected IInternetRepository InternetRepository
        {
            get
            {
                if (internetRepository == null)
                    internetRepository = RepositoryFactory.GetRepository<IInternetRepository>();
                return internetRepository;
            }
        }

        private ILookupRepository lookuprepository;
        protected ILookupRepository LookupRepository
        {
            get
            {
                if (lookuprepository == null)
                    lookuprepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return lookuprepository;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public InternetLeadUsers(IInternetLeadUsers view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            View.btnAddClick += AddButtonClicked;
            View.btnCancelClick += CancelButtonClicked;
            //View.btnRefreshClick += RefreshButtonClicked;
            View.btnRemoveClick += RemoveButtonClicked;
            View.btnUpdateClick += UpdateButtonClicked;
            View.lstActiveUsersSelectedIndexChanged += lstActiveUsersSelectedIndexChanged;
            View.lstInactiveUsersSelectedIndexChanged += lstInactiveUsersSelectedIndexChanged;

            // Initial State loads Internet Lead Users : Active, and inactive in separate columns
            //Save
            if (!View.IsPostBack)
            {
                RefreshInternetLeadUsers();
                //SetupInternetLeadUsers();
                //LoadData();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (View.IsPostBack)
                SetupInterface();


        }

        void lstActiveUsersSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int ActiveUserIndex = View.ActiveUsersIndex;
            ActiveUsersselectedIndexChanged(ActiveUserIndex);
        }

        private void ActiveUsersselectedIndexChanged(int ActiveUserIndex)
        {

            PrivateCacheData.Remove("ActiveUsersIndex");
            PrivateCacheData.Add("ActiveUsersIndex", ActiveUserIndex);
            PrivateCacheData.Remove("ActiveInternetLeadUserKey");
            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            if (active_internetleadusers != null)
                if (active_internetleadusers.Rows.Count > 0)
                    PrivateCacheData.Add("ActiveInternetLeadUserKey", active_internetleadusers.Rows[ActiveUserIndex]["Key"]);
                else
                    PrivateCacheData.Add("ActiveInternetLeadUserKey", 0);
        }

        void lstInactiveUsersSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int InactiveUserIndex = View.InactiveUsersIndex;
            InactiveUsersselectedIndexChanged(InactiveUserIndex);
        }

        private void InactiveUsersselectedIndexChanged(int InactiveUserIndex)
        {

            PrivateCacheData.Remove("InactiveUsersIndex");
            PrivateCacheData.Add("InactiveUsersIndex", InactiveUserIndex);
            PrivateCacheData.Remove("InactiveInternetLeadUserKey");
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;
            if (inactive_internetleadusers != null)
                if (inactive_internetleadusers.Rows.Count > 0)
                    PrivateCacheData.Add("InactiveInternetLeadUserKey", inactive_internetleadusers.Rows[InactiveUserIndex]["Key"]);
                else
                    PrivateCacheData.Add("InactiveInternetLeadUserKey", 0);
        }


        private void SetupInterface()
        {
            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;

            View.PopulatelstActiveUsers(active_internetleadusers);
            View.PopulatelstInactiveUsers(inactive_internetleadusers);

        }


        void RemoveButtonClicked(object sender, EventArgs e)
        {
            int inactivekey = Convert.ToInt32(PrivateCacheData["ActiveInternetLeadUserKey"]);

            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;

            if (active_internetleadusers != null)
                for (int i = 0; i < active_internetleadusers.Rows.Count; i++)
                {
                    if (Convert.ToInt32(active_internetleadusers.Rows[i]["Key"]) == inactivekey)
                    {
                        if (inactive_internetleadusers != null)
                        {
                            DataRow dr = inactive_internetleadusers.NewRow();
                            dr["Key"] = active_internetleadusers.Rows[i]["Key"];
                            dr["ADUserKey"] = active_internetleadusers.Rows[i]["ADUserKey"];
                            dr["ADUserName"] = active_internetleadusers.Rows[i]["ADUserName"];
                            dr["Flag"] = active_internetleadusers.Rows[i]["Flag"];
                            dr["CaseCount"] = active_internetleadusers.Rows[i]["CaseCount"];
                            dr["GeneralStatus"] = (int)GeneralStatuses.Inactive;
                            dr["LastCaseKey"] = active_internetleadusers.Rows[i]["LastCaseKey"];
                            dr["Edited"] = 1;
                            inactive_internetleadusers.Rows.Add(dr);
                        }

                        DataRow removerow = active_internetleadusers.Rows[i];
                        active_internetleadusers.Rows.Remove(removerow);

                        PrivateCacheData.Remove("ActiveInternetLeadUsers");
                        PrivateCacheData.Add("ActiveInternetLeadUsers", active_internetleadusers);
                        PrivateCacheData.Remove("InactiveInternetLeadUsers");
                        PrivateCacheData.Add("InactiveInternetLeadUsers", inactive_internetleadusers);

                        if (inactive_internetleadusers != null)
                            View.InactiveUsersIndex = inactive_internetleadusers.Rows.Count - 1;
                        View.ActiveUsersIndex = 0;

                        ActiveUsersselectedIndexChanged(0);
                        if (inactive_internetleadusers != null)
                            InactiveUsersselectedIndexChanged(inactive_internetleadusers.Rows.Count - 1);

                        SetupInterface();
                        return;
                    }
                }


        }

        void AddButtonClicked(object sender, EventArgs e)
        {
            int activekey = Convert.ToInt32(PrivateCacheData["InactiveInternetLeadUserKey"]);

            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;

            if (inactive_internetleadusers != null)
                for (int i = 0; i < inactive_internetleadusers.Rows.Count; i++)
                {
                    if (Convert.ToInt32(inactive_internetleadusers.Rows[i]["Key"]) == activekey)
                    {
                        if (active_internetleadusers != null)
                        {
                            DataRow dr = active_internetleadusers.NewRow();
                            dr["Key"] = inactive_internetleadusers.Rows[i]["Key"];
                            dr["ADUserKey"] = inactive_internetleadusers.Rows[i]["ADUserKey"];
                            dr["ADUserName"] = inactive_internetleadusers.Rows[i]["ADUserName"];
                            dr["Flag"] = inactive_internetleadusers.Rows[i]["Flag"];
                            dr["CaseCount"] = inactive_internetleadusers.Rows[i]["CaseCount"];
                            dr["GeneralStatus"] = (int)GeneralStatuses.Active;
                            dr["LastCaseKey"] = inactive_internetleadusers.Rows[i]["LastCaseKey"];
                            dr["Edited"] = 1;
                            active_internetleadusers.Rows.Add(dr);
                        }

                        DataRow removerow = inactive_internetleadusers.Rows[i];
                        inactive_internetleadusers.Rows.Remove(removerow);



                        PrivateCacheData.Remove("ActiveInternetLeadUsers");
                        PrivateCacheData.Add("ActiveInternetLeadUsers", active_internetleadusers);
                        PrivateCacheData.Remove("InactiveInternetLeadUsers");
                        PrivateCacheData.Add("InactiveInternetLeadUsers", inactive_internetleadusers);

                        View.InactiveUsersIndex = 0;
                        if (active_internetleadusers != null)
                            View.ActiveUsersIndex = active_internetleadusers.Rows.Count - 1;

                        if (active_internetleadusers != null)
                            ActiveUsersselectedIndexChanged(active_internetleadusers.Rows.Count - 1);
                        InactiveUsersselectedIndexChanged(0);

                        SetupInterface();
                        return;
                    }
                }

        }

        void CancelButtonClicked(object sender, EventArgs e)
        {
            PrivateCacheData.Remove("ActiveInternetLeadUsers");
            PrivateCacheData.Remove("InactiveInternetLeadUsers");
            _view.Navigator.Navigate("AdminHaloConfig");
        }


        //void RefreshButtonClicked(object sender, EventArgs e)
        //{
        //    RefreshInternetLeadUsers();
        //}

        private void RefreshInternetLeadUsers()
        {
            if (InternetRepository.RefreshInternetLeadUsers())
            {
                PrivateCacheData.Remove("ActiveInternetLeadUsers");
                PrivateCacheData.Remove("InactiveInternetLeadUsers");

                SetupInternetLeadUsers();
                LoadData();
                UpdateData();
                SetupInterface();
            }
        }


        void UpdateButtonClicked(object sender, EventArgs e)
        {
            UpdateData();
            PrivateCacheData.Remove("ActiveInternetLeadUsers");
            PrivateCacheData.Remove("InactiveInternetLeadUsers");
            _view.Navigator.Navigate("AdminHaloConfig");
        }

        private void UpdateData()
        {
            EnsureRoundRobinFlag();
            
            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;

            if (active_internetleadusers != null)
                for (int i = 0; i < active_internetleadusers.Rows.Count; i++)
                    if (active_internetleadusers.Rows[i]["Edited"].ToString() == "1")
                    {
                        TransactionScope txn = new TransactionScope();
                        try
                        {
                            bool Flag = false;
                            if (active_internetleadusers.Rows[i]["Flag"].ToString() == "1")
                                Flag = true;
                                InternetRepository.UpdateInternetLeadUser(Convert.ToInt32(active_internetleadusers.Rows[i]["Key"]), internetRepository.GetActiveGeneralStatus(), Flag);

                            txn.VoteCommit();
                        }
                        catch (Exception)
                        {
                            txn.VoteRollBack();
                            if (_view.IsValid)
                                throw;
                        }
                        finally
                        {
                            txn.Dispose();
                        }
                    }

            if (inactive_internetleadusers != null)
                for (int i = 0; i < inactive_internetleadusers.Rows.Count; i++)
                    if (inactive_internetleadusers.Rows[i]["Edited"].ToString() == "1")
                    {
                        TransactionScope txn = new TransactionScope();
                        try
                        {
                            bool Flag = false;
                            if (inactive_internetleadusers.Rows[i]["Flag"].ToString() == "1")
                                Flag = true;
                            InternetRepository.UpdateInternetLeadUser(Convert.ToInt32(inactive_internetleadusers.Rows[i]["Key"]), internetRepository.GetInActiveGeneralStatus(), Flag);
                            txn.VoteCommit();
                        }
                        catch (Exception)
                        {
                            txn.VoteRollBack();
                            if (View.IsValid)
                                throw;
                        }
                        finally
                        {
                            txn.Dispose();
                        }
                    }


            PrivateCacheData.Remove("ActiveInternetLeadUsers");
            PrivateCacheData.Remove("InactiveInternetLeadUsers");
            PrivateCacheData.Add("ActiveInternetLeadUsers", active_internetleadusers);
            PrivateCacheData.Add("InactiveInternetLeadUsers", inactive_internetleadusers);
        }


        private void LoadData()
        {

            IEventList<SAHL.Common.BusinessModel.Interfaces.IInternetLeadUsers> ILU = InternetRepository.GetAllInternetLeadUsers();
            // Populate the Dataset and Cache It in the Private Cache
            for (int i = 0; i < ILU.Count; i++)
            {
                DataRow dr = ILU[i].GeneralStatus.Key == (int)GeneralStatuses.Active ? active_internetleadusers.NewRow() : inactive_internetleadusers.NewRow();
                dr["Key"] = ILU[i].Key;
                dr["ADUserKey"] = ILU[i].ADUser.Key;
                dr["ADUserName"] = ILU[i].ADUser.ADUserName;
                dr["Flag"] = ILU[i].Flag;
                dr["CaseCount"] = ILU[i].CaseCount;
                dr["GeneralStatus"] = ILU[i].GeneralStatus.Key;
                dr["LastCaseKey"] = ILU[i].LastCaseKey;
                dr["Edited"] = 0;
                if (ILU[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    active_internetleadusers.Rows.Add(dr);
                else
                    inactive_internetleadusers.Rows.Add(dr);
            }

            PrivateCacheData.Remove("ActiveInternetLeadUsers");
            PrivateCacheData.Remove("InactiveInternetLeadUsers");
            PrivateCacheData.Add("ActiveInternetLeadUsers", active_internetleadusers);
            PrivateCacheData.Add("InactiveInternetLeadUsers", inactive_internetleadusers);

        }


        private void SetupInternetLeadUsers()
        {
            active_internetleadusers = new DataTable();
            active_internetleadusers.Columns.Add("Key", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("ADUserKey", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("ADUserName", Type.GetType("System.String"));
            active_internetleadusers.Columns.Add("Flag", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("CaseCount", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("GeneralStatus", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("LastCaseKey", Type.GetType("System.Int32"));
            active_internetleadusers.Columns.Add("Edited", Type.GetType("System.Int32"));
            active_internetleadusers.TableName = "internetleadusers";

            inactive_internetleadusers = new DataTable();
            inactive_internetleadusers.Columns.Add("Key", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("ADUserKey", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("ADUserName", Type.GetType("System.String"));
            inactive_internetleadusers.Columns.Add("Flag", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("CaseCount", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("GeneralStatus", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("LastCaseKey", Type.GetType("System.Int32"));
            inactive_internetleadusers.Columns.Add("Edited", Type.GetType("System.Int32"));
            inactive_internetleadusers.TableName = "internetleadusers";

        }

        private void EnsureRoundRobinFlag()
        {
            active_internetleadusers = PrivateCacheData["ActiveInternetLeadUsers"] as DataTable;
            inactive_internetleadusers = PrivateCacheData["InactiveInternetLeadUsers"] as DataTable;
            bool Flagfound = false;
            // check if it is in the inactive table
            if (inactive_internetleadusers != null)
                for (int i = 0; i < inactive_internetleadusers.Rows.Count; i++)
                    if (inactive_internetleadusers.Rows[i]["Flag"].ToString() == "1")
                    {
                        inactive_internetleadusers.Rows[i]["Flag"] = 0;
                        inactive_internetleadusers.Rows[i]["Edited"] = 1;
                    }

            // check if it doesnt exist
            if (active_internetleadusers != null)
            {
                for (int i = 0; i < active_internetleadusers.Rows.Count; i++)
                    if (active_internetleadusers.Rows[i]["Flag"].ToString() == "1")
                    {
                        Flagfound = true;
                        break;
                    }
                // there is no round Robin flag in the Active group 
                if (!Flagfound && active_internetleadusers.Rows.Count > 0)
                {
                    // Add a flag to the first record in the active group
                    active_internetleadusers.Rows[0]["Flag"] = 1;
                    active_internetleadusers.Rows[0]["Edited"] = 1;
                }
            }

            PrivateCacheData.Remove("ActiveInternetLeadUsers");
            PrivateCacheData.Remove("InactiveInternetLeadUsers");
            PrivateCacheData.Add("ActiveInternetLeadUsers", active_internetleadusers);
            PrivateCacheData.Add("InactiveInternetLeadUsers", inactive_internetleadusers);

        }

    }
}
