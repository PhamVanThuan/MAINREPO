using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDebtSettlement", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDebtSettlement_DAO : DB_2AM<ApplicationDebtSettlement_DAO>
    {
        private double _settlementAmount;

        private System.DateTime? _settlementDate;

        private double _rateApplied;

        private System.DateTime? _interestStartDate;

        private double _capitalAmount;

        private double _guaranteeAmount;

        private int _key;

        private BankAccount_DAO _bankAccount;

        private Disbursement_DAO _disbursement;

        private DisbursementInterestApplied_DAO _disbursementInterestApplied;

        private DisbursementType_DAO _disbursementType;

        private ApplicationExpense_DAO _offerExpense;

        [Property("SettlementAmount", ColumnType = "Double")]
        public virtual double SettlementAmount
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
        public virtual double RateApplied
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
        public virtual double CapitalAmount
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
        public virtual double GuaranteeAmount
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

        [PrimaryKey(PrimaryKeyType.Native, "OfferDebtSettlementKey", ColumnType = "Int32")]
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

        [BelongsTo("DisbursementKey")]
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

        [BelongsTo("InterestAppliedTypeKey")]
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

        [BelongsTo("OfferExpenseKey", Cascade = CascadeEnum.None)]
        public virtual ApplicationExpense_DAO OfferExpense
        {
            get
            {
                return this._offerExpense;
            }
            set
            {
                this._offerExpense = value;
            }
        }
    }
}