using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.CounterMetricSpecs
{
    [Subject("CounterMetric.DecrementBy")]
    public class when_decrementing_counter : WithFakes
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
            counter.Decrement();
        };

        It should_set_name_on_counter = () =>
        {
            counter.Name.ShouldEqual(name);
        };

        It should_decrement_by_one = () =>
        {
            counter.Count.ShouldEqual(-1);
        };
    }
}
