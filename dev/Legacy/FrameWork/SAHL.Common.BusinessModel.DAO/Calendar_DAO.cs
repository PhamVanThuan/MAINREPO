using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Calendar", Schema = "dbo")]
    public partial class Calendar_DAO : DB_2AM<Calendar_DAO>
    {
        private System.DateTime _calendarDate;

        private bool _isSaturday;

        private bool _isSunday;

        private bool _isHoliday;

        private int _Key;

        [Property("CalendarDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Calendar Date is a mandatory field")]
        public virtual System.DateTime CalendarDate
        {
            get
            {
                return this._calendarDate;
            }
            set
            {
                this._calendarDate = value;
            }
        }

        [Property("IsSaturday", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Saturday is a mandatory field")]
        public virtual bool IsSaturday
        {
            get
            {
                return this._isSaturday;
            }
            set
            {
                this._isSaturday = value;
            }
        }

        [Property("IsSunday", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Sunday is a mandatory field")]
        public virtual bool IsSunday
        {
            get
            {
                return this._isSunday;
            }
            set
            {
                this._isSunday = value;
            }
        }

        [Property("IsHoliday", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Holiday is a mandatory field")]
        public virtual bool IsHoliday
        {
            get
            {
                return this._isHoliday;
            }
            set
            {
                this._isHoliday = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CalendarKey", ColumnType = "Int32")]
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
    }
}