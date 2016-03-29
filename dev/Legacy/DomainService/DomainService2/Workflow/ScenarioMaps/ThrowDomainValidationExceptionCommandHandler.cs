using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDomainValidationExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowDomainValidationExceptionCommand>
    {
        public ThrowDomainValidationExceptionCommandHandler()
        {
        }

        public void Handle(IDomainMessageCollection messages, ThrowDomainValidationExceptionCommand command)
        {
            throw new DomainValidationException("Domain Validation exception thrown from the DomainService");
        }
    }
}