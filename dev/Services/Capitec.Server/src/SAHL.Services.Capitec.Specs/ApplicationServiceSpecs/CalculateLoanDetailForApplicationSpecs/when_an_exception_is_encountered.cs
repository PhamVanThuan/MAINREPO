using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs.CalculateLoanDetailForApplicationSpecs
{
    public class when_an_exception_is_encountered : WithFakes
    {
        private static IApplicationManager applicationService;
        private static ILookupManager lookupService;
        private static IApplicationDataManager applicationDataService;
        private static IApplicantManager applicantService;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static IDecisionTreeResultManager decisionTreeResultService;
        private static IITCManager applicantITCService;
        private static CalculatedLoanDetailsModel calculationResults;
        private static ApplicationLoanDetails applicationLoanDetails;
        private static List<Applicant> applicants;
        private static bool eligibleBorrower;
        private static Guid applicationId;
        private static ApplicantStubs stubs;
        private static SwitchLoanDetails switchLoanDetails;
        private static decimal interestRate, instalment, loanAmount, loanToValue, paymentToIncome;
        private static string dtEmploymentTypeEnum;
        private static string dtOccupancyTypeEnum;
        private static string dtApplicationTypeEnum;
        private static IServiceQueryResult<CapitecOriginationCreditPricing_QueryResult> decisionTreeResult;
        private static Exception ex;

        private Establish context = () =>
        {
            interestRate = 0.086M;
            instalment = 5000M;
            loanAmount = 500000M;
            loanToValue = 0.60M;
            paymentToIncome = 0.25M;
            dtEmploymentTypeEnum = new Enumerations.SAHomeLoans.HouseholdIncomeType().Salaried;
            dtOccupancyTypeEnum = new Enumerations.SAHomeLoans.PropertyOccupancyType().OwnerOccupied;
            dtApplicationTypeEnum = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch;
            decisionTreeService = An<IDecisionTreeServiceClient>();
            lookupService = An<ILookupManager>();
            decisionTreeResultService = An<IDecisionTreeResultManager>();
            applicantService = An<IApplicantManager>();
            applicantService.WhenToldTo(x => x.CalculateAge(Param.IsAny<DateTime?>(), Param.IsAny<DateTime>())).Return(25);
            lookupService.WhenToldTo(x => x.GetDecisionTreeHouseholdIncomeType(Param.IsAny<Guid>())).Return(dtEmploymentTypeEnum);
            lookupService.WhenToldTo(x => x.GetDecisionTreeOccupancyType(Param.IsAny<Guid>())).Return(dtOccupancyTypeEnum);
            stubs = new ApplicantStubs();
            stubs.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);
            applicationService = new ApplicationManager(lookupService, applicantService, applicationDataService, decisionTreeService, decisionTreeResultService, applicantITCService, serviceCommandRouter);
            applicationId = Guid.NewGuid();
            eligibleBorrower = true;
            applicants = new List<Applicant>() { stubs.GetApplicant };
            applicants.First().EmpiricaScore = 650;
            applicants.First().EmploymentDetails.EmploymentTypeEnumId = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            applicants.First().EmploymentDetails.SalariedDetails = new SalariedDetails(20000M);
            eligibleBorrower = true;
            switchLoanDetails = new SwitchLoanDetails(Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), 20000M, 500000M, 100000M, 100000M, 5000M, 0M, 240, true);
            applicationLoanDetails = new ApplicationLoanDetails(switchLoanDetails);
            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Throw(new NullReferenceException());
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                calculationResults = applicationService.CalculateLoanDetailForApplication(applicationLoanDetails, applicants, eligibleBorrower, applicationId);
            });
        };

        private It should_throw_the_exception = () =>
        {
            ex.ShouldBeOfExactType<Exception>();
        };

        private It should_return_a_message_indicating_the_error_occurred_when_running_the_tree = () =>
        {
            ex.Message.ShouldEqual("An error ocurred while running the Capitec Origination Credit Pricing Decision Tree");
        };
        private static IServiceCommandRouter serviceCommandRouter;
    }
}
