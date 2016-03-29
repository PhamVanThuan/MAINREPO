namespace SAHL.Core.Services
{
    public interface IServiceQueryHandlerDecorator : IServiceRequestHandlerDecorator
    {
    }

    public interface IServiceQueryHandlerDecorator<T, U> : IServiceQueryHandlerDecorator, IServiceQueryHandler<T> where T : IServiceQuery<IServiceQueryResult<U>>
    {
        IServiceQueryHandler<T> InnerQueryHandler { get; }
    }
}