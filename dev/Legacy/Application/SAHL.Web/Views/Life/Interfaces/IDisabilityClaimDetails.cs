using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface IDisabilityClaimDetails : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="disabilityTypes"></param>
        void BindDisabilityTypes(IDictionary<int, string> disabilityTypes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="disabilityClaim"></param>
        void BindDisabilityClaim(DisabilityClaimDetailModel disabilityClaim); 

        bool DateOfDiagnosisEditable { set; }
        bool DisabilityTypeEditable { set; }
        bool AdditionalDisabilityDetailsEditable { set; }
        bool OccupationEditable { set; }
        bool LastDateWorkedEditable { set; }
        bool ExpectedReturnToWorkDateEditable { set; }
        bool UpdateButtonsVisible { set; }

        DateTime? LastDateWorked { get; }

        DateTime? DateOfDiagnosis { get; }

        string ClaimantOccupation { get; }

        int? DisabilityTypeKey { get; }

        string OtherDisabilityComments { get; }

        DateTime? ExpectedReturnToWorkDate { get; }

        void BindPaymentDetails(string approvedByUser, DateTime approvedDate, int numberOfPayments, DateTime paymentStartDate, DateTime paymentEndDate);
    }
}