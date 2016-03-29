
namespace SAHL.Services.DomainQuery.Managers.Address
{
    public interface IAddressDataManager
    {
        bool IsAddressAClientAddress(int addressKey, int clientKey);
    }
}
