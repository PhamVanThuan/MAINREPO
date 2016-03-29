using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("LoanBalance", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class LoanBalance_DAO : DB_2AM<LoanBalance_DAO>
    {
        private int _Key;

        private int _term;

        private double _initialBalance;

        private int _remainingInstalments;

        private double _interestRate;

        private double _rateAdjustment;

        private double _activeMarketRate;

        private double _mTDInterest;

        private RateConfiguration_DAO _rateConfiguration;

        private ResetConfiguration_DAO _resetConfiguration;

        [Property("Term", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Term is a mandatory field")]
        public virtual int Term
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

        [Property("InitialBalance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("InitialBalance is a mandatory field")]
        public virtual double InitialBalance
        {
            get
            {
                return this._initialBalance;
            }
            set
            {
                this._initialBalance = value;
            }
        }

        [Property("RemainingInstalments", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("RemainingInstalments is a mandatory field")]
        public virtual int RemainingInstalments
        {
            get
            {
                return this._remainingInstalments;
            }
            set
            {
                this._remainingInstalments = value;
            }
        }

        [Property("InterestRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("InterestRate is a mandatory field")]
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

        [Property("RateAdjustment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("RateAdjustment is a mandatory field")]
        public virtual double RateAdjustment
        {
            get
            {
                return this._rateAdjustment;
            }
            set
            {
                this._rateAdjustment = value;
            }
        }

        [Property("ActiveMarketRate", ColumnType = "Double", NotNull = false)]
        public virtual double ActiveMarketRate
        {
            get
            {
                return this._activeMarketRate;
            }
            set
            {
                this._activeMarketRate = value;
            }
        }

        [Property("MTDInterest", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("MTDInterest is a mandatory field")]
        public virtual double MTDInterest
        {
            get
            {
                return this._mTDInterest;
            }
            set
            {
                this._mTDInterest = value;
            }
        }

        [BelongsTo("RateConfigurationKey", NotNull = true)]
        [ValidateNonEmpty("Rate Configuration is a mandatory field")]
        public virtual RateConfiguration_DAO RateConfiguration
        {
            get
            {
                return this._rateConfiguration;
            }
            set
            {
                this._rateConfiguration = value;
            }
        }

        [BelongsTo("ResetConfigurationKey", NotNull = true)]
        [ValidateNonEmpty("Reset Configuration is a mandatory field")]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get
            {
                return this._resetConfiguration;
            }
            set
            {
                this._resetConfiguration = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServiceKey", ColumnType = "Int32")]
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
    }
}