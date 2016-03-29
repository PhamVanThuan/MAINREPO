using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ConsultantDetails
    {
        public ConsultantDetails(string name, string branch)
        {
            this.Name = name;
            this.Branch = branch;
        }

        [DataMember]
        public string Name { get; protected set; }

        [DataMember]
        public string Branch { get; protected set; }
    }
}