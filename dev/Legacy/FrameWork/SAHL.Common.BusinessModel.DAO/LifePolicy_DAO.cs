using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LifePolicy", Schema = "dbo", Lazy = true)]
    [ConstructorInjector]
    public partial class LifePolicy_DAO : DB_2AM<LifePolicy_DAO>
    {
        private int _key;

        private double _deathBenefit;

        private double _installmentProtectionBenefit;

        private double _deathBenefitPremium;

        private double _installmentProtectionPremium;

        private System.DateTime? _dateOfCommencement;

        private System.DateTime _dateOfExpiry;

        private double _deathRetentionLimit;

        private double _installmentProtectionRetentionLimit;

        private decimal _upliftFactor;

        private decimal _jointDiscountFactor;

        private System.DateTime? _dateOfCancellation;

        private double _yearlyPremium;

        private System.DateTime? _dateOfAcceptance;

        private double _sumAssured;

        private System.DateTime? _dateLastUpdated;

        private string _consultant;

        private ClaimStatus_DAO _claimStatus;

        private ClaimType_DAO _claimType;

        private double? _currentSumAssured;

        private double? _premiumShortfall;

        private Insurer_DAO _insurer;

        private string _externalPolicyNumber;

        private System.DateTime? _dateCeded;

        private Priority_DAO _priority;

        private System.DateTime? _claimStatusDate;

        private LegalEntity_DAO _policyHolderLE;

        private string _rPARInsurer;

        private string _rPARPolicyNumber;

        private double? _deathReassuranceRetention;

        private double? _iPBReassuranceRetention;

        private LifePolicyType_DAO _lifePolicyType;

        private Broker_DAO _broker;

        private LifePolicyStatus_DAO _lifePolicyStatus;

        private System.DateTime? _AnniversaryDate;

        private FinancialService_DAO _financialService;

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "FinancialServiceKey")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        [Property("DeathBenefit", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Death Benefit is a mandatory field")]
        public virtual double DeathBenefit
        {
            get
            {
                return this._deathBenefit;
            }
            set
            {
                this._deathBenefit = value;
            }
        }

        [Property("InstallmentProtectionBenefit", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Installment Protection Benefit is a mandatory field")]
        public virtual double InstallmentProtectionBenefit
        {
            get
            {
                return this._installmentProtectionBenefit;
            }
            set
            {
                this._installmentProtectionBenefit = value;
            }
        }

        [Property("DeathBenefitPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Death Benefit Premium is a mandatory field")]
        public virtual double DeathBenefitPremium
        {
            get
            {
                return this._deathBenefitPremium;
            }
            set
            {
                this._deathBenefitPremium = value;
            }
        }

        [Property("InstallmentProtectionPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Installment Protection Premium is a mandatory field")]
        public virtual double InstallmentProtectionPremium
        {
            get
            {
                return this._installmentProtectionPremium;
            }
            set
            {
                this._installmentProtectionPremium = value;
            }
        }

        [Property("DateOfCommencement", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateOfCommencement
        {
            get
            {
                return this._dateOfCommencement;
            }
            set
            {
                this._dateOfCommencement = value;
            }
        }

        [Property("DateOfExpiry", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Date of Expiry is a mandatory field")]
        public virtual System.DateTime DateOfExpiry
        {
            get
            {
                return this._dateOfExpiry;
            }
            set
            {
                this._dateOfExpiry = value;
            }
        }

        [Property("DeathRetentionLimit", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Death Retention Limit is a mandatory field")]
        public virtual double DeathRetentionLimit
        {
            get
            {
                return this._deathRetentionLimit;
            }
            set
            {
                this._deathRetentionLimit = value;
            }
        }

        [Property("InstallmentProtectionRetentionLimit", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Installment Protection Retention Limit is a mandatory field")]
        public virtual double InstallmentProtectionRetentionLimit
        {
            get
            {
                return this._installmentProtectionRetentionLimit;
            }
            set
            {
                this._installmentProtectionRetentionLimit = value;
            }
        }

        [Property("UpliftFactor", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("Uplift Factor is a mandatory field")]
        public virtual decimal UpliftFactor
        {
            get
            {
                return this._upliftFactor;
            }
            set
            {
                this._upliftFactor = value;
            }
        }

        [Property("JointDiscountFactor", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("Joint Discount Factor is a mandatory field")]
        public virtual decimal JointDiscountFactor
        {
            get
            {
                return this._jointDiscountFactor;
            }
            set
            {
                this._jointDiscountFactor = value;
            }
        }

        [Property("DateOfCancellation", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateOfCancellation
        {
            get
            {
                return this._dateOfCancellation;
            }
            set
            {
                this._dateOfCancellation = value;
            }
        }

        [Property("DeathReassuranceRetention", ColumnType = "Double")]
        public virtual double? DeathReassuranceRetention
        {
            get
            {
                return this._deathReassuranceRetention;
            }
            set
            {
                this._deathReassuranceRetention = value;
            }
        }

        [Property("IPBReassuranceRetention", ColumnType = "Double")]
        public virtual double? IPBReassuranceRetention
        {
            get
            {
                return this._iPBReassuranceRetention;
            }
            set
            {
                this._iPBReassuranceRetention = value;
            }
        }

        [Property("YearlyPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Yearly Premium is a mandatory field")]
        public virtual double YearlyPremium
        {
            get
            {
                return this._yearlyPremium;
            }
            set
            {
                this._yearlyPremium = value;
            }
        }

        [Property("DateOfAcceptance", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateOfAcceptance
        {
            get
            {
                return this._dateOfAcceptance;
            }
            set
            {
                this._dateOfAcceptance = value;
            }
        }

        [Property("SumAssured", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Sum Assured is a mandatory field")]
        public virtual double SumAssured
        {
            get
            {
                return this._sumAssured;
            }
            set
            {
                this._sumAssured = value;
            }
        }

        [Property("DateLastUpdated", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateLastUpdated
        {
            get
            {
                return this._dateLastUpdated;
            }
            set
            {
                this._dateLastUpdated = value;
            }
        }

        [Property("Consultant", ColumnType = "String")]
        public virtual string Consultant
        {
            get
            {
                return this._consultant;
            }
            set
            {
                this._consultant = value;
            }
        }

        [Property("CurrentSumAssured", ColumnType = "Double")]
        public virtual double? CurrentSumAssured
        {
            get
            {
                return this._currentSumAssured;
            }
            set
            {
                this._currentSumAssured = value;
            }
        }

        [Property("PremiumShortfall", ColumnType = "Double")]
        public virtual double? PremiumShortfall
        {
            get
            {
                return this._premiumShortfall;
            }
            set
            {
                this._premiumShortfall = value;
            }
        }

        [Property("ExternalPolicyNumber", ColumnType = "String")]
        public virtual string ExternalPolicyNumber
        {
            get
            {
                return this._externalPolicyNumber;
            }
            set
            {
                this._externalPolicyNumber = value;
            }
        }

        [Property("DateCeded", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateCeded
        {
            get
            {
                return this._dateCeded;
            }
            set
            {
                this._dateCeded = value;
            }
        }

        [Property("ClaimStatusDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ClaimStatusDate
        {
            get
            {
                return this._claimStatusDate;
            }
            set
            {
                this._claimStatusDate = value;
            }
        }

        /// <summary>
        /// The Primary Legal entity for this policy.
        /// </summary>
        [BelongsTo("PolicyHolderLEKey", NotNull = false)]
        public virtual LegalEntity_DAO PolicyHolderLE
        {
            get
            {
                return this._policyHolderLE;
            }
            set
            {
                this._policyHolderLE = value;
            }
        }

        [Property("RPARInsurer", ColumnType = "String")]
        public virtual string RPARInsurer
        {
            get
            {
                return this._rPARInsurer;
            }
            set
            {
                this._rPARInsurer = value;
            }
        }

        [Property("RPARPolicyNumber", ColumnType = "String")]
        public virtual string RPARPolicyNumber
        {
            get
            {
                return this._rPARPolicyNumber;
            }
            set
            {
                this._rPARPolicyNumber = value;
            }
        }

        [BelongsTo("BrokerKey", NotNull = false)]
        public virtual Broker_DAO Broker
        {
            get
            {
                return this._broker;
            }
            set
            {
                this._broker = value;
            }
        }

        [BelongsTo("InsurerKey", NotNull = false)]
        public virtual Insurer_DAO Insurer
        {
            get
            {
                return this._insurer;
            }
            set
            {
                this._insurer = value;
            }
        }

        [BelongsTo("ClaimStatusKey", NotNull = false)]
        public virtual ClaimStatus_DAO ClaimStatus
        {
            get
            {
                return this._claimStatus;
            }
            set
            {
                this._claimStatus = value;
            }
        }

        [BelongsTo("ClaimTypeKey", NotNull = false)]
        public virtual ClaimType_DAO ClaimType
        {
            get
            {
                return this._claimType;
            }
            set
            {
                this._claimType = value;
            }
        }

        [BelongsTo("PolicyStatusKey", NotNull = true)]
        [ValidateNonEmpty("Life Policy Status is a mandatory field")]
        public virtual LifePolicyStatus_DAO LifePolicyStatus
        {
            get
            {
                return this._lifePolicyStatus;
            }
            set
            {
                this._lifePolicyStatus = value;
            }
        }

        [BelongsTo("PriorityKey", NotNull = false)]
        public virtual Priority_DAO Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        [BelongsTo("LifePolicyTypeKey", NotNull = true)]
        public virtual LifePolicyType_DAO LifePolicyType
        {
            get
            {
                return this._lifePolicyType;
            }
            set
            {
                this._lifePolicyType = value;
            }
        }

        [Property("AnniversaryDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? AnniversaryDate
        {
            get
            {
                return this._AnniversaryDate;
            }
            set
            {
                this._AnniversaryDate = value;
            }
        }

        [OneToOne]
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
    }
}