using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FixedRateAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FixedRateAdjustment_DAO : DB_2AM<FixedRateAdjustment_DAO>
    {
        private double _rate;

        private TransactionType_DAO _transactionType;

        private int _Key;

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialAdjustmentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        [Property("Rate", ColumnType = "Double", NotNull = true)]
        public virtual double Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
            }
        }

        [BelongsTo("TransactionTypeKey", NotNull = true)]
        [ValidateNonEmpty("Transaction Type is a mandatory field")]
        public virtual TransactionType_DAO TransactionType
        {
            get
            {
                return this._transactionType;
            }
            set
            {
                this._transactionType = value;
            }
        }
    }
}