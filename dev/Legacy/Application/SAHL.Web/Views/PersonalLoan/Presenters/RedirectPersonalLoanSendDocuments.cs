using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Configuration;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class RedirectPersonalLoanSendDocuments : SAHLCommonBasePresenter<IRedirect>
    {
        const string PendingDomiciliumFalse = "PendingDomiciliumFalse";
        const string PendingDomiciliumTrue = "PendingDomiciliumTrue";

        public RedirectPersonalLoanSendDocuments(IRedirect view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            CBONode CurrentNode = CBOManager.GetCurrentCBONode(base._view.CurrentPrincipal);
            var applicationRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
            var applicationKey = CurrentNode.GenericKey;
            var applicationUnsecuredLending =  applicationRepository.GetApplicationByKey(applicationKey);

            IReadOnlyEventList<ILegalEntityNaturalPerson> clients = applicationUnsecuredLending.GetNaturalPersonsByExternalRoleType(ExternalRoleTypes.Client, GeneralStatuses.Active);

            bool hasPendingDomiciliumForOffer = false;

            foreach (ILegalEntityNaturalPerson client in clients)
            {
                var externalRole = client.GetActiveClientExternalRoleForOffer(applicationUnsecuredLending.Key);

                if (externalRole.PendingExternalRoleDomicilium != null)
                {
                    hasPendingDomiciliumForOffer = true;
                }
            }

            var discriminationName = hasPendingDomiciliumForOffer ? PendingDomiciliumTrue : PendingDomiciliumFalse;

            SAHLRedirectionSection RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");
            if (RedirectionSection != null)
            {
                RedirectionElement Redirect = RedirectionSection.GetRedirection(discriminationName, base._view.ViewName);
                base._view.Navigator.Navigate(Redirect.NavigationView);
            }
        }

    }
}