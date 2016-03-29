using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class SetApplicationEmploymentTypeToUnknownCommand : ServiceCommand, IFrontEndTestCommand
    {
        public SetApplicationEmploymentTypeToUnknownCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public int ApplicationNumber { get; protected set; }
    }
}