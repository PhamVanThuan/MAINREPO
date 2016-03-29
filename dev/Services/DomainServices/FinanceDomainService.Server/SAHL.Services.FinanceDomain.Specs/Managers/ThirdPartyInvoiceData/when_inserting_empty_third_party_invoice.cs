using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using System;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_inserting_empty_third_party_invoice : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static int accountKey;
        private static Guid correlationId;
        private static FakeDbFactory dbFactory;
        private static string sahlReference;
        private static string emailAddress;
        private static DateTime receivedDate;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            emailAddress = "test@sahomeloans.com";
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            accountKey = 123;
            correlationId = Guid.Parse("{D22EBA8E-A983-4AA3-855A-2C03C1B4D72E}");
            sahlReference = "SAHL03-2015/03/1";
            receivedDate = DateTime.Now;

            dbFactory.FakedDb.InAppContext().WhenToldTo(x => x.Select(Param.IsAny<GetSAHLReferenceStatement>())).Return(new string[] { sahlReference });
        };

        private Because of = () =>
        {
            dataManager.SaveEmptyThirdPartyInvoice(accountKey, correlationId, emailAddress, receivedDate);
        };

        private It should_assign_a_unique_sahl_reference = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Select(Param<GetSAHLReferenceStatement>.IsNotNull));
        };

        private It should_insert_the_third_party_invoice = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ThirdPartyInvoiceDataModel>.Matches(m =>
                m.SahlReference == sahlReference &&
                m.InvoiceStatusKey == 1 &&
                m.AccountKey == accountKey && m.ReceivedFromEmailAddress == emailAddress)));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}