using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.CreditProtectionPlan.Interfaces
{
    public interface ILifePolicyClaim : IViewBase
    {
        void SetupControls(bool display, bool adding = false);

        void BindLifePolicyClaimGrid(IList<SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim> lifePolicyClaims);

        void BindLifePolicyClaimFields(SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim lifePolicyClaim, bool display, bool adding);

        void BindClaimTypes(IDictionary<int, string> claimTypes);

        void BindClaimStatuses(IDictionary<int, string> claimStatuses);


        event EventHandler OnCancelButtonClicked;

        event EventHandler OnSubmitButtonClicked;

        event KeyChangedEventHandler OnLifePolicyClaimGridSelectedIndexChanged;


        GridPostBackType LifePolicyClaimGrid_PostBackType { set; }

        bool ButtonRow_visability { set; }

        int LifePolicyClaimKey { get; }

        int ClaimTypeKey { get; }

        int ClaimStatusKey { get; }

        DateTime? ClaimDate { get; }
    }
}
