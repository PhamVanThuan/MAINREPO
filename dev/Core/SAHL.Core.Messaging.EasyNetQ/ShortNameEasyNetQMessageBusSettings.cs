using EasyNetQ;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public class ShortNameEasyNetQMessageBusSettings : IEasyNetQMessageBusSettings
    {
        public void RegisterServices(global::EasyNetQ.IServiceRegister serviceRegister)
        {
            serviceRegister.Register<ISerializer>(y => { return new SAHL.Core.Messaging.EasyNetQ.JsonSerializer(); });
            serviceRegister.Register<ITypeNameSerializer>(y => { return new SAHL.Core.Messaging.EasyNetQ.ShortNameSerializer(); });
        }
    }
}