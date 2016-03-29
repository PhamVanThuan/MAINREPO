using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicantOnActiveApplicationQueryStatement : IServiceQuerySqlStatement<GetApplicantOnActiveApplicationQuery, GetApplicantOnActiveApplicationQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 ofr.* from [FeTest].dbo.OpenNewBusinessApplications app
                        join FETest.dbo.ActiveNewBusinessApplicants aa on app.OfferKey = aa.OfferKey
                        join [2am].dbo.OfferRole ofr on aa.offerRoleKey = ofr.offerRoleKey
                        order by newid()";
        }
    }
}