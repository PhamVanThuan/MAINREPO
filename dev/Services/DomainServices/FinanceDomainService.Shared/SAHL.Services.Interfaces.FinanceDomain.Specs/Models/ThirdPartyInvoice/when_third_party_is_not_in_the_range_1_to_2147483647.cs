using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Models.ThirdPartyInvoice
{
    internal class when_third_party_invoic_key_is_not_in_the_range_1_to_2147483647 : WithFakes
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
                var bigInt = -1;
                thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(bigInt, Guid.NewGuid(), "DD1001", DateTime.Now, null, true, string.Empty);
            });
        };

        private It should_throw_third_party_invoic_key_out_of_range_exception = () =>
        {
            ex.Message.Equals("invalid ThirdPartyInvoiceKey. A ThirdPartyInvoiceKey must in the range (0, 2 147 483 647).");
        };
    }
}