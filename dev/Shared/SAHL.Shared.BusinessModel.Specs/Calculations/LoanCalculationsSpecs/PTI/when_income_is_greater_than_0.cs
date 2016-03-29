using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Shared.BusinessModel.Specs.Calculations.LoanCalculationsSpecs.PTI
{
    public class when_income_is_greater_than_0 : WithFakes
    {
        private static ILoanCalculations functions;
        private static decimal instalment;
        private static decimal householdIncome;
        private static decimal pti;

        private Establish context = () =>
        {
            functions = new LoanCalculations();
            instalment = 20;
            householdIncome = 100;
        };

        private Because of = () =>
        {
            pti = functions.CalculatePTI(instalment, householdIncome);
        };

        private It should_calculate_pti_as_instalment_divided_by_income = () =>
        {
            pti.ShouldEqual(instalment / householdIncome);
        };
    }
}