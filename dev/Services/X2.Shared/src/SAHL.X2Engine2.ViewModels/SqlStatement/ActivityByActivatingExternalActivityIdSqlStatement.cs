using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ActivityByActivatingExternalActivityIdSqlStatement : ISqlStatement<ActivityDataModel>
    {
        public int ExternalActivityId { get; protected set; }

        public int CurrentStateIdForInstance { get; protected set; }

        public ActivityByActivatingExternalActivityIdSqlStatement(int externalActivityId, int currentStateIdForInstance)
        {
            this.ExternalActivityId = externalActivityId;
            this.CurrentStateIdForInstance = currentStateIdForInstance;
        }

        public string GetStatement()
        {
            string sql = @"declare @Target int
select @Target = ExternalActivityTarget from x2.x2.Activity where ActivatedByExternalActivity = @ExternalActivityId
if @Target=3
begin
	select ID, WorkFlowID, Name, Type, StateID, NextStateID, SplitWorkFlow, Priority, FormID, ActivityMessage, RaiseExternalActivity,
	ExternalActivityTarget, ActivatedByExternalActivity, ChainedActivityName, Sequence, X2ID
	from x2.x2.Activity a
	where a.ActivatedByExternalActivity = @ExternalActivityId
end
else
begin
	select ID, WorkFlowID, Name, Type, StateID, NextStateID, SplitWorkFlow, Priority, FormID, ActivityMessage, RaiseExternalActivity,
	ExternalActivityTarget, ActivatedByExternalActivity, ChainedActivityName, Sequence, X2ID
	from x2.x2.Activity a
	where a.ActivatedByExternalActivity = @ExternalActivityId and a.StateId=@currentStateIdForInstance
end
";
            return sql;
        }
    }
}