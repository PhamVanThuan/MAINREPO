using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.CounterMetricSpecs
{
    [Subject("CounterMetric.Clear")]
    public class when_clearing_counter : WithFakes
    {
        static ICounterMetric counter;
        static IMetricName name;

        Establish context = () =>
        {
            name = An<IMetricName>();
            counter = new CounterMetric(name);
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

        It should_reset_to_zero = () =>
        {
            counter.Count.ShouldEqual(0);
        };
    }
}
