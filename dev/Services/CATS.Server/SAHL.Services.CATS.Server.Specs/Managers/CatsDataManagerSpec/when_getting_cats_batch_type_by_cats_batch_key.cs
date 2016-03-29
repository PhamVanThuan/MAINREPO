using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using SAHL.Services.Interfaces.CATS.Enums;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_getting_cats_batch_type_by_cats_batch_key : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int catsPaymentBatchKey;
        private static IDbFactory dbFactory;
        private static CATSPaymentBatchTypeDataModel catsPaymentBatchType;
        private static CATSPaymentBatchTypeDataModel expectedCatsPaymentBatchType;
        private static CATsEnvironment catsEnvironment;
        private static string fullFileName;
        private static string SSVSUserID;

        Establish context = () =>
        {
            catsPaymentBatchKey = 44;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);
            fullFileName = @"c:\\temp";
            catsEnvironment = CATsEnvironment.Live;
            SSVSUserID = "SHL04";
            expectedCatsPaymentBatchType = new CATSPaymentBatchTypeDataModel("ThirdpartyInvoice", SSVSUserID, fullFileName, (int)catsEnvironment, 1);
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x =>
                x.SelectOne<CATSPaymentBatchTypeDataModel>(Param.IsAny<GetPaymentBatchTypeByPaymentBatchKeyStatement>()))
                .Return(expectedCatsPaymentBatchType);
        };

        Because of = () =>
        {
            catsPaymentBatchType = dataManager.GetBatchTypeInfo(catsPaymentBatchKey);
        };

        It should_return_cats_payment_batch_type = () =>
        {
            catsPaymentBatchType.ShouldEqual(expectedCatsPaymentBatchType);
        };
    }
}
