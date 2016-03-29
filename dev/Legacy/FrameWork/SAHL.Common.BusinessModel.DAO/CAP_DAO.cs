using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("CAP", Schema = "product", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class CAP_DAO : DB_2AM<CAP_DAO>
    {
        private int _key;

        private int _tradeKey;

        private System.DateTime? _cancellationDate;

        private int? _cancellationReasonKey;

        private double _cAPBalance;

        private double _cAPPrePaymentAmount;

        private double _mTDCAPPrePaymentAmount;

        private bool _invoked;

        private CapApplicationDetail_DAO _capOfferDetail;

        private CapPaymentOption_DAO _cAPPaymentOption;

        private FinancialServiceAttribute_DAO _financialServiceAttribute;

        [PrimaryKey(PrimaryKeyType.Foreign, Column = "FinancialServiceAttributeKey")]
        public virtual int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [Property("TradeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int TradeKey
        {
            get
            {
                return this._tradeKey;
            }
            set
            {
                this._tradeKey = value;
            }
        }

        [Property("CancellationDate", ColumnType = "DateTime")]
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

        [Property("CancellationReasonKey", ColumnType = "Int32")]
        public virtual int? CancellationReasonKey
        {
            get
            {
                return this._cancellationReasonKey;
            }
            set
            {
                this._cancellationReasonKey = value;
            }
        }

        [Property("CAPBalance", ColumnType = "Double", NotNull = true, Update = false)]
        public virtual double CAPBalance
        {
            get
            {
                return this._cAPBalance;
            }
            set
            {
                this._cAPBalance = value;
            }
        }

        [Property("CAPPrePaymentAmount", ColumnType = "Double", Update = false)]
        public virtual double CAPPrePaymentAmount
        {
            get
            {
                return this._cAPPrePaymentAmount;
            }
            set
            {
                this._cAPPrePaymentAmount = value;
            }
        }

        [Property("MTDCAPPrePaymentAmount", ColumnType = "Double", Update = false)]
        public virtual double MTDCAPPrePaymentAmount
        {
            get
            {
                return this._mTDCAPPrePaymentAmount;
            }
            set
            {
                this._mTDCAPPrePaymentAmount = value;
            }
        }

        [Property("Invoked", ColumnType = "Boolean")]
        public virtual bool Invoked
        {
            get
            {
                return this._invoked;
            }
            set
            {
                this._invoked = value;
            }
        }

        [BelongsTo("CapOfferDetailKey", NotNull = true)]
        public virtual CapApplicationDetail_DAO CapOfferDetail
        {
            get
            {
                return this._capOfferDetail;
            }
            set
            {
                this._capOfferDetail = value;
            }
        }

        [BelongsTo("CAPPaymentOptionKey", NotNull = true)]
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

        [OneToOne]
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