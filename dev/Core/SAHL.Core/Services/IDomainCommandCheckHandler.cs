using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IDomainCommandCheckHandler<TCommandCheck> where TCommandCheck : IDomainCommandCheck
    {
        ISystemMessageCollection HandleCheckCommand(TCommandCheck command);
    }
}