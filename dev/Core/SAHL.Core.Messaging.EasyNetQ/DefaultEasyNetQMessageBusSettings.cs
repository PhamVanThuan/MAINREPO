using EasyNetQ;
using EasyNetQ.Loggers;
using Newtonsoft.Json;
using SAHL.Core.Serialisation;
using System.Text;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public class DefaultEasyNetQMessageBusSettings : IEasyNetQMessageBusSettings
    {
        public void RegisterServices(global::EasyNetQ.IServiceRegister serviceRegister)
        {
            serviceRegister.Register<ISerializer>(y => { return new SAHL.Core.Messaging.EasyNetQ.JsonSerializer(); });
            serviceRegister.Register<IEasyNetQLogger>(y => { return new ConsoleLogger(); });
        }
    }

    public class JsonSerializer : ISerializer
    {
        public byte[] MessageToBytes<T>(T message) where T : class
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, JsonSerialisation.Settings));
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), JsonSerialisation.Settings);
        }

        public object BytesToMessage(string typeName, byte[] bytes)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), JsonSerialisation.Settings);
        }
    }
}