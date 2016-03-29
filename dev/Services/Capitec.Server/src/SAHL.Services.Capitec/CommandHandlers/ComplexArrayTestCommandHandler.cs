using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ComplexArrayTestCommandHandler : IServiceCommandHandler<ComplexArrayTestCommand>
    {
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ComplexArrayTestCommand command)
        {
            return SystemMessageCollection.Empty();
        }
    }
}
