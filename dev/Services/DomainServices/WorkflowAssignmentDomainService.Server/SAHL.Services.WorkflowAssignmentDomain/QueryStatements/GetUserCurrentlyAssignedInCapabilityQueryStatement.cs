using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Services.WorkflowAssignmentDomain.QueryStatements
{
    public class GetUserCurrentlyAssignedInCapabilityQueryStatement
        : IServiceQuerySqlStatement<GetUserCurrentlyAssignedInCapabilityQuery, GetUserCurrentlyAssignedInCapabilityQueryResult>
    {
        public string GetStatement()
        {
            return @"select ad.ADUserName as UserName, a.AssignmentDate, a.UserOrganisationStructureKey from x2.x2.Assignment a
            join [2am].dbo.UserOrganisationStructure uos on a.UserOrganisationStructureKey=uos.UserOrganisationStructureKey
            join [2am].dbo.AdUser ad on uos.ADUserKey=ad.ADUserKey
            where a.InstanceId = @instanceId and a.CapabilityKey = @capability";
        }
    }
}