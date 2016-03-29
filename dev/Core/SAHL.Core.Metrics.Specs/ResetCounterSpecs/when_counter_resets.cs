using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.ResetCounterSpecs
{
    [Subject("ResetCounter")]
    public class when_counter_resets : WithFakes
    {
        static IResetCounter counter;
        static IMetricName name;

        Establish context = () =>
        {
            name = An<IMetricName>();
            counter = ResetCounter.New(name, TimeUnit.Nanoseconds);
        };

        Because of = () =>
        {
            counter.Increment();
            System.Threading.Thread.Sleep(100);
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
