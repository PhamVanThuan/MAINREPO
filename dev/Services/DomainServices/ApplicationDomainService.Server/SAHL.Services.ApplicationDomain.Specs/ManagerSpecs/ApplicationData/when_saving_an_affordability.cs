using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_affordability : WithFakes
    {
        private static IAffordabilityDataManager affordabilityDataManager;
        private static FakeDbFactory dbFactory;
        private static AffordabilityTypeModel affordabilityModel;
        private static LegalEntityAffordabilityDataModel clientAffordabilityDataModel;
        private static int clientKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            clientKey = 2323;
            applicationNumber = 34322;
            dbFactory = new FakeDbFactory();
            affordabilityDataManager = new AffordabilityDataManager(dbFactory);
            affordabilityModel = new AffordabilityTypeModel(AffordabilityType.BasicSalary, 200000, "");
            clientAffordabilityDataModel = new LegalEntityAffordabilityDataModel(clientKey, (int)affordabilityModel.AffordabilityType,
                affordabilityModel.Amount, affordabilityModel.Description, applicationNumber);
        };

        private Because of = () =>
        {
            affordabilityDataManager.SaveAffordability(affordabilityModel, clientKey, applicationNumber);
        };

        private It should_insert_the_client_affordability = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<LegalEntityAffordabilityDataModel>(Arg.Is<LegalEntityAffordabilityDataModel>(y => y.Amount == clientAffordabilityDataModel.Amount)));
        };
    }
}