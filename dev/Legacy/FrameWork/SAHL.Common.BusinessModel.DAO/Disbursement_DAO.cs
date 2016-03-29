using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Disbursement", Schema = "dbo", Lazy = true)]
    public partial class Disbursement_DAO : DB_2AM<Disbursement_DAO>
    {
        private System.DateTime? _preparedDate;

        private System.DateTime? _actionDate;

        private string _accountName;

        private string _accountNumber;

        private double? _amount;

        private double? _capitalAmount;

        private double? _guaranteeAmount;

        private double? _interestRate;

        private System.DateTime? _interestStartDate;

        private char _interestApplied;

        private double? _paymentAmount;

        private int _Key;

        private IList<AccountDebtSettlement_DAO> _accountDebtSettlements;

        private IList<FinancialTransaction_DAO> _loanTransactions;

        private ACBBank_DAO _aCBBank;

        private ACBBranch_DAO _aCBBranch;

        private ACBType_DAO _aCBType;

        private Account_DAO _account;

        private DisbursementStatus_DAO _disbursementStatus;

        private DisbursementTransactionType_DAO _disbursementTransactionType;

        [Property("PreparedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? PreparedDate
        {
            get
            {
                return this._preparedDate;
            }
            set
            {
                this._preparedDate = value;
            }
        }

        [Property("ActionDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ActionDate
        {
            get
            {
                return this._actionDate;
            }
            set
            {
                this._actionDate = value;
            }
        }

        [Property("AccountName", ColumnType = "String")]
        public virtual string AccountName
        {
            get
            {
                return this._accountName;
            }
            set
            {
                this._accountName = value;
            }
        }

        [Property("AccountNumber", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Account Number is a mandatory field")]
        public virtual string AccountNumber
        {
            get
            {
                return this._accountNumber;
            }
            set
            {
                this._accountNumber = value;
            }
        }

        [Property("Amount", ColumnType = "Double", NotNull = true)]
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

        [Property("CapitalAmount", ColumnType = "Double")]
        public virtual double? CapitalAmount
        {
            get
            {
                return this._capitalAmount;
            }
            set
            {
                this._capitalAmount = value;
            }
        }

        [Property("GuaranteeAmount", ColumnType = "Double")]
        public virtual double? GuaranteeAmount
        {
            get
            {
                return this._guaranteeAmount;
            }
            set
            {
                this._guaranteeAmount = value;
            }
        }

        [Property("InterestRate", ColumnType = "Double")]
        public virtual double? InterestRate
        {
            get
            {
                return this._interestRate;
            }
            set
            {
                this._interestRate = value;
            }
        }

        [Property("InterestStartDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? InterestStartDate
        {
            get
            {
                return this._interestStartDate;
            }
            set
            {
                this._interestStartDate = value;
            }
        }

        [Property("InterestApplied", ColumnType = "AnsiChar")]
        public virtual char InterestApplied
        {
            get
            {
                return this._interestApplied;
            }
            set
            {
                this._interestApplied = value;
            }
        }

        [Property("PaymentAmount", ColumnType = "Double")]
        public virtual double? PaymentAmount
        {
            get
            {
                return this._paymentAmount;
            }
            set
            {
                this._paymentAmount = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DisbursementKey", ColumnType = "Int32")]
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

        [HasMany(typeof(AccountDebtSettlement_DAO), ColumnKey = "DisbursementKey", Table = "AccountDebtSettlement")]
        public virtual IList<AccountDebtSettlement_DAO> AccountDebtSettlements
        {
            get
            {
                return this._accountDebtSettlements;
            }
            set
            {
                this._accountDebtSettlements = value;
            }
        }

        [HasAndBelongsToMany(typeof(FinancialTransaction_DAO), Table = "DisbursementFinancialTransaction", ColumnKey = "DisbursementKey", ColumnRef = "FinancialTransactionKey", Inverse = false, Lazy = true, Cascade = ManyRelationCascadeEnum.All)]
        public virtual IList<FinancialTransaction_DAO> LoanTransactions
        {
            get { return _loanTransactions; }
            set { _loanTransactions = value; }
        }

        // todo: Uncomment when DisbursementLoanTransaction has been implemented check if this should be relationship
        //[HasMany(typeof(DisbursementLoanTransaction), ColumnKey = "DisbursementKey", Table = "DisbursementLoanTransaction")]
        //public virtual IList<DisbursementLoanTransaction_DAO> DisbursementLoanTransactions
        //{
        //    get
        //    {
        //        return this._disbursementLoanTransactions;
        //    }
        //    set
        //    {
        //        this._disbursementLoanTransactions = value;
        //    }
        //}

        [BelongsTo("ACBBankCode", NotNull = true)]
        [ValidateNonEmpty("ACB Bank is a mandatory field")]
        public virtual ACBBank_DAO ACBBank
        {
            get
            {
                return this._aCBBank;
            }
            set
            {
                this._aCBBank = value;
            }
        }

        [BelongsTo("ACBBranchCode", NotNull = true)]
        [ValidateNonEmpty("ACB Branch is a mandatory field")]
        public virtual ACBBranch_DAO ACBBranch
        {
            get
            {
                return this._aCBBranch;
            }
            set
            {
                this._aCBBranch = value;
            }
        }

        [BelongsTo("ACBTypeNumber", NotNull = true)]
        [ValidateNonEmpty("ACB Type is a mandatory field")]
        public virtual ACBType_DAO ACBType
        {
            get
            {
                return this._aCBType;
            }
            set
            {
                this._aCBType = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        [BelongsTo("DisbursementStatusKey", NotNull = false)]
        public virtual DisbursementStatus_DAO DisbursementStatus
        {
            get
            {
                return this._disbursementStatus;
            }
            set
            {
                this._disbursementStatus = value;
            }
        }

        [BelongsTo("DisbursementTransactionTypeKey")]
        public virtual DisbursementTransactionType_DAO DisbursementTransactionType
        {
            get
            {
                return this._disbursementTransactionType;
            }
            set
            {
                this._disbursementTransactionType = value;
            }
        }
    }
}