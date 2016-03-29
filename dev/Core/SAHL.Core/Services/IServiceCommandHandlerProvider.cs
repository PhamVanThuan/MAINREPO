namespace SAHL.Core.Services
{
    public interface IServiceCommandHandlerProvider
    {
        IServiceCommandHandler<T> GetCommandHandler<T>() where T : IServiceCommand;

        dynamic GetCommandHandler(object commandObject);
    }
}