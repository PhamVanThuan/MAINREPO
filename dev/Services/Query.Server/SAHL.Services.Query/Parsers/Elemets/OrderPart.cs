using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers.Elemets
{
    public class OrderPart : IOrderPart
    {

        public OrderPart()
        {
            Direction = OrderDirection.ASC;
        }

        public int Sequence { get; set; }
        public string Field { get; set; }
        public OrderDirection Direction { get; set; }

    }

}