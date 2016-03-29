using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetAddressFromPostalAddressStatement : ISqlStatement<AddressDataModel>
    {
        public PostalAddressModel PostalAddressModel { get; protected set; }

        public int AddressFormat { get { return (int)PostalAddressModel.AddressFormat; } }

        public string BoxNumber { get { return PostalAddressModel.BoxNumber; } }

        public string City { get { return PostalAddressModel.City; } }

        public string PostalCode { get { return PostalAddressModel.PostalCode; } }

        public string Province { get { return PostalAddressModel.Province; } }

        public GetAddressFromPostalAddressStatement(PostalAddressModel postalAddress)
        {
            this.PostalAddressModel = postalAddress;
        }

        public string GetStatement()
        {
            var query = @"select *
                        from
                            [Address] adr
                        where
                            adr.BoxNumber = @BoxNumber
                            and adr.RRR_ProvinceDescription = @Province
                            and adr.RRR_CityDescription = @City
                            and adr.RRR_PostalCode = @PostalCode
                            and adr.AddressFormatKey = @AddressFormat";

            return query;
        }
    }
}