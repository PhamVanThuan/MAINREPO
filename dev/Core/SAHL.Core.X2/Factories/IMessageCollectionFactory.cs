using SAHL.Core.SystemMessages;

namespace SAHL.Core.X2.Factories
{
    public interface IMessageCollectionFactory
    {
        ISystemMessageCollection CreateEmptyCollection();
    }
}