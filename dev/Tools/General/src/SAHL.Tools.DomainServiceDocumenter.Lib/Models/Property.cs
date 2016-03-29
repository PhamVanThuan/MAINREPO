using System.Collections.Generic;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class Property
    {
        
        public Property()
        {
            this.Properties = new List<Property>();
            this.Validations = new List<ValidationModel>();
        }

        public string DescriptionComment { get; set; }

        public string Name { get; set; }

        public string ClearName { get { return Sentencer.ToSentenceFromTypeName(Name); } }

        public string Type { get; set; }

        public string ClearType { get { return Sentencer.ToSentenceFromTypeName(Type); } }

        public string FullType { get; set; }

        public bool IsSAHLModel { get; set; }

        public bool IsCollection { get; set; }
        
        public string CollectionPropertyType { get; set; }

        public bool IsCollectionSAHLModel { get; set; }

        public bool IsSAHLEnum;

        public List<ValidationModel> Validations { get; protected set; }

        public List<Property> Properties { get; protected set; }
    }
}