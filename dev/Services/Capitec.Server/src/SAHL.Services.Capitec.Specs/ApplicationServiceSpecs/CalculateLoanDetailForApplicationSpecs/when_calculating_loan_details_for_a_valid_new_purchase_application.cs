﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
    public class when_calculating_loan_details_for_a_valid_new_purchase_application : WithFakes
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
        private static Guid applicationId;
        private static ApplicantStubs stubs;
        private static NewPurchaseLoanDetails newPurchaseLoanDetails;
        private static decimal interestRate, instalment, loanAmount, loanToValue, paymentToIncome;
        private static string dtEmploymentTypeEnum;
        private static string dtOccupancyTypeEnum;
        private static string dtApplicationTypeEnum;
        private static IServiceQueryResult<CapitecOriginationCreditPricing_QueryResult> decisionTreeResult;
        private static bool eligibleBorrower;

        private Establish context = () =>
        {
            interestRate = 0.086M;
            instalment = 5000M;
            loanAmount = 300000M;
            loanToValue = 0.60M;
            paymentToIncome = 0.25M;
            dtEmploymentTypeEnum = new Enumerations.SAHomeLoans.HouseholdIncomeType().Salaried;
            dtOccupancyTypeEnum = new Enumerations.SAHomeLoans.PropertyOccupancyType().OwnerOccupied;
            dtApplicationTypeEnum = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().NewPurchase;
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
            newPurchaseLoanDetails = new NewPurchaseLoanDetails(Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), 20000M, 500000M, 200000M, 6000M, 120, false);
            applicationLoanDetails = new ApplicationLoanDetails(newPurchaseLoanDetails);
            var capitecOriginationCreditPricing_QueryResult = new CapitecOriginationCreditPricing_QueryResult[] { new CapitecOriginationCreditPricing_QueryResult(){
                     InterestRate = (double)interestRate,
                     Instalment = (double)instalment,
                     LoanAmount = (double)loanAmount,
                     LoantoValue = (double)loanToValue,
                     PaymenttoIncome = (double)paymentToIncome,
                     EligibleApplication = true
                } };
            decisionTreeResult = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(capitecOriginationCreditPricing_QueryResult);
            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Callback<CapitecOriginationCreditPricing_Query>(y =>
            {
                y.Result = decisionTreeResult;
            });
        };

        private Because of = () =>
        {
            calculationResults = applicationService.CalculateLoanDetailForApplication(applicationLoanDetails, applicants, eligibleBorrower, applicationId);
        };

        private It should_call_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>()));
        };

        private It should_use_the_correct_occupancy_type_enumeration_for_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(y => y.PropertyOccupancyType == dtOccupancyTypeEnum)));
        };

        private It should_use_the_correct_household_income_type_enumeration_for_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(y => y.HouseholdIncomeType == dtEmploymentTypeEnum)));
        };

        private It should_use_the_correct_application_type_enumeration_for_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(y => y.ApplicationType == dtApplicationTypeEnum)));
        };

        private It should_call_the_decision_tree_with_the_correct_application_details = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.PropertyPurchasePrice == (double)applicationLoanDetails.PurchasePrice &&
                y.DepositAmount == (double)applicationLoanDetails.Deposit &&
                y.Fees == (double)applicationLoanDetails.Fees &&
                y.HouseholdIncome == 20000D
                )));
        };

        private It should_always_send_interim_interest_of_zero_to_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
            y => y.InterimInterest == 0
            )));
        };

        private It should_always_send_capitalise_as_true_to_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
            y => y.CapitaliseFees == false
            )));
        };

        private It should_default_the_switch_loan_specific_parameters_to_minus_one = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
            y => y.CurrentMortgageLoanBalance == -1 && y.CashAmountRequired == -1 && y.EstimatedMarketValueofProperty == -1
            )));
        };

        private It should_return_decision_tree_results_in_the_calculated_loan_details_model = () =>
        {
            calculationResults.EligibleApplication.ShouldEqual(decisionTreeResult.Results.First().EligibleApplication);
            calculationResults.TermInMonths.ShouldEqual(240);
            ((double)calculationResults.InterestRate).ShouldEqual(decisionTreeResult.Results.First().InterestRate);
            ((double)calculationResults.LTV).ShouldEqual(decisionTreeResult.Results.First().LoantoValue);
            ((double)calculationResults.PTI).ShouldEqual(decisionTreeResult.Results.First().PaymenttoIncome);
            ((double)calculationResults.Instalment).ShouldEqual(decisionTreeResult.Results.First().Instalment);
            ((double)calculationResults.LoanAmount).ShouldEqual(decisionTreeResult.Results.First().LoanAmount);
        };
        private static IServiceCommandRouter serviceCommandRouter;
    }
}