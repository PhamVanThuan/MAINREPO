using SAHL.Core.Data.Models._2AM.Managers.LinkedKeyManager.Statements;
using System;

namespace SAHL.Core.Data.Models._2AM.Managers
{
    public class LinkedKeyDataManager : ILinkedKeyDataManager
    {
        private IDbFactory dbFactory;

        public LinkedKeyDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void InsertLinkedKey(int key, Guid guid)
        {
            var linkedKeyDataModel = new LinkedKeysDataModel(key, guid);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LinkedKeysDataModel>(linkedKeyDataModel);
                db.Complete();
            }
        }

        public void DeleteLinkedKey(Guid guid)
        {
            var removeLinkedKeysForGuidQuery = new RemoveLinkedKeysForGuidStatement(guid);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Delete<LinkedKeysDataModel>(removeLinkedKeysForGuidQuery);
                db.Complete();
            }
        }

        public int RetrieveLinkedKey(Guid guid)
        {
            GetLinkedKeyFromGuidStatement getLinkedKeyFromGuidQuery = new GetLinkedKeyFromGuidStatement(guid);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var linkedKeysDataModel = db.SelectOne<LinkedKeysDataModel>(getLinkedKeyFromGuidQuery);
                return linkedKeysDataModel.LinkedKey;
            }
        }
    }
}