using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetNaturalPersonClientQueryStatement : IServiceQuerySqlStatement<GetNaturalPersonClientQuery, LegalEntityDataModel>
    {
        public string GetStatement()
        {
            var query = @"select top 1 le.*
                            from [FETest].dbo.NaturalPersonClient npc
                            join [2am].dbo.LegalEntity le on npc.legalEntityKey = le.legalEntityKey
                            where npc.isActive = @IsActive
                            order by newid()";
            return query;
        }
    }
}