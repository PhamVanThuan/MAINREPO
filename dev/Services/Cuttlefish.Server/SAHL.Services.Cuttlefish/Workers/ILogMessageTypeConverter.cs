namespace SAHL.Services.Cuttlefish.Workers
{
    public interface ILogMessageTypeConverter
    {
        string ConvertLogMessageTypeToString(int logMessageType);
    }
}