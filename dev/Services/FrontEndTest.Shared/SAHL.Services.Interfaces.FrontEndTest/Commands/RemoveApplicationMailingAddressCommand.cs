using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveApplicationMailingAddressCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public int ApplicationNumber { get; protected set; }

        public RemoveApplicationMailingAddressCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}