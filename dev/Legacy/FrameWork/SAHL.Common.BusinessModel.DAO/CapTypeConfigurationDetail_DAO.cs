using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapTypeConfigurationDetail_DAO is created in order to store the detailed sales configurations for each of the
    /// CapTypes.
    /// </summary>
    [ActiveRecord("CapTypeConfigurationDetail", Schema = "dbo")]
    public partial class CapTypeConfigurationDetail_DAO : DB_2AM<CapTypeConfigurationDetail_DAO>
    {
        private double _rate;

        private GeneralStatus_DAO _generalStatus;

        private double _premium;

        private double _feePremium;

        private double _feeAdmin;

        private double _rateFinance;

        private System.DateTime? _changeDate;

        private string _userID;

        private int _key;

        private IList<CapApplicationDetail_DAO> _capApplicationDetails;

        private CapType_DAO _capType;

        private CapTypeConfiguration_DAO _capTypeConfiguration;

        /// <summary>
        /// The CAP Base Rate for the CapOffer. This is calculated as JIBAR as of the last rate reset + the margin for the CAP Type.
        /// e.g. For the CAP sales period commencing 18/01/08 and ending 18/04/08, a 2% CAP would have a CAP Base Rate of 13.30%,
        /// which is JIBAR as of the 18/01/08 reset (11.30%) + the 2% margin.
        /// </summary>
        [Property("Rate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Rate is a mandatory field")]
        public virtual double Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
            }
        }

        /// <summary>
        /// The status of the CapTypeConfigurationDetail
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
        /// The total premium (FeePremium + AdminPremium)  payable for the CAP product. This is expressed per rand that the
        /// client wishes to CAP
        /// </summary>
        [Property("Premium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Premium is a mandatory field")]
        public virtual double Premium
        {
            get
            {
                return this._premium;
            }
            set
            {
                this._premium = value;
            }
        }

        /// <summary>
        /// The Premium Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        [Property("FeePremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Fee Premium is a mandatory field")]
        public virtual double FeePremium
        {
            get
            {
                return this._feePremium;
            }
            set
            {
                this._feePremium = value;
            }
        }

        /// <summary>
        /// The Administration Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        [Property("FeeAdmin", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Fee Admin is a mandatory field")]
        public virtual double FeeAdmin
        {
            get
            {
                return this._feeAdmin;
            }
            set
            {
                this._feeAdmin = value;
            }
        }

        /// <summary>
        /// This the strike rate for the CAP, which is related to the trade bought for the CAP.
        /// </summary>
        [Property("RateFinance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Rate Finance is a mandatory field")]
        public virtual double RateFinance
        {
            get
            {
                return this._rateFinance;
            }
            set
            {
                this._rateFinance = value;
            }
        }

        /// <summary>
        /// The date on which the CapTypeConfigurationDetail records were last changed.
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
        /// The UserID of the user who last changed the CapTypeConfigurationDetail records.
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
        [PrimaryKey(PrimaryKeyType.Native, "CapTypeConfigurationDetailKey", ColumnType = "Int32")]
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
        /// A foreign key reference to the CapTypeConfigurationDetailKey is stored against the CapOfferDetail record. Each of the
        /// CapOffers will have a 1%,2% and 3% CapOfferDetail record which is then linked back to the Sales Configuration via this
        /// one-to-many relationship.
        /// </summary>
        [HasMany(typeof(CapApplicationDetail_DAO), ColumnKey = "CapTypeConfigurationDetailKey", Table = "CapOfferDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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
        /// Each CapTypeConfigurationDetail record belongs to a CapType.
        /// </summary>

        [BelongsTo("CapTypeKey", NotNull = true)]
        [ValidateNonEmpty("Cap Type is a mandatory field")]
        public virtual CapType_DAO CapType
        {
            get
            {
                return this._capType;
            }
            set
            {
                this._capType = value;
            }
        }

        /// <summary>
        /// Each CapTypeConfigurationDetail belongs to a CapTypeConfiguration.
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
    }
}