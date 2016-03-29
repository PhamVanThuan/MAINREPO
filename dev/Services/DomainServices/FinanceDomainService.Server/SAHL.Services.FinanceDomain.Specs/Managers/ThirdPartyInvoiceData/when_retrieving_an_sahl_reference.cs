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
    public class when_retrieving_an_sahl_reference : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static string expectedReference, generatedReference;
        private static FakeDbFactory dbFactory;
        private static int thirdPartyInvoiceKey = 3;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;
        private static int accountKey = 892437;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            expectedReference = "SAHL03-2015/03/1";
            thirdPartyInvoice = new ThirdPartyInvoiceDataModel(expectedReference, 1, accountKey, combGuid.Generate(), "", DateTime.Now,
                "attorney@partners.co.za", null, null, null, true, DateTime.Now, string.Empty);
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(y => y.SelectOne(Param.IsAny<GetThirdPartyInvoiceByKeyStatement>())).Return(thirdPartyInvoice);
        };

        private Because of = () =>
        {
            generatedReference = dataManager.RetrieveSAHLReference(thirdPartyInvoiceKey);
        };

        private It should_get_a_refrence_generated = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetThirdPartyInvoiceByKeyStatement>.Matches(m => m.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };

        private It should_get_a_valid_reference = () =>
        {
            generatedReference.ShouldEqual(expectedReference);
        };
    }
}