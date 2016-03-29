using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters.LegalEntityDetails;
using System;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    public class AffordabilityAssessmentLegalEntityDetailsAddRelateExisting : LegalEntityDetailsAddRelateExisting
    {
        public AffordabilityAssessmentLegalEntityDetailsAddRelateExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                Navigator.Navigate("Cancel");
            }
            if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.AffordabilityAssessment)
            {
                Navigator.Navigate("LinkIncomeContributors");
            }
        }
    }
}