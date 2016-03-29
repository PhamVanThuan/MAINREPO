using System;
using System.Collections.Generic;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class ModelModel
    {
        public ModelModel()
        {
            this.Properties = new List<Property>();
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

        public List<Property> Properties { get; protected set; }
    }
}