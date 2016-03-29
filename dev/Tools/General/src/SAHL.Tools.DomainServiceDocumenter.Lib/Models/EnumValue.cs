using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class EnumValue
    {
        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public int Value { get; set; }
    }
}