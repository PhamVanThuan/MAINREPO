using Newtonsoft.Json;
using SAHL.Core.Services;

namespace SAHL.Core.Web.Services
{
    public interface IJsonActivator
    {
        IServiceQuery DeserializeQuery(string jsonString);

        IServiceCommand DeserializeCommand(string jsonString);

        T DeserializeObject<T>(string jsonString);

        object DeserializeObject(string jsonString);

        string SerializeObject<T>(T model, JsonSerializerSettings settings = null);
    }
}