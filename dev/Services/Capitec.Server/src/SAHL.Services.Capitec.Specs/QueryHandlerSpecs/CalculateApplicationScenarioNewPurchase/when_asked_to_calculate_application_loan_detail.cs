using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
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

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.CalculateApplicationScenarioNewPurchaseQueryHandlerSpecs
{
    public class when_asked_to_calculate_application_loan_detail : WithFakes
    {
        private static CalculateApplicationScenarioNewPurchaseQuery query;
        private static CalculateApplicationScenarioQueryResult queryResult;
        private static CalculateApplicationScenarioNewPurchaseQueryHandler handler;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static ILookupManager lookupService;
        private static NewPurchaseLoanDetails loanDetails;
        private static Guid occupancyType;
        private static Guid incomeType;
        private static decimal householdIncome;
        private static decimal purchasePrice;
        private static decimal deposit;
        private static decimal fees;
        private static decimal interestRate = 0.2m;
        private static decimal instalment = 5000;
        private static decimal loanAmount = 60000;
        private static decimal loanToValue = 0.22m;
        private static decimal paymentToIncome = 0.03m;
        private static string decisionTreeHouseholdIncomeType;
        private static string decisionTreeOccupancyType;
        private static string decisionTreeApplicationType;

        private Establish context = () =>
            {
                decisionTreeHouseholdIncomeType = new Enumerations.SAHomeLoans.HouseholdIncomeType().Salaried;
                decisionTreeOccupancyType = new Enumerations.SAHomeLoans.PropertyOccupancyType().OwnerOccupied;
                decisionTreeApplicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().NewPurchase;
                decisionTreeService = An<IDecisionTreeServiceClient>();
                lookupService = An<ILookupManager>();
                lookupService.WhenToldTo(x => x.GetDecisionTreeHouseholdIncomeType(Param.IsAny<Guid>())).Return(decisionTreeHouseholdIncomeType);
                lookupService.WhenToldTo(x => x.GetDecisionTreeOccupancyType(Param.IsAny<Guid>())).Return(decisionTreeOccupancyType);
                occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
                incomeType = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
                householdIncome = 30000;
                purchasePrice = 800000;
                deposit = 90000;
                fees = Param<decimal>.IsAnything;

                loanDetails = new NewPurchaseLoanDetails(occupancyType, incomeType, householdIncome, purchasePrice, deposit, fees, 240, false);

                query = new CalculateApplicationScenarioNewPurchaseQuery(loanDetails);
                handler = new CalculateApplicationScenarioNewPurchaseQueryHandler(decisionTreeService, lookupService);
                var capitecOriginationCreditPricing_QueryResult = new CapitecOriginationCreditPricing_QueryResult[] { new CapitecOriginationCreditPricing_QueryResult(){
                     InterestRate = (double)interestRate,
                     Instalment = (double)instalment,
                     LoanAmount = (double)loanAmount,
                     LoantoValue = (double)loanToValue,
                     PaymenttoIncome = (double)paymentToIncome,
                     EligibleApplication = true
                } };
                IServiceQueryResult<CapitecOriginationCreditPricing_QueryResult> result = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(capitecOriginationCreditPricing_QueryResult);

                ISystemMessageCollection messages = An<ISystemMessageCollection>();

                decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Callback<CapitecOriginationCreditPricing_Query>(y =>
                {
                    y.Result = result;
                });
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
                y => y.PropertyPurchasePrice == (double)loanDetails.PurchasePrice &&
                y.DepositAmount == (double)loanDetails.Deposit &&
                y.HouseholdIncome == (double)loanDetails.HouseholdIncome &&
                y.ApplicationType == decisionTreeApplicationType)));
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