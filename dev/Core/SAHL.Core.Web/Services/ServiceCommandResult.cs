using Newtonsoft.Json;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Web.Services
{
    public class ServiceCommandResult
    {
        public ServiceCommandResult()
        {
            this.SystemMessages = new SystemMessageCollection();
        }

        [JsonConstructor]
        public ServiceCommandResult(ISystemMessageCollection systemMessages)
            : this()
        {
            this.SystemMessages.Aggregate(systemMessages);
        }

        public ISystemMessageCollection SystemMessages { get; protected set; }
    }
}