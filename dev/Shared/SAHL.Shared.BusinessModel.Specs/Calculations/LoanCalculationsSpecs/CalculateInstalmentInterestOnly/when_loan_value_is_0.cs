using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalmentInterestOnly
{
    public class when_loan_value_is_0 : WithFakes
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
            totalLoanValue = 0;
            annualInterestRate = 0;
            remainingTerm = 0;
            interestOnly = true;
        };

        private Because of = () =>
        {
            instalment = functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
        };

        private It should_return_0 = () =>
        {
            instalment.ShouldEqual(0);
        };
    }
}