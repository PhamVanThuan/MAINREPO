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
    public class when_given_an_empty_string : WithFakes
    {
        private static Calculator caculator;
        private static string input;
        private static int output;

        private Establish context = () =>
            {
                caculator = new Calculator();
                input = string.Format("");
            };

        private Because of = () =>
            {
                output = caculator.Add(input);
            };

        private It should_return_0 = () =>
            {
                output.ShouldEqual(0);
            };
    }
}
