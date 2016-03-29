using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("MarketRateHistory", Schema = "dbo")]
    public partial class MarketRateHistory_DAO : DB_2AM<MarketRateHistory_DAO>
    {
        private System.DateTime _changeDate;

        private double _rateBefore;

        private double _rateAfter;

        private string _changedBy;

        private string _changedByHost;

        private string _changedByApp;

        private int _key;

        private MarketRate_DAO _marketRate;

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

        [Property("RateBefore", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Rate Before is a mandatory field")]
        public virtual double RateBefore
        {
            get
            {
                return this._rateBefore;
            }
            set
            {
                this._rateBefore = value;
            }
        }

        [Property("RateAfter", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Rate After is a mandatory field")]
        public virtual double RateAfter
        {
            get
            {
                return this._rateAfter;
            }
            set
            {
                this._rateAfter = value;
            }
        }

        [Property("ChangedBy", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Changed By is a mandatory field")]
        public virtual string ChangedBy
        {
            get
            {
                return this._changedBy;
            }
            set
            {
                this._changedBy = value;
            }
        }

        [Property("ChangedByHost", ColumnType = "String")]
        public virtual string ChangedByHost
        {
            get
            {
                return this._changedByHost;
            }
            set
            {
                this._changedByHost = value;
            }
        }

        [Property("ChangedByApp", ColumnType = "String")]
        public virtual string ChangedByApp
        {
            get
            {
                return this._changedByApp;
            }
            set
            {
                this._changedByApp = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "MarketRateHistoryKey", ColumnType = "Int32")]
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

        [BelongsTo("MarketRateKey", NotNull = true)]
        [ValidateNonEmpty("Market Rate is a mandatory field")]
        public virtual MarketRate_DAO MarketRate
        {
            get
            {
                return this._marketRate;
            }
            set
            {
                this._marketRate = value;
            }
        }
    }
}