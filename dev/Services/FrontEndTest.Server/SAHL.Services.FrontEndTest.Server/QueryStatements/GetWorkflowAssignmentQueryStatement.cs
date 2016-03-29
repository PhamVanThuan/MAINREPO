using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetWorkflowAssignmentQueryStatement : IServiceQuerySqlStatement<GetWorkflowAssignmentQuery, GetWorkflowAssignmentQueryResult>
    {
        public string GetStatement()
        {
            var query = @"select wa.aduserkey, wa.generalstatuskey, wa.insertDate, wa.instanceId, wa.OfferRoleTypeOrganisationStructureMappingKey
                            from x2.x2.WorkflowAssignment wa
				          join [x2].[x2data].Application_Capture wt on wa.instanceid = wt.instanceid
			              where ApplicationKey = @AppKey";
            return query;
        }
    }
}