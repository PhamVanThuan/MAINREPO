using Newtonsoft.Json.Linq;
using WebApi.Hal;

namespace SAHL.Services.Query.Serialiser
{
    public interface IHalSerialiser
    {
        string Serialise(Representation item, string acceptHeader = null);
    }
}