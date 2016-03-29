using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.QueryHandlers;

using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using System.Linq;
using SAHL.Services.Capitec.Managers.Application;

namespace SAHL.Services.Capitec.Specs.QueryHandlers
{
    public class when_getting_application_result_with_non_income_contrib : WithFakes
    {
        private static GetApplicationResultQueryHandler handler;
        private static GetApplicationResultQuery query;
        private static IApplicationManager applicationService;
        private static IDecisionTreeResultManager decisionTreeResultService;
        private static Guid applicationID;
        private static Guid firstApplicantID;
        private static Guid secondApplicantID;
        private static ApplicantModel firstApplicant;
        private static ApplicantModel secondApplicant;
        private static CreditPricingResult calculationResult;
        private static CreditBureauAssessmentResult itcResult;
        Establish context = () =>
        {
            applicationService = An<IApplicationManager>();
            decisionTreeResultService = An<IDecisionTreeResultManager>();
            applicationID = Guid.NewGuid();
            firstApplicantID = Guid.NewGuid();
            secondApplicantID = Guid.NewGuid();
            firstApplicant = new ApplicantModel { Name = "Adam", ID = firstApplicantID, IncomeContributor = false};
            secondApplicant = new ApplicantModel { Name = "Betty", ID = secondApplicantID, IncomeContributor = true };

            var message = new SystemMessage("Empirica score is too low.", SystemMessageSeverityEnum.Warning);
            var itcMessages = new SystemMessageCollection();
            itcMessages.AddMessage(message);
            calculationResult = new CreditPricingResult { Messages = SystemMessageCollection.Empty() };
            itcResult = new CreditBureauAssessmentResult { ITCMessages = itcMessages, ITCPassed = true };

            query = new GetApplicationResultQuery(applicationID);
            handler = new GetApplicationResultQueryHandler(applicationService, decisionTreeResultService);

            applicationService.WhenToldTo(x => x.GetApplicantsForApplication(applicationID)).Return(new List<ApplicantModel>() { secondApplicant, firstApplicant });
            decisionTreeResultService.WhenToldTo(x => x.GetITCResultForApplicant(secondApplicantID)).Return(itcResult);
            decisionTreeResultService.WhenToldTo(x => x.GetCalculationResultForApplication(applicationID)).Return(calculationResult);
        };
        Because of = () =>
        {
            handler.HandleQuery(query);
        };
        It should_get_the_applicants_related_to_the_application = () =>
            {
                applicationService.WasToldTo(x => x.GetApplicantsForApplication(applicationID));
            };
        It should_get_the_first_itc_for_the_applicant = () =>
            {
                decisionTreeResultService.WasToldTo(x => x.GetITCResultForApplicant(secondApplicantID));
            };
        It should_not_get_the_second_itc_for_the_second_applicant = () =>
            {
                decisionTreeResultService.WasNotToldTo(x => x.GetITCResultForApplicant(firstApplicantID));
            };
        It should_get_the_calculation_result_for_the_application = () =>
            {
                decisionTreeResultService.WasToldTo(x => x.GetCalculationResultForApplication(applicationID));
            };
        It should_include_messages_for_the_income_contributor = () =>
        {
            query.Result.FirstApplicantName.ShouldEqual("Betty");
            query.Result.FirstApplicantITCMessages.AllMessages.ShouldContain(itcResult.ITCMessages.AllMessages.First());
        };
        It should_not_include_messages_for_the_non_income_contributor_applicant = () =>
        {
            query.Result.SecondApplicantName.ShouldBeNull();
            query.Result.SecondApplicantITCMessages.ShouldBeNull();
        };
    }
}
