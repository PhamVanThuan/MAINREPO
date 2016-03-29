using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Query.Connector.Core
{
    public class OrderDirectionPart : IOrderDirectionPart
    {
        private readonly IOrderPart _orderPart;
        private readonly IQuery Query;
        private OrderDirection orderDirection;

        public OrderDirectionPart(IOrderPart orderPart, IQuery query)
        {
            _orderPart = orderPart;
            Query = query;
            orderDirection = OrderDirection.Asc;
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
            return orderDirection.ToString();
        }
        
    }
 
}
