using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Services.WorkflowAssignmentDomain.QueryStatements
{
    public class GetActiveUsersWithCapabilityStatement
        : IServiceQuerySqlStatement<GetActiveUsersWithCapabilityQuery, GetActiveUsersWithCapabilityQueryResult>
    {
        public string GetStatement()
        {
            var query = @"select ad.ADUserName [UserName], 
                                uc.UserOrganisationStructureKey, 
                            lg.FirstNames + ' ' + lg.Surname [FullName] 
                       from [2AM].OrgStruct.UserOrganisationStructureCapability uc 
                                    join [2am].dbo.UserOrganisationStructure uos 
                                        on uc.UserOrganisationStructureKey = uos.UserOrganisationStructureKey 
                                join [2am].dbo.AdUser ad 
                                on uos.ADUserKey = ad.ADUserKey 
                                join [2am].dbo.LegalEntity lg 
                                on lg.LegalEntityKey = ad.LegalEntityKey 
                        where 
                                uc.CapabilityKey = @CapabilityKey
                            and ad.GeneralStatusKey = 1";

            return query;
        }
    }
}