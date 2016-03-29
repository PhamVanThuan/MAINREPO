using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Query.Connector
{
    public class QueryToJson
    {
        public string PrepareQuery(string queryJson, string pagingJson)
        {
            return queryJson.Length > 0 && pagingJson.Length > 0
                ? queryJson + "&" + pagingJson
                : queryJson + pagingJson;
        }

        public string IncludePagingJson(IPagingPart pagingPart)
        {
            return pagingPart != null ? "paging={" + pagingPart.AsString() + "}" : "";
        }

        public string IncludeRelatedItemsJson(List<string> includeRelationships, bool includePartSeperator)
        {
            string json = "";
            if (!HasRelationshipFields(includeRelationships))
            {
                return json;
            }
            string partSeperator = CreateSeperator(includePartSeperator);
            json += partSeperator + "include: '";
            string seperator = "";
            foreach (var includeRelationship in includeRelationships)
            {
                json += seperator + includeRelationship;
                seperator = ",";
            }
            json += "'";
            return json;
        }

        private bool HasRelationshipFields(List<string> includeRelationships)
        {
            return includeRelationships.Count > 0;
        }

        public string IncludeFieldsJson(List<string> includeFields, bool includePartSeperator)
        {
            string json = "";
            if (!HasIncludeFields(includeFields))
            {
                return json;
            }
            string partSeperator = CreateSeperator(includePartSeperator);
            json += partSeperator + "fields: {";
            string seperator = "";
            foreach (var includeField in includeFields)
            {
                json += seperator + "'" + includeField + "':'true'";
                seperator = ",";
            }
            json += "}";
            return json;
        }

        private bool HasIncludeFields(List<string> includeFields)
        {
            return includeFields.Count > 0;
        }

        public string WrapAsFilter(string json)
        {
            return HasJsonSet(json) ? "filter={" + json + "}" : json;
        }

        private bool HasJsonSet(string json)
        {
            return json.Length > 0;
        }

        public string IncludeWhereJson(List<IWhereClauseOperatorPart> whereClauseOperatorParts, bool includePartSeperator)
        {
            string json = "";
            if (!HasWhereItems(whereClauseOperatorParts))
            {
                return json;
            }
            
            var groupedWhereItems = whereClauseOperatorParts.GroupBy(x => new { x.ClauseOperator, x.OperatorPart.QueryOperator });

            string jsonClauseOperator = "";
            foreach (var groupedWhereItem    in groupedWhereItems)
            {

                ClauseOperator clauseOperator = clauseOperator = groupedWhereItem.Key.ClauseOperator;
                QueryOperator queryOperator = queryOperator = groupedWhereItem.Key.QueryOperator;
                jsonClauseOperator = clauseOperator.ToString().ToLower();   
                string jsonQueryOperator = queryOperator.ToString().ToLower();                
                json = BreakOutWhereItems(whereClauseOperatorParts, clauseOperator, queryOperator, json, jsonQueryOperator);
            }

            json = WrapOpertors(json);
            json = WarpClauseOperator(jsonClauseOperator, json);

            string partSeperator = CreateSeperator(includePartSeperator);
            return partSeperator + "where: " + json;

        }

        private string WarpClauseOperator(string jsonClauseOperator, string json)
        {
            return "{" + jsonClauseOperator + ":" + json + "}";
        }

        private string WrapOpertors(string json)
        {
            return " {" + json + "}";
        }

        private string BreakOutWhereItems(List<IWhereClauseOperatorPart> whereClauseOperatorParts, ClauseOperator clauseOperator,
            QueryOperator queryOperator, string json, string jsonQueryOperator)
        {
            var whereItems =
                whereClauseOperatorParts.Where(
                    x => x.ClauseOperator == clauseOperator && x.OperatorPart.QueryOperator == queryOperator);

            string values = "";
            foreach (var where in whereItems)
            {
                values += InsertSeperator(values) + @where.OperatorPart.WhereFieldPart.AsString();
            }

            if (values.Length > 0)
            {
                json += InsertSeperator(json) + jsonQueryOperator + ": {" + values + "}";
            }
            return json;
        }

        private string InsertSeperator(string values)
        {
            return values.Length > 0 ? "," : "";
        }

        private string CreateSeperator(bool hasJson)
        {
            return hasJson ? "," : "";
        }

        public static bool ShouldIncludeSeperator(string json)
        {
            return json.Length > 0;
        }

        private bool HasWhereItems(List<IWhereClauseOperatorPart> whereClauseOperatorParts)
        {
            return whereClauseOperatorParts.Count > 0;
        }

        public string IncludeLimitJson(ILimitPart limitPart, bool includePartSeperator)
        {
            string json = "";
            if (!HasLimit(limitPart))
            {
                return json;
            }
            string partSeperator = CreateSeperator(includePartSeperator);
            json += partSeperator + limitPart.AsString();
            return json;
        }

        private bool HasLimit(ILimitPart limitPart)
        {
            return limitPart != null;
        }

        public string IncludeSkipJson(ISkipPart skipPart, bool includePartSeperator)
        {
            string json = "";
            if (!HasSkip(skipPart))
            {
                return json;
            }
            string partSeperator = CreateSeperator(includePartSeperator);
            json += partSeperator + skipPart.AsString();
            return json;
        }

        private bool HasSkip(ISkipPart skipPart)
        {
            return skipPart != null;
        }

        public string IncludeOrderByJson(List<IOrderPart> orderParts, bool includePartSeperator)
        {
            string json = "";
            if (!HasOrderBy(orderParts))
            {
                return json;
            }
            int i = 0;
            string partSeperator = CreateSeperator(includePartSeperator);
            string seperator = "";
            json += partSeperator + "order: {";
            foreach (var orderPart in orderParts)
            {
                json += seperator + i + ": " + orderPart.AsString();
                i++;
                seperator = ",";
            }
            json += "}";
            return json;
        }

        private bool HasOrderBy(List<IOrderPart> orderParts)
        {
            return orderParts.Count > 0;
        }
    }
}
