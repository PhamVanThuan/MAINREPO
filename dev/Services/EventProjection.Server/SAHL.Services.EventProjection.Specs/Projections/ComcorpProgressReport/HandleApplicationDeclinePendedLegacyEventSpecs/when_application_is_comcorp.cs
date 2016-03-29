using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Model;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Statements;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.ComcorpProgressReport.HandleApplicationDeclinePendedLegacyEventSpecs
{
    [Subject("namespace SAHL.Services.EventProjection.Projections.ComcorpProgress.ComcorpProgressReport.HandleApplicationDeclinePendedLegacyEvent")]
    public class when_application_is_comcorp : WithFakes
    {
        private static IServiceProjector<ApplicationDeclinePendedLegacyEvent, ICommunicationsServiceClient> projector;
        private static ApplicationDeclinePendedLegacyEvent @event;
        private static FakeDbFactory dbFactory;
        private static ICommunicationsServiceClient communicationService;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            communicationService = An<ICommunicationsServiceClient>();
            @event = new ApplicationDeclinePendedLegacyEvent(
                Param<Guid>.IsAnything,
                Param<DateTime>.IsAnything,
                Param<int>.IsAnything,
                Param<string>.IsAnything,
                Param<int>.IsAnything,
                Param<int>.IsAnything,
                Param<string>.IsAnything,
                Param<string>.IsAnything
            );
            dbFactory = new FakeDbFactory();
            dbFactory.FakedDb.DbReadOnlyContext
                .WhenToldTo(x => x.SelectOne(Param<HasOfferAttributeStatement>.IsAnything))
                .Return(new OfferAttributeDataModel(0, 0, 0));
            dbFactory.FakedDb.DbReadOnlyContext
                .WhenToldTo(x => x.SelectOne(Param<GetOfferInformationForLiveReplyStatement>.IsAnything))
                .Return(new ComcorpApplicationInformationDataModel(0, "", 0, 0, 0));
            projector = new SAHL.Services.EventProjection.Projections.ComcorpProgress.ComcorpProgressLiveReply(dbFactory);
        };

        private Because of = () =>
        {
            projector.Handle(@event, metadata, communicationService);
        };

        private It should_determine_if_application_has_comcorp_attribute = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne(Param<HasOfferAttributeStatement>.IsAnything));
        };

        private It should_not_get_application_infomation_from_db = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne(Param<GetOfferInformationForLiveReplyStatement>.IsAnything));
        };

        private It should_not_send_communication_message = () =>
        {
            communicationService.WasToldTo(x => x.PerformCommand(Param<SendComcorpLiveReplyCommand>.IsAnything, metadata));
        };
    }
}