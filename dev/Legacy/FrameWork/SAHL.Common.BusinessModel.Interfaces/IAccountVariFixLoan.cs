using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from account. Instantiated to represent VariFix Loan Accounts.
    /// </summary>
    public partial interface IAccountVariFixLoan : IEntityValidation, IBusinessModelObject, IAccount
    {
    }
}