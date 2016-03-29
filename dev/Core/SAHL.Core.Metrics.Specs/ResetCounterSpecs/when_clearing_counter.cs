using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metrics = SAHL.Core.Metrics;

namespace SAHL.Core.Metrics.Specs.ResetCounterSpecs
{
    [Subject("ResetCounter.Clear")]
    public class when_clearing_counter : WithFakes
    {
        static IResetCounter counter;
        static IMetricName name;

        Establish context = () =>
        {
            name = An<IMetricName>();
            counter = ResetCounter.New(name, TimeUnit.Seconds);
            counter.Increment();
        };

        Because of = () =>
        {
            counter.Clear();
        };

        It should_set_name_on_counter = () =>
        {
            counter.Name.ShouldEqual(name);
        };

        It should_be_reset_to_zero = () =>
        {
            counter.Count.ShouldEqual(0);
        };
    }
}
