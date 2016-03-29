using System;
using System.Collections.Generic;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class CommandModel
    {

        public CommandModel()
        {
            this.Properties = new List<Property>();
            this.Rules = new List<RuleModel>();
            this.Checks = new List<DomainCheckModel>();
            this.Id = Guid.NewGuid().ToString();
       }

        public string Id { get; protected set; }

        public string DescriptionComment { get; set; }

        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string FullType { get; set; }

        public EventModel RaisedEvent { get; set; }

        public List<Property> Properties { get; protected set; }

        public List<RuleModel> Rules { get; protected set; }

        public List<DomainCheckModel> Checks { get; protected set; }
    }
}