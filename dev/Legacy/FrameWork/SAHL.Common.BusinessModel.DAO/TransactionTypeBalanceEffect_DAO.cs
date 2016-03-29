using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("TransactionTypeBalanceEffect", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class TransactionTypeBalanceEffect_DAO : DB_2AM<TransactionTypeBalanceEffect_DAO>
    {
        private int _key;

        private TransactionType_DAO _transactionType;

        private BalanceType_DAO _balanceType;

        private TransactionEffect_DAO _transactionEffect;

        private TransactionEffect_DAO _SPVTransactionEffect;

        private TransactionTypeBalanceEffect_DAO _parentTransactionTypeBalanceEffectKey;

        [PrimaryKey(PrimaryKeyType.Assigned, "TransactionTypeBalanceEffectKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
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

        [BelongsTo("TransactionEffectKey", NotNull = true)]
        [ValidateNonEmpty("Transaction Effect is a mandatory field")]
        public virtual TransactionEffect_DAO TransactionEffect
        {
            get
            {
                return this._transactionEffect;
            }
            set
            {
                this._transactionEffect = value;
            }
        }

        [BelongsTo("SPVTransactionEffectKey", NotNull = true)]
        [ValidateNonEmpty("SPV Transaction Effect is a mandatory field")]
        public virtual TransactionEffect_DAO SPVTransactionEffect
        {
            get
            {
                return this._SPVTransactionEffect;
            }
            set
            {
                this._SPVTransactionEffect = value;
            }
        }

        [BelongsTo("ParentTransactionTypeBalanceEffectKey")]
        public virtual TransactionTypeBalanceEffect_DAO ParentTransactionTypeBalanceEffectKey
        {
            get
            {
                return this._parentTransactionTypeBalanceEffectKey;
            }
            set
            {
                this._parentTransactionTypeBalanceEffectKey = value;
            }
        }
    }
}