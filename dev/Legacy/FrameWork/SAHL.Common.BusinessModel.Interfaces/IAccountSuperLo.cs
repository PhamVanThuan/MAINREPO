using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Account_DAO and is instantiated to represent new Super Lo Loan Accounts.
    /// </summary>
    public partial interface IAccountSuperLo : IEntityValidation, IBusinessModelObject, IAccount
    {
    }
}