using SAHL.Common.CacheData;
using SAHL.Common.Configuration;
using SAHL.Common.DomainMessages;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsUpdateRedirect : SAHLCommonBasePresenter<IRedirect>
    {
        private IV3ServiceManager v3ServiceManager;
        private IMortgageLoanDomainService mortgageLoanDomainService;
        private ILifeDomainService lifeDomainService;


        public DisabilityClaimDetailsUpdateRedirect(IRedirect view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage) return;

            // get the disability claim key from the cbo
            CBONode cboNode = CBOManager.GetCurrentCBONode(base._view.CurrentPrincipal);

            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            GetDisabilityClaimStatusDescriptionQuery query = new GetDisabilityClaimStatusDescriptionQuery(cboNode.GenericKey);
            ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformQuery(query);

            if (!systemMessageCollection.HasErrors)
            {
                string disabilityClaimStatusDescription = query.Result.Results.FirstOrDefault();

                SAHLRedirectionSection RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");
                if (RedirectionSection != null)
                {
                    RedirectionElement Redirect = RedirectionSection.GetRedirection(disabilityClaimStatusDescription, base._view.ViewName);
                    base._view.Navigator.Navigate(Redirect.NavigationView);
                }
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }
    }
}