using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_recalculating_a_switch_loan_and_ITCs_are_unavailable : with_application_service
    {
        private static SwitchLoanApplication newPurchaseApplication;
        private static SwitchLoanDetails loanDetails;
        private static LoanApplication application;

        private static ApplicantStubs applicantStubs_1;
        private static ApplicantStubs applicantStubs_2;
        private static Guid occupancyTypeID;
        private static Guid incomeTypeID;
        private static Guid generatedID;
        private static List<Applicant> applicants;
        private static Guid applicationID;
        private static Dictionary<Guid, Applicant> addedApplicants;

        private static decimal cashRequired = 150000;
        private static decimal currentBalance = 500000;
        private static decimal estimatedMarketValue = 3000000;
        private static decimal totalHouseHoldIncome;
        private static decimal interestRate = 0;
        private static decimal instalment = 0;
        private static decimal loanAmount = 0;
        private static decimal loanToValue = 0;
        private static decimal paymentToIncome = 0;
        private static Guid branchId;

        private static bool result;

        private Establish context = () =>
        {
            occupancyTypeID = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
            incomeTypeID = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            generatedID = Guid.NewGuid();
            totalHouseHoldIncome = 45000;
            branchId = Guid.NewGuid();
            applicationID = Guid.NewGuid();
            addedApplicants = new Dictionary<Guid, Applicant>();
            applicants = new List<Applicant>();

            applicantStubs_1 = new ApplicantStubs();
            applicantStubs_1.GrossMonthlyIncome = 45000;
            applicantStubs_1.EmploymentTypeEnumId = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            applicantStubs_1.IncomeContributor = Guid.Parse(DeclarationTypeEnumDataModel.YES);
            var firstApplicant = applicantStubs_1.GetApplicant;
            firstApplicant.EmpiricaScore = -999;

            applicantStubs_2 = new ApplicantStubs();
            applicantStubs_2.GrossMonthlyIncome = 45000;
            var secondApplicant = applicantStubs_2.GetApplicant;
            secondApplicant.EmpiricaScore = -999;

            applicants.Add(applicantStubs_1.GetApplicant);
            applicants.Add(applicantStubs_2.GetApplicant);
            addedApplicants.Add(Guid.NewGuid(), firstApplicant);
            addedApplicants.Add(Guid.NewGuid(), secondApplicant);

            newPurchaseApplication = new SwitchLoan(cashRequired, currentBalance, estimatedMarketValue, totalHouseHoldIncome, occupancyTypeID, incomeTypeID, applicants).GetSwitchLoanApplication;
            loanDetails = newPurchaseApplication.SwitchLoanDetails;

            application = new LoanApplication(newPurchaseApplication.ApplicationDate, new ApplicationLoanDetails(loanDetails), newPurchaseApplication.Applicants, newPurchaseApplication.UserId, new DateTime(2014, 10, 10), branchId);

            var serviceQueryResult = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(new CapitecOriginationCreditPricing_QueryResult[]
                {
                    new CapitecOriginationCreditPricing_QueryResult()
                    {
                        InterestRate = (double)interestRate,
                        Instalment = (double)instalment,
                        LoanAmount = (double)loanAmount,
                        LoantoValue = (double)loanAmount,
                        PaymenttoIncome = (double)paymentToIncome,
                        EligibleApplication = true
                    }
                });
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(generatedID);
            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>()))
                    .Callback<CapitecOriginationCreditPricing_Query>(y => { y.Result = serviceQueryResult; });
        };

        private Because of = () =>
        {
            result = applicationService.RecalculateSwitchApplication(application, addedApplicants, true, applicationID);
        };

        private It should_query_the_credit_pricing_decision_tree_as_an_affordability_tree = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecOriginationCreditPricing_Query>.Matches(y =>
                y.FirstIncomeContributorApplicantEmpirica == -999 &&
                y.SecondIncomeContributorApplicantEmpirica == -999 &&
                y.FirstIncomeContributorApplicantIncome == -1 &&
                y.SecondIncomeContributorApplicantIncome == -1)));
        };

        private It should_save_the_tree_result = () =>
            {
                decisionTreeResultService.WasToldTo(x => x.SaveCreditPricingTreeResult(Param.IsAny<CapitecOriginationCreditPricing_Query>(), Param.IsAny<ISystemMessageCollection>(), applicationID));
            };
    }
}