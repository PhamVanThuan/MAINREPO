using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Insurer", Schema = "dbo")]
    public partial class Insurer_DAO : DB_2AM<Insurer_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<LifePolicy_DAO> _lifePolicies;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "InsurerKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(LifePolicy_DAO), ColumnKey = "InsurerKey", Table = "LifePolicy", Lazy = true)]
        //public virtual IList<LifePolicy_DAO> LifePolicies
        //{
        //    get
        //    {
        //        return this._lifePolicies;
        //    }
        //    set
        //    {
        //        this._lifePolicies = value;
        //    }
        //}
    }
}