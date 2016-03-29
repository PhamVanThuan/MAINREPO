using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using SAHL.Services.LegacyEventGenerator.Services.Statements.Events;
using System;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.Services.LegacyEventDataServiceSpecs
{
    public class when_getting_the_app_progress_model_by_stc_key : WithFakes
    {
        private static LegacyEventDataService service;
        private static FakeDbFactory fakeDbFactory;
        private static int stageTransitionCompositeKey;
        private static AppProgressInApplicationCaptureModel model;
        private static AppProgressInApplicationCaptureModel expected;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            stageTransitionCompositeKey = 5656541;
            service = new LegacyEventDataService(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Arg.Any<GetAppProgressInApplicationCaptureModelStatement>()))
                .Return(expected);
        };

        private Because of = () =>
        {
            model = service.GetAppProgressModelByStageTransitionCompositeKey(stageTransitionCompositeKey);
        };

        private It should_return_the_model_from_the_query = () =>
        {
            model.ShouldEqual(expected);
        };

        private It should_use_the_correct_statement_with_the_key_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext()
                .WasToldTo(x => x.SelectOne(Arg.Is<GetAppProgressInApplicationCaptureModelStatement>(y => y.StageTransitionCompositeKey == stageTransitionCompositeKey)));
        };
    }
}