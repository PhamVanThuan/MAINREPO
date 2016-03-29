using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Web.Views.Life.Interfaces;
using System;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsUpdateApproved : DisabilityClaimDetailsUpdateBase
    {
        public DisabilityClaimDetailsUpdateApproved(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            _view.DateOfDiagnosisEditable = false;
            _view.LastDateWorkedEditable = false;
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            base.ValidateScreenInput();

            if (_view.IsValid == false)
                return;

            try
            {
                IServiceRequestMetadata metadata = new ServiceRequestMetadata();

                AmendApprovedDisabilityClaimCommand command = new AmendApprovedDisabilityClaimCommand(disabilityClaim.DisabilityClaimKey, _view.DisabilityTypeKey.Value, _view.OtherDisabilityComments, _view.ClaimantOccupation, disabilityClaim.LastDateWorked.Value, _view.ExpectedReturnToWorkDate);
                ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformCommand(command);

                if (systemMessageCollection.HasErrors)
                {
                    v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                }
                else
                {
                    _view.Navigator.Navigate("Submit");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }
    }
}