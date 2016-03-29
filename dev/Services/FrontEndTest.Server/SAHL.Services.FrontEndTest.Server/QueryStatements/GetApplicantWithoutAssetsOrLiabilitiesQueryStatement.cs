using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicantWithoutAssetsOrLiabilitiesQueryStatement : IServiceQuerySqlStatement<GetApplicantWithoutAssetsOrLiabilitiesQuery, GetApplicantWithoutAssetsOrLiabilitiesQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select top 1 ofr.*
                from [FeTest].dbo.ActiveNewBusinessApplicants a
                join [FeTest].dbo.ClientAddresses ca on a.LegalEntityKey = ca.legalEntityKey
                    and ca.AddressTypeKey = 1
                join [2am].dbo.OfferRole ofr on a.offerRoleKey = ofr.offerRoleKey
                where a.HasAssetsLiabilities = 0
                order by newid()";
            return query;
        }
    }
}