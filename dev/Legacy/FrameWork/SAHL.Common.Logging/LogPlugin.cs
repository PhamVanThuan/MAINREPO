namespace SAHL.Common.Logging
{
    public static class LogPlugin
    {
        private static ILogger logger;

        public static ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = new DefaultLogger();
                }
                return logger;
            }
            set
            {
                if (value == null)
                {
                    logger = new DefaultLogger();
                }
                else
                {
                    logger = new SafeLoggingWrapper(value);
                }
            }
        }
    }
}