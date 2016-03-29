using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckCreateActivityHasOnlyStaticRoleCommandHandlerSpecs
{
    public class when_checking_creation_access_fail : WithFakes
    {
        private static AutoMocker<CheckCreateActivityHasOnlyStaticRoleCommandHandler> automocker = new NSubstituteAutoMocker<CheckCreateActivityHasOnlyStaticRoleCommandHandler>();
        private static CheckCreateActivityHasOnlyStaticRoleCommand command;
        private static Activity activity;
        private static ActivitySecurityDataModel activitySecurity;
        private static SecurityGroupDataModel securityGroup;
        private static ISystemMessageCollection messages;
        private static Core.Services.ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            securityGroup = new SecurityGroupDataModel(false, "WorkList", "", 1, 1);
            activitySecurity = new ActivitySecurityDataModel(1, 1);
            activity = new Activity(1, "Create", null, null, 1, "Created", 1, false);
            command = new CheckCreateActivityHasOnlyStaticRoleCommand(activity, "userName");
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivitySecurityForActivity(activity.ActivityID)).Return(new List<ActivitySecurityDataModel> { activitySecurity });
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(activitySecurity.SecurityGroupID)).Return(securityGroup);
        };

        private Because of = () =>
        {
            messages = automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_security_for_the_activity = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivitySecurityForActivity(activity.ActivityID));
        };

        private It should_get_the_security_group_for_the_activity = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetSecurityGroup(activitySecurity.SecurityGroupID));
        };

        private It should_return_false = () =>
        {
            command.Result.ShouldEqual(false);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };
    }
}