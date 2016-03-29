using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Subsidy Provider Base
    /// </summary>
    public class SubsidyProviderBase : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.ISubsidyProvider>
    {
        /// <summary>
        /// Employment Repository
        /// </summary>
        protected IEmploymentRepository empRepo;
        /// <summary>
        /// Look Ups repository
        /// </summary>
        protected ILookupRepository lookups;
        /// <summary>
        /// Susbidy Provider
        /// </summary>
        protected SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProvider;
        /// <summary>
        /// Selected Subsidy Provider - added to private cache
        /// </summary>
        protected const string SubsidyProviderAddress = "SelectedSusbidyProviderAddress";
        /// <summary>
        /// Selected Subsidy - added to private cache
        /// </summary>
        protected const string SelectedSubsidyProvider = "SelectedSusbidy";
        /// <summary>
        /// Address 
        /// </summary>
        protected IAddress address;
        /// <summary>
        /// Address Repository
        /// </summary>
        protected IAddressRepository addressRepository;
    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SubsidyProviderBase(SAHL.Web.Views.Administration.Interfaces.ISubsidyProvider view, SAHLCommonBaseController controller)
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

            addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();

            empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();
            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("SubsidyProviderDetails");
        }
    
    }
}
