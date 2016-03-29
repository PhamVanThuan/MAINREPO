namespace SAHL.Shared.Messages
{
    public interface ILogMessage : IMessage
    {
        LogMessageType LogMessageType { get; }

        string MethodName { get; }

        string Message { get; }
    }
}