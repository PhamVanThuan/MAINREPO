using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs.GetDecisionTreeHouseholdIncomeTypeSpecs
{
    public class when_the_household_income_type_is_unemployed : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid employmentType;
        private static string expectedDecisionTreeType;
        private static string result;

        private Establish context = () =>
        {
            employmentType = Guid.Parse(EmploymentTypeEnumDataModel.UNEMPLOYED);
            dbFactory = new FakeDbFactory();
            lookupService = new LookupManager(dbFactory);
            expectedDecisionTreeType = new Enumerations.SAHomeLoans.HouseholdIncomeType().Unemployed;
        };

        private Because of = () =>
        {
            result = lookupService.GetDecisionTreeHouseholdIncomeType(employmentType);
        };

        private It should_return_the_unemployed_type_for_the_decision_tree = () =>
        {
            result.ShouldEqual(expectedDecisionTreeType);
        };
    }
}