using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Server.Specs.Factory
{
    public static class FindQueryFactory
    {

        public static FindManyQuery Query()
        {
            FindManyQuery query = new FindManyQuery();
            query.Where = new List<IWherePart>();
            query.Where.Add(new WherePart()
            {
                ClauseOperator = "and",
                Field = "RegisterName",
                Operator = "=",
                Value = "Simple_Value"
            });

            return query;
        }

        public static FindManyQuery EmpyQuery()
        {
            FindManyQuery query = new FindManyQuery();
            query.Limit = null;
            query.OrderBy = new List<IOrderPart>();
            query.Skip = null;
            query.Where = new List<IWherePart>();
            query.Fields = new List<string>();
            return query;
        }
    }
}