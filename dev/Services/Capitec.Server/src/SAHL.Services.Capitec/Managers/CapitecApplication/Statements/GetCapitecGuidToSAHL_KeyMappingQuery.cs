using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;

namespace SAHL.Services.Capitec.Managers.CapitecApplication.Statements
{
    [NolockConventionExclude]
    public class GetCapitecGuidToSAHL_KeyMappingQuery : ISqlStatement<CapitecGuidToSAHL_KeyMappingModel>
    {
        public GetCapitecGuidToSAHL_KeyMappingQuery()
        {
        }

        public string GetStatement()
        {
            return @"select [Id] as CapitecKey, [SAHLAddressFormatKey] as SAHL_Key
                from Capitec.dbo.AddressFormatEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLAddressTypeKey] as SAHL_Key
                from Capitec.dbo.AddressTypeEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLMortgageLoanPurposeKey] as SAHL_Key
                from Capitec.dbo.ApplicationPurposeEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLEmploymentTypeKey] as SAHL_Key
                from Capitec.dbo.EmploymentTypeEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLOccupancyTypeKey] as SAHL_Key
                from Capitec.dbo.OccupancyTypeEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLSalutationKey] as SAHL_Key
                from Capitec.dbo.SalutationEnum (nolock)
                union
                select [Id] as CapitecKey, [SAHLSuburbKey] as SAHL_Key
                from Capitec.geo.Suburb (nolock)
                union
                select [Id] as CapitecKey, [SAHLPostOfficeKey] as SAHL_Key
                from Capitec.geo.PostOffice (nolock)";
        }
    }
}