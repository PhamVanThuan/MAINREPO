using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetImageIndexDataQueryStatement : IServiceQuerySqlStatement<GetImageIndexDataQuery, GetImageIndexDataQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 cast(ID as int) as ID, cast(STOR as INT) as STOR, CAST(GUID AS varchar(38)) as GUID
                    FROM 
                        [ImageIndex].[dbo].[Data] 
                    WHERE 
                        STOR = 44
					order by NEWID()";
        }
    }
}
