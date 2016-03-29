using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalmentInterestOnly
{
    public class when_calculating_different_terms : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment1term;
        private static decimal instalment10term;
        private static decimal instalment70term;
        private static decimal instalment150term;
        private static decimal instalment240term;
        private static decimal instalment276term;
        private static decimal totalLoanValue;
        private static decimal annualInterestRate;
        private static bool interestOnly;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 150;
            annualInterestRate = 1;
            interestOnly = true;
        };

        private Because of = () =>
        {
            instalment1term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
            instalment10term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
            instalment70term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
            instalment150term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
            instalment240term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
            instalment276term = functions.CalculateInstalment(totalLoanValue, annualInterestRate, 1, interestOnly);
        };

        private It should_always_return_the_same_calculated_interest_only_instalment = () =>
        {
            instalment1term.ShouldEqual(instalment10term.ShouldEqual(instalment70term.ShouldEqual(instalment150term)));
            instalment150term.ShouldEqual(instalment240term.ShouldEqual(instalment276term));
        };
    }
}