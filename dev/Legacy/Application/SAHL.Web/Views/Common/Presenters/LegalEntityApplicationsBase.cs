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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityApplicationsBase : SAHLCommonBasePresenter<ILegalEntityApplications>
    {
        public IEventList<IApplication> ApplicationsList;
        protected IApplicationRepository _applicaitonRepository;
        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicaitonRepository == null)
                    _applicaitonRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicaitonRepository;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityApplicationsBase(ILegalEntityApplications view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnSelectButtonClicked += new KeyChangedEventHandler(OnSelectButtonClicked);

            // bind the data
            _view.BindApplicationsGrid(ApplicationsList);
        }

        void OnSelectButtonClicked(object sender, KeyChangedEventArgs e)
        {
            if (e == null)
                return;

            int applicationKey = Convert.ToInt32(e.Key);
            if (applicationKey > 0)
            {
                // If the application is RCS then dont show detail screen.
                IApplication application = ApplicationRepository.GetApplicationByKey(applicationKey);
                if (application.Account != null && application.Account.OriginationSourceProduct != null
                && application.Account.OriginationSourceProduct.OriginationSource.Key == (int)SAHL.Common.Globals.OriginationSources.RCS)
                    _view.Messages.Add(new Error("You have selected an RCS application, details cannot be displayed.", "You have selected an RCS application, details cannot be displayed."));

                if (_view.IsValid)
                {
                    // add the applicationkey to the global cache
                    GlobalCacheData.Remove(ViewConstants.ApplicationKey);
                    GlobalCacheData.Add(ViewConstants.ApplicationKey, applicationKey, new List<ICacheObjectLifeTime>());

                    // navigate to the application details view - this will be controlled by th redirect view
                    _view.Navigator.Navigate("Submit");
                }
            }
        }



    }
}

