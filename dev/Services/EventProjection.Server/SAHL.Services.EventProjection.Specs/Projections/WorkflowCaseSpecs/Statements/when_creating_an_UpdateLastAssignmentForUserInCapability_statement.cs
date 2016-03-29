using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowCaseSpecs.Statements
{
    public class when_creating_an_UpdateLastAssignmentForUserInCapability_statement : WithFakes
    {
        Establish that = () =>
        {
            instanceId = 1L;
            capabilityKey = 2;
            userOrganisationStructureKey = 3;
            genericKeyTypeKey = 4;
            genericKey = 5;
            userName = "banana";
        };

        private Because of = () =>
        {
            statement = new UpdateLastAssignedUserByCapabilityForInstanceStatement(instanceId, capabilityKey, userOrganisationStructureKey, genericKeyTypeKey, genericKey, userName);
        };

        private It should_have_set_the_expected_fields = () =>
        {
            statement.InstanceId.ShouldEqual(instanceId);
            statement.CapabilityKey.ShouldEqual(capabilityKey);
            statement.UserOrganisationStructureKey.ShouldEqual(userOrganisationStructureKey);
            statement.GenericKeyTypeKey.ShouldEqual(genericKeyTypeKey);
            statement.GenericKey.ShouldEqual(genericKey);
            statement.UserName.ShouldEqual(userName);
        };

        private static long instanceId;
        private static int capabilityKey;
        private static int userOrganisationStructureKey;
        private static UpdateLastAssignedUserByCapabilityForInstanceStatement statement;
        private static int genericKeyTypeKey;
        private static int genericKey;
        private static string userName;
    }
}
