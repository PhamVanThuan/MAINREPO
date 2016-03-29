using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IAccountLifePolicy : IEntityValidation, IAccount
    {
        ILifePolicy LifePolicy
        {
            get;
        }

        /// <summary>
        /// Returns the parent MortgageLoan account. Will throw an exception if there is more than 1 parent account.
        /// </summary>
        IMortgageLoanAccount MortgageLoanAccount { get; }

        IApplicationLife CurrentLifeApplication { get; }
    }
}