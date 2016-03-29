namespace SAHL.DomainServiceChecks.Managers.AccountDataManager
{
    public interface IAccountDataManager
    {
        bool DoesAccountExist(int accountKey);
    }
}