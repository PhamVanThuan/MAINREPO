using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Rules;
using SAHL.Services.Interfaces.CATS.Models;
using System;

namespace SAHL.Services.CATS.Server.Specs.Rules
{
    public class when_batch_is_in_processed_state : WithFakes
    {
        static BatchShouldBeInProcessedStateRule rule;
        static CatsPaymentBatchRuleModel ruleModel;
        static ICATSDataManager catsDataManager;
        static CATSPaymentBatchDataModel catsBatchRecord;
        static ISystemMessageCollection messages;

        Establish context = () =>
        {
            catsDataManager = An<ICATSDataManager>();
            ruleModel = new CatsPaymentBatchRuleModel(batchKey: 96);
            rule = new BatchShouldBeInProcessedStateRule(catsDataManager);
            messages = SystemMessageCollection.Empty();

            catsBatchRecord = new CATSPaymentBatchDataModel(cATSPaymentBatchKey: 96, cATSPaymentBatchTypeKey: (int)CATSPaymentBatchType.ThirdPartyInvoice,
                createdDate: DateTime.Now, processedDate: DateTime.Now, cATSPaymentBatchStatusKey: (int)CATSPaymentBatchStatus.Processed, cATSFileSequenceNo: 1,
                cATSFileName: "catsFileName");
            catsDataManager.WhenToldTo(x => x.GetBatchByKey(ruleModel.BatchKey)).Return(catsBatchRecord);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_retrieve_the_batch_information = () =>
        {
            catsDataManager.WasToldTo(x => x.GetBatchByKey(ruleModel.BatchKey));
        };

        It should_not_add_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
