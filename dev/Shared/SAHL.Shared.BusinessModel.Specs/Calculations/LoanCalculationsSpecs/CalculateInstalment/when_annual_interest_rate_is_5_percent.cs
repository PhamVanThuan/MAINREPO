using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalment
{
    public class when_annual_interest_rate_is_5_percent : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal totalLoanValue = 200000;
        private static decimal annualInterestRate = 0.05m;
        private static decimal remainingTerm = 240;
        private static bool interestOnly = false;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 200000;
            annualInterestRate = 0.05m;
            remainingTerm = 240;
            interestOnly = false;
        };

        private Because of = () =>
        {
            instalment = functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
        };

        private It should_calculate_the_instalment = () =>
        {
            instalment.ShouldEqual(1319.9114784333170892196969400m);
        };
    }
}