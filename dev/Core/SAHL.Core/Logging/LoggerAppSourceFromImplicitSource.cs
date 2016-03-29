namespace SAHL.Core.Logging
{
    public class LoggerAppSourceFromImplicitSource : ILoggerAppSource
    {
        private string applicationName;

        public LoggerAppSourceFromImplicitSource(string implicitApplicationSourceName)
        {
            this.applicationName = implicitApplicationSourceName;
        }

        public string ApplicationName
        {
            get { return this.applicationName; }
        }
    }
}