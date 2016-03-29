using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface IDisabilityClaimApprove : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        int LoanDebitOrderDay { set; }

        DateTime DateLastWorked {set; }

        DateTime DisbilityPaymentStartDate { set; }

        DateTime? DisbilityPaymentEndDate { get; }

        int? NoOfInstalmentsAuthorised { get; }

        void BindFurtherLendingExclusions(IEnumerable<DisabilityClaimFurtherLendingExclusionModel> furtherLendingExclusions);
    }
}