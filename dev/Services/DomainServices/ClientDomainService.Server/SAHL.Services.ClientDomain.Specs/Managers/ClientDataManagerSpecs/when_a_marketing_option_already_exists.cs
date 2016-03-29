using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using System;
using SAHL.Services.ClientDomain.Managers.Client.Statements;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_a_marketing_option_already_exists : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static LegalEntityMarketingOptionDataModel legalEntityMarketingOptionDataModel;
        private static int clientKey, marketingOptionKey;      
        private static FakeDbFactory fakedDb;
        private static bool result;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            clientDataManager = An<IClientDataManager>();
            clientKey = 77;
            marketingOptionKey = 8;
            legalEntityMarketingOptionDataModel = new LegalEntityMarketingOptionDataModel(clientKey,8,DateTime.Now,"x2");
          
            fakedDb.FakedDb.InReadOnlyAppContext().WhenToldTo(
                x => x.SelectOne(Param.IsAny<DoesClientMarketingOptionExistStatement>())).Return(0);
        };

        private Because of = () =>
        {
            result = clientDataManager.DoesClientMarketingOptionExist(clientKey, marketingOptionKey);
        };

        private It should_not_save_the_marketing_option = () =>
        {
            fakedDb.FakedDb.InAppContext().WasNotToldTo(x => x.Insert<LegalEntityMarketingOptionDataModel>(legalEntityMarketingOptionDataModel));  
        };
    }
}