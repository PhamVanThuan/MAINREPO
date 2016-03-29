namespace SAHL.Services.Cuttlefish.Workers
{
    public class LogMessageTypeConverter : ILogMessageTypeConverter
    {
        public string ConvertLogMessageTypeToString(int logMessageType)
        {
            switch (logMessageType)
            {
                case 1:
                    return "Error";

                case 2:
                    return "Warning";

                case 3:
                    return "Info";

                case 4:
                    return "Debug";

                default:
                    return "Unknown";
            }
        }
    }
}