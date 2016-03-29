using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DebtCounsellingProposalItem", Schema = "migration")]
    public partial class MigrationDebtCounsellingProposalItem_DAO : DB_2AM<MigrationDebtCounsellingProposalItem_DAO>
    {
        private int _key;

        private System.DateTime _startDate;

        private System.DateTime _endDate;

        private int _marketRateKey;

        private decimal _interestRate;

        private decimal _amount;

        private decimal _additionalAmount;

        private System.DateTime _createDate;

        private decimal _instalmentPercentage;

        private decimal _annualEscalation;

        private int _startPeriod;

        private int _endPeriod;

        private MigrationDebtCounsellingProposal_DAO _debtCounsellingProposal;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingProposalItemKey", ColumnType = "Int32")]
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

        [Property("StartDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime StartDate
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

        [Property("EndDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        [Property("MarketRateKey", ColumnType = "Int32", NotNull = true)]
        public virtual int MarketRateKey
        {
            get
            {
                return this._marketRateKey;
            }
            set
            {
                this._marketRateKey = value;
            }
        }

        [Property("InterestRate", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal InterestRate
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

        [Property("Amount", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal Amount
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

        [Property("AdditionalAmount", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal AdditionalAmount
        {
            get
            {
                return this._additionalAmount;
            }
            set
            {
                this._additionalAmount = value;
            }
        }

        [Property("CreateDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
            }
        }

        [Property("InstalmentPercentage", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal InstalmentPercentage
        {
            get
            {
                return this._instalmentPercentage;
            }
            set
            {
                this._instalmentPercentage = value;
            }
        }

        [Property("AnnualEscalation", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal AnnualEscalation
        {
            get
            {
                return this._annualEscalation;
            }
            set
            {
                this._annualEscalation = value;
            }
        }

        [Property("StartPeriod", ColumnType = "Int32", NotNull = true)]
        public virtual int StartPeriod
        {
            get
            {
                return this._startPeriod;
            }
            set
            {
                this._startPeriod = value;
            }
        }

        [Property("EndPeriod", ColumnType = "Int32", NotNull = true)]
        public virtual int EndPeriod
        {
            get
            {
                return this._endPeriod;
            }
            set
            {
                this._endPeriod = value;
            }
        }

        [BelongsTo("DebtCounsellingProposalKey", NotNull = true)]
        public virtual MigrationDebtCounsellingProposal_DAO DebtCounsellingProposal
        {
            get
            {
                return this._debtCounsellingProposal;
            }
            set
            {
                this._debtCounsellingProposal = value;
            }
        }

        [ValidateSelf]
        public virtual void ValidateDate(ErrorSummary errors)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            if (StartPeriod > EndPeriod)
            {
                errors.RegisterErrorMessage("Start Period", "Start Period cannot be greater than the End Period.");
                spc.DomainMessages.Add(new Error("Start Period cannot be greater than the End Period.", "Start Period cannot be greater than the End Period."));
            }
        }
    }
}