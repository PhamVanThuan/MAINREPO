using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetNonApplicantsWithExternalRoleRelationshipForApplicationQueryStatement : IServiceQuerySqlStatement<GetNonApplicantsWithExternalRoleRelationshipForApplicationQuery, LegalEntityModel>
    {
        public string GetStatement()
        {
            return @"with NonApplicants_CTE (LegalEntityKey)
                        as
                        (
	                        select distinct ler.RelatedLegalEntityKey
                            from [2AM].[dbo].[LegalEntityRelationship] ler
                            join [2AM].[dbo].[ExternalRole] etr on etr.LegalEntityKey = ler.LegalEntityKey
                            join [2AM].[dbo].[ExternalRoleType] etrt on etrt.[ExternalRoleTypeKey] = etr.[ExternalRoleTypeKey]
								and etrt.[ExternalRoleTypeGroupKey] = 1
                            where etr.GenericKey = @ApplicationKey
							and etr.GenericKeyTypeKey = 2
                            and etr.GeneralStatusKey = 1
	                        and ler.RelatedLegalEntityKey not in (select etr.LegalEntityKey
									                        from [2AM].[dbo].[ExternalRole] etr
															join [2AM].[dbo].[ExternalRoleType] etrt on etrt.[ExternalRoleTypeKey] = etr.[ExternalRoleTypeKey]
																and etrt.[ExternalRoleTypeGroupKey] = 1
															where etr.GenericKey = @ApplicationKey
															and etr.GenericKeyTypeKey = 2
															and etr.GeneralStatusKey = 1)
                        )
                        select
	                        cte.LegalEntityKey,
	                        (
		                        Select top 1 [2AM].[dbo].[LegalEntityLegalName](ler.RelatedLegalEntityKey, 0) + ' (' + lert.[Description] + ')' from  [2AM].[dbo].[LegalEntityRelationship] ler
		                        join [2AM].[dbo].[LegalEntityRelationshipType] lert on lert.RelationshipTypeKey = ler.RelationshipTypeKey
		                        where ler.RelatedLegalEntityKey = cte.LegalEntityKey order by ler.LegalEntityRelationshipKey desc
	                        ) as LegalEntityDescription
                        from
	                        NonApplicants_CTE cte ";
        }
    }
}