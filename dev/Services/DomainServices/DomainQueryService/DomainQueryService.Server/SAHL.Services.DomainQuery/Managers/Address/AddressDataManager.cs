using SAHL.Core.Data;
using SAHL.Services.DomainQuery.Managers.Address.Statements;

namespace SAHL.Services.DomainQuery.Managers.Address
{
    public class AddressDataManager : IAddressDataManager
    {
        private IDbFactory dbFactory;

        public AddressDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsAddressAClientAddress(int addressKey, int clientKey)
        {
            bool response;
            var query = new AddressIsAClientAddressStatement(addressKey, clientKey);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(query);
                response = (results >= 1);
            }
            return response;
        }
    }
}