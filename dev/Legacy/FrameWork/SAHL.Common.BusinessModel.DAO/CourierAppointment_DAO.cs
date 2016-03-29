using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CourierAppointment", Schema = "dbo")]
    public partial class CourierAppointment_DAO : DB_2AM<CourierAppointment_DAO>
    {
        private System.DateTime? _appointmentDate;

        private string _notes;

        private int _Key;

        private Account_DAO _account;

        private Courier_DAO _courier;

        [Property("AppointmentDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? AppointmentDate
        {
            get
            {
                return this._appointmentDate;
            }
            set
            {
                this._appointmentDate = value;
            }
        }

        [Property("Notes", ColumnType = "String")]
        public virtual string Notes
        {
            get
            {
                return this._notes;
            }
            set
            {
                this._notes = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CourierAppointmentKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountKey", NotNull = false)]
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

        [BelongsTo("CourierKey", NotNull = false)]
        public virtual Courier_DAO Courier
        {
            get
            {
                return this._courier;
            }
            set
            {
                this._courier = value;
            }
        }
    }
}