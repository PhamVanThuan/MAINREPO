using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.QueryHandlers;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.QueryHandlerSpec
{
    public class when_checking_if_there_are_pending_file : WithFakes
    {
        private static IsThereACatsFileBeingProcessedForProfileQuery query;
        private static IsThereACatsFileBeingProcessedForProfileQueryHandler handler;
        private static ICATSDataManager catsDataManager;
        private static ICATSManager catsHelper;
        private static CATSPaymentBatchTypeDataModel batchTypeDataModel;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
         {
             catsDataManager = An<ICATSDataManager>();
             catsHelper = An<ICATSManager>();

             query = new IsThereACatsFileBeingProcessedForProfileQuery(1);
             handler = new IsThereACatsFileBeingProcessedForProfileQueryHandler(catsHelper, catsDataManager);

             batchTypeDataModel = new CATSPaymentBatchTypeDataModel(
                 cATSPaymentBatchTypeKey: 234
                 , description: "third party"
                 , cATSProfile: "SH02"
                 , cATSFileNamePrefix: "SAHL_Attorney_invoice"
                 , cATSEnvironment: 1
                 , nextCATSFileSequenceNo: 2);

             catsDataManager.WhenToldTo(x => x.GetBatchTypeByKey(query.CatsBatchTypeKey)).Return(batchTypeDataModel);

             catsHelper.WhenToldTo(x => x.IsThereACatsFileBeingProcessedForProfile(batchTypeDataModel.CATSProfile)).Return(true);
         };

        private Because of = () =>
         {
             messages = handler.HandleQuery(query);
         };

        private It should_get_batch_tpye = () =>
         {
             catsDataManager.WasToldTo(x => x.GetBatchTypeByKey(query.CatsBatchTypeKey));
         };

        private It should_confirm_that_there_is_a_pending_file = () =>
         {
             catsHelper.WasToldTo(x => x.IsThereACatsFileBeingProcessedForProfile(batchTypeDataModel.CATSProfile));
         };

        private It should_return_true = () =>
         {
             query.Result.Results.FirstOrDefault().ShouldBeTrue();
         };

        private It should_return_an_empty_message_collection = () =>
         {
             messages.AllMessages.Count().ShouldEqual(0);
         };
    }
}