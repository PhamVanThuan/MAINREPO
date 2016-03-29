using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAccountSuperLo : IEntityValidation, IAccount, IMortgageLoanAccount
    {
        /// <summary>
        ///
        /// </summary>
        ISuperLo SuperLo { get; }
    }
}