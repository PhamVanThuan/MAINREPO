using SAHL.Core.Data;
using SAHL.Services.EventPublisher.Services.Models;

namespace SAHL.Services.EventPublisher.Services.Statements
{
    public class GetEventDataStatement : ISqlStatement<EventDataModel>
    {
        public int EventKey { get; protected set; }

        public GetEventDataStatement(int eventKey)
        {
            this.EventKey = eventKey;
        }

        public string GetStatement()
        {
            return @"select [Data],[Metadata]
                        from [EventStore].[event].[Event]
                        where EventKey = @EventKey";
        }
    }
}