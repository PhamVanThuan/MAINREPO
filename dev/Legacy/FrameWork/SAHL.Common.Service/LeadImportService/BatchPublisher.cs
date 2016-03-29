using CsvHelper;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Communication;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Common.Service
{
    public class BatchPublisher : IBatchPublisher
    {
        readonly IMessageBus messageBus;
        readonly MessageBusDefaultConfiguration messageBusDefaultConfiguration;

        public BatchPublisher(IMessageBus messageBus, MessageBusDefaultConfiguration messageBusDefaultConfiguration)
        {
            this.messageBus = messageBus;
            this.messageBusDefaultConfiguration = messageBusDefaultConfiguration;
        }

        public void Publish<T>(IEnumerable<T> leads, IBatchService batchService) where T : class
        {
            
            foreach (var lead in leads)
            {
                var message = ConvertToMessage(lead);
                message.BatchID = batchService.Key;
                messageBus.Publish((dynamic)message);
            }
        }

        public IBatchMessage ConvertToMessage<T>(T model)
        {
            var types = (typeof(IBatchMessage)).Assembly.GetExportedTypes();
            Type messageType = types.Where(x => x.Name == string.Format("{0}Message", model.GetType().Name)).SingleOrDefault();
            var ctor = messageType.GetConstructors().Where(x => x.GetParameters().Length > 0).First();

            List<object> parameters = new List<object>();
            var modelProperties = model.GetType().GetProperties();
            foreach (var parameter in ctor.GetParameters())
            {
                if (parameter.Name.Equals("Application", StringComparison.InvariantCultureIgnoreCase))
                    parameters.Add(this.messageBusDefaultConfiguration.ApplicationName);

                var modelProperty = modelProperties.Where(x => x.Name.Equals(parameter.Name, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                if (modelProperty != null)
                    parameters.Add(modelProperty.GetValue(model, null));
            }

            var message = ctor.Invoke(parameters.ToArray()) as IBatchMessage;
            return message;
        }


    }
}
