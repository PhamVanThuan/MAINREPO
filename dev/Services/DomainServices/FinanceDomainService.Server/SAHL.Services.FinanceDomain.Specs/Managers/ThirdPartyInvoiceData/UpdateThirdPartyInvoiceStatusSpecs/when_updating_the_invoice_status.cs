using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.UpdateThirdPartyInvoiceStatusSpecs
{
    public class when_updating_the_invoice_status : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static ThirdPartyInvoiceDataManager dataManager;
        private static int thirdPartyInvoiceKey;
        private static InvoiceStatus newInvoiceStatus;

        private Establish context = () =>
            {
                thirdPartyInvoiceKey = 92364;
                newInvoiceStatus = InvoiceStatus.Paid;
                fakeDb = new FakeDbFactory();
                dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            };

        private Because of = () =>
            {
                dataManager.UpdateThirdPartyInvoiceStatus(thirdPartyInvoiceKey, newInvoiceStatus);
            };

        private It should_use_the_correct_statement = () =>
            {
                fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Update<ThirdPartyInvoiceDataModel>(Arg.Any<UpdateThirdPartyInvoiceStatusStatement>()));
            };

        private It should_use_the_invoice_key_provided = () =>
            {
                fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Update<ThirdPartyInvoiceDataModel>(Param<UpdateThirdPartyInvoiceStatusStatement>.Matches(
                    y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
            };

        private It should_use_the_status_provided = () =>
            {
                fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Update<ThirdPartyInvoiceDataModel>(Param<UpdateThirdPartyInvoiceStatusStatement>.Matches(
                    y => y.InvoiceStatusKey == (int)newInvoiceStatus)));
            };

        private It should_complete_the_db_context = () =>
            {
                fakeDb.FakedDb.InAppContext().Received().Complete();
            };
    }
}