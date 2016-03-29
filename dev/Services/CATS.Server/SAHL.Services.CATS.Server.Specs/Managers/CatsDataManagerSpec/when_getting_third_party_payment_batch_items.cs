using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_getting_cats_payment_batch_items : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int batchKey;
        private static IDbFactory dbFactory;
        private static List<CATSPaymentBatchItemDataModel> catsPaymentBatchLineItems;
        private static List<CATSPaymentBatchItemDataModel> expectedCatsPaymentBatchLineItems;

        Establish context = () =>
        {
            batchKey = 44;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);
            expectedCatsPaymentBatchLineItems = new List<CATSPaymentBatchItemDataModel>() { new CATSPaymentBatchItemDataModel(5, 54, 10001, 2336.00m, 23, 22, 
                11210, "SAHL", "SAHL      SPV 32", "Strauss Daly", "External Reference", "straus@sd.co.za", 2004, true) };
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x =>
                x.Select<CATSPaymentBatchItemDataModel>(Param.IsAny<GetPaymentBatchLineItemsByBatchKeyStatement>()))
                .Return(expectedCatsPaymentBatchLineItems);
        };

        Because of = () =>
        {
            catsPaymentBatchLineItems = dataManager.GetPaymentBatchLineItemsByBatchKey(batchKey).ToList();
        };

        It should_return_the_batch_key_as_the_reference = () =>
        {
            catsPaymentBatchLineItems.ShouldEqual(expectedCatsPaymentBatchLineItems);
        };
    }
}
