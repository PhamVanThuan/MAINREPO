using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapOfferDetail_DAO is instantiated in order to represent the detailed information regarding the 3 different CAPOffers
    /// which the client is offered.
    /// </summary>
    [ActiveRecord("CapOfferDetail", Schema = "dbo", Lazy = true)]
    public partial class CapApplicationDetail_DAO : DB_2AM<CapApplicationDetail_DAO>
    {
        private double _effectiveRate;

        private double _payment;

        private double _fee;

        private System.DateTime? _acceptanceDate;

        private System.DateTime? _capNTUReasonDate;

        private System.DateTime _changeDate;

        private string _userID;

        private int _key;

        private CapNTUReason_DAO _capNTUReason;

        private CapApplication_DAO _capApplication;

        private CapStatus_DAO _capStatus;

        private CapTypeConfigurationDetail_DAO _capTypeConfigurationDetail;

        /// <summary>
        /// The CAP effective rate. This is the rate which the client is being offered to CAP their loan at. This rate will either
        /// be 1%, 2% or 3% above their current rate.
        /// </summary>
        [Property("EffectiveRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Effective Rate is a mandatory field")]
        public virtual double EffectiveRate
        {
            get
            {
                return this._effectiveRate;
            }
            set
            {
                this._effectiveRate = value;
            }
        }

        /// <summary>
        /// This is the loan instalment which will be due by the client after the CAP 2 Readvance has been performed.
        /// </summary>
        [Property("Payment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Payment is a mandatory field")]
        public virtual double Payment
        {
            get
            {
                return this._payment;
            }
            set
            {
                this._payment = value;
            }
        }

        /// <summary>
        /// This the amount that the client is paying for the CAP 2 product.
        /// </summary>
        [Property("Fee", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Fee is a mandatory field")]
        public virtual double Fee
        {
            get
            {
                return this._fee;
            }
            set
            {
                this._fee = value;
            }
        }

        /// <summary>
        /// The date on which the CapOffer was accepted.
        /// </summary>
        [Property("AcceptanceDate")]
        public virtual System.DateTime? AcceptanceDate
        {
            get
            {
                return this._acceptanceDate;
            }
            set
            {
                this._acceptanceDate = value;
            }
        }

        /// <summary>
        /// The date on which the client decided to not take up the CapOffer.
        /// </summary>
        [Property("CapNTUReasonDate")]
        public virtual System.DateTime? CapNTUReasonDate
        {
            get
            {
                return this._capNTUReasonDate;
            }
            set
            {
                this._capNTUReasonDate = value;
            }
        }

        /// <summary>
        /// The date on which the CapOfferDetail record was last updated.
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
        /// The UserID of the person who last updated the CapOfferDetail record.
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "CapOfferDetailKey", ColumnType = "Int32")]
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
        /// The foreign key reference to the Reason table. Each CapOfferDetail record that is NTU'd by the client requires a Reason
        /// for the NTU decision. The CapOfferDetail can only belong to a single Reason.
        /// </summary>
        [BelongsTo("CapNTUReasonKey", NotNull = false)]
        public virtual CapNTUReason_DAO CapNTUReason
        {
            get
            {
                return this._capNTUReason;
            }
            set
            {
                this._capNTUReason = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the CapOffer table. Each CapOfferDetail record belongs to a single CapOffer record.
        /// </summary>
        [BelongsTo("CapOfferKey", NotNull = true)]
        [ValidateNonEmpty("Cap Application is a mandatory field")]
        public virtual CapApplication_DAO CapApplication
        {
            get
            {
                return this._capApplication;
            }
            set
            {
                this._capApplication = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOfferDetail record can have only one status which changes
        /// throughout the life of the loan.
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
        /// The foreign key reference to the CapTypeConfigurationDetail table. Each CapOfferDetail record belongs to a
        /// CapTypeConfigurationDetail record. This is dependent on the type of the CapOffer. i.e. The 1%
        /// </summary>
        [BelongsTo("CapTypeConfigurationDetailKey", NotNull = true)]
        [ValidateNonEmpty("Cap Type Configuration Details is a mandatory field")]
        public virtual CapTypeConfigurationDetail_DAO CapTypeConfigurationDetail
        {
            get
            {
                return this._capTypeConfigurationDetail;
            }
            set
            {
                this._capTypeConfigurationDetail = value;
            }
        }
    }
}