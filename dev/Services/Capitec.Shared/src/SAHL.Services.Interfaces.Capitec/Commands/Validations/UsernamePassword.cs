using SAHL.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Commands.Validations
{
    public class UsernamePassword : IValidation
    {
        [MinLength(4, ErrorMessage = "Username Min Length is 4")]
        [MaxLength(20, ErrorMessage = "Username Max Length is 20")]
        public string Username { get; set; }

        [MinLength(0, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
