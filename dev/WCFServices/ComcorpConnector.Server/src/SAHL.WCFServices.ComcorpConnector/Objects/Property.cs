using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class Property
    {
        [DataMember]
        public string ComplexName { get; set; }

        [DataMember]
        public string PortionNo { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Property Address City cannot be longer than 50 characters.")]
        public string AddressCity { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Property Address Suburb cannot be longer than 50 characters.")]
        public string AddressSuburb { get; set; }

        [DataMember]
        public string ContactCellphone { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Property Address Code cannot be longer than 50 characters.")]
        public string PostalCode { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Property Address Province cannot be longer than 50 characters.")]
        public string Province { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Property Address Street Name cannot be longer than 50 characters.")]
        public string StreetName { get; set; }

        [DataMember]
        [MaxLength(10, ErrorMessage = "Property Address Street Number cannot be longer than 10 characters.")]
        public string StreetNo { get; set; }

        [DataMember]
        public string SellerIDNo { get; set; }

        [DataMember]
        public string StandErfNo { get; set; }

        [DataMember]
        public bool UsePropertyAddressAsLoanMailingAddress { get; set; }

        [DataMember]
        public bool UsePropertyAddressAsDomiciliumAddress { get; set; }
    }
}