using SAHL.Core.Data;
using SAHL.Services.LegacyEventGenerator.Services.Models;

namespace SAHL.Services.LegacyEventGenerator.Services.Statements
{
    public class GetEventTypeMappingStatement : ISqlStatement<EventTypeMappingModel>
    {
        public string GetStatement()
        {
            return @"select [EventTypeKey], [Name]
                        from [EventStore].[event].[EventType]";
        }
    }
}