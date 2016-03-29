using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.HasThirdPartyInvoiceHeaderChangedSpecs
{
    public class when_the_capitalise_invoice_has_changed : WithFakes
    {
        private static ThirdPartyInvoiceDataModel oldInvoice;
        private static ThirdPartyInvoiceModel newInvoice;
        private static FakeDbFactory fakeDb;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceManager dataFilter;
        private static bool headerHasChanged;

        private Establish context = () =>
            {
                fakeDb = new FakeDbFactory();
                dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
                dataFilter = new ThirdPartyInvoiceManager(dataManager);
                var thirdPartyId = Guid.NewGuid();
                oldInvoice = new ThirdPartyInvoiceDataModel(1111, "SAHL-2015/05/1111", (int)InvoiceStatus.Received, 1408282, thirdPartyId, "Inv/11//11/222", DateTime.MinValue,
                    "test@sahomeloans.com", null, null, null, true, DateTime.Now, string.Empty);
                newInvoice = new ThirdPartyInvoiceModel(1111, thirdPartyId, "Inv/11//11/222", DateTime.MinValue, Enumerable.Empty<InvoiceLineItemModel>(), false, string.Empty);
                fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Arg.Any<GetThirdPartyInvoiceByKeyStatement>()))
                    .Return(oldInvoice);
            };

        private Because of = () =>
        {
            headerHasChanged = dataFilter.HasThirdPartyInvoiceHeaderChanged(newInvoice);
        };

        private It should_return_true = () =>
        {
            headerHasChanged.ShouldBeTrue();
        };
    }
}