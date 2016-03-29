﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
	public class GetOccupancyTypesQueryStatement : IServiceQuerySqlStatement<GetOccupancyTypesQuery, GetOccupancyTypesResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[Name]
FROM [Capitec].[dbo].[OccupancyTypeEnum] where IsActive = 1";
        }
    }
}