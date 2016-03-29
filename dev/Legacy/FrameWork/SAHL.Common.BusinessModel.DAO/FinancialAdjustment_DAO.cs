using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FinancialAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess), JoinedBase]
    public partial class FinancialAdjustment_DAO : DB_2AM<FinancialAdjustment_DAO>
    {
        private int _key;

        private System.DateTime? _fromDate;

        private System.DateTime? _endDate;

        private System.DateTime? _cancellationDate;

        private FinancialAdjustmentSource_DAO _financialAdjustmentSource;

        private FinancialAdjustmentStatus_DAO _financialAdjustmentStatus;

        private FinancialAdjustmentType_DAO _financialAdjustmentType;

        private FinancialService_DAO _financialService;

        private ReversalProvisionAdjustment_DAO _reversalProvisionAdjustment;

        private DifferentialProvisionAdjustment_DAO _differentialProvisionAdjustment;

        private PaymentAdjustment_DAO _paymentAdjustment;

        private InterestRateAdjustment_DAO _interestRateAdjustment;

        private StaticRateAdjustment_DAO _staticRateAdjustment;

        private FixedRateAdjustment_DAO _fixedRateAdjustment;

        private CancellationReason_DAO _cancellationReason;

        private FinancialServiceAttribute_DAO _financialServiceAttribute;

        [PrimaryKey(PrimaryKeyType.Native, "FinancialAdjustmentKey", ColumnType = "Int32")]
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

        [Property("FromDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? FromDate
        {
            get
            {
                return this._fromDate;
            }
            set
            {
                this._fromDate = value;
            }
        }

        [Property("EndDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        [Property("CancellationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CancellationDate
        {
            get
            {
                return this._cancellationDate;
            }
            set
            {
                this._cancellationDate = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service is a mandatory field")]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("FinancialAdjustmentSourceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Adjustment Source is a mandatory field")]
        public virtual FinancialAdjustmentSource_DAO FinancialAdjustmentSource
        {
            get
            {
                return this._financialAdjustmentSource;
            }
            set
            {
                this._financialAdjustmentSource = value;
            }
        }

        [BelongsTo("FinancialAdjustmentStatusKey", NotNull = true)]
        [ValidateNonEmpty("Financial Adjustment Status is a mandatory field")]
        public virtual FinancialAdjustmentStatus_DAO FinancialAdjustmentStatus
        {
            get
            {
                return this._financialAdjustmentStatus;
            }
            set
            {
                this._financialAdjustmentStatus = value;
            }
        }

        [BelongsTo("FinancialAdjustmentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Financial Adjustment Type is a mandatory field")]
        public virtual FinancialAdjustmentType_DAO FinancialAdjustmentType
        {
            get
            {
                return this._financialAdjustmentType;
            }
            set
            {
                this._financialAdjustmentType = value;
            }
        }

        [OneToOne]
        public virtual ReversalProvisionAdjustment_DAO ReversalProvisionAdjustment
        {
            get
            {
                return this._reversalProvisionAdjustment;
            }
            set
            {
                this._reversalProvisionAdjustment = value;
            }
        }

        [OneToOne]
        public virtual DifferentialProvisionAdjustment_DAO DifferentialProvisionAdjustment
        {
            get
            {
                return this._differentialProvisionAdjustment;
            }
            set
            {
                this._differentialProvisionAdjustment = value;
            }
        }

        [OneToOne]
        public virtual PaymentAdjustment_DAO PaymentAdjustment
        {
            get
            {
                return this._paymentAdjustment;
            }
            set
            {
                this._paymentAdjustment = value;
            }
        }

        [OneToOne]
        public virtual InterestRateAdjustment_DAO InterestRateAdjustment
        {
            get
            {
                return this._interestRateAdjustment;
            }
            set
            {
                this._interestRateAdjustment = value;
            }
        }

        [OneToOne]
        public virtual StaticRateAdjustment_DAO StaticRateAdjustment
        {
            get
            {
                return this._staticRateAdjustment;
            }
            set
            {
                this._staticRateAdjustment = value;
            }
        }

        [OneToOne]
        public virtual FixedRateAdjustment_DAO FixedRateAdjustment
        {
            get
            {
                return this._fixedRateAdjustment;
            }
            set
            {
                this._fixedRateAdjustment = value;
            }
        }

        [BelongsTo("CancellationReasonKey")]
        public virtual CancellationReason_DAO CancellationReason
        {
            get
            {
                return this._cancellationReason;
            }
            set
            {
                this._cancellationReason = value;
            }
        }

        [BelongsTo("FinancialServiceAttributeKey")]
        public virtual FinancialServiceAttribute_DAO FinancialServiceAttribute
        {
            get
            {
                return this._financialServiceAttribute;
            }
            set
            {
                this._financialServiceAttribute = value;
            }
        }
    }
}