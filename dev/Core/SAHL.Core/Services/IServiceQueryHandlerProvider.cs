namespace SAHL.Core.Services
{
    public interface IServiceQueryHandlerProvider
    {
        IServiceQueryHandler<T> GetQueryHandler<T>() where T : IServiceQuery;

        dynamic GetQueryHandler(object queryObject);
    }
}