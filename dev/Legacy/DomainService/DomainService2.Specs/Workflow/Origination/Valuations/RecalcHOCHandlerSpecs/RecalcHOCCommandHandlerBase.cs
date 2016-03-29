using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.RecalcHOC
{
    public class RecalcHOCCommandHandlerBase : WithFakes
    {
        protected static RecalcHOCCommand command;
        protected static RecalcHOCCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}