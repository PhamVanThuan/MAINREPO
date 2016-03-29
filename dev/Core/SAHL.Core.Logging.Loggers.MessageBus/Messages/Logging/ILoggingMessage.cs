namespace SAHL.Core.Logging.Messages
{
    public interface ILoggingMessage : IBaseMessage
    {
        string Message { get; }

        string LogLevel { get; }
    }
}