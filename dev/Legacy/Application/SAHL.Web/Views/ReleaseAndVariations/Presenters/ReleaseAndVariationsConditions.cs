using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseAndVariationsConditions : SAHLCommonBasePresenter<Interfaces.IReleaseAndVariationsConditions>
    {
        private DataSet RVDS;
        //private int accountkey;
        //private IApplicationRepository applicationRepository;
        private IReleaseAndVariationsRepository releaseAndVariationsRepository;


        //private IApplicationRepository ApplicationRepository
        //{
        //    get
        //    {
        //        if (applicationRepository == null)
        //            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        //        return applicationRepository;
        //    }
        //}

        private IReleaseAndVariationsRepository ReleaseAndVariationsRepository
        {
            get
            {
                if (releaseAndVariationsRepository == null)
                    releaseAndVariationsRepository = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                return releaseAndVariationsRepository;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReleaseAndVariationsConditions(Interfaces.IReleaseAndVariationsConditions view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            if (!PrivateCacheData.ContainsKey("add"))
                PrivateCacheData.Add("add", false);

            // Setup the View Interface
            View.ShowbtnAdd = true;
            View.ShowbtnCancel = true;
            View.ShowbtnUpdate = false;

            View.btnAddClicked += View_btnAddClicked;
            View.btnCancelClicked += View_btnCancelClicked;
            View.btnUpdateClicked += View_btnUpdateClicked;
            View.gridConditionsClicked += View_gridConditionsClicked;

            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            View.BindConditionGrid(RVDS.Tables["Conditions"]);

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

        }


        void View_btnAddClicked(object sender, EventArgs e)
        {
            PrivateCacheData.Remove("add");
            PrivateCacheData.Add("add", true);

            View.SettxtNotesText = "";
            // Set the gridindex to -1
            View.ShowbtnUpdate = true;
            View.ShowbtnAdd = false;
        }

        void View_btnUpdateClicked(object sender, EventArgs e)
        {
            bool Adding = Convert.ToBoolean(PrivateCacheData["add"]);
            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            TransactionScope txn = new TransactionScope();
            try
            {
                // Add a New record, and get back the New Memo Key - for adding to the dataset
                int NewMemoKey = Convert.ToInt32(RVDS.Tables["Release"].Rows[0]["MemoKey"]);
                if (Adding)
                {
                    DataRow dr = RVDS.Tables["Conditions"].NewRow();
                    dr["ConditionKey"] = RVDS.Tables["Conditions"].Rows.Count + 1;
                    dr["MemoKey"] = NewMemoKey;
                    dr["Condition"] = View.SettxtNotesText;
                    RVDS.Tables["Conditions"].Rows.Add(dr);
                    RVDS.AcceptChanges();
                }
                else
                {
                    // Grid index will correlate exactly to the row index in the table
                    int Gridindex = View.SetgridConditionsIndex;
                    RVDS.Tables["Conditions"].Rows[Gridindex]["Condition"] = View.SettxtNotesText;
                    RVDS.AcceptChanges();
                }

                ReleaseAndVariationsRepository.UpdateMemo(NewMemoKey, RVDS);
                GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
                GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
                txn.VoteCommit();
                View.BindConditionGrid(RVDS.Tables["Conditions"]);
                View.SettxtNotesText = "";
                View.ShowbtnUpdate = false;
                View.ShowbtnAdd = true;

                PrivateCacheData.Remove("add");
                PrivateCacheData.Add("add", false);

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

        void View_btnCancelClicked(object sender, EventArgs e)
        {
            bool Adding = false;
            if (PrivateCacheData.ContainsKey("add"))
                Adding = Convert.ToBoolean(PrivateCacheData["add"]);
            if (!Adding)
            {
                PrivateCacheData.Remove("add");
                Navigator.Navigate("ReleaseAndVariationsSummary");
            }
            else
            {
                View.ShowbtnAdd = true;
                View.ShowbtnUpdate = false;
                View.SettxtNotesText = "";

                PrivateCacheData.Remove("add");
                PrivateCacheData.Add("add", false);
            }
        }


        /// <summary>
        /// Runs when the Grid is clicked and the grid index changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void View_gridConditionsClicked(object sender, EventArgs e)
        {
            View.SettxtNotesText = View.GetgridConditionsText;
            View.ShowbtnUpdate = true;
            View.ShowbtnAdd = true;

            PrivateCacheData.Remove("add");
            PrivateCacheData.Add("add", false);
        }



    }
}
