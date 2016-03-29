using SAHL.Core.Data;
using SAHL.Services.ApplicationDomain.Managers.Domicilium.Statements;

namespace SAHL.Services.ApplicationDomain.Managers.Domicilium
{
    public class DomiciliumDataManager : IDomiciliumDataManager
    {
        private IDbFactory dbFactory;
        public DomiciliumDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsDomiciliumAddressPendingDomiciliumAddress(int ClientDomiciliumKey)
        {
            bool response;
            IsDomiciliumAddressPendingDomiciliumAddressStatement query = new IsDomiciliumAddressPendingDomiciliumAddressStatement(ClientDomiciliumKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<int>(query);
                response = (results > 0);
            }
            return response;
        }
    }
}
