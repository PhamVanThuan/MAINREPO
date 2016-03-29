namespace SAHL.Common.BusinessModel.DAO
{
    /*[ActiveRecord("FinancialServiceRecurringTransactions", Schema = "dbo", Lazy = true)]
    public partial class FinancialServiceRecurringTransaction_DAO : DB_2AM<FinancialServiceRecurringTransaction_DAO>
    {
        private System.DateTime? _insertDate;

        private int? _frequency;

        private TransactionType_DAO _transactionType;

        private char _frequencyType;

        private int? _numUntilNextRun;

        private string _reference;

        private bool _active;

        private System.DateTime? _startDate;

        private int? _term;

        private int? _remainingTerm;

        private int? _transactionDay;

        private int? _hourOfRun;

        private double? _amount;

        private string _statementName;

        private System.DateTime? _previousRunDate;

        private string _userName;

        private string _notes;

        private int _Key;

        private BankAccount_DAO _bankAccount;

        private FinancialService_DAO _financialService;

        private RecurringTransactionType_DAO _recurringTransactionType;

        [Property("InsertDate", ColumnType = "Timestamp")]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual System.DateTime? InsertDate
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

        [Property("Frequency", ColumnType = "Int32")]
        [ValidateNonEmpty("Frequency is a mandatory field")]
        public virtual int? Frequency
        {
            get
            {
                return this._frequency;
            }
            set
            {
                this._frequency = value;
            }
        }

        [BelongsTo("TransactionTypeNumber",NotNull = false)]
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

        [Property("FrequencyType", ColumnType = "AnsiChar")]
        [ValidateNonEmpty("Frequency Type is a mandatory field")]
        public virtual char FrequencyType
        {
            get
            {
                return this._frequencyType;
            }
            set
            {
                this._frequencyType = value;
            }
        }

        [Property("NumUntilNextRun", ColumnType = "Int32")]
        [ValidateNonEmpty("Num Until Next Run is a mandatory field")]
        public virtual int? NumUntilNextRun
        {
            get
            {
                return this._numUntilNextRun;
            }
            set
            {
                this._numUntilNextRun = value;
            }
        }

        [Property("Reference", ColumnType = "String")]
        [ValidateNonEmpty("Reference is a mandatory field")]
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

        [Property("Active", ColumnType = "Boolean")]
        public virtual bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }

        [Property("StartDate", ColumnType = "Timestamp")]
        [ValidateNonEmpty("Start Date is a mandatory field")]
        public virtual System.DateTime? StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        [Property("Term", ColumnType = "Int32")]
        public virtual int? Term
        {
            get
            {
                return this._term;
            }
            set
            {
                this._term = value;
            }
        }

        [Property("RemainingTerm", ColumnType = "Int32")]
        public virtual int? RemainingTerm
        {
            get
            {
                return this._remainingTerm;
            }
            set
            {
                this._remainingTerm = value;
            }
        }

        [Property("TransactionDay", ColumnType = "Int32")]
        public virtual int? TransactionDay
        {
            get
            {
                return this._transactionDay;
            }
            set
            {
                this._transactionDay = value;
            }
        }

        [Property("HourOfRun", ColumnType = "Int32")]
        public virtual int? HourOfRun
        {
            get
            {
                return this._hourOfRun;
            }
            set
            {
                this._hourOfRun = value;
            }
        }

        [Property("Amount", ColumnType = "Double")]
        [ValidateNonEmpty("Amount is a mandatory field")]
        public virtual double? Amount
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

        [Property("StatementName", ColumnType = "String")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [Property("PreviousRunDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? PreviousRunDate
        {
            get
            {
                return this._previousRunDate;
            }
            set
            {
                this._previousRunDate = value;
            }
        }

        [Property("UserName", ColumnType = "String", Length = 100)]
        [ValidateNonEmpty("User Name is a mandatory field")]
        public virtual string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        [Property("Notes", ColumnType = "String", Length = 250)]
        public virtual string Notes
        {
            get
            {
                return this._notes;
            }
            set
            {
                this._notes = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FinancialServiceRecurringTransactionKey", ColumnType = "Int32")]
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

        [BelongsTo("BankAccountKey", NotNull = false)]
        public virtual BankAccount_DAO BankAccount
        {
            get
            {
                return this._bankAccount;
            }
            set
            {
                this._bankAccount = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = false)]
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

        [BelongsTo("RecurringTransactionTypeKey", NotNull = false)]
        [ValidateNonEmpty("Recurring Transaction Type is a mandatory field")]
        public virtual RecurringTransactionType_DAO RecurringTransactionType
        {
            get
            {
                return this._recurringTransactionType;
            }
            set
            {
                this._recurringTransactionType = value;
            }
        }
    }*/
}