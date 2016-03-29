using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicantRolesByIdNumberQueryStatement : IServiceQuerySqlStatement<GetApplicantRolesByIdNumberQuery, OfferRoleDataModel>
    {
        public string GetStatement()
        {
            return @"select o.* from [2am].dbo.OfferRole o
                    join [2am].dbo.LegalEntity l on o.legalEntityKey = l.legalEntityKey
                    where l.IdNumber = @IdNumber";
        }
    }
}