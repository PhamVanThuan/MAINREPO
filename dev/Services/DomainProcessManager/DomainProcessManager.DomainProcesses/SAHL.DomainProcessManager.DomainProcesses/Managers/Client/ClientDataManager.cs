using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Client.Statements;

namespace SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client
{
    public class ClientDataManager : IClientDataManager
    {
        private IDbFactory dbFactory;

        public ClientDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int? GetEmployerKey(string employerName)
        {
            var employerExistsStatement = new DoesEmployerExistStatement(employerName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<int?>(employerExistsStatement);
            }
        }

        public int GetClientKeyForClientAddress(int clientAddressKep)
        {
            var getClientKeyForClientAddressStatement = new GetClientKeyForClientAddressStatement(clientAddressKep);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<int>(getClientKeyForClientAddressStatement);
            }
        }
    }
}