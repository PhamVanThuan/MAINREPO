using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimHistoryDetails : SAHLCommonBasePresenter<IDisabilityClaimHistoryDetails>
    {
        private CBOMenuNode cboNode;
        private IV3ServiceManager v3ServiceManager;
        private IMortgageLoanDomainService mortgageLoanDomainService;
        private ILifeDomainService lifeDomainService;


        public DisabilityClaimHistoryDetails(IDisabilityClaimHistoryDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            ISystemMessageCollection systemMessageCollection;

            GetDisabilityClaimHistoryForAccountQuery query = new GetDisabilityClaimHistoryForAccountQuery(cboNode.GenericKey);
            systemMessageCollection = lifeDomainService.PerformQuery(query);

            if (!systemMessageCollection.HasErrors)
            {
                _view.BindDisabilityClaimsHistoryGrid(query.Result.Results);
                _view.DisabilityClaims_OnSelectedIndexChanged += new KeyChangedEventHandler(_view_OnDisabilityClaimSelected);
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }

        protected void _view_OnDisabilityClaimSelected(object sender, KeyChangedEventArgs e)
        {
            _view.BindDisabilityClaim(_view.ListDisabilityClaimsHistory[_view.SelectedIndex]);                       
        }        
    }
}