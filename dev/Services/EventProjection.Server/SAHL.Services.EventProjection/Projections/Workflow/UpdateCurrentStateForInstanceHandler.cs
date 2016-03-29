using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Core.X2.Events;
using SAHL.Services.EventProjection.Projections.Workflow.Statements;

namespace SAHL.Services.EventProjection.Projections.Workflow
{
    public class UpdateCurrentStateForInstanceHandler : ITableProjector<WorkflowTransitionEvent, IDataModel>
    {
        private readonly IDbFactory dbFactory;

        public UpdateCurrentStateForInstanceHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Handle(WorkflowTransitionEvent @event, IServiceRequestMetadata metadata)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new UpdateCurrentStateForInstanceStatement(@event.InstanceId, @event.ToStateName, @event.WorkflowName, @event.Date, @event.GenericKey,
                    @event.GenericKeyTypeKey.GetValueOrDefault());
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }
    }
}