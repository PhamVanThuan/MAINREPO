using SAHL.Core.Services;

namespace SAHL.Core.Web.Services
{
    public interface IHttpCommandAuthoriser
    {
        SAHL.Core.Web.Services.HttpCommandAuthoriser.AuthToken AuthoriseCommand(IServiceCommand command);
    }
}