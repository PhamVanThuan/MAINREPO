using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using EasyNetQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace SAHL.Communication
{
	public class JsonSerializer : ISerializer
	{
        TypeNameSerializer typeNameSerializer;
        public JsonSerializer(TypeNameSerializer typeNameSerializer)
        {
            this.typeNameSerializer = typeNameSerializer;
        }
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
            var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), serializerSettings);
            return result;
		}

		public object BytesToMessage(string typeName, byte[] bytes)
		{
            var type =typeNameSerializer.DeSerialize(typeName);

            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), type, serializerSettings);
		}
	}
}