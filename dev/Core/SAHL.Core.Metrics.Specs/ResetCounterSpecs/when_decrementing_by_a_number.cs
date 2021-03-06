﻿using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Metrics.Specs.ResetCounterSpecs
{
    [Subject("ResetCounter.DecrementBy")]
    public class when_decrementing_by_a_number : WithFakes
    {
        static IResetCounter counter;
        static IMetricName name;
        static int number;
        static int expectedNumber;
        Establish context = () =>
        {
            number = new Random().Next(100);
            expectedNumber = -1 * number;
            name = An<IMetricName>();
            counter = ResetCounter.New(name, TimeUnit.Seconds);
            
        };

        Because of = () =>
        {
            counter.DecrementBy(number);
        };

        It should_set_name_on_counter = () =>
        {
            counter.Name.ShouldEqual(name);
        };

        It should_decrement_by_expected_number = () =>
        {
            counter.Count.ShouldEqual(expectedNumber);
        };
    }
}
