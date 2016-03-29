using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Services;
using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Core.Web.Services
{
    public class JsonActivator : IJsonActivator
    {
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling           = TypeNameHandling.All,
            ReferenceLoopHandling      = ReferenceLoopHandling.Serialize,
            TypeNameAssemblyFormat     = FormatterAssemblyStyle.Full,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            DateTimeZoneHandling       = DateTimeZoneHandling.Local,
            ContractResolver           = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance
                },
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Error = (error, eventargs) => { }
        };

        public string SerializeObject<T>(T model, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(model, settings ?? jsonSettings).Replace("$type", "_name");
        }

        public IServiceQuery DeserializeQuery(string jsonString)
        {
            try
            {
                return this.DeserializeObject<IServiceQuery>(jsonString);
            }
            catch
            {
                return null;
            }
        }

        public IServiceCommand DeserializeCommand(string jsonString)
        {
            try
            {
                IServiceCommand serviceCommand = this.DeserializeObject<IServiceCommand>(jsonString);
                //json.net deserialize workaround: set the protected Id property on the command
                var jObject = JObject.Parse(jsonString);
                if (jObject.Property("Id") != null)
                {
                    serviceCommand.GetType().GetProperty("Id").SetValue(serviceCommand, new Guid(JObject.Parse(jsonString).Property("Id").Value.ToString()));
                }
                return serviceCommand;
            }
            catch
            {
                return null;
            }
        }

        public T DeserializeObject<T>(string jsonString)
        {
            try
            {
                var jsonStringWithTypeVariable = jsonString.Replace("_name", "$type");
                var jObject = JObject.Parse(jsonStringWithTypeVariable);

                return (T)JsonConvert.DeserializeObject(jsonStringWithTypeVariable, Type.GetType(jObject["$type"].ToString()), this.jsonSettings);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public object DeserializeObject(string jsonString)
        {
            try
            {
                var jsonStringWithTypeVariable = jsonString.Replace("_name", "$type");
                var jObject = JObject.Parse(jsonStringWithTypeVariable);

                return JsonConvert.DeserializeObject(jsonStringWithTypeVariable, Type.GetType(jObject["$type"].ToString()), this.jsonSettings);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}