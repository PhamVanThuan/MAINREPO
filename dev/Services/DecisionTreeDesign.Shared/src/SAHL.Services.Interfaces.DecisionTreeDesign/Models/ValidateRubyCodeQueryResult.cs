using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class ValidateRubyCodeQueryResult
    {
        public bool Valid { get; set; }

        public string Message { get; set; }

        public string ErrorString { get; set; }
    }
}
