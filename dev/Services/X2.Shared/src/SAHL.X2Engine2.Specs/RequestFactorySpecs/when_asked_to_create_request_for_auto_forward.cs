using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.RequestFactorySpecs
{
    public class when_asked_to_create_request_for_auto_forward : WithFakes
    {
        private static AutoMocker<RequestFactory> automocker;
        private static NotificationOfNewAutoForwardCommand command;
        private static long instanceId = 10;
        private static IX2Request result;


        Establish context = () =>
            {
                automocker = new NSubstituteAutoMocker<RequestFactory>();
                command = new NotificationOfNewAutoForwardCommand(instanceId);
            };

        Because of = () =>
            {
                result = automocker.ClassUnderTest.CreateRequest(command);
            };

        It should_create_a_request = () =>
            {
                result.ShouldNotBeNull();
            };

        It should_create_a_workflow_activity_request = () =>
            {
                result.ShouldBeOfExactType<X2RequestForAutoForward>();
            };

        It should_create_a_valid_request = () =>
            {
                result.InstanceId.ShouldEqual(instanceId);
            };
    }
}
