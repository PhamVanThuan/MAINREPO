using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DeedsPropertyType", Schema = "dbo")]
    public partial class DeedsPropertyType_DAO : DB_2AM<DeedsPropertyType_DAO>
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

        [PrimaryKey(PrimaryKeyType.Assigned, "DeedsPropertyTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Property), ColumnKey = "DeedsPropertyTypeKey", Table = "Property")]
        //public virtual IList<Property> Properties
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