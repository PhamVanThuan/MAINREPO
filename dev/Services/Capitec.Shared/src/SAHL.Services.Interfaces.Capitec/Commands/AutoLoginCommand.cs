using SAHL.Core.Services;
using SAHL.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    public class AutoLoginCommand : ServiceCommand, ICapitecServiceCommand, IValidation
    {
        public AutoLoginCommand(string cp, string branchCode, string password)
        {
            this.CP = cp;
            this.BranchCode = branchCode;
            this.Password = password;
        }

        [Required]
        public string CP { get; protected set; }

        [Required]
        public string BranchCode { get; protected set; }

        [Required]
        public string Password { get; protected set; }
    }
}
