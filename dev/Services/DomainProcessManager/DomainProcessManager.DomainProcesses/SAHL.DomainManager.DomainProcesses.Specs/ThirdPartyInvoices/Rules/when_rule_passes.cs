using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcesses.Rules;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.Rules.PayAttorneyInvoiceProcessShouldRunOnceADayRuleSpecs
{
    public class when_rule_passes : WithFakes
    {
        private static PayAttorneyInvoiceProcessShouldRunOnceADayRule payAttorneyInvoiceProcessShouldRunOnceADayRule;
        private static SystemMessageCollection systemMessages;
        private static ICATSServiceClient catsService;

        private Establish context = () =>
        {
            catsService = An<ICATSServiceClient>();
            systemMessages = new SystemMessageCollection();
            payAttorneyInvoiceProcessShouldRunOnceADayRule = new PayAttorneyInvoiceProcessShouldRunOnceADayRule(catsService);
            catsService.WhenToldTo(x => x.PerformQuery(Param.IsAny<DoesCatsPaymentBatchForTodayExistQuery>()))
                .Return<DoesCatsPaymentBatchForTodayExistQuery>((getPreviousFileFailureQueryResult) =>
                {
                    getPreviousFileFailureQueryResult.Result =
                    new ServiceQueryResult<DoesCatsPaymentBatchForTodayExistModel>
                        (new List<DoesCatsPaymentBatchForTodayExistModel> { new DoesCatsPaymentBatchForTodayExistModel { BatchExists = false } });
                    return SystemMessageCollection.Empty();
                });
        };

        private Because of = () =>
        {
            payAttorneyInvoiceProcessShouldRunOnceADayRule.ExecuteRule(systemMessages, null);
        };

        private It should_get_confirm_that_the_process_has_not_executed = () =>
        {
            catsService.WasToldTo(x => x.PerformQuery(Param.IsAny<DoesCatsPaymentBatchForTodayExistQuery>()));
        };

        private It should_not_return_error_message = () =>
        {
            systemMessages.HasErrors.ShouldBeFalse();
        };
    }
}