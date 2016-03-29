using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements
{
    public class RemoveCurrentlyAssignedUserForInstanceStatement : ISqlStatement<object>
    {
        public long InstanceId { get; protected set; }

        public RemoveCurrentlyAssignedUserForInstanceStatement(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            return @"delete from [eventprojection].[projection].[CurrentlyAssignedUserForInstance] where InstanceId = @InstanceId";
        }
    }
}