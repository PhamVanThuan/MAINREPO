using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Core
{
    public class SkipPart : ISkipPart
    {
        public int SkipCount { get; set; }

        public string AsString()
        {
            return "skip: " + SkipCount;
        }
    }
}