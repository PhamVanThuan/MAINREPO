using System;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel
{
    //[ActiveRecord("ScheduledActivity", Lazy = true, Schema = "X2")]
    public partial class ScheduledActivity : DB_X2<ScheduledActivity>
    {
        private int _priority;

        private string _workFlowProviderName;

        private ScheduledActivityCompositeKey _scheduledActivityCompositeKey;

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

        [CompositeKey()]
        public virtual ScheduledActivityCompositeKey ScheduledActivityCompositeKey
        {
            get
            {
                return this._scheduledActivityCompositeKey;
            }
            set
            {
                this._scheduledActivityCompositeKey = value;
            }
        }
    }

    [Serializable()]
    public partial class ScheduledActivityCompositeKey
    {
        private long _instanceID;

        private System.DateTime _time;

        private int _activityID;

        [KeyProperty(Column = "InstanceID", ColumnType = "Int64", NotNull = true)]
        public virtual long InstanceID
        {
            get
            {
                return this._instanceID;
            }
            set
            {
                this._instanceID = value;
            }
        }

        [KeyProperty(Column = "Time", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
            }
        }

        [KeyProperty(Column = "ActivityID", ColumnType = "Int32", NotNull = true)]
        public virtual int ActivityID
        {
            get
            {
                return this._activityID;
            }
            set
            {
                this._activityID = value;
            }
        }

        public override string ToString()
        {
            return String.Join(":", new string[] {
                        this._instanceID.ToString(),
                        this._time.ToString(),
                        this._activityID.ToString()});
        }

        public override bool Equals(object obj)
        {
            if ((obj == this))
            {
                return true;
            }
            if (((obj == null)
                        || (obj.GetType() != this.GetType())))
            {
                return false;
            }
            ScheduledActivityCompositeKey test = ((ScheduledActivityCompositeKey)(obj));
            return (((_instanceID == test._instanceID)
                        || (_instanceID.Equals(test._instanceID)))
                        && (((_time == test._time)
                        || ((_time != null)
                        && _time.Equals(test._time)))
                        && ((_activityID == test._activityID)
                        || (_activityID.Equals(test._activityID)))));
        }

        public override int GetHashCode()
        {
            return XorHelper(_instanceID.GetHashCode(), XorHelper(_time.GetHashCode(), _activityID.GetHashCode()));
        }

        private int XorHelper(int left, int right)
        {
            return left ^ right;
        }
    }
}