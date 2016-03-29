using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DebitOrderDay", Schema = "dbo")]
    public partial class DebitOrderDay_DAO : DB_2AM<DebitOrderDay_DAO>
    {
        private int _debitOrderDay;

        private int _Key;

        [Property("DebitOrderDay", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Day is a mandatory field")]
        public virtual int Day
        {
            get
            {
                return this._debitOrderDay;
            }
            set
            {
                this._debitOrderDay = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DebitOrderDayKey", ColumnType = "Int32")]
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