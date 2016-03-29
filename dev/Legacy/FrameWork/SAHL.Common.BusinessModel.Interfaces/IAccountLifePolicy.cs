using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from Account_DAO. Instantiated to represent a lifepolicy account.
    /// </summary>
    public partial interface IAccountLifePolicy : IEntityValidation, IBusinessModelObject, IAccount
    {
        /// <summary>
        /// The Parent Mortgage Loan Account associated to the Life Policy.
        /// </summary>
        IAccount ParentMortgageLoan
        {
            get;
        }

        /// <summary>
        /// The Financial Service associated to the Life Policy
        /// </summary>
        ILifePolicy LifePolicyFinancialService
        {
            get;
        }

        /// <summary>
        /// Each of the Legal Entities playing a Role on a Life Policy Account require an Insurable Interest. This relationship is defined
        /// in the LifeInsurableInterest table, which relates the Account to the Legal Entity and the type of Insurable Interest that the Legal
        /// Entity has on the Account.
        /// </summary>
        IEventList<ILifeInsurableInterest> LifeInsurableInterests
        {
            get;
        }
    }
}