using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ITC.Commands
{
    public class PerformCapitecITCCheckCommand : ServiceCommand, IITCServiceCommand
    {
        [Required]
        public Guid ItcID { get; protected set; }

        [Required]
        public ApplicantITCRequestDetailsModel ApplicantITCRequestDetails { get; protected set; }

        public PerformCapitecITCCheckCommand(Guid itcID, ApplicantITCRequestDetailsModel applicantITCRequestDetails)
        {
            this.ApplicantITCRequestDetails = applicantITCRequestDetails;
            this.ItcID = itcID;
        }
    }
}
