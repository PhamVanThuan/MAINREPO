using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_adding_marketing_options : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static LegalEntityMarketingOptionDataModel legalEntityMarketingOptionDataModel;
        private static int clientKey;      
        private static FakeDbFactory fakedDb;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakedDb);
            clientKey = 77;
            legalEntityMarketingOptionDataModel = new LegalEntityMarketingOptionDataModel(clientKey,8,DateTime.Now,"x2");            
        };

        private Because of = () =>
        {
            clientDataManager.AddNewMarketingOptions(legalEntityMarketingOptionDataModel);
        };

        private It should_save_the_marketing_option = () =>
        {
            fakedDb.FakedDb.InAppContext().WasToldTo(x => x.Insert<LegalEntityMarketingOptionDataModel>(legalEntityMarketingOptionDataModel));
        };
    }
}