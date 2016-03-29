using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DisbursementInterestApplied", Schema = "dbo", Lazy = true)]
    public partial class DisbursementInterestApplied_DAO : DB_2AM<DisbursementInterestApplied_DAO>
    {
        private string _description;

        private int _Key;

        //commented, this is a lookup
        //private IList<AccountDebtSettlement_DAO> _accountDebtSettlements;

        [Property("Description", ColumnType = "String", Length = 10)]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "InterestAppliedTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        // commented, this is a lookup.
        //[HasMany(typeof(AccountDebtSettlement_DAO), ColumnKey = "InterestAppliedTypeKey", Table = "AccountDebtSettlement")]
        //public virtual IList<AccountDebtSettlement_DAO> AccountDebtSettlements
        //{
        //    get
        //    {
        //        return this._accountDebtSettlements;
        //    }
        //    set
        //    {
        //        this._accountDebtSettlements = value;
        //    }
        //}
    }
}