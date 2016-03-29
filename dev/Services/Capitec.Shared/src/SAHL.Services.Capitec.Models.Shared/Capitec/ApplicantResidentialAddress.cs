using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicantResidentialAddress
    {
        public ApplicantResidentialAddress(string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, string suburb, string province, string city, string postalCode, int suburbKey)
        {
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.Province = province;
            this.City = city;
            this.PostalCode = postalCode;
            this.SuburbKey = suburbKey;
        }

        [DataMember]
        public string UnitNumber { get; protected set; }

        [DataMember]
        public string BuildingNumber { get; protected set; }

        [DataMember]
        public string BuildingName { get; protected set; }

        [DataMember]
        public string StreetNumber { get; protected set; }

        [DataMember]
        public string StreetName { get; protected set; }

        [DataMember]
        public string Suburb { get; protected set; }

        [DataMember]
        public int SuburbKey { get; protected set; }

        [DataMember]
        public string Province { get; protected set; }

        [DataMember]
        public string City { get; protected set; }

        [DataMember]
        public string PostalCode { get; protected set; }
    }
}