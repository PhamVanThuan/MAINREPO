using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class PerformITCForApplicantCommand : ServiceCommand, ICapitecServiceInternalCommand
    {
        public Applicant Applicant { get; protected set; }
        public Guid ApplicantID { get; protected set; }

        public PerformITCForApplicantCommand(Guid applicantID, Applicant applicant)
        {
            this.Applicant = applicant;
            this.ApplicantID = applicantID;
        }
    }
}