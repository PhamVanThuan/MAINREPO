using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IApplicationMortgageLoanRefinance : IEntityValidation, IApplication, IApplicationMortgageLoan, IApplicationMortgageLoanWithCashOut, ISupportsQuickCashApplicationInformation
    {
        void SetProduct(ProductsRefinance NewProduct);
        double? CashOut { set; get; }

        double? InitiationFee { get; }//set;

        double? RegistrationFee { get; }//set;

        double? TotalFees { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Override"></param>
        void SetRegistrationFee(double? Amount, bool Override);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Override"></param>
        void SetInitiationFee(double? Amount, bool Override);

        /// <summary>
        /// Get or Set whether the client would like to capitalise any fees to the loan amount.
        /// </summary>
        bool CapitaliseFees { get; set; }
    }
}