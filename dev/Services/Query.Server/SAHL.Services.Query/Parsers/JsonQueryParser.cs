using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Mappers;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Parsers
{
    public class JsonQueryParser : IJsonQueryParser
    {
        private const string limitFilter = "limit";
        private const string whereFilter = "where";
        private const string orderFilter = "order";
        private const string fieldsFilter = "fields";
        private const string includeFilter = "include";
        private const string skipFilter = "skip";
        private const string databaseWildcardCharacter = "%";
        private const string queryServiceWildcardCharacter = "*";
        private const int defaultTakeValue = 100;
        private const int defaultLimitValue = 100;
        private const int defaultSkipValue = 0;

        public IFindQuery FindManyQuery(string jsonInput)
        {
            IDictionary<string, object> queryParts = GetQueryParts(jsonInput);

            IFindQuery findManyQuery = new FindManyQuery();
            findManyQuery.Fields = GetIncludedFields(queryParts);
            findManyQuery.Includes = GetIncludedRelationships(queryParts);
            findManyQuery.Limit = GetLimitPart(queryParts);
            findManyQuery.Where = GetWhereParts(queryParts);
            findManyQuery.OrderBy = GetOrderByParts(queryParts);
            findManyQuery.Skip = GetSkipPart(queryParts, findManyQuery.Limit);
            return findManyQuery;
        }

        private List<string> GetIncludedRelationships(IDictionary<string, object> query)
        {
            List<string> includedFields = new List<string>();

            if (!HasRelationshipItems(query))
            {
                return includedFields;
            }
            //get fields values 
            var usableFields = ReadIncludedRelationships(query);

            foreach (var field in usableFields)
            {
                includedFields.Add(field);
            }

            return includedFields;
        }

        public ILimitPart GetLimitPart(IDictionary<string, object> query)
        {
            if (HasLimitItem(query))
            {
                int count = GetItemValue(query, limitFilter, defaultLimitValue);

                count = CheckDefaultLimit(count);

                return CreateLimitPart(count);
            }

            return CreateLimitPart(100);
        }

        private ISkipPart GetSkipPart(IDictionary<string, object> query, ILimitPart limitPart)
        {
            if (!HasSkipItem(query))
            {
                return null;
            }
            var take = CalculateTakeValue(limitPart);
            return new SkipPart
            {
                Take = take,
                Skip = GetItemValue(query, skipFilter, defaultLimitValue)
            };
        }

        private List<string> GetIncludedFields(IDictionary<string, object> query)
        {
            List<string> includedFields = new List<string>();

            if (!HasFieldItems(query))
            {
                return includedFields;
            }
            //get fields values 
            var usableFields = ReadIncludedFields(query);
            foreach (var field in usableFields)
            {
                includedFields.Add(field.Key);
            }

            return includedFields;
        }

        private IEnumerable<string> ReadIncludedRelationships(IDictionary<string, object> query)
        {
            List<string> includedRelationships = new List<string>();
            string includes = query[includeFilter].ToString();

            string[] allIncludes = includes.Split(",".ToCharArray());

            foreach (var toInclude in allIncludes)
            {
                includedRelationships.Add(toInclude.Trim());
            }

            return includedRelationships;
        }

        private List<IOrderPart> GetOrderByParts(IDictionary<string, object> query)
        {
            List<IOrderPart> orderBy = new List<IOrderPart>();

            if (!query.ContainsKey(orderFilter))
            {
                return orderBy;
            }
            var usableOrderByFields = GetOrderByFields(query);
            int sequence = 0;
            foreach (var orderByField in usableOrderByFields)
            {
                string field = orderByField.Value.ToString();
                orderBy.Add(new OrderPart
                {
                    Sequence = sequence,
                    Field = field
                });
                sequence++;
            }

            return orderBy;
        }

        private IOrderedEnumerable<KeyValuePair<string, object>> GetOrderByFields(IDictionary<string, object> query)
        {
            var orderByFields = GetProperties((ExpandoObject) query[orderFilter]);
            return orderByFields.OrderBy(x => x.Key);
        }

        public List<IWherePart> GetWhereParts(IDictionary<string, object> query)
        {
            List<IWherePart> whereParts = new List<IWherePart>();

            if (!query.ContainsKey(whereFilter))
            {
                return whereParts;
            }
            var wherePieces = GetProperties((ExpandoObject) query[whereFilter]);
            whereParts.Add(CreateWherePart("", "", "", "where", ""));
            UnpackWherePiece(whereParts[0], null, wherePieces);

            return RenameDuplicateParameters(whereParts);

        }

        private List<IWherePart> RenameDuplicateParameters(List<IWherePart> whereParts)
        {
            //Get all where parts that have a duplicate field name
            var partsToRename = FlattenWhereParts(whereParts)
                .Where(x => x.Field != String.Empty)
                .GroupBy(x => x.Field)
                .Select(group => new
                {
                    Field = group.Key,
                    WhereItems = group.ToList(),
                    Count = group.Count()
                })
                .Where(x => x.Count > 1);

            //for each of the where groups rename the parameter field from the second parameter
            foreach (var partToRename in partsToRename)
            {
                int i = 1;
                foreach (var someItem in partToRename.WhereItems)
                {
                    someItem.ParameterName += i != 1 ? i.ToString() : "";
                    i++;
                }

            }

            return whereParts;
        }

        private List<IWherePart> FlattenWhereParts(List<IWherePart> whereParts)
        {
            List<IWherePart> where = new List<IWherePart>();

            foreach (var wherePart in whereParts)
            {
                where.AddRange(FlattenWhereParts(wherePart.Where));
                where.Add(wherePart);
            }

            return where;

        }

        private void UnpackWherePiece(IWherePart mainWherePart, IWherePart wherePart,
            IDictionary<string, object> wherePieces)
        {
            var cleanedWherePart = wherePart ?? CreateWherePart("", "", "", "and", "=");

            foreach (var wherePiece in wherePieces)
            {
                string key = wherePiece.Key;
                if (IfLogicalOperator(key))
                {
                    var pieces = GetProperties((ExpandoObject) wherePiece.Value);
                    UnpackWherePiece(mainWherePart, CreateWherePart("", "", "", key, "="), pieces);
                }
                else if (OperatorMapper.IsOperator(key))
                {
                    cleanedWherePart.Operator = wherePiece.Key;
                    var pieces = GetProperties((ExpandoObject) wherePiece.Value);
                    UnpackWherePiece(mainWherePart, cleanedWherePart, pieces);
                }
                else
                {
                    var nextPart = CopyAndCreateWherePart(cleanedWherePart, key, wherePiece);
                    mainWherePart.Where.Add(nextPart);
                }
            }
        }

        private IWherePart CopyAndCreateWherePart(IWherePart wherePart, string key,
            KeyValuePair<string, object> wherePiece)
        {
            string clauseOperator = wherePart.ClauseOperator.Length != 0 ? wherePart.ClauseOperator : "and";
            string operation = wherePart.Operator.Length != 0 ? OperatorMapper.MapOperator(wherePart.Operator) : "=";

            string value = wherePiece.Value.ToString();
            value = ApplyWildCards(wherePart, wherePiece, value);

            IWherePart nextPart = CreateWherePart(key, key, value, clauseOperator, operation);
            return nextPart;
        }

        private bool IfLogicalOperator(string key)
        {
            return key.Equals("and", StringComparison.InvariantCultureIgnoreCase) ||
                   key.Equals("or", StringComparison.InvariantCultureIgnoreCase);
        }

        private WherePart CreateWherePart(string key, string paramatereName, string value, string clauseOperator,
            string operation)
        {
            return new WherePart
            {
                Field = key,
                Value = value,
                ParameterName = paramatereName,
                ClauseOperator = clauseOperator,
                Operator = operation,
                Where = new List<IWherePart>()
            };
        }

        private IDictionary<string, object> GetProperties(ExpandoObject expandoObject)
        {
            IDictionary<string, object> properties = DictionaryFactory.CreateCaseInsensitiveObjectDictionary();

            if (expandoObject == null)
            {
                return DictionaryFactory.CreateCaseInsensitiveObjectDictionary();
            }
            IDictionary<string, object> tempProperties = expandoObject;
            foreach (var key in tempProperties.Keys)
            {
                properties.Add(key, tempProperties[key]);
            }

            return properties;
        }

        private IDictionary<string, object> GetQueryParts(string jsonInput)
        {
            dynamic theFilter = JsonConvert.DeserializeObject<ExpandoObject>(jsonInput);
            return GetProperties(theFilter);
        }

        private IEnumerable<KeyValuePair<string, object>> ReadIncludedFields(IDictionary<string, object> query)
        {
            IDictionary<string, object> fields = GetProperties((ExpandoObject) query[fieldsFilter]);
            var usableFields = fields.Where(x => x.Value.ToString().ToLower() == "true");
            return usableFields;
        }

        private int GetItemValue(IDictionary<string, object> query, string key, int defaultLimit)
        {
            int limitCount;
            if (!int.TryParse(query[key].ToString(), out limitCount))
            {
                limitCount = defaultLimit;
            }
            return limitCount;
        }

        private bool HasSkipItem(IDictionary<string, object> query)
        {
            return query.ContainsKey(skipFilter);
        }

        private bool HasLimitItem(IDictionary<string, object> query)
        {
            return query.ContainsKey(limitFilter);
        }

        private bool HasFieldItems(IDictionary<string, object> query)
        {
            return query.ContainsKey(fieldsFilter);
        }

        private bool HasRelationshipItems(IDictionary<string, object> query)
        {
            return query.ContainsKey(includeFilter);
        }

        private int CalculateTakeValue(ILimitPart limitPart)
        {
            int take = defaultTakeValue;
            if (limitPart != null)
            {
                take = limitPart.Count;
            }
            return take;
        }

        private string ApplyWildCards(IWherePart wherePart, KeyValuePair<string, object> wherePiece, string value)
        {

            if (wherePart.Operator == "endswith")
            {
                return databaseWildcardCharacter + wherePiece.Value;
            }
            if (wherePart.Operator == "startswith")
            {
                return wherePiece.Value + databaseWildcardCharacter;
            }
            if (wherePart.Operator == "contains")
            {
                return databaseWildcardCharacter + wherePiece.Value + databaseWildcardCharacter;
            }
            if (wherePart.Operator == "like")
            {
                return wherePiece.Value.ToString().Replace(queryServiceWildcardCharacter, databaseWildcardCharacter);
            }
            return value;
        }

        private LimitPart CreateLimitPart(int count)
        {
            return new LimitPart
            {
                Count = count
            };
        }

        private int CheckDefaultLimit(int count)
        {
            return count > 100 ? 100 : count;
        }

    }

}
