using SAHL.Core.Services;
using SAHL.Core.Services.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    public class LoginCommand : ServiceCommand, ICapitecServiceCommand, IValidation
    {
        public LoginCommand(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        [Required]
        public string Username { get; protected set; }

        [Required]
        public string Password { get; protected set; }
    }
}