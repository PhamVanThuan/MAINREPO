using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseAndVariationsConditionsReadOnly : SAHLCommonBasePresenter<Interfaces.IReleaseAndVariationsConditions>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReleaseAndVariationsConditionsReadOnly(Interfaces.IReleaseAndVariationsConditions view, SAHLCommonBaseController controller)
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
            if (!View.ShouldRunPage)
                return;

            //TODO Need to create a presenter for a read only view
            // Setup the View Interface
            View.ShowbtnCancel = true;
            View.btnCancelClicked += View_btnCancelClicked;
            View.gridConditionsClicked += View_gridConditionsClicked;

            View.SetReadOnlytxtNotes = true;

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




        void View_btnCancelClicked(object sender, EventArgs e)
        {
                Navigator.Navigate("ReleaseAndVariationsSummaryReadOnly");

        }

        /// <summary>
        /// Runs when the Grid is clicked and the grid index changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void View_gridConditionsClicked(object sender, EventArgs e)
        {
            View.SettxtNotesText = View.GetgridConditionsText;
        }



    }
}
