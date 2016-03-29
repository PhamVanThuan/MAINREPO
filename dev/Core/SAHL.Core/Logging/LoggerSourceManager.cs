using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Logging
{
    public class LoggerSourceManager : ILoggerSourceManager
    {
        private readonly ConcurrentDictionary<Guid, ILoggerSource> loggerSources;

        public LoggerSourceManager(IEnumerable<ILoggerSource> loggerSources)
        {
            this.loggerSources = new ConcurrentDictionary<Guid, ILoggerSource>();
            foreach (var loggerSource in loggerSources)
            {
                this.RegisterSource(loggerSource);
            }
        }

        public void RegisterSource(ILoggerSource loggerSource)
        {
            if (this.loggerSources.ContainsKey(loggerSource.Id))
            {
                return;
            }
            if (this.loggerSources.All(x => x.Value.Name != loggerSource.Name))
            {
                this.loggerSources.TryAdd(loggerSource.Id, loggerSource);
            }
            else
            {
                throw new ArgumentException(string.Format("A logging source with name: {0} has already been registered.", loggerSource.Name), "loggersource");
            }
        }

        public void UnregisterSource(ILoggerSource loggerSource)
        {
            if (!this.loggerSources.ContainsKey(loggerSource.Id))
            {
                return;
            }
            ILoggerSource removedLoggerSource;
            this.loggerSources.TryRemove(loggerSource.Id, out removedLoggerSource);
        }

        public IDictionary<Guid, ILoggerSource> AvailableSources
        {
            get { return this.loggerSources; }
        }
    }
}