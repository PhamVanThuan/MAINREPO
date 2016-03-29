using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Specs.Commands
{
    public class when_issuing_the_create_legacy_event_from_composite_command : WithFakes
    {
        private static CreateLegacyEventFromCompositeCommand command;
        private static bool hasRequiredProperty;
        private static Type t;
        private static PropertyInfo p;

        private Establish context = () =>
        {
            t = typeof(CreateLegacyEventFromCompositeCommand);
            p = t.GetProperty("StageTransitionCompositeKey");
        };

        private Because of = () =>
        {
            hasRequiredProperty = Attribute.IsDefined(p, typeof(RequiredAttribute));
        };

        private It should_have_the_StageTransitionCompositeKey_As_required = () =>
        {
            hasRequiredProperty.ShouldBeTrue();
        };
    }
}