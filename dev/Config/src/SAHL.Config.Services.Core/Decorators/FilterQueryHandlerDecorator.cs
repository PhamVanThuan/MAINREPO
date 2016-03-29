using System.Linq;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DecorationOrder(3)]
    public class FilterQueryHandlerDecorator<TQuery, TQueryResult> :
        IServiceQueryHandlerDecorator<TQuery, TQueryResult> where TQuery : IServiceQuery<IServiceQueryResult<TQueryResult>>
    {
        private IServiceQueryHandler<TQuery> innerHandler;
        private IQueryParameterManager queryParameterManager;

        public IServiceQueryHandler<TQuery> InnerQueryHandler
        {
            get { return this.innerHandler; }
        }

        public FilterQueryHandlerDecorator(IServiceQueryHandler<TQuery> innerHandler, IQueryParameterManager queryParameterManager)
        {
            this.innerHandler = innerHandler;
            this.queryParameterManager = queryParameterManager;
        }

        public ISystemMessageCollection HandleQuery(TQuery query)
        {
            var messages = new SystemMessageCollection();

            FilterQueryParameter filterQueryParameter = this.queryParameterManager.GetParameter<FilterQueryParameter>();

            messages.Aggregate(innerHandler.HandleQuery(query));

            if (query.Result == null || query.Result.Results == null || !query.Result.Results.Any()
                || string.IsNullOrEmpty(filterQueryParameter.FilterOn) || string.IsNullOrEmpty(filterQueryParameter.FilterValue))
            {
                return messages;
            }

            var queryResults = query.Result.Results.
                Where(x => x.GetType()
                    .GetProperty(filterQueryParameter.FilterOn)
                    .GetValue(x)
                    .ToString()
                    .StartsWith(filterQueryParameter.FilterValue, true, new System.Globalization.CultureInfo("en-ZA"))
                )
                .ToList();

            query.Result = new ServiceQueryResult<TQueryResult>(queryResults);
            return messages;
        }
    }
}