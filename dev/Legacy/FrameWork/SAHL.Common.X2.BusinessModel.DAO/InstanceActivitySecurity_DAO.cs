using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("InstanceActivitySecurity", Schema = "X2")]
    public partial class InstanceActivitySecurity_DAO : DB_X2<InstanceActivitySecurity_DAO>
    {
        private int _iD;

        private string _aDUserName;

        private Activity_DAO _activity;

        private Instance_DAO _instance;

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [BelongsTo("ActivityID", NotNull = true)]
        public virtual Activity_DAO Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
            }
        }

        [BelongsTo("InstanceID", NotNull = true)]
        public virtual Instance_DAO Instance
        {
            get
            {
                return this._instance;
            }
            set
            {
                this._instance = value;
            }
        }
    }
}