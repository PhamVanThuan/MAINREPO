using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class SetHouseholdIncomeToZeroCommand : ServiceCommand, IFrontEndTestCommand
    {
        public SetHouseholdIncomeToZeroCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        [Required]
        public int ApplicationNumber { get; protected set; }
    }
}