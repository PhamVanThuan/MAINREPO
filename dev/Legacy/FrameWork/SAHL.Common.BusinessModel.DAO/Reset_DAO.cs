using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Reset", Schema = "dbo")]
    public partial class Reset_DAO : DB_2AM<Reset_DAO>
    {
        private int _Key;

        private System.DateTime _resetDate;

        private System.DateTime _runDate;

        private ResetConfiguration_DAO _ResetConfiguration;

        private double _jIBARRate;

        private double _jIBARDiscountRate;

        [PrimaryKey(PrimaryKeyType.Native, "ResetKey", ColumnType = "Int32")]
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

        [Property("ResetDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Reset Date is a mandatory field")]
        public virtual System.DateTime ResetDate
        {
            get
            {
                return this._resetDate;
            }
            set
            {
                this._resetDate = value;
            }
        }

        [Property("RunDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Run Date is a mandatory field")]
        public virtual System.DateTime RunDate
        {
            get
            {
                return this._runDate;
            }
            set
            {
                this._runDate = value;
            }
        }

        [Property("JIBARRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("JIBAR Rate is a mandatory field")]
        public virtual double JIBARRate
        {
            get
            {
                return this._jIBARRate;
            }
            set
            {
                this._jIBARRate = value;
            }
        }

        [Property("JIBARDiscountRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("JIBAR Discount Rate is a mandatory field")]
        public virtual double JIBARDiscountRate
        {
            get
            {
                return this._jIBARDiscountRate;
            }
            set
            {
                this._jIBARDiscountRate = value;
            }
        }

        [BelongsTo("ResetConfigurationKey", NotNull = true)]
        [ValidateNonEmpty("Reset Configuration is a mandatory field")]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get
            {
                return _ResetConfiguration;
            }
            set
            {
                _ResetConfiguration = value;
            }
        }
    }
}