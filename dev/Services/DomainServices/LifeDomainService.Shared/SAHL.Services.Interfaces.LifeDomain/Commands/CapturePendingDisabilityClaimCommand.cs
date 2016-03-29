using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class CapturePendingDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand, IDisabilityClaimRuleModel, IRequiresPendingDisabilityClaim
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        [Required]
        public DateTime DateOfDiagnosis { get; protected set; }

        [Required]
        public int DisabilityTypeKey { get; protected set; }

        public string OtherDisabilityComments { get; protected set; }

        [Required]
        public string ClaimantOccupation { get; protected set; }

        [Required]
        public DateTime LastDateWorked { get; protected set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public CapturePendingDisabilityClaimCommand(int disabilityClaimKey, DateTime dateOfDiagnosis, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation, 
            DateTime lastDateWorked, DateTime? expectedReturnToWorkDate)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.DateOfDiagnosis = dateOfDiagnosis;
            this.DisabilityTypeKey = disabilityTypeKey;
            this.OtherDisabilityComments = otherDisabilityComments;
            this.ClaimantOccupation = claimantOccupation;
            this.LastDateWorked = lastDateWorked;
            this.ExpectedReturnToWorkDate = expectedReturnToWorkDate;
        }
    }
}
