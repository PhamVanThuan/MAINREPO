using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IMarketRateRepository
    {
        IReadOnlyEventList<IMarketRate> GetMarketRates();

        IMarketRate GetMarketRateByKey(int marketRateKey);

        bool UpdateMarketRate(IMarketRate marketRate, double oldval, string userID);
    }
}