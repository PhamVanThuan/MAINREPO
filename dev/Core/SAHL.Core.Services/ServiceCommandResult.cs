using SAHL.Core.SystemMessages;
using System.Collections.Generic;

namespace SAHL.Core.Services
{
    public class ServiceCommandResult : IServiceCommandResult
    {
        public ServiceCommandResult(IEnumerable<SystemMessage> commandMessages)
        {
            this.CommandMessages = new SystemMessageCollection();
            this.CommandMessages.AddMessages(commandMessages);
        }

        public SystemMessageCollection CommandMessages { get; protected set; }
    }

    public class ServiceCommandResult<T> : ServiceCommandResult, IServiceCommandResult<T>
    {
        public ServiceCommandResult(IEnumerable<SystemMessage> commandMessages, T returnedData)
            : base(commandMessages)
        {
            this.ReturnedData = returnedData;
        }

        public T ReturnedData { get; protected set; }
    }
}