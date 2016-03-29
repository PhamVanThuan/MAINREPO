using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetApplicationByUserQueryStatement : IServiceQuerySqlStatement<GetApplicationByUserQuery, GetApplicationQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 app.Id, app.ApplicationDate, app.ApplicationNumber, app.ApplicationPurposeEnumId from [Capitec].dbo.[Application] app
join [Capitec].[security].[User] usr on usr.Id = app.UserId
where
    usr.UserName = @UserName
order by app.ApplicationDate desc";
        }
    }
}