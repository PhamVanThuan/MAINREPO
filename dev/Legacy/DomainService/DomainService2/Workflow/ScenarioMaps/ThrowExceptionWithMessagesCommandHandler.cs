using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using System;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowExceptionWithMessagesCommandHandler : IHandlesDomainServiceCommand<ThrowExceptionWithMessagesCommand>
    {
        public ThrowExceptionWithMessagesCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowExceptionWithMessagesCommand command)
        {
            messages.Add(new DomainMessage("Test Domain Message", "Test Domain Message"));
            throw new Exception("Exception thrown from the DomainService");
        }
    }
}