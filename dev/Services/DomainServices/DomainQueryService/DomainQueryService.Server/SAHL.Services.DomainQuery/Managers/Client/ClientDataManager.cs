using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.DomainQuery.Managers.Client.Statements;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DomainQuery.Managers.Client
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
            ClientExistsStatement statement = new ClientExistsStatement(clientKey);
            int matchingClientsCount;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                matchingClientsCount = db.SelectOne(statement);
            }
            return matchingClientsCount == 1;
        }

        public bool IsClientANaturalPerson(int clientKey)
        {
            IsClientANaturalPersonStatement statement = new IsClientANaturalPersonStatement(clientKey);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(statement);
                return (results == (int)LegalEntityType.NaturalPerson);
            }
        }
    }
}