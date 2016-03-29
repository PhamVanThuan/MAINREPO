using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
using SAHL.Core.Web.Services;

using DomainService2V3  = DomainService2.V3.Client.Commands;
using V3Framework       = SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.V3.Framework.Test.CommandSerialisation
{ 
    [TestFixture]
    public class ConfirmAffordabilityAssessments
    {
        // COPIED FROM  SAHL.Core.Web.Services.JsonActivator
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            ContractResolver = new DefaultContractResolver
            {
                DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance
            },
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Error = (error, eventargs) => { }
        };

        [Test]
        public void Test_DomainService_ConfirmAffordabilityAssessmentsCommandJSON()
        {
            int applicationKey = 9000;
            string userKey = @"SAHL\BCUser3";

            DomainService2V3.ConfirmAffordabilityAssessmentsCommand domainServiceCommand = new DomainService2V3.ConfirmAffordabilityAssessmentsCommand(applicationKey, userKey);
            string domainServiceCommandJSON = domainServiceCommand.ToJSON();
            JObject domainServiceJSONObject = JObject.Parse(domainServiceCommandJSON);

            JsonActivator jsonActivator = new JsonActivator();

            V3Framework.ConfirmApplicationAffordabilityAssessmentsCommand v3command = new V3Framework.ConfirmApplicationAffordabilityAssessmentsCommand(applicationKey);
            string v3commandJSON = jsonActivator.SerializeObject(v3command, jsonSettings);
            JObject v3CommandJSONObject = JObject.Parse(v3commandJSON);

            foreach(var property in v3CommandJSONObject.Properties())
            {
                if (domainServiceJSONObject[property.Name] == null)
                {
                    Assert.Fail(string.Format("Property Missing from DomainService2.V3.Client.Commands.ConfirmAffordabilityAssessmentsCommand: {0}", property.Name));
                }
            }
            Assert.Pass();
        }
    }
}
