using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class DoesClientMarketingOptionExistStatement : ISqlStatement<int>
    {
        public int LegalEntityKey
        { get; protected set; }

        public int MarketingOptionKey
        { get; protected set; }

        public DoesClientMarketingOptionExistStatement(int legalEntityKey, int marketingOptionKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.MarketingOptionKey = marketingOptionKey;
        }

        public string GetStatement()
        {
            var sql = @"SELECT LegalEntityMarketingOptionKey from [2am].dbo.LegalEntityMarketingOption
                        WHERE LegalEntityKey = @legalEntityKey AND MarketingOptionKey = @marketingOptionKey";
            return sql;
        }
    }
}