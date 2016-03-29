using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Web.Views.Life.Interfaces;
using System;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsCapture : DisabilityClaimDetailsUpdateBase
    {
        public DisabilityClaimDetailsCapture(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            base.ValidateScreenInput();

            if (!_view.IsValid)
                return;

            try
            {
                IServiceRequestMetadata metadata = new ServiceRequestMetadata();

                CapturePendingDisabilityClaimCommand capturePendingDisabilityCommand = new CapturePendingDisabilityClaimCommand(disabilityClaim.DisabilityClaimKey, _view.DateOfDiagnosis.Value, _view.DisabilityTypeKey.Value, _view.OtherDisabilityComments, _view.ClaimantOccupation, _view.LastDateWorked.Value, _view.ExpectedReturnToWorkDate);
                ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformCommand(capturePendingDisabilityCommand);

                if (systemMessageCollection.HasErrors)
                {
                    v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                }
                else
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    X2ServiceResponse response = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);

                    if (response.IsError)
                        throw new Exception("Error Completing Capture Details");

                    if (_view.IsValid)
                        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    base.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}