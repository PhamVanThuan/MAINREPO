namespace SAHL.Core.SystemMessages
{
    public interface ISystemMessage
    {
        string Message { get; }

        SystemMessageSeverityEnum Severity { get; }
    }
}