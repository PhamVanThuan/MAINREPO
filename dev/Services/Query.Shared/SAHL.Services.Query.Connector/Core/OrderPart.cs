using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Query.Connector.Core
{
    public class OrderPart : IOrderPart
    {
        private OrderDirection orderDirection;
        private IQuery Query { get; set; }
        private string Column { get; set; }

        public OrderPart(IQuery query, string column)
        {
            Query = query;
            orderDirection = OrderDirection.Asc;
            Column = column;
        }

        public IQuery Asc()
        {
            orderDirection = OrderDirection.Asc;
            return Query;
        }
        
        public IQuery Desc()
        {
            orderDirection = OrderDirection.Desc;
            return Query;
        }
        
        public string AsString()
        {
            return "'" + Column + " " + orderDirection.ToString().ToLower() + "'";
        }

    }
}