using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Models;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.CapitecApplicationDaily.HandleApplicationDeclinedLegacyEventSpecs
{
    [Subject("SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.CapitecApplicationDaily.HandleApplicationDeclinedLegacyEvent")]
    public class when_consultant_details_are_unavailable : WithFakes
    {
        private static CapitecApplicationDailyReport projection;
        private static ApplicationDeclinedLegacyEvent applicationCreditEvent;
        private static FakeDbFactory dbFactory;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            dbFactory = new FakeDbFactory();
            dbFactory.FakedDb.DbReadOnlyContext
                .WhenToldTo(x => x.Select<OfferAttributeDataModel>(Param<GetOfferAttributeStatment>.IsAnything))
                .Return(new OfferAttributeDataModel[] { new OfferAttributeDataModel(0, 30) });

            dbFactory.FakedDb.DbContext
                .WhenToldTo(x => x.SelectOne<ConsultantInfoDataModel>(Param<GetConsultantLegalEntityDetailsStatement>.IsAnything))
                .Return(new ConsultantInfoDataModel("", ""));

            applicationCreditEvent = new ApplicationDeclinedLegacyEvent(
                Param<Guid>.IsAnything,
                Param<DateTime>.IsAnything,
                Param<int>.IsAnything,
                Param<string>.IsAnything,
                Param<int>.IsAnything,
                Param<int>.IsAnything,
                Param<string>.IsAnything,
                Param<string>.IsAnything
            );

            projection = new CapitecApplicationDailyReport(dbFactory);
        };

        private Because of = () =>
        {
            projection.Handle(applicationCreditEvent, metadata);
        };

        private It should_check_if_application_is_capitec = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<OfferAttributeDataModel>(Param<GetOfferAttributeStatment>.IsAnything));
        };

        private It should_project_changes_to_capitec_db = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update(Param<ISqlStatement<ApplicationDataModel>>.IsAnything));
        };

        private It should_complete_db_changes = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Complete());
        };
    }
}