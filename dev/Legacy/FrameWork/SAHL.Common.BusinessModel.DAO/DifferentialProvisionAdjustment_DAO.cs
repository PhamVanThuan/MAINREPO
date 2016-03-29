using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DifferentialProvisionAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class DifferentialProvisionAdjustment_DAO : DB_2AM<DifferentialProvisionAdjustment_DAO>
    {
        private double _differentialAdjustment;

        private BalanceType_DAO _balanceType;

        private TransactionType_DAO _transactionType;

        private int _key;

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialAdjustmentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        [Property("DifferentialAdjustment", ColumnType = "Double", NotNull = true)]
        public virtual double DifferentialAdjustment
        {
            get
            {
                return this._differentialAdjustment;
            }
            set
            {
                this._differentialAdjustment = value;
            }
        }

        [BelongsTo("BalanceTypeKey", NotNull = true)]
        [ValidateNonEmpty("Balance Type is a mandatory field")]
        public virtual BalanceType_DAO BalanceType
        {
            get
            {
                return this._balanceType;
            }
            set
            {
                this._balanceType = value;
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