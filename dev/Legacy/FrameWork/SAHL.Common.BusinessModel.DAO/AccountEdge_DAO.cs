using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent new Edge accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "11", Lazy = true)]
    public partial class AccountEdge_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountEdge_DAO Find(int id)
        {
            return ActiveRecordBase<AccountEdge_DAO>.Find(id).As<AccountEdge_DAO>();
        }

        public new static AccountEdge_DAO Find(object id)
        {
            return ActiveRecordBase<AccountEdge_DAO>.Find(id).As<AccountEdge_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountEdge_DAO FindFirst()
        {
            return ActiveRecordBase<AccountEdge_DAO>.FindFirst().As<AccountEdge_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountEdge_DAO FindOne()
        {
            return ActiveRecordBase<AccountEdge_DAO>.FindOne().As<AccountEdge_DAO>();
        }

        #endregion Static Overrides
    }
}