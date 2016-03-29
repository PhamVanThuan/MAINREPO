using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Parsers;
using System.Linq;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Validators;

namespace SAHL.Services.Query.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IFindQueryValidator findQueryValidator;
        private IQueryStringQueryParser QueryStringQueryParser { get; set; }
        private IJsonQueryParser JsonQueryParser { get; set; }
        private IPagingParser PagingParser { get; set; }

        public QueryFactory(IFindQueryValidator findQueryValidator, IQueryStringQueryParser queryStringQueryParser, IJsonQueryParser jsonQueryParser, IPagingParser pagingParser)
        {
            this.findQueryValidator = findQueryValidator;
            JsonQueryParser = jsonQueryParser;
            PagingParser = pagingParser;
            QueryStringQueryParser = queryStringQueryParser;
        }

        public IFindQuery CreateFindManyQuery(NameValueCollection input)
        {

            IFindQuery findQuery = CreateEmptyFindQuery();

            if (HasNoQueryParameters(input))
            {
                findQuery = CreateEmptyFindQuery();
            }
            else if (HasJsonFilerParameters(input))
            {
                string filter = input["filter"];
                findQuery = JsonQueryParser.FindManyQuery(filter);
                findQuery.FullFilterString = filter;
            }
            else
            {
                findQuery = QueryStringQueryParser.FindManyQuery(input);
            }

            if (HasPagingParameters(input))
            {
                string paging = input["paging"];
                findQuery.PagedPart = PagingParser.FindPaging(paging);

                if (findQuery.OrderBy.Count == 0)
                {
                    AddOrderById(findQuery);
                }

            }
            
            findQueryValidator.IsValid(findQuery);

            return findQuery;

        }

        private static void AddOrderById(IFindQuery findQuery)
        {
            findQuery.OrderBy.Add(new OrderPart()
            {
                Sequence = 0,
                Field = "Id",
                Direction = OrderDirection.ASC,
            });
        }

        private static bool HasPagingParameters(NameValueCollection input)
        {
            return input.AllKeys.Contains("paging");
        }

        private static bool HasJsonFilerParameters(NameValueCollection input)
        {
            return input.AllKeys.Contains("filter");
        }

        private static bool HasNoQueryParameters(NameValueCollection input)
        {
            return input.Count == 0;
        }

        public IFindQuery CreateEmptyFindQuery()
        {
            return new FindManyQuery()
            {
                Fields = new List<string>(),
                Limit = CreateLimitPart(100),
                Includes = new List<string>(),
                OrderBy = new List<IOrderPart>(),
                Skip = null,
                Where = new List<IWherePart>(),
                PagedPart = null,
            };
        }

        private ILimitPart CreateLimitPart(int count)
        {
            return new LimitPart()
            {
                Count = count
            };
        }
    }

    public interface IQueryFactory
    {
        IFindQuery CreateFindManyQuery(NameValueCollection input);
        IFindQuery CreateEmptyFindQuery();
    }
}