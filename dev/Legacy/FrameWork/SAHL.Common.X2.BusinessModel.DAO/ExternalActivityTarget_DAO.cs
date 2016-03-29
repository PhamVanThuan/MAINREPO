using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("ExternalActivityTarget", Schema = "X2")]
    public partial class ExternalActivityTarget_DAO : DB_X2<ExternalActivityTarget_DAO>
    {
        private string _name;

        private int _iD;

        private IList<Activity_DAO> _activities;

        [Property("Name", ColumnType = "String", NotNull = true)]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                this._iD = value;
            }
        }

        [HasMany(typeof(Activity_DAO), ColumnKey = "ExternalActivityTarget", Table = "Activity", Lazy = true)]
        public virtual IList<Activity_DAO> Activities
        {
            get
            {
                return this._activities;
            }
            set
            {
                this._activities = value;
            }
        }
    }
}