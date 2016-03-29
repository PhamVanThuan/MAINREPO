using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.Services.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ChangePasswordCommand : ServiceCommand, ICapitecServiceCommand, IValidation
    {
        public ChangePasswordCommand(string password, string passwordconfirm)
        {
            this.Password = password;
            this.Passwordconfirm = passwordconfirm;
        }

        [Required(ErrorMessage="Please provide a new password")]
        public string Password { get; protected set; }

        [Required(ErrorMessage="Please confirm your new password")]
        public string Passwordconfirm { get; protected set; }
    }
}