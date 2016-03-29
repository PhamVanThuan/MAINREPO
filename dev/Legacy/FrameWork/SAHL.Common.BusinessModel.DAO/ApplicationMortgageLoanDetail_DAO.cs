using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    ///
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferMortgageLoan", Schema = "dbo", Lazy = true)]
    [Lurker]
    public partial class ApplicationMortgageLoanDetail_DAO : DB_2AM<ApplicationMortgageLoanDetail_DAO>
    {
        //private Double? _applicationAmount;

        private MortgageLoanPurpose_DAO _mortgageLoanPurpose;

        private ApplicantType_DAO _applicantType;

        private int? _numApplicants;

        //private DateTime? _homePurchaseDate;

        //private DateTime? _bondRegistrationDate;

        //private Double? _currentBondValue;

        //private DateTime? _deedsOfficeDate;

        //private string _bondFinancialInstitution;

        private Double? _purchasePrice;

        private ResetConfiguration_DAO _resetConfiguration;

        private Application_DAO _application;

        private int _applicationKey;

        private String _transferringAttorney;

        private Double? _clientEstimatePropertyValuation;
        private Property_DAO _property;
        private int? _dependentsPerHousehold;
        private int? _contributingDependents;

        private Language_DAO _language;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferKey")]
        public virtual int Key
        {
            get { return _applicationKey; }
            set { _applicationKey = value; }
        }

        //  ****** This is basically the same as the discriminated application type, we'll automatically set this based on the applicationtype
        /// <summary>
        /// This is the same as the discriminated Application Type and is set based on the ApplicationType.
        /// </summary>
        [Lurker]
        [BelongsTo("MortgageLoanPurposeKey")] //, ColumnType = "Int32")]
        public virtual MortgageLoanPurpose_DAO MortgageLoanPurpose
        {
            get
            {
                return this._mortgageLoanPurpose;
            }
            set
            {
                this._mortgageLoanPurpose = value;
            }
        }

        [Lurker]
        [OneToOne]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        /// <summary>
        /// The Applicant Type for the Application e.g. Single/Joint/Trust.
        /// </summary>
        [BelongsTo("ApplicantTypeKey")]
        public virtual ApplicantType_DAO ApplicantType
        {
            get
            {
                return this._applicantType;
            }
            set
            {
                this._applicantType = value;
            }
        }

        /// <summary>
        /// The link to the Property record stored in the Property table.
        /// </summary>
        [BelongsTo("PropertyKey", NotNull = false)]
        public virtual Property_DAO Property
        {
            get { return _property; }
            set { _property = value; }
        }

        /// <summary>
        /// The total number of Applicants on the Application.
        /// </summary>
        [Property("NumApplicants")]
        public virtual int? NumApplicants
        {
            get
            {
                return this._numApplicants;
            }
            set
            {
                this._numApplicants = value;
            }
        }

        /// <summary>
        /// The Transferring Attorney for the Application. This is the Attorney that transfers the property ownership from one
        /// individual to another. This Attorney is required for a New Purchase Application.
        /// </summary>
        [Property("TransferringAttorney", ColumnType = "String", NotNull = false)]
        public virtual String TransferringAttorney
        {
            get { return _transferringAttorney; }
            set { _transferringAttorney = value; }
        }

        /// <summary>
        /// The Reset Date for the Application. This is the date on which the base rate for the loan will be reset in order to
        /// reflect the current JIBAR rate and this is done on a quarterly basis on the following dates. New Applications for Origination
        /// will be sold on the 18th Reset (18/01,18/04,18/07 and 18/10)
        /// </summary>
        [BelongsTo("ResetConfigurationKey")]
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

        /// <summary>
        /// The Client's Estimated value of the Property.
        /// </summary>
        [Property("ClientEstimatePropertyValuation")]
        public virtual Double? ClientEstimatePropertyValuation
        {
            get { return _clientEstimatePropertyValuation; }
            set { _clientEstimatePropertyValuation = value; }
        }

        [Property("PurchasePrice")] //, ColumnType = "Double")]
        public virtual Double? PurchasePrice
        {
            get { return this._purchasePrice; }
            set { this._purchasePrice = value; }
        }

        /// <summary>
        /// The total number of dependents per household.
        /// </summary>
        [Property("DependentsPerHousehold")]
        public virtual int? DependentsPerHousehold
        {
            get
            {
                return this._dependentsPerHousehold;
            }
            set
            {
                this._dependentsPerHousehold = value;
            }
        }

        /// <summary>
        /// The total number of contributing dependents per household.
        /// </summary>
        [Property("ContributingDependents")]
        public virtual int? ContributingDependents
        {
            get
            {
                return this._contributingDependents;
            }
            set
            {
                this._contributingDependents = value;
            }
        }

        [BelongsTo("DocumentLanguageKey", NotNull = true)]
        [ValidateNonEmpty("Language is a mandatory field")]
        public virtual Language_DAO Language
        {
            get
            {
                // default this property to English
                if (this._language == null)
                    this._language = Language_DAO.Find(2);

                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        #region THESE ARE BEING HIDDEN FROM THE DOMAIN BECAUSE THEY DO NOT APPLY TO SAHL ONLY RCS, RCS NEEDS TO BE RETHOUGHT AND DESIGNED AT SOME STAGE IN A MAJOR WAY

        /*

        [Property("OfferAmount")] //, ColumnType = "Double")]
        public virtual Double? ApplicationAmount
        {
            get
            {
                return this._applicationAmount;
            }
            set
            {
                this._applicationAmount = value;
            }
        }

        [Property("HomePurchaseDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime? HomePurchaseDate
        {
            get
            {
                return this._homePurchaseDate;
            }
            set
            {
                this._homePurchaseDate = value;
            }
        }

        [Property("BondRegistrationDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime? BondRegistrationDate
        {
            get
            {
                return this._bondRegistrationDate;
            }
            set
            {
                this._bondRegistrationDate = value;
            }
        }

        [Property("CurrentBondValue")] //, ColumnType = "Double")]
        public virtual Double? CurrentBondValue
        {
            get
            {
                return this._currentBondValue;
            }
            set
            {
                this._currentBondValue = value;
            }
        }

        [Property("DeedsOfficeDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime? DeedsOfficeDate
        {
            get
            {
                return this._deedsOfficeDate;
            }
            set
            {
                this._deedsOfficeDate = value;
            }
        }

        [Property("BondFinancialInstitution", ColumnType = "String")]
        public virtual string BondFinancialInstitution
        {
            get
            {
                return this._bondFinancialInstitution;
            }
            set
            {
                this._bondFinancialInstitution = value;
            }
        }

         */

        #endregion THESE ARE BEING HIDDEN FROM THE DOMAIN BECAUSE THEY DO NOT APPLY TO SAHL ONLY RCS, RCS NEEDS TO BE RETHOUGHT AND DESIGNED AT SOME STAGE IN A MAJOR WAY
    }
}