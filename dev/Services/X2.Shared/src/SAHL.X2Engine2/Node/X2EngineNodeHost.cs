namespace SAHL.X2Engine2.Node
{
    public class X2EngineNodeHost : IX2EngineNodeHost
    {
        private readonly IX2EngineNode x2EngineNode;

        public X2EngineNodeHost(IX2EngineNode x2EngineNode)
        {
            this.x2EngineNode = x2EngineNode;
        }

        public void Start()
        {
            x2EngineNode.Initialise();
        }

        public void Stop()
        {
            x2EngineNode.Teardown();
        }
    }
}