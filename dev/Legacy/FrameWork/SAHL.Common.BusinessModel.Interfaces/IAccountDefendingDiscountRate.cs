using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Account_DAO and is used to instantiate a Defending Discount Rate Account.
    /// </summary>
    public partial interface IAccountDefendingDiscountRate : IEntityValidation, IBusinessModelObject, IAccount
    {
    }
}