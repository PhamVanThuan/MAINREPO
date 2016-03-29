using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Application_DAO is the base class from which the Application Type specific Applications are derived.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorColumn = "OfferTypeKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    public partial class Application_DAO : DB_2AM<Application_DAO>
    {
        private DateTime? _applicationStartDate;

        private DateTime? _applicationEndDate;

        private Account_DAO _account;

        private string _reference;

        private int _key;

        private OriginationSource_DAO _originationSource;

        private IList<ApplicationCondition_DAO> _applicationConditions;

        private IList<ApplicationAttribute_DAO> _applicationAttributes;

        private ApplicationCampaign_DAO _applicationCampaign;

        private ApplicationStatus_DAO _applicationStatus;

        private ApplicationType_DAO _applicationType;

        private IList<ApplicationInformation_DAO> _applicationInformations;

        private IList<Callback_DAO> _callbacks;

        private IList<ApplicationMarketingSurveyType_DAO> _applicationMarketingSurveyTypes;

        private AccountSequence_DAO _ReservedAccount;

        private IList<ApplicationRole_DAO> _ApplicationRoles;

        private ApplicationSource_DAO _applicationSource;

        private IList<ApplicationMailingAddress_DAO> _applicationMailingAddresses;

        private IList<ApplicationDebitOrder_DAO> _applicationDebitOrders;

        private IList<Account_DAO> _relatedAccounts;

        private IList<ApplicationExpense_DAO> _applicationExpenses;

        private IList<Subsidy_DAO> _subsidies;

        private int? _estimateNumberApplicants;

        private IList<ExternalLifePolicy_DAO> _externalLifePolicy;

        /// <summary>
        /// The Account Key reserved for the Application.
        /// </summary>
        [BelongsTo("ReservedAccountKey", NotNull = true)]
        [ValidateNonEmpty("Reserved Account is a mandatory field")]
        public virtual AccountSequence_DAO ReservedAccount
        {
            get { return _ReservedAccount; }
            set { _ReservedAccount = value; }
        }

        [HasMany(typeof(Callback_DAO), Table = "CallBack", ColumnKey = "CallBackKey", Lazy = true, Inverse = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<Callback_DAO> Callbacks
        {
            get { return _callbacks; }
            set { _callbacks = value; }
        }

        /// <summary>
        /// The date on which the Application commenced.
        /// </summary>
        [Property("OfferStartDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime? ApplicationStartDate
        {
            get
            {
                return this._applicationStartDate;
            }
            set
            {
                this._applicationStartDate = value;
            }
        }

        /// <summary>
        /// The date on which the Application ends.
        /// </summary>
        [Property("OfferEndDate")] //, ColumnType = "Timestamp")]
        public virtual DateTime? ApplicationEndDate
        {
            get
            {
                return this._applicationEndDate;
            }
            set
            {
                this._applicationEndDate = value;
            }
        }

        /// <summary>
        /// Indicates the estimated number of applicants when the application is first captured.
        /// </summary>
        [Property("EstimateNumberApplicants", NotNull = false)]
        public virtual int? EstimateNumberApplicants
        {
            get { return _estimateNumberApplicants; }
            set { _estimateNumberApplicants = value; }
        }

        [BelongsTo("AccountKey", NotNull = false)]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        /// <summary>
        /// A Reference number for the Application.
        /// </summary>
        [Property("Reference", ColumnType = "String")]
        public virtual string Reference
        {
            get
            {
                return this._reference;
            }
            set
            {
                this._reference = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "OfferKey", ColumnType = "Int32")]
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

        // no longer exists.
        //[HasMany(typeof(OfferCampaignDetail_DAO), ColumnKey = "OfferKey", Table = "OfferCampaignDetail")]
        //[OneToOne]
        //public virtual OfferCampaignDetail_DAO ApplicationCampaignDetail
        //{
        //    get
        //    {
        //        return this._applicationCampaignDetail;
        //    }
        //    set
        //    {
        //        this._applicationCampaignDetail = value;
        //    }
        //}
        /// <summary>
        /// An Application can have a many Conditions associated to it. This relationship is defined in the OfferCondition table where the
        /// Offer.OfferKey = OfferCondition.OfferKey. The OfferCondition.ConditionKey's which are retrieved are the Conditions attached
        /// to the Application.
        /// </summary>
        [HasMany(typeof(ApplicationCondition_DAO), ColumnKey = "OfferKey", Table = "OfferCondition", Lazy = true)]
        public virtual IList<ApplicationCondition_DAO> ApplicationConditions
        {
            get
            {
                return this._applicationConditions;
            }
            set
            {
                this._applicationConditions = value;
            }
        }

        /// <summary>
        /// An Application can have a many Attributes associated to it. This relationship is defined in the OfferAttribute table where the
        /// Offer.OfferKey = OfferAttribute.OfferKey.
        /// </summary>
        [HasMany(typeof(ApplicationAttribute_DAO), ColumnKey = "OfferKey", Table = "OfferAttribute", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<ApplicationAttribute_DAO> ApplicationAttributes
        {
            get
            {
                return this._applicationAttributes;
            }
            set
            {
                this._applicationAttributes = value;
            }
        }

        /// <summary>
        /// An Application can have many Application Information records associated to it. The Application Information records are
        /// stored in the OfferInformation table
        /// </summary>
        [HasMany(typeof(ApplicationInformation_DAO), ColumnKey = "OfferKey", Table = "OfferInformation", OrderBy = "OfferInformationKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationInformation_DAO> ApplicationInformations
        {
            get { return _applicationInformations; }
            set { _applicationInformations = value; }
        }

        /// <summary>
        /// Many people can play a role in the Application throughout the Origination process. The relationship is defined in the
        /// OfferRole table where the Offer.OfferKey = OfferRole.OfferKey. The LegalEntityKey's that are retrieved are those which play a
        /// role in the Application, the OfferRoleTypeKey is the type of role they are playing e.g. Seller
        /// </summary>
        [Lurker]
        [HasMany(typeof(ApplicationRole_DAO), ColumnKey = "OfferKey", Table = "OfferRole", OrderBy = "OfferRoleKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationRole_DAO> ApplicationRoles
        {
            get
            {
                if (_ApplicationRoles == null)
                {
                    _ApplicationRoles = new List<ApplicationRole_DAO>();
                }
                return _ApplicationRoles;
            }
            set
            {
                _ApplicationRoles = value;
            }
        }

        /// <summary>
        /// Each Application is linked to a specific Campaign reference which business areas like Telecenter and SAHL Direct use
        /// as references.
        /// </summary>
        [BelongsTo("OfferCampaignKey")]
        public virtual ApplicationCampaign_DAO ApplicationCampaign
        {
            get
            {
                return this._applicationCampaign;
            }
            set
            {
                this._applicationCampaign = value;
            }
        }

        /// <summary>
        /// The Status of the Application (Open/Closed/Accepted/Declined/NTU'd)
        /// </summary>
        [BelongsTo("OfferStatusKey", NotNull = true)]
        [ValidateNonEmpty("Application Status is a mandatory field")]
        public virtual ApplicationStatus_DAO ApplicationStatus
        {
            get
            {
                return this._applicationStatus;
            }
            set
            {
                this._applicationStatus = value;
            }
        }

        /// <summary>
        /// The Origination Source of this application.
        /// </summary>
        [BelongsTo("OriginationSourceKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source is a mandatory field")]
        public virtual OriginationSource_DAO OriginationSource
        {
            get { return _originationSource; }
            set { _originationSource = value; }
        }

        /// <summary>
        /// Specifies where the application came from.
        /// </summary>
        [BelongsTo("OfferSourceKey", NotNull = false)]
        public virtual ApplicationSource_DAO ApplicationSource
        {
            get { return _applicationSource; }
            set { _applicationSource = value; }
        }

        /// <summary>
        /// Specifies the type of Application  e.g. Readvance, Further Loan etc.
        /// </summary>
        [BelongsTo("OfferTypeKey", NotNull = true, Access = PropertyAccess.FieldCamelcaseUnderscore, Insert = false, Update = false)]
        public virtual ApplicationType_DAO ApplicationType
        {
            get
            {
                return this._applicationType;
            }
        }

        [HasAndBelongsToMany(typeof(ApplicationMarketingSurveyType_DAO), ColumnRef = "OfferMarketingSurveyTypeKey", ColumnKey = "OfferKey", Schema = "dbo", Table = "OfferMarketingSurvey", Lazy = true)]
        public virtual IList<ApplicationMarketingSurveyType_DAO> ApplicationMarketingSurveyTypes
        {
            get
            {
                return this._applicationMarketingSurveyTypes;
            }
            set
            {
                this._applicationMarketingSurveyTypes = value;
            }
        }

        /// <summary>
        /// An Application has a Mailing Address to which correspondence can be sent. This relationship is defined in the OfferMailingAddress
        /// table where Offer.OfferKey = OfferMailingAddress.OfferKey. The AddressKey which is retrieved is the Mailing Address for the
        /// Application.
        /// </summary>
        [HasMany(typeof(ApplicationMailingAddress_DAO), ColumnKey = "OfferKey", Table = "OfferMailingAddress", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationMailingAddress_DAO> ApplicationMailingAddresses
        {
            get { return _applicationMailingAddresses; }
            set { _applicationMailingAddresses = value; }
        }

        [HasMany(typeof(ApplicationDebitOrder_DAO), ColumnKey = "OfferKey", Table = "OfferDebitOrder", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationDebitOrder_DAO> ApplicationDebitOrders
        {
            get { return _applicationDebitOrders; }
            set { _applicationDebitOrders = value; }
        }

        [HasAndBelongsToMany(typeof(Account_DAO), Table = "OfferAccountRelationship", ColumnKey = "OfferKey", ColumnRef = "AccountKey", Lazy = true, Cascade = ManyRelationCascadeEnum.All)]
        public virtual IList<Account_DAO> RelatedAccounts
        {
            get { return _relatedAccounts; }
            set { _relatedAccounts = value; }
        }

        [HasMany(typeof(ApplicationExpense_DAO), ColumnKey = "OfferKey", Table = "OfferExpense", Inverse = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true)]
        public virtual IList<ApplicationExpense_DAO> ApplicationExpenses
        {
            get
            {
                return this._applicationExpenses;
            }
            set
            {
                this._applicationExpenses = value;
            }
        }

        [HasAndBelongsToMany(typeof(Subsidy_DAO), Schema = "dbo", Table = "OfferSubsidy", ColumnKey = "OfferKey", ColumnRef = "SubsidyKey", Lazy = true)]
        public virtual IList<Subsidy_DAO> Subsidies
        {
            get { return _subsidies; }
            set { _subsidies = value; }
        }

        [Lurker]
        [HasAndBelongsToMany(typeof(ExternalLifePolicy_DAO), Table = "OfferExternalLife", ColumnKey = "OfferKey", ColumnRef = "ExternalLifePolicyKey", Lazy = true, Cascade = ManyRelationCascadeEnum.All)]
        public virtual IList<ExternalLifePolicy_DAO> ExternalLifePolicy
        {
            get
            {
                return _externalLifePolicy;
            }
            set
            {
                _externalLifePolicy = value;
            }
        }
    }
}