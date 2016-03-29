using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DomainService2.V3.Client.Commands
{
    public class ConfirmAffordabilityAssessmentsCommand : IV3Command
    {
        public int ApplicationKey { get; protected set; }

        public string ADUserName { get; protected set; }

        public ConfirmAffordabilityAssessmentsCommand(int applicationKey, string adUserName)
        {
            this.ApplicationKey = applicationKey;
            this.ADUserName = adUserName;
        }

        public string ToJSON()
        {
            var obj = new JObject();
            obj.Add("$id", "1");
            obj.Add("_name", "SAHL.Services.Interfaces.ApplicationDomain.Commands.ConfirmApplicationAffordabilityAssessmentsCommand, SAHL.Services.Interfaces.ApplicationDomain");
            obj.Add("ApplicationKey", this.ApplicationKey);
            obj.Add("Id", CombGuid.Instance.GenerateString());
            return obj.ToString();
        }
    }
}
