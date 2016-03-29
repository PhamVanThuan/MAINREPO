using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Search.QueryStatements
{
    public class GetClientSearchDetailQueryStatement : IServiceQuerySqlStatement<GetClientSearchDetailQuery, GetClientSearchDetailQueryResult>
    {
        public string GetStatement()
        {
            return @"with Accounts(Product, AccountKey, ParentAccountKey, RoleTypeKey, AccountLevel) AS
            (
                select a.RRR_ProductKey as Product, a.AccountKey, a.AccountKey, r.RoleTypeKey, 0 as AccountLevel from [2am].dbo.Role r
                join [2am].dbo.[Account] a on r.AccountKey = a.AccountKey
                where a.AccountStatusKey = 1 and 
                a.ParentAccountKey is null and r.LegalEntityKey = @LegalEntityKey
                UNION ALL
                select b.RRR_ProductKey as Product, b.AccountKey, b.ParentAccountKey, br.RoleTypeKey, AccountLevel + 1 from [2am].dbo.Role br
                join [2am].dbo.[Account] b on br.AccountKey = b.AccountKey
                join Accounts aa on aa.AccountKey = b.ParentAccountKey and br.LegalEntityKey = @LegalEntityKey
                where b.AccountStatusKey = 1
            )
            select 'Account' as [Type], Product, a.AccountKey as [Key], ParentAccountKey as ParentKey, rt.Description as [Role], AccountLevel as [Level], 
            ISNULL([2am].dbo.fGetFormattedAddressDelimited(p.AddressKey, 0),'') as [Address] from Accounts a
            join [2am].dbo.RoleType rt on rt.RoleTypeKey = a.RoleTypeKey
            join [2am].dbo.FinancialService fs on fs.AccountKey = a.AccountKey
            join [2am].dbo.FinancialServiceType fst on fst.FinancialServiceTypeKey = fs.FinancialServiceTypeKey and fst.BalanceTypeKey = 1
            left join [2am].fin.MortgageLoan ml on ml.FinancialServiceKey = fs.FinancialServiceKey
            left join [2am].dbo.Property p on p.PropertyKey = ml.PropertyKey
            union 

            select 'Application' as [Type], Product, ApplicationNumber as [Key], null as ParentKey, [Role], 0 as [Level], Address from (
            select row_number() over (partition by o.offerkey order by oi.offerinformationkey) r, oi.ProductKey as Product, o.OfferKey as ApplicationNumber, rot.Description as [Role],
             ISNULL([2am].dbo.fGetFormattedAddressDelimited(p.AddressKey, 0),'') as [Address] from [2am].dbo.[offer] o
            join [2am].dbo.[OfferInformation] oi on oi.OfferKey = o.OfferKey
            join [2am].dbo.[offerrole] ro on ro.OfferKey = o.OfferKey
            join [2am].dbo.[OfferRoleType] rot on rot.OfferRoleTypeKey = ro.OfferRoleTypeKey and rot.OfferRoleTypeGroupKey = 3
            left join [2am].dbo.OfferMortgageLoan oml on oml.OfferKey = o.OfferKey
            left join [2am].dbo.Property p on p.PropertyKey = oml.PropertyKey
            where ro.LegalEntityKey = @LegalEntityKey and o.OfferEndDate is null
            ) a
            where r=1
            union
            select 'Address' as [Type], 1 as Product, 0 as [Key], 0 as ParentKey, at.Description as [Role], 0 as [Level], 
            ISNULL([2am].dbo.fGetFormattedAddressDelimited(a.AddressKey, 0),'') as [Address] from [2am].dbo.LegalEntity le
            left join [2am].dbo.LegalEntityAddress lea on lea.LegalEntityKey = le.LegalEntityKey and lea.GeneralStatusKey = 1
            left join [2am].dbo.Address a on a.AddressKey = lea.AddressKey 
            left join [2am].dbo.AddressType at on at.AddressTypeKey = lea.AddressTypeKey
            where le.LegalEntityKey = @LegalEntityKey

            order by [Type], ParentKey, [Level]";
        }
    }
}
