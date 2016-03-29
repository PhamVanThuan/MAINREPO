using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("PropertyType", Schema = "dbo")]
    public partial class PropertyType_DAO : DB_2AM<PropertyType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<Property_DAO> _properties;

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

        [PrimaryKey(PrimaryKeyType.Native, "PropertyTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Property), ColumnKey = "PropertyTypeKey", Table = "Property")]
        //public virtual IList<Property_DAO> Properties
        //{
        //    get
        //    {
        //        return this._properties;
        //    }
        //    set
        //    {
        //        this._properties = value;
        //    }
        //}
    }
}