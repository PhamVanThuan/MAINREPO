using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckValuationExistsRecent
{
    public class CheckValuationExistsRecentRulesCommandHandlerBase : WithFakes
    {
        protected static CheckValuationExistsRecentRulesCommand command;
        protected static CheckValuationExistsRecentRulesCommandHandler handler;
        protected static IDomainMessageCollection messages;

        protected static IApplicationRepository applicationRepository;
    }
}