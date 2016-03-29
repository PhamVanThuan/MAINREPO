using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProposalItem", Schema = "debtcounselling", Lazy = false)]
    public partial class ProposalItem_DAO : DB_2AM<ProposalItem_DAO>
    {
        private int _key;

        private System.DateTime _startDate;

        private System.DateTime _endDate;

        private double _interestRate;

        private double _amount;

        private double _additionalAmount;

        private ADUser_DAO _aDUser;

        private System.DateTime _createDate;

        private MarketRate_DAO _marketRate;

        private Proposal_DAO _proposal;

        private double? _instalmentPercent;

        private double? _annualEscalation;

        private short _startPeriod;

        private short _endPeriod;

        [PrimaryKey(PrimaryKeyType.Native, "ProposalItemKey", ColumnType = "Int32")]
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

        [Property("InterestRate", ColumnType = "Double", NotNull = true)]
        public virtual double InterestRate
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

        [Property("AdditionalAmount", ColumnType = "Double", NotNull = true)]
        public virtual double AdditionalAmount
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

        [BelongsTo(Column = "ADUserKey", NotNull = true)]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
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

        [BelongsTo(Column = "MarketRateKey", NotNull = false)]
        public virtual MarketRate_DAO MarketRate
        {
            get
            {
                return this._marketRate;
            }
            set
            {
                this._marketRate = value;
            }
        }

        [BelongsTo("ProposalKey", NotNull = true)]
        public virtual Proposal_DAO Proposal
        {
            get
            {
                return this._proposal;
            }
            set
            {
                this._proposal = value;
            }
        }

        [Property("InstalmentPercent", ColumnType = "Double", NotNull = false)]
        public virtual double? InstalmentPercent
        {
            get
            {
                return this._instalmentPercent;
            }
            set
            {
                this._instalmentPercent = value;
            }
        }

        [Property("AnnualEscalation", ColumnType = "Double", NotNull = false)]
        public virtual double? AnnualEscalation
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

        [Property("StartPeriod", ColumnType = "Int16")]
        public virtual short StartPeriod
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

        [Property("EndPeriod", ColumnType = "Int16")]
        public virtual short EndPeriod
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
    }
}