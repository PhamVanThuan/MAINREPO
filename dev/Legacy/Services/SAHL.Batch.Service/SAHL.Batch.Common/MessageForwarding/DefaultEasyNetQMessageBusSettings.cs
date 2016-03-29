using EasyNetQ;
using EasyNetQ.Loggers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common.MessageForwarding
{
    public class DefaultEasyNetQMessageBusSettings
    {
        public void RegisterServices(global::EasyNetQ.IServiceRegister serviceRegister)
        {
            serviceRegister.Register<ISerializer>(y => { return new SAHL.Batch.Common.MessageForwarding.JsonSerializer(); });
            serviceRegister.Register<IEasyNetQLogger>(y => { return new ConsoleLogger(); });
        }
    }

    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            },
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public byte[] MessageToBytes<T>(T message) where T : class
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, serializerSettings));
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), serializerSettings);
        }

        public object BytesToMessage(string typeName, byte[] bytes)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), serializerSettings);
        }
    }
}
