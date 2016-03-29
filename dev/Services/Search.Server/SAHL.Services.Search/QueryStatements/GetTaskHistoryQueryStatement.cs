using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Search.QueryStatements
{
    public class GetTaskHistoryQueryStatement : IServiceQuerySqlStatement<GetTaskHistoryQuery, GetTaskHistoryQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 5 s_from.Name as FromState, s_to.Name as ToState, a.Name as Activity, 
                    case wfh.ADUserName when 'X2SYSTEM' then 'System' when 'X2' then 'System' else wfh.ADUserName end as ActivityUser,
                    wfh.ActivityDate from [x2].[x2].WorkFlowHistory wfh
                    join [x2].[x2].[State] s_to on s_to.ID = wfh.StateID
                    join [x2].[x2].Activity a on a.ID = wfh.ActivityID
                    join [x2].[x2].[State] s_from on s_from.Id = a.StateID
                    where wfh.InstanceID = @InstanceId -- and a.[Type] = 1
                    order by wfh.ActivityDate desc";
        }
    }
}