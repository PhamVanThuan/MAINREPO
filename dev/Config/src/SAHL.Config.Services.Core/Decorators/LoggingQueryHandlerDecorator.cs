using System;
using System.Collections.Generic;
using System.Threading;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DecorationOrder(1)]
    public class LoggingQueryHandlerDecorator<TQuery, TQueryResult> :
        IServiceQueryHandlerDecorator<TQuery, TQueryResult> where TQuery : IServiceQuery<IServiceQueryResult<TQueryResult>>
    {
        private readonly IServiceQueryHandler<TQuery> innerHandler;
        private readonly ILogger logger;
        private readonly ILoggerSource loggerSource;

        public LoggingQueryHandlerDecorator(IServiceQueryHandler<TQuery> innerHandler, ILogger logger, IIocContainer container)
        {
            this.innerHandler = innerHandler;
            this.logger = logger;
            this.loggerSource = container.GetInstance<ILoggerSource>("QueryHandlerLogSource");
        }

        public IServiceQueryHandler<TQuery> InnerQueryHandler
        {
            get { return this.innerHandler; }
        }

        public ISystemMessageCollection HandleQuery(TQuery query)
        {
            var messages = new SystemMessageCollection();
            try
            {
                messages.Aggregate(innerHandler.HandleQuery(query));
            }
            catch (Exception ex)
            {
                //perform logging
                var parameters = new Dictionary<string, object>();
                parameters["QueryMsgId"] = query.Id;
                parameters["QueryObject"] = query;
                string userName = Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null ? Thread.CurrentPrincipal.Identity.Name : "";
                logger.LogErrorWithException(this.loggerSource, userName, GetInnerQueryHandlerType(innerHandler), ex.Message, ex, parameters);
                throw;
            }
            return messages;
        }

        private string GetInnerQueryHandlerType(IServiceQueryHandler<TQuery> handler)
        {
            if (handler is IServiceQueryHandlerDecorator<TQuery, TQueryResult>)
            {
                return GetInnerQueryHandlerType((handler as IServiceQueryHandlerDecorator<TQuery, TQueryResult>).InnerQueryHandler);
            }
            return handler.GetType().FullName;
        }
    }
}