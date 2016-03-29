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
    internal class when_getting_a_bank_for_a_branch : WithFakes
    {    
        private static string branchCode = "100943";      
        private static CDVDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<ACBBankDataModel> result;
        private static IEnumerable<ACBBankDataModel> dataModels;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new CDVDataManager(dbFactory);  
            dataModels = new List<ACBBankDataModel>{new ACBBankDataModel(12, "Nedbank")}; 
            
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<ACBBankDataModel>(Param.IsAny<ISqlStatement<ACBBankDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            result = dataManager.GetBankForACBBranch(branchCode);
        };

        private It should_select_using_the_branch_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Is<SelectACBBankForACBBranchStatement>(y => y.ACBBranchCode == branchCode)));
        };
        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldBeTheSameAs(dataModels);            
        };
    }
}