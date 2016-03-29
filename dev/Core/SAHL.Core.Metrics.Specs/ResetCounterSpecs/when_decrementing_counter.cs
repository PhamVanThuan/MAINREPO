using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.ResetCounterSpecs
{
    [Subject("ResetCounter.DecrementBy")]
    public class when_decrementing_counter : WithFakes
    {
        static IResetCounter counter;
        static IMetricName name;

        Establish context = () =>
        {
            name = An<IMetricName>();
            counter = ResetCounter.New(name,TimeUnit.Seconds);
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
