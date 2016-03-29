namespace SAHL.Core.Services
{
    public interface IServiceCommandHandlerDecorator : IServiceRequestHandlerDecorator
    {
    }

    public interface IServiceCommandHandlerDecorator<T> : IServiceCommandHandlerDecorator, IServiceCommandHandler<T> where T : IServiceCommand
    {
        IServiceCommandHandler<T> InnerCommandHandler { get; }
    }
}