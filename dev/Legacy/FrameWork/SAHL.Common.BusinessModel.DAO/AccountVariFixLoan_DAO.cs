using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from account. Instantiated to represent VariFix Loan Accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "2", Lazy = true)]
    public class AccountVariFixLoan_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariFixLoan_DAO Find(int id)
        {
            return ActiveRecordBase<AccountVariFixLoan_DAO>.Find(id).As<AccountVariFixLoan_DAO>();
        }

        public new static AccountVariFixLoan_DAO Find(object id)
        {
            return ActiveRecordBase<AccountVariFixLoan_DAO>.Find(id).As<AccountVariFixLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariFixLoan_DAO FindFirst()
        {
            return ActiveRecordBase<AccountVariFixLoan_DAO>.FindFirst().As<AccountVariFixLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariFixLoan_DAO FindOne()
        {
            return ActiveRecordBase<AccountVariFixLoan_DAO>.FindOne().As<AccountVariFixLoan_DAO>();
        }

        #endregion Static Overrides
    }
}