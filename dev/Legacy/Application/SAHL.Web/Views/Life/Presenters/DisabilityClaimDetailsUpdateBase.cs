using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsUpdateBase : DisabilityClaimDetailsBase
    {
        public DisabilityClaimDetailsUpdateBase(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.DateOfDiagnosisEditable = true;
            _view.DisabilityTypeEditable = true;
            _view.AdditionalDisabilityDetailsEditable = true;
            _view.OccupationEditable = true;
            _view.LastDateWorkedEditable = true;
            _view.ExpectedReturnToWorkDateEditable = true;
            _view.UpdateButtonsVisible = true;
        }

        public void ValidateScreenInput()
        {
            string errorMessage = "";

            if (!_view.DateOfDiagnosis.HasValue &&
                disabilityClaim.DisabilityClaimStatusKey != (int)SAHL.Common.Globals.DisabilityClaimStatuses.Approved)
            {
                errorMessage = "Date of Diagnosis must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (!_view.DisabilityTypeKey.HasValue)
            {
                errorMessage = "Nature of the Disability must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
            else if (_view.DisabilityTypeKey.Value == (int)DisabilityTypes.Other &&
                string.IsNullOrWhiteSpace(_view.OtherDisabilityComments))
            {
                errorMessage = "Additional Details of the Disability must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (string.IsNullOrWhiteSpace(_view.ClaimantOccupation))
            {
                errorMessage = "Occupation must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (!_view.LastDateWorked.HasValue &&
                disabilityClaim.DisabilityClaimStatusKey != (int)SAHL.Common.Globals.DisabilityClaimStatuses.Approved)
            {
                errorMessage = "Last Date Worked must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }
    }
}