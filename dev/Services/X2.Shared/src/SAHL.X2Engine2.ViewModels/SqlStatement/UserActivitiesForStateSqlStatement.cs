using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class UserActivitiesForStateSqlStatement : ISqlStatement<ActivityDataModel>
    {
        public int StateId { get; protected set; }

        public UserActivitiesForStateSqlStatement(int stateId)
        {
            this.StateId = stateId;
        }

        public string GetStatement()
        {
            string sql = @"select A.ID as ID, A.WorkFlowID, A.[Name] as Name, a.[Type] as 'Type', s.ID as StateID, a.NextStateID, a.SplitWorkFlow, a.Priority as Priority,
                        a.FormID as FormID, a.ActivityMessage as ActivityMessage, a.RaiseExternalActivity, a.ExternalActivityTarget, a.ActivatedByExternalActivity,
                        a.ChainedActivityName, a.Sequence, a.X2ID
                        from x2.[X2].Activity A  (nolock)
                        inner join x2.[X2].State S (nolock) on S.ID = A.StateID
                        where A.StateID = @StateId
                        and a.Type =1
                        order by A.Priority";
            return sql;
        }
    }
}