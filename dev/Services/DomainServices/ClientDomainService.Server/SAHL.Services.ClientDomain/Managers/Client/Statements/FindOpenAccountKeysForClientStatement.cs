using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class FindOpenAccountKeysForClientStatement : ISqlStatement<int>
    {
        public FindOpenAccountKeysForClientStatement(int clientKey)
        {
            this.ClientKey = clientKey;
        }

        public int ClientKey
        {
            get;
            protected set;
        }

        public string GetStatement()
        {
            return @"select a.AccountKey
                    from
                        [2am].dbo.account a
                    inner join
                        [2am].dbo.[role] r
                    on
                        r.AccountKey = a.AccountKey and r.GeneralStatusKey = 1
                    where
                        a.AccountStatusKey = 1
                    and
                        r.LegalEntityKey = @ClientKey";
        }
    }
}