using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.AccountDataManager.Statements
{
    public class AccountExistsStatement : ISqlStatement<int>
    {
        public int AccountKey { get; protected set; }

        public AccountExistsStatement(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public string GetStatement()
        {
            return @"SELECT COUNT(1) AS Total FROM [2AM].[dbo].[Account] WHERE AccountKey = @AccountKey";
        }
    }
}