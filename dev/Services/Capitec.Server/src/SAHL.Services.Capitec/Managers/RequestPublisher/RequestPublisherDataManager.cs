using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Serialisation;
using SAHL.Core.Web.Services;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public class RequestPublisherDataManager : IRequestPublisherDataManager
    {
        private const int completeGenericStatusId = 2;
        private IDbFactory dbFactory;
        public RequestPublisherDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AddPublishMessageFailure(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication)
        {
            var capitecApplicationAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(capitecApplication, JsonSerialisation.Settings);
            var publishMessageFailure = new PublishMessageFailureDataModel(Guid.NewGuid(), capitecApplicationAsJson, DateTime.Now);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<PublishMessageFailureDataModel>(publishMessageFailure);
                db.Complete();
            }
        }

        public void AddGenericMessage(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication)
        {
            var capitecApplicationAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(capitecApplication, JsonSerialisation.Settings);
            var genericMessage = new GenericMessageDataModel(DateTime.Now, capitecApplicationAsJson, capitecApplication.GetType().ToString(), completeGenericStatusId, 0, 0);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<GenericMessageDataModel>(genericMessage);
                db.Complete();
            }
        }
    }
}