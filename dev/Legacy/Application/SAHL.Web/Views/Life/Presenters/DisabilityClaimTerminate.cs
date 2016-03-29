using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters.CommonReason;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimTerminate : CommonReasonBase
    {
        private IV3ServiceManager v3ServiceManager;
        private ILifeDomainService lifeDomainService;

        public DisabilityClaimTerminate(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            v3ServiceManager = V3ServiceManager.Instance;
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            _view.OnlyOneReasonCanBeSelected = true;

            // get the instance node
            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

            base.GenericKey = node.GenericKey;
        }

        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base._view_OnSubmitButtonClicked(sender, e);

            try
            {
                ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
                TerminateDisabilityClaimCommand terminateDisabilityClaimCommand = new TerminateDisabilityClaimCommand(base.GenericKey, base.InsertedReasonKeys[0]);
                systemMessageCollection = lifeDomainService.PerformCommand(terminateDisabilityClaimCommand);
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    throw;
                }
            }

            if (_view.Messages.Count > 0)
                return;

            CompleteActivityAndNavigate();
        }

        public override void CancelActivity()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
            {
                Navigator.Navigate(GlobalCacheData[ViewConstants.CancelView].ToString());
                GlobalCacheData.Remove(ViewConstants.CancelView);
            }
            else
            {
                base.X2Service.CancelActivity(_view.CurrentPrincipal);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        public override void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            X2ServiceResponse response = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);

            if (response.IsError)
                throw new Exception("Error Completing Decline");

            if (base.sdsdgKeys.Count > 0)
            {
                // Update the reason with the StageTransitionKey
                UpdateReasonsWithStageTransitionKey();
            }

            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}