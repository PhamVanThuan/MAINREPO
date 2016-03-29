namespace SAHL.Common.Collections.Interfaces
{
    public enum DomainMessageType
    {
        Error,
        Warning,
        Info
    }

    public interface IDomainMessage
    {
        string Message { get; }

        string Details { get; }

        DomainMessageType MessageType { get; }
    }
}