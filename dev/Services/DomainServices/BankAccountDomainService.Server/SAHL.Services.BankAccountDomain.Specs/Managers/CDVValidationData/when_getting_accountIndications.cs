using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Data;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using NSubstitute;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.CDVValidationData
{
    internal class when_getting_accountIndications : WithFakes
    {   
        private static CDVDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<AccountIndicationDataModel> result;
        private static IEnumerable<AccountIndicationDataModel> dataModels;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new CDVDataManager(dbFactory);
            dataModels = new List<AccountIndicationDataModel> { new AccountIndicationDataModel(12, 12, "", "RichieRich", DateTime.Now) }; 
            
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<AccountIndicationDataModel>(Param.IsAny<ISqlStatement<AccountIndicationDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            result = dataManager.GetAccountIndications();
        };

        private It should_select_all_accountIndications_with_no_params = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Any<SelectAccountIndicationsStatement>()));
        };
        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldBeTheSameAs(dataModels);            
        };
    }
}