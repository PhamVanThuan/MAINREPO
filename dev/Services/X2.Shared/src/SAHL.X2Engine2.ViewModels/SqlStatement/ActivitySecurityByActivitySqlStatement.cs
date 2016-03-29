using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ActivitySecurityByActivitySqlStatement : ISqlStatement<ActivitySecurityDataModel>
    {
        public int ActivityId { get; protected set; }

        public ActivitySecurityByActivitySqlStatement(int activityId)
        {
            this.ActivityId = activityId;
        }

        public string GetStatement()
        {
            string sql = @"select ID as ID, ActivityID as ActivityID, SecurityGroupID as SecurityGroupID
                            from x2.x2.ActivitySecurity
                            where ActivityId=@ActivityId";
            return sql;
        }
    }
}