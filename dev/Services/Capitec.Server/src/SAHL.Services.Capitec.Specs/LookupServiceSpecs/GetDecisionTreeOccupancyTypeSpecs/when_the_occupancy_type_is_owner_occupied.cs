using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs.GetDecisionTreeOccupancyTypeSpecs
{
    public class when_the_occupancy_type_is_owner_occupied : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid occupancyType;
        private static string result;    
        private static string expectedDecisionTreeType;

        private Establish context = () =>
        {
            expectedDecisionTreeType = new Enumerations.SAHomeLoans.PropertyOccupancyType().OwnerOccupied;
            occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
            dbFactory = new FakeDbFactory();
            lookupService = new LookupManager(dbFactory);
        };

        private Because of = () =>
        {
            result = lookupService.GetDecisionTreeOccupancyType(occupancyType);
        };

        private It should_return_investment_property = () =>
        {
            result.ShouldEqual(expectedDecisionTreeType);
        };
    }
}