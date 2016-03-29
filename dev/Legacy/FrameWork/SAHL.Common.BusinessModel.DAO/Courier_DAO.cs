using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Courier", Schema = "dbo")]
    public partial class Courier_DAO : DB_2AM<Courier_DAO>
    {
        private string _courierName;

        private string _emailAddress;

        private int _Key;

        //private IList<CourierAppointment_DAO> _courierAppointments;

        [Property("CourierName", ColumnType = "String")]
        public virtual string CourierName
        {
            get
            {
                return this._courierName;
            }
            set
            {
                this._courierName = value;
            }
        }

        [Property("EmailAddress", ColumnType = "String")]
        public virtual string EmailAddress
        {
            get
            {
                return this._emailAddress;
            }
            set
            {
                this._emailAddress = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CourierKey", ColumnType = "Int32")]
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

        // Commented, this is a performance issue.
        //[HasMany(typeof(CourierAppointment_DAO), ColumnKey = "CourierKey", Table = "CourierAppointment")]
        //public virtual IList<CourierAppointment_DAO> CourierAppointments
        //{
        //    get
        //    {
        //        return this._courierAppointments;
        //    }
        //    set
        //    {
        //        this._courierAppointments = value;
        //    }
        //}
    }
}