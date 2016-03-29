using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.QueryHandlers;

using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;

namespace SAHL.Services.Capitec.Specs.QueryHandlers
{
    public class when_getting_application_result_with_two_applicants : WithFakes
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
            firstApplicant = new ApplicantModel { Name = "Adam", ID = firstApplicantID, IncomeContributor = true};
            secondApplicant = new ApplicantModel { Name = "Betty", ID = secondApplicantID, IncomeContributor = true };

            calculationResult = new CreditPricingResult { Messages = SystemMessageCollection.Empty() };
            itcResult = new CreditBureauAssessmentResult { ITCMessages = SystemMessageCollection.Empty(), ITCPassed = true };

            query = new GetApplicationResultQuery(applicationID);
            handler = new GetApplicationResultQueryHandler(applicationService, decisionTreeResultService);

            applicationService.WhenToldTo(x => x.GetApplicantsForApplication(applicationID)).Return(new List<ApplicantModel>() { secondApplicant, firstApplicant });
            decisionTreeResultService.WhenToldTo(x => x.GetITCResultForApplicant(firstApplicantID)).Return(itcResult);
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
                decisionTreeResultService.WasToldTo(x => x.GetITCResultForApplicant(firstApplicantID));
            };
        It should_get_the_second_itc_for_the_second_applicant = () =>
            {
                decisionTreeResultService.WasToldTo(x => x.GetITCResultForApplicant(secondApplicantID));
            };
        It should_get_the_calculation_result_for_the_application = () =>
            {
                decisionTreeResultService.WasToldTo(x => x.GetCalculationResultForApplication(applicationID));
            };
        It should_order_the_applicants_alphabetically = () =>
        {
            query.Result.FirstApplicantName.ShouldEqual("Adam");
            query.Result.SecondApplicantName.ShouldEqual("Betty");
        };
    }
}
