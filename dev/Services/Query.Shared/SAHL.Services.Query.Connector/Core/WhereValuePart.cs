
namespace SAHL.Services.Interfaces.Query.Connector
{
    public class WhereValuePart : IWhereValuePart
    {
        private Interfaces.Query.Connector.IQuery Query { get; set; }
        private string queryValue { get; set; }

        public IWhereOperatorPart WhereOperatorPart;

        public WhereValuePart(IWhereOperatorPart WhereOperatorPart, Interfaces.Query.Connector.IQuery query)
        {
            Query = query;
            this.WhereOperatorPart = WhereOperatorPart;
        }

        public WhereValuePart(Interfaces.Query.Connector.IQuery query)
        {
            Query = query;
        }

        public Interfaces.Query.Connector.IQuery Value(string value)
        {
            queryValue = value;
            return Query;
        }

        public string AsString()
        {
            return "'" + queryValue + "'";
        }
        
    }
}