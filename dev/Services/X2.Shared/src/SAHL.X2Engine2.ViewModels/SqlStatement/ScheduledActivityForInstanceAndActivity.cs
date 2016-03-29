using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ScheduledActivityForInstanceAndActivity : ISqlStatement<ScheduledActivityDataModel>
    {
        public long InstanceID { get; protected set; }

        public int ActivityID { get; protected set; }

        public ScheduledActivityForInstanceAndActivity(long instanceId, int activityId)
        {
            this.InstanceID = instanceId;
            this.ActivityID = activityId;
        }

        public string GetStatement()
        {
            string sql = @"select InstanceID, Time, ActivityID, Priority, WorkFlowProviderName, ID
                        from x2.x2.scheduledactivity
                        where InstanceID=@InstanceID and ActivityID=@ActivityID";
            return sql;
        }
    }
}