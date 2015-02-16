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
    public class when_subtracting_two_numbers
    {
        private static Calculator calc;
        private static double operand1;
        private static double operand2;
        private static double difference;

        private Establish context = () =>
        {
            calc = new Calculator();
            operand1 = 0;
            operand2 = 0;
            difference = calc.subtract(operand1,operand2);
        };

        private Because of =()=> calc.subtract(operand1,operand2);

        private It should_return_operand1_subtract_operand2 =()=>
        {
           //Assert.AreEqual(difference,operand1-operand2);
        }



    }
}
