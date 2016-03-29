using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveApplicationFromOpenNewBusinessApplicationsCommand : ServiceCommand, IFrontEndTestCommand
    {
        public RemoveApplicationFromOpenNewBusinessApplicationsCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public int ApplicationNumber { get; protected set; }
    }
}