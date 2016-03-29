using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationIsActive
{
    public class SetValuationIsActiveCommandHandlerBase : WithFakes
    {
        protected static SetValuationIsActiveCommand command;
        protected static SetValuationIsActiveCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}