using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckIfCanPerformFurtherValuation
{
    public class CheckIfCanPerformFurtherValuationCommandHandlerBase : WithFakes
    {
        protected static CheckIfCanPerformFurtherValuationCommand command;
        protected static CheckIfCanPerformFurtherValuationCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}