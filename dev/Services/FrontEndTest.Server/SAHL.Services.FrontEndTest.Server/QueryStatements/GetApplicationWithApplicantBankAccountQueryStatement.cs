using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicationWithApplicantBankAccountQueryStatement : IServiceQuerySqlStatement<GetApplicationWithApplicantBankAccountQuery, GetApplicationWithApplicantBankAccountQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select top 1 o.OfferKey as ApplicationNumber, 1 as DebitOrderDay, leba.LegalEntityBankAccountKey as ClientBankAccountKey
                            from [FeTest].dbo.OpenNewBusinessApplications o
                            join [FeTest].dbo.ActiveNewBusinessApplicants app on o.OfferKey = app.OfferKey
                            join [2am].dbo.LegalEntityBankAccount leba on app.LegalEntityKey = leba.LegalEntityKey
                            where hasDebitOrder = @HasDebitOrder and app.hasBankAccount = 1
                            order by newid()";
            return query;
        }
    }
}