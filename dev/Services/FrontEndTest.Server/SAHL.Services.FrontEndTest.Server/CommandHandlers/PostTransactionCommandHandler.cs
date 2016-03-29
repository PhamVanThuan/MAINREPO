using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class PostTransactionCommandHandler : IServiceCommandHandler<PostTransactionCommand>
    {
        private ILoanTransactions loanTransactions;

        public PostTransactionCommandHandler(ILoanTransactions loanTransactions)
        {
            this.loanTransactions = loanTransactions;
        }
        public ISystemMessageCollection HandleCommand(PostTransactionCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();

            var transaction = new PostTransactionModel(command.financialServiceKey, command.transactionTypeKey, command.amount, command.effectiveDate, command.reference, command.userId);
            messages.Aggregate(loanTransactions.PostTransaction(transaction));

            return messages;
        }
    }
}
