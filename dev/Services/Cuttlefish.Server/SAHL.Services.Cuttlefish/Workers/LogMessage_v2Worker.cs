using Newtonsoft.Json;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Shared.Messages;

namespace SAHL.Services.Cuttlefish.Workers
{
    public class LogMessage_v2Worker : IQueueConsumerWorker
    {
        private ILogMessageWriter logMessageWriter;
        private ILogMessageTypeConverter logMessageConverter;

        public LogMessage_v2Worker(ILogMessageWriter logMessageWriter, ILogMessageTypeConverter logMessageConverter)
        {
            this.logMessageConverter = logMessageConverter;
            this.logMessageWriter = logMessageWriter;
        }

        public void ProcessMessage(string queueMessage)
        {
            // parse the string into a logmessage
            var logMessage = JsonConvert.DeserializeObject<LogMessage>(queueMessage, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });

            LogMessageDataModel model = new LogMessageDataModel(logMessage.MessageDate,
                logMessageConverter.ConvertLogMessageTypeToString(logMessage.LogMessageType),
                logMessage.MethodName,
                logMessage.Message,
                logMessage.Source,
                logMessage.User,
                logMessage.MachineName,
                logMessage.Application
                );

            logMessageWriter.WriteMessage(model, logMessage.Parameters);
        }
    }
}