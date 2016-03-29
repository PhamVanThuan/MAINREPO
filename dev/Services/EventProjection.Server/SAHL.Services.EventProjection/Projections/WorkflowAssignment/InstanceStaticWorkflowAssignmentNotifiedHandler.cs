using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;

namespace SAHL.Services.EventProjection.Projections.WorkflowAssignment
{
    public class InstanceStaticWorkflowAssignmentNotifiedHandler : ITableProjector<InstanceStaticWorkflowAssignmentNotifiedEvent, IDataModel>
    {
        private readonly IDbFactory dbFactory;

        public InstanceStaticWorkflowAssignmentNotifiedHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Handle(InstanceStaticWorkflowAssignmentNotifiedEvent @event, IServiceRequestMetadata metadata)
        {
            using (var dbContext = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveCurrentlyAssignedUserForInstanceStatement(@event.InstanceId);
                dbContext.ExecuteNonQuery(statement);
                dbContext.Complete();
            }
        }
    }
}