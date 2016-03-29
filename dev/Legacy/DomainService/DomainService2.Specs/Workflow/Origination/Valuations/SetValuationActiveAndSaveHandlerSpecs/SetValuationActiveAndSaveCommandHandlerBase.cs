using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationActiveAndSave
{
    public class SetValuationActiveAndSaveCommandHandlerBase : WithFakes
    {
        protected static SetValuationActiveAndSaveCommand command;
        protected static SetValuationActiveAndSaveCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}