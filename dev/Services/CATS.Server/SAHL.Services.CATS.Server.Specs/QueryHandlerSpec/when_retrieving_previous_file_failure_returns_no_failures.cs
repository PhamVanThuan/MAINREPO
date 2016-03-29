using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.QueryHandlers;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.QueryHandlerSpec
{
    public class when_retrieving_previous_file_failure_returns_no_failures : WithFakes
    {
        private static GetPreviousFileFailureQuery query;
        private static GetPreviousFileFailureQueryHandler handler;
        private static ICATSDataManager catsDataManager;
        private static ICATSManager catsHelper;
        private static CATSPaymentBatchTypeDataModel batchTypeDataModel;
        private static ISystemMessageCollection messages;
        private static CATSPaymentBatchDataModel lastProcessedBath;

        private Establish context = () =>
         {
             catsDataManager = An<ICATSDataManager>();
             catsHelper = An<ICATSManager>();
             query = new GetPreviousFileFailureQuery(CATSPaymentBatchType.ThirdPartyInvoice);
             handler = new GetPreviousFileFailureQueryHandler(catsDataManager, catsHelper);

             batchTypeDataModel = new CATSPaymentBatchTypeDataModel(
                 cATSPaymentBatchTypeKey: 234
                 , description: "third party"
                 , cATSProfile: ""
                 , cATSFileNamePrefix: "SAHL_Attorney_invoice"
                 , cATSEnvironment: 1
                 , nextCATSFileSequenceNo: 2);

             lastProcessedBath = new CATSPaymentBatchDataModel(cATSPaymentBatchKey: 96, cATSPaymentBatchTypeKey: (int)CATSPaymentBatchType.ThirdPartyInvoice,
                 createdDate: DateTime.Now, processedDate: DateTime.Now, cATSPaymentBatchStatusKey: (int)CATSPaymentBatchStatus.Processed, cATSFileSequenceNo: 1,
                 cATSFileName: "catsFileName");

             catsDataManager.WhenToldTo(x => x.GetLastProcessedBatch(query.CATSPaymentBatchType)).Return(lastProcessedBath);
         };

        private Because of = () =>
         {
             messages = handler.HandleQuery(query);
         };

        private It should_get_the_last_processed_or_processing_batch = () =>
         {
             catsDataManager.WasToldTo(x => x.GetLastProcessedBatch(query.CATSPaymentBatchType));
         };

        private It should_check_if_the_the_file_failed_processing = () =>
         {
             catsHelper.WasToldTo(x => x.HasFileFailedProcessing(lastProcessedBath.CATSFileName));
         };

        private It should_set_an_empty_collection_in_the_query_result = () =>
         {
             query.Result.Results.Count().ShouldEqual(0);
         };

        private It should_return_an_empty_message_collection = () =>
         {
             messages.AllMessages.Count().ShouldEqual(0);
         };
    }
}