using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent Variable Loan accounts.
    /// </summary>
    public partial interface IAccountVariableLoan : IEntityValidation, IBusinessModelObject, IAccount
    {
    }
}