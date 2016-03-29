using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress
{
    public class DomiciliumAddressDataManager : IDomiciliumAddressDataManager
    {
        private IDbFactory dbFactory;
        public DomiciliumAddressDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<LegalEntityAddressDataModel> FindExistingActiveClientAddress(int clientAddressKey)
        {
            var clientAddressExistQuery = new GetActiveClientAddressStatement(clientAddressKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var clientAddress = db.Select<LegalEntityAddressDataModel>(clientAddressExistQuery);
                return clientAddress;
            }
        }

        public IEnumerable<LegalEntityDomiciliumDataModel> FindExistingClientPendingDomicilium(int clientKey)
        {
            var clientExistingPendingDomiciliumQuery = new FindExistingClientPendingDomiciliumStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var clientExistingPendingDomicilium = db.Select<LegalEntityDomiciliumDataModel>(clientExistingPendingDomiciliumQuery);
                return clientExistingPendingDomicilium;
            }
        }

        public int SavePendingDomiciliumAddress(LegalEntityDomiciliumDataModel clientAddressAsPendingDomicilium)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityDomiciliumDataModel>(clientAddressAsPendingDomicilium);
                db.Complete();
                return clientAddressAsPendingDomicilium.LegalEntityDomiciliumKey;
            }
        }

        public bool CheckIsAddressTypeAResidentialAddress(int clientAddressKey)
        {
            bool response;
            var query = new AddressTypeIsAResidentialAddressTypeStatement(clientAddressKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<int>(query);
                response = (result > 0);
            }
            return response;
        }

        public bool IsClientAddressPendingDomicilium(int clientAddressKey)
        {
            bool response;
            var query = new ClientAddressIsPendingDomiciliumStatement(clientAddressKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<int>(query);
                response = (result > 0);
            }
            return response;
        }

        public bool IsClientAddressActiveDomicilium(int clientAddressKey, int clientKey)
        {
            bool response;
            var query = new ClientAddressIsActiveDomiciliumAddressOnClientStatement(clientAddressKey, clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<int>(query);
                response = (result > 0);
            }
            return response;
        }
    }
}