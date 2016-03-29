using System;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;

namespace SAHL.Services.Query.DataManagers.Statements
{
    public class GetAttorneyStatement : ISqlStatement<AttorneyDataModel>
    {
        public string GetStatement()
        {
            return @"
                Select TP.Id as Id,
                    A.AttorneyKey as AttorneyKey,
                    LE.LegalEntityKey as LegalEntityKey,
	                LE.RegisteredName as Name,
	                Coalesce(A.AttorneyLitigationInd, 0) as IsLitigationAttorney,
	                Coalesce(A.AttorneyRegistrationInd, 0) as IsRegistrationAttorney,
	                Coalesce(TP.IsPanel, 0) as IsPanelAttorney,
	                Coalesce(A.AttorneyContact, '') as AttorneyContact,
	                TP.GeneralStatusKey as GeneralStatusKey,
	                GS.Description as GeneralStatusDescription,
	                A.DeedsOfficeKey as DeedsOfficeKey,
	                DO.Description as DeedsOfficeDescription,
	                A.AttorneyMandate as Mandate,
	                A.AttorneyWorkFlowEnabled as WorkFlowEnabled
                From [2AM].dbo.Attorney A
                Inner Join [2AM].dbo.ThirdParty TP
	                On A.AttorneyKey = TP.GenericKey
                Inner Join [2AM].dbo.LegalEntity LE
	                On LE.LegalEntityKey = A.LegalEntityKey
                Inner Join [2AM].dbo.GeneralStatus GS
	                On TP.GeneralStatusKey = GS.GeneralStatusKey 
                Inner Join [2AM].dbo.DeedsOffice DO
	                On DO.DeedsOfficeKey = A.DeedsOfficeKey
                Where TP.GenericKeyTypeKey = 35";
        }
    }
}