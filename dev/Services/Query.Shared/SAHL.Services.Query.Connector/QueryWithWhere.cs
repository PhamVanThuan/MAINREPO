using System.Dynamic;
using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector
{
    public class QueryWithWhere : IQueryWithWhere
    {
        public IWhereClauseOperatorPart Where()
        {
            throw new System.NotImplementedException();
        }

        public ExpandoObject Execute()
        {
            throw new System.NotImplementedException();
        }

        public string AsJson()
        {
            throw new System.NotImplementedException();
        }
    }
}