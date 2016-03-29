using SAHL.Core.Services.CommandPersistence;

namespace SAHL.Core.Web.Services
{
    public interface IHttpCommandReRun
    {
        ServiceCommandResult TryRunCommand(ICommandSession commandSession);
    }
}