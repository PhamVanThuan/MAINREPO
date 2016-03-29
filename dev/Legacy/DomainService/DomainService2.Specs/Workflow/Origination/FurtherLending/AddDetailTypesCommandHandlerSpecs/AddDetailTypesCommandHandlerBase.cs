using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.AddDetailTypesCommandHandlerSpecs
{
    public class AddDetailTypesCommandHandlerBase : WithFakes
    {
        protected static AddDetailTypesCommand command;
        protected static AddDetailTypesCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IAccountRepository accountRepository;
    }
}