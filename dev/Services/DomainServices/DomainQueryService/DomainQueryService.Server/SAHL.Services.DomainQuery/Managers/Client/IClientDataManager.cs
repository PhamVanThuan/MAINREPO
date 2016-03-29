namespace SAHL.Services.DomainQuery.Managers.Client
{
    public interface IClientDataManager
    {
        bool IsClientOnOurSystem(int clientKey);

        bool IsClientANaturalPerson(int clientKey);
    }
}