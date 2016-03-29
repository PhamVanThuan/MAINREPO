using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class ValidateRubyCodeQuery : ServiceQuery<ValidateRubyCodeQueryResult>
    {
        public ValidateRubyCodeQuery(string rubyCode, string enumJson, string messages, string localVars, string globalVars)
        {
            this.RubyCode = rubyCode;
            this.Enums = enumJson;
            this.Messages = messages;
            this.LocalVariables = localVars;
            this.GlobalVariables = globalVars;
        }

        public string Enums { get; set; }

        public string RubyCode { get; set; }

        public string Messages { get; set; }

        public string LocalVariables { get; set; }

        public string GlobalVariables { get; set; }
    }
}
