using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Core.Serialisation
{
    public static class JsonSerialisation
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                },
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeNameHandling       = TypeNameHandling.All,
            ConstructorHandling    = ConstructorHandling.AllowNonPublicDefaultConstructor
        };
    }
}