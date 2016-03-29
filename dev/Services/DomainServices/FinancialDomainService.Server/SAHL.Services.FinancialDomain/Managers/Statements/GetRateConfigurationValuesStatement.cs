using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetRateConfigurationValuesStatement : ISqlStatement<RateConfigurationValuesModel>
    {
        public int MarginKey { get; protected set; }
        public int MarketRateKey { get; protected set; }

        public GetRateConfigurationValuesStatement(int marginKey, int marketRateKey)
        {
            this.MarginKey = marginKey;
            this.MarketRateKey = marketRateKey;
        }

        public string GetStatement()
        {
            return @"select rc.RateConfigurationKey,
                    Cast(m.Value as decimal(22, 10)) [MarginValue],
                    Cast(mr.Value as decimal(22, 10)) [MarketRateValue]
                    from [2AM].dbo.RateConfiguration rc
                    join [2AM].dbo.Margin m on rc.MarginKey = m.MarginKey
                    join [2AM].dbo.MarketRate mr on rc.MarketRateKey = mr.MarketRateKey
                    where rc.MarginKey = @MarginKey and rc.MarketRateKey = @MarketRateKey";
        }
    }
}
