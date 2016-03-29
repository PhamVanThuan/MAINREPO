using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class ExternalActivityCommand : ServiceCommand, IContinueWithCommands
    {
        public int ExternalActivityID { get; set; }

        public int WorkFlowID { get; set; }

        public long? ActivatingInstanceID { get; set; }

        public DateTime ActivationTime { get; set; }

        public Dictionary<string, string> MapVariables { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public ExternalActivityCommand(int externalActivityID, int workFlowID, long? activatingInstanceID, DateTime activationTime, Dictionary<string, string> mapVariables, object data = null)
        {
            this.ExternalActivityID = externalActivityID;
            this.WorkFlowID = workFlowID;
            this.ActivatingInstanceID = activatingInstanceID;
            this.ActivationTime = activationTime;
            this.MapVariables = mapVariables;
            this.IgnoreWarnings = true;
            this.Data = data;
        }
    }
}