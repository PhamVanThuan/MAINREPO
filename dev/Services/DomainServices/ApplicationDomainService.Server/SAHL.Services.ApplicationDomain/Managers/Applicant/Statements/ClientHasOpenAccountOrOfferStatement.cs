using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class ClientHasOpenAccountOrOfferStatement: ISqlStatement<bool>
    {
        public int LegalEntityKey { get; protected set; }

        public ClientHasOpenAccountOrOfferStatement(int legalEntityKey)
        {
            this.LegalEntityKey = legalEntityKey;
        }

        public string GetStatement()
        {
            var query = @"select cast(sum(checkExists) as bit) from 
                (
                    select count(1) checkExists
                    from [2am].dbo.offer o
                    join [2am].dbo.offerrole ofr  on ofr.OfferKey = o.OfferKey
                    join [2am].dbo.offerroletype ort  on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                    where 
                    ofr.LegalEntityKey = @legalEntityKey
                    and o.OfferStatusKey = 1
                    and ort.OfferRoleTypeGroupKey = 3
                    union all
                    select count(1) checkExists
                    from [2am].dbo.Account a
                    join [2am].dbo.Role r on a.AccountKey = r.AccountKey
                    where r.LegalEntityKey = 105248
                    and a.AccountStatusKey in (1, 4, 5)
                    and a.ParentAccountKey is null
                ) checkAll";
            return query;
        }
    }
}
