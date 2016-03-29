using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Mappers;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Parsers
{
    public class QueryStringQueryParser : IQueryStringQueryParser
    {
        private const string limitFilter = "filter[limit]";
        private const string whereFilter = "filter[where]";
        private const string orderFilter = "filter[order]";
        private const string fieldsFilter = "filter[fields]";
        private const string includeFilter = "filter[include]";
        private const string skipFilter = "filter[skip]";
        private const int defaultTakeValue = 100;

        public IFindQuery FindManyQuery(NameValueCollection input)
        {
            IFindQuery findManyQuery = new FindManyQuery();
            findManyQuery.Fields = GetIncludedFields(input);
            findManyQuery.Includes = GetIncludedRelationships(input);
            findManyQuery.Limit = GetLimitPart(input);
            findManyQuery.Where = GetWhereParts(input);
            findManyQuery.OrderBy = GetOrderByParts(input);
            findManyQuery.Skip = GetSkipPart(input, findManyQuery.Limit);
            return findManyQuery;
        }

        public ILimitPart GetLimitPart(NameValueCollection input)
        {
            var limit = FindLimitPart(input);
            return ContainsValidLimitInput(limit) ? CreateLimitPart(input[limitFilter]) : null;
        }

        private ILimitPart CreateDefaultLimit()
        {
            return new LimitPart()
            {
                Count = 100
            };
        }

        public List<IWherePart> GetWhereParts(NameValueCollection input)
        {
            var wheres = FindAllWhereFilterParts(input);
            return AddWhereParts(input, wheres);
        }

        public string GetColumnName(string key)
        {
            Regex regex = new Regex(@"(\[([^(or|and|where|lte|gte|like|eq|lt|gt)].)(\w+)\])");
            Match match = regex.Match(key);

            return match.Success ? CleanMatchedValue(match) : "";
        }

        private List<string> GetIncludedRelationships(NameValueCollection input)
        {
            return ContainsValidIncludeInput(input) ? GetRelationshipFields(input) : new List<string>();
        }

        private List<string> GetRelationshipFields(NameValueCollection input)
        {
            string suppliedFields = input[includeFilter];
            string[] fields = suppliedFields.Split(",".ToCharArray());
            List<string> includedRelationships = new List<string>();
            foreach (var field in fields)
            {
                includedRelationships.Add(field);
            }
            return includedRelationships;
        }

        private bool ContainsValidIncludeInput(NameValueCollection input)
        {
            return input.AllKeys.FirstOrDefault(a => a.StartsWith(includeFilter)) != null;
        }

        public ISkipPart GetSkipPart(NameValueCollection input, ILimitPart limitPart)
        {
            var skip = FindSkipParts(input);
            return ContainsValidSkipInput(skip) ? CreateSkipPart(input, DetermineTakeValue(limitPart)) : null;
        }

        public string GetOperator(string key)
        {
            Regex regex = new Regex(@"\[(eq|gt|lt|like|lte|gte|in|inq)\]");
            Match match = regex.Match(key);

            return OperatorMapper.MapOperator(match.Success ? CleanMatchedValue(match) : "eq");
        }

        public string GetClauseOperation(string key)
        {
            Regex regex = new Regex(@"\[(and|or)\]");
            Match match = regex.Match(key);

            return match.Success ? CleanMatchedValue(match) : "and";
        }

        public string GetIncludeField(string key)
        {
            Regex regex = new Regex(@"\[([^(fields)])(\w+)\]");
            Match match = regex.Match(key);

            return match.Success ? CleanMatchedValue(match) : "";
        }

        private string CleanMatchedValue(Match match)
        {
            return match.Value.Substring(1, match.Value.Length - 2);
        }

        public string GetOrderField(string key)
        {
            Regex regex = new Regex(@"\[([0-9]+)\]");
            Match match = regex.Match(key);

            return match.Success ? CleanMatchedValue(match) : "";
        }

        private int DetermineTakeValue(ILimitPart limitPart)
        {
            return limitPart != null ? limitPart.Count : defaultTakeValue;
        }

        private ISkipPart CreateSkipPart(NameValueCollection input, int take)
        {
            string suppliedSkip = input[skipFilter];
            int skip;
            int.TryParse(suppliedSkip, out skip);

            return new SkipPart
            {
                Take = take,
                Skip = skip
            };
        }

        private List<string> GetIncludedFields(NameValueCollection input)
        {
            List<string> returnedFields = new List<string>();
            var includedFields = FindAllFields(input);
            foreach (string includedField in includedFields)
            {
                string fieldName = GetIncludeField(includedField);
                if (input[includedField].ToLower() == "true")
                {
                    returnedFields.Add(fieldName);
                }
            }

            return returnedFields;
        }

        private List<IOrderPart> GetOrderByParts(NameValueCollection input)
        {
            List<IOrderPart> orderByFields = new List<IOrderPart>();
            var orderByParts = FindOrderByParts(input);
            foreach (string orderByPart in orderByParts)
            {
                string orderItem = GetOrderField(orderByPart);
                string orderField = input[orderByPart];
                int sequence;
                int.TryParse(orderItem, out sequence);
                IncludeOrderItem(orderByFields, orderField, sequence);
            }

            return orderByFields;
        }

        private void IncludeOrderItem(List<IOrderPart> orderByFields, string orderField, int sequence)
        {
            orderByFields.Add(new OrderPart
            {
                Sequence = sequence,
                Field = orderField
            });
        }

        private IEnumerable<string> FindOrderByParts(NameValueCollection input)
        {
            return input.AllKeys.Where(a => a.StartsWith(orderFilter));
        }

        private IEnumerable<string> FindSkipParts(NameValueCollection input)
        {
            return input.AllKeys.Where(a => a.StartsWith(skipFilter));
        }

        private List<IWherePart> AddWhereParts(NameValueCollection input, IEnumerable<string> wheres)
        {
            List<IWherePart> whereItems = new List<IWherePart>();
            foreach (var whereKey in wheres)
            {
                whereItems.Add(CreateWhereItem(whereKey, input[whereKey]));
            }
            return whereItems;
        }

        private IEnumerable<string> FindAllWhereFilterParts(NameValueCollection input)
        {
            return input.AllKeys.Where(a => a.StartsWith(whereFilter));
        }

        private IEnumerable<string> FindAllFields(NameValueCollection input)
        {
            return input.AllKeys.Where(a => a.StartsWith(fieldsFilter));
        }

        private bool ContainsValidLimitInput(IEnumerable<string> limit)
        {
            if (limit.Count() > 1)
            {
                throw new ArgumentException("Only one skip is permitted in a filter query");
            }

            return limit.Count() == 1;
        }

        private bool ContainsValidSkipInput(IEnumerable<string> skip)
        {
            if (skip.Count() > 1)
            {
                throw new ArgumentException("Only one skip is permitted in a filter query");
            }

            return skip.Count() == 1;
        }

        private ILimitPart CreateLimitPart(string passedCount)
        {
            int count = int.Parse(passedCount);
            return new LimitPart { Count = count };
        }

        private static IEnumerable<string> FindLimitPart(NameValueCollection input)
        {
            return input.AllKeys.Where(a => a.StartsWith(limitFilter));
        }

        private WherePart CreateWhereItem(string key, string value)
        {
            string columnName = GetColumnName(key);
            string operation = GetOperator(key);
            string clauseOperation = GetClauseOperation(key);

            return new WherePart
            {
                ClauseOperator = clauseOperation,
                Field = columnName,
                Operator = operation,
                Value = value
            };
        }
    }
}
