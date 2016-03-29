using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcesses.Rules;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.Rules.PrevBatchFileShouldNotHaveFailedSpecs
{
    public class when_prev_file_failed : WithFakes
    {
        private static PreviousBatchFileShouldNotHaveFailedRule previousBatchFileShouldNotHaveFailedRule;
        private static SystemMessageCollection systemMessages;
        private static ICATSServiceClient catsService;
       // private static GetPreviousFileFailureQuery getPreviousFileFailureQuery;
        private static string errorMessage;
        private Establish context = () =>
        {
            catsService = An<ICATSServiceClient>();
            systemMessages = new SystemMessageCollection();
            errorMessage = "The previous batch file processing failed";
            previousBatchFileShouldNotHaveFailedRule = new PreviousBatchFileShouldNotHaveFailedRule(catsService);
            catsService.WhenToldTo(x => x.PerformQuery(Param<GetPreviousFileFailureQuery>
                .Matches(y => y.CATSPaymentBatchType == Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice))).Return<GetPreviousFileFailureQuery>((getPreviousFileFailureQueryResult) => {getPreviousFileFailureQueryResult.Result = 
                    new ServiceQueryResult<GetPreviousFileFailureQueryResult>
                        (new List<GetPreviousFileFailureQueryResult>{new GetPreviousFileFailureQueryResult{BatchCreationDate= DateTime.Now,CATSFileName = ""}});
                     return SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            previousBatchFileShouldNotHaveFailedRule.ExecuteRule(systemMessages, null);
        };

        private It should_get_confirm_file_failure = () =>
        {
            catsService.WasToldTo(x => x.PerformQuery(Param<GetPreviousFileFailureQuery>
                .Matches(y => y.CATSPaymentBatchType == Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice)));
        };

        private It should_return_an_previous_file_failed_error_message = () =>
        {
            systemMessages.ErrorMessages().Any(x => x.Message.Equals(errorMessage)).ShouldBeTrue();
        };
    }
}