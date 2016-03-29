
namespace SAHL.Services.Interfaces.AddressDomain.Model
{

    public class GetClientStreetAddressByClientKeyQueryResult
    {
        public GetClientStreetAddressByClientKeyQueryResult() { }


        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}