using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntity_DAO is the base class from which Legal Entity Type specific Legal Entities are derived. Legal Entities are
    /// discriminated based on the LegalEntityType and has the following discriminations:
    /// <list type="bullet">
    /// <item>
    /// <description>Natural Person</description>
    /// </item>
    /// <item>
    /// <description>Close Corporation</description>
    /// </item>
    /// <item>
    /// <description>Trust</description>
    /// </item>
    /// <item>
    /// <description>Company</description>
    /// </item>
    /// <item>
    /// <description>Unknown</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <seealso cref="LegalEntityCloseCorporation_DAO"/>
    /// <seealso cref="LegalEntityCompany_DAO"/>
    /// <seealso cref="LegalEntityTrust_DAO"/>
    /// <seealso cref="LegalEntityUnknown_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntity", Schema = "dbo", DiscriminatorColumn = "LegalEntityTypeKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    [HideBaseClass]
    public partial class LegalEntity_DAO : DB_2AM<LegalEntity_DAO>
    {
        private System.DateTime _introductionDate;

        private string _taxNumber;

        private string _homePhoneCode;

        private string _homePhoneNumber;

        private string _workPhoneCode;

        private string _workPhoneNumber;

        private string _cellPhoneNumber;

        private string _emailAddress;

        private string _faxCode;

        private string _faxNumber;

        private string _password;

        private string _comments;

        private string _userID;

        private System.DateTime? _changeDate;

        private Language_DAO _documentLanguage;

        private ResidenceStatus_DAO _residenceStatus;

        private int _key;

        //private LegalEntityType_DAO _legalEntityType;

        //private IList<LegalEntityMemo_DAO> _legalEntityMemos;

        private IList<LegalEntityAddress_DAO> _legalEntityAddresses;

        private IList<FailedLegalEntityAddress_DAO> _failedLegalEntityAddresses;

        private IList<Employment_DAO> _legalEntityEmployment;

        private IList<LegalEntityAffordability_DAO> _legalEntityAffordabilities;

        private IList<LegalEntityAssetLiability_DAO> _legalEntityAssetLiabilities;

        private IList<LegalEntityBankAccount_DAO> _legalEntityBankAccounts;

        private IList<LegalEntityMarketingOption_DAO> _legalEntityMarketingOptions;

        private IList<LegalEntityRelationship_DAO> _legalEntityRelationships;

        private LegalEntityExceptionStatus_DAO _legalEntityExceptionStatus;

        private LegalEntityStatus_DAO _legalEntityStatus;

        //private IList<LegalEntityExceptionReason_DAO> _legalEntities;

        private IList<Role_DAO> _roles;

        private IList<ApplicationRole_DAO> _applicationRoles;

        private IList<DebtCounsellorDetail_DAO> _debtCounsellorDetails;

        private IList<LegalEntityLogin_DAO> _legalEntityLogins;

        /// <summary>
        /// The date on which the Legal Entity was first introduced to SA Home Loans
        /// </summary>
        [Property("IntroductionDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Introduction Date is a mandatory field")]
        public virtual System.DateTime IntroductionDate
        {
            get
            {
                return this._introductionDate;
            }
            set
            {
                this._introductionDate = value;
            }
        }

        /// <summary>
        /// The Legal Entity's Tax Number
        /// </summary>
        [Property("TaxNumber", ColumnType = "String")]
        public virtual string TaxNumber
        {
            get
            {
                return this._taxNumber;
            }
            set
            {
                this._taxNumber = value;
            }
        }

        /// <summary>
        /// The Area Code for the Legal Entity's Home Phone Number
        /// </summary>
        [Property("HomePhoneCode", ColumnType = "String", Length = 10)]
        public virtual string HomePhoneCode
        {
            get
            {
                return this._homePhoneCode;
            }
            set
            {
                this._homePhoneCode = value;
            }
        }

        /// <summary>
        /// The Home Phone Number
        /// </summary>
        [Property("HomePhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string HomePhoneNumber
        {
            get
            {
                return this._homePhoneNumber;
            }
            set
            {
                this._homePhoneNumber = value;
            }
        }

        /// <summary>
        /// The Area Code for the Legal Entity's Work Phone Number
        /// </summary>
        [Property("WorkPhoneCode", ColumnType = "String", Length = 10)]
        public virtual string WorkPhoneCode
        {
            get
            {
                return this._workPhoneCode;
            }
            set
            {
                this._workPhoneCode = value;
            }
        }

        /// <summary>
        /// The Work Phone Number
        /// </summary>
        [Property("WorkPhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string WorkPhoneNumber
        {
            get
            {
                return this._workPhoneNumber;
            }
            set
            {
                this._workPhoneNumber = value;
            }
        }

        /// <summary>
        /// The Legal Entity's Cell Phone Number including the Code.
        /// </summary>
        [Property("CellPhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string CellPhoneNumber
        {
            get
            {
                return this._cellPhoneNumber;
            }
            set
            {
                this._cellPhoneNumber = value;
            }
        }

        /// <summary>
        /// The Legal Entity's Email Address
        /// </summary>
        [Property("EmailAddress", ColumnType = "String")]
        public virtual string EmailAddress
        {
            get
            {
                return this._emailAddress;
            }
            set
            {
                this._emailAddress = value;
            }
        }

        /// <summary>
        /// The Area code for the Legal Entity's Fax Number
        /// </summary>
        [Property("FaxCode", ColumnType = "String", Length = 10)]
        public virtual string FaxCode
        {
            get
            {
                return this._faxCode;
            }
            set
            {
                this._faxCode = value;
            }
        }

        /// <summary>
        /// The Fax Number
        /// </summary>
        [Property("FaxNumber", ColumnType = "String", Length = 15)]
        public virtual string FaxNumber
        {
            get
            {
                return this._faxNumber;
            }
            set
            {
                this._faxNumber = value;
            }
        }

        /// <summary>
        /// A Password chosen by the Legal Entity in order access Loan information online.
        /// </summary>
        [Property("Password", ColumnType = "String")]
        public virtual string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        /// <summary>
        /// A Comments Property
        /// </summary>
        [Property("Comments", ColumnType = "String")]
        public virtual string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        /// <summary>
        /// The UserID of the person who last updated the Legal Entity record.
        /// </summary>
        [Property("UserID", ColumnType = "String", NotNull = false)]
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
        /// The date on which the Legal Entity record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = false)]
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
        /// The preferred language in which the Legal Entity receives documentation.
        /// </summary>
        [BelongsTo("DocumentLanguageKey", NotNull = true)]
        [ValidateNonEmpty("Document Language is a mandatory field")]
        public virtual Language_DAO DocumentLanguage
        {
            get
            {
                return this._documentLanguage;
            }
            set
            {
                this._documentLanguage = value;
            }
        }

        /// <summary>
        /// The South African Residence status of the Legal Entity.
        /// </summary>
        [BelongsTo("ResidenceStatusKey", NotNull = false)]
        public virtual ResidenceStatus_DAO ResidenceStatus
        {
            get
            {
                return this._residenceStatus;
            }
            set
            {
                this._residenceStatus = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityKey", ColumnType = "Int32")]
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

        // removed as the generic memo structures should be used.
        //[HasMany(typeof(LegalEntityMemo_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityMemo", Lazy = true)]
        //public virtual IList<LegalEntityMemo_DAO> LegalEntityMemos
        //{
        //    get
        //    {
        //        return this._legalEntityMemos;
        //    }
        //    set
        //    {
        //        this._legalEntityMemos = value;
        //    }
        //}
        /// <summary>
        /// A Legal Entity can have many Addresses. This relationship is defined in the LegalEntityAddress table.
        /// </summary>
        [HasMany(typeof(LegalEntityAddress_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityAddress", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityAddress_DAO> LegalEntityAddresses
        {
            get
            {
                return this._legalEntityAddresses;
            }
            set
            {
                this._legalEntityAddresses = value;
            }
        }

        /// <summary>
        /// Provides a list of dirty legal entity addresses.  This list should never be added to: it contains old
        /// records that could not be successfully migrated and required cleaning up.  Over time, these should be
        /// replaced with valid LegalEntityAddress objects and their IsCleaned properties set to true.
        /// </summary>
        [HasMany(typeof(FailedLegalEntityAddress_DAO), ColumnKey = "LegalEntityKey", Schema = "mig", Table = "FailedLegalEntityAddress", Lazy = true, Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Where = "((FailedStreetMigrationKey is not null and ISNULL(IsCleaned, 0) = 0) or (FailedPostalMigrationKey is not null and ISNULL(PostalIsCleaned, 0) = 0))")]
        public virtual IList<FailedLegalEntityAddress_DAO> FailedLegalEntityAddresses
        {
            get
            {
                return this._failedLegalEntityAddresses;
            }
            set
            {
                this._failedLegalEntityAddresses = value;
            }
        }

        /// <summary>
        /// A Legal Entity can have many Employment records. The Employment table stores a Foreign Key Reference to the LegalEntityKey.
        /// </summary>
        [HasMany(typeof(Employment_DAO), ColumnKey = "LegalEntityKey", Table = "Employment", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Employment_DAO> Employment
        {
            get
            {
                return this._legalEntityEmployment;
            }
            set
            {
                this._legalEntityEmployment = value;
            }
        }

        /// <summary>
        /// The details of the Affordability Assessment for the Legal Entity is required to be stored. A Legal Entity has a set of Affordability entries
        /// which is related to the Legal Entity by way of the LegalEntityAffordability table, which stores a foreign key reference
        /// to the Legal Entity.
        /// </summary>
        [HasMany(typeof(LegalEntityAffordability_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityAffordability", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityAffordability_DAO> LegalEntityAffordabilities
        {
            get
            {
                return this._legalEntityAffordabilities;
            }
            set
            {
                this._legalEntityAffordabilities = value;
            }
        }

        /// <summary>
        /// In certain cases the Legal Entity is required to provide Asset and Liability information. A Legal Entity will have a set
        /// of Asset/Liability records which is related to the Legal Entity by way of the LegalEntityAssetLiability table, which stores
        /// a foreign key reference to the Legal Entity.
        /// </summary>
        [HasMany(typeof(LegalEntityAssetLiability_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityAssetLiability", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityAssetLiability_DAO> LegalEntityAssetLiabilities
        {
            get
            {
                return this._legalEntityAssetLiabilities;
            }
            set
            {
                this._legalEntityAssetLiabilities = value;
            }
        }

        /// <summary>
        /// A Legal Entity can have many Bank Account records. This relationship is defined in the LegalEntityBankAccount table,
        /// which has a foreign key reference to the Legal Entity.
        /// </summary>
        [HasMany(typeof(LegalEntityBankAccount_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityBankAccount", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityBankAccount_DAO> LegalEntityBankAccounts
        {
            get
            {
                return this._legalEntityBankAccounts;
            }
            set
            {
                this._legalEntityBankAccounts = value;
            }
        }

        /// <summary>
        /// A Legal Entity can choose many options by which SAHL can market new products to the client. This relationship is defined
        /// in the LegalEntityMarketingOption table, which has a foreign key reference to the Legal Entity.
        /// </summary>
        [HasMany(typeof(LegalEntityMarketingOption_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityMarketingOption", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityMarketingOption_DAO> LegalEntityMarketingOptions
        {
            get
            {
                return this._legalEntityMarketingOptions;
            }
            set
            {
                this._legalEntityMarketingOptions = value;
            }
        }

        /// <summary>
        /// We are assuming the relationship exist in the relationship table
        /// </summary>

        [HasMany(typeof(LegalEntityRelationship_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityRelationship", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LegalEntityRelationship_DAO> LegalEntityRelationships
        {
            get
            {
                return this._legalEntityRelationships;
            }
            set
            {
                this._legalEntityRelationships = value;
            }
        }

        /// <summary>
        /// This is the result of the validation of the Legal Entity ID Number validation (Valid, Duplicate ID Numbers or Invalid)
        /// </summary>
        [BelongsTo("LegalEntityExceptionStatusKey", NotNull = false)]
        public virtual LegalEntityExceptionStatus_DAO LegalEntityExceptionStatus
        {
            get
            {
                return this._legalEntityExceptionStatus;
            }
            set
            {
                this._legalEntityExceptionStatus = value;
            }
        }

        /// <summary>
        /// This is the foreign key reference to the status of the Legal Entity from the LegalEntityStatus table. e.g. Alive, Deceased
        /// or Disabled.
        /// </summary>
        [BelongsTo("LegalEntityStatusKey", NotNull = false)]
        public virtual LegalEntityStatus_DAO LegalEntityStatus
        {
            get
            {
                return this._legalEntityStatus;
            }
            set
            {
                this._legalEntityStatus = value;
            }
        }

        // TODO: Create relationship
        //[HasAndBelongsToMany(typeof(LegalEntityExceptionReason), ColumnRef = "LegalEntityExceptionReasonKey", ColumnKey = "LegalEntityKey", Schema = "dbo", Table = "LegalEntityException")]
        //public virtual IList<LegalEntityExceptionReason> LegalEntities
        //{
        //    get
        //    {
        //        return this._legalEntities;
        //    }
        //    set
        //    {
        //        this._legalEntities = value;
        //    }
        //}
        /// <summary>
        /// A Legal Entity can play many Roles in many different Accounts at SA Home Loans. The Role table stores a foreign key
        /// reference to the Legal Entity and relates the Legal Entity to the RoleType and the Account.
        /// </summary>
        [HasMany(typeof(Role_DAO), ColumnKey = "LegalEntityKey", Table = "Role", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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
        /// A Legal Entity can play many Roles in many different Applications at SA Home Loans. The OfferRole table stores a foreign key
        /// reference to the Legal Entity and links it to the Application.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///     <item><description>Cascading switched off as the cascade comes from the offer and this cannot be in both.</description></item>
        ///     <item><description>Care needs to be taken when using this method to avoid performance problems - this collection can get huge when a legal entity is a consultant AND a client.  Use <c>GetApplicationRoles*</c> methods instead.</description></item>
        /// </list>
        /// </remarks>
        [HasMany(typeof(ApplicationRole_DAO), ColumnKey = "LegalEntityKey", Table = "OfferRole", Lazy = true, Cascade = ManyRelationCascadeEnum.None, Inverse = true)]
        public virtual IList<ApplicationRole_DAO> ApplicationRoles
        {
            get { return _applicationRoles; }
            set { _applicationRoles = value; }
        }

        [Lurker]
        [HasMany(typeof(DebtCounsellorDetail_DAO), ColumnKey = "LegalEntityKey", Schema = "DebtCounselling", Table = "DebtCounsellorDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<DebtCounsellorDetail_DAO> DebtCounsellorDetails
        {
            get
            {
                return this._debtCounsellorDetails;
            }
            set
            {
                this._debtCounsellorDetails = value;
            }
        }

        [Lurker]
        [HasMany(typeof(LegalEntityLogin_DAO), ColumnKey = "LegalEntityKey", Table = "LegalEntityLogin", Lazy = true)]
        public virtual IList<LegalEntityLogin_DAO> LegalEntityLogins
        {
            get
            {
                return this._legalEntityLogins;
            }
            set
            {
                this._legalEntityLogins = value;
            }
        }
    }
}