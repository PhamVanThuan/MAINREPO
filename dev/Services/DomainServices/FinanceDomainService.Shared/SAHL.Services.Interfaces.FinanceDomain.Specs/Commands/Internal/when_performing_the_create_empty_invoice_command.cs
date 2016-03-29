using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Commands.Internal
{
    internal class when_performing_the_create_empty_invoice_command : WithFakes
    {
        private static CreateEmptyInvoiceCommand command;
        private static int accountKey;
        private static Guid sahlReference;
        private static string receivedFromEmailAddress;
        private static DateTime receivedDate;

        private Establish context = () =>
        {
            accountKey = 1;
            sahlReference = Guid.NewGuid();
            receivedFromEmailAddress = "someemail@adomain.com";
            receivedDate = DateTime.Now;
        };

        private Because of = () =>
        {
            command = new CreateEmptyInvoiceCommand(accountKey, sahlReference, receivedFromEmailAddress, receivedDate);
        };

        private It should_require_a_valid_accountKey = () =>
        {
            command.ShouldBeAssignableTo<IRequiresAccount>();
        };
    }
}