using System;

namespace SAHL.Core.Services
{
    public class StructureMapServiceHandlerProvider : IServiceCommandHandlerProvider, IServiceQueryHandlerProvider
    {
        private IIocContainer container;

        public StructureMapServiceHandlerProvider(IIocContainer container)
        {
            this.container = container;
        }

        public IServiceCommandHandler<T> GetCommandHandler<T>() where T : IServiceCommand
        {
            return this.GetHandler<IServiceCommandHandler<T>>();
        }

        public dynamic GetCommandHandler(object commandObject)
        {
            return this.GetHandler(commandObject, typeof(IServiceCommandHandler<>));
        }

        public IServiceQueryHandler<T> GetQueryHandler<T>() where T : IServiceQuery
        {
            return this.GetHandler<IServiceQueryHandler<T>>();
        }

        public dynamic GetQueryHandler(object queryObject)
        {
            return this.GetHandler(queryObject, typeof(IServiceQueryHandler<>));
        }

        private T GetHandler<T>()
        {
            return container.GetInstance<T>();
        }

        private dynamic GetHandler(object requestObject, Type openGenericHandler)
        {
            Type objectType = requestObject.GetType();
            Type genericHandler = openGenericHandler.MakeGenericType(objectType);
            object requestHandler = container.GetInstance(genericHandler);
            return requestHandler as dynamic;
        }
    }
}