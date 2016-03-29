using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Node
{
    public class X2EngineNode : IX2EngineNode
    {
        private IX2RequestSubscriber x2RequestSubscriber;
        private IX2EngineConfigurationProvider engineConfigurationProvider;
        private IX2ProcessProvider processProvider;
        private IX2ConsumerManager x2ConsumerManager;
        private IX2NodeManagementSubscriber nodeManagementSubscriber;

        public X2EngineNode(IX2RequestSubscriber x2RequestSubscriber, IX2EngineConfigurationProvider engineConfigurationProvider, 
            IX2ProcessProvider processProvider, IX2ConsumerManager x2ConsumerManager, IX2NodeManagementSubscriber nodeManagementSubscriber)
        {
            this.x2RequestSubscriber = x2RequestSubscriber;
            this.engineConfigurationProvider = engineConfigurationProvider;
            this.processProvider = processProvider;
            this.x2ConsumerManager = x2ConsumerManager;
            this.nodeManagementSubscriber = nodeManagementSubscriber;
        }

        public void Initialise()
        {
            processProvider.Initialise();
            if (!engineConfigurationProvider.PublishingNode())
            {
                x2RequestSubscriber.Initialise();
                x2ConsumerManager.Initialise();
                nodeManagementSubscriber.Initialise();

            }
        }

        public void Teardown()
        {
            x2ConsumerManager.TearDown();
            nodeManagementSubscriber.Teardown();
        }
    }
}