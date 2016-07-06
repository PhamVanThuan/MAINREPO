using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stringCalc
{
    public class when_having_1_number : WithFakes
    {
        private static String Numbers;
        private static  StringCalc calc;
        private static int expectedAnswer;
        
        private Establish context = () =>
        {
            Numbers = "1";
            calc = new StringCalc();
        };

        private Because of = () =>
        {
            expectedAnswer = calc.AddNumbers(Numbers);
        };

        private It should_add_the_numbers = () =>
        {
            expectedAnswer.ShouldEqual(1);
        };
    }
}
