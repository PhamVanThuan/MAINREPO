using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountDebtSettlement", Schema = "RCS", Lazy = true)]
    public partial class AccountDebtSettlement_DAO : DB_2AM<AccountDebtSettlement_DAO>
    {
        private double? _settlementAmount;

        private System.DateTime? _settlementDate;

        private double? _rateApplied;

        private System.DateTime? _interestStartDate;

        private double? _capitalAmount;

        private double? _guaranteeAmount;

        private int _Key;

        //private IList<Property_DAO> _propertyAccountDebtSettlements;

        private AccountExpense_DAO _accountExpense;

        private BankAccount_DAO _bankAccount;

        private Disbursement_DAO _disbursement;

        private DisbursementInterestApplied_DAO _disbursementInterestApplied;

        private DisbursementType_DAO _disbursementType;

        [Property("SettlementAmount", ColumnType = "Double")]
        public virtual double? SettlementAmount
        {
            get
            {
                return this._settlementAmount;
            }
            set
            {
                this._settlementAmount = value;
            }
        }

        [Property("SettlementDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? SettlementDate
        {
            get
            {
                return this._settlementDate;
            }
            set
            {
                this._settlementDate = value;
            }
        }

        [Property("RateApplied", ColumnType = "Double")]
        public virtual double? RateApplied
        {
            get
            {
                return this._rateApplied;
            }
            set
            {
                this._rateApplied = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "DebtSettlementKey", ColumnType = "Int32")]
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

        [BelongsTo("ExpenseKey", NotNull = false)]
        public virtual AccountExpense_DAO AccountExpense
        {
            get
            {
                return this._accountExpense;
            }
            set
            {
                this._accountExpense = value;
            }
        }

        [BelongsTo("BankAccountKey", NotNull = true)]
        [ValidateNonEmpty("Bank Account is a mandatory field")]
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

        [BelongsTo("DisbursementKey", NotNull = true)]
        [ValidateNonEmpty("Disbursement is a mandatory field")]
        public virtual Disbursement_DAO Disbursement
        {
            get
            {
                return this._disbursement;
            }
            set
            {
                this._disbursement = value;
            }
        }

        [BelongsTo("InterestAppliedTypeKey", NotNull = false)]
        public virtual DisbursementInterestApplied_DAO DisbursementInterestApplied
        {
            get
            {
                return this._disbursementInterestApplied;
            }
            set
            {
                this._disbursementInterestApplied = value;
            }
        }

        [BelongsTo("DisbursementTypeKey", NotNull = true)]
        [ValidateNonEmpty("Disbursement Type is a mandatory field")]
        public virtual DisbursementType_DAO DisbursementType
        {
            get
            {
                return this._disbursementType;
            }
            set
            {
                this._disbursementType = value;
            }
        }

        //[HasAndBelongsToMany(typeof(Property_DAO), Table = "PropertyAccountDebtSettlement", ColumnKey = "AccountDebtSettlementKey", ColumnRef = "PropertyKey", Lazy = true)]
        //public virtual IList<Property_DAO> PropertyAccountDebtSettlements
        //{
        //    get
        //    {
        //        return _propertyAccountDebtSettlements;
        //    }
        //    set
        //    {
        //        _propertyAccountDebtSettlements = value;
        //    }
        //}
    }
}