using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IMarketingSourceRepository
    {
        IReadOnlyEventList<IApplicationSource> GetMarketingSources();

        IApplicationSource GetMarketingSourceByKey(int SourceKey);

        IApplicationSource GetEmptyApplicationSource();

        void SaveApplicationSource(IApplicationSource ApplicationSource);

        bool ApplicationSourceExists(string strDescription);
    }
}