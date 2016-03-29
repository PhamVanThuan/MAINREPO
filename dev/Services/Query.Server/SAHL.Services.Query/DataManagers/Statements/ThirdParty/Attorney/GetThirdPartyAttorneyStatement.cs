using SAHL.Core.Data;
using SAHL.Services.Query.Models.ThirdParty;

namespace SAHL.Services.Query.DataManagers.Statements.ThirdParty
{
    public class GetThirdPartyAttorneyStatement : ISqlStatement<ThirdPartyAttorneyDataModel>
    {
        public string GetStatement()
        {
            return @"Select Tp.Id as Id,
	TP.ThirdPartyKey as ThirdPartyKey,
	LE.LegalEntityKey as LegalEntityKey,
	LE.RegisteredName as LegalName,
    Coalesce(LE.RegistrationNumber, '') as RegistrationNumber,
	Coalesce(A.AttorneyLitigationInd, 0) as IsLitigationAttorney,
	Coalesce(A.AttorneyRegistrationInd, 0) as IsRegistrationAttorney,
	Coalesce(TP.IsPanel, 0) as IsPanelAttorney,
	Coalesce(A.AttorneyContact, '') as AttorneyContact,
	A.DeedsOfficeKey as DeedsOfficeKey,
	DO.Description as DeedsOfficeDescription,
	Coalesce(A.AttorneyMandate, 0) as Mandate,
	A.AttorneyWorkFlowEnabled as WorkFlowEnabled,
	TP.GeneralStatusKey as GeneralStatusKey,
	GS.Description as GeneralStatusDescription,
	LE.LegalEntityTypeKey as LegalEntityTypeKey,
	CASE WHEN LE.LegalEntityTypeKey = 2
        THEN LES.[Description]
        ELSE CASE WHEN LES.LegaLEntityStatusKey = 1 
			THEN 'Registered' 
			ELSE 'De-Registered' 
		END
    END as LegaLEntityStatusDescription,
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
	From [2AM].[dbo].ThirdParty TP
	Inner Join [2AM].dbo.Attorney A
	     On A.AttorneyKey = TP.GenericKey
	Join [2AM].[dbo].[LegaLEntity] LE
		On TP.LegalEntityKey = LE.LegalEntityKey
    Join [2AM].[dbo].[LegaLEntityStatus] LES 
		ON LES.LegaLEntityStatusKey = LE.LegaLEntityStatusKey
	Join [2AM].[dbo].[GeneralStatus] GS
		ON TP.GeneralStatusKey = GS.GeneralStatusKey
	Join [2AM].dbo.DeedsOffice DO
	    On DO.DeedsOfficeKey = A.DeedsOfficeKey
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
	Where TP.GenericKeyTypeKey = 35";
        }
    }
}