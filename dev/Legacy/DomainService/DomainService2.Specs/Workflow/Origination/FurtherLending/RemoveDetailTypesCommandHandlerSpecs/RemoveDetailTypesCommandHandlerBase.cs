using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.RemoveDetailTypesCommandHandlerSpecs
{
    public class RemoveDetailTypesCommandHandlerBase : WithFakes
    {
        protected static RemoveDetailTypesCommandHandler handler;
        protected static RemoveDetailTypesCommand command;
        protected static IApplicationRepository applicationRepository;
        protected static IAccountRepository accountRepository;
        protected static IDomainMessageCollection messages;
    }
}