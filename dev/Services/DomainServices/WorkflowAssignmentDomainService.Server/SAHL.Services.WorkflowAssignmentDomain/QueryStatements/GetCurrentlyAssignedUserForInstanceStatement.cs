using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.WorkflowAssignmentDomain.QueryStatements
{
    public class GetCurrentlyAssignedUserForInstanceStatement : IServiceQuerySqlStatement<GetCurrentlyAssignedUserForInstanceQuery, GetCurrentlyAssignedUserForInstanceQueryResult>
    {
        public string GetStatement()
        {
            var query = @"SELECT
	                         ins.[InstanceId]
	                        ,ins.[CapabilityKey]
	                        ,ins.[UserOrganisationStructureKey]
	                        ,ins.[UserName]
	                        ,ins.[LastUpdated]
	                        ,lge.FirstNames + ' ' + lge.Surname [FullName]
                        FROM 
	                        [EventProjection].[projection].[CurrentlyAssignedUserForInstance] ins
	                        join [2am].dbo.UserOrganisationStructure uos on uos.UserOrganisationStructureKey = ins.UserOrganisationStructureKey
	                        join [2am].dbo.AdUser adu on adu.ADUserKey = uos.ADUserKey
	                        join [2am].dbo.LegalEntity lge on lge.LegalEntityKey = adu.LegalEntityKey
                        WHERE
	                        ins.InstanceId = @InstanceId";

            return query;
        }
    }
}
