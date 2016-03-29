using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAccountEdge : IEntityValidation, IAccount, IMortgageLoanAccount
    {
    }
}