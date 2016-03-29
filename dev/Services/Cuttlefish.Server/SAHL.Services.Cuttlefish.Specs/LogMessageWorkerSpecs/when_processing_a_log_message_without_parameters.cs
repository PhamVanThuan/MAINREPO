using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using NSubstitute;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Workers;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Cuttlefish.Specs.LogMessageWorkerSpecs
{
    public class when_processing_a_log_message_without_parameters : WithFakes
    {
        private static LogMessage_v2Worker worker;
        private static ILogMessageWriter messageWriter;
        private static LogMessageTypeConverter logMessageTypeConverter;
        private static string messageData;

        private static int logMessageType;
        private static string methodName;
        private static string message;
        private static string source;
        private static string user;
        private static DateTime messageDate;
        private static string machineName;
        private static string application;

        private Establish context = () =>
        {
            logMessageType = 1;
            methodName = "TestMethod";
            message = "TestMessage data";
            source = "TestSource";
            user = "somedomain\\\\someuser";
            messageDate = DateTime.Now;
            machineName = "ATestMachine";
            application = "ATestApplication";

            messageData = string.Format(@"{{
            ""$type"": ""SAHL.Shared.Messages.LogMessage, SAHL.Shared"",
            ""__identity"": null,
            ""LogMessageType"": {0},
            ""MethodName"": ""{1}"",
            ""Message"": ""{2}"",
            ""Source"": ""{3}"",
            ""User"": ""{4}"",
            ""MessageDate"": {5},
            ""MachineName"": ""{6}"",
            ""Application"": ""{7}"",
            ""Parameters"": {{
                }}
            }}", logMessageType, methodName, message, source, user, JsonConvert.SerializeObject(messageDate), machineName, application);

            logMessageTypeConverter = new LogMessageTypeConverter();
            messageWriter = An<ILogMessageWriter>();

            worker = new LogMessage_v2Worker(messageWriter, logMessageTypeConverter);
        };

        private Because of = () =>
        {
            worker.ProcessMessage(messageData);
        };

        private It should_try_write_the_message_to_storage_with_no_parameters = () =>
            {
                messageWriter.WasToldTo(x => x.WriteMessage(Arg.Is<LogMessageDataModel>(y =>
                     y.Application == application &&
                     y.LogMessageType == logMessageTypeConverter.ConvertLogMessageTypeToString(logMessageType) &&
                     y.MachineName == machineName &&
                     y.Message == message &&
                     y.MessageDate.Value == messageDate &&
                     y.MethodName == methodName &&
                     y.Source == source &&
                     y.UserName == user.Replace("\\\\", "\\")
                    ), Arg.Is<Dictionary<string,string>>(y=>y.Count == 0)));
            };
    }
}