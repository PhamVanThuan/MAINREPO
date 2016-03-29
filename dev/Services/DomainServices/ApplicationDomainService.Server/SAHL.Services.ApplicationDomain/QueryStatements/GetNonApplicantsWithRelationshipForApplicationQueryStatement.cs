using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetNonApplicantsWithRelationshipForApplicationQueryStatement : IServiceQuerySqlStatement<GetNonApplicantsWithRelationshipForApplicationQuery, LegalEntityModel>
    {
        public string GetStatement()
        {
            return @"with NonApplicants_CTE (LegalEntityKey)
                        as
                        (
	                        select distinct ler.RelatedLegalEntityKey
                            from [2AM].[dbo].[LegalEntityRelationship] ler
                            join [2AM].[dbo].OfferRole ofr on ofr.LegalEntityKey = ler.LegalEntityKey
                            join [2AM].[dbo].OfferRoleType ofrt on ofrt.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                                and ofrt.[OfferRoleTypeGroupKey] = 3
                            where ofr.OfferKey = @ApplicationKey
                            and ofr.GeneralStatusKey = 1
	                        and ler.RelatedLegalEntityKey not in (select ofr.LegalEntityKey
									                        from [2AM].[dbo].OfferRole ofr
									                        join [2AM].[dbo].OfferRoleType ofrt on ofrt.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                                                                and ofrt.[OfferRoleTypeGroupKey] = 3
									                        where ofr.OfferKey = @ApplicationKey and ofr.GeneralStatusKey = 1)
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