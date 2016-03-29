using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications.AggregatedApplications
{
    public class AggregatedApplicationsRootTileContentDataProvider : HaloTileBaseContentDataProvider<AggregatedApplicationsRootModel>
    {
        public AggregatedApplicationsRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select le.LegalEntityTypeKey,
                                    [2AM].[dbo].[LegalEntityLegalName](le.LegalEntityKey, 0) as LegalName,
                                    le.CitizenTypeKey,
                                    le.IDNumber,
                                    le.PassportNumber,
                                    le.RegistrationNumber,
                                    le.LegalEntityTypeKey as LegalEntityType,
                                    CASE WHEN le.LegalEntityTypeKey = 2
                                        THEN les.[Description]
                                        ELSE CASE WHEN les.LegalEntityStatusKey = 1 THEN 'Registered' ELSE 'De-Registered' END
                                    END as LegalEntityStatus,
                                    ms.Description as MaritalStatus,
                                    CASE WHEN le.HomePhoneNumber is null or le.HomePhoneNumber = ''
                                    THEN NULL
                                    ELSE CASE WHEN le.HomephoneCode is null THEN '' ELSE '('+le.HomephoneCode+')' + SPACE(1) END + le.HomePhoneNumber
                                    END as HomephoneNumber,
                                    CASE WHEN le.WorkPhoneNumber is null
                                    THEN NULL
                                    ELSE CASE WHEN le.WorkPhoneCode is null THEN '' ELSE '('+le.WorkPhoneCode+')' + SPACE(1) END + le.WorkPhoneNumber
                                    END as WorkphoneNumber,
                                    le.CellPhoneNumber as CellPhoneNumber,
                                    le.DateOfBirth,
                                    CASE 
                                    WHEN lepa.AddressKey IS NULL THEN 
                                    CASE 
                                    WHEN lear.AddressKey IS NULL THEN '' 
                                    ELSE [2AM].[dbo].[fGetFormattedAddress](lear.AddressKey) 
                                    END  
                                    ELSE [2AM].[dbo].[fGetFormattedAddress](lepa.AddressKey) 
                                    END as Address,
                                    CASE WHEN lea_dc.AddressKey IS NULL THEN '' ELSE [2AM].[dbo].[fGetFormattedAddress](lea_dc.AddressKey) END as DomiciliumAddress,
                                    le.EmailAddress,
                                    bk.ACBBankDescription as BankingInstitute,
                                    le.GenderKey as Gender,
                                    le.LegalEntityStatusKey as [Status]
                                from [2AM].[dbo].[LegalEntity] le
                                left join [2AM].[dbo].[LegalEntityStatus] les on les.LegalEntityStatusKey = le.LegalEntityStatusKey
                                left join (select LegalEntityKey, max(AddressKey) as AddressKey
                                from [2AM].[dbo].[LegalEntityAddress]
                                where AddressTypeKey = 2
                                and GeneralStatusKey = 1
                                and EffectiveDate < getdate()
                                group by LegalEntityKey) lepa on lepa.LegalEntityKey = le.LegalEntityKey
                                left join (select LegalEntityKey, max(AddressKey) as AddressKey
                                from [2AM].[dbo].[LegalEntityAddress]
                                where AddressTypeKey = 1
                                and GeneralStatusKey = 1
                                and EffectiveDate < getdate()
                                group by LegalEntityKey) lear on lear.LegalEntityKey = le.LegalEntityKey
                                left join (select lea.LegalEntityKey, max(lea.AddressKey) as AddressKey
                                from [2AM].[dbo].[LegalEntityAddress] lea
                                join [2AM].[dbo].[LegalEntityDomicilium] led on led.LegalEntityAddressKey = lea.LegalEntityAddressKey
                                and led.GeneralStatusKey = 1
                                group by lea.LegalEntityKey) lea_dc on lea_dc.LegalEntityKey = le.LegalEntityKey
                                left join (select leba.LegalEntityKey, max(ba.BankAccountKey) as BankAccountKey
                                from [2AM].[dbo].[LegalEntityBankAccount] leba
                                join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                and ba.ACBTypeNumber in (0, 1, 2)
                                where leba.GeneralStatusKey = 1
                                group by leba.LegalEntityKey) leba on leba.LegalEntityKey = le.LegalEntityKey
                                left join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                left join [2AM].[dbo].[ACBBranch] br on br.ACBBranchCode = ba.ACBBranchCode
                                left join [2AM].[dbo].[ACBBank] bk on bk.ACBBankCode = br.ACBBankCode
                                left join [2am].dbo.MaritalStatus ms on ms.MaritalStatusKey = le.MaritalStatusKey
                                where le.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}