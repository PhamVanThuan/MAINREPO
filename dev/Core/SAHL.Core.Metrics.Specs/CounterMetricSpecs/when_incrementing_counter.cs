using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.CounterMetricSpecs
{
    [Subject("CounterMetric.Increment")]
    public class when_incrementing_counter : WithFakes
    {
        static ICounterMetric counter;
        static IMetricName name;

        Establish context = () =>
        {
            name = An<IMetricName>();
            counter = new CounterMetric(name);
        };

        Because of = () =>
        {
            counter.Increment();
        };

        It should_set_name_on_counter = () =>
        {
            counter.Name.ShouldEqual(name);
        };

        It should_increment_by_one = () =>
        {
            counter.Count.ShouldEqual(1);
        };
    }
}
