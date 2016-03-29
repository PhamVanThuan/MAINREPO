using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatus
{
    public class SetValuationStatusCommandHandlerBase : WithFakes
    {
        protected static SetValuationStatusCommand command;
        protected static SetValuationStatusCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}