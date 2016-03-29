using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetNewBusinessApplicantQueryStatement : IServiceQuerySqlStatement<GetNewBusinessApplicantQuery, GetNewBusinessApplicantQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 ofr.*
                    from [FETest].dbo.ActiveNewBusinessApplicants a
                    join [2am].dbo.OfferRole ofr on a.offerRoleKey = ofr.OfferRoleKey
                    where a.hasAffordabilityAssessment = @HasAffordabilityAssessment
                    and a.IsIncomeContributor = @IsIncomeContributor
                    and a.HasDeclarations = @HasDeclarations
                    and a.HasAssetsLiabilities = @HasAssetsLiabilities
                    and a.HasBankAccount = @HasBankAccount
                    and a.HasEmployment = @HasEmployment
                    and a.HasDomicilium = @HasDomicilium
                    and a.HasResidentialAddress = @HasResidentialAddress
                    and a.HasPostalAddress = @HasPostalAddress
                    order by newid()";
        }
    }
}