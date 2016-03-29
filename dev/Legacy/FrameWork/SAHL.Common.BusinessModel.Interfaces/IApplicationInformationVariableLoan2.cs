using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IApplicationInformationVariableLoan : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        Double? PropertyValuation
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? LoanAgreementAmount
        {
            get;
            //set;
        }

        /// <summary>
        /// The sum total of all the Fees applicable to the Application. Consists of fees such as the Cancellation Fee, an Initiation Fee
        /// and the Registration Fee.
        /// </summary>
        Double? FeesTotal
        {
            get;
            set;
        }

        /// <summary>
        /// The Loan Amount with No Fees
        /// </summary>
        Double? LoanAmountNoFees
        {
            get;
            set;
        }

        /// <summary>
        /// The percentage discount applied to the initiation fee
        /// </summary>
        Double? AppliedInitiationFeeDiscount
        {
            get;
            set;
        }
    }
}