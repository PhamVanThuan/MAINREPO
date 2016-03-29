using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class GetApplicantsWithRoleForApplicationQueryStatement : IServiceQuerySqlStatement<GetApplicantsWithRoleForApplicationQuery, LegalEntityModel>
    {
        public string GetStatement()
        {
            return @"select ofr.LegalEntityKey, [2AM].[dbo].[LegalEntityLegalName](ofr.LegalEntityKey, 0) + ' (' + ofrt.[Description] + ')' LegalEntityDescription
                        from [2AM].[dbo].OfferRole ofr
                        join [2AM].[dbo].OfferRoleType ofrt on ofrt.OfferRoleTypeKey = ofr.OfferRoleTypeKey
	                        and ofrt.[OfferRoleTypeGroupKey] = 3
                        where ofr.OfferKey = @ApplicationKey
                        and ofr.GeneralStatusKey = 1
                        order by StatusChangeDate";
        }
    }
}