using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class UpdateApprovedDisabilityClaimStatement :  ISqlStatement<int>
    {
        public int DisabilityClaimKey { get; protected set; }
        public int DisabilityTypeKey { get; protected set; }
        public string OtherDisabilityComments { get; protected set; }
        public string ClaimantOccupation { get; protected set; }
        public DateTime? ExpectedReturnToWorkDate { get; protected set; }

        public UpdateApprovedDisabilityClaimStatement(int disabilityClaimKey, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation, DateTime? expectedReturnToWorkDate)
        {
            DisabilityClaimKey = disabilityClaimKey;
            DisabilityTypeKey = disabilityTypeKey;
            OtherDisabilityComments = otherDisabilityComments;
            ClaimantOccupation = claimantOccupation;
            ExpectedReturnToWorkDate = expectedReturnToWorkDate;
        }

        public string GetStatement()
        {
            return @"update [2am].[dbo].DisabilityClaim set DisabilityTypeKey = @DisabilityTypeKey, OtherDisabilityComments = @OtherDisabilityComments, ClaimantOccupation = @ClaimantOccupation,
                        ExpectedReturnToWorkDate = @ExpectedReturnToWorkDate where DisabilityClaimKey = @DisabilityClaimKey";            
        }
    }
}