using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientByIdNumberQueryStatement : IServiceQuerySqlStatement<GetClientByIdNumberQuery, LegalEntityDataModel>
    {
        public string GetStatement()
        {
            return @"select * from [2am].dbo.LegalEntity where IdNumber = @IdNumber";
        }
    }
}