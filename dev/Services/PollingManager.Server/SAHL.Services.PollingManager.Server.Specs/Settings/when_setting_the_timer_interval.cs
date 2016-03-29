using System.Collections.Specialized;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.PollingManager.Settings;

namespace SAHL.Services.PollingManager.Server.Specs.Settings
{
    public class when_setting_the_timer_interval : WithFakes
    {
        private static NameValueCollection nvc;
        private static LossControlPolledHandlerSettings settings;

        private Establish context = () =>
         {
             nvc = new NameValueCollection(){ { "TimerInterval", "5000"} };
         };

        private Because of = () =>
        {
            settings = new LossControlPolledHandlerSettings(nvc);
        };

        private It should_set_the_timer_interval_to_the_settings_value = () =>
        {
            settings.TimerInterval.ShouldEqual(5000);
        };
    }
}