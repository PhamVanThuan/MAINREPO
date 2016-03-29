namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Waits on x2 requests for a node.
    /// </summary> 
    public interface IX2RequestSubscriber : IX2ConsumerConfigurationProvider
    {
        void Initialise();
    }
}