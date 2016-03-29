using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OccupancyType", Schema = "dbo")]
    public partial class OccupancyType_DAO : DB_2AM<OccupancyType_DAO>
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

        [PrimaryKey(PrimaryKeyType.Assigned, "OccupancyTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Property_DAO), ColumnKey = "OccupancyTypeKey", Table = "Property")]
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