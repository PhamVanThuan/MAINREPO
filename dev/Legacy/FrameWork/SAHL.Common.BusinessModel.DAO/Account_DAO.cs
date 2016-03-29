using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Account_DAO is the base class from which the product specific accounts are derived. The Account class has been discriminated
    /// according to the Product Type and has the following discriminations:
    /// <list type="bullet">
    /// <item>
    /// <description>Account Life Policy</description>
    /// </item>
    /// <item>
    /// <description>Account New Variable Loan</description>
    /// </item>
    /// <item>
    /// <description>Account Super Lo</description>
    /// </item>
    /// <item>
    /// <description>Account Variable Loan</description>
    /// </item>
    /// <item>
    /// <description>Account Varifix Loan</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <seealso cref="AccountLifePolicy_DAO"/>
    /// <seealso cref="AccountNewVariableLoan_DAO"/>
    /// <seealso cref="AccountSuperLo_DAO"/>
    /// <seealso cref="AccountVariableLoan_DAO"/>
    /// <seealso cref="AccountVariFixLoan_DAO"/>

    //[ActiveRecord("Account", Schema = "dbo")]
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorColumn = "RRR_ProductKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    [HideBaseClass]
    public class Account_DAO : DB_2AM<Account_DAO>
    {
        private double _fixedPayment;

        private AccountStatus_DAO _accountStatus;

        private System.DateTime _insertedDate;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        private DateTime? _openDate;

        private DateTime? _closeDate;

        // set by NHibernate - do not remove!
        private Product_DAO _product;

        // set by NHibernate - do not remove!
        private OriginationSource_DAO _originationSource;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _key;

        private IList<Role_DAO> _roles;

        private IList<Account_DAO> _relatedChildAccounts;

        protected IList<FinancialService_DAO> _financialServices;

        private IList<Subsidy_DAO> _subsidies;

        private IList<Application_DAO> _applications;

        private IList<MailingAddress_DAO> _mailingAddresses;

        private IList<AccountInformation_DAO> _accountInformations;

        private IList<Detail_DAO> _details;

        private IList<AccountBaselII_DAO> _AccountBaselII;

        private SPV_DAO _spv;

        //private IList<Account_DAO> _accounts;

        private Account_DAO _account;
        private IList<ExternalLifePolicy_DAO> _externalLifePolicy;

        [Property("FixedPayment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Fixed Payment is a mandatory field")]
        public virtual double FixedPayment
        {
            get
            {
                return this._fixedPayment;
            }
            set
            {
                this._fixedPayment = value;
            }
        }

        /// <summary>
        /// The status of the account.  When selecting open accounts, you will need to look at accounts with a status of Open or Dormant.
        /// When selecting closed accounts, you will need to look at accounts with a status of Closed or Locked.
        /// </summary>
        [BelongsTo("AccountStatusKey", NotNull = true)]
        [ValidateNonEmpty("Account Status is a mandatory field")]
        public virtual AccountStatus_DAO AccountStatus
        {
            get
            {
                return this._accountStatus;
            }
            set
            {
                this._accountStatus = value;
            }
        }

        /// <summary>
        /// The date when the account record was inserted.
        /// </summary>
        [Property("InsertedDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Inserted Date is a mandatory field")]
        public virtual System.DateTime InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        /// <summary>
        /// Each of the Origination Sources are related to products which they are allowed to sell to clients. The primary key
        /// from the OriginationSourceProduct table where this relationship is held is stored here for each account.
        /// </summary>
        [BelongsTo("OriginationSourceProductKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source Product is a mandatory field")]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }

        /// <summary>
        /// The date when the account was opened.
        /// </summary>
        [Property("OpenDate")]
        public virtual DateTime? OpenDate
        {
            get
            {
                return this._openDate;
            }
            set
            {
                this._openDate = value;
            }
        }

        /// <summary>
        /// The date when the account is closed. This remains a NULL value until the account is closed.
        /// </summary>
        [Property("CloseDate")]
        public virtual DateTime? CloseDate
        {
            get
            {
                return this._closeDate;
            }
            set
            {
                this._closeDate = value;
            }
        }

        /// <summary>
        /// The Account base class is discriminated based on the Product Type. The different product types include VariFix,
        /// New Variable, Super Lo, Defending Discount Rate and Life Policies.
        /// </summary>
        [BelongsTo("RRR_ProductKey", Access = PropertyAccess.FieldCamelcaseUnderscore, Insert = false, Update = false)]
        public virtual Product_DAO Product
        {
            get
            {
                return this._product;
            }
        }

        /// <summary>
        /// This is the source of the account which is who was responsible its origination. This would include SA Home Loans, RCS
        /// Home Loans, the Agency Channel or the Mortgage Originators.
        /// </summary>
        [BelongsTo("RRR_OriginationSourceKey", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual OriginationSource_DAO OriginationSource
        {
            get
            {
                return this._originationSource;
            }
        }

        /// <summary>
        /// The UserID of the last person who updated information on the Account.
        /// </summary>
        [Property("UserID", ColumnType = "String")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        /// <summary>
        /// The date when the Account record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "AccountKey", ColumnType = "Int32")]
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
        /// An Account can have one or many Legal Entities which play a specific Role in the Account. i.e. Main Applicant/Suretor
        /// </summary>
        [HasMany(typeof(Role_DAO), ColumnKey = "AccountKey", Table = "Role", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Role_DAO> Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                _roles = value;
            }
        }

        /// <summary>
        /// An Account can have many Financial Services. E.g. A VariFix account will have a Fixed Financial Service and a Variable Financial
        /// Service, where the payment due on each Financial Service will be recorded.
        /// </summary>
        [HasMany(typeof(FinancialService_DAO), ColumnKey = "AccountKey", Table = "FinancialService", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialService_DAO> FinancialServices
        {
            get { return _financialServices; }
            set { _financialServices = value; }
        }

        /// <summary>
        /// The applications from the Offer table related to the Account.
        /// </summary>
        [HasMany(typeof(Application_DAO), ColumnKey = "AccountKey", Table = "Offer", Lazy = true, Inverse = true)]
        public virtual IList<Application_DAO> Applications
        {
            get { return _applications; }
            set { _applications = value; }
        }

        /// <summary>
        /// This property retrieves the related child accounts through the use of the AccountRelationship table, where the AccountKey
        /// is equal to the AccountRelationship.AccountKey. The RelatedAccountKeys which are retrieved are the child accounts.
        /// </summary>
        [HasMany(typeof(Account_DAO), Table = "Account", ColumnKey = "ParentAccountKey", Lazy = true, Inverse = true)]
        public virtual IList<Account_DAO> RelatedChildAccounts
        {
            get { return _relatedChildAccounts; }
            set { _relatedChildAccounts = value; }
        }

        [BelongsTo("ParentAccountKey")]
        public virtual Account_DAO ParentAccount
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
        /// An Account can have many Subsidy Providers associated to it. This relationship is defined in the AccountSubsidy table where
        /// the AccountKey is equal to the AccountSubsidy.AccountKey. The SubsiderKeys which are retrieved are those Subsidy Providers related
        /// to the account.
        /// </summary>
        [HasAndBelongsToMany(typeof(Subsidy_DAO), Schema = "dbo", Table = "AccountSubsidy", ColumnKey = "AccountKey", ColumnRef = "SubsidyKey", Lazy = true)]
        public virtual IList<Subsidy_DAO> Subsidies
        {
            get { return _subsidies; }
            set { _subsidies = value; }
        }

        /// <summary>
        /// An Account has a Mailing Address to which correspondence is sent. The relationship is defined in the MailingAddress table where
        /// the AccountKey is equal to the MailingAddress.AcccountKey. The AddressKey which is retrieved is the Mailing Address for the
        /// Account.
        /// </summary>
        [HasMany(typeof(MailingAddress_DAO), Table = "MailingAddress", ColumnKey = "AccountKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<MailingAddress_DAO> MailingAddresses
        {
            get
            {
                return _mailingAddresses;
            }
            set
            {
                _mailingAddresses = value;
            }
        }

        /// <summary>
        /// Certain pieces of information regarding the Account are stored in the AccountInformation table. Currently these would include
        /// Product Opt In/Out and Conversions, the Account Legal Name and the Interest Only Maturity Date.
        /// </summary>
        [HasMany(typeof(AccountInformation_DAO), Table = "AccountInformation", ColumnKey = "AccountKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<AccountInformation_DAO> AccountInformations
        {
            get
            {
                return _accountInformations;
            }
            set
            {
                _accountInformations = value;
            }
        }

        ///// <summary>
        ///// An Account is related to a Property. This is currently being used by RCS.
        ///// </summary>
        //[HasAndBelongsToMany(typeof(Property_DAO), Table = "AccountProperty", ColumnKey = "AccountKey", ColumnRef = "PropertyKey", Lazy = true)]
        //public virtual IList<Property_DAO> AccountProperties
        //{
        //    get
        //    {
        //        return _accountProperties;
        //    }
        //    set
        //    {
        //        _accountProperties = value;
        //    }
        // }

        /// <summary>
        /// An Account has many Details.  These will eventually be removed as Detail Types are being made obsolete.
        /// </summary>
        [HasMany(typeof(Detail_DAO), ColumnKey = "AccountKey", Table = "Detail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Detail_DAO> Details
        {
            get
            {
                return this._details;
            }
            set
            {
                this._details = value;
            }
        }

        /// <summary>
        /// Basel Import Scores
        /// </summary>
        [HasMany(typeof(AccountBaselII_DAO), ColumnKey = "AccountKey", Table = "BaselImport", Lazy = true)]
        public virtual IList<AccountBaselII_DAO> AccountBaselII
        {
            get
            {
                return this._AccountBaselII;
            }
            set
            {
                this._AccountBaselII = value;
            }
        }

        [BelongsTo("SPVKey", NotNull = false)]
        public virtual SPV_DAO SPV
        {
            get
            {
                return this._spv;
            }
            set
            {
                this._spv = value;
            }
        }

        [Lurker]
        [HasAndBelongsToMany(typeof(ExternalLifePolicy_DAO), Table = "AccountExternalLife", ColumnKey = "AccountKey", ColumnRef = "ExternalLifePolicyKey", Lazy = true, Cascade = ManyRelationCascadeEnum.All)]
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