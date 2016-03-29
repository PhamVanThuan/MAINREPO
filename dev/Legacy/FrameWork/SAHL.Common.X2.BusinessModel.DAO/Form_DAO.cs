using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("Form", Schema = "X2")]
    public partial class Form_DAO : DB_X2<Form_DAO>
    {
        private string _name;

        private string _description;

        private int _iD;

        private IList<Activity_DAO> _activities;

        private WorkFlow_DAO _workFlow;

        private IList<State_DAO> _states;

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

        [HasMany(typeof(Activity_DAO), ColumnKey = "FormID", Table = "Activity", Lazy = true)]
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

        [BelongsTo("WorkFlowID", NotNull = false)]
        public virtual WorkFlow_DAO WorkFlow
        {
            get
            {
                return this._workFlow;
            }
            set
            {
                this._workFlow = value;
            }
        }

        [HasAndBelongsToMany(typeof(State_DAO), ColumnRef = "StateID", ColumnKey = "FormID", Lazy = true, OrderBy = "FormOrder", Schema = "X2", Table = "StateForm")]
        public virtual IList<State_DAO> States
        {
            get
            {
                return this._states;
            }
            set
            {
                this._states = value;
            }
        }
    }
}