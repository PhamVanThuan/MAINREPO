using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetControlValueQueryStatement : IServiceQuerySqlStatement<GetControlValueQuery, GetControlValueQueryResult>
    {
        public string GetStatement()
        {
            return "SELECT ControlDescription, ControlNumeric, ControlText FROM [Capitec].[dbo].[Control] (nolock) where ControlDescription = @ControlDescription";
        }
    }
}