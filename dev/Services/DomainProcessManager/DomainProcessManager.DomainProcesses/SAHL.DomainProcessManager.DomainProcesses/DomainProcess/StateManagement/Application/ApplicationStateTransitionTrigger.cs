namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public enum ApplicationStateTransitionTrigger
    {
        ValidApplicationCreationConfirmed,
        BasicApplicationCreationConfirmed,
        ApplicantAdditionConfirmed,
        EmploymentAdditionConfirmed,
        ApplicationEmploymentDeterminationConfirmed,
        ApplicationHouseHoldIncomeDeterminationConfirmed,

        ApplicationPricingConfirmed,
        ApplicationFundingConfirmed,
        ApplicationLinkingToExternalVendorConfirmed,
        CriticalErrorReported,

        AddressCaptureConfirmed,
        BankAccountCaptureConfirmed,
        AllAddressesCaptured,
        AllBankAccountsCaptured,

        ApplicationDebitOrderCaptureConfirmed,
        ApplicationMailingAddressCaptureConfirmed,
        ComcorpPropertyCaptureConfirmed,

        AffordabilityDetailCaptureConfirmed,
        DeclarationsCaptureConfirmed,
        ClientPendingDomiciliumCaptureConfirmed,
        DomiciliumAddressCaptureConfirmed,

        AssetLiabilityDetailCapturedConfirmed,

        X2CaseCreationConfirmed,

        CompletionConfirmed,

        NonCriticalErrorReported,
    }
}