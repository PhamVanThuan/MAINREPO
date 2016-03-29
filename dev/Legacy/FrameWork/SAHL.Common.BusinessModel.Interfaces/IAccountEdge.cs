using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent new Edge accounts.
    /// </summary>
    public partial interface IAccountEdge : IEntityValidation, IBusinessModelObject, IAccount
    {
    }
}