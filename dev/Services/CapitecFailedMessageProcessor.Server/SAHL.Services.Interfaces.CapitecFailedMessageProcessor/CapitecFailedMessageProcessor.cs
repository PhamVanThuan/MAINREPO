using Newtonsoft.Json.Linq;
using SAHL.Core.Configuration;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.IoC;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.Shared;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
    public class CapitecFailedMessageProcessor : ICapitecFailedMessageProcessor
    {
        private IMessageBus messageBus;

        private ISerializationProvider serializationProvider;

        private ITimer timer;

        private ITimerFactory timerFactory;

        public CapitecFailedMessageProcessor(IMessageBus messageBus, ISerializationProvider serializationProvider, ITimerFactory timerFactory)
        {
            this.messageBus = messageBus;
            this.serializationProvider = serializationProvider;
            this.timerFactory = timerFactory;
        }
        
        public void Initialize()
        {
            timer = timerFactory.Get(ProcessFailedMessages);
        }

        public void ProcessFailedMessages(object state)
        {
            timer.Stop();
            var failedRequests = new List<PublishMessageFailureDataModel>();
            using (var db = new Db().InAppContext())
            {
                failedRequests = db.Select<PublishMessageFailureDataModel>("select * from [Capitec].dbo.PublishMessageFailure").ToList();
            }
            foreach (var failedRequest in failedRequests)
            {
                try
                {
                    var capitecApplication = GetCapitecApplication(failedRequest.Message);
                    var request = new CreateCapitecApplicationRequest(capitecApplication, Guid.NewGuid());
                    messageBus.Publish((dynamic)request);
                    using (var db = new Db().InAppContext())
                    {
                        db.DeleteByKey<PublishMessageFailureDataModel, Guid>(failedRequest.Id);
                        db.Complete();
                    }
                }
                catch (Exception)
                {
                    //Leave the message in the failed message table.
                }
            }
            timer = timerFactory.Get(ProcessFailedMessages);
        }

        public void Start()
        {
            Initialize();
        }

        public void Stop()
        {
            Teardown();
        }

        public void Teardown()
        {
            if (messageBus != null)
            {
                messageBus.Dispose();
                messageBus = null;
            }
        }

        internal CapitecApplication GetCapitecApplication(string serializedCapitecApplication)
        {
            var jsonObject = JObject.Parse(serializedCapitecApplication);
            var capitecApplicationType = jsonObject["$type"];
            var openGenericMethod = typeof(JsonSerializationProvider).GetMethod("Deserialize");
            var closedGenericMethod = openGenericMethod.MakeGenericMethod(Type.GetType(capitecApplicationType.ToString()));
            var application = closedGenericMethod.Invoke(serializationProvider, new object[] { serializedCapitecApplication });
            return application as CapitecApplication;
        }
    }
}