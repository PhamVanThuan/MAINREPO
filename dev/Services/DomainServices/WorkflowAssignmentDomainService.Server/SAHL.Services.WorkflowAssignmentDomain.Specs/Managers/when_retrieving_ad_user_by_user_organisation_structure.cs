using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_retrieving_ad_user_by_user_organisation_structure : WithFakes
    {

        Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            dataManager = new WorkflowCaseDataManager(dbFactory);

            userOrganisationStructureKey = 1;
        };


        private Because of = () =>
        {
            result = dataManager.GetCapabilitiesForUserOrganisationStructureKey(userOrganisationStructureKey);
        };

        

        private static IDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
        private static int userOrganisationStructureKey;
        private static IEnumerable<CapabilityDataModel> result;
    }
}
