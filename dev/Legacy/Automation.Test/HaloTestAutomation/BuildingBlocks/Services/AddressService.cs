using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using System;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class AddressService : _2AMDataHelper, IAddressService
    {
        /// <summary>
        /// Gets a populated Automation.DataModels.Address from the database matching the parameters
        /// </summary>
        /// <param name="buildingname"></param>
        /// <param name="buildingnumber"></param>
        /// <param name="streetname"></param>
        /// <param name="streetNumber"></param>
        /// <param name="countryDescription"></param>
        /// <param name="provinceDescription"></param>
        /// <param name="suburbDescription"></param>
        /// <returns></returns>
        public Automation.DataModels.Address GetAddress(string buildingname = "", string buildingnumber = "", string streetname = "",
                string streetNumber = "", string countryDescription = "", string provinceDescription = "", string suburbDescription = "",
                string cityDescription = "", string boxNumber = "", string PostalCode = "", int addresskey = 0)
        {
            var addresses = base.GetAddresses(cityDescription: cityDescription, addresskey: addresskey);

            return (from address in addresses
                    where
                        (address.BuildingName == buildingname && address.BuildingNumber == buildingnumber
                            && address.StreetName == streetname && address.StreetNumber == streetNumber
                            && address.RRR_CountryDescription == countryDescription && address.RRR_ProvinceDescription == provinceDescription
                            && address.RRR_SuburbDescription == suburbDescription)
                        || (address.BoxNumber != null && address.BoxNumber == boxNumber && address.RRR_SuburbDescription == suburbDescription)
                        || (address.AddressKey == addresskey)
                    select address).FirstOrDefault();
        }

        public Automation.DataModels.Address GetStreetAddressData()
        {
            return new Automation.DataModels.Address()
            {
                StreetNumber = "78",
                StreetName = "Armstrong Avenue",
                RRR_CountryDescription = "South Africa",
                RRR_ProvinceDescription = "Kwazulu-natal",
                RRR_SuburbDescription = "La Lucia Ridge"
            };
        }

        public Automation.DataModels.Address GetEmptyAddress()
        {
            var address = new Automation.DataModels.Address();
            var pleaseSelectVal =
            address.UnitNumber = "";
            address.BuildingName = "";
            address.BuildingNumber = "";
            address.StreetName = "";
            address.StreetNumber = "";
            address.RRR_CountryDescription = "South Africa";
            address.RRR_ProvinceDescription = "- Please Select -";
            address.RRR_SuburbDescription = "";
            return address;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Automation.DataModels.Address GetChangedAddress(Automation.DataModels.Address address)
        {
            var random = new Random();
            address.UnitNumber = random.Next(0, 10000).ToString();
            address.BuildingName = String.Format("BuildingName{0}", random.Next(0, 10000));
            address.BuildingNumber = random.Next(0, 10000).ToString();
            address.StreetName = String.Format("StreetName{0}", random.Next(0, 10000));
            address.StreetNumber = random.Next(0, 10000).ToString();
            return address;
        }
    }
}