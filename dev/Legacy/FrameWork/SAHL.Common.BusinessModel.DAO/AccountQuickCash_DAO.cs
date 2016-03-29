using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "10", Lazy = true)]
    [GenericTest(Globals.TestType.Find)]
    public class AccountQuickCash_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountQuickCash_DAO Find(int id)
        {
            return ActiveRecordBase<AccountQuickCash_DAO>.Find(id).As<AccountQuickCash_DAO>();
        }

        public new static AccountQuickCash_DAO Find(object id)
        {
            return ActiveRecordBase<AccountQuickCash_DAO>.Find(id).As<AccountQuickCash_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountQuickCash_DAO FindFirst()
        {
            return ActiveRecordBase<AccountQuickCash_DAO>.FindFirst().As<AccountQuickCash_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountQuickCash_DAO FindOne()
        {
            return ActiveRecordBase<AccountQuickCash_DAO>.FindOne().As<AccountQuickCash_DAO>();
        }

        #endregion Static Overrides
    }
}