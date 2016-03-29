using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class UpdatePendingDisabilityClaimStatement : ISqlStatement<int>
    {
        public UpdatePendingDisabilityClaimStatement(int disabilityClaimKey, DateTime dateOfDiagnosis, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation,
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

        public int DisabilityClaimKey { get; protected set; }
        
        public DateTime DateOfDiagnosis { get; protected set; }

        public int DisabilityTypeKey { get; protected set; }

        public string OtherDisabilityComments { get; protected set; }

        public string ClaimantOccupation { get; protected set; }

        public DateTime LastDateWorked { get; protected set; }

        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public string GetStatement()
        {
            return @"update [2am].[dbo].DisabilityClaim 
                    set DateOfDiagnosis = @DateOfDiagnosis, DisabilityTypeKey = @DisabilityTypeKey, OtherDisabilityComments = @OtherDisabilityComments,
                    ClaimantOccupation = @ClaimantOccupation, LastDateWorked = @LastDateWorked, ExpectedReturnToWorkDate = @ExpectedReturnToWorkDate
                    where DisabilityClaimKey = @DisabilityClaimKey";            
        }
    }
}