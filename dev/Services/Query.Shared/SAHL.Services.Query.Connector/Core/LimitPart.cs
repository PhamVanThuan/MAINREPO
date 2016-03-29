using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Core
{
    public class LimitPart : ILimitPart
    {
        public int Count { get; set; }

        public string AsString()
        {
            return "limit: " + Count;
        }
    }
}
