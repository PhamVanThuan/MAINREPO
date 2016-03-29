using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Services.EventPublisher.Services.Models;
using SAHL.Services.EventPublisher.Services.Statements;

namespace SAHL.Services.EventPublisher.Services
{
    public class EventPublisherDataService : IEventPublisherDataService
    {
        private IDbFactory dbFactory;

        public EventPublisherDataService(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public EventDataModel GetEventDataModelByEventKey(int eventKey)
        {
            using (IReadOnlyDbContext db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetEventDataStatement query = new GetEventDataStatement(eventKey);
                return db.SelectOne<EventDataModel>(query);
            }
        }
    }
}