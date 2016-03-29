using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;

namespace SAHL.Services.EventProjection.Projections.WorkflowAssignment
{
    public class UpdateCurrentlyAssignedUserForInstanceHandler : ITableProjector<WorkflowCaseAssignedEvent, IDataModel>
    {
        private readonly IDbFactory dbFactory;

        public UpdateCurrentlyAssignedUserForInstanceHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Handle(WorkflowCaseAssignedEvent @event, IServiceRequestMetadata metadata)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var getUserNameStatement = new GetUserNameForUserOrganisationStructureKeyStatement(@event.UserOrganisationStructureKey);
                var userName = context.SelectOne(getUserNameStatement);

                var statement = new UpdateCurrentlyAssignedUserForInstanceStatement(@event.InstanceId,
                    @event.CapabilityKey,
                    @event.UserOrganisationStructureKey,
                    @event.GenericKeyTypeKey,
                    @event.GenericKey,
                    userName
                    );

                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }
    }
}