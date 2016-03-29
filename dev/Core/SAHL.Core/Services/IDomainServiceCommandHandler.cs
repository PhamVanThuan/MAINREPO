using SAHL.Core.Events;

namespace SAHL.Core.Services
{
    public interface IDomainServiceCommandHandler
    {
    }

    public interface IDomainServiceCommandHandler<T, V> : IDomainServiceCommandHandler, IServiceCommandHandler<T>
        where T : IServiceCommand
        where V : IEvent
    {
    }
}