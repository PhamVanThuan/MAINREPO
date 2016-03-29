using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using System.Data;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.FurtherLending.AccountDetailTypeCheck
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeCheck))]
    public class when_the_account_does_not_contain_any_of_the_detail_types : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeCheck>
    {
        protected static IAccount account;
        protected static ICastleTransactionsService castleTransactionService;
        protected static IUIStatementService uistatementService;
        Establish context = () =>
        {
            account = An<IAccount>();
            castleTransactionService = An<ICastleTransactionsService>();
            uistatementService = An<IUIStatementService>();
            DataSet dataSet = new DataSet();

            uistatementService.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("some sql");

            account.WhenToldTo(x => x.Key).Return(1);
            castleTransactionService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<DataAccess.ParameterCollection>())).Return(dataSet);

            businessRule = new SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeCheck(castleTransactionService, uistatementService);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeCheck>.startrule.Invoke();
        };

        Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, account);
        };

        It should_fail_the_rule = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}