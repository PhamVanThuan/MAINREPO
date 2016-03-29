using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicantWithApplicationCriteriaQueryStatement : IServiceQuerySqlStatement<GetApplicantWithApplicationCriteriaQuery, GetApplicantWithApplicationCriteriaQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 ofr.* from [fetest].[dbo].OpenNewBusinessApplications a
                     join [fetest].dbo.ActiveNewBusinessApplicants app on a.OfferKey = app.OfferKey
                     join [2am].dbo.OfferRole ofr on app.OfferRoleKey = ofr.OfferRoleKey
                     where HasResidentialAddress = @ApplicantHasAddress 
                     and HasMailingAddress = @ApplicationHasMailingAddress order by newid()";
        }
    }
}