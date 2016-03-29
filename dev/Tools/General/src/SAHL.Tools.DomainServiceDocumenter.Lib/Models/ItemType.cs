using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class ItemType
    {
        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string FullName { get; set; }

        public string ClearFullName
        {
            get { return Sentencer.ToSentenceFromTypeName(FullName); }
        }

        public string AssemblyQualifiedName { get; set; }

        public bool IsComplex { get; set; }

        public bool IsEnum { get; set; }
    }
}