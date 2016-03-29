using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Factories;

namespace SAHL.X2Engine2.Factories
{
    public class MessageCollectionFactory : IMessageCollectionFactory
    {
        public ISystemMessageCollection CreateEmptyCollection()
        {
            return new MessageCollection();
        }
    }
}