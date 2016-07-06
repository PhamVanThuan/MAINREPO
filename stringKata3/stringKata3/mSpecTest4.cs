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
    public class when_given_2_randomly_generated_numbers : WithFakes
    {
        private static Calculator caculator;

        private static int randomNumber1, randomNumber2, expectedValue;
        private static string input;
        private static int output;

        private Establish context = () =>
            {
                caculator = new Calculator();
                Random r = new Random();
                randomNumber1 =  r.Next();
                randomNumber2 = r.Next();
                expectedValue = randomNumber1 + randomNumber2;

                input = string.Format("{0},{1}", randomNumber1, randomNumber2);
            };

        private Because of = () =>
            {
                output = caculator.Add(input);
            };

        private It should_return_the_sum_of_the_numbers = () =>
            {
                output.ShouldEqual(expectedValue);
            };
    }
}
