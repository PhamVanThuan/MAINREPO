using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Commands
{
    internal class when_performing_the_capture_third_party_invoice_command : WithFakes
    {
        private static CaptureThirdPartyInvoiceCommand command;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;

        private Establish context = () =>
        {
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(123, Guid.NewGuid(), "invoiceNumber", DateTime.Now, Enumerable.Empty<InvoiceLineItemModel>(), true, string.Empty);
        };

        private Because of = () =>
        {
            command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
        };

        private It should_run_the_third_party_invoice_domain_check = () =>
        {
            command.ShouldBeAssignableTo<IRequiresThirdPartyInvoice>();
        };

        private It should_run_the_third_party_domain_check = () =>
        {
            command.ShouldBeAssignableTo<IRequiresThirdParty>();
        };
    }
}