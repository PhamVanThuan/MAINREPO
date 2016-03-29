using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveDisabilityClaimCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public RemoveDisabilityClaimCommand(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}