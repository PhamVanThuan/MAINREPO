using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using stringKata3;

namespace KataTemplate
{
    public class when_given_the_number_6 : WithFakes
    {
        private static Calculator caculator;
        private static string input;
        private static int output;

        private Establish context = () =>
            {
                caculator = new Calculator();
                input = "6";
            };

        private Because of = () =>
            {
                output = caculator.Add(input);
            };

        private It should_return_6 = () =>
            {
                output.ShouldEqual(6);
            };
    }
}
