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
    public class when_decrementing_by_a_number : WithFakes
    {
        static ICounterMetric counter;
        static IMetricName name;
        static int number;
        static int expectedNumber;
        Establish context = () =>
        {
            number = new Random().Next(100);
            expectedNumber = -1 * number;
            name = An<IMetricName>();
            counter = new CounterMetric(name);
            
        };

        Because of = () =>
        {
            counter.DecrementBy(number);
        };

        It should_set_name_on_counter = () =>
        {
            counter.Name.ShouldEqual(name);
        };

        It should_increment_by_expected_number = () =>
        {
            counter.Count.ShouldEqual(expectedNumber);
        };
    }
}
