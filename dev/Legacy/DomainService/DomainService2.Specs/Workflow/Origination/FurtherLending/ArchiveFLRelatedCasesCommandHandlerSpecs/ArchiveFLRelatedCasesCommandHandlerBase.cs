using DomainService2.SharedServices;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.ArchiveFLRelatedCasesCommandHandlerSpecs
{
    public class ArchiveFLRelatedCasesCommandHandlerBase : WithFakes
    {
        protected static ArchiveFLRelatedCasesCommandHandler handler;
        protected static ArchiveFLRelatedCasesCommand command;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repository;
        protected static IX2WorkflowService x2WorkflowService;
    }
}