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

namespace SAHL.Common.BusinessModel.Specs.Repositories.PersonalLoan.ChangeTerm
{
    public class when_should_ChangeTerm : WithFakes
    {
        static IPersonalLoanRepository personalLoanRepository;
        static ICastleTransactionsService castleTransactionService;
        static ParameterCollection parameters;
        static int accountKey;
        static int remainingInstalments;
        static string userId;
        static string applicationName;
        static string statementName;

        Establish context = () =>
        {
            accountKey = 3238604;
            userId = "userId";
            remainingInstalments = 5;
            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@RemainingInstalments", remainingInstalments));
            parameters.Add(new SqlParameter("@UserID", userId));

            applicationName = "Repositories.PersonalLoanRepository";
            statementName = "ChangeTerm";

            castleTransactionService = An<ICastleTransactionsService>();

            personalLoanRepository = new PersonalLoanRepository(castleTransactionService);
        };

        Because of_personal_loan_account_close_called = () =>
        {
            personalLoanRepository.ChangeTerm(accountKey, remainingInstalments, userId);
        };

        It should_have_correct_parameters = () =>
        {
            castleTransactionService.WasToldTo(x => x.ExecuteHaloAPI_uiS_OnCastleTranForUpdate(Arg.Is<string>(applicationName),
                                                                                Arg.Is<string>(statementName),
                                                                                Arg<ParameterCollection>.Matches(y => y.SqlParametersCompare(parameters))));
        };
    }
}
