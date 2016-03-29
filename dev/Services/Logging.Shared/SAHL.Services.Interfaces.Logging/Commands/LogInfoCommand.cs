using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Logging.Commands
{
    public class LogInfoCommand : ServiceCommand, IServiceCommand
    {
        public string Source { get; protected set; }
        public string Message { get; protected set; }

        public LogInfoCommand(string source,string message)
        {
            this.Source = source;
            this.Message = message;
        }
    }
}
