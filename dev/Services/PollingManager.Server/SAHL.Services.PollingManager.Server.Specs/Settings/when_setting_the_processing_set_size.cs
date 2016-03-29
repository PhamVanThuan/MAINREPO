using System.Collections.Specialized;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.PollingManager.Settings;

namespace SAHL.Services.PollingManager.Server.Specs.Settings
{
    public class when_setting_the_processing_set_size : WithFakes
    {
        private static NameValueCollection nvc;
        private static LossControlPolledHandlerSettings settings;

        private Establish context = () =>
         {
             nvc = new NameValueCollection() { { "ProcessingSetSize", "10" } };
         };

        private Because of = () =>
        {
            settings = new LossControlPolledHandlerSettings(nvc);
        };

        private It should_set_the_processing_set_size_to_the_settings_value = () =>
         {
             settings.ProcessingSetSize.ShouldEqual(10);
         };
    }
}