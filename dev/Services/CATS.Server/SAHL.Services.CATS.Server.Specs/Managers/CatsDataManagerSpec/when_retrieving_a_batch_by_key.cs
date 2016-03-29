using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using System;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_retrieving_a_batch_by_key : WithFakes
    {
        static ICATSDataManager catsDataManager;
        static FakeDbFactory dbFactory;
        static int batchNumber;
        static CATSPaymentBatchDataModel batchRecord, expectedBatchRecord;

        Establish context = () =>
        {
            batchNumber = 96;
            dbFactory = new FakeDbFactory();
            catsDataManager = new CATSDataManager(dbFactory);

            expectedBatchRecord = new CATSPaymentBatchDataModel(cATSPaymentBatchKey: 96, cATSPaymentBatchTypeKey: (int)CATSPaymentBatchType.ThirdPartyInvoice,
                createdDate: DateTime.Now, processedDate: DateTime.Now, cATSPaymentBatchStatusKey: (int)CATSPaymentBatchStatus.Processed, cATSFileSequenceNo: 1,
                cATSFileName: "catsFileName");

            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<GetBatchByKeyStatement>.Matches(s => s.BatchKey == batchNumber)))
                .Return(expectedBatchRecord);
        };

        Because of = () =>
        {
            batchRecord = catsDataManager.GetBatchByKey(batchNumber);
        };

        It should_get_the_batch_from_the_db = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetBatchByKeyStatement>.Matches(s => s.BatchKey == batchNumber)));
        };

        It should_return_the_batch = () =>
        {
            batchRecord.ShouldEqual(expectedBatchRecord);
        };
    }
}
