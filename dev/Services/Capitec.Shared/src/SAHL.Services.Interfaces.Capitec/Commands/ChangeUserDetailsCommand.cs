using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [ConstructorTestParams("rolesToAssign=6D9147A5-9980-4345-90C9-22D49F3C485F,6D9147A5-9980-4345-90C9-22D49F3C485F;rolesToRemove=6D9147A5-9980-4345-90C9-22D49F3C485F,6D9147A5-9980-4345-90C9-22D49F3C485F")]
    [AuthorisedCommand(Roles = "User")]
    public class ChangeUserDetailsCommand : ServiceCommand, ICapitecServiceCommand
    {
        [Required]
        public string EmailAddress { get; protected set; }

        [Required]
        public string FirstName { get; protected set; }

        [Required]
        public string LastName { get; protected set; }

        [Required]
        public bool Status { get; protected set; }

        [Required]
        public Guid[] RolesToAssign { get; protected set; }

        public Guid[] RolesToRemove { get; protected set; }

        [Required]
        public Guid BranchId { get; protected set; }

        public ChangeUserDetailsCommand(Guid id, string emailAddress, string firstName, string lastName, bool status, string rolesToAssign, string rolesToRemove, Guid branchId)
            : base(id)
        {
            this.EmailAddress = emailAddress;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Status = status;
            this.BranchId = branchId;
            this.RolesToAssign = string.IsNullOrEmpty(rolesToAssign) ? new Guid[0] : rolesToAssign.Split(',').Select(x => Guid.Parse(x)).ToArray();
            this.RolesToRemove = string.IsNullOrEmpty(rolesToRemove) ? new Guid[0] : rolesToRemove.Split(',').Select(x => Guid.Parse(x)).ToArray();
        }
    }
}