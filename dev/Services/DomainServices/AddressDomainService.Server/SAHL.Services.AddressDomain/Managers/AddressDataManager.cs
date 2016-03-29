using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.AddressDomain.Managers.Statements;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.Managers
{
    public class AddressDataManager : IAddressDataManager
    {
        private IDbFactory dbFactory;

        public AddressDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveAddress(AddressDataModel newAddress)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<AddressDataModel>(newAddress);
                db.Complete();
                return newAddress.AddressKey;
            }
        }

        public int SaveClientAddress(ClientAddressModel clientAddress)
        {
            var effectiveDate = DateTime.Now;
            var generalStatusKey = 1;

            var newClientAddress = new LegalEntityAddressDataModel(clientAddress.ClientKey, clientAddress.AddressKey, (int)clientAddress.AddressType, effectiveDate, generalStatusKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityAddressDataModel>(newClientAddress);
                db.Complete();
                return newClientAddress.LegalEntityAddressKey;
            }
        }

        public IEnumerable<PostOfficeDataModel> GetPostOfficeForModelData(string province, string city, string postalCode)
        {
            IEnumerable<PostOfficeDataModel> postOffice;
            var postOfficeKeyQuery = new GetPostOfficeStatement(city, postalCode, province);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                postOffice = db.Select<PostOfficeDataModel>(postOfficeKeyQuery);
            }
            return postOffice;
        }

        public IEnumerable<SuburbDataModel> GetSuburbForModelData(string suburb, string city, string postalCode, string province)
        {
            IEnumerable<SuburbDataModel> suburbs;
            var suburbKeyQuery = new GetSuburbStatement(suburb, city, postalCode, province);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                suburbs = db.Select<SuburbDataModel>(suburbKeyQuery);
            }
            return suburbs;
        }

        public IEnumerable<AddressDataModel> FindAddressFromStreetAddress(StreetAddressModel streetAddress)
        {
            IEnumerable<AddressDataModel> addresses = Enumerable.Empty<AddressDataModel>();
            var getAddressFromStreetAddressQuery = new GetAddressFromStreetAddressStatement(streetAddress);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                addresses = db.Select<AddressDataModel>(getAddressFromStreetAddressQuery);
            }
            return addresses;
        }

        public IEnumerable<AddressDataModel> FindPostalAddressFromAddressValues(Interfaces.AddressDomain.Model.PostalAddressModel postalAddress)
        {
            IEnumerable<AddressDataModel> addresses = Enumerable.Empty<AddressDataModel>();

            var postalAddressQuery = new GetAddressFromPostalAddressStatement(postalAddress);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                addresses = db.Select<AddressDataModel>(postalAddressQuery);
            }

            return addresses;
        }

        public void LinkAddressToProperty(int propertyKey, int addressKey)
        {
            var updatePropertyAddress = new SetPropertyAddressStatement(propertyKey, addressKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<PropertyDataModel>(updatePropertyAddress);
                db.Complete();
            }
        }

        public IEnumerable<AddressDataModel> FindAddressFromFreeTextAddress(FreeTextAddressModel freeTextAddressModel)
        {
            var findAddressFromFreeTextAddressStatement = new FindAddressFromFreeTextAddressStatement(freeTextAddressModel);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(findAddressFromFreeTextAddressStatement);
            }
        }

        public int? GetPostOfficeKeyForCountry(string country)
        {
            var getPostOfficeKeyStatement = new GetPostOfficeKeyForCountryStatement(country);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<int?>(getPostOfficeKeyStatement);
            }
        }

        public LegalEntityAddressDataModel GetExistingActiveClientAddress(int clientKey, int addressKey, Core.BusinessModel.Enums.AddressType addressType)
        {
            GetExistingActiveClientAddressStatement statement = new GetExistingActiveClientAddressStatement(clientKey, addressKey, (int)addressType);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(statement);
            }
        }

        public IEnumerable<AddressDataModel> GetExistingActiveClientStreetAddressByClientKey(int clientKey)
        {
            GetActiveClientStreetAddressByClientKeyStatement statement = new GetActiveClientStreetAddressByClientKeyStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(statement);
            }
        }
    }
}