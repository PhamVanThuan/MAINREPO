using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class Applicant
    {
        public Applicant(ApplicantInformation information, ApplicantResidentialAddress residentialAddress, ApplicantEmploymentDetails employmentDetails, ApplicantDeclarations declarations, ApplicantITC itc)
        {
            this.Information = information;
            this.ResidentialAddress = residentialAddress;
            this.EmploymentDetails = employmentDetails;
            this.Declarations = declarations;
            this.ITC = itc;
        }

        [DataMember]
        public ApplicantInformation Information { get; protected set; }

        [DataMember]
        public ApplicantResidentialAddress ResidentialAddress { get; protected set; }

        [DataMember]
        public ApplicantEmploymentDetails EmploymentDetails { get; protected set; }

        [DataMember]
        public ApplicantDeclarations Declarations { get; protected set; }

        [DataMember]
        public ApplicantITC ITC { get; protected set; }
    }
}