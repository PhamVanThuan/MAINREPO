using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;
using System;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.CalculateInstalment
{
    public class when_annual_interest_rate_is_0_percent : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal totalLoanValue;
        private static decimal annualInterestRate;
        private static decimal remainingTerm;
        private static bool interestOnly;

        private static Exception ex;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            totalLoanValue = 1;
            annualInterestRate = 0;
            remainingTerm = 1;
            interestOnly = false;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                functions.CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm, interestOnly);
            });
        };

        private It should_throw_an_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(Exception));
        };
    }
}