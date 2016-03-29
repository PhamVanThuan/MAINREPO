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
    internal class when_getting_accounttyperecognitions : WithFakes
    {
        private static int bankCode = 12;       
        private static int accountType = 1;     
        private static CDVDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<AccountTypeRecognitionDataModel> result;
        private static IEnumerable<AccountTypeRecognitionDataModel> dataModels;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new CDVDataManager(dbFactory);
            dataModels = new List<AccountTypeRecognitionDataModel> 
                             { 
                                 new AccountTypeRecognitionDataModel(12, 12, 1, null, null, null, null, null, null, null, null, "Y", null,null, "RichieRich", DateTime.Now) 
                             };

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<AccountTypeRecognitionDataModel>(Param.IsAny<ISqlStatement<AccountTypeRecognitionDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            result = dataManager.GetAccountTypeRecognitions(bankCode,accountType);
        };

        private It should_select_using_the_bankCode_and_accounttype_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Is<SelectAccountTypeRecognitionForACBBankStatement>(y => y.ACBBankCode == bankCode && y.ACBTypeNumber == accountType)));
        };
        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldBeTheSameAs(dataModels);            
        };
    }
}