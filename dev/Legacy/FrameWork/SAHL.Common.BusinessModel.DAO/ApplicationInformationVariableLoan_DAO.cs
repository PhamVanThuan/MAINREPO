using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using System;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationVariableLoan_DAO is instantiated in order to retrieve those details specific to a Variable Loan
    /// Application.
    /// </summary>
    [GenericTest(TestType = Globals.TestType.Find,
        Description = @"This specific DAO has an issue when it comes to doing a normal Load Save Load, without changing our Load Save Load tests,
					  The issue is that the OfferInformation is a Foreign Key and because it's a foreign key, the load save load tires to find the first entry to link to in that list
					  And tries to set the DAO's primary key to that and then save it.
					  The problem is that the DAO was newed up and NHibernate is not seeing this as an udpate but a new entry.")]
    [ActiveRecord("OfferInformationVariableLoan", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationVariableLoan_DAO : DB_2AM<ApplicationInformationVariableLoan_DAO>
    {
        private Category_DAO _category;

        private int? _term;

        //private int? _rateConfigurationKey;

        private Double? _existingLoan;

        private Double? _cashDeposit;

        private Double? _propertyValuation;

        private Double? _householdIncome;

        private Double? _feesTotal;

        private Double? _interimInterest;

        private Double? _monthlyInstalment;

        //private Double? _lifePremium;

        //private Double? _hOCPremium;

        //private Double? _minLoanRequired;

        //private Double? _minBondRequired;

        //private Double? _preApprovedAmount;

        //private Double? _minCashAllowed;

        //private Double? _maxCashAllowed;

        private Double? _requestedCashAmount;

        private Double? _loanAgreementAmount;

        private Double? _bondToRegister;

        private Double? _lTV;

        private Double? _pTI;

        private Double? _marketRate;

        private SPV_DAO _sPV;

        private EmploymentType_DAO _employmentType;

        private ApplicationInformation_DAO _applicationInformation;

        private int _applicationInformationKey;

        private CreditMatrix_DAO _creditMatrix;

        private CreditCriteria_DAO _creditCriteria;

        private RateConfiguration_DAO _rateConfiguration;

        private Double? _loanAmountNoFees;

        private Double? _appliedInitiationFeeDiscount;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferInformationKey")]
        public virtual int Key
        {
            get { return _applicationInformationKey; }
            set { _applicationInformationKey = value; }
        }

        /// <summary>
        /// Each Variable Loan is assigned to a specific Category. This Category (from 1-5) is determined by the Credit Matrix
        /// and takes into account factors such as the employment type, monthly income, LTV/PTI and the value of the loan.
        /// </summary>
        [BelongsTo("CategoryKey", NotNull = false)]
        public virtual Category_DAO Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        /// <summary>
        /// The term of the loan. The maximum allowed is 30 years (360 months)
        /// </summary>
        [Property("Term")] //, ColumnType = "Int32")]
        public virtual int? Term
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

        /// <summary>
        /// The value of the client's existing loan, in the case of a Switch Loan Application.
        /// </summary>
        [Property("ExistingLoan")] //, ColumnType = "Double")]
        public virtual Double? ExistingLoan
        {
            get
            {
                return this._existingLoan;
            }
            set
            {
                this._existingLoan = value;
            }
        }

        /// <summary>
        /// The value of the Cash Deposit the client makes on a New Purchase Loan Application.
        /// </summary>
        [Property("CashDeposit")] //, ColumnType = "Double")]
        public virtual Double? CashDeposit
        {
            get
            {
                return this._cashDeposit;
            }
            set
            {
                this._cashDeposit = value;
            }
        }

        /// <summary>
        /// The value of the Property to be bonded.
        /// </summary>
        [Lurker]
        [Property("PropertyValuation")] //, ColumnType = "Double")]
        public virtual Double? PropertyValuation
        {
            get
            {
                return this._propertyValuation;
            }
            set
            {
                this._propertyValuation = value;
            }
        }

        /// <summary>
        /// The total Household Income for the Application.
        /// </summary>
        [Property("HouseholdIncome")] //, ColumnType = "Double")]
        public virtual Double? HouseholdIncome
        {
            get
            {
                return this._householdIncome;
            }
            set
            {
                this._householdIncome = value;
            }
        }

        /// <summary>
        /// The sum total of all the Fees applicable to the Application. Consists of fees such as the Cancellation Fee, an Initiation Fee
        /// and the Registration Fee.
        /// </summary>
        [Lurker]
        [Property("FeesTotal")] //, ColumnType = "Double")]
        public virtual Double? FeesTotal
        {
            get
            {
                return this._feesTotal;
            }
            set
            {
                this._feesTotal = value;
            }
        }

        /// <summary>
        /// The Interest Provision made to cater for interest charged by the bank where the existing mortgage loan is held. This
        /// applies to Switch Loan Applications. This is to ensure that are sufficient funds to settle the outstanding balance on
        /// the existing loan at Disbursement.
        /// </summary>
        [Property("InterimInterest")] //, ColumnType = "Double")]
        public virtual Double? InterimInterest
        {
            get
            {
                return this._interimInterest;
            }
            set
            {
                this._interimInterest = value;
            }
        }

        /// <summary>
        /// The Monthly Instalment which would be due by the client if the Application is approved.
        /// </summary>
        [Property("MonthlyInstalment")] //, ColumnType = "Double")]
        public virtual Double? MonthlyInstalment
        {
            get
            {
                return this._monthlyInstalment;
            }
            set
            {
                this._monthlyInstalment = value;
            }
        }

        /// <summary>
        /// The value of the Loan which the client wants
        /// </summary>
        [Lurker]
        [Property("LoanAgreementAmount", Access = PropertyAccess.FieldCamelcaseUnderscore)] //, ColumnType = "Double")]
        public virtual Double? LoanAgreementAmount
        {
            get
            {
                return this._loanAgreementAmount;
            }
            set
            {
                this._loanAgreementAmount = value;
            }
        }

        /// <summary>
        /// The value of the Bond which the client wishes to register at the Deeds Office.
        /// </summary>
        [Property("BondToRegister")] //, ColumnType = "Double")]
        public virtual Double? BondToRegister
        {
            get
            {
                return this._bondToRegister;
            }
            set
            {
                this._bondToRegister = value;
            }
        }

        /// <summary>
        /// The value of the Loan-to-Value calculation. This is the ratio of the Loan Required to the Current Valuation on the
        /// Property.
        /// </summary>
        [Property("LTV")] //, ColumnType = "Double")]
        public virtual Double? LTV
        {
            get
            {
                return this._lTV;
            }
            set
            {
                this._lTV = value;
            }
        }

        /// <summary>
        /// The value of the Payment-to-Income calculation. This is the ratio of the Monthly Instalment to the Household Income.
        /// </summary>
        [Property("PTI")] //, ColumnType = "Double")]
        public virtual Double? PTI
        {
            get
            {
                return this._pTI;
            }
            set
            {
                this._pTI = value;
            }
        }

        /// <summary>
        /// The Market Rate applicable to the Loan.
        /// </summary>
        [Property("MarketRate")] //, ColumnType = "Double")]
        public virtual Double? MarketRate
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

        /// <summary>
        /// The Loan Amount with No Fees
        /// </summary>
        [Lurker]
        [Property("LoanAmountNoFees")]
        public virtual Double? LoanAmountNoFees
        {
            get
            {
                return this._loanAmountNoFees;
            }
            set
            {
                this._loanAmountNoFees = value;
            }
        }

        /// <summary>
        /// The percentage discount applied to the initiation fee
        /// </summary>
        [Lurker]
        [Property("AppliedInitiationFeeDiscount")]
        public virtual Double? AppliedInitiationFeeDiscount
        {
            get
            {
                return this._appliedInitiationFeeDiscount;
            }
            set
            {
                this._appliedInitiationFeeDiscount = value;
            }
        }

        /// <summary>
        /// The SPV from which the loan will be issued from if the Application is approved.
        /// </summary>
        [BelongsTo("SPVKey", NotNull = false)] //, ColumnType = "Int32")]
        public virtual SPV_DAO SPV
        {
            get
            {
                return this._sPV;
            }
            set
            {
                this._sPV = value;
            }
        }

        /// <summary>
        /// An Application is assigned an employment type e.g. Salaried or Self Employed. The Employment Type applicable to an
        /// Application is determined by calculating which Employment Type contributes the most to the total Household Income.
        /// </summary>
        [BelongsTo("EmploymentTypeKey", NotNull = false)] //, ColumnType = "Int32")]
        public virtual EmploymentType_DAO EmploymentType
        {
            get
            {
                return this._employmentType;
            }
            set
            {
                this._employmentType = value;
            }
        }

        [Lurker]
        [OneToOne]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get
            {
                return this._applicationInformation;
            }
            set
            {
                this._applicationInformation = value;
            }
        }

        /// <summary>
        /// The Credit Matrix version on which the Application is approved.
        /// </summary>
        [BelongsTo("CreditMatrixKey", NotNull = false)]
        public virtual CreditMatrix_DAO CreditMatrix
        {
            get
            {
                return this._creditMatrix;
            }
            set
            {
                this._creditMatrix = value;
            }
        }

        /// <summary>
        /// The Credit Criteria version on which the Application is approved.
        /// </summary>
        [BelongsTo("CreditCriteriaKey", NotNull = false)]
        public virtual CreditCriteria_DAO CreditCriteria
        {
            get
            {
                return this._creditCriteria;
            }
            set
            {
                this._creditCriteria = value;
            }
        }

        /// <summary>
        /// Each Application is assigned a Rate Configuration. This allows the retrieval of the Market Rate and the Margin (Link Rate)
        /// applicable to the Application.
        /// </summary>
        [BelongsTo("RateConfigurationKey", NotNull = false)]
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

        /// <summary>
        /// A Refinance Application requires the client to request a certain amount of Cash. This property can also exist on a
        /// Switch Loan Application if the client wishes to have Cash Out.
        /// </summary>
        [Lurker]
        [Property("RequestedCashAmount")] //, ColumnType = "Double")]
        public virtual Double? RequestedCashAmount
        {
            get
            {
                return this._requestedCashAmount;
            }
            set
            {
                this._requestedCashAmount = value;
            }
        }

        #region THESE ARE BEING HIDDEN FROM THE DOMAIN BECAUSE THEY DO NOT APPLY TO SAHL ONLY RCS, RCS NEEDS TO BE RETHOUGHT AND DESIGNED AT SOME STAGE IN A MAJOR WAY

        /*
        [Property("LifePremium")] //, ColumnType = "Double")]
        public virtual Double? LifePremium
        {
            get
            {
                return this._lifePremium;
            }
            set
            {
                this._lifePremium = value;
            }
        }

        [Property("HOCPremium")] //, ColumnType = "Double")]
        public virtual Double? HOCPremium
        {
            get
            {
                return this._hOCPremium;
            }
            set
            {
                this._hOCPremium = value;
            }
        }

        [Property("MinLoanRequired")] //, ColumnType = "Double")]
        public virtual Double? MinLoanRequired
        {
            get
            {
                return this._minLoanRequired;
            }
            set
            {
                this._minLoanRequired = value;
            }
        }

        [Property("MinBondRequired")] //, ColumnType = "Double")]
        public virtual Double? MinBondRequired
        {
            get
            {
                return this._minBondRequired;
            }
            set
            {
                this._minBondRequired = value;
            }
        }

        [Property("PreApprovedAmount")] //, ColumnType = "Double")]
        public virtual Double? PreApprovedAmount
        {
            get
            {
                return this._preApprovedAmount;
            }
            set
            {
                this._preApprovedAmount = value;
            }
        }

        [Property("MinCashAllowed")] //, ColumnType = "Double")]
        public virtual Double? MinCashAllowed
        {
            get
            {
                return this._minCashAllowed;
            }
            set
            {
                this._minCashAllowed = value;
            }
        }

        [Property("MaxCashAllowed")] //, ColumnType = "Double")]
        public virtual Double? MaxCashAllowed
        {
            get
            {
                return this._maxCashAllowed;
            }
            set
            {
                this._maxCashAllowed = value;
            }
        }
       */

        #endregion THESE ARE BEING HIDDEN FROM THE DOMAIN BECAUSE THEY DO NOT APPLY TO SAHL ONLY RCS, RCS NEEDS TO BE RETHOUGHT AND DESIGNED AT SOME STAGE IN A MAJOR WAY

        public virtual void Clone(ApplicationInformationVariableLoan_DAO VL)
        {
            VL.BondToRegister = this.BondToRegister;
            VL.CashDeposit = this.CashDeposit;
            VL.Category = this.Category;
            VL.CreditMatrix = this.CreditMatrix;
            VL.EmploymentType = this.EmploymentType;
            VL.ExistingLoan = this.ExistingLoan;
            VL.FeesTotal = this.FeesTotal;
            VL.HouseholdIncome = this.HouseholdIncome;
            VL.InterimInterest = this.InterimInterest;
            VL.LoanAgreementAmount = this.LoanAgreementAmount;
            VL.LTV = this.LTV;
            VL.MarketRate = this.MarketRate;
            VL.MonthlyInstalment = this.MonthlyInstalment;
            VL.PropertyValuation = this.PropertyValuation;
            VL.PTI = this.PTI;
            VL.RateConfiguration = this.RateConfiguration;
            VL.RequestedCashAmount = this.RequestedCashAmount;
            VL.SPV = this.SPV;
            VL.Term = this.Term;
            VL._loanAmountNoFees = this.LoanAmountNoFees;
            VL.CreditCriteria = this.CreditCriteria;
            VL.AppliedInitiationFeeDiscount = this.AppliedInitiationFeeDiscount;
        }
    }
}