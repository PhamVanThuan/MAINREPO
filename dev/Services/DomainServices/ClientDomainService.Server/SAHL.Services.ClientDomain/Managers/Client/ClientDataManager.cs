using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Managers.Client.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Managers
{
    public class ClientDataManager : IClientDataManager
    {
        private IDbFactory dbFactory;

        public ClientDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public LegalEntityDataModel FindExistingClient(int clientKey)
        {
            var clientQuery = new GetClientStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<LegalEntityDataModel>(clientQuery);
            }
        }

        public int AddNewLegalEntity(LegalEntityDataModel legalEntity)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityDataModel>(legalEntity);
                db.Complete();
                return legalEntity.LegalEntityKey;
            }
        }

        public bool DoesClientMarketingOptionExist(int clientKey, int marketingOptionKey)
        {            
            var statement = new DoesClientMarketingOptionExistStatement(clientKey, marketingOptionKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select(statement);
                return results.Any();
            }
        }

        public void AddNewMarketingOptions(LegalEntityMarketingOptionDataModel marketingOption)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityMarketingOptionDataModel>(marketingOption);
                db.Complete();
            }
        }

        public void UpdateLegalEntity(LegalEntityDataModel legalEntity)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<LegalEntityDataModel>(legalEntity);
                db.Complete();
            }
        }

        public LegalEntityDataModel FindExistingClientByIdNumber(string identityNumber)
        {
            LegalEntityDataModel existingLegalEntityDataModel = null;
            var statement = new FindClientByIdNumberStatement(identityNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                existingLegalEntityDataModel = db.SelectOne(statement);
            }
            return existingLegalEntityDataModel;
        }

        public LegalEntityDataModel FindExistingClientByPassportNumber(string passportNumber)
        {
            LegalEntityDataModel existingLegalEntityDataModel = null;
            var statement = new FindClientByPassportNumberStatement(passportNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                existingLegalEntityDataModel = db.SelectOne(statement);
            }
            return existingLegalEntityDataModel;
        }

        public IEnumerable<int> FindOpenAccountKeysForClient(int clientKey)
        {
            var statement = new FindOpenAccountKeysForClientStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<int>(statement);
            }
        }

        public IEnumerable<int> FindOpenApplicationNumbersForClient(int clientKey)
        {
            var statement = new FindOpenApplicationNumbersForClientStatement(clientKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<int>(statement);
            }
        }
    }
}