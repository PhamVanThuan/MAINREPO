using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    internal class when_invoice_number_is_not_set : WithFakes
    {
        private static Exception ex;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), null, DateTime.Now, null, true, string.Empty);
            });
        };

        private It should_throw_ThirdPartyInvoiceNumber_not_set_exception = () =>
        {
            ex.Message.Equals("An ThirdPartyInvoiceNumber must be provided.");
        };
    }
}