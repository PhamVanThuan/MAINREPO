using SAHL.Core.Identity;
using SAHL.Core.Services.CommandPersistence;
using System.Net;

namespace SAHL.Core.Web.Services
{
    public interface IHttpCommandRun
    {
        HttpStatusCode RunCommand(ServiceCommandResult result, ICommandSession commandSession);
    }
}