using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Logging
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
