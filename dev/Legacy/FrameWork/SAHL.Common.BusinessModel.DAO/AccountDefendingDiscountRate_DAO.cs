using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Account_DAO and is used to instantiate a Defending Discount Rate Account.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "6", Lazy = true)]
    public class AccountDefendingDiscountRate_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountDefendingDiscountRate_DAO Find(int id)
        {
            return ActiveRecordBase<AccountDefendingDiscountRate_DAO>.Find(id).As<AccountDefendingDiscountRate_DAO>();
        }

        public new static AccountDefendingDiscountRate_DAO Find(object id)
        {
            return ActiveRecordBase<AccountDefendingDiscountRate_DAO>.Find(id).As<AccountDefendingDiscountRate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountDefendingDiscountRate_DAO FindFirst()
        {
            return ActiveRecordBase<AccountDefendingDiscountRate_DAO>.FindFirst().As<AccountDefendingDiscountRate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountDefendingDiscountRate_DAO FindOne()
        {
            return ActiveRecordBase<AccountDefendingDiscountRate_DAO>.FindOne().As<AccountDefendingDiscountRate_DAO>();
        }

        #endregion Static Overrides
    }
}