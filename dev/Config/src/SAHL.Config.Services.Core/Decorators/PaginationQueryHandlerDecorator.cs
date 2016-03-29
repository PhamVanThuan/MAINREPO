using System;
using System.Linq;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DecorationOrder(5)]
    public class PaginationQueryHandlerDecorator<TQuery, TQueryResult> : IServiceQueryHandlerDecorator<TQuery, TQueryResult>
        where TQuery : IServiceQuery<IServiceQueryResult<TQueryResult>>
    {
        private readonly IServiceQueryHandler<TQuery> innerHandler;
        private readonly IQueryParameterManager queryParameterManager;

        public PaginationQueryHandlerDecorator(IServiceQueryHandler<TQuery> innerHandler, IQueryParameterManager queryParameterManager)
        {
            this.innerHandler = innerHandler;
            this.queryParameterManager = queryParameterManager;
        }

        public IServiceQueryHandler<TQuery> InnerQueryHandler
        {
            get { return this.innerHandler; }
        }

        public ISystemMessageCollection HandleQuery(TQuery query)
        {
            var messages = new SystemMessageCollection();
            var paginationQueryParameter = this.queryParameterManager.GetParameter<PaginationQueryParameter>();

            // check if the handler is registered to implement pagination itself
            var actualHandler = this.GetActualHandler(this.InnerQueryHandler);

            if (actualHandler is IServiceQueryPaginationHandler)
            {
                ((IServiceQueryPaginationHandler)actualHandler).ReceivePaginationOptions(paginationQueryParameter.PageSize, paginationQueryParameter.CurrentPage);

                messages.Aggregate(innerHandler.HandleQuery(query));
            }
            else
            {
                messages.Aggregate(innerHandler.HandleQuery(query));
                if (query.Result == null) { return messages; }

                if (query.Result.Results.Any())
                {
                    var resultCountInAllPages = query.Result.Results.Count();

                    if (paginationQueryParameter.PageSize > 0 && paginationQueryParameter.CurrentPage >= 1)
                    {
                        var numberOfPages = (int)Math.Ceiling(resultCountInAllPages / (double)paginationQueryParameter.PageSize);

                        if (paginationQueryParameter.CurrentPage <= numberOfPages)
                        {
                            var pagedServiceQueryResult = new ServiceQueryResult<TQueryResult>(query.Result.Results.Skip((paginationQueryParameter.CurrentPage - 1) *
                                                                                               paginationQueryParameter.PageSize).Take(paginationQueryParameter.PageSize).ToList());
                            query.Result = pagedServiceQueryResult;
                            query.Result.ResultCountInPage = query.Result.Results.Count();
                            query.Result.ResultCountInAllPages = resultCountInAllPages;
                            query.Result.NumberOfPages = numberOfPages;
                        }
                        else
                        {
                            // requested page is out of bounds
                            var emptyServiceQueryResult = new ServiceQueryResult<TQueryResult>(Enumerable.Empty<TQueryResult>());
                            query.Result = emptyServiceQueryResult;
                            messages.AddMessage(new SystemMessage("Requested page is out of bounds.", SystemMessageSeverityEnum.Error));
                        }
                    }
                    else
                    {
                        // no pagination
                        query.Result.ResultCountInPage = resultCountInAllPages;
                        query.Result.ResultCountInAllPages = resultCountInAllPages;
                        query.Result.NumberOfPages = 1;
                    }
                }
                else
                {
                    // no results
                    query.Result.ResultCountInAllPages = 0;
                    query.Result.NumberOfPages = 0;
                    query.Result.ResultCountInPage = 0;
                }
            }

            return messages;
        }

        private IServiceQueryHandler<TQuery> GetActualHandler(IServiceQueryHandler<TQuery> handler)
        {
            if (handler is IServiceRequestHandlerDecorator)
            {
                return GetActualHandler(((IServiceQueryHandlerDecorator<TQuery, TQueryResult>)handler).InnerQueryHandler);
            }
            else
            {
                return handler;
            }
        }
    }
}