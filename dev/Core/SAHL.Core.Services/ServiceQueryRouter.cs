using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public class ServiceQueryRouter : IServiceQueryRouter
    {
        private IServiceQueryHandlerProvider queryHandlerProvider;

        public ServiceQueryRouter(IServiceQueryHandlerProvider queryHandlerProvider)
        {
            this.queryHandlerProvider = queryHandlerProvider;
        }

        public ISystemMessageCollection HandleQuery<T>(T query) where T : IServiceQuery
        {
            IServiceQueryHandler<T> handler = this.queryHandlerProvider.GetQueryHandler<T>();
            return handler.HandleQuery(query);
        }

        public ISystemMessageCollection HandleQuery(object query)
        {
            dynamic handler = this.queryHandlerProvider.GetQueryHandler(query);
            return handler.HandleQuery((dynamic)query);
        }
    }
}