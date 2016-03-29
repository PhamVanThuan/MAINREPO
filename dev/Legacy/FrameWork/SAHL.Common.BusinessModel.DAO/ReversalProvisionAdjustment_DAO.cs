using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReversalProvisionAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class ReversalProvisionAdjustment_DAO : DB_2AM<ReversalProvisionAdjustment_DAO>
    {
        private double _reversalPercentage;

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

        [Property("ReversalPercentage", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Reversal Percentage is a mandatory field")]
        public virtual double ReversalPercentage
        {
            get
            {
                return this._reversalPercentage;
            }
            set
            {
                this._reversalPercentage = value;
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