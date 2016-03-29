using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ReleaseAndVariations.Interfaces;

namespace SAHL.Web.Views.ReleaseAndVariations.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseAndVariationsERFDetailsReadOnly : SAHLCommonBasePresenter<IReleaseAndVariationsERFDetails>
    {
        private DataSet RVDS;
        //private int accountkey;
        //private IApplicationRepository applicationRepository;
        //private IReleaseAndVariationsRepository releaseAndVariationsRepository;


        //private IApplicationRepository ApplicationRepository
        //{
        //    get
        //    {
        //        if (applicationRepository == null)
        //            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        //        return applicationRepository;
        //    }
        //}

        //private IReleaseAndVariationsRepository ReleaseAndVariationsRepository
        //{
        //    get
        //    {
        //        if (releaseAndVariationsRepository == null)
        //            releaseAndVariationsRepository = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
        //        return releaseAndVariationsRepository;
        //    }
        //}

        public ReleaseAndVariationsERFDetailsReadOnly(IReleaseAndVariationsERFDetails view, SAHLCommonBaseController controller)
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
            View.ShowbtnCancel = true;
            View.ShowbtnUpdate = false;

            SetupInterface();

            View.SetReadOnlytxtExistingSecurity = true;
            View.SetReadOnlytxtExistingSecurityEXT = true;
            View.SetReadOnlytxtExistingSecurityValuation = true;
            View.SetReadOnlytxtToBeReleased = true;
            View.SetReadOnlytxtToBeReleasedExt = true;
            View.SetReadOnlytxtToBeReleasedValuation = true;
            View.SetReadOnlytxtRemainingSecurity = true;
            View.SetReadOnlytxtRemainingSecurityExt = true;
            View.SetReadOnlytxtRemainingSecurityValuation = true;

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


        void View_btnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ReleaseAndVariationsSummaryReadOnly");
        }

    }
}
