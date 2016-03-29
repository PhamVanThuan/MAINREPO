using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SuperLo", Schema = "product", Lazy = true)]
    public partial class SuperLo_DAO : DB_2AM<SuperLo_DAO>
    {
        private int _key;

        private System.DateTime _electionDate;

        private System.DateTime _convertedDate;

        private System.DateTime _nextPaymentDate;

        private double _pPThresholdYr1;

        private double _pPThresholdYr2;

        private double _pPThresholdYr3;

        private double _pPThresholdYr4;

        private double _pPThresholdYr5;

        private double _mTDLoyaltyBenefit;

        private double _pPAllowed;

        private GeneralStatus_DAO _generalStatus;

        private bool? _exclusion;

        private System.DateTime? _exclusionEndDate;

        private string _exclusionReason;

        private double _overPaymentAmount;

        private FinancialServiceAttribute_DAO _financialServiceAttribute;

        [PrimaryKey(PrimaryKeyType.Foreign, Column = "FinancialServiceAttributeKey")]
        public virtual int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [Property("ElectionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ElectionDate
        {
            get
            {
                return this._electionDate;
            }
            set
            {
                this._electionDate = value;
            }
        }

        [Property("ConvertedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ConvertedDate
        {
            get
            {
                return this._convertedDate;
            }
            set
            {
                this._convertedDate = value;
            }
        }

        [Property("NextPaymentDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime NextPaymentDate
        {
            get
            {
                return this._nextPaymentDate;
            }
            set
            {
                this._nextPaymentDate = value;
            }
        }

        [Property("PPThresholdYr1", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPThresholdYr1
        {
            get
            {
                return this._pPThresholdYr1;
            }
            set
            {
                this._pPThresholdYr1 = value;
            }
        }

        [Property("PPThresholdYr2", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPThresholdYr2
        {
            get
            {
                return this._pPThresholdYr2;
            }
            set
            {
                this._pPThresholdYr2 = value;
            }
        }

        [Property("PPThresholdYr3", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPThresholdYr3
        {
            get
            {
                return this._pPThresholdYr3;
            }
            set
            {
                this._pPThresholdYr3 = value;
            }
        }

        [Property("PPThresholdYr4", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPThresholdYr4
        {
            get
            {
                return this._pPThresholdYr4;
            }
            set
            {
                this._pPThresholdYr4 = value;
            }
        }

        [Property("PPThresholdYr5", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPThresholdYr5
        {
            get
            {
                return this._pPThresholdYr5;
            }
            set
            {
                this._pPThresholdYr5 = value;
            }
        }

        [Property("MTDLoyaltyBenefit", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double MTDLoyaltyBenefit
        {
            get
            {
                return this._mTDLoyaltyBenefit;
            }
            set
            {
                this._mTDLoyaltyBenefit = value;
            }
        }

        [Property("PPAllowed", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double PPAllowed
        {
            get
            {
                return this._pPAllowed;
            }
            set
            {
                this._pPAllowed = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [Property("Exclusion", ColumnType = "Boolean", NotNull = false)]
        public virtual bool? Exclusion
        {
            get
            {
                return this._exclusion;
            }
            set
            {
                this._exclusion = value;
            }
        }

        [Property("ExclusionEndDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ExclusionEndDate
        {
            get
            {
                return this._exclusionEndDate;
            }
            set
            {
                this._exclusionEndDate = value;
            }
        }

        [Property("ExclusionReason", ColumnType = "String")]
        public virtual string ExclusionReason
        {
            get
            {
                return this._exclusionReason;
            }
            set
            {
                this._exclusionReason = value;
            }
        }

        [Property("OverPaymentAmount", ColumnType = "Double", Update = false)]
        public virtual double OverPaymentAmount
        {
            get
            {
                return this._overPaymentAmount;
            }
            set
            {
                this._overPaymentAmount = value;
            }
        }

        [OneToOne]
        public virtual FinancialServiceAttribute_DAO FinancialServiceAttribute
        {
            get
            {
                return this._financialServiceAttribute;
            }
            set
            {
                this._financialServiceAttribute = value;
            }
        }
    }
}