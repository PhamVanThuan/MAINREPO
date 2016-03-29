using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HOCConstruction", Schema = "dbo")]
    public partial class HOCConstruction_DAO : DB_2AM<HOCConstruction_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<HOC> _hOCs;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "HOCConstructionKey", ColumnType = "Int32")]
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

        // commented, this is a lookup
        //[HasMany(typeof(HOC), ColumnKey = "HOCConstructionKey", Table = "HOC")]
        //public virtual IList<HOC> HOCs
        //{
        //    get
        //    {
        //        return this._hOCs;
        //    }
        //    set
        //    {
        //        this._hOCs = value;
        //    }
        //}
    }
}