using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.JsonDocument.Managers.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.JsonDocument.Managers
{
    public class JsonDocumentDataManager : IJsonDocumentDataManager
    {
        private IDbFactory dbFactory;

        public JsonDocumentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void CreateJsonDocument(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data)
        {
            var statement = new InsertJsonDocumentStatement(id, name, description, version, documentFormatVersion, documentType, data);
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert<JsonDocumentDataModel>(statement);
                dbContext.Complete();
            }
        }

        public void SaveJsonDocument(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data)
        {
            var statement = new UpdateJsonDocumentStatement(id, name, description, version, documentFormatVersion, documentType, data);
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update<JsonDocumentDataModel>(statement);
                dbContext.Complete();
            }
        }

        public bool DoesDocumentExist(Guid id)
        {
            bool result = false;
            var statement = new DoesDocumentExistStatement(id);
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                result = dbContext.Select<JsonDocumentDataModel>(statement).Count() > 0;
                dbContext.Complete();
            }
            return result;
        }
    }
}
