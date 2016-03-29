using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]
    // If GenericLoadSaveLoad fails with this DAO check OfferLife table does not contain OfferKey = 1
    [ActiveRecord("OfferLife", Schema = "dbo", Lazy = true)]
    [Lurker]
    public partial class ApplicationLifeDetail_DAO : DB_2AM<ApplicationLifeDetail_DAO>
    {
        private int _key;

        private double _deathBenefit;

        private double _installmentProtectionBenefit;

        private double _deathBenefitPremium;

        private double _installmentProtectionPremium;

        private System.DateTime _dateOfExpiry;

        private decimal _upliftFactor;

        private decimal _jointDiscountFactor;

        private double _monthlyPremium;

        private double _yearlyPremium;

        private double _sumAssured;

        private DateTime? _dateLastUpdated;

        //private string _consultant;

        private double? _currentSumAssured;

        private double? _premiumShortfall;

        private Insurer_DAO _insurer;

        private string _externalPolicyNumber;

        private DateTime? _dateCeded;

        private Priority_DAO _priority;

        private LegalEntity_DAO _policyHolderLegalEntity;

        private string _rPARInsurer;

        private string _rPARPolicyNumber;

        private DateTime? _dateOfAcceptance;

        //private Broker_DAO _broker;

        private Application_DAO _application;

        private string _consultantADUserName;

        private LifePolicyType_DAO _lifePolicyType;

        [PrimaryKey(PrimaryKeyType.Foreign, "OfferKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The benefit that will be paid out should the client die while the Policy is active.
        /// </summary>
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

        /// <summary>
        /// The benefit protects the client against paying the Monthly Instalment under particular circumstances.
        /// </summary>
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

        /// <summary>
        /// The Premium the client pays in order to receive the Death Benefit.
        /// </summary>
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

        /// <summary>
        /// The Premium which the client pays in order to receive the Instalment Protection Benefit.
        /// </summary>
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

        /// <summary>
        /// The Date on which the Life Policy expires.
        /// </summary>
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

        /// <summary>
        /// The Date the client takes up the Life Application.
        /// </summary>
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

        /// <summary>
        /// The total Monthly Premium the client pays on the Life Policy.
        /// </summary>
        [Property("MonthlyPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Monthly Premium is a mandatory field")]
        public virtual double MonthlyPremium
        {
            get
            {
                return this._monthlyPremium;
            }
            set
            {
                this._monthlyPremium = value;
            }
        }

        /// <summary>
        /// The total Monthly Premiums for a Year.
        /// </summary>
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

        /// <summary>
        /// The Date on which the Life Policy was last updated.
        /// </summary>
        [Property("DateLastUpdated")]
        public virtual DateTime? DateLastUpdated
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

        /// <summary>
        /// The amount which the Life Policy covers. This should equal the Current Outstanding Balance on the Loan and this
        /// value is recalculated at various stages. i.e. Yearly/Client takes a Further Loan.
        /// </summary>
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

        /// <summary>
        /// Each Life Policy belongs to a particular Insurer.
        /// </summary>
        [BelongsTo("InsurerKey", NotNull = true)]
        [ValidateNonEmpty("Insurer is a mandatory field")]
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

        /// <summary>
        /// The date which a client's existing Life Policy was ceded to SA Home Loans.
        /// </summary>
        [Property("DateCeded")]
        public virtual DateTime? DateCeded
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

        [Property("Consultant", ColumnType = "String")]
        public virtual string ConsultantADUserName
        {
            get
            {
                return this._consultantADUserName;
            }
            set
            {
                this._consultantADUserName = value;
            }
        }

        [BelongsTo("PriorityKey")]
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

        /// <summary>
        /// The Policy Holder is stored as a Legal Entity. This property refers to the LegalEntity.LegalEntityKey
        /// </summary>
        [BelongsTo("PolicyHolderLEKey")]
        public virtual LegalEntity_DAO PolicyHolderLegalEntity
        {
            get
            {
                return this._policyHolderLegalEntity;
            }
            set
            {
                this._policyHolderLegalEntity = value;
            }
        }

        /// <summary>
        /// The Life Policy issued by SA Life can replace existing policies which a Client may alrady have. In this a
        /// Replacement Policy Advice Record is required. This property is the Insurer which is being replaced.
        /// </summary>
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

        /// <summary>
        /// The number of the Life Policy that is being replaced.
        /// </summary>
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

        [OneToOne]
        public virtual Application_DAO Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
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
    }
}