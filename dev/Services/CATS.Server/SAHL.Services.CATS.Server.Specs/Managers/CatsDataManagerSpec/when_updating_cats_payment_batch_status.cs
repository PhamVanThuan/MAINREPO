using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_updating_cats_payment_batch_status
 : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int batchKey, fileSequenceNumber;
        private static int catsPaymentBatchStatus;
        private static string filename;

        private static IDbFactory dbFactory;

        Establish context = () =>
        {
            filename = "filename";
            batchKey = 44;
            fileSequenceNumber = 2;
            catsPaymentBatchStatus = (int)CATSPaymentBatchStatus.Processed;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);

        };

        Because of = () =>
        {
            dataManager.CloseCatsPaymentBatch(batchKey, catsPaymentBatchStatus, fileSequenceNumber, filename);
        };

        It should_update_the_cats_payment_batch_to_processed = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Update<CATSPaymentBatchStatusDataModel>(Param<UpdatePaymentBatchStatusStatement>.Matches(y => y.PaymentBatchKey.Equals(batchKey)
                    && y.PaymentBatchStatus.Equals(catsPaymentBatchStatus)
                    && y.FileSequenceNumber == fileSequenceNumber
                    )));
        };

        It should_complete = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
