using System.Collections.Specialized;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.PollingManager.Settings;

namespace SAHL.Services.PollingManager.Server.Specs.Settings
{
    public class when_setting_the_timer_interval_for_an_invalid_integer : WithFakes
    {
        private static NameValueCollection nvc;
        private static LossControlPolledHandlerSettings settings;

        private Establish context = () =>
         {
             nvc = new NameValueCollection(){ { "TimerInterval", "-1a"} };
         };

        private Because of = () =>
        {
            settings = new LossControlPolledHandlerSettings(nvc);
        };

        private It should_set_the_timer_interval_to_one_second = () =>
        {
            settings.TimerInterval.ShouldEqual(1000);
        };
    }
}