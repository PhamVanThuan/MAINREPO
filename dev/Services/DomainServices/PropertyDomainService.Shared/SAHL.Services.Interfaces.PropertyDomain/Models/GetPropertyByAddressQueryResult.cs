namespace SAHL.Services.Interfaces.PropertyDomain.Models
{
    public class GetPropertyByAddressQueryResult
    {
        public GetPropertyByAddressQueryResult(int propertyKey) 
        {
            PropertyKey = propertyKey;
        }
        public int PropertyKey { get; set; }
    }
}