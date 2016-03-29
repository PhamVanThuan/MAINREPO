namespace SAHL.X2Engine2.MessageHandlers
{
    public interface IX2MessageHandler<TMessage>
    {
        void Initialise();

        void HandleCommand(TMessage message);
    }
}