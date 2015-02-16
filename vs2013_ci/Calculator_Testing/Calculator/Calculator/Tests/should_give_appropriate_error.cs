using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;

namespace Calculator
{
    public class should_give_appropriate_error
    {
        private static Calculator calc;
        private static Exception error;
        private static double operand1;
        private static double operand2;

        private Establish context = () =>
        {
            operand1 = 0;
            operand2 = 0;
            calc = new Calculator();
        };

        private Because of = () =>
        {
            error = Catch.Exception(()=>(calc.divide(operand1, operand2)));
        };

        private It should_give_an_exception_when_dividing_by_zero = () =>
        {
            error.Message.ShouldEqual("Cannot divide by 0");
        };

    }
}
