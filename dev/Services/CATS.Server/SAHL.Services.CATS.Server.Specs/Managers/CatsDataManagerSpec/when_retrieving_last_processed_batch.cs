using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using System;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_retrieving_last_processed_batch : WithFakes
    {
        static ICATSDataManager catsDataManager;
        static IDbFactory dbFactory;
        static CATSPaymentBatchType batchType;
        static CATSPaymentBatchDataModel lastProcessedBatch, expectedBatch;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            catsDataManager = new CATSDataManager(dbFactory);
            batchType = CATSPaymentBatchType.ThirdPartyInvoice;
            expectedBatch = new CATSPaymentBatchDataModel(
                cATSPaymentBatchKey: 120
                , cATSPaymentBatchTypeKey: (int)batchType
                , createdDate: DateTime.Today
                , processedDate: DateTime.Now
                , cATSPaymentBatchStatusKey: (int)CATSPaymentBatchStatus.Processed
                , cATSFileSequenceNo: 2
                , cATSFileName: "SAHL_Disbursment_" + DateTime.Today.ToString("yyyyMMddhhmmss"));

            dbFactory.NewDb().InAppContext().WhenToldTo(x => x.SelectOne(Param<GetLastProcessedBatchQueryStatement>.Matches(y =>
                y.BatchTypeKey == (int)batchType)
              )).Return(expectedBatch);
        };

        Because of = () =>
        {
            lastProcessedBatch = catsDataManager.GetLastProcessedBatch(batchType);
        };

        It should_get_the_last_processed_batch_from_the_db = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.SelectOne(Param<GetLastProcessedBatchQueryStatement>.Matches(y =>
                y.BatchTypeKey == (int)batchType)
              ));
        };

        It should_return_the_batch = () =>
        {
            lastProcessedBatch.ShouldEqual(expectedBatch);
        };
    }
}
