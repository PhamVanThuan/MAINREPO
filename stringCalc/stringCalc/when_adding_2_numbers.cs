using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stringCalc
{
    public class when_adding_2_numbers : WithFakes
    {
        private static string Numbers;
        private static StringCalc calc;
        private static int expectedAnswer;

        private Establish context = () =>
        {
            Numbers = "12";
            calc = new StringCalc();
        };

        private Because of = () =>
        {
            expectedAnswer = calc.AddNumbers(Numbers);
        };

        private It should_add_the_numbers = () =>
        {
            expectedAnswer.ShouldEqual(3);
        }; 
    }
}
