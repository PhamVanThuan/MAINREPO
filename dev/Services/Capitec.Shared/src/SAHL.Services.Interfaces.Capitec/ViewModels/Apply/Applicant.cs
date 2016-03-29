using System;
using System.Xml.Linq;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class Applicant
    {
        public Applicant(ApplicantInformation information, ApplicantResidentialAddress residentialAddress, ApplicantEmploymentDetails employmentDetails, ApplicantDeclarations declarations)
        {
            this.Information = information;
            this.ResidentialAddress = residentialAddress;
            this.EmploymentDetails = employmentDetails;
            this.Declarations = declarations;
        }

        public ApplicantInformation Information
        { get; protected set; }

        public ApplicantResidentialAddress ResidentialAddress
        { get; protected set; }

        public ApplicantEmploymentDetails EmploymentDetails
        { get; protected set; }

        public ApplicantDeclarations Declarations
        { get; protected set; }

        public XDocument ITCRequest
        { get; set; }

        public XDocument ITCResponse
        { get; set; }

        public DateTime ITCDate
        { get; set; }

        public int EmpiricaScore
        { get; set; }
    }
}