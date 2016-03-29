using Automation.DataAccess;
using Common.Enums;

namespace BuildingBlocks.Services.Contracts
{
    public interface IAddressService
    {
        Automation.DataModels.Address GetAddress(string buildingname = "", string buildingnumber = "", string streetname = "",
                string streetNumber = "", string countryDescription = "", string provinceDescription = "", string suburbDescription = "",
                string cityDescription = "", string boxNumber = "", string PostalCode = "", int addresskey = 0);

        Automation.DataModels.Address GetStreetAddressData();

        int GetExistingResidentialAddress(string streetNum, string streetName, string p, string suburb);

        bool IsAddressLegalEntityLinkByGeneralStatus(int legalEntityKey, int addressKey, GeneralStatusEnum gsKey);

        bool IsFreeTextAddress(string line1, string line2, string line3, string line4, string line5, out int addressKey);

        string GetAddressType(int _legalEntityKey, int addressKey);

        bool IsExistingPOBoxAddress(string boxNumber, string postOffice, out int addressKey);

        bool IsExistingClusterBoxAddress(string clusterBox, string postOffice, out int addressKey);

        bool IsExistingPostNetAddress(string postNetSuite, string privateBag, string postOffice, out int addressKey);

        bool IsExistingPrivateBagAddress(string privateBag, string postOffice, out int addressKey);

        QueryResults GetAddressesByStreetName(string streetNameExpression);

        string GetFormattedAddressByKey(int addressKey);

        Automation.DataModels.Address InsertAddress(Automation.DataModels.Address address);

        Automation.DataModels.Address GetEmptyAddress();

        Automation.DataModels.Address GetChangedAddress(Automation.DataModels.Address address);
    }
}