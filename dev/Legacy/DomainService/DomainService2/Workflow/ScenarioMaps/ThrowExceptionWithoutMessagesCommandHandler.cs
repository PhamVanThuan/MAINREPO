using SAHL.Common.Collections.Interfaces;
using System;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowExceptionWithoutMessagesCommandHandler : IHandlesDomainServiceCommand<ThrowExceptionWithoutMessagesCommand>
    {
        public ThrowExceptionWithoutMessagesCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowExceptionWithoutMessagesCommand command)
        {
            throw new Exception("Exception thrown from the DomainService");
        }
    }
}