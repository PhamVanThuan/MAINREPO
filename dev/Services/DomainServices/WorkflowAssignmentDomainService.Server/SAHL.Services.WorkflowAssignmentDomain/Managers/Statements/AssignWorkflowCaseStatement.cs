using SAHL.Core.Attributes;
using SAHL.Core.Data;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers.Statements
{
    [InsertConventionExclude] //excluded because of ForeignKey on InstanceId
    public class AssignWorkflowCaseStatement : ISqlStatement<object>
    {
        public long InstanceId { get; private set; }
        public int UserOrganisationStructureKey { get; private set; }
        public int CapabilityKey { get; private set; }

        public AssignWorkflowCaseStatement(long instanceId, int userOrganisationStructureKey, int capabilityKey)
        {
            InstanceId = instanceId;
            UserOrganisationStructureKey = userOrganisationStructureKey;
            CapabilityKey = capabilityKey;
        }

        public string GetStatement()
        {
            return @"
MERGE [X2].[X2].Assignment WITH(HOLDLOCK) AS target --with holdlock ensures we perform the join and subsequent insert (if the value is missing) atomically
USING (SELECT @InstanceId, @UserOrganisationStructureKey, @CapabilityKey)
AS source (InstanceId, UserOrganisationStructureKey, CapabilityKey)
ON (target.InstanceId = source.InstanceId)
WHEN MATCHED THEN
    UPDATE SET UserOrganisationStructureKey = source.UserOrganisationStructureKey, CapabilityKey = source.CapabilityKey, AssignmentDate = SYSDATETIME()
WHEN NOT MATCHED THEN
    INSERT (InstanceId, UserOrganisationStructureKey, CapabilityKey, AssignmentDate)
    VALUES (source.InstanceId, source.UserOrganisationStructureKey, source.CapabilityKey, SYSDATETIME());
";
        }
    }
}
