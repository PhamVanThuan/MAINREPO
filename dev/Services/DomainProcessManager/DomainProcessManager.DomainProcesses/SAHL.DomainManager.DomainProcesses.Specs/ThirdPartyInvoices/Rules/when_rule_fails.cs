using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcesses.Rules;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.Rules.PayAttorneyInvoiceProcessShouldRunOnceADaySpecs
{
    public class when_rule_fails : WithFakes
    {
        private static PayAttorneyInvoiceProcessShouldRunOnceADayRule payAttorneyInvoiceProcessShouldRunOnceADayRule;
        private static SystemMessageCollection systemMessages;
        private static ICATSServiceClient catsService;
        private static string errorMessage;

        private Establish context = () =>
        {
            catsService = An<ICATSServiceClient>();
            systemMessages = new SystemMessageCollection();
            errorMessage = "The Pay Attorney Invoice Process has already been executed";
            payAttorneyInvoiceProcessShouldRunOnceADayRule = new PayAttorneyInvoiceProcessShouldRunOnceADayRule(catsService);
            catsService.WhenToldTo(x => x.PerformQuery(Param.IsAny<DoesCatsPaymentBatchForTodayExistQuery>()))
                .Return<DoesCatsPaymentBatchForTodayExistQuery>((getPreviousFileFailureQueryResult) =>
                {
                    getPreviousFileFailureQueryResult.Result =
                    new ServiceQueryResult<DoesCatsPaymentBatchForTodayExistModel>
                        (new List<DoesCatsPaymentBatchForTodayExistModel> { new DoesCatsPaymentBatchForTodayExistModel { BatchExists = true } });
                    return SystemMessageCollection.Empty();
                });
        };

        private Because of = () =>
        {
            payAttorneyInvoiceProcessShouldRunOnceADayRule.ExecuteRule(systemMessages, null);
        };

        private It should_get_confirm_that_the_process_has_already_executed = () =>
        {
            catsService.WasToldTo(x => x.PerformQuery(Param.IsAny<DoesCatsPaymentBatchForTodayExistQuery>()));
        };

        private It should_return_an_error_message = () =>
        {
            systemMessages.ErrorMessages().Any(x => x.Message.Equals(errorMessage)).ShouldBeTrue();
        };
    }
}