using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DecorationOrder(4)]
    public class SortQueryHandlerDecorator<TQuery, TQueryResult> :
        IServiceQueryHandlerDecorator<TQuery, TQueryResult> where TQuery : IServiceQuery<IServiceQueryResult<TQueryResult>>
    {
        private readonly IServiceQueryHandler<TQuery> innerHandler;
        private readonly IQueryParameterManager queryParameterManager;

        public SortQueryHandlerDecorator(IServiceQueryHandler<TQuery> innerHandler, IQueryParameterManager queryParameterManager)
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
            var sortQueryParameter = this.queryParameterManager.GetParameter<SortQueryParameter>();

            messages.Aggregate(innerHandler.HandleQuery(query));

            if (!RequiresSorting(query, sortQueryParameter))
            {
                return messages;
            }

            var sortedList = CreateSortList(query.Result.Results, sortQueryParameter.OrderBy, sortQueryParameter.SortDirection);
            query.Result = new ServiceQueryResult<TQueryResult>(sortedList);

            return messages;
        }

        private static bool RequiresSorting(TQuery query, SortQueryParameter sortQueryParameter)
        {
            return query.Result != null
                && query.Result.Results != null
                && query.Result.Results.Count() > 1
                && !string.IsNullOrEmpty(sortQueryParameter.OrderBy)
                && !string.IsNullOrEmpty(sortQueryParameter.SortDirection.ToString());
        }

        private List<T> CreateSortList<T>(IEnumerable<T> dataSource, string orderBy, SortQueryParameter.SortDirectionOptions sortDirection)
        {
            List<T> returnList = new List<T>();
            returnList.AddRange(dataSource);
            PropertyInfo propInfo = typeof(T).GetProperty(orderBy);
            Comparison<T> compare = delegate (T a, T b)
            {
                bool asc = sortDirection == SortQueryParameter.SortDirectionOptions.Ascending;
                object valueA = asc ? propInfo.GetValue(a, null) : propInfo.GetValue(b, null);
                object valueB = asc ? propInfo.GetValue(b, null) : propInfo.GetValue(a, null);

                return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
            };
            returnList.Sort(compare);
            return returnList;
        }
    }
}