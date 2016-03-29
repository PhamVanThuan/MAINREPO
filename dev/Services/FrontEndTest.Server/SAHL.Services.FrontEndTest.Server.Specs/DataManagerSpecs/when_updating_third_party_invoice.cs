using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;
using System;


namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_third_party_invoice : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            thirdPartyInvoice = new ThirdPartyInvoiceDataModel("", 0, 0, new System.Guid(), "", DateTime.Now,
                                                               "", null, null, null,  false, DateTime.Now, string.Empty);
        };

        private Because of = () =>
        {
            testDataManager.UpdateThirdPartyInvoice(thirdPartyInvoice);
        };

        private It should_complete_the_in_app_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x=>x.Complete());
        };

        private It should_use_a_string_for_creating_the_disability_payment_schedule = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo
                  (x => x.Update <ThirdPartyInvoiceDataModel>(Arg.Is<ThirdPartyInvoiceDataModel>
                      (y => y.ThirdPartyInvoiceKey == thirdPartyInvoice.ThirdPartyInvoiceKey 
                          && y.InvoiceStatusKey == thirdPartyInvoice.InvoiceStatusKey)));
        };
    }
}
