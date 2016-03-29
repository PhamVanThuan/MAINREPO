using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using System;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface IDisabilityClaimCreate : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        int PolicyNumber { get; set; }

        string PolicyType { set; }

        string PolicyStatus { set; }

        DateTime? DateOfAcceptance { get;  set; }

        DateTime? CommencementDate { set; }

        double CurrentSumAssured { set; }

        double ReassuredIPBAmount { set; }

        int LoanNumber { set; }

        string LoanStatus { set; }

        int LoanTerm { set; }

        double LoanAmount { set; }

        int DebitOrderDay { set; }

        double OutstandingBondAmount { set; }

        double BondInstalment { set; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="assuredLives"></param>
        void BindAssuredLivesGrid(IReadOnlyEventList<ILegalEntity> assuredLives);

        /// <summary>
        ///
        /// </summary>
        void BindClaimants(IReadOnlyEventList<ILegalEntity> claimants);

        /// <summary>
        /// The legalentitykey of the selected claimant
        /// </summary>
        int SelectedLegalEntityKey { get; set; }
    }
}