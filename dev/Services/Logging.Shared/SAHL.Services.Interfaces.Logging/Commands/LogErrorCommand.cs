using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Logging.Commands
{
    public class LogErrorCommand : ServiceCommand, IServiceCommand
    {
        public string Source { get; protected set; }
        public string Message { get; protected set; }
        public string StackTrace { get; protected set; }

        public LogErrorCommand(string source, string message, string stackTrace)
        {
            this.Source = source;
            this.Message = message;
            this.StackTrace = stackTrace;
        }
    }
}
