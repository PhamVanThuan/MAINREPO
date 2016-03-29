using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CommissionTransaction", Schema = "dbo")]
    public partial class CommissionTransaction_DAO : DB_2AM<CommissionTransaction_DAO>
    {
        private int _financialServiceKey;

        private double _commissionCalcAmount;

        private double? _commissionAmount;

        private decimal? _commissionFactor;

        private string _commissionType;

        private double? _kickerCalcAmount;

        private double? _kickerAmount;

        private System.DateTime _transactionDate;

        private System.DateTime? _batchRunDate;

        private int _Key;

        //private BatchType _batchType;

        private ADUser_DAO _adUser;

        private FinancialServiceType_DAO _financialServiceType;

        [Property("FinancialServiceKey", ColumnType = "Int32")]
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

        [Property("CommissionCalcAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Commission Calc Amount is a mandatory field")]
        public virtual double CommissionCalcAmount
        {
            get
            {
                return this._commissionCalcAmount;
            }
            set
            {
                this._commissionCalcAmount = value;
            }
        }

        [Property("CommissionAmount", ColumnType = "Double")]
        public virtual double? CommissionAmount
        {
            get
            {
                return this._commissionAmount;
            }
            set
            {
                this._commissionAmount = value;
            }
        }

        [Property("CommissionFactor", ColumnType = "Decimal")]
        public virtual decimal? CommissionFactor
        {
            get
            {
                return this._commissionFactor;
            }
            set
            {
                this._commissionFactor = value;
            }
        }

        [Property("CommissionType", ColumnType = "String", Length = 10)]
        public virtual string CommissionType
        {
            get
            {
                return this._commissionType;
            }
            set
            {
                this._commissionType = value;
            }
        }

        [Property("KickerCalcAmount", ColumnType = "Double")]
        public virtual double? KickerCalcAmount
        {
            get
            {
                return this._kickerCalcAmount;
            }
            set
            {
                this._kickerCalcAmount = value;
            }
        }

        [Property("KickerAmount", ColumnType = "Double")]
        public virtual double? KickerAmount
        {
            get
            {
                return this._kickerAmount;
            }
            set
            {
                this._kickerAmount = value;
            }
        }

        [Property("TransactionDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Transaction Date is a mandatory field")]
        public virtual System.DateTime TransactionDate
        {
            get
            {
                return this._transactionDate;
            }
            set
            {
                this._transactionDate = value;
            }
        }

        [Property("BatchRunDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? BatchRunDate
        {
            get
            {
                return this._batchRunDate;
            }
            set
            {
                this._batchRunDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CommissionTransactionKey", ColumnType = "Int32")]
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

        //[BelongsTo("BatchTypeKey", NotNull = false)]
        //public virtual BatchType BatchType
        //{
        //    get
        //    {
        //        return this._batchType;
        //    }
        //    set
        //    {
        //        this._batchType = value;
        //    }
        //}

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._adUser;
            }
            set
            {
                this._adUser = value;
            }
        }

        [BelongsTo("FinancialServiceTypeKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service Type is a mandatory field")]
        public virtual FinancialServiceType_DAO FinancialServiceType
        {
            get
            {
                return this._financialServiceType;
            }
            set
            {
                this._financialServiceType = value;
            }
        }
    }
}