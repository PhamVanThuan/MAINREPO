using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ConditionType", Schema = "dbo")]
    public partial class ConditionType_DAO : DB_2AM<ConditionType_DAO>
    {
        private string _description;

        private int _key;

        // Commented as this is a lookup.
        //private IList<Condition_DAO> _conditions;

        [Property("Description", ColumnType = "String")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ConditionTypeKey", ColumnType = "Int32")]
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

        // Commented as this is a lookup.
        //[HasMany(typeof(Condition_DAO), ColumnKey = "ConditionTypeKey", Lazy = true, Table = "Condition")]
        //public virtual IList<Condition_DAO> Conditions
        //{
        //    get
        //    {
        //        return this._conditions;
        //    }
        //    set
        //    {
        //        this._conditions = value;
        //    }
        //}
    }
}