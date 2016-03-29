using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.LTV
{
    public class when_property_value_is_0 : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal propertyValue;
        private static decimal totalLoanAmount;
        private static decimal ltv;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            propertyValue = 0;
            totalLoanAmount = 50;
        };

        private Because of = () =>
        {
            ltv = functions.CalculateLTV(totalLoanAmount, propertyValue);
        };

        private It should_return_a_high_ltv_equal_to_the_loan_amount = () =>
        {
            ltv.ShouldEqual(totalLoanAmount);
        };
    }
}