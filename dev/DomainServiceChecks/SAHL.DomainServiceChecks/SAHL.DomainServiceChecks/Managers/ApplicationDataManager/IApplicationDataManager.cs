
namespace SAHL.DomainServiceChecks.Managers.ApplicationDataManager
{
    public interface IApplicationDataManager
    {
        bool IsApplicationOpen(int applicationNumber);

        bool IsLatestApplicationInformationOpen(int applicationNumber);

        bool IsActiveClientRole(int applicationRoleKey);
    }
}
