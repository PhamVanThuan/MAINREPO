using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Managers
{
    public interface ILifeDomainDataManager
    {
        int LodgeDisabilityClaim(int lifeAccountKey, int claimantLegalEntityKey);

        void UpdateDisabilityClaimStatus(int disabilityClaimKey, DisabilityClaimStatus disabilityClaimStatus);

        void UpdateDisabilityClaimPaymentDates(int disabilityClaimKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate);

        void UpdateApprovedDisabilityClaim(int disabilityClaimKey, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation, DateTime? expectedReturnToWorkDate);

        string CreateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName);

        void DeleteDisabilityClaim(int disabilityClaimKey);

        IEnumerable<DisabilityClaimModel> GetDisabilityClaimsByAccount(int accountNumber);

        IEnumerable<DisabilityClaimDetailModel> GetDisabilityClaimHistory(int accountNumber);

        string GetDisabilityClaimInstanceSubject(int disabilityClaimKey);

        string GetDisabilityClaimStatusDescription(int disabilityClaimKey);

        DisabilityClaimDetailModel GetDisabilityClaimDetailByKey(int disabilityClaimKey);

        string TerminateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName);

        void UpdatePendingDisabilityClaim(int disabilityClaimKey, DateTime dateOfDiagnosis, int disabilityTypeKey, string otherDisabilityComments, string claimantOccupation,
            DateTime lastDateWorked, DateTime? expectedReturnToWorkDate);

        string GetEmailAddressForUserWhoApprovedDisabilityClaim(int disabilityClaimKey);
    }
}