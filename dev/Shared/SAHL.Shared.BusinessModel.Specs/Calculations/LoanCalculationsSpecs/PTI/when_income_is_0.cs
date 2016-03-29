using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.PTI
{
    public class when_income_is_0 : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal householdIncome;
        private static decimal pti;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            instalment = 0;
            householdIncome = 0;
        };

        private Because of = () =>
        {
            pti = functions.CalculatePTI(instalment, householdIncome);
        };

        private It should_calculate_pti_of_100_percent = () =>
        {
            pti.ShouldEqual(1m);
        };
    }
}