using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
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

		public T Deserialize<T>(string serializedObject)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serializedObject, serializerSettings);
		}
	}
}
