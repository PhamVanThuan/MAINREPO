using EasyNetQ;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public interface IEasyNetQMessageBusSettings
    {
        void RegisterServices(IServiceRegister serviceRegister);
    }
}