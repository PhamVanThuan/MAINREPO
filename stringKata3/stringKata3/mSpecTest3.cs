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
    public class when_given_a_randomly_generated_number : WithFakes
    {
        private static Calculator caculator;
        private static int randomNumber;
        private static string input;
        private static int output;

        private Establish context = () =>
            {
                caculator = new Calculator();
                Random r = new Random();
                randomNumber =  r.Next();
                input = string.Format("{0}", randomNumber);
            };

        private Because of = () =>
            {
                output = caculator.Add(input);
            };

        private It should_return_the_random_number = () =>
            {
                output.ShouldEqual(randomNumber);
            };
    }
}
