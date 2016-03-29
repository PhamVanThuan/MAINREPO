using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_posting_a_transaction : WithFakes
    {
        private static ILoanTransactions loanTransactions;
        private static PostTransactionCommand command;
        private static PostTransactionCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int financialServiceKey;
        private static PostTransactionModel model;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            loanTransactions = An<ILoanTransactions>();
            metadata = An<IServiceRequestMetadata>();
            financialServiceKey = 123;
            command = new PostTransactionCommand(financialServiceKey, 1, 9.99M, DateTime.Now, "Reference", "VishavP");
            model = new PostTransactionModel(command.financialServiceKey, command.transactionTypeKey, command.amount, command.effectiveDate, command.reference, command.userId);
            commandHandler = new PostTransactionCommandHandler(loanTransactions);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}