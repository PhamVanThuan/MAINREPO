using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class ValidationModel
    {
        public ValidationModel()
        {
            this.ErrorMessage = "";
        }

        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string Type { get; set; }

        public string FullType { get; set; }

        public string ErrorMessage { get; set; }
    }
}
