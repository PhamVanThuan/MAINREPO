using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This class interacts with the FinancialTransaction view in 2AM that points to the table of the same name in SAHLDB.
    ///
    /// NB: This object should NOT be queried with HQL, it will fall over spectacularly
    ///     The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
    /// </summary>
    //[DoNotTestWithGenericTest()]
    [GenericTest(TestType.Find)]
    [ActiveRecord("FinancialTransaction", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FinancialTransaction_DAO : DB_2AM<FinancialTransaction_DAO>
    {
        private int _key;

        private FinancialService_DAO _financialService;

        private TransactionType_DAO _transactionType;

        private System.DateTime _InsertDate;

        private System.DateTime _EffectiveDate;

        private System.DateTime _CorrectionDate;

        private float? _InterestRate;

        private double? _ActiveMarketRate;

        private double _Amount;

        private double _Balance;

        private string _Reference;

        private string _UserID;

        private SPV_DAO _SPV;

        private bool _IsRolledBack;

        [PrimaryKey(PrimaryKeyType.Native, "FinancialTransactionKey")]
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

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._InsertDate;
            }
            set
            {
                this._InsertDate = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._EffectiveDate;
            }
            set
            {
                this._EffectiveDate = value;
            }
        }

        [Property("CorrectionDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Correction Date is a mandatory field")]
        public virtual System.DateTime CorrectionDate
        {
            get
            {
                return this._CorrectionDate;
            }
            set
            {
                this._CorrectionDate = value;
            }
        }

        [Property("InterestRate", ColumnType = "Single")]
        public virtual float? InterestRate
        {
            get
            {
                return this._InterestRate;
            }
            set
            {
                this._InterestRate = value;
            }
        }

        [Property("ActiveMarketRate", ColumnType = "Double")]
        public virtual double? ActiveMarketRate
        {
            get
            {
                return this._ActiveMarketRate;
            }
            set
            {
                this._ActiveMarketRate = value;
            }
        }

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Amount is a mandatory field")]
        public virtual double Amount
        {
            get
            {
                return this._Amount;
            }
            set
            {
                this._Amount = value;
            }
        }

        [Property("Balance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Balance is a mandatory field")]
        public virtual double Balance
        {
            get
            {
                return this._Balance;
            }
            set
            {
                this._Balance = value;
            }
        }

        [Property("Reference", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Reference is a mandatory field")]
        public virtual string Reference
        {
            get
            {
                return this._Reference;
            }
            set
            {
                this._Reference = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("UserID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                this._UserID = value;
            }
        }

        [BelongsTo("SPVKey", NotNull = true)]
        [ValidateNonEmpty("SPV is a mandatory field")]
        public virtual SPV_DAO SPV
        {
            get
            {
                return this._SPV;
            }
            set
            {
                this._SPV = value;
            }
        }

        [Property("IsRolledBack")]
        public virtual bool IsRolledBack
        {
            get
            {
                return this._IsRolledBack;
            }
            set
            {
                this._IsRolledBack = value;
            }
        }
    }
}