using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities._15_Day_Auto_Reminder.OnComplete
{
    [Subject("Activity => 15_Day_Auto_Reminder => OnComplete")]
    internal class when_15_day_auto_reminder_fire : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static IApplicationCapture client;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            client = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);
            instanceData.ParentInstanceID = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_15_Day_Auto_Reminder(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_send_reminder_email = () =>
        {
            client.WasToldTo(x => x.SendReminderEMail(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>()));
        };

        //If SendReminderEMail() above fails then will never get to "return true", rely on raising exceptions.
        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}