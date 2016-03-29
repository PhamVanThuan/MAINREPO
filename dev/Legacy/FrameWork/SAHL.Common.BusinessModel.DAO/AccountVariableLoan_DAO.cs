using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent Variable Loan accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "1", Lazy = true)]
    public partial class AccountVariableLoan_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariableLoan_DAO Find(int id)
        {
            return ActiveRecordBase<AccountVariableLoan_DAO>.Find(id).As<AccountVariableLoan_DAO>();
        }

        public new static AccountVariableLoan_DAO Find(object id)
        {
            return ActiveRecordBase<AccountVariableLoan_DAO>.Find(id).As<AccountVariableLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariableLoan_DAO FindFirst()
        {
            return ActiveRecordBase<AccountVariableLoan_DAO>.FindFirst().As<AccountVariableLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountVariableLoan_DAO FindOne()
        {
            return ActiveRecordBase<AccountVariableLoan_DAO>.FindOne().As<AccountVariableLoan_DAO>();
        }

        #endregion Static Overrides
    }
}