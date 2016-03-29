using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetSuburbsQuery : ServiceQuery<GetSuburbsQueryResult>, ISqlServiceQuery<GetSuburbsQueryResult>
    {
    }
}