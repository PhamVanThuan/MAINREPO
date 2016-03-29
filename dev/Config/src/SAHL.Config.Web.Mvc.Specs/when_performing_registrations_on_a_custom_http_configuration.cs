using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.MediaTypeFormatting;
using SAHL.Config.Web.Mvc.MediaTypeFormatting.Configuration;
using SAHL.Core;

namespace SAHL.Config.Web.Mvc.Specs
{
    public class when_performing_registrations_on_a_custom_http_configuration : WithFakes
    {
        Establish that = () =>
        {
            registrables = new[]
            {
                An<IRegistrable>(),
                An<IRegistrable>(),
                An<IRegistrable>(),
            };

            configuration = new CustomHttpConfiguration(registrables);
        };

        private Because of = () =>
        {
            configuration.PerformRegistrations();
        };

        private It should_have_called_register_on_all_the_registrables = () =>
        {
            foreach (var item in registrables)
            {
                item.WasToldTo(a => a.Register());
            }
        };

        private static IEnumerable<IRegistrable> registrables;
        private static CustomHttpConfiguration configuration;
    }
}
