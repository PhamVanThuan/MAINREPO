using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapTypeConfiguration_DAO is used when creating a new CAP Sales Configuration for a CAP Selling Period.
    /// </summary>
    /// <seealso cref="CapTypeConfigurationDetail_DAO"/>
    /// <seealso cref="CapType_DAO"/>
    /// <seealso cref="CapStatus_DAO"/>
    [ActiveRecord("CapTypeConfiguration", Schema = "dbo")]
    public partial class CapTypeConfiguration_DAO : DB_2AM<CapTypeConfiguration_DAO>
    {
        private System.DateTime _applicationStartDate;

        private System.DateTime _applicationEndDate;

        private GeneralStatus_DAO _generalStatus;

        private System.DateTime _capEffectiveDate;

        private System.DateTime _capClosureDate;

        private ResetConfiguration_DAO _resetConfiguration;

        private System.DateTime _resetDate;

        private int _term;

        private System.DateTime? _changeDate;

        private string _userID;

        private double? _nACQDiscount;

        private int _key;

        //private IList<CapApplication_DAO> _capApplications;

        private IList<CapTypeConfigurationDetail_DAO> _capTypeConfigurationDetails;

        /// <summary>
        /// The date on which the CAP Selling Period begins.
        /// </summary>
        [Property("OfferStartDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Application Start Date is a mandatory field")]
        public virtual System.DateTime ApplicationStartDate
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
        /// The date on which the CAP Selling Period ends.
        /// </summary>
        [Property("OfferEndDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Application End Date is a mandatory field")]
        public virtual System.DateTime ApplicationEndDate
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
        /// The status of the Sales Configuration
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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

        /// <summary>
        /// The date on which the CAP sold will become effective. This is on the next reset date the client reaches after
        /// accepting the CAP Offer.
        /// </summary>
        [Property("CapEffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Cap Effective Date is a mandatory field")]
        public virtual System.DateTime CapEffectiveDate
        {
            get
            {
                return this._capEffectiveDate;
            }
            set
            {
                this._capEffectiveDate = value;
            }
        }

        /// <summary>
        /// The Date on which the CAP ends. This is currently set at 24 months after the CapEffectiveDate.
        /// </summary>
        [Property("CapClosureDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Cap Closure Date is a mandatory field")]
        public virtual System.DateTime CapClosureDate
        {
            get
            {
                return this._capClosureDate;
            }
            set
            {
                this._capClosureDate = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the ResetConfiguration table, where the details regarding the next reset dates are stored.
        /// Each CapTypeConfiguration belongs to a single ResetConfiguration that determines whether the CAP is sold to 21st or 18th
        /// reset clients.
        /// </summary>
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

        /// <summary>
        /// The ResetDate which is applicable for the Cap Sales Configuration.
        /// </summary>
        [Property("ResetDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Reset Date is a mandatory field")]
        public virtual System.DateTime ResetDate
        {
            get
            {
                return this._resetDate;
            }
            set
            {
                this._resetDate = value;
            }
        }

        /// <summary>
        /// The term of the CAP. Currently this is 24 months.
        /// </summary>
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

        /// <summary>
        /// The date on which the CAP Configuration records were last changed.
        /// </summary>
        [Property("ChangeDate")]
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
        /// The UserID of the person who last updated the CAP Configuration records.
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

        [Property("NACQDiscount", ColumnType = "Double")]
        public virtual double? NACQDiscount
        {
            get
            {
                return this._nACQDiscount;
            }
            set
            {
                this._nACQDiscount = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "CapTypeConfigurationKey", ColumnType = "Int32")]
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

        // commented, this is configuration structures.
        //[HasMany(typeof(CapApplication_DAO), ColumnKey = "CapTypeConfigurationKey", Table = "CapOffer", Lazy = true)]
        //public virtual IList<CapApplication_DAO> CapApplicationsm
        //{
        //    get
        //    {
        //        return this._capApplications;
        //    }
        //    set
        //    {
        //        this._capApplications = value;
        //    }
        //}
        /// <summary>
        /// Each CapTypeConfiguration has many detail records in the CapTypeConfigurationDetail, where the individual admin fees/premiums
        /// per CapType for the sales configuration is stored.
        /// </summary>
        [HasMany(typeof(CapTypeConfigurationDetail_DAO), ColumnKey = "CapTypeConfigurationKey", Table = "CapTypeConfigurationDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CapTypeConfigurationDetail_DAO> CapTypeConfigurationDetails
        {
            get
            {
                return this._capTypeConfigurationDetails;
            }
            set
            {
                this._capTypeConfigurationDetails = value;
            }
        }
    }
}