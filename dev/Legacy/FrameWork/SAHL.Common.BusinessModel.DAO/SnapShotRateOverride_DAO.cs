using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("SnapShotRateOverride", Schema = "DebtCounselling")]
    public class SnapShotRateOverride_DAO : DB_2AM<SnapShotRateOverride_DAO>
    {
        private int _key;

        private int _accountKey;

        private int _rateOverrideKey;

        private int _financialServiceKey;

        private int _rateOverrideTypeKey;

        private System.DateTime _fromDate;

        private int _term;

        private double _capRate;

        private double _floorRate;

        private double _fixedRate;

        private double _discount;

        private int _generalStatusKey;

        private int _tradeKey;

        private System.DateTime _cancellationDate;

        private int _cancellationReasonKey;

        private double _capBalance;

        private double _amount;

        private int _cAPPaymentOptionKey;

        private System.DateTime _endDate;

        private SnapShotFinancialService_DAO _snapShotFinancialService;

        [PrimaryKey(PrimaryKeyType.Native, "SnapShotRateOverrideKey", ColumnType = "Int32")]
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

        [Property("AccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [Property("RateOverrideKey", ColumnType = "Int32", NotNull = true)]
        public virtual int RateOverrideKey
        {
            get
            {
                return this._rateOverrideKey;
            }
            set
            {
                this._rateOverrideKey = value;
            }
        }

        [Property("FinancialServiceKey", ColumnType = "Int32", NotNull = true)]
        public virtual int FinancialServiceKey
        {
            get
            {
                return this._financialServiceKey;
            }
            set
            {
                this._financialServiceKey = value;
            }
        }

        [Property("RateOverrideTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int RateOverrideTypeKey
        {
            get
            {
                return this._rateOverrideTypeKey;
            }
            set
            {
                this._rateOverrideTypeKey = value;
            }
        }

        [Property("FromDate", ColumnType = "Timestamp")]
        public virtual System.DateTime FromDate
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

        [Property("Term", ColumnType = "Int32")]
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

        [Property("CapRate", ColumnType = "Double")]
        public virtual double CapRate
        {
            get
            {
                return this._capRate;
            }
            set
            {
                this._capRate = value;
            }
        }

        [Property("FloorRate", ColumnType = "Double")]
        public virtual double FloorRate
        {
            get
            {
                return this._floorRate;
            }
            set
            {
                this._floorRate = value;
            }
        }

        [Property("FixedRate", ColumnType = "Double")]
        public virtual double FixedRate
        {
            get
            {
                return this._fixedRate;
            }
            set
            {
                this._fixedRate = value;
            }
        }

        [Property("Discount", ColumnType = "Double")]
        public virtual double Discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        [Property("GeneralStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GeneralStatusKey
        {
            get
            {
                return this._generalStatusKey;
            }
            set
            {
                this._generalStatusKey = value;
            }
        }

        [Property("TradeKey", ColumnType = "Int32")]
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

        [Property("CancellationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime CancellationDate
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

        [Property("CapBalance", ColumnType = "Double")]
        public virtual double CapBalance
        {
            get
            {
                return this._capBalance;
            }
            set
            {
                this._capBalance = value;
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

        [Property("CAPPaymentOptionKey", ColumnType = "Int32")]
        public virtual int CAPPaymentOptionKey
        {
            get
            {
                return this._cAPPaymentOptionKey;
            }
            set
            {
                this._cAPPaymentOptionKey = value;
            }
        }

        [Property("EndDate", ColumnType = "Timestamp")]
        public virtual System.DateTime EndDate
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
