using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class AddNewBranchCommand : ServiceCommand, ICapitecServiceCommand
    {
        public AddNewBranchCommand(string branchName, bool isActive, Guid suburbId, string branchCode)
        {
            this.BranchName = branchName;
            this.IsActive = isActive;
            this.SuburbId = suburbId;
            this.BranchCode = branchCode;
        }

        [Required]
        public string BranchName { get; protected set; }

        [Required]
        public bool IsActive { get; protected set; }

        [Required]
        public Guid SuburbId { get; protected set; }

        [Required]
        public string BranchCode { get;protected set; }
    }
}