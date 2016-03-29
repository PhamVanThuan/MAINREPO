using System;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;

namespace SAHL.Services.Query.DataManagers.Statements
{
    public class GetAttorneyContactInformationStatement : ISqlStatement<AttorneyContactInformationDataModel>
    {
        public string GetStatement()
        {
            return @"
            Select A.AttorneyKey as Id,
	            TP.GenericKey as AttorneyKey,
	            TP.Id as AttorneyId,
	            coalesce(LE.CellPhoneNumber, '') as CellPhoneNumber,
	            coalesce(LE.HomePhoneCode, '') as HomePhoneCode,
	            coalesce(LE.HomePhoneNumber, '') as HomePhoneNumber,
	            coalesce(LE.WorkPhoneCode, '') as WorkPhoneCode,
	            coalesce(LE.WorkPhoneNumber, '') as WorkPhoneNumber,
	            coalesce(LE.FaxCode, '') as FaxCode,
	            coalesce(LE.FaxNumber, '') as FaxNumber,
	            coalesce(LE.EmailAddress, '') as EmailAddress
            From [2AM].[dbo].[LegalEntity] LE
            Inner Join [2AM].[dbo].[ThirdParty] TP
	            On LE.LegalEntityKey = TP.LegalEntityKey
            Inner Join [2AM].[dbo].[Attorney] A
                On TP.GenericKey = A.AttorneyKey";
        }
    }
}
