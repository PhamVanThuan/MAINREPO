using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Specs.Repositories.Account.ClosePersonalLoanAccount
{
    internal class when_HaloApi_ClosePersonalLoanAccount_called_with_Correct_Parameters : WithFakes
    {
        static IAccountRepository accountRepository;
        static ICastleTransactionsService castleTransactionService;
        static ParameterCollection parameters;
        static int accountKey;
        static string userId;
        static string applicationName;
        static string statementName;

        Establish context = () =>
        {
            accountKey = 3238604;
            userId = "userId";

            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            parameters.Add(new SqlParameter("@UID", userId));

            applicationName = "Repositories.AccountRepository";
            statementName = "ClosePersonalLoanAccount";

            castleTransactionService = An<ICastleTransactionsService>();

            accountRepository = new AccountRepository(castleTransactionService);
        };

        Because of_personal_loan_account_close_called = () =>
        {
            accountRepository.ClosePersonalLoanAccount(accountKey, userId);
        };

        It should_have_correct_parameters = () =>
        {
            castleTransactionService.WasToldTo(x => x.ExecuteHaloAPI_uiS_OnCastleTranForUpdate(Arg.Is<string>(applicationName),
                                                                                Arg.Is<string>(statementName),
                                                                                Arg<ParameterCollection>.Matches(y => y.SqlParametersCompare(parameters))));
        };
    }
}