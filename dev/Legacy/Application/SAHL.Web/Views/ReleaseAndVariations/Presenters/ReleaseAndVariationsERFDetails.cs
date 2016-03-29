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
    public class ReleaseAndVariationsERFDetails : SAHLCommonBasePresenter<Interfaces.IReleaseAndVariationsERFDetails>
    {
        private DataSet RVDS;
        //private int accountkey;
       // private IApplicationRepository applicationRepository;
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

        public ReleaseAndVariationsERFDetails(Interfaces.IReleaseAndVariationsERFDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            View.btnCancelClicked += View_btnCancelClicked;
            View.btnUpdateClicked += View_btnUpdateClicked;
            View.ShowbtnCancel = true;
            View.ShowbtnUpdate = true;
            // Setup the View Interface
            SetupInterface();

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


        private void SetupInterface()
        {
            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            if (RVDS.Tables["ErfExistingSecurity"].Rows.Count > 0)
            {
                View.SettxtExistingSecurity = RVDS.Tables["ErfExistingSecurity"].Rows[0]["Notes"].ToString();
                View.SettxtExistingSecurityEXT = RVDS.Tables["ErfExistingSecurity"].Rows[0]["EXT"].ToString();
                View.SettxtExistingSecurityValuation = Convert.ToString(RVDS.Tables["ErfExistingSecurity"].Rows[0]["Valuation"]);

            }
            if (RVDS.Tables["ErfRemainingSecurity"].Rows.Count > 0)
            {
                View.SettxtRemainingSecurity = RVDS.Tables["ErfRemainingSecurity"].Rows[0]["Notes"].ToString();
                View.SettxtRemainingSecurityExt = RVDS.Tables["ErfRemainingSecurity"].Rows[0]["EXT"].ToString();
                View.SettxtRemainingSecurityValuation = Convert.ToString(RVDS.Tables["ErfRemainingSecurity"].Rows[0]["Valuation"]);
            }
            if (RVDS.Tables["ErfToBeReleased"].Rows.Count > 0)
            {
                View.SettxtToBeReleased = RVDS.Tables["ErfToBeReleased"].Rows[0]["Notes"].ToString();
                View.SettxtToBeReleasedExt = RVDS.Tables["ErfToBeReleased"].Rows[0]["EXT"].ToString();
                View.SettxtToBeReleasedValuation = Convert.ToString(RVDS.Tables["ErfToBeReleased"].Rows[0]["Valuation"]);
            }
        }


        void View_btnUpdateClicked(object sender, EventArgs e)
        {
            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            TransactionScope txn = new TransactionScope();
            try
            {
                // Add a New record, and get back the New Memo Key - for adding to the dataset
                int NewMemoKey = Convert.ToInt32(RVDS.Tables["Release"].Rows[0]["MemoKey"]);

                if (RVDS.Tables["ErfExistingSecurity"].Rows.Count > 0)
                {
                    // Edit
                    RVDS.Tables["ErfExistingSecurity"].Rows[0]["Notes"] = View.SettxtExistingSecurity;
                    RVDS.Tables["ErfExistingSecurity"].Rows[0]["EXT"] = View.SettxtExistingSecurityEXT;
                    if (!(View.SettxtExistingSecurityValuation.Length == 0))
                        RVDS.Tables["ErfExistingSecurity"].Rows[0]["Valuation"] = Convert.ToDouble(View.SettxtExistingSecurityValuation);
                }
                else
                {
                    // Add
                    DataRow dr = RVDS.Tables["ErfExistingSecurity"].NewRow();
                    dr["ExistingKey"] = RVDS.Tables["ErfExistingSecurity"].Rows.Count + 1;
                    dr["MemoKey"] = NewMemoKey;
                    dr["Notes"] = View.SettxtExistingSecurity;
                    dr["EXT"] = View.SettxtExistingSecurityEXT;
                    if (!(View.SettxtExistingSecurityValuation.Length == 0))
                        dr["Valuation"] = Convert.ToDouble(View.SettxtExistingSecurityValuation);
                    else
                        dr["Valuation"] = 0;
                    RVDS.Tables["ErfExistingSecurity"].Rows.Add(dr);
                }

                if (RVDS.Tables["ErfRemainingSecurity"].Rows.Count > 0)
                {
                    // Edit
                    RVDS.Tables["ErfRemainingSecurity"].Rows[0]["Notes"] = View.SettxtRemainingSecurity;
                    RVDS.Tables["ErfRemainingSecurity"].Rows[0]["EXT"] = View.SettxtRemainingSecurityExt;
                    if (!(View.SettxtRemainingSecurityValuation.Length == 0))
                        RVDS.Tables["ErfRemainingSecurity"].Rows[0]["Valuation"] = Convert.ToDouble(View.SettxtRemainingSecurityValuation);
                }
                else
                {
                    //Add
                    DataRow dr = RVDS.Tables["ErfRemainingSecurity"].NewRow();
                    dr["RemainingKey"] = RVDS.Tables["ErfRemainingSecurity"].Rows.Count + 1;
                    dr["MemoKey"] = NewMemoKey;
                    dr["Notes"] = View.SettxtRemainingSecurity;
                    dr["EXT"] = View.SettxtRemainingSecurityExt;
                    if (!(View.SettxtRemainingSecurityValuation.Length == 0))
                        dr["Valuation"] = Convert.ToDouble(View.SettxtRemainingSecurityValuation);
                   else
                        dr["Valuation"] = 0;
                    RVDS.Tables["ErfRemainingSecurity"].Rows.Add(dr);
                }

                if (RVDS.Tables["ErfToBeReleased"].Rows.Count > 0)
                {
                    //Edit
                    RVDS.Tables["ErfToBeReleased"].Rows[0]["Notes"] = View.SettxtToBeReleased;
                    RVDS.Tables["ErfToBeReleased"].Rows[0]["EXT"] = View.SettxtToBeReleasedExt;
                    if (!(View.SettxtToBeReleasedValuation.Length == 0))
                        RVDS.Tables["ErfToBeReleased"].Rows[0]["Valuation"] = Convert.ToDouble(View.SettxtToBeReleasedValuation);

                }
                else
                {
                    //ADD
                    DataRow dr = RVDS.Tables["ErfToBeReleased"].NewRow();
                    dr["ReleaseKey"] = RVDS.Tables["ErfToBeReleased"].Rows.Count + 1;
                    dr["MemoKey"] = NewMemoKey;
                    dr["Notes"] = View.SettxtToBeReleased;
                    dr["EXT"] = View.SettxtToBeReleasedExt;
                    if (!(View.SettxtToBeReleasedValuation.Length == 0))
                        dr["Valuation"] = Convert.ToDouble(View.SettxtToBeReleasedValuation);
                    else
                        dr["Valuation"] = 0;
                    RVDS.Tables["ErfToBeReleased"].Rows.Add(dr);

                }
                
                RVDS.AcceptChanges();
                ReleaseAndVariationsRepository.UpdateMemo(NewMemoKey, RVDS);
                GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
                GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
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

            Navigator.Navigate("Cancel");
        }

        void View_btnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

    }
}
