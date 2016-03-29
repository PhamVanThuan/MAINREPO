
namespace SAHL.DomainServiceChecks.Managers.ClientDataManager
{
    public interface IClientDataManager
    {
        bool IsClientOnOurSystem(int ClientKey);

        bool IsClientANaturalPerson(int ClientKey);
    }
}
