namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public enum ApplicationState
    {
        Processing,
        NonTrackedState,

        ValidMinimumApplicationCreated,
        BasicApplicationCreated,
        ApplicantAdded,
        EmploymentAdded,
        ApplicationEmploymentDetermined,
        ApplicationHouseHoldIncomeDetermined,
        ApplicationPriced,
        ApplicationFunded,
        ApplicationLinkedToExternalVendor,
        CriticalErrorOccured,

        AllAddressesCaptured,
        AddressCaptured,
        ApplicationMailingAddressCaptured,
        DomiciliumAddressCaptured,
        ClientPendingDomiciliumCaptured,
        AssetLiabilityDetailCaptured,

        AllBankAccountsCaptured,
        BankAccountCaptured,
        ApplicationDebitOrderCaptured,

        AffordabilityDetailCaptured,
        DeclarationsCaptured,

        X2CaseCreated,

        NonCriticalErrorOccured,

        Completed,
        CriticalPath,
        NonCriticalPath,
        BankAccountStates,
        AddressStates,
        StandaloneNonCriticalStates
    }
}