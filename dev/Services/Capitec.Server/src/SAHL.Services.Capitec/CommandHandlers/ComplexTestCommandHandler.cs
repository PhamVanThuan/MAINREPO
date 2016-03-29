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
    public class ComplexTestCommandHandler : IServiceCommandHandler<ComplexTestCommand>
    {
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ComplexTestCommand command)
        {
            return SystemMessageCollection.Empty();
        }
    }
}
