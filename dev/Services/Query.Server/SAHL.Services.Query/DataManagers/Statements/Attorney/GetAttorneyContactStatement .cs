using System;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;

namespace SAHL.Services.Query.DataManagers.Statements
{
    public class GetAttorneyContactStatement : ISqlStatement<AttorneyContactDataModel>
    {
        public string GetStatement()
        {
            return @"
            Select LE_C.LegalEntityKey as Id,
	            TP.Id as AttorneyId,
	            A.AttorneyKey as AttorneyKey,
                coalesce(LE_C.CellPhoneNumber, '') as CellPhoneNumber,
                coalesce(LE_C.HomePhoneCode, '') as HomePhoneCode,
	            coalesce(LE_C.HomePhoneNumber, '') as HomePhoneNumber,
	            coalesce(LE_C.WorkPhoneCode, '') as WorkPhoneCode,
	            coalesce(LE_C.WorkPhoneNumber, '') as WorkPhoneNumber,
                coalesce(LE_C.FaxCode, '') as FaxCode,
	            coalesce(LE_C.FaxNumber, '') as FaxNumber,
	            coalesce(LE_C.EmailAddress, '') as EmailAddress
            From [2AM].[dbo].[ExternalRole] ER
            Inner Join [2AM].[dbo].[Attorney] A
	            On Er.GenericKey = A.AttorneyKey
            Inner Join [2AM].[dbo].[LegalEntity] LE_C
	            On ER.LegalEntityKey = LE_C.LegalEntityKey
            Inner Join [2AM].[dbo].[ThirdParty] TP
	            On TP.GenericKey = A.AttorneyKey
            Where ER.GenericKeyTypeKey = 35
            And Er.ExternalRoleTypeKey = 10";
        }
    }
}