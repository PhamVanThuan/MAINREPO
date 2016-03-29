using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("TitleType", Schema = "dbo")]
    public partial class TitleType_DAO : DB_2AM<TitleType_DAO>
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

        [PrimaryKey(PrimaryKeyType.Assigned, "TitleTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Property), ColumnKey = "TitleTypeKey", Table = "Property")]
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