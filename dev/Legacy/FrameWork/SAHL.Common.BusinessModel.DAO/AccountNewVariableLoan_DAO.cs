using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent new Variable Loan accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "9", Lazy = true)]
    public partial class AccountNewVariableLoan_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountNewVariableLoan_DAO Find(int id)
        {
            return ActiveRecordBase<AccountNewVariableLoan_DAO>.Find(id).As<AccountNewVariableLoan_DAO>();
        }

        public new static AccountNewVariableLoan_DAO Find(object id)
        {
            return ActiveRecordBase<AccountNewVariableLoan_DAO>.Find(id).As<AccountNewVariableLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountNewVariableLoan_DAO FindFirst()
        {
            return ActiveRecordBase<AccountNewVariableLoan_DAO>.FindFirst().As<AccountNewVariableLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountNewVariableLoan_DAO FindOne()
        {
            return ActiveRecordBase<AccountNewVariableLoan_DAO>.FindOne().As<AccountNewVariableLoan_DAO>();
        }

        #endregion Static Overrides
    }
}