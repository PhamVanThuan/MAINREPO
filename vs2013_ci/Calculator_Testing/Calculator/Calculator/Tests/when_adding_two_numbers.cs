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
    public class when_adding_two_numbers 
    {
        private static Calculator calc;
        private static double operand1;
        private static double operand2;
        private static double sum;
        //private static Exception error;

        private Establish context = () => 
        {
            calc = new Calculator();
            operand1 = 1;
            operand2 = 2;
            sum = calc.add(operand1,operand2);
        };

        private Because of = () =>
        {
            calc.add(operand1, operand2);
        };

        private It should_return_sum_of_two_numbers =()=>
        {
            Assert.AreEqual(sum,operand1+operand2);
        };

    }
}
