using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Mappers
{
    public static class OperatorMapper
    {
        private static Dictionary<string, string> ValidOperators = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"eq", "="}, {"lt", "<"}, 
            {"lte", "<="}, {"gt", ">"}, 
            {"gte", ">="}, {"like", "like"},
            {"startswith", "like"}, {"endswith", "like"},
            {"contains", "like"}, {"in", "in"},
            {"between", "between"}, {"inq", "not in"}
        };

        public static string MapOperator(string op)
        {
            if (ValidOperators.ContainsKey(op))
            {
                return ValidOperators[op];
            }
            return "=";
        }

        public static bool IsOperator(string op)
        {
            return ValidOperators.ContainsKey(op);
        }

    }

}