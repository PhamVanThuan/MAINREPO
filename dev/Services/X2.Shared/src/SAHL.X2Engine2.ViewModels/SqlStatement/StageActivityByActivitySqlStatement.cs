using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class StageActivityByActivitySqlStatement : ISqlStatement<StageActivityDataModel>
    {
        public int ActivityID { get; protected set; }

        public StageActivityByActivitySqlStatement(int activityId)
        {
            this.ActivityID = activityId;
        }

        public string GetStatement()
        {
            string sql = @"select ID, ActivityID, StageDefinitionKey, StageDefinitionStageDefinitionGroupKey from x2.x2.stageactivity where ActivityID=@ActivityID";
            return sql;
        }
    }
}