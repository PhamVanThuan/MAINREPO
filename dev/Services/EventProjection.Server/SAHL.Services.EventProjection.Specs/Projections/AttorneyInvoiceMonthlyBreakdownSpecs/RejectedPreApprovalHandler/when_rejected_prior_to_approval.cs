using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Projections;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.ThirdPartyInvoiceRejectionNotification;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.RejectedPreApprovalHandler
{
    public class when_rejected_prior_to_approval : WithFakes
    {
        private static ICombGuid combGuidGenerator;
        private static ThirdPartyInvoiceRejectedPreApprovalEvent thirdPartyInvoiceRejectedEvent;
        private static IServiceProjector<ThirdPartyInvoiceRejectedPreApprovalEvent, ICommunicationsServiceClient> projector;
        private static ICommunicationsServiceClient communicationService;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            combGuidGenerator = An<ICombGuid>();
            communicationService = An<ICommunicationsServiceClient>();
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceRejectedEvent = new ThirdPartyInvoiceRejectedPreApprovalEvent(
                  DateTime.Now
                , 378264
                , "attorney@practice.co.za"
                , "INV814327"
                , "SAHL\\LCCUser1"
                , "Attorney blah blah"
                , "SAHL-2015/04/3"
                , 9843
                , "378264 - INV814327"
                , Guid.NewGuid()
                );

            projector = new ThirdPartyInvoiceRejectionMailHandler(combGuidGenerator);
        };

        private Because of = () =>
        {
            projector.Handle(thirdPartyInvoiceRejectedEvent, metadata, communicationService);
        };

        private It should_send_an_invoice_rejection_email_notification = () =>
        {
            communicationService.WasToldTo(y => y.PerformCommand(Param<SendInternalEmailCommand>
                .Matches(m => m.EmailTemplate.EmailModel.To.Equals(thirdPartyInvoiceRejectedEvent.AttorneyEmailAddress, StringComparison.Ordinal)
                           && m.EmailTemplate.TemplateName.Equals("RejectedInvoiceEmailTemplate", StringComparison.Ordinal)
                           && m.EmailTemplate.EmailModel.Subject.Equals(thirdPartyInvoiceRejectedEvent.EmailSubject)
                )
                , Param.IsAny<IServiceRequestMetadata>()
           ));
        };
    }
}