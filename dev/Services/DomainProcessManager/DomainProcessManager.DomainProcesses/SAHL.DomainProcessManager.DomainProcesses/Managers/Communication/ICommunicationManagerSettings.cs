namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Communication
{
    public interface ICommunicationManagerSettings
    {
        string CamTeamEmailAddress { get; }
        string ITFrontEndTeamEmailAddress { get; }
        string AttorneyInvoiceFailuresEmailAddress { get; }
        string ThirdPartyInvoiceProcessorEmailAddress { get; }
    }
}