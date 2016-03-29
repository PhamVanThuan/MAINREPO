using System;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    //[ActiveRecord("ScheduledActivity", Lazy = true, Schema = "X2")]
    public partial class ScheduledActivity_DAO : DB_X2<ScheduledActivity_DAO>
    {
        private int _id;
        private int _priority;
        private string _workFlowProviderName;
        private int _ActivityID;
        private DateTime _Time;
        private Int64 _InstanceID;

        [Property("Priority", ColumnType = "Int32", NotNull = true)]
        public virtual int Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        [Property("WorkFlowProviderName", ColumnType = "String")]
        public virtual string WorkFlowProviderName
        {
            get
            {
                return this._workFlowProviderName;
            }
            set
            {
                this._workFlowProviderName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, ColumnType = "Int32")]
        public virtual int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property("InstanceID", ColumnType = "Int64", NotNull = true)]
        public Int64 InstanceID
        {
            get
            {
                return _InstanceID;
            }
            set
            {
                _InstanceID = value;
            }
        }

        [Property("Time", ColumnType = "TimeStamp", NotNull = true)]
        public DateTime TIme
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
            }
        }

        [Property("ActivityID", ColumnType = "Int32", NotNull = true)]
        public int ActivityID
        {
            get { return _ActivityID; }
            set { _ActivityID = value; }
        }
    }
}