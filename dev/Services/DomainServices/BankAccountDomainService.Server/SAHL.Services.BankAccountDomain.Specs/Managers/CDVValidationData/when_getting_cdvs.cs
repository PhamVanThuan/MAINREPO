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
    internal class when_getting_cdvs : WithFakes
    {
        private static string branchCode = "50042";
        private static int bankCode = 12;       
        private static int accountType = 1;     
        private static CDVDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<CDVDataModel> result;
        private static IEnumerable<CDVDataModel> dataModels;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new CDVDataManager(dbFactory);
            dataModels = new List<CDVDataModel> { new CDVDataModel(12, 12, "", 34, null, null, "", null, null, "F", null, "RichieRich", DateTime.Now) }; 
            
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<CDVDataModel>(Param.IsAny<ISqlStatement<CDVDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            result = dataManager.GetCDVs(bankCode, branchCode, accountType);
        };

        private It should_select_using_the_branch__bank_and_type_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(
                x => x.Select(Arg.Is<SelectCDVForACBBankStatement>(
                         y => y.ACBBankCode == bankCode 
                           && y.ACBTypeNumber == accountType 
                           && y.ACBBranchCode == branchCode
              )));
        };
        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldBeTheSameAs(dataModels);            
        };
    }
}