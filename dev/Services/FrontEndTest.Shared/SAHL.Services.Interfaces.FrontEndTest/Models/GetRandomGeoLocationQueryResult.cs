namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetRandomGeoLocationQueryResult
    {
        public GetRandomGeoLocationQueryResult(int suburbKey, string suburb, int cityKey, string city, int provinceKey, string province,
                                               int countryKey, string country, int postOfficeKey, string postalCode)
        {
            this.SuburbKey = suburbKey;
            this.Suburb = suburb;
            this.CityKey = cityKey;
            this.City = city;
            this.ProvinceKey = provinceKey;
            this.Province = province;
            this.CountryKey = countryKey;
            this.Country = country;
            this.PostOfficeKey = postOfficeKey;
            this.PostalCode = postalCode;
        }

        public int SuburbKey { get; set; }

        public string Suburb { get; set; }

        public int CityKey { get; set; }

        public string City { get; set; }

        public int ProvinceKey { get; set; }

        public string Province { get; set; }

        public int CountryKey { get; set; }

        public string Country { get; set; }

        public int PostOfficeKey { get; set; }

        public string PostalCode { get; set; }
    }
}