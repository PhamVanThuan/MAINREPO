using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.X2.Framework;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDomainMessageExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowDomainMessageExceptionCommand>
    {
        public ThrowDomainMessageExceptionCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowDomainMessageExceptionCommand command)
        {
            messages = new DomainMessageCollection();
            messages.Add(new DomainMessage("Test Domain Message", "Test Domain Message"));
            throw new DomainMessageException("Domain Message exception thrown from the DomainService", messages, command.IgnoreWarnings);
        }
    }
}