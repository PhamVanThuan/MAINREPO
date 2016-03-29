namespace SAHL.Services.LegacyEventGenerator.Services.Models
{
    public class EventTypeMappingModel
    {
        public int EventTypeKey { get; private set; }

        public string Name { get; private set; }

        public EventTypeMappingModel(int eventTypeKey, string name)
        {
            this.EventTypeKey = eventTypeKey;
            this.Name = name;
        }
    }
}