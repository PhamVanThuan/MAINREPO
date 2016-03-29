using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("ExternalActivity", Schema = "X2")]
    public partial class ExternalActivity_DAO : DB_X2<ExternalActivity_DAO>
    {
        private int _workFlowID;

        private string _name;

        private string _description;

        private int _iD;

        private IList<ActiveExternalActivity_DAO> _activeExternalActivities;

        private IList<Activity_DAO> _activities;

        [Property("WorkFlowID", ColumnType = "Int32", NotNull = true)]
        public virtual int WorkFlowID
        {
            get
            {
                return this._workFlowID;
            }
            set
            {
                this._workFlowID = value;
            }
        }

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

        [HasMany(typeof(ActiveExternalActivity_DAO), ColumnKey = "ExternalActivityID", Table = "ActiveExternalActivity", Lazy = true)]
        public virtual IList<ActiveExternalActivity_DAO> ActiveExternalActivities
        {
            get
            {
                return this._activeExternalActivities;
            }
            set
            {
                this._activeExternalActivities = value;
            }
        }

        [HasMany(typeof(Activity_DAO), ColumnKey = "RaiseExternalActivity", Table = "Activity", Lazy = true)]
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