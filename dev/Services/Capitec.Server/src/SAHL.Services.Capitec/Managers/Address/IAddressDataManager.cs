using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.Address
{
    public interface IAddressDataManager
    {
        void AddResidentialAddress(Guid addressID, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, string province, Guid suburbID, string city, string postalCode);
    }
}