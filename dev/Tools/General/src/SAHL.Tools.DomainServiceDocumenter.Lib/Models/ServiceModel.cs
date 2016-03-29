using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.Stringifier;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Models
{
    public class ServiceModel
    {
        public ServiceModel(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }

        public string Name { get; set; }

        public string ClearName
        {
            get { return Sentencer.ToSentenceFromTypeName(Name); }
        }

        public string Version { get; set; }
    }
}