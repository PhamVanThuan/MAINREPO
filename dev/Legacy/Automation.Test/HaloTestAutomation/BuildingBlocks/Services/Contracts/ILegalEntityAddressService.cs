using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILegalEntityAddressService
    {
        Automation.DataModels.LegalEntityAddress GetRandomLegalEntityAddress(string legalentityIdnumber = "", string legalentityRegisteredName = "", string legalentityRegistrationNumber = "");

        void SetupAddressData(int _legalEntityKey);

        void SetupAddressAsApplicationMailingAddress(int legalEntityKey, int offerKey);

        void SetupAddressAsAccountMailingAddress(int legalEntityKey, int accountKey);

        string GetFormattedAddressByKey(int addressKey);

        IEnumerable<Automation.DataModels.LegalEntityAddress> GetLegalEntityAddresses(int legalEntityKey);

        Automation.DataModels.LegalEntityAddress InsertLegalEntityAddress(Automation.DataModels.LegalEntityAddress legalEntityAddress);

        Automation.DataModels.LegalEntityDomicilium InsertLegalEntityDomiciliumAddress(int legalEntityAddressKey, int legalEntityKey, GeneralStatusEnum generalStatus);

        void DeleteLegalEntityDomiciliumAddress(int legalEntityKey);

        IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetLegalEntityDomiciliumAddress(int leKey, GeneralStatusEnum domiciliumStatus);

        IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetLegalEntityDomiciliumAddresses(int legalentitykey);

        Automation.DataModels.LegalEntityAddress InsertLegalEntityAddressByAddressType(int leKey, AddressFormatEnum addressFormatEnum, AddressTypeEnum addressTypeEnum, GeneralStatusEnum generalStatus);

        Dictionary<string, int> GetLegalEntityAddressesByAccountKey(int accountkey);

        void CleanupLegalEntityAddresses(int legalEntityKey, bool delete, GeneralStatusEnum generalStatus);
    }
}