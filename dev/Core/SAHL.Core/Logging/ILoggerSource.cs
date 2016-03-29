using System;

namespace SAHL.Core.Logging
{
    public interface ILoggerSource
    {
        Guid Id { get; }

        string Name { get; }

        LogLevel LogLevel { get; set; }

        bool LogMetrics { get; set; }
    }
}