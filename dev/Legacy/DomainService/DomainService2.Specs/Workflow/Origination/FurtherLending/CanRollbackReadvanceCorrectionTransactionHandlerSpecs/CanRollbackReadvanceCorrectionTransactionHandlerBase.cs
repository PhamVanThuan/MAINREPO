using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.CanRollbackReadvanceCorrectionTransactionHandlerSpecs
{
    public class CanRollbackReadvanceCorrectionTransactionHandlerBase : WithFakes
    {
        protected static CanRollbackReadvanceCorrectionTransactionCommandHandler handler;
        protected static CanRollbackReadvanceCorrectionTransactionCommand command;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static ILoanTransactionRepository loanTransactionRepository;
    }
}