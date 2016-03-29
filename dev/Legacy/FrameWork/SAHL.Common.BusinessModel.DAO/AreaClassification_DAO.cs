using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AreaClassification", Schema = "dbo", Lazy = true)]
    public partial class AreaClassification_DAO : DB_2AM<AreaClassification_DAO>
    {
        private string _description;

        private int _key;

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

        [PrimaryKey(PrimaryKeyType.Native, "AreaClassificationKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Property), ColumnKey = "AreaClassificationKey", Table = "Property")]
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