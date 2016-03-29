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
    public class when_file_for_profile_is_being_processed : WithFakes
    {
        private static NoCatsFileForProfileShouldBePendindingRule noProfileCatsFileProcessingShouldBeInProgressRule;
        private static SystemMessageCollection systemMessages;
        private static ICATSServiceClient catsService;
        private static int catsBatchType = 1;
       // private static GetPreviousFileFailureQuery getPreviousFileFailureQuery;
        private static string errorMessage;
        private Establish context = () =>
        {
            catsService = An<ICATSServiceClient>();
            systemMessages = new SystemMessageCollection();
            noProfileCatsFileProcessingShouldBeInProgressRule = new NoCatsFileForProfileShouldBePendindingRule(catsService);
            errorMessage = "There is a pending payment, please try again after 15 minutes.";
            catsService.WhenToldTo(x => x.PerformQuery(Param<IsThereACatsFileBeingProcessedForProfileQuery>
                .Matches(y => y.CatsBatchTypeKey == catsBatchType))).Return<IsThereACatsFileBeingProcessedForProfileQuery>((getPreviousFileFailureQueryResult) =>
                {
                    getPreviousFileFailureQueryResult.Result = 
                    new ServiceQueryResult<bool>
                        (new List<bool>{true});
                     return SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            noProfileCatsFileProcessingShouldBeInProgressRule.ExecuteRule(systemMessages, null);
        };

        private It should_get_confirm_profile_cats_file_processing_in_progress = () =>
        {
            catsService.WasToldTo(x => x.PerformQuery(Param<IsThereACatsFileBeingProcessedForProfileQuery>
                .Matches(y => y.CatsBatchTypeKey == catsBatchType)));
        };

        private It should_return_an_previous_file_failed_error_message = () =>
        {
            systemMessages.ErrorMessages().Any(x => x.Message.Equals(errorMessage)).ShouldBeTrue();
        };
    }
}