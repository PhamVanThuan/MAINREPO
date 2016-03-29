using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ChangeBranchDetailsCommand : ServiceCommand, ICapitecServiceCommand
    {
        public ChangeBranchDetailsCommand(Guid id, string branchName, bool isActive, Guid suburbId)
            : base(id)
        {
            this.BranchName = branchName;
            this.IsActive = isActive;
            this.SuburbId = suburbId;
        }

        [Required]
        public string BranchName { get; protected set; }

        [Required]
        public bool IsActive { get; protected set; }

        [Required]
        public Guid SuburbId { get; protected set; }
    }
}