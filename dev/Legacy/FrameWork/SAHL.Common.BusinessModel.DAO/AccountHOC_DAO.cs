using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent HOC accounts.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "3", Lazy = true)]
    public class AccountHOC_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountHOC_DAO Find(int id)
        {
            return ActiveRecordBase<AccountHOC_DAO>.Find(id).As<AccountHOC_DAO>();
        }

        public new static AccountHOC_DAO Find(object id)
        {
            return ActiveRecordBase<AccountHOC_DAO>.Find(id).As<AccountHOC_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountHOC_DAO FindFirst()
        {
            return ActiveRecordBase<AccountHOC_DAO>.FindFirst().As<AccountHOC_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountHOC_DAO FindOne()
        {
            return ActiveRecordBase<AccountHOC_DAO>.FindOne().As<AccountHOC_DAO>();
        }

        #endregion Static Overrides
    }
}