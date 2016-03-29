using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.ExternalServices.Notification;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.NotifyThatApplicationReceivedCommandHandlerSpecs
{
    public class when_told_to_notify_applicants_that_the_applicant_has_been_received : WithFakes
    {
        private static INotificationService notificationService;
        private static NotifyThatApplicationReceivedCommandHandler handler;
        private static NotifyThatApplicationReceivedCommand command;
        private static List<Applicant> applicants;
        private static ApplicantStubs applicantStubs;
        private static ISystemMessageCollection messageCollection;
        private static int applicationId;
        static ServiceRequestMetadata metadata;
    
        Establish context = () =>
            {
                notificationService = An<INotificationService>();
                applicantStubs = new ApplicantStubs();
                applicants = new List<Applicant>();
                applicationId = 1000;
                applicants.Add(applicantStubs.GetApplicant);
                handler = new NotifyThatApplicationReceivedCommandHandler(notificationService);
                command = new NotifyThatApplicationReceivedCommand(applicants, applicationId);
            };

        Because of = () =>
            {
                messageCollection = handler.HandleCommand(command,metadata);
            };

        It should_return_no_errors = () =>
            {
                messageCollection.HasErrors.ShouldBeFalse();
            };

        It should_send_the_notification_to_the_clients = () =>
        {
            notificationService.WasToldTo(x => x.NotifyRecipients(Param<List<Recipient>>.Matches(m => m[0].CellPhoneNumber == applicantStubs.GetApplicant.Information.CellPhoneNumber), Notifications.UpdateApplicationRecievedNotification(applicationId)));
        };
    }
}
