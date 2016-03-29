using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SAHL.X2Engine2.Providers
{
    public class JsonSerializationProvider : ISerializationProvider
    {
        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings
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

        public string Serialize<T>(T instanceToSerialize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(instanceToSerialize, serializerSettings);
        }

        public T Deserialize<T>(string serializedObject)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serializedObject, serializerSettings);
        }

        public byte[] MessageToBytes<T>(T message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, serializerSettings));
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), serializerSettings);
        }
    }
}