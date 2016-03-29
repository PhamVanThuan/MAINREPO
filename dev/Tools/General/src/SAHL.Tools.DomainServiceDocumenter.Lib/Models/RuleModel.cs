using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class RuleModel
    {
        public RuleModel()
        {
            this.ParentCommands = new List<AssociateItemModel>();
        }

        public string DescriptionComment { get; set; }

        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string FullType { get; set; }

        public string Model { get; set; }

        public List<AssociateItemModel> ParentCommands { get; set; } 

    }
}
