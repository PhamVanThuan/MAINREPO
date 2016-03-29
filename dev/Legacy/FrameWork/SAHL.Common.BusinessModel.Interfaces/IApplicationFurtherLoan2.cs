using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationFurtherLoan : IEntityValidation, IApplicationFurtherLending, IApplicationMortgageLoan, ISupportsQuickCashApplicationInformation
    {
        void SetProduct(IProduct product);

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
    }
}