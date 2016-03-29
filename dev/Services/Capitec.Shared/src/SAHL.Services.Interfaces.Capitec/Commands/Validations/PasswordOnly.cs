using SAHL.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Commands.Validations
{
    public class PasswordOnly : IValidation
    {
        [MinLength(0, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }

        [MinLength(0, ErrorMessage = "Confirm cannot be empty")]
        public string Passwordconfirm { get; set; }
    }
}
