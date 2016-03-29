using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalmentInterestOnly
{
    public class when_annual_interest_rate_is_7_percent : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal totalLoanValue;
        private static decimal annualInterestRate;
        private static decimal remainingTerm;
        private static bool interestOnly;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 200000;
            annualInterestRate = 0.07m;
            remainingTerm = 240;
            interestOnly = true;
        };

        private Because of = () =>
        {
            instalment = functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
        };

        private It should_calculate_the_instalment = () =>
        {
            instalment.ShouldEqual(1189.0410958904109589041096400m);
        };
    }
}