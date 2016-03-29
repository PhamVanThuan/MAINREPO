namespace SAHL.Core.BusinessModel.Enums
{
    public enum CorrespondenceTemplate
    {
        MortgageLoanCancelledContinuePaying = 1,
        MortgageLoanCancelledDontContinuePaying = 2,
        DeceasedNotificationNoLiving = 3,
        DeceasedNotificationLivingExists = 4,
        SequestrationNotificationNoOthers = 5,
        SequestrationNotificationOthersExist = 6,
        DeceasedEstatesDebtCounsellingArchived = 7,
        SAHLBankDetails = 8,
        RCSBankDetails = 9,
        FacilitationSendEmailAndSMS = 10,
        PersonalLoanDisbursementSMS = 11,
        EmailCorrespondenceGeneric = 12,
        EmailCorrespondenceSAHL = 13,
        EmailCorrespondenceHOC = 14,
        EmailCorrespondenceLife = 15,
        EmailCorrespondenceAttorney = 16,
        EmailCorrespondenceValuer = 17,
        EmailCorrespondenceInternal = 18,
        EmailCorrespondenceDebtcounselling = 19,
        EmailCorrespondenceDefault = 20,
        EmailCorrespondencePersonalLoan = 21,
        ValuationConsultantUpdateNotification = 22,
        HOCUpdateFailedOnValuationRequest = 23,
        CreditDecisionInternal = 24,
        NewClientConsultantDetailsSMS = 25,
        AlphaHousingSurvey = 26,
        NonAlphaHousingSurvey = 27,
        HaloException = 28,
        EmailCorrespondenceDisabilityClaimPack = 29,
        EmailCorrespondenceDisabilityClaimApprovedLetter = 30
    }
}