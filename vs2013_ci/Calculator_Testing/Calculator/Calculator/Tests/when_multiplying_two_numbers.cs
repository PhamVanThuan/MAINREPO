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
    public class when_dividing_two_numbers
    {
        private static double operand1;
        private static double operand2;
        private static Calculator calc;
        private static double product;
        private static double calculatedDividend;

        private Establish context = () => 
        {
            calc = new Calculator();
            operand1 = 1;
            operand2 = 2;
            product = 0.5;
            calculatedDividend = calc.divide(operand1,operand2);
        };

        private Because of = () => calc.divide(operand1, operand2);
        
        private It should_return_correct_product =()=>
        {
            Assert.AreEqual(calculatedDividend,product);
        };


    }
}
