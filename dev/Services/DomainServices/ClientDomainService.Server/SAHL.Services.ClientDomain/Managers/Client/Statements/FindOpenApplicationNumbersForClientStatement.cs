using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class FindOpenApplicationNumbersForClientStatement : ISqlStatement<int>
    {
        public FindOpenApplicationNumbersForClientStatement(int clientKey)
        {
            this.ClientKey = clientKey;
        }

        public int ClientKey { get; protected set; }

        public string GetStatement()
        {
            return @"   select distinct(o.offerkey)
                        from [2am].dbo.legalentity le
                        inner join [2am].dbo.offerrole r on le.legalentitykey=r.legalentitykey
                            and r.OfferRoleTypeKey in (8,10,11,12)
                            and r.GeneralStatusKey = 1
                        inner join [2am].dbo.offer o on r.offerkey=o.offerkey
                            and o.OfferTypeKey not in (2, 3, 4)
                        left join [2am].dbo.stagetransitioncomposite stc on o.offerkey=stc.generickey
                            and stc.stagedefinitionstagedefinitiongroupkey in (110,111)
                        where
                        o.offerstatuskey in (1,4,5)  and stc.generickey is null  and le.legalentityKey = @ClientKey
                    ";
        }
    }
}