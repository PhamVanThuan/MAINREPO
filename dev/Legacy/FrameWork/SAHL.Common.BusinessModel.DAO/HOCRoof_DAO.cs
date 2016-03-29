using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("HOCRoof", Schema = "dbo")]
    public partial class HOCRoof_DAO : DB_2AM<HOCRoof_DAO>
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

        [PrimaryKey(PrimaryKeyType.Assigned, "HOCRoofKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(HOC), ColumnKey = "HOCRoofKey", Table = "HOC")]
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