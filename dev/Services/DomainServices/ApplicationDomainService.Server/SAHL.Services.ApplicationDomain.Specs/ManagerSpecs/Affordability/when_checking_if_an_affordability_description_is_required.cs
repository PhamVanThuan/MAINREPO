using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Managers.Affordability.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Affordability
{
    public class when_checking_if_an_affordability_description_is_required : WithFakes
    {
        private static AffordabilityDataManager affordabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static AffordabilityTypeModel affordabilityModel;
        private static DoesAffordabilityRequireDescriptionStatement doesAffordabilityRequireDescriptionQuery;

        private Establish context = () =>
        {
            doesAffordabilityRequireDescriptionQuery
                = new DoesAffordabilityRequireDescriptionStatement(AffordabilityType.Creditlinerepayment);
            dbFactory = new FakeDbFactory();
            affordabilityDataManager = new AffordabilityDataManager(dbFactory);
            affordabilityModel = new AffordabilityTypeModel(AffordabilityType.BasicSalary, 300000, string.Empty);
        };

    
        private Because of = () =>
        {
            affordabilityDataManager.IsDescriptionRequired(AffordabilityType.Creditlinerepayment);
        };

        private It should_query_AffordabilityType_to_get_description_required_indicator = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<bool>(Param.IsAny<DoesAffordabilityRequireDescriptionStatement>()));
        };
    }
}
