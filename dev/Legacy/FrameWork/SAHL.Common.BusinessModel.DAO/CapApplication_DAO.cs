using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapOffer_DAO is instantiated in order to represent a CapOffer in the system, where a client will be offered the CAP 2 product.
    /// </summary>
    [ActiveRecord("CapOffer", Schema = "dbo", Lazy = true)]
    public partial class CapApplication_DAO : DB_2AM<CapApplication_DAO>
    {
        private int _remainingInstallments;

        private double _currentBalance;

        private double _currentInstallment;

        private double _linkRate;

        private System.DateTime _applicationDate;

        private bool? _promotion;

        private System.DateTime? _capitalisationDate;

        private System.DateTime _changeDate;

        private string _userID;

        private int _key;

        private IList<CapApplicationDetail_DAO> _capApplicationDetails;

        private Account_DAO _account;

        private Broker_DAO _broker;

        private CapStatus_DAO _capStatus;

        private CapTypeConfiguration_DAO _capTypeConfiguration;

        private CapPaymentOption_DAO _cAPPaymentOption;

        /// <summary>
        /// The remaining instalments on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        [Property("RemainingInstallments", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Remaining Installments is a mandatory field")]
        public virtual int RemainingInstallments
        {
            get
            {
                return this._remainingInstallments;
            }
            set
            {
                this._remainingInstallments = value;
            }
        }

        /// <summary>
        /// The outstanding balance on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        [Property("CurrentBalance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Current Balance is a mandatory field")]
        public virtual double CurrentBalance
        {
            get
            {
                return this._currentBalance;
            }
            set
            {
                this._currentBalance = value;
            }
        }

        /// <summary>
        /// The current instalment due on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        [Property("CurrentInstallment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Current Installment is a mandatory field")]
        public virtual double CurrentInstallment
        {
            get
            {
                return this._currentInstallment;
            }
            set
            {
                this._currentInstallment = value;
            }
        }

        /// <summary>
        /// The loan link rate at the time of the CapOffer being calculated.
        /// </summary>
        [Property("LinkRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Link Rate is a mandatory field")]
        public virtual double LinkRate
        {
            get
            {
                return this._linkRate;
            }
            set
            {
                this._linkRate = value;
            }
        }

        /// <summary>
        /// The date on which the CapOffer was calculated.
        /// </summary>
        [Property("OfferDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Application Date is a mandatory field")]
        public virtual System.DateTime ApplicationDate
        {
            get
            {
                return this._applicationDate;
            }
            set
            {
                this._applicationDate = value;
            }
        }

        /// <summary>
        /// An indicator as to whether the CAP is forming part of a promotion given to the client. In order to defend cancellations
        /// clients are offered a free 3% CAP.
        /// </summary>
        [Property("Promotion", ColumnType = "Boolean")]
        public virtual bool? Promotion
        {
            get
            {
                return this._promotion;
            }
            set
            {
                this._promotion = value;
            }
        }

        /// <summary>
        /// The date on which the CapOffer is created.
        /// </summary>
        [Lurker]
        [Property("CapitalisationDate")]
        public virtual System.DateTime? CapitalisationDate
        {
            get
            {
                return this._capitalisationDate;
            }
            set
            {
                this._capitalisationDate = value;
            }
        }

        /// <summary>
        /// The date on which the CapOffer was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
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
        /// The UserID of the person who last changed the CapOffer.
        /// </summary>
        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "CapOfferKey", ColumnType = "Int32")]
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
        /// CapOffer_DAO has a one-to-many relationship to the CapOfferDetail_DAO. Each CapOffer record has many CapOfferDetail
        /// records in the database, one for each of the 1%/2%/3% applications made available to the client.
        /// </summary>
        [HasMany(typeof(CapApplicationDetail_DAO), ColumnKey = "CapOfferKey", Table = "CapOfferDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CapApplicationDetail_DAO> CapApplicationDetails
        {
            get
            {
                return this._capApplicationDetails;
            }
            set
            {
                this._capApplicationDetails = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the Account table. Each CapOffer can only belong to one Account.
        /// </summary>
        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
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
        /// The foreign key reference to the Broker table. Each CapOffer can only belong to a single Broker at a time. This broker can change
        /// throughout the CapOffer process.
        /// </summary>
        [BelongsTo("BrokerKey", NotNull = true)]
        [ValidateNonEmpty("Broker is a mandatory field")]
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

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOffer can only belong to a single CapStatus, which changes
        /// throughout the life of the CapOffer.
        /// </summary>
        [BelongsTo("CapStatusKey", NotNull = true)]
        [ValidateNonEmpty("Cap Status is a mandatory field")]
        public virtual CapStatus_DAO CapStatus
        {
            get
            {
                return this._capStatus;
            }
            set
            {
                this._capStatus = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the CapTypeConfiguration table. Each CapOffer can only belong to a single CapTypeConfiguration
        /// where information regarding the sales configuration for the CAP product is maintained.
        /// </summary>
        [BelongsTo("CapTypeConfigurationKey", NotNull = true)]
        [ValidateNonEmpty("Cap Type Configuration is a mandatory field")]
        public virtual CapTypeConfiguration_DAO CapTypeConfiguration
        {
            get
            {
                return this._capTypeConfiguration;
            }
            set
            {
                this._capTypeConfiguration = value;
            }
        }

        [BelongsTo("CAPPaymentOptionKey")]
        public virtual CapPaymentOption_DAO CAPPaymentOption
        {
            get
            {
                return this._cAPPaymentOption;
            }
            set
            {
                this._cAPPaymentOption = value;
            }
        }
    }
}