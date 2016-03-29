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
    internal class when_getting_cdv_exceptions_for_cdv : WithFakes
    {
        private static int bankCode = 12;
        private static string cdvExceptionCode = "F";
        private static int accountType = 1;     
        private static CDVDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<CDVExceptionsDataModel> result;
        private static IEnumerable<CDVExceptionsDataModel> dataModels;
        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new CDVDataManager(dbFactory);
            dataModels = new List<CDVExceptionsDataModel> { new CDVExceptionsDataModel(12,12,1,3,"123234343",null,null,"F","RichieRich",DateTime.Now) }; 

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<CDVExceptionsDataModel>(Param.IsAny<ISqlStatement<CDVExceptionsDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            result = dataManager.GetCDVExceptions(bankCode,cdvExceptionCode,accountType);
        };

        private It should_select_using_the_branch_bank_type_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(
                x => x.Select(Arg.Is<SelectCDVExceptionsForCDVStatement>(
                        y => y.ACBBankCode == bankCode 
                          && y.ACBTypeNumber == accountType 
                          && y.ExceptionCode == cdvExceptionCode
            )));
        };
        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldBeTheSameAs(dataModels);            
        };
    }
}