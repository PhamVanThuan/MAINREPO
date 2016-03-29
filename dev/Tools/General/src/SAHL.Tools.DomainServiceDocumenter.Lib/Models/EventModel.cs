using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class EventModel
    {
        public EventModel()
        {
            this.Properties = new List<Property>();
            this.ParentCommands = new List<AssociateItemModel>();
        }

        public string DescriptionComment { get; set; }

        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string FullType { get; set; }

        public List<Property> Properties { get; protected set; }

        public List<AssociateItemModel> ParentCommands { get; set; } 

    }
}
