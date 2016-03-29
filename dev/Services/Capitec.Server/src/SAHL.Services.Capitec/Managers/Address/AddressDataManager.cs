using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.Address
{
    public class AddressDataManager : IAddressDataManager
    {
        private IDbFactory dbFactory;
        public AddressDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AddResidentialAddress(Guid addressID, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, string province, Guid suburbID, string city, string postalCode)
        {
            var addressFormatEnumId = Guid.Parse(AddressFormatEnumDataModel.STREET);

            var address = new AddressDataModel(addressID, addressFormatEnumId, null, unitNumber, buildingNumber, buildingName, streetNumber, streetName, suburbID, null, null, null, null, null, null, null);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(address);
                db.Complete();
            }
        }
    }
}