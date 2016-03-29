using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using Castle.Components.Validator;

using SAHL.Common.Globals; using SAHL.Common.BusinessModel.DAO.Attributes; namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This class interacts with the LoanTransaction view in 2AM that points to the table of the same name in SAHLDB.
    ///
    /// NB: This object should NOT be queried with HQL, it will fall over spectacularly
    ///     The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
    /// </summary>
    [DoNotTestWithGenericTest()]
    [GenericTest(TestType.Find)][ActiveRecord("Loantransaction", Schema = "dbo", Lazy = true)]
    public partial class LoanTransaction_DAO : DB_2AM<LoanTransaction_DAO>
    {

        private FinancialService_DAO _financialService;

        private TransactionType_DAO _transactionType;

        private System.DateTime _loanTransactionInsertDate;

        private System.DateTime _loanTransactionEffectiveDate; 

        private float _loanTransactionRate;

        private double _loanTransactionAmount;

        private double _loanTransactionNewBalance;

        private string _loanTransactionReference;

        private string _loanTransactionUserID;

        private decimal _sPVNumber;

        private System.DateTime? _loanTransactionActualEffectiveDate;

        private string _rolledBackInd;

        private double _loanAccountCurrentBalance;

        private double _adjustments;

        private float _standardRate;

        private double _coPayment;

        private double _loanTransactionActiveMarketRate;

        private decimal _loanTransactionNumber;

        //private IList<Disbursement_DAO> _disbursements;

        [BelongsTo("TransactionTypeNumber", NotNull = true)]
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

        [BelongsTo("LoanNumber", NotNull = true)]
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
        [Property("LoanTransactionInsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction Insert Date is a mandatory field")]
        public virtual System.DateTime LoanTransactionInsertDate
        {
            get
            {
                return this._loanTransactionInsertDate;
            }
            set
            {
                this._loanTransactionInsertDate = value;
            }
        }

        [Property("LoanTransactionEffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction Effective Date is a mandatory field")]
        public virtual System.DateTime LoanTransactionEffectiveDate
        {
            get
            {
                return this._loanTransactionEffectiveDate;
            }
            set
            {
                this._loanTransactionEffectiveDate = value;
            }
        }

        [Property("LoanTransactionRate", ColumnType = "Single", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction Rate is a mandatory field")]
        public virtual float LoanTransactionRate
        {
            get
            {
                return this._loanTransactionRate;
            }
            set
            {
                this._loanTransactionRate = value;
            }
        }

        [Property("LoanTransactionAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction Amount is a mandatory field")]
        public virtual double LoanTransactionAmount
        {
            get
            {
                return this._loanTransactionAmount;
            }
            set
            {
                this._loanTransactionAmount = value;
            }
        }

        [Property("LoanTransactionNewBalance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction New Balance is a mandatory field")]
        public virtual double LoanTransactionNewBalance
        {
            get
            {
                return this._loanTransactionNewBalance;
            }
            set
            {
                this._loanTransactionNewBalance = value;
            }
        }

        [Property("LoanTransactionReference", ColumnType = "String")]
        public virtual string LoanTransactionReference
        {
            get
            {
                return this._loanTransactionReference;
            }
            set
            {
                this._loanTransactionReference = value;
            }
        }

        [Property("LoanTransactionUserID", ColumnType = "String", NotNull = true)]
        public virtual string LoanTransactionUserID
        {
            get
            {
                return this._loanTransactionUserID;
            }
            set
            {
                this._loanTransactionUserID = value;
            }
        }

        [Property("SPVNumber", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("SPV Number is a mandatory field")]
        public virtual decimal SPVNumber
        {
            get
            {
                return this._sPVNumber;
            }
            set
            {
                this._sPVNumber = value;
            }
        }

        [Property("LoanTransactionActualEffectiveDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? LoanTransactionActualEffectiveDate
        {
            get
            {
                return this._loanTransactionActualEffectiveDate;
            }
            set
            {
                this._loanTransactionActualEffectiveDate = value;
            }
        }

        [Property("RolledBackInd")]
        public virtual string RolledBackInd
        {
            get
            {
                return this._rolledBackInd;
            }
            set
            {
                this._rolledBackInd = value;
            }
        }

        [Property("LoanAccountCurrentBalance", ColumnType = "Double")]
        public virtual double LoanAccountCurrentBalance
        {
            get
            {
                return this._loanAccountCurrentBalance;
            }
            set
            {
                this._loanAccountCurrentBalance = value;
            }
        }

        [Property("Adjustments", ColumnType = "Double")]
        public virtual double Adjustments
        {
            get
            {
                return this._adjustments;
            }
            set
            {
                this._adjustments = value;
            }
        }

        [Property("StandardRate", ColumnType = "Single")]
        public virtual float StandardRate
        {
            get
            {
                return this._standardRate;
            }
            set
            {
                this._standardRate = value;
            }
        }

        [Property("CoPayment", ColumnType = "Double")]
        public virtual double CoPayment
        {
            get
            {
                return this._coPayment;
            }
            set
            {
                this._coPayment = value;
            }
        }

        [Property("LoanTransactionActiveMarketRate", ColumnType = "Double")]
        public virtual double LoanTransactionActiveMarketRate
        {
            get
            {
                return this._loanTransactionActiveMarketRate;
            }
            set
            {
                this._loanTransactionActiveMarketRate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LoanTransactionNumber")]
        public virtual decimal LoanTransactionNumber
        {
            get
            {
                return this._loanTransactionNumber;
            }
            set
            {
                this._loanTransactionNumber = value;
            }
        }

		//NOTE: Not all loantransactions are disbursement transactions, so this relationship is incorrect. The disbursement object will have a collection of LoanTransactions.
		//[HasAndBelongsToMany(typeof(Disbursement_DAO), Table = "DisbursementLoanTransaction", ColumnRef = "DisbursementKey", ColumnKey = "LoanTransactionNumber", Lazy = true, Inverse = true, Cascade = ManyRelationCascadeEnum.SaveUpdate)]
		//public virtual IList<Disbursement_DAO> Disbursements
		//{
		//    get { return _disbursements; }
		//    set { _disbursements = value; }
		//}

    }

}
