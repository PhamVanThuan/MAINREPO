namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IApplicationReadOnlyRepository
    {
        int GetApplicationTypeFromApplicationKey(int applicationKey);

        IApplication GetApplicationByKey(int applicationkey);

        IApplicationLife GetApplicationLifeByKey(int lifeApplicationkey);

        ICallback GetLatestCallBackByApplicationKey(int applicationKey, bool openCallbacksOnly);
    }
}