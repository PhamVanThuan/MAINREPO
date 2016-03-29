using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceDataSpec
{
    public class when_retrieving_invoice_batch_payment_information : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static FakeDbFactory dbFactory;
        private static int thirdPartyInvoiceKey;
        private static ThirdPartyInvoicePaymentBatchItem result, expectedResult;
        private static int catsPaymentBatchKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            thirdPartyInvoiceDataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            thirdPartyInvoiceKey = 12004;
            catsPaymentBatchKey = 324;

            expectedResult = new ThirdPartyInvoicePaymentBatchItem(
                1,
                thirdPartyInvoiceKey
                , (int)GenericKeyType.ThirdPartyInvoice
                , 1
                , 1.0m
                , 1
                , 1
                , "SAHL-Ref"
                , "inv-01"
                , catsPaymentBatchKey
                , ""
                , ""
                , ""
                , "Payment Reference"
            );

            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(y => y.SelectOne(Param.IsAny<GetCatsPaymentBatchItemInfoStatement>())).Return(expectedResult);
        };

        Because of = () =>
        {
            result = thirdPartyInvoiceDataManager.GetCatsPaymentBatchItemInformation(catsPaymentBatchKey, thirdPartyInvoiceKey);
        };

        It should_retrieve_the_data_from_the_db = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(y => y.SelectOne(
                Param<GetCatsPaymentBatchItemInfoStatement>.Matches(m =>
                    m.ThirdPartyInvoiceKey == thirdPartyInvoiceKey &&
                    m.ThirdPartyPaymentBatchKey == catsPaymentBatchKey
             )));
        };

        It should_return_the_invoice_payment_information = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}