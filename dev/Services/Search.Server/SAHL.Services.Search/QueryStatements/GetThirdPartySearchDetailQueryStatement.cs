using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Search.QueryStatements
{
    public class GetThirdPartySearchDetailQueryStatement : IServiceQuerySqlStatement<GetThirdPartySearchDetailQuery, GetThirdPartySearchDetailQueryResult>
    {
        public string GetStatement()
        {
            return @"select 'attorney' as [Type], 
                    at.[Description] as AddressType,
                    ISNULL([2am].dbo.fGetFormattedAddressDelimited(lea.AddressKey, 0),'') as Address,
                    case a.GeneralStatusKey
                    when 1 then CAST(1 as BIT)
                    else CAST(2 AS BIT)
                    end as IsActive,
                    a.AttorneyContact as Contact,
                    CAST(a.AttorneyLitigationInd as BIT) as IsLitigationAttorney,
                    CAST(a.AttorneyRegistrationInd as BIT) as IsRegistrationAttorney,
                    d.[Description] as DeedsOffice
                     from [2am].[dbo].[Attorney] a
                    left join [2am].[dbo].LegalEntityAddress lea on lea.LegalEntityKey = a.LegalEntityKey
                    left join [2am].dbo.AddressType at on at.AddressTypeKey = lea.AddressTypeKey
                    join [2am].[dbo].[DeedsOffice] d on d.DeedsOfficeKey = a.DeedsOfficeKey
                where a.LegalEntityKey = @legalEntityKey

                    union

                    select 'valuator' as [Type],
                    at.[Description] as AddressType,
                    ISNULL([2am].dbo.fGetFormattedAddressDelimited(lea.AddressKey, 0),'') as Address,
                    case v.GeneralStatusKey
                    when 1 then CAST(1 as BIT)
                    else CAST(2 AS BIT)
                    end as IsActive,
                    v.ValuatorContact as Contact,
                    CAST(0 as BIT) as IsLitigationAttorney,
                    CAST(0 as BIT) as IsRegistrationAttorney,
                    '' as DeedsOffice                
                     from [2am].[dbo].[Valuator] v
                    left join [2am].[dbo].LegalEntityAddress lea on lea.LegalEntityKey = v.LegalEntityKey
                    left join [2am].dbo.AddressType at on at.AddressTypeKey = lea.AddressTypeKey
                    where v.LegalEntityKey = @legalEntityKey";
        }
    }
}