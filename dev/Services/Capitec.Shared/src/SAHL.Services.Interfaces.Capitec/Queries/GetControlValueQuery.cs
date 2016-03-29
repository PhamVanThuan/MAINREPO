using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetControlValueQuery : ServiceQuery<GetControlValueQueryResult>, ISqlServiceQuery<GetControlValueQueryResult>
    {
        public GetControlValueQuery(string controlDescription)
        {
            this.ControlDescription = controlDescription;
        }

        public string ControlDescription { get; protected set; }
    }
}