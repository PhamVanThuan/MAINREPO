
using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.ITC.Commands
{
    public class PerformClientITCCheckCommand : ServiceCommand, IITCServiceCommand
    {
        [Required]
        public string IdNumber { get; protected set; }

        public int? AccountKey { get; protected set; }

        [Required]
        public string UserId { get; protected set; }

        public PerformClientITCCheckCommand(string IdNumber, int? accountKey, string userId)
        {
            this.IdNumber = IdNumber;
            this.AccountKey = accountKey;
            this.UserId = userId;
        }
    }
}
