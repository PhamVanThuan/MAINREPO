using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Commands
{
    public class when_performing_the_approve_third_party_invoice_command : WithFakes
    {
        private static ApproveThirdPartyInvoiceCommand command;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            thirdPartyInvoiceKey = 1234;
        };

        private Because of = () =>
        {
            command = new ApproveThirdPartyInvoiceCommand(thirdPartyInvoiceKey);
        };

        private It should_run_the_third_party_invoice_domain_check = () =>
        {
            command.ShouldBeAssignableTo<IRequiresThirdPartyInvoice>();
        };
    }
}