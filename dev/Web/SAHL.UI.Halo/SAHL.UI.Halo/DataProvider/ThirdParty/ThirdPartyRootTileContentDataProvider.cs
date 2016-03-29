using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty
{
    public class ThirdPartyRootTileContentDataProvider : HaloTileBaseContentDataProvider<ThirdPartyRootModel>
    {
        public ThirdPartyRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                                    [2AM].[dbo].[LegalEntityLegalName](le.LegalEntityKey, 0) as LegalName,
                                    le.IDNumber,
                                    le.RegistrationNumber,
                                    CASE WHEN le.LegalEntityTypeKey = 2
                                        THEN les.[Description]
                                        ELSE CASE WHEN les.LegalEntityStatusKey = 1 THEN 'Registered' ELSE 'De-Registered' END
                                    END as LegalEntityStatus,
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
                                    CASE WHEN lea.AddressKey IS NULL THEN '' ELSE [2AM].[dbo].[fGetFormattedAddress](lea.AddressKey) END as PostalAddress,
                                    CASE WHEN lea_r.AddressKey IS NULL THEN '' ELSE [2AM].[dbo].[fGetFormattedAddress](lea_r.AddressKey) END as ResidentialAddress,
                                    le.EmailAddress,
                                    bk.ACBBankDescription as BankingInstitute
                                    from [2AM].[dbo].[LegalEntity] le
                                    join [2AM].[dbo].[LegalEntityStatus] les on les.LegalEntityStatusKey = le.LegalEntityStatusKey
                                    left join (select LegalEntityKey, max(AddressKey) as AddressKey
                                                      from [2AM].[dbo].[LegalEntityAddress]
                                                      where AddressTypeKey = 2
                                                      and GeneralStatusKey = 1
                                                      and EffectiveDate < getdate()
                                                      group by LegalEntityKey) lea on lea.LegalEntityKey = le.LegalEntityKey
                                    left join (select LegalEntityKey, max(AddressKey) as AddressKey
                                                      from [2AM].[dbo].[LegalEntityAddress]
                                                      where AddressTypeKey = 1
                                                      and GeneralStatusKey = 1
                                                      and EffectiveDate < getdate()
                                                      group by LegalEntityKey) lea_r on lea_r.LegalEntityKey = le.LegalEntityKey
                                    left join (select leba.LegalEntityKey, max(ba.BankAccountKey) as BankAccountKey
                                                      from [2AM].[dbo].[LegalEntityBankAccount] leba
                                                      join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                                            and ba.ACBTypeNumber in (0, 1, 2)
                                                      where leba.GeneralStatusKey = 1
                                                      group by leba.LegalEntityKey) leba on leba.LegalEntityKey = le.LegalEntityKey
                                    left join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                    left join [2AM].[dbo].[ACBBranch] br on br.ACBBranchCode = ba.ACBBranchCode
                                    left join [2AM].[dbo].[ACBBank] bk on bk.ACBBankCode = br.ACBBankCode
                                    where le.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}