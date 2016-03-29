using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
	public class GetApplicationByUserQuery : ServiceQuery<GetApplicationQueryResult>, ISqlServiceQuery<GetApplicationQueryResult>
    {
    	public string UserName { get; protected set; }

        public GetApplicationByUserQuery(string userName)
        {
            this.UserName = userName;
        }
    }
}