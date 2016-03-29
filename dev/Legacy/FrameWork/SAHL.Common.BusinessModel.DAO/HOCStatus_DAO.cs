using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HOCStatus", Schema = "dbo")]
    public partial class HOCStatus_DAO : DB_2AM<HOCStatus_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<HOC> _hOCs;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "HOCStatusKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(HOC), ColumnKey = "HOCStatusKey", Table = "HOC")]
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