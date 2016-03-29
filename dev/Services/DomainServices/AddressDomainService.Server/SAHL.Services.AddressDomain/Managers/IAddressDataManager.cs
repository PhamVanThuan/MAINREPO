using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.AddressDomain.Managers
{
    public interface IAddressDataManager
    {
        IEnumerable<SuburbDataModel> GetSuburbForModelData(string suburb, string city, string postalCode, string province);

        int SaveAddress(AddressDataModel addressDataModel);

        int SaveClientAddress(ClientAddressModel clientAddress);

        IEnumerable<AddressDataModel> FindAddressFromStreetAddress(StreetAddressModel streetAddress);

        IEnumerable<AddressDataModel> FindPostalAddressFromAddressValues(PostalAddressModel postalAddress);

        IEnumerable<PostOfficeDataModel> GetPostOfficeForModelData(string province, string city, string postalCode);

        void LinkAddressToProperty(int propertyKey, int addressKey);

        IEnumerable<AddressDataModel> FindAddressFromFreeTextAddress(FreeTextAddressModel freeTextAddressModel);

        int? GetPostOfficeKeyForCountry(string country);

        LegalEntityAddressDataModel GetExistingActiveClientAddress(int clientKey, int addressKey, Core.BusinessModel.Enums.AddressType addressType);

        IEnumerable<AddressDataModel> GetExistingActiveClientStreetAddressByClientKey(int clientKey);
    }
}