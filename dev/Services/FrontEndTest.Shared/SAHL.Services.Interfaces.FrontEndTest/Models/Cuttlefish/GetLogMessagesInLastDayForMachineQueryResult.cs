using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish
{
    public class GetLogMessagesInLastDayForMachineQueryResult
    {
        public int Id { get; set; }

        public string Application { get; set; }

        public string LogMessageType { get; set; }

        public string MethodName { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public DateTime MessageDate { get; set; }

        public GetLogMessagesInLastDayForMachineQueryResult(int id, DateTime messageDate, string application, string logMessageType, string methodName, string message, string source, string userName)
        {
            this.Id = id;
            this.MessageDate = messageDate;
            this.Application = application;
            this.LogMessageType = logMessageType;
            this.MethodName = methodName;
            this.Message = message;
            this.Source = source;
            this.UserName = userName;
        }
    }
}