using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicantEmploymentDetails
    {
        public ApplicantEmploymentDetails(int employmentTypeKey, EmploymentDetails employmentDetails)
        {
            this.EmploymentTypeKey = employmentTypeKey;
            this.Employment = employmentDetails;
        }

        [DataMember]
        public int EmploymentTypeKey { get; protected set; }

        [DataMember]
        public EmploymentDetails Employment { get; protected set; }
    }
}