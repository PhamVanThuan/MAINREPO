using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.CommandHandlers;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification.Constants;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Communications.Specs.CommandHandlers.NotifyThatApplicationReceivedCommandHandlerSpecs
{
    public class when_told_to_notify_applicants_that_the_applicant_has_been_received : WithFakes
    {
        private static INotificationService notificationService;
        private static NotifyThatApplicationReceivedCommandHandler handler;
        private static NotifyThatApplicationReceivedCommand command;
        private static List<Recipient> recipients;
        private static ISystemMessageCollection messageCollection;
        private static int applicationId;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                metadata = An<IServiceRequestMetadata>();
                notificationService = An<INotificationService>();
                recipients = new List<Recipient>()
                {
                    new Recipient("0724310696"),
                    new Recipient("0835897287")
                };
                applicationId = 1000;

                handler = new NotifyThatApplicationReceivedCommandHandler(notificationService);
                command = new NotifyThatApplicationReceivedCommand(recipients, applicationId);
            };

        private Because of = () =>
            {
                messageCollection = handler.HandleCommand(command, metadata);
            };

        private It should_return_no_errors = () =>
            {
                messageCollection.HasErrors.ShouldBeFalse();
            };

        private It should_send_the_notification_to_the_clients = () =>
        {
            notificationService.WasToldTo(x => x.NotifyRecipients(Param<List<Recipient>>.Matches(m => m[0].CellPhoneNumber == recipients.First().CellPhoneNumber), Notifications.UpdateApplicationRecievedNotification(applicationId)));
        };
    }
}