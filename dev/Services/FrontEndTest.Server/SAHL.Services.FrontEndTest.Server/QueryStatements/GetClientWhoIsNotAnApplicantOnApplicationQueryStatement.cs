using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientWhoIsNotAnApplicantOnApplicationQueryStatement : IServiceQuerySqlStatement<GetClientWhoIsNotAnApplicantOnApplicationQuery, LegalEntityDataModel>
    {
        public string GetStatement()
        {
            string query = @"select top 1 * 
                            from  [2am].dbo.LegalEntity
                            where LegalEntityTypeKey = 2 and LegalEntityStatusKey = 1 and IDNumber is not null
                            and DateDiff(yy, DateOfBirth, GetDate()) between 18 and 65
                            and LegalEntityKey not in (
                                select le.LegalEntityKey 
                                        from [2am].dbo.LegalEntity le
                                        join [2am].dbo.OfferRole r on le.LegalEntityKey=r.LegalEntityKey
                                        where r.OfferKey = @ApplicationNumber 
                                        and r.OfferRoleTypeKey in (8,11))
                            order by newid()";
            return query;
        }
    }
}