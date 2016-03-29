using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Valuation_DAO stores the information on a valuation for a property. It is discriminated using the ValuationDataProviderDataService
    /// which allows currently:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// SAHL Client Estimate
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// SAHL Manual Valuation
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Lightstone Automated Valuation
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// AdCheck Desktop Valuation
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// AdCheck Physical Valuation
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    /// <seealso cref="ValuationDiscriminatedAdCheckDesktop_DAO"/>
    /// <seealso cref="ValuationDiscriminatedSAHLClientEstimate_DAO"/>
    /// <seealso cref="ValuationDiscriminatedLightstoneAVM_DAO"/>
    /// <seealso cref="ValuationDiscriminatedSAHLManual_DAO"/>
    /// <seealso cref="ValuationDiscriminatedAdCheckPhysical_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Valuation", Schema = "dbo", DiscriminatorColumn = "ValuationDataProviderDataServiceKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    [HideBaseClass]
    public partial class Valuation_DAO : DB_2AM<Valuation_DAO>
    {
        private int _Key;
        private Valuator_DAO _valuator;
        private System.DateTime _valuationDate;
        private double? _valuationAmount;
        private double? _valuationHOCValue;
        private double? _valuationMunicipal;
        private string _valuationUserID;
        private Property_DAO _property;
        private double? _hOCThatchAmount;
        private double? _hOCConventionalAmount;
        private double? _hOCShingleAmount;
        //private System.DateTime _changeDate;
        private ValuationClassification_DAO _valuationClassification;
        private double? _valuationEscalationPercentage;
        private ValuationStatus_DAO _valuationStatus;
        private string _data;
        private ValuationDataProviderDataService_DAO _valuationDataProviderDataService;
        private bool _isActive;
        private HOCRoof_DAO _hOCRoof;

        /// <summary>
        /// The date on which the Valuation is completed.
        /// </summary>
        [Property("ValuationDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime ValuationDate
        {
            get
            {
                return this._valuationDate;
            }
            set
            {
                this._valuationDate = value;
            }
        }

        /// <summary>
        /// The Total Valuation Amount.
        /// </summary>
        [Property("ValuationAmount", ColumnType = "Double", NotNull = false)]
        public virtual double? ValuationAmount
        {
            get
            {
                return this._valuationAmount;
            }
            set
            {
                this._valuationAmount = value;
            }
        }

        /// <summary>
        /// The replacement HOC Total Valuation Amount
        /// </summary>
        [Property("ValuationHOCValue", ColumnType = "Double", NotNull = false)]
        public virtual double? ValuationHOCValue
        {
            get
            {
                return this._valuationHOCValue;
            }
            set
            {
                this._valuationHOCValue = value;
            }
        }

        /// <summary>
        /// The Municipal Valuation as provided by manual municipal enquiry.
        /// </summary>
        [Property("ValuationMunicipal", ColumnType = "Double", NotNull = false)]
        public virtual double? ValuationMunicipal
        {
            get
            {
                return this._valuationMunicipal;
            }
            set
            {
                this._valuationMunicipal = value;
            }
        }

        /// <summary>
        /// The Valuation User who captured the Valuation.
        /// </summary>
        [Property("ValuationUserID", ColumnType = "String", NotNull = false)]
        public virtual string ValuationUserID
        {
            get
            {
                return this._valuationUserID;
            }
            set
            {
                this._valuationUserID = value;
            }
        }

        /// <summary>
        /// The HOC Thatch Roof Type Total value.
        /// </summary>
        [Property("HOCThatchAmount", ColumnType = "Double", NotNull = false)]
        public virtual double? HOCThatchAmount
        {
            get
            {
                return this._hOCThatchAmount;
            }
            set
            {
                this._hOCThatchAmount = value;
            }
        }

        /// <summary>
        /// The HOC Conventional Roof Type Total value.
        /// </summary>
        [Property("HOCConventionalAmount", ColumnType = "Double", NotNull = false)]
        public virtual double? HOCConventionalAmount
        {
            get
            {
                return this._hOCConventionalAmount;
            }
            set
            {
                this._hOCConventionalAmount = value;
            }
        }

        /// <summary>
        /// The HOC Shingle Roof Type Total value.
        /// </summary>
        [Property("HOCShingleAmount", ColumnType = "Double", NotNull = false)]
        public virtual double? HOCShingleAmount
        {
            get
            {
                return this._hOCShingleAmount;
            }
            set
            {
                this._hOCShingleAmount = value;
            }
        }

        //[Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        //public virtual System.DateTime ChangeDate
        //{
        //    get
        //    {
        //        return this._changeDate;
        //    }
        //    set
        //    {
        //        this._changeDate = value;
        //    }
        //}
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ValuationKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        /// <summary>
        /// The foreign reference to the Property table. Each Valuation belongs to a single Property.
        /// </summary>
        [BelongsTo("PropertyKey", NotNull = true)]
        [ValidateNonEmpty("Property is a mandatory field")]
        public virtual Property_DAO Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        /// <summary>
        /// The foreign reference to the Valuator table. Each Valuation is carried out by a single Valuator. The Valuator details
        /// are stored as Legal Entity details.
        /// </summary>
        [BelongsTo("ValuatorKey", NotNull = false)]
        public virtual Valuator_DAO Valuator
        {
            get
            {
                return this._valuator;
            }
            set
            {
                this._valuator = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [BelongsTo("ValuationClassificationKey", NotNull = false)]
        public virtual ValuationClassification_DAO ValuationClassification
        {
            get
            {
                return this._valuationClassification;
            }
            set
            {
                this._valuationClassification = value;
            }
        }

        /// <summary>
        /// The HOC escalation percentage applied to the HOC Valuation portions of this Valuation.
        /// </summary>
        [Property("ValuationEscalationPercentage", ColumnType = "Double", NotNull = false)]
        public virtual double? ValuationEscalationPercentage
        {
            get
            {
                return this._valuationEscalationPercentage;
            }
            set
            {
                this._valuationEscalationPercentage = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the ValuationStatus table. Each Valuation is assigned a status of pending or complete. The
        /// status is updated via the X2 workflow.
        /// </summary>
        [BelongsTo("ValuationStatusKey", NotNull = true)]
        [ValidateNonEmpty("Valuation Status is a mandatory field")]
        public virtual ValuationStatus_DAO ValuationStatus
        {
            get
            {
                return this._valuationStatus;
            }
            set
            {
                this._valuationStatus = value;
            }
        }

        /// <summary>
        /// This is the XML data detailing the valuation. Its structure is specific to each of the discriminations and in the case
        /// of Lightstone and AdCheck are the resulting XML documents received via the respective web services for a completed Valuation.
        /// </summary>
        [Property("Data", ColumnType = "StringClob", NotNull = false)]
        public virtual string Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        ///// <summary>
        ///// The foreign key reference to the ValuationDataProviderDataService table. Each Valuation is assigned a ValuationDataProviderDataService
        ///// which corresponds to the Valuations discrimination and determines the source of the XML data.
        ///// </summary>
        //[BelongsTo("ValuationDataProviderDataServiceKey", Access = PropertyAccess.FieldCamelcaseUnderscore, NotNull = true, Insert = false, Update = false)]
        //public virtual ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        //{
        //    get
        //    {
        //        return this._valuationDataProviderDataService;
        //    }
        //}
        //[BelongsTo("ValuationDataProviderDataServiceKey", NotNull = true)]
        public virtual ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                return this._valuationDataProviderDataService;
            }
            set
            {
                this._valuationDataProviderDataService = value;
            }
        }

        /// <summary>
        /// This is a Valuation flag which is set by business rules (can be overridden manually). A property can have many completed
        /// valuations against it, from many sources, but only one active Valuation which is what this flag indicates.
        /// </summary>
        [Property("IsActive", ColumnType = "Boolean", NotNull = false)]
        public virtual bool IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                this._isActive = value;
            }
        }

        [BelongsTo("HOCRoofKey")]
        public virtual HOCRoof_DAO HOCRoof
        {
            get
            {
                return this._hOCRoof;
            }
            set
            {
                this._hOCRoof = value;
            }
        }
    }
}