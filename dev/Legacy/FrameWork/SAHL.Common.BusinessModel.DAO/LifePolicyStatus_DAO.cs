using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LifePolicyStatus", Schema = "dbo")]
    public partial class LifePolicyStatus_DAO : DB_2AM<LifePolicyStatus_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<LifePolicy> _lifePolicies;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "PolicyStatusKey", ColumnType = "Int32")]
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

        // Commented, this is a lookup.
        //[HasMany(typeof(LifePolicy), ColumnKey = "PolicyStatusKey", Table = "LifePolicy")]
        //public virtual IList<LifePolicy> LifePolicies
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