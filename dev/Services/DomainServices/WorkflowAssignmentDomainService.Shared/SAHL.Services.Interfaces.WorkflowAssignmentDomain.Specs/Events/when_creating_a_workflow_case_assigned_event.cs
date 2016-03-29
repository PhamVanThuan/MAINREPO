using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Specs.Events
{
    public class when_creating_a_workflow_case_assigned_event : WithFakes
    {
        Establish that = () =>
        {
            dateTime = DateTime.Now;

            genericKeyTypeKey = 1;
            genericKey = 2;
            instanceId = 3L;
            userOrganisationStructureKey = 4;
            capabilityKey = 5;
        };

        private Because of = () =>
        {
            result = new WorkflowCaseAssignedEvent(dateTime, genericKeyTypeKey, genericKey, instanceId, userOrganisationStructureKey, capabilityKey);
        };

        private It should_have_set_the_expected_fields = () =>
        {
            new object[]
            {
                genericKeyTypeKey,
                genericKey,
                instanceId,
                userOrganisationStructureKey,
                capabilityKey,
            }.ShouldEqual(new object[]
            {
                result.GenericKeyTypeKey,
                result.GenericKey,
                result.InstanceId,
                result.UserOrganisationStructureKey,
                result.CapabilityKey,
            });
        };

        private static WorkflowCaseAssignedEvent result;
        private static DateTime dateTime;
        private static long instanceId;
        private static int userOrganisationStructureKey;
        private static int capabilityKey;
        private static int genericKeyTypeKey;
        private static int genericKey;
    }
}
