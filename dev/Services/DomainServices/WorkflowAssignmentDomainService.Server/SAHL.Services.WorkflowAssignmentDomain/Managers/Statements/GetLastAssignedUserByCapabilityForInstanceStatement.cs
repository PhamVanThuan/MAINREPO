using SAHL.Core.Data;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers.Statements
{
    public class GetLastAssignedUserByCapabilityForInstanceStatement : ISqlStatement<UserModel>
    {
        public long InstanceId { get; private set; }

        public int CapabilityKey { get; private set; }

        public GetLastAssignedUserByCapabilityForInstanceStatement(long instanceId, int capabilityKey)
        {
            InstanceId = instanceId;
            CapabilityKey = capabilityKey;
        }

        public string GetStatement()
        {
            return @"SELECT [UserName],[UserOrganisationStructureKey],'' as FullName
                      FROM [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance]
                      WHERE [InstanceId] = @InstanceId
                       AND  [CapabilityKey] = @CapabilityKey";
        }
    }
}