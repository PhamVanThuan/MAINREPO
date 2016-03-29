using SAHL.Core.Data;
using SAHL.Services.Query.Models.ThirdParty;

namespace SAHL.Services.Query.DataManagers.Statements.ThirdParty
{
    public class GetThirdPartyStatement : ISqlStatement<ThirdPartyDataModel>
    {
        public string GetStatement()
        {
            return @"
                    Select Tp.Id as Id,
	TP.ThirdPartyKey as ThirdPartyKey,
	LE.LegalEntityKey as LegalEntityKey,
	LE.RegisteredName as LegalName,
    Coalesce(LE.RegistrationNumber, '') as RegistrationNumber,
	TP.GeneralStatusKey as GeneralStatusKey,
	GS.Description as GeneralStatusDescription,
	LE.LegalEntityTypeKey as LegalEntityTypeKey,
	CASE WHEN LE.LegalEntityTypeKey = 2
        THEN LES.[Description]
        ELSE CASE WHEN LES.LegaLEntityStatusKey = 1 
			THEN 'Registered' 
			ELSE 'De-Registered' 
		END
    END as LegaLEntityStatus,
	TP.ThirdPartyTypeKey as ThirdPartyTypeKey,
	TPT.Description as ThirdPartyTypeDescription
	From [2AM].[dbo].ThirdParty TP
	Join [2AM].[dbo].[LegaLEntity] LE
		On TP.LegalEntityKey = LE.LegalEntityKey
    Join [2AM].[dbo].[LegaLEntityStatus] LES 
		ON LES.LegaLEntityStatusKey = LE.LegaLEntityStatusKey
	Join [2AM].[dbo].[GeneralStatus] GS
		ON TP.GeneralStatusKey = GS.GeneralStatusKey
	Join [2AM].[dbo].[ThirdPartyType] TPT
		ON TP.ThirdPartyTypeKey = TPT.ThirdPartyTypeKey";
        }
    }
}