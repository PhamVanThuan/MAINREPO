using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using SAHL.Services.Capitec.Managers.Lookup;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.DecisionTreeResultsServiceSpecs
{
    public class when_getting_a_credit_assessment_tree_result_with_no_itc : WithFakes
    {
        private static IDecisionTreeResultDataManager dataService;
        private static ILookupManager lookupService;
        private static DecisionTreeResultManager service;
        private static CreditBureauAssessmentResult result;
        private static Guid applicantID;

        private Establish context = () =>
        {
            dataService = An<IDecisionTreeResultDataManager>();
            lookupService = An<ILookupManager>();
            service = new DecisionTreeResultManager(dataService, lookupService);
            applicantID = Guid.NewGuid();
        };

        private Because of = () =>
        {
            result = service.GetITCResultForApplicant(applicantID);
        };

        private It should_get_the_credit_assessment_tree_result = () =>
        {
            dataService.WasToldTo(x => x.GetCreditBureauAssessmentTreeResultForApplicant(applicantID));
        };

        private It should_return_the_warning_message_that_no_itc_was_performed = () =>
        {
            result.ITCMessages.AllMessages.First().Message.ShouldEqual("No credit bureau check was performed.");
            result.ITCMessages.AllMessages.First().Severity.ShouldEqual(SystemMessageSeverityEnum.Warning);
        };

        private It should_return_the_itc_passed_result = () =>
        {
            result.ITCPassed.ShouldBeTrue();
        };
    }
}