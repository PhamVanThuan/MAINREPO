using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicationConsultantDetails
    {
        public ApplicationConsultantDetails(string name, string branch, int? applicationNumber)
        {
            this.Name = name;
            this.Branch = branch;
            this.ApplicationNumber = applicationNumber;
        }

        [DataMember]
        public string Name { get; protected set; }

        [DataMember]
        public string Branch { get; protected set; }

        [DataMember]
        public int? ApplicationNumber { get; protected set; }
    }
}