using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.Services.Query.Models.ThirdParty;

namespace SAHL.Services.Query.DataManagers.Statements.ThirdParty
{
    public class GetThirdPartyContactInformationStatement:ISqlStatement<ThirdPartyContactInformationDataModel>
    {
        public string GetStatement()
        {
            return @"Select 
                        TP.Id as Id,
                        TP.ThirdPartyKey as ThirdPartyKey,
                        LE.LegalEntityKey as LegalEntityKey,
                        CASE 
	                        WHEN le.HomephoneCode is null THEN '' 
	                        ELSE '('+ Rtrim(le.HomephoneCode) +')' + SPACE(1) 
                        END + RTrim(Coalesce (le.HomePhoneNumber, ''))
                        AS HomePhoneNumber,
                        CASE 
	                        WHEN le.WorkPhoneCode is null THEN '' 
	                        ELSE '('+ Rtrim(le.WorkPhoneCode) +')' + SPACE(1)  
                        END + RTrim(Coalesce (le.WorkPhoneNumber, '')) 
                        As WorkPhoneNumber,
                        CASE 
	                        WHEN le.FaxCode is null THEN '' 
	                        ELSE '('+ Rtrim(le.FaxCode) +')' + SPACE(1)  
                        END + RTrim(Coalesce (LE.FaxNumber, '')) 
                        As FaxNumber,
                        Coalesce (LE.CellPhoneNumber, '') as CellPhoneNumber,
                        CASE WHEN lea.AddressKey IS NULL THEN '' ELSE [2AM].[dbo].[fGetFormattedAddress](lea.AddressKey) END as PostalAddress,
                        CASE WHEN lea_r.AddressKey IS NULL THEN '' ELSE [2AM].[dbo].[fGetFormattedAddress](lea_r.AddressKey) END as ResidentialAddress,
                        Coalesce (LE.EmailAddress, '') as EmailAddress
                        From [2AM].[dbo].[ThirdParty] TP
                        Join [2AM].[dbo].[LegalEntity] LE
	                        On TP.LegalEntityKey = LE.LegalEntityKey
                        Left Join (Select LegalEntityKey, max(AddressKey) as AddressKey
                                      From [2AM].[dbo].[LegalEntityAddress]
                                            Where AddressTypeKey = 2
                                            And GeneralStatusKey = 1
                                            And EffectiveDate < getdate()
                                            Group By LegalEntityKey) lea on lea.LegalEntityKey = le.LegalEntityKey
                        Left Join (Select LegalEntityKey, max(AddressKey) as AddressKey
                                      From [2AM].[dbo].[LegalEntityAddress]
                                            where AddressTypeKey = 1
                                            and GeneralStatusKey = 1
                                            and EffectiveDate < getdate()
                                            group by LegalEntityKey) lea_r on lea_r.LegalEntityKey = le.LegalEntityKey
                ";
        }
    }
}
