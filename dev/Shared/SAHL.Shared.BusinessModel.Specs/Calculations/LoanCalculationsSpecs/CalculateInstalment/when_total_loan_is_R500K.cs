using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalment
{
    public class when_total_loan_is_R500K : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal totalLoanValue = 500000;
        private static decimal annualInterestRate = 0.1m;
        private static decimal remainingTerm = 240;
        private static bool interestOnly = false;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 500000;
            annualInterestRate = 0.1m;
            remainingTerm = 240;
            interestOnly = false;
        };

        private Because of = () =>
        {
            instalment = functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
        };

        private It should_calculate_the_instalment = () =>
        {
            instalment.ShouldEqual(4825.1082253700445068166514000m);
        };
    }
}