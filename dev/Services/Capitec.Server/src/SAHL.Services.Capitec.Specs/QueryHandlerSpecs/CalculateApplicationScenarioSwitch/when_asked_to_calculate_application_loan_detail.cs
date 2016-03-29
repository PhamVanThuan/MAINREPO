using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.CalculateApplicationScenarioSwitchQueryHandlerSpecs
{
    public class when_asked_to_calculate_application_loan_detail : WithFakes
    {
        private static CalculateApplicationScenarioSwitchQuery query;
        private static CalculateApplicationScenarioQueryResult queryResult;
        private static CalculateApplicationScenarioSwitchQueryHandler handler;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static ILookupManager lookupService;
        private static SwitchLoanDetails loanDetails;

        private static Guid occupancyType, incomeType;
        private static decimal householdIncome, estimatedMarketValueOfTheHome, cashRequired, currentBalance, fees, interimInterest;

        private static decimal interestRate = 0.2m;
        private static decimal instalment = 5000;
        private static decimal loanAmount = 60000;
        private static decimal loanToValue = 0.22m;
        private static decimal paymentToIncome = 0.03m;
        private static string decisionTreeOccupancyType, decisionTreeHouseholdIncomeType, decisionTreeApplicationType;

        private Establish context = () =>
            {
                decisionTreeService = An<IDecisionTreeServiceClient>();
                lookupService = An<ILookupManager>();
                decisionTreeOccupancyType = new Enumerations.SAHomeLoans.PropertyOccupancyType().OwnerOccupied;
                decisionTreeHouseholdIncomeType = new Enumerations.SAHomeLoans.HouseholdIncomeType().Salaried;
                decisionTreeApplicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch;
                lookupService.WhenToldTo(x => x.GetDecisionTreeOccupancyType(Param.IsAny<Guid>())).Return(decisionTreeOccupancyType);
                lookupService.WhenToldTo(x => x.GetDecisionTreeHouseholdIncomeType(Param.IsAny<Guid>())).Return(decisionTreeHouseholdIncomeType);
                occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
                incomeType = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
                householdIncome = 30000;
                estimatedMarketValueOfTheHome = 800000;
                cashRequired = 50000;
                currentBalance = 400000;
                fees = Param<decimal>.IsAnything;

                loanDetails = new SwitchLoanDetails(occupancyType, incomeType, householdIncome, estimatedMarketValueOfTheHome, cashRequired, currentBalance, fees, interimInterest, 240, true);

                var serviceQueryResult = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(new CapitecOriginationCreditPricing_QueryResult[]
                {
                    new CapitecOriginationCreditPricing_QueryResult()
                    {
                        InterestRate = (double)interestRate,
                        Instalment = (double)instalment,
                        LoanAmount = (double)loanAmount,
                        LoantoValue = (double)loanToValue,
                        PaymenttoIncome = (double)paymentToIncome,
                        EligibleApplication = true
                    }
                });

                decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Callback<CapitecOriginationCreditPricing_Query>(y =>
                {
                    y.Result = serviceQueryResult;
                });

                query = new CalculateApplicationScenarioSwitchQuery(loanDetails);
                handler = new CalculateApplicationScenarioSwitchQueryHandler(decisionTreeService, lookupService);
            };

        private Because of = () =>
            {
                handler.HandleQuery(query);
                queryResult = query.Result;
            };

        private It should_default_the_application_empirica_values_to_minus_999 = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.SecondIncomeContributorApplicantEmpirica == -999 &&
                    y.FirstIncomeContributorApplicantEmpirica == -999)));
        };

        private It should_default_the_eligible_borrower_parameter_to_true = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.EligibleBorrower == true)));
        };

        private It should_default_the_oldest_and_youngest_applicant_age_values_to_minus_one = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.EldestApplicantAge == -1 &&
                    y.YoungestApplicantAge == -1)));
        };

        private It should_have_looked_up_the_appropriate_employment_type_for_the_decision_tree_query = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.HouseholdIncomeType == decisionTreeHouseholdIncomeType)));
        };

        private It should_have_looked_up_the_appropriate_occupancy_type_for_the_decision_tree_query = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
             y => y.PropertyOccupancyType == decisionTreeOccupancyType)));
        };

        private It should_default_the_applicant_income_values_to_minus_one = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.FirstIncomeContributorApplicantIncome == -1 &&
                y.SecondIncomeContributorApplicantIncome == -1)));
        };

        private It should_set_the_correct_application_details = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Arg.Is<CapitecOriginationCreditPricing_Query>(
                y => y.CashAmountRequired == (double)loanDetails.CashRequired &&
                y.CurrentMortgageLoanBalance == (double)loanDetails.CurrentBalance &&
                y.HouseholdIncome == (double)loanDetails.HouseholdIncome &&
                y.ApplicationType == decisionTreeApplicationType &&
                y.CapitaliseFees == true)));
        };

        private It should_return_expected_calculated_results = () =>
        {
            queryResult.InterestRate.ShouldEqual(interestRate);
            queryResult.Instalment.ShouldEqual(instalment);
            queryResult.LoanAmount.ShouldEqual(loanAmount);
            queryResult.LTV.ShouldEqual(loanToValue);
            queryResult.PTI.ShouldEqual(paymentToIncome);
        };
    }
}