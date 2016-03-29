using System;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("WorkFlowActivity", Schema = "X2")]
    public partial class WorkFlowActivity_DAO : DB_X2<WorkFlowActivity_DAO>
    {
        private string _name;

        private Int32? _stateID;

        private int _iD;

        private Activity_DAO _nextactivity;

        private WorkFlow_DAO _workFlow;

        private WorkFlow_DAO _nextWorkFlow;

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

        [Property("StateID")]
        public virtual Int32? StateID
        {
            get
            {
                return this._stateID;
            }
            set
            {
                this._stateID = value;
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

        [BelongsTo("NextActivityID", NotNull = true)]
        public virtual Activity_DAO NextActivity
        {
            get
            {
                return this._nextactivity;
            }
            set
            {
                this._nextactivity = value;
            }
        }

        [BelongsTo("NextWorkFlowID", NotNull = true)]
        public virtual WorkFlow_DAO NextWorkFlow
        {
            get
            {
                return this._nextWorkFlow;
            }
            set
            {
                this._nextWorkFlow = value;
            }
        }

        [BelongsTo("WorkFlowID", NotNull = true)]
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
    }
}