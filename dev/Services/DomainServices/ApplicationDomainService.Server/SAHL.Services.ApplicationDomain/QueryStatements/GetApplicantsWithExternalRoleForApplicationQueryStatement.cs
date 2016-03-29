using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetApplicantsWithExternalRoleForApplicationQueryStatement : IServiceQuerySqlStatement<GetApplicantsWithExternalRoleForApplicationQuery, LegalEntityModel>
    {
        public string GetStatement()
        {
            return @"select etr.LegalEntityKey, [2AM].[dbo].[LegalEntityLegalName](etr.LegalEntityKey, 0) + ' (' + etrt.[Description] + ')' LegalEntityDescription
                        from [2AM].[dbo].[ExternalRole] etr
                        join [2AM].[dbo].[ExternalRoleType] etrt on etrt.[ExternalRoleTypeKey] = etr.[ExternalRoleTypeKey]
	                        and etrt.[ExternalRoleTypeGroupKey] = 1
                        where etr.GenericKey = @ApplicationKey
                        and etr.GenericKeyTypeKey = 2
                        and etr.GeneralStatusKey = 1
                        order by ChangeDate";
        }
    }
}