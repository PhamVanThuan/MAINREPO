using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.CATS.Managers.CATS;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.NotifyBatchPaymentRecipients
{
    public class when_retrieving_batch_items_fails : WithFakes
    {
        private static NotifyCATSPaymentBatchRecipientsCommandHandler handler;
        private static NotifyCATSPaymentBatchRecipientsCommand command;
        private static ICATSDataManager catsDataManager;
        private static ICATSManager catsManager;
        private static ISystemMessageCollection messages;
        private static int batchKey;
        private static IServiceRequestMetadata metadata;
        private static IEventRaiser eventRaiser;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IEnumerable<CATSPaymentBatchItemDataModel> catsPaymentBatchItems;
        private static IDomainRuleManager<CatsPaymentBatchRuleModel> domainRuleManager;

        private Establish context = () =>
         {
             batchKey = 96;
             catsDataManager = An<ICATSDataManager>();
             catsManager = An<ICATSManager>();
             eventRaiser = An<IEventRaiser>();
             serviceCommandRouter = An<IServiceCommandRouter>();
             serviceQueryRouter = An<IServiceQueryRouter>();
             metadata = An<IServiceRequestMetadata>();
             domainRuleManager = An<IDomainRuleManager<CatsPaymentBatchRuleModel>>();
             command = new NotifyCATSPaymentBatchRecipientsCommand(batchKey);
             handler = new NotifyCATSPaymentBatchRecipientsCommandHandler(catsDataManager, catsManager, serviceCommandRouter, serviceQueryRouter, eventRaiser, domainRuleManager);

             catsPaymentBatchItems = new List<CATSPaymentBatchItemDataModel>{
                        new CATSPaymentBatchItemDataModel(34, 14008, (int) GenericKeyType.ThirdPartyInvoice, 42006,
                            200m, 32109, 8409, 96,
                            "SAHL/02/08/20015-23", "SAHL SPV", "Strauss Daly", "", "strauss@sd.co.za", 54007, true)
                     };
             var errorMessages = new SystemMessageCollection();
             errorMessages.AddMessage(new SystemMessage("An error has occured", SystemMessageSeverityEnum.Error));

             serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param<GetCatsPaymentBatchItemsByBatchReferenceQuery>.Matches(q =>
                     q.BatchKey == command.CATSPaymentBatchKey)
                 )).Return<GetCatsPaymentBatchItemsByBatchReferenceQuery>(x =>
                 {
                     x.Result = new ServiceQueryResult<CATSPaymentBatchItemDataModel>(new List<CATSPaymentBatchItemDataModel>());
                     return errorMessages;
                 });
         };

        private Because of = () =>
         {
             messages = handler.HandleCommand(command, metadata);
         };

        private It should_retrieve_the_batch_payment_items = () =>
         {
             serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetCatsPaymentBatchItemsByBatchReferenceQuery>.Matches(q =>
                     q.BatchKey == command.CATSPaymentBatchKey)
                 ));
         };

        private It should_return_error_messages = () =>
         {
             messages.HasErrors.ShouldBeTrue();
         };

        private It should_not_group_the_payment_items_by_legal_entity = () =>
         {
             catsManager.WasNotToldTo(x => x.GroupBatchPaymentsByRecipient(Param.IsAny<IEnumerable<CATSPaymentBatchItemDataModel>>()));
         };

        private It should_not_summarise_each_payment_group = () =>
         {
             serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<ICATSServiceCommand>(), Param.IsAny<IServiceRequestMetadata>()));
         };

        private It should_not_raise_an_event = () =>
         {
             eventRaiser.WasNotToldTo(x => x.RaiseEvent(
                   Param.IsAny<DateTime>()
                 , Param.IsAny<IEvent>()
                 , Param.IsAny<int>()
                 , Param.IsAny<int>()
                 , Param.IsAny<IServiceRequestMetadata>())
              );
         };
    }
}