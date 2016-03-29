using SAHL.Core.Logging;

namespace SAHL.Core.Specs.Fakes
{
    public class FakeLoggerAppSource : ILoggerAppSource
    {
        public FakeLoggerAppSource(string appName)
        {
            this.ApplicationName = appName;
        }

        public string ApplicationName
        {
            get;
            protected set;
        }
    }
}