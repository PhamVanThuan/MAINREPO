using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_third_party_invoice : WithFakes
    {
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             thirdPartyInvoice = new ThirdPartyInvoiceDataModel(132546,"sahlReference",(int)InvoiceStatus.Captured,
                                                                123456,Guid.NewGuid(),"invoiceNumber",DateTime.Now,
                                                                "vishavp@sahomeloans.com",99.0M,0,99.0M,false,DateTime.Now,
                                                                "paymentReference");
         };

        private Because of = () =>
         {
             testDataManager.UpdateThirdPartyInvoiceEmailAddress(thirdPartyInvoice);
         };

        private It should_update_the_third_party_invoice = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo
                 (x => x.Update<ThirdPartyInvoiceDataModel>(Arg.Is<ThirdPartyInvoiceDataModel>
                     (y=>y.ReceivedFromEmailAddress == thirdPartyInvoice.ReceivedFromEmailAddress
                   && y.InvoiceNumber == thirdPartyInvoice.InvoiceNumber)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}
