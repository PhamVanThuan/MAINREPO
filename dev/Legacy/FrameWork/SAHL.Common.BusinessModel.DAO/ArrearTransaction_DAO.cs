using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ArrearTransaction", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class ArrearTransaction_DAO : DB_2AM<ArrearTransaction_DAO>
    {
        private int _key;

        private System.DateTime _insertDate;

        private System.DateTime _effectiveDate;

        private System.DateTime? _correctionDate;

        private double _amount;

        private double _balance;

        private string _reference;

        private string _userid;

        private bool _isRolledBack;

        private FinancialService_DAO _financialService;

        private TransactionType_DAO _transactionType;

        [PrimaryKey(PrimaryKeyType.Native, "ArrearTransactionKey", ColumnType = "Int32")]
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

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("CorrectionDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CorrectionDate
        {
            get
            {
                return this._correctionDate;
            }
            set
            {
                this._correctionDate = value;
            }
        }

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [Property("Balance", ColumnType = "Double", NotNull = true)]
        public virtual double Balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }

        [Property("Reference", ColumnType = "String")]
        public virtual string Reference
        {
            get
            {
                return this._reference;
            }
            set
            {
                this._reference = value;
            }
        }

        [Property("Userid", ColumnType = "String", NotNull = true)]
        public virtual string Userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        [Property("IsRolledBack", ColumnType = "Boolean", NotNull = true)]
        public virtual bool IsRolledBack
        {
            get
            {
                return this._isRolledBack;
            }
            set
            {
                this._isRolledBack = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service is a mandatory field")]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
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