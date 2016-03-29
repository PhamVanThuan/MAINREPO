using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements
{
    public class UpdateCurrentlyAssignedUserForInstanceStatement : ISqlStatement<object>
    {
        public long InstanceId { get; private set; }
        public int CapabilityKey { get; private set; }
        public int UserOrganisationStructureKey { get; private set; }
        public int GenericKeyTypeKey { get; private set; }
        public int GenericKey { get; private set; }
        public string UserName { get; private set; }

        public UpdateCurrentlyAssignedUserForInstanceStatement(long instanceId, int capabilityKey, int userOrganisationStructureKey, int genericKeyTypeKey, int genericKey, string userName)
        {
            this.InstanceId = instanceId;
            this.CapabilityKey = capabilityKey;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.UserName = userName;
        }

        public string GetStatement()
        {
            return @"
MERGE [EventProjection].[projection].[CurrentlyAssignedUserForInstance] WITH(HOLDLOCK) AS target --with holdlock ensures we perform the join and subsequent insert (if the value is missing) atomically
USING (SELECT @InstanceId, @UserOrganisationStructureKey, @CapabilityKey, @GenericKeyTypeKey, @GenericKey, @UserName)
AS source (InstanceId, UserOrganisationStructureKey, CapabilityKey, GenericKeyTypeKey, GenericKey, UserName)
ON (target.InstanceId = source.InstanceId)
WHEN MATCHED THEN
    UPDATE SET
    LastUpdated = SYSDATETIME(),
    UserOrganisationStructureKey = source.UserOrganisationStructureKey,
    CapabilityKey = source.CapabilityKey,
    GenericKeyTypeKey = source.GenericKeyTypeKey,
    GenericKey = source.GenericKey,
    UserName = source.UserName
WHEN NOT MATCHED THEN
    INSERT (LastUpdated, InstanceId, UserOrganisationStructureKey, CapabilityKey, GenericKeyTypeKey, GenericKey, UserName)
    VALUES (SYSDATETIME(), source.InstanceId, source.UserOrganisationStructureKey, source.CapabilityKey, source.GenericKeyTypeKey, source.GenericKey, source.UserName);
";
        }
    }
}