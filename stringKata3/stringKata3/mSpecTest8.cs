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
    public class when_given_random_numbers_seperated_by_new_lines_instead_of_commas : WithFakes
    {
        private static Calculator caculator;
        private static int randomNumber1, randomNumber2, randomNumber3, randomNumber4, expectedValue;
        private static string input;
        private static int output;

        private Establish context = () =>
            {
                caculator = new Calculator();
                Random r = new Random();
                randomNumber1 =  r.Next();
                randomNumber2 = r.Next();
                randomNumber3 = r.Next();
                randomNumber4 = r.Next();

                expectedValue = randomNumber1 + randomNumber2 + randomNumber3 + randomNumber4;

                input = string.Format("{0}\n{1}\n{2}\n{3}", randomNumber1, randomNumber2, randomNumber3, randomNumber4);
            };

        private Because of = () =>
            {
                output = caculator.Add(input);
            };

        private It should_return_the_random_number = () =>
            {
                output.ShouldEqual(expectedValue);
            };
    }
}
