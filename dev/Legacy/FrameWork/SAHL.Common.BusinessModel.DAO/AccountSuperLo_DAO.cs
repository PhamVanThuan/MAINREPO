using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Account_DAO and is instantiated to represent new Super Lo Loan Accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "5", Lazy = true)]
    public class AccountSuperLo_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountSuperLo_DAO Find(int id)
        {
            return ActiveRecordBase<AccountSuperLo_DAO>.Find(id).As<AccountSuperLo_DAO>();
        }

        public new static AccountSuperLo_DAO Find(object id)
        {
            return ActiveRecordBase<AccountSuperLo_DAO>.Find(id).As<AccountSuperLo_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountSuperLo_DAO FindFirst()
        {
            return ActiveRecordBase<AccountSuperLo_DAO>.FindFirst().As<AccountSuperLo_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountSuperLo_DAO FindOne()
        {
            return ActiveRecordBase<AccountSuperLo_DAO>.FindOne().As<AccountSuperLo_DAO>();
        }

        #endregion Static Overrides
    }
}