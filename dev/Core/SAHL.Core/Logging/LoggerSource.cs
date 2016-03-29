using SAHL.Core.Identity;

namespace SAHL.Core.Logging
{
    public class LoggerSource : ILoggerSource
    {
        public LoggerSource(string name, LogLevel logLevel, bool logMetrics)
        {
            this.Name = name;
            this.LogLevel = logLevel;
            this.Id = CombGuid.Instance.Generate();
            this.LogMetrics = logMetrics;
        }

        public string Name { get; protected set; }

        public LogLevel LogLevel { get; set; }

        public bool LogMetrics { get; set; }

        public System.Guid Id { get; protected set; }
    }
}