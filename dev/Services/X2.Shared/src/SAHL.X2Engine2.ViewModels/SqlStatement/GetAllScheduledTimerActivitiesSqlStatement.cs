using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class GetAllScheduledTimerActivitiesSqlStatement : ISqlStatement<ScheduledActivityDataModel>
    {
        public string GetStatement()
        {
            string sql = @"select sa.InstanceID, sa.Time, sa.ActivityID, sa.Priority, sa.WorkFlowProviderName, sa.ID
                            from x2.x2.scheduledactivity sa
                            join x2.x2.Activity a on a.ID = sa.ActivityID
                            where ISNULL(WorkFlowProviderName,'')=''
                            and Time < DATEADD(d, 29, getdate())
                            and a.Type = 4";
            return sql;
        }
    }
}