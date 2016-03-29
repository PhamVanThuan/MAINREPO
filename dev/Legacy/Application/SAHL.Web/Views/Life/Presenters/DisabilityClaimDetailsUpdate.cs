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
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Life.Interfaces;
using System;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsUpdate : DisabilityClaimDetailsUpdateBase
    {

        public DisabilityClaimDetailsUpdate(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
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

            if (_view.IsValid == false)
                return;

            try
            {
                CapturePendingDisabilityClaimCommand capturePendingDisabilityCommand = new CapturePendingDisabilityClaimCommand(disabilityClaim.DisabilityClaimKey, _view.DateOfDiagnosis.Value, _view.DisabilityTypeKey.Value, _view.OtherDisabilityComments, _view.ClaimantOccupation, _view.LastDateWorked.Value, _view.ExpectedReturnToWorkDate);
                ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformCommand(capturePendingDisabilityCommand);

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