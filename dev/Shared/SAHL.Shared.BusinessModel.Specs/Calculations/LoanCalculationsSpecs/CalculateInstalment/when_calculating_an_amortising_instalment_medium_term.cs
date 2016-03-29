using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalment
{
    public class when_calculating_an_amortising_instalment_medium_term : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal totalLoanValue = 200000;
        private static decimal annualInterestRate = 0.1m;
        private static decimal remainingTerm = 100;
        private static bool interestOnly = false;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 200000;
            annualInterestRate = 0.1m;
            remainingTerm = 100;
            interestOnly = false;
        };

        private Because of = () =>
        {
            instalment = functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
        };

        private It should_calculate_the_instalment = () =>
        {
            instalment.ShouldEqual(2955.6146094958676602960491400m);
        };
    }
}