using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.Components.Validator;


namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Certain products require the effective rate on the Account to be overriden. For example, a Super Lo loan will result in a 
    /// 0.60% discount to the effective rate or an invoked CAP will result in a discounted rate. RateOverride_DAO is instantiated 
    /// when the client elects to take up these types of products.
    /// </summary>
    [ActiveRecord("RateOverride", Schema = "dbo")]
    public partial class RateOverride_DAO : DB_2AM<RateOverride_DAO>
    {

        private System.DateTime? _fromDate;

        private int? _term;

        private double? _capRate;

        private double? _floorRate;

        private double? _fixedRate;

        private double? _discount;

        private System.DateTime? _cancellationDate;

        private double? _capBalance;

        private double? _amount;

        private int _Key;

        private CancellationReason_DAO _cancellationReason;

        private FinancialService_DAO _financialService;

        private GeneralStatus_DAO _generalStatus;

        private RateOverrideType_DAO _rateOverrideType;

        private Trade_DAO _trade;

        private System.DateTime? _endDate;
        
        /// <summary>
        /// The date from which the Rate Override will be in effect.
        /// </summary>
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
        /// <summary>
        /// The period, in months, for which the Rate Override will be applied.
        /// </summary>
        [Property("Term", ColumnType = "Int32")]
        public virtual int? Term
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
        /// The rate at which the client has elected to CAP the rate applicable to their CAP Balance.
        /// </summary>
        [Property("CapRate", ColumnType = "Double")]
        public virtual double? CapRate
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
        /// <summary>
        /// This is not currently being used.
        /// </summary>
        [Property("FloorRate", ColumnType = "Double")]
        public virtual double? FloorRate
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
        /// <summary>
        /// This is not currently being used.
        /// </summary>
        [Property("FixedRate", ColumnType = "Double")]
        public virtual double? FixedRate
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
        /// <summary>
        /// The percentage discount which applies for the rate override. For a Super Lo loan this would be 0.60%.
        /// </summary>
        [Property("Discount", ColumnType = "Double")]
        public virtual double? Discount
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
        /// <summary>
        /// The date on which the Rate Override was cancelled.
        /// </summary>
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
        /// <summary>
        /// The balance which the client elected to CAP. If the client has taken a further loan, the CapRate is only applied
        /// to the CapBalance and not any subsequent increase.
        /// </summary>
        [Property("CapBalance", ColumnType = "Double")]
        public virtual double? CapBalance
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
        /// <summary>
        /// This property is used to capture a reduced debit order amount for the client, which will override the normal instalment due
        /// on the account. This could be used when a client is under debt review and could only afford a certain instalment or even a zero
        /// instalment.
        /// </summary>
        [Property("Amount", ColumnType = "Double")]
        public virtual double? Amount
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
        /// <summary>
        /// The date until which the Rate Override will be in effect.
        /// </summary>
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "RateOverrideKey", ColumnType = "Int32")]
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
        /// This is the foreign key reference to the Reason table. Each Rateoverride that is cancelled by a client requires a cancellation reason. 
        /// </summary>
        [BelongsTo("CancellationReasonKey", NotNull = false)]
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
        /// <summary>
        /// This is the foreign key reference to the FinancialService table. Each Rateoverride belongs to a Financial Service.
        /// </summary>
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
        /// <summary>
        /// This is the foreign key reference to the GeneralStatus table. Each RateOverride belongs to a specific status which determines
        /// whether it is active or not.
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
        /// This is the foreign key reference to the RateOverrideType table. Each RateOverride belongs to a specific type i.e.
        /// Super Lo, Interest Only, CAP.
        /// </summary>
        [BelongsTo("RateOverrideTypeKey", NotNull = true)]
        [ValidateNonEmpty("Rate Override Type is a mandatory field")]
        public virtual RateOverrideType_DAO RateOverrideType
        {
            get
            {
                return this._rateOverrideType;
            }
            set
            {
                this._rateOverrideType = value;
            }
        }
        /// <summary>
        /// This is the foreign key reference to the Trade table. A CAP RateOverride belongs to a specific trade. 
        /// The CapBalance on the RateOverride is allocated to this trade, which is bought in order to fund the CAP product.
        /// </summary>
        [BelongsTo("TradeKey", NotNull = false)]
        public virtual Trade_DAO Trade
        {
            get
            {
                return this._trade;
            }
            set
            {
                this._trade = value;
            }
        }
    }
}
