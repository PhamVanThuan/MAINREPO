using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.ClientDataManager.Statements;

namespace SAHL.DomainServiceChecks.Managers.ClientDataManager
{
    public class ClientDataManager : IClientDataManager
    {
        private IDbFactory dbFactory;

        public ClientDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsClientOnOurSystem(int clientKey)
        {
            ClientExistsStatement query = new ClientExistsStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(query);
                return (results > 0);
            }
        }

        public bool IsClientANaturalPerson(int clientKey)
        {
            IsClientANaturalPersonStatement query = new IsClientANaturalPersonStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(query);
                return (results == (int)LegalEntityType.NaturalPerson);
            }
        }
    }
}