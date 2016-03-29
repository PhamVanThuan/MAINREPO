using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAccountVariFixLoan : IEntityValidation, IAccount, IMortgageLoanAccount
    {
        IMortgageLoan FixedSecuredMortgageLoan
        {
            get;
        }
    }
}