using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FutureDatedChangeType", Schema = "dbo")]
    public partial class FutureDatedChangeType_DAO : DB_2AM<FutureDatedChangeType_DAO>
    {
        private string _description;

        private int _Key;

        // private IList<FutureDatedChange_DAO> _futureDatedChanges;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "FutureDatedChangeTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(FutureDatedChange_DAO), ColumnKey = "FutureDatedChangeTypeKey", Table = "FutureDatedChange")]
        //public virtual IList<FutureDatedChange_DAO> FutureDatedChanges
        //{
        //    get
        //    {
        //        return this._futureDatedChanges;
        //    }
        //    set
        //    {
        //        this._futureDatedChanges = value;
        //    }
        //}
    }
}