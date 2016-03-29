using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("SnapShotFinancialAdjustment", Schema = "DebtCounselling")]
    public class SnapShotFinancialAdjustment_DAO : DB_2AM<SnapShotFinancialAdjustment_DAO>
    {
        private int _snapShotFinancialAdjustmentKey;

        private Account_DAO _account;

        private FinancialAdjustment_DAO _financialAdjustment;

        private FinancialService_DAO _financialService;

        private FinancialAdjustmentSource_DAO _financialAdjustmentSource;

        private FinancialAdjustmentType_DAO _financialAdjustmentType;

        private FinancialAdjustmentStatus_DAO _financialAdjustmentStatus;

        private System.DateTime? _fromDate;

        private System.DateTime? _endDate;

        private System.DateTime? _cancellationDate;

        private int _cancellationReasonKey;

        private TransactionType_DAO _transactionType;

        private double _fRARate;

        private double _iRAAdjustment;

        private double _rPAReversalPercentage;

        private double _dPADifferentialAdjustment;

        private BalanceType_DAO _dPABalanceType;

        private double _amount;

        private SnapShotFinancialService_DAO _snapShotFinancialService;

        [PrimaryKey(PrimaryKeyType.Native, "SnapShotFinancialAdjustmentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._snapShotFinancialAdjustmentKey;
            }
            set
            {
                this._snapShotFinancialAdjustmentKey = value;
            }
        }

        [BelongsTo("AccountKey")]
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

        [BelongsTo("FinancialAdjustmentKey")]
        public virtual FinancialAdjustment_DAO FinancialAdjustment
        {
            get
            {
                return this._financialAdjustment;
            }
            set
            {
                this._financialAdjustment = value;
            }
        }

        [BelongsTo("FinancialServiceKey")]
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

        [BelongsTo("FinancialAdjustmentSourceKey")]
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

        [BelongsTo("FinancialAdjustmentTypeKey")]
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

        [BelongsTo("FinancialAdjustmentStatusKey")]
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

        [Property("CancellationReasonKey", ColumnType = "Int32")]
        public virtual int CancellationReasonKey
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

        [BelongsTo("TransactionTypeKey")]
        public virtual TransactionType_DAO TransactionType
        {
            get
            {
                return this._transactionType;
            }
            set
            {
                this._transactionType = value;
            }
        }

        [Property("FRARate", ColumnType = "Double")]
        public virtual double FRARate
        {
            get
            {
                return this._fRARate;
            }
            set
            {
                this._fRARate = value;
            }
        }

        [Property("IRAAdjustment", ColumnType = "Double")]
        public virtual double IRAAdjustment
        {
            get
            {
                return this._iRAAdjustment;
            }
            set
            {
                this._iRAAdjustment = value;
            }
        }

        [Property("RPAReversalPercentage", ColumnType = "Double")]
        public virtual double RPAReversalPercentage
        {
            get
            {
                return this._rPAReversalPercentage;
            }
            set
            {
                this._rPAReversalPercentage = value;
            }
        }

        [Property("DPADifferentialAdjustment", ColumnType = "Double")]
        public virtual double DPADifferentialAdjustment
        {
            get
            {
                return this._dPADifferentialAdjustment;
            }
            set
            {
                this._dPADifferentialAdjustment = value;
            }
        }

        [BelongsTo("DPABalanceTypeKey")]
        public virtual BalanceType_DAO DPABalanceType
        {
            get
            {
                return this._dPABalanceType;
            }
            set
            {
                this._dPABalanceType = value;
            }
        }

        [Property("Amount", ColumnType = "Double")]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [BelongsTo("SnapShotFinancialServiceKey")]
        public virtual SnapShotFinancialService_DAO SnapShotFinancialService
        {
            get
            {
                return this._snapShotFinancialService;
            }
            set
            {
                this._snapShotFinancialService = value;
            }
        }
    }
}